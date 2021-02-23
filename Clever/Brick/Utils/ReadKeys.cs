using System;
using System.Collections.Generic;
using System.IO;

namespace Clever.Brick.Utils
{
    class ReadKeys
    {
        public ReadKeys()
        {
            Keys.FolderKey = "";
            Keys.FolderName = "";

            if (Keys.ImageList == null)
                Keys.ImageList = new List<string>();
            else
                Keys.ImageList.Clear();

            if (Keys.SoundList == null)
                Keys.SoundList = new List<string>();
            else
                Keys.SoundList.Clear();

            if (Keys.ImagePath == null)
                Keys.ImagePath = new List<string>();
            else
                Keys.ImagePath.Clear();

            if (Keys.SoundPath == null)
                Keys.SoundPath = new List<string>();
            else
                Keys.SoundPath.Clear();

            if (Keys.ImageFullName == null)
                Keys.ImageFullName = new List<string>();
            else
                Keys.ImageFullName.Clear();

            if (Keys.SoundFullName == null)
                Keys.SoundFullName = new List<string>();
            else
                Keys.SoundFullName.Clear();
            //
            if (Keys.FileList == null)
                Keys.FileList = new List<string>();
            else
                Keys.FileList.Clear();

            if (Keys.FilePath == null)
                Keys.FilePath = new List<string>();
            else
                Keys.FilePath.Clear();

            if (Keys.FileFullName == null)
                Keys.FileFullName = new List<string>();
            else
                Keys.FileFullName.Clear();
        }

        public void Read(string file)
        {
            var text = File.ReadAllLines(file);

            foreach (var line in text)
            {
                if (line.IndexOf("FOLDER_KEY") != -1)
                {
                    var words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Keys.FolderKey = words[1].Trim().ToUpper();
                }
                else if (line.IndexOf("FOLDER_NAME") != -1)
                {
                    var words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Keys.FolderName = words[1].Trim();
                }
                else if (line.IndexOf("IMAGE") != -1 && line.IndexOf("IMAGE_LIST ") == -1)
                {
                    var words = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Keys.ImageList.Add(words[1].Trim());
                    Keys.ImagePath.Add(words[2].Trim());
                    Keys.ImageFullName.Add(words[3].Trim());
                }
                else if (line.IndexOf("SOUND") != -1 && line.IndexOf("SOUND_LIST ") == -1)
                {
                    var words = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Keys.SoundList.Add(words[1].Trim());
                    Keys.SoundPath.Add(words[2].Trim());
                    Keys.SoundFullName.Add(words[3].Trim());
                }
                else if (line.IndexOf("FILE") != -1 && line.IndexOf("FILE_LIST ") == -1)
                {
                    var words = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Keys.FileList.Add(words[1].Trim());
                    Keys.FilePath.Add(words[2].Trim());
                    Keys.FileFullName.Add(words[3].Trim());
                }
            }
        }
    }
}
