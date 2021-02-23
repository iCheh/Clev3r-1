using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Clever.Model.Utils
{
    class KeyRegistry
    {
        string regMode;

        public KeyRegistry(string mode)
        {
            if (mode == "r")
            {
                IsRegistry = true;
            }
            else
            {
                IsRegistry = false;
            }

            regMode = mode;
            SetKey();
        }

        [System.Security.SecurityCritical()]
        private void SetKey()
        {
            var appName = Application.ResourceAssembly.GetName().Name;
            var appPath = Application.ResourceAssembly.Location;
            var appGUID = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            var psi = new ProcessStartInfo();
            psi.Arguments = appPath + " " + appGUID + " " + regMode;
            psi.FileName = appPath.Replace("Clever.exe", "Clever Registry.exe");
            psi.Verb = "runas";

            var process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
            var result = process.ExitCode;
            if (regMode == "r" && result == 0)
                RegistryValidation(appName);
            if (result == 1)
                IsRegistry = false;
        }

        public bool IsRegistry { get; set; }

        private void RegistryValidation(string appName)
        {
            const string FILE_EXTENSION_1 = ".bp";
            const string FILE_EXTENSION_2 = ".bpi";
            const string FILE_EXTENSION_3 = ".bpm";

            const string EXTENSION_1_OPEN_KEY = "bp_file";
            const string EXTENSION_2_OPEN_KEY = "bpi_file";
            const string EXTENSION_3_OPEN_KEY = "bpm_file";


            if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_1, false) == null)
            {
                IsRegistry = false;
            }

            if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_2, false) == null)
            {
                IsRegistry = false;
            }

            if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_3, false) == null)
            {
                IsRegistry = false;
            }

            if (Registry.ClassesRoot.OpenSubKey(EXTENSION_1_OPEN_KEY, false) == null)
            {
                IsRegistry = false;
            }

            if (Registry.ClassesRoot.OpenSubKey(EXTENSION_2_OPEN_KEY, false) == null)
            {
                IsRegistry = false;
            }

            if (Registry.ClassesRoot.OpenSubKey(EXTENSION_3_OPEN_KEY, false) == null)
            {
                IsRegistry = false;
            }
        }
    }
}
