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
            ((EntryPointDelegate)Assembly.Load(CreateAssembly()).EntryPoint.CreateDelegate(typeof(EntryPointDelegate)))(new string[]{});

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

            manager.CreateAssembly(newAssemblyName, debug);
            return manager.SaveAndGetBytes(PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit, ImageFileMachine.I386, !debug);
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
        {
            var n = new AssemblyName(e.Name);

            if(n.Name == "AssemblyTranslator")
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var name = assembly.GetName().Name + ".AssemblyTranslator.dll";
                using (Stream stream = assembly.GetManifestResourceStream(name))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    assembly = Assembly.Load(buffer);
                    return assembly;
                }
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
