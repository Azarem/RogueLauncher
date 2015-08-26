using AssemblyTranslator;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RogueLauncher
{
    public class Program : MarshalByRefObject
    {
        internal static int SteamAppId;

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

            Rewrite.Program.Process(manager);
            Rewrite.SpellSystem.Process(manager);
            Rewrite.Game.Process(manager);

            manager.ReplaceType("RogueCastle.ProjectileData", typeof(RogueAPI.Projectiles.ProjectileInstance));

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
