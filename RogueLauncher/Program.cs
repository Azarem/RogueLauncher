using AssemblyTranslator;
using AssemblyTranslator.IL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace RogueLauncher
{
    public class Program : MarshalByRefObject
    {
        //internal static int SteamAppId;

        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveReflectionAssembly;
        }

        private static readonly string assemblyFileName = "RogueLegacy";
        private static readonly string newAssemblyName = "RogueLegacyTest";
        private delegate void EntryPointDelegate(string[] args);

        static void Main(string[] args)
        {
            bool started = EnsureSteamStarted();

            AppDomain generateDomain = null;
            byte[] asmBytes;
            try
            {
                generateDomain = AppDomain.CreateDomain("GenerateDomain", null, new AppDomainSetup()
                {
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    ApplicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName,
                    LoaderOptimization = LoaderOptimization.MultiDomainHost
                });

                generateDomain.DoCallBack(new CrossAppDomainDelegate(CreateAssemblyRemote));
                asmBytes = (byte[])generateDomain.GetData("ASMDATA");
            }
            finally
            {
                if (generateDomain != null)
                    AppDomain.Unload(generateDomain);
            }

            //var asmBytes = CreateAssembly();

            if (!started && !WaitForSteamStart())
                MessageBox.Show("Unable to start the Steam client!");
            else
            {
                byte[] pdbBytes = null;
#if DEBUG
                var pdbFile = newAssemblyName + ".pdb";
                if (!File.Exists(pdbFile))
                    pdbFile = FindPdbFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "JustDecompile", "Symbols"), pdbFile);

                if (pdbFile != null && File.Exists(pdbFile))
                    using (var file = File.OpenRead(pdbFile))
                    {
                        pdbBytes = new byte[file.Length];
                        file.Read(pdbBytes, 0, pdbBytes.Length);
                    }
#endif
                var entryPoint = (EntryPointDelegate)Assembly.Load(asmBytes, pdbBytes).EntryPoint.CreateDelegate(typeof(EntryPointDelegate));
                entryPoint(new string[] { });
            }
        }

        public static string FindPdbFile(string path, string file)
        {
            var found = Directory.GetFiles(path, file, SearchOption.TopDirectoryOnly);
            if (found.Length > 0)
                return found[0];

            foreach (var sub in Directory.GetDirectories(path))
            {
                try
                {
                    var subFound = FindPdbFile(sub, file);
                    if (subFound != null)
                        return subFound;
                }
                catch { }
            }

            return null;
        }

        public static bool EnsureSteamStarted()
        {
            if (Process.GetProcessesByName("Steam").Length == 0)
            {
                Process.Start(new ProcessStartInfo("..\\..\\..\\Steam.exe"));
                return false;
            }
            return true;
        }

        public static bool WaitForSteamStart(int timeout = 20000)
        {
            Process[] proc;
            var timestamp = Environment.TickCount + timeout;
            while (((proc = Process.GetProcessesByName("Steam")).Length == 0 || proc[0].MainWindowHandle == IntPtr.Zero) && Environment.TickCount < timestamp)
                Thread.Sleep(1000);

            return Environment.TickCount < timestamp;
        }

        //Used as a remote endpoint for creating the assembly in a workspace app domain
        public static void CreateAssemblyRemote()
        {
            var asmBytes = CreateAssembly();
            AppDomain.CurrentDomain.SetData("ASMDATA", asmBytes);
        }

        public static byte[] CreateAssembly()
        {
#if DEBUG
            bool debug = true;
#else
            bool debug = false;
#endif

            var asmFile = assemblyFileName + (debug && File.Exists(assemblyFileName + "_Debug.exe") ? "_Debug" : "") + ".exe";

            GraphManager manager = new GraphManager(assemblyFileName + ".exe");

            //Pull out Steam app ID
            var appId = (from x in manager.Graph.Modules
                         from y in x.TypeGraphs
                         where y.FullName == "RogueCastle.Program"
                         from z in y.Methods
                         where z.Name == "Main"
                         from i in z.InstructionList
                         where i is MethodInstruction && ((MethodInstruction)i).Operand.Name == "init"
                         select z.InstructionList[z.InstructionList.IndexOf(i) - 1].RawOperand.ToString()).FirstOrDefault();

            if (appId != null)
                Environment.SetEnvironmentVariable("SteamAppId", appId);

            //manager.ReplaceType("RogueCastle.ProjectileData", typeof(RogueAPI.Projectiles.ProjectileInstance));
            manager.ReplaceType("RogueCastle.ProjectileObj", typeof(RogueAPI.Projectiles.ProjectileObj));
            manager.ReplaceType("RogueCastle.EquipmentData", typeof(RogueAPI.Equipment.EquipmentBase));

            manager.CreateAssembly(newAssemblyName, debug);
            return manager.SaveAndGetBytes(PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit, ImageFileMachine.I386, !debug);
        }

        private static Assembly ResolveReflectionAssembly(object sender, ResolveEventArgs e)
        {
            var asm = ResolveAssemblyInternal(sender, e, true);

            if (asm == null)
                asm = Assembly.ReflectionOnlyLoad(e.Name);

            return asm;
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
        {
            return ResolveAssemblyInternal(sender, e, false);
        }

        private static Assembly ResolveAssemblyInternal(object sender, ResolveEventArgs e, bool reflectionOnly)
        {
            var n = new AssemblyName(e.Name);

            if (n.Name == "AssemblyTranslator" || n.Name == "RogueAPI")
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var asmName = assembly.GetName().Name;
                var name = String.Format("{0}.{1}.dll", asmName, n.Name);
                byte[] asmBytes;
                using (Stream stream = assembly.GetManifestResourceStream(name))
                {
                    asmBytes = new byte[stream.Length];
                    stream.Read(asmBytes, 0, asmBytes.Length);
                }

                //if (n.Name == "RogueAPI")
                //{
                //    name = n.Name + ".dll";
                //    using (var outStream = File.Open(name, FileMode.Create, FileAccess.Write))
                //        outStream.Write(asmBytes, 0, asmBytes.Length);

                //    return reflectionOnly ? Assembly.ReflectionOnlyLoadFrom(name) : Assembly.LoadFrom(name);
                //}

                byte[] pdbBytes = null;
#if DEBUG
                if (!reflectionOnly)
                {
                    name = String.Format("{0}.{1}.pdb", asmName, n.Name);
                    using (Stream stream = assembly.GetManifestResourceStream(name))
                    {
                        pdbBytes = new byte[stream.Length];
                        stream.Read(pdbBytes, 0, pdbBytes.Length);
                    }
                    //if (File.Exists(name))
                    //    using (Stream stream = File.OpenRead(name))
                    //    {
                    //        pdbBytes = new byte[stream.Length];
                    //        stream.Read(pdbBytes, 0, pdbBytes.Length);
                    //    }
                }
#endif
                return reflectionOnly ? Assembly.ReflectionOnlyLoad(asmBytes) : Assembly.Load(asmBytes, pdbBytes);
            }

            var path = Path.Combine(Environment.CurrentDirectory, n.Name);
            if (!File.Exists(path))
                path += ".dll";
            if (File.Exists(path))
                return reflectionOnly ? Assembly.ReflectionOnlyLoadFrom(path) : Assembly.LoadFrom(path);
            return null;
        }

    }
}
