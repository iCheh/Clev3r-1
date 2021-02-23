using Clever.CommonData;
using Clever.ViewModel;
using System.IO;

namespace Clever.Brick.Utils
{
    class Reader
    {
        private string[] text;

        public Reader()
        {
        }

        public string[] ReadFile(string fuulPath)
        {
            if (!File.Exists(fuulPath))
            {
                Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + fuulPath + " " + MainWindowVM.GetLocalization["prepRead2"]);
                return text = new string[] { };
            }
            return text = File.ReadAllLines(fuulPath);
        }

        public string[] ReadFile(string path, string name, out bool errore)
        {
            errore = false;
            if (!File.Exists(path + name))
            {
                Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + path + name + " " + MainWindowVM.GetLocalization["prepRead2"]);
                errore = true;
                return text = new string[] { };
            }
            return text = File.ReadAllLines(path + name);
        }
    }
}
