using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Clever_Registry
{
    class Program
    {
        static int Main(string[] args)
        {
            const string FILE_EXTENSION_1 = ".bp";
            const string FILE_EXTENSION_2 = ".bpi";
            const string FILE_EXTENSION_3 = ".bpm";
            const string EXTENSION_1_OPEN_KEY = "bp_file";
            const string EXTENSION_2_OPEN_KEY = "bpi_file";
            const string EXTENSION_3_OPEN_KEY = "bpm_file";
            const int SHCNE_ASSOCCHANGED = 0x8000000;
            const uint SHCNF_IDLIST = 0x0U;
            string appName = "Clever";
            string appPath = "";
            string appGUID = "";
            string appMode = "";

            if (args.Length != 3)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (i < args.Length - 2)
                    {
                        appPath += args[i];
                        if (i < args.Length - 3)
                        {
                            appPath += " ";
                        }
                    }
                    else if (i == args.Length - 2)
                    {
                        appGUID = args[i];
                    }
                    else if (i == args.Length - 1)
                    {
                        appMode = args[i];
                    }
                }
            }
            else
            {
                appPath = args[0];
                appGUID = args[1];
                appMode = args[2];
            }

            string bpIcon = appPath.Replace("Clever.exe", "Icons\\bp.ico");
            string bpiIcon = appPath.Replace("Clever.exe", "Icons\\bpi.ico");
            string bpmIcon = appPath.Replace("Clever.exe", "Icons\\bpm.ico");

            FileInfo fiP = new FileInfo(appPath);
            FileInfo fiBp = new FileInfo(bpIcon);
            FileInfo fiBpi = new FileInfo(bpiIcon);
            FileInfo fiBpm = new FileInfo(bpmIcon);

            if (appPath == "" || appGUID == "" && (appMode != "r" || appMode != "u"))
            {
                Console.WriteLine("Invalide arguments");
                Console.WriteLine("");

                Console.WriteLine(appPath);
                Console.WriteLine(appGUID);
                Console.WriteLine(appMode);
                Console.WriteLine("");

                Console.WriteLine("For exit press any key.");
                Console.ReadKey();
                return 1;
            }
            else if (!fiP.Exists)
            {
                Console.WriteLine("File not found");
                Console.WriteLine("");

                Console.WriteLine(appPath);

                Console.WriteLine("");

                Console.WriteLine("For exit press any key.");
                Console.ReadKey();
                return 1;
            }
            else if (!fiBp.Exists)
            {
                Console.WriteLine("File not found");
                Console.WriteLine("");

                Console.WriteLine(fiBp);

                Console.WriteLine("");

                Console.WriteLine("For exit press any key.");
                Console.ReadKey();
                return 1;
            }
            else if (!fiBpi.Exists)
            {
                Console.WriteLine("File not found");
                Console.WriteLine("");

                Console.WriteLine(fiBpi);

                Console.WriteLine("");

                Console.WriteLine("For exit press any key.");
                Console.ReadKey();
                return 1;
            }
            else if (!fiBpm.Exists)
            {
                Console.WriteLine("File not found");
                Console.WriteLine("");

                Console.WriteLine(fiBpm);

                Console.WriteLine("");

                Console.WriteLine("For exit press any key.");
                Console.ReadKey();
                return 1;
            }

            bool reg = true;
            if (appMode == "r") { reg = true; }
            else if (appMode == "u") { reg = false; }

            try
            {
                if (reg)
                {

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("", true))
                    {
                        key.CreateSubKey(FILE_EXTENSION_1).SetValue("", EXTENSION_1_OPEN_KEY);
                        if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_1, false) != null)
                        {
                            Console.WriteLine(".bp key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine(".bp key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_1_OPEN_KEY).SetValue("", appName + " \"BP\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_1_OPEN_KEY, false) != null)
                        {
                            Console.WriteLine("bp_file key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("bp_file key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_1_OPEN_KEY + @"\DefaultIcon").SetValue("", ToShortPathName(bpIcon));
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_1_OPEN_KEY + @"\DefaultIcon", false) != null)
                        {
                            Console.WriteLine("Icon .bp path create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Icon .bp path not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_1_OPEN_KEY + @"\Shell\Open\Command").SetValue("", ToShortPathName(appPath) + " \"%1\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_1_OPEN_KEY + @"\Shell\Open\Command", false) != null)
                        {
                            Console.WriteLine("Command for open .bp file create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Command for open .bp file not create in Registry.");
                        }

                        key.CreateSubKey(FILE_EXTENSION_2).SetValue("", EXTENSION_2_OPEN_KEY);
                        if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_2, false) != null)
                        {
                            Console.WriteLine(".bpi key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine(".bpi key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_2_OPEN_KEY).SetValue("", appName + " \"BPI\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_2_OPEN_KEY, false) != null)
                        {
                            Console.WriteLine("bpi_file key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("bpi_file key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_2_OPEN_KEY + @"\DefaultIcon").SetValue("", ToShortPathName(bpiIcon));
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_2_OPEN_KEY + @"\DefaultIcon", false) != null)
                        {
                            Console.WriteLine("Icon .bpi path create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Icon .bpi path not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_2_OPEN_KEY + @"\Shell\Open\Command").SetValue("", ToShortPathName(appPath) + " \"%1\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_2_OPEN_KEY + @"\Shell\Open\Command", false) != null)
                        {
                            Console.WriteLine("Command for open .bpi file create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Command for open .bpi file not create in Registry.");
                        }

                        key.CreateSubKey(FILE_EXTENSION_3).SetValue("", EXTENSION_3_OPEN_KEY);
                        if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_3, false) != null)
                        {
                            Console.WriteLine(".bpm key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine(".bpm key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_3_OPEN_KEY).SetValue("", appName + " \"BPM\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_3_OPEN_KEY, false) != null)
                        {
                            Console.WriteLine("bpm_file key create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("bpm_file key not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_3_OPEN_KEY + @"\DefaultIcon").SetValue("", ToShortPathName(bpmIcon));
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_3_OPEN_KEY + @"\DefaultIcon", false) != null)
                        {
                            Console.WriteLine("Icon .bpm path create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Icon .bpm path not create in Registry.");
                        }
                        key.CreateSubKey(EXTENSION_3_OPEN_KEY + @"\Shell\Open\Command").SetValue("", ToShortPathName(appPath) + " \"%1\"");
                        if (Registry.ClassesRoot.OpenSubKey(EXTENSION_3_OPEN_KEY + @"\Shell\Open\Command", false) != null)
                        {
                            Console.WriteLine("Command for open .bpm file create in Registry.");
                        }
                        else
                        {
                            Console.WriteLine("Command for open .bpm file not create in Registry.");
                        }

                        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
                        Console.WriteLine("");
                        Console.WriteLine("For exit press any key.");
                        Console.ReadKey();
                        return 0;
                    }
                }
                else
                {
                    if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_1, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(FILE_EXTENSION_1);
                        Console.WriteLine(".bp key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine(".bp key not found in Registry.");
                    }

                    if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_2, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(FILE_EXTENSION_2);
                        Console.WriteLine(".bpi key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine(".bpi key not found in Registry.");
                    }

                    if (Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION_3, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(FILE_EXTENSION_3);
                        Console.WriteLine(".bpm key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine(".bpm key not found in Registry.");
                    }

                    if (Registry.ClassesRoot.OpenSubKey(EXTENSION_1_OPEN_KEY, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(EXTENSION_1_OPEN_KEY);
                        Console.WriteLine("bp_file key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine("bp_file key not found in Registry.");
                    }

                    if (Registry.ClassesRoot.OpenSubKey(EXTENSION_2_OPEN_KEY, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(EXTENSION_2_OPEN_KEY);
                        Console.WriteLine("bpi_file key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine("bpi_file not found in Registry.");
                    }

                    if (Registry.ClassesRoot.OpenSubKey(EXTENSION_3_OPEN_KEY, false) != null)
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(EXTENSION_3_OPEN_KEY);
                        Console.WriteLine("bpm_file key delete in Registry.");
                    }
                    else
                    {
                        Console.WriteLine("bpm_file key not found in Registry.");
                    }

                    Console.WriteLine("");
                    Console.WriteLine("For exit press any key.");
                    Console.ReadKey();

                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return 1;
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [DllImport("Kernel32.dll")]
        private static extern uint GetShortPathName(string lpszLongPath, [Out]StringBuilder lpszShortPath, uint cchBuffer);

        private static string ToShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = (uint)s.Capacity;
            uint iRet = GetShortPathName(longName, s, iSize);
            return s.ToString();
        }
    }
}
