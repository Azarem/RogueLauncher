using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace RogueLauncher
{
    public static class PInvoke
    {
        [DllImport("Kernel32", SetLastError = true)]
        public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

        [DllImport("Kernel32")]
        public static extern IntPtr LoadLibrary(string lpFileName);
    }
}
