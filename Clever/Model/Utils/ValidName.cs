using System.Text.RegularExpressions;

namespace Clever.Model.Utils
{
    class ValidName
    {
        public bool Valid(string name)
        {
            bool ret = false;

            if (name.Length > 30)
            {
                string message = new SystemMessage().ToNameLength;
                ShowMessage(message);
            }
            else if (name == "")
            {
                string message = new SystemMessage().ToNameNull;
                ShowMessage(message);
            }
            else if (name[0] == '_')
            {
                string message = new SystemMessage().ToNameUndersgore;
                ShowMessage(message);
            }
            else if (name[0] == '0' || name[0] == '1' || name[0] == '2' || name[0] == '3' || name[0] == '4' || name[0] == '5' || name[0] == '6' || name[0] == '7' || name[0] == '8' || name[0] == '9')
            {
                string message = new SystemMessage().ToNameNumber;
                ShowMessage(message);
            }
            else
            {
                Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = regex.Matches(name);
                if (matches.Count != name.Length)
                {
                    string message = new SystemMessage().ToNameRegex;
                    ShowMessage(message);
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        public string ValidMessage(string name)
        {
            string message = "";

            if (name.Length > 30)
            {
                message = new SystemMessage().ToNameLength;
            }
            else if (name == "")
            {
                message = new SystemMessage().ToNameNull;
            }
            else if (name[0] == '_')
            {
                message = new SystemMessage().ToNameUndersgore;
            }
            else if (name[0] == '0' || name[0] == '1' || name[0] == '2' || name[0] == '3' || name[0] == '4' || name[0] == '5' || name[0] == '6' || name[0] == '7' || name[0] == '8' || name[0] == '9')
            {
                message = new SystemMessage().ToNameNumber;
            }
            else
            {
                Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = regex.Matches(name);
                if (matches.Count != name.Length)
                {
                    message = new SystemMessage().ToNameRegex;
                }
            }
            return message;
        }

        private void ShowMessage(string message)
        {
            var win = new View.Dialogs.NameValidator(message);
            win.ShowDialog();
        }
    }
}
