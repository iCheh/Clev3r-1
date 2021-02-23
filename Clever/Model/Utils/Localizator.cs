using Clever.CommonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Clever.Model.Utils
{
    class Localizator
    {
        public Dictionary<string, string> ReadInterface()
        {
            string path = Application.ResourceAssembly.Location.Replace("Clever.exe", "Localization\\");
            string loc = Configurations.Get.Language;
            path += loc + "\\interface_" + loc + ".txt";
            var dictionary = new Dictionary<string, string>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    var tmp = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    dictionary.Add(tmp[0].Trim(), tmp[1].Trim());
                }
            }
            return dictionary;
        }
        public Dictionary<string, string> ReadErrors()
        {
            string path = Application.ResourceAssembly.Location.Replace("Clever.exe", "Localization\\");
            string loc = Configurations.Get.Language;
            path += loc + "\\errors_" + loc + ".txt";
            var dictionary = new Dictionary<string, string>();
            var fi = new FileInfo(path);

            if (!fi.Exists)
            {
                return null;
            }

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    var tmp = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    dictionary.Add(tmp[0].Trim(), tmp[1].Trim());
                }
            }
            return dictionary;
        }
    }
}
