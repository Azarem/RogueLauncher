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
    public class Program
    {
        private static bool _buildOnly;
        private static readonly string _assemblyFileName;
        private static readonly string _newAssemblyName;
        private delegate void EntryPointDelegate(string[] args);
        //internal static int SteamAppId;

        static Program()
        {
            _assemblyFileName = "RogueLegacy";
            _newAssemblyName = "RogueLegacyTest";
            _buildOnly = false;

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveReflectionAssembly;
            //LoadEmbeddedAssembly("AssemblyTranslator", false);
            //LoadEmbeddedAssembly("RogueAPI", false);
        }


        static void Main(string[] args)
        {
            _buildOnly = args != null && args.Contains("-buildonly", StringComparer.OrdinalIgnoreCase);
            bool started = false;

#if !DEBUG
            if (_buildOnly)
                return;
#endif

            if (!_buildOnly)
                started = EnsureSteamStarted();

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

#if DEBUG
                generateDomain.SetData("BUILDONLY", _buildOnly);
#endif
                generateDomain.DoCallBack(CreateAssemblyRemote);
                asmBytes = (byte[])generateDomain.GetData("ASMDATA");
            }
            finally
            {
                if (generateDomain != null)
                    AppDomain.Unload(generateDomain);
            }

            if (_buildOnly)
                return;

            if (!started && !WaitForSteamStart())
                MessageBox.Show("Unable to start the Steam client!");
            else
            {
                byte[] pdbBytes = null;
#if DEBUG
                var pdbFile = _newAssemblyName + ".pdb";
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
            try
            {
#if DEBUG
                _buildOnly = (bool)AppDomain.CurrentDomain.GetData("BUILDONLY");
#endif
                var asmBytes = CreateAssembly();
                AppDomain.CurrentDomain.SetData("ASMDATA", asmBytes);
            }
            catch (Exception x)
            {
                using (var str = new StreamWriter("ErrorLog.txt"))
                    str.Write(x.ToString());
                throw x;
            }
        }

        public static byte[] CreateAssembly()
        {
            var timestamp = DateTime.Now;

#if DEBUG
            bool debug = true;
#else
            bool debug = false;
#endif

            var asmFile = _assemblyFileName + (debug && File.Exists(_assemblyFileName + "_Debug.exe") ? "_Debug" : "") + ".exe";

            GraphManager manager = new GraphManager(_assemblyFileName + ".exe");

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

            manager.CreateAssembly(_newAssemblyName, debug);
            var asmBytes = manager.SaveAndGetBytes(PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit, ImageFileMachine.I386, !debug);

            Console.WriteLine("** ASSEMBLY CREATION COMPLETED IN " + DateTime.Now.Subtract(timestamp).TotalSeconds + "s **");

            return asmBytes;
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

        private static Assembly LoadEmbeddedAssembly(string asmName, bool reflectionOnly)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var thisName = assembly.GetName().Name;
            var name = String.Format("{0}.{1}.dll", thisName, asmName);
            byte[] asmBytes;
            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                asmBytes = new byte[stream.Length];
                stream.Read(asmBytes, 0, asmBytes.Length);
            }

            byte[] pdbBytes = null;
#if DEBUG
            if (!reflectionOnly)
            {
                name = String.Format("{0}.{1}.pdb", thisName, asmName);
                using (Stream stream = assembly.GetManifestResourceStream(name))
                {
                    pdbBytes = new byte[stream.Length];
                    stream.Read(pdbBytes, 0, pdbBytes.Length);
                }
            }

            if (_buildOnly && asmName == "RogueAPI")
            {
                if (pdbBytes != null)
                {
                    name = asmName + ".pdb";
                    using (var outStream = File.Open(name, FileMode.Create, FileAccess.Write))
                        outStream.Write(pdbBytes, 0, pdbBytes.Length);
                }

                name = asmName + ".dll";
                using (var outStream = File.Open(name, FileMode.Create, FileAccess.Write))
                    outStream.Write(asmBytes, 0, asmBytes.Length);

                return reflectionOnly ? Assembly.ReflectionOnlyLoadFrom(name) : Assembly.LoadFrom(name);
            }
#endif

            return reflectionOnly ? Assembly.ReflectionOnlyLoad(asmBytes) : Assembly.Load(asmBytes, pdbBytes);
        }

        private static Assembly ResolveAssemblyInternal(object sender, ResolveEventArgs e, bool reflectionOnly)
        {
            var n = new AssemblyName(e.Name);

            if (n.Name == "AssemblyTranslator" || n.Name == "RogueAPI")
                return LoadEmbeddedAssembly(n.Name, reflectionOnly);

            var path = Path.Combine(Environment.CurrentDirectory, n.Name);
            if (!File.Exists(path))
                path += ".dll";
            if (File.Exists(path))
                return reflectionOnly ? Assembly.ReflectionOnlyLoadFrom(path) : Assembly.LoadFrom(path);
            return null;
        }

    }
}
