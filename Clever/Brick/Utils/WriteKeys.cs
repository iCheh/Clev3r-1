using System.IO;

namespace Clever.Brick.Utils
{
    class WriteKeys
    {
        public WriteKeys()
        {
        }

        public void Write(string path, string name)
        {
            string text = SetText();
            File.WriteAllText(path + "\\~" + name.Replace(".bp", "") + "\\" + name.Replace(".bp", ".info"), text);
        }

        private string SetText()
        {
            string text = "";

            text += "FOLDER_KEY " + Keys.FolderKey + '\n';
            text += "FOLDER_NAME " + Keys.FolderName + '\n';

            if (Keys.ImageList != null)
            {
                text += "IMAGE_LIST " + Keys.ImageList.Count + '\n';
            }
            if (Keys.ImageList != null && Keys.ImageList.Count > 0)
            {
                for (int i = 0; i < Keys.ImageList.Count; i++)
                {
                    text += "IMAGE" + " ; " + Keys.ImageList[i] + " ; " + Keys.ImagePath[i] + " ; " + Keys.ImageFullName[i] + '\n';
                }
            }

            if (Keys.SoundList != null)
            {
                text += "SOUND_LIST " + Keys.SoundList.Count + '\n';
            }
            if (Keys.SoundList != null && Keys.SoundList.Count > 0)
            {
                for (int i = 0; i < Keys.SoundList.Count; i++)
                {
                    text += "SOUND" + " ; " + Keys.SoundList[i] + " ; " + Keys.SoundPath[i] + " ; " + Keys.SoundFullName[i] + '\n';
                }
            }

            //MainWindowVM.Status.Add(Keys.FileList.Count.ToString() + " " + Keys.FilePath.Count.ToString() + " " + Keys.FileFullName.Count.ToString() + " ");
            if (Keys.FileList != null)
            {
                text += "FILE_LIST " + Keys.FileList.Count + '\n';
            }
            if (Keys.FileList != null && Keys.FileList.Count > 0)
            {
                for (int i = 0; i < Keys.FileList.Count; i++)
                {
                    text += "FILE" + " ; " + Keys.FileList[i] + " ; " + Keys.FilePath[i] + " ; " + Keys.FileFullName[i] + '\n';
                }
            }
            return text;
        }
    }
}
