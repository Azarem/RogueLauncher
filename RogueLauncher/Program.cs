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
        }

        private static readonly string assemblyFileName = "RogueLegacy.exe";
        private static readonly string newAssemblyName = "RogueLegacyTest";
        private delegate void EntryPointDelegate(string[] args);

        static void Main(string[] args)
        {
            bool started = EnsureSteamStarted();

            var asmBytes = CreateAssembly();

            if (!started && !WaitForSteamStart())
                MessageBox.Show("Unable to start the Steam client!");
            else
                ((EntryPointDelegate)Assembly.Load(asmBytes).EntryPoint.CreateDelegate(typeof(EntryPointDelegate)))(new string[] { });
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
        public static byte[] CreateAssembly()
        {
#if DEBUG
            bool debug = true;
#else
            bool debug = false;
#endif
            GraphManager manager = new GraphManager(assemblyFileName);

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

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
        {
            var n = new AssemblyName(e.Name);

            if (n.Name == "AssemblyTranslator" || n.Name == "RogueAPI")
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var name = String.Format("{0}.{1}.dll", assembly.GetName().Name, n.Name);
                byte[] asmBytes;
                using (Stream stream = assembly.GetManifestResourceStream(name))
                {
                    asmBytes = new byte[stream.Length];
                    stream.Read(asmBytes, 0, asmBytes.Length);
                }

                if (n.Name == "RogueAPI")
                {
                    name = n.Name + ".dll";
                    using (var outStream = File.Open(name, FileMode.Create, FileAccess.Write))
                        outStream.Write(asmBytes, 0, asmBytes.Length);

                    return Assembly.LoadFrom(name);
                }

                byte[] pdbBytes = null;
#if DEBUG
                name = n.Name + ".pdb";
                if (File.Exists(name))
                    using (Stream stream = File.OpenRead(name))
                    {
                        pdbBytes = new byte[stream.Length];
                        stream.Read(pdbBytes, 0, pdbBytes.Length);
                    }
#endif
                return Assembly.Load(asmBytes, pdbBytes);
            }

            var path = Path.Combine(Environment.CurrentDirectory, n.Name);
            if (!File.Exists(path))
                path += ".dll";
            if (File.Exists(path))
                return Assembly.LoadFrom(path);
            return null;
        }

    }
}
