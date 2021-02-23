using System.IO;
using System.IO.Pipes;

namespace Clever.Model.Utils
{
    class CleverClient
    {
        string _path = "";

        public CleverClient(string path)
        {
            _path = path;
        }

        public void StartClient()
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream("clever_pipe"))
            {
                pipeClient.Connect();
                using (StreamWriter sw = new StreamWriter(pipeClient))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(_path);
                }
            }
        }
    }
}
