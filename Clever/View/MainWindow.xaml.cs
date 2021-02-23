using Clever.CommonData;
using Clever.Model.Utils;
using Clever.Properties;
using Clever.View.Controls.Editors;
using Clever.View.Controls.Helps;
using Clever.ViewModel;
using Clever.ViewModel.PanelsVM;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Clever
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindowVM context;
        static double _widthGrid = 0;
        static Grid _eidtorGrid;
        static MainWindow _main;
        //static Popup _popup;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM();
            context = (MainWindowVM)this.DataContext;
            _eidtorGrid = docGrid;
            _main = this;
            App.SplashScreen.Close(new TimeSpan(0, 0, 1));
        }

        public static MainWindow GetContext
        {
            get { return _main; }
        }

        /*
        public static bool Topmost
        {
            get { return _main.Parent}
        }
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindowVM.Project.Count == 0)
            {
                App.Current.Shutdown();
            }
            bool save = false;
            var prjV = MainWindowVM.Project.GetDictionary().Values;
            foreach (var pd in prjV)
            {
                //MessageBox.Show("===" + pd.Name + pd.GetHeaderChangeText + "===");
                if (pd.GetHeaderChangeText != "" || pd.TextChange == true)
                {
                    pd.TextChange = true;
                    save = true;
                    break;
                }
            }

            if (save)
            {
                int rez = DoYouWantToClose();
                if (rez == 1)
                {
                    //MainWindowVM.SaveAllAndClose();
                    e.Cancel = false;
                }
                else if (rez == -1)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private int DoYouWantToClose()
        {
            var win = new View.Dialogs.SaveAllPrograms(MainWindowVM.GetLocalization["dSaveAll1"] + " " + MainWindowVM.GetLocalization["dSaveAll2"]);
            if (win.ShowDialog() == true)
            {
                return -1;
            }
            else
            {
                return 0;
            }
            /*
            MessageBoxResult result = MessageBox.Show("В программах есть изменения.\n" + "Хотите их сохранить?", "", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return true;
            }
            */
            //return true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Configurations.Save();
            var id = Process.GetCurrentProcess().Id;
            var prc = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (prc.Length > 0)
            {
                foreach (var p in prc)
                {
                    if (p.Id != id)
                    {
                        p.Kill();
                    }
                }
            }
            base.OnClosing(e);
        }

        private void WindowWPF_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.N && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                context.FileNew();
                e.Handled = true;
            }
            else if (e.Key == Key.O && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                context.FileOpen();
                e.Handled = true;
            }
            else if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                context.FileSave();
                e.Handled = true;
            }
            else if (e.Key == Key.F4)
            {
                EV3Brick.GetEV3Brick.Compile_PreviewMouseLeftButtonDown(null, null);
            }
            else if (e.Key == Key.F5)
            {
                EV3Brick.GetEV3Brick.CompileAndRun_PreviewMouseLeftButtonDown(null, null);
            }
            else if (e.Key == Key.F6)
            {
                EV3Brick.GetEV3Brick.BrickStop_PreviewMouseLeftButtonDown(null, null);
            }
        }

        public static void WindowForm_PreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F4)
            {
                EV3Brick.GetEV3Brick.Compile_PreviewMouseLeftButtonDown(null, null);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                EV3Brick.GetEV3Brick.CompileAndRun_PreviewMouseLeftButtonDown(null, null);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F6)
            {
                EV3Brick.GetEV3Brick.BrickStop_PreviewMouseLeftButtonDown(null, null);
            }
        }
        /*
        private void StatusMessage_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                        if (File.Exists(name))
                        {
                            MainWindowVM.FileNew(name, false);
                        }
                        if (MainWindowVM.Project.GetDictionary().ContainsKey(fileName))
                        {
                            var item = MainWindowVM.Project.GetDictionary()[fileName];
                            item.Item.IsSelected = true;
                            item.Editor.SelectLine(Convert.ToInt32(line) - 1);
                        }
                        else
                        {
                            Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + name + " (" + fileName + ") " + MainWindowVM.GetLocalization["prepRead2"]);
                        }
                    }
                }
            }
        }
        */
        private void EditorPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _widthGrid = docGrid.Children[0].DesiredSize.Width;
            _eidtorGrid = docGrid;
            ChangeSize();
        }

        public static void UpdateSize()
        {
            _widthGrid = _eidtorGrid.Children[0].DesiredSize.Width;
        }

        public void UpdateGridSize()
        {
            _widthGrid = docGrid.Children[0].DesiredSize.Width;
            _eidtorGrid = docGrid;
            if (_widthGrid <= 1)
            {
                _widthGrid = Configurations.Get.Grid_Column0.Value;
            }
        }

        public void ChangeSize()
        {
            if (MainWindowVM.ProgramNameList == null || MainWindowVM.ProgramNameList.Count == 0)
            {
                return;
            }

            double wh = 150;
            for (int i = 0; i < MainWindowVM.ProgramNameList.Count; i++)
            {
                TabItem item = MainWindowVM.ProgramNameList[i];
                var h = (EditorHeader)item.Header;
                if (i == 0 && h.GetWidth == 0)
                {
                    wh += h.GetWidth + 1;
                }
                else if (i > 0)
                {
                    wh += h.GetWidth + 1;
                }

                if (wh <= _eidtorGrid.Children[0].DesiredSize.Width - 50)
                {
                    item.Visibility = Visibility.Visible;
                    MainWindowVM.HiddenProgramButton = Visibility.Hidden;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                    MainWindowVM.HiddenProgramButton = Visibility.Visible;
                }
            }
        }

        public static double GetWidthGrid
        {
            get { return _widthGrid; }
        }

        public static Grid GetEditorGrid
        {
            get { return _eidtorGrid; }
        }
        /*
        private string GetName(string word)
        {
            int start = word.LastIndexOf("\\") + 1;
            return word.Substring(start);
        }
        */
    }
}
