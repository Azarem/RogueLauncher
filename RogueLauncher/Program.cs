using AssemblyTranslator;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

//[assembly: Obfuscation(Exclude = false, Feature = "external module:RogueAPI.dll")]
namespace RogueLauncher
{
    public class Program : MarshalByRefObject
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        private static readonly string assemblyFileName = "RogueLegacy.exe";
        private static readonly string newAssemblyName = "RogueLegacyTest";
        private delegate void EntryPointDelegate(string[] args);

        static void Main(string[] args)
        {
            ((EntryPointDelegate)Assembly.Load(CreateAssembly()).EntryPoint.CreateDelegate(typeof(EntryPointDelegate)))(new string[] { });

            //            byte[] asmBytes = null;
            //            //AppDomain domain = AppDomain.CurrentDomain;
            //            Assembly newAssembly;


            //#if DEBUG
            //            //Environment.CurrentDirectory = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Rogue Legacy";
            //            //domain.SetData("APPBASE", Environment.CurrentDirectory);



            //            //asm.EntryPoint.Invoke()

            //            return;
            //#else
            //            //The following does not work with maximum obfuscation
            //            //Create new app domain to contain the assembly and perform translation
            //            //domain = AppDomain.CreateDomain("ReflectionDomain", AppDomain.CurrentDomain.Evidence, new AppDomainSetup() { ApplicationBase = domain.BaseDirectory, ApplicationName = domain.SetupInformation.ApplicationName });

            //            //domain.CreateInstanceAndUnwrap()
            //            //Create remote instance of Program class
            //            //var program = domain.CreateInstanceAndUnwrap(typeof(Program).Assembly.FullName, typeof(Program).FullName) as Program;
            //            //var program = domain.CreateInstanceFromAndUnwrap(typeof(Program).Assembly.Location, typeof(Program).FullName) as Program;
            //            //var program = new Program();

            //            //Create assembly and retrieve the bytes
            //            asmBytes = CreateAssembly();

            //            //Destroy remote AppDomain
            //            //AppDomain.Unload(domain);

            //            //Clean up resources
            //            //GC.Collect();
            //            //GC.WaitForPendingFinalizers();
            //            //GC.Collect();

            //            //Attach assembly resolve handler to load dependent libraries (not automatic)

            //            //Load raw assembly into running application
            //            newAssembly = Assembly.Load(asmBytes);

            //            //Execute new assembly entry point
            //            newAssembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
            //#endif
        }

        //Used as a remote endpoint for creating the assembly in a workspace app domain
        public static byte[] CreateAssembly(bool debug = false)
        {
            GraphManager manager = new GraphManager(assemblyFileName);

            Rewrite.RogueCastle.Program.Rewrite(manager.Graph.Modules.SelectMany(x => x.TypeGraphs.Where(y => y.FullName == "RogueCastle.Program")).Single());
            Rewrite.SpellSystem.Rewrite(manager);
            Rewrite.Game.Rewrite(manager);

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
