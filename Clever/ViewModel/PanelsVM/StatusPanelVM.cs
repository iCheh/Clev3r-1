using Clever.CommonData;
using Clever.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.ViewModel.PanelsVM
{
    internal class StatusPanelVM : BaseViewModel
    {
        public ICommand ClearStatusCommand { get; set; }

        internal StatusPanelVM()
        {
            SetSetting();           
        }

        #region Bindings

        public string ClearStatusLine
        {
            get { return GUILanguage.GetItem("tClearStatus"); }
        }

        private ObservableCollection<string> _status;
        public ObservableCollection<string> Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private bool _statusMenuOpen;
        public bool StatusMenuOpen
        {
            get { return _statusMenuOpen; }
            set
            {
                _statusMenuOpen = value;
                OnPropertyChanged("StatusMenuOpen");
            }
        }

        #endregion

        #region Method For Command

        private void ClearStatus()
        {
            Status.Clear();
            StatusMenuOpen = false;
        }

        #endregion

        #region Events

        private void Status_StatusChanged(object sender, EventArgs e)
        {
            var items = sender as ObservableCollection<string>;
            Status = items;
        }

        internal void StatusMessage_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainWindowVM.Project.Count == 0)
            {
                return;
            }
            ListBox lb = (ListBox)sender;
            string message = (string)lb.SelectedItem;
            if (message == null)
            {
                return;
            }
            if (message.IndexOf("file:") != -1 && message.IndexOf("line:") != -1 && message.IndexOf("| code:") != -1)
            {
                int startName = message.IndexOf("file:") + 5;
                int endName = message.IndexOf("line:") - 1;
                int startLine = message.IndexOf("line:") + 5;
                int endLine = message.IndexOf("| code:") - 1;

                string name = message.Substring(startName, endName - startName).Trim();
                string line = message.Substring(startLine, endLine - startLine).Trim();
                string fileName = GetName(name);
                if (MainWindowVM.Project.GetDictionary().ContainsKey(fileName))
                {
                    var item = MainWindowVM.Project.GetDictionary()[fileName];
                    item.Item.IsSelected = true;
                    item.Editor.SelectLine(Convert.ToInt32(line) - 1);
                }
                else
                {
                    if (name != "")
                    {
                        if (File.Exists(name.Replace("/","\\")))
                        {
                            MainWindowVM.FileNew(name.Replace("/", "\\"), false);
                        }
                        if (MainWindowVM.Project.GetDictionary().ContainsKey(fileName))
                        {
                            var item = MainWindowVM.Project.GetDictionary()[fileName];
                            item.Item.IsSelected = true;
                            item.Editor.SelectLine(Convert.ToInt32(line) - 1);
                        }
                        else
                        {
                            Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + name.Replace("/", "\\") + " (" + fileName + ") " + MainWindowVM.GetLocalization["prepRead2"]);
                        }
                    }
                }
            }
        }

        internal void StatusMessage_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            StatusMenuOpen = true;
        }

        #endregion

        #region Configuration

        private void SetSetting()
        {
            Status = new ObservableCollection<string>();
            CommonData.Status.Items = Status;
            CommonData.Status.StatusChanged += Status_StatusChanged;
            StatusMenuOpen = false;
            SetCommand();
        }

        

        private void SetCommand()
        {
            ClearStatusCommand = new RelayCommand(param => ClearStatus(), true);
        }

        #endregion

        #region Utils

        private string GetName(string word)
        {
            int start = word.Replace("/","\\").LastIndexOf("\\") + 1;
            return word.Substring(start);
        }

        #endregion
    }
}
