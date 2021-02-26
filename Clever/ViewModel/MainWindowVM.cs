using Clever.Model.Bplus;
using Clever.Model.Program;
using Clever.Model.Utils;
using Clever.View.Controls;
using Clever.View.Controls.Editors;
using Clever.CommonData;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Clever.ViewModel.BaseVM;
using Clever.ViewModel.PanelsVM;
using Clever.Model.Intellisense;

namespace Clever.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public ICommand FileNewCommand { get; set; }
        public ICommand FileOpenCommand { get; set; }
        public ICommand FileSaveCommand { get; set; }
        public ICommand FileSaveAllCommand { get; set; }
        public ICommand FileSaveAsCommand { get; set; }
        public ICommand FileCloseAllCommand { get; set; }
        public ICommand EditCopyCommand { get; set; }
        public ICommand EditPasteCommand { get; set; }
        public ICommand EditCutCommand { get; set; }
        public ICommand EditUndoCommand { get; set; }
        public ICommand EditRedoCommand { get; set; }
        public ICommand EditSelectAllCommand { get; set; }
        public ICommand EditFormatCommand { get; set; }
        public ICommand EditShowLineCommand { get; set; }
        public ICommand EditShowNumberCommand { get; set; }
        public ICommand EditShowSpaceCommand { get; set; }
        public ICommand EditWrapCommand { get; set; }
        public ICommand EditShowLineMenuCommand { get; set; }
        public ICommand EditShowNumberMenuCommand { get; set; }
        public ICommand EditShowSpaceMenuCommand { get; set; }
        public ICommand EditWrapMenuCommand { get; set; }
        public ICommand EditCommentCommand { get; set; }
        public ICommand EditUnCommentCommand { get; set; }
        public ICommand EditFoldAllCommand { get; set; }
        public ICommand EditUnFoldAllCommand { get; set; }
        public ICommand CloseProgramCommand { get; set; }
        
        public ICommand FindCommand { get; set; }
        public ICommand GetProgramNamesCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        public static ProjectData Project;
        public static string ext = ".bp";
        static string currentPath = "";

        Elements elements;
        BPInterop interop;

        public static SearchFlags searchFlags = SearchFlags.None;
        public static bool highlightAll = false;

        public MainWindowVM()
        {
            CurrentName = "";
            Project = new ProjectData();

            //=====================
            //Brick
            BrickConnect = false;
            //=====================

            elements = new Elements();
            _programNameList = new ObservableCollection<TabItem>();

            FileNewCommand = new RelayCommand(param => FileNew(), true);
            FileOpenCommand = new RelayCommand(param => FileOpen(), true);
            FileSaveCommand = new RelayCommand(param => FileSave(), true);
            FileSaveAllCommand = new RelayCommand(param => FileSaveAll(), true);
            FileSaveAsCommand = new RelayCommand(param => FileSaveAs(), true);
            FileCloseAllCommand = new RelayCommand(param => FileCloseAll(), true);
            EditCopyCommand = new RelayCommand(param => EditCopy(), true);
            EditPasteCommand = new RelayCommand(param => EditPaste(), true);
            EditCutCommand = new RelayCommand(param => EditCut(), true);
            EditUndoCommand = new RelayCommand(param => EditUndo(), true);
            EditRedoCommand = new RelayCommand(param => EditRedo(), true);
            EditSelectAllCommand = new RelayCommand(param => EditSelectAll(), true);
            EditFormatCommand = new RelayCommand(param => EditFormat(), true);
            EditShowLineCommand = new RelayCommand(param => EditShowLine(), true);
            EditShowNumberCommand = new RelayCommand(param => EditShowNumber(), true);
            EditShowSpaceCommand = new RelayCommand(param => EditShowSpace(), true);
            EditWrapCommand = new RelayCommand(param => EditWrap(), true);
            EditShowLineMenuCommand = new RelayCommand(param => EditShowLineMenu(), true);
            EditShowNumberMenuCommand = new RelayCommand(param => EditShowNumberMenu(), true);
            EditShowSpaceMenuCommand = new RelayCommand(param => EditShowSpaceMenu(), true);
            EditWrapMenuCommand = new RelayCommand(param => EditWrapMenu(), true);

            EditCommentCommand = new RelayCommand(param => EditComment(), true);
            EditUnCommentCommand = new RelayCommand(param => EditUnComment(), true);
            EditFoldAllCommand = new RelayCommand(param => EditFoldAll(), true);
            EditUnFoldAllCommand = new RelayCommand(param => EditUnFoldAll(), true);

            
            CloseProgramCommand = new RelayCommand(param => CloseProgram(), true);
            FindCommand = new RelayCommand(param => FindText(), true);
            GetProgramNamesCommand = new RelayCommand(param => SetProgramName(), true);
            AboutCommand = new RelayCommand(param => About(), true);

            ShowNumber = Configurations.Get.ShowNumber;
            EditShowNumber();
            ShowLine = Configurations.Get.ShowLine;
            EditShowLine();
            ShowSpace = Configurations.Get.ShowSpace;
            EditShowSpace();
            ShowWrap = Configurations.Get.Wrap;
            EditWrap();

            interop = new BPInterop();
            SpWidth = 300;

            if (Configurations.Get.File_New == true) FileNewV = Visibility.Visible;
            else FileNewV = Visibility.Collapsed;
            if (Configurations.Get.File_Open == true) FileOpenV = Visibility.Visible;
            else FileOpenV = Visibility.Collapsed;
            if (Configurations.Get.File_CloseAll == true) FileCloseAllV = Visibility.Visible;
            else FileCloseAllV = Visibility.Collapsed;
            if (Configurations.Get.File_Save == true) FileSaveV = Visibility.Visible;
            else FileSaveV = Visibility.Collapsed;
            if (Configurations.Get.File_SaveAll == true) FileSaveAllV = Visibility.Visible;
            else FileSaveAllV = Visibility.Collapsed;
            if (Configurations.Get.File_SaveAs == true) FileSaveAsV = Visibility.Visible;
            else FileSaveAsV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Copy == true) EditCopyV = Visibility.Visible;
            else EditCopyV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Paste == true) EditPasteV = Visibility.Visible;
            else EditPasteV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Cut == true) EditCutV = Visibility.Visible;
            else EditCutV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Undo == true) EditUndoV = Visibility.Visible;
            else EditUndoV = Visibility.Collapsed;
            if (Configurations.Get .Edit_Redo == true) EditRedoV = Visibility.Visible;
            else EditRedoV = Visibility.Collapsed;
            if (Configurations.Get.Edit_SelectAll == true) EditSelectAllV = Visibility.Visible;
            else EditSelectAllV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Format == true) EditFormatV = Visibility.Visible;
            else EditFormatV = Visibility.Collapsed;
            if (Configurations.Get.Edit_ShowLine == true) EditShowLineV = Visibility.Visible;
            else EditShowLineV = Visibility.Collapsed;
            if (Configurations.Get.Edit_ShowNumber == true) EditShowNumberV = Visibility.Visible;
            else EditShowNumberV = Visibility.Collapsed;
            if (Configurations.Get.Edit_ShowSpace == true) EditShowSpaceV = Visibility.Visible;
            else EditShowSpaceV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Wrap == true) EditWrapV = Visibility.Visible;
            else EditWrapV = Visibility.Collapsed;
            if (Configurations.Get.Edit_Find == true) EditFindV = Visibility.Visible;
            else EditFindV = Visibility.Collapsed;

            if (Configurations.Get.Brick_Compile == true) CompileV = Visibility.Visible;
            else CompileV = Visibility.Collapsed;
            if (Configurations.Get.Brick_CompileAndRun == true) CompileAndRunV = Visibility.Visible;
            else CompileAndRunV = Visibility.Collapsed;
            if (Configurations.Get.Brick_Connect == true) ConnectV = Visibility.Visible;
            else ConnectV = Visibility.Collapsed;
            if (Configurations.Get.Brick_Download == true) DownloadV = Visibility.Visible;
            else DownloadV = Visibility.Collapsed;
            if (Configurations.Get.Brick_Stop == true) StopV = Visibility.Visible;
            else StopV = Visibility.Collapsed;

            HiddenProgramButton = Visibility.Hidden;

            IntellisenseParser.Install();

            Topmost();
        }

        private static void Topmost()
        {
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.Topmost = false;
        }

        private static void StartNewServer()
        {
            var server = new CleverServer();
            server.Start();
        }

        private static ObservableCollection<TabItem> _programNameList;
        public static ObservableCollection<TabItem> ProgramNameList
        {
            get { return _programNameList; }
            set
            {
                _programNameList = value;


                if (_programNameList.Count <= 1)
                {
                    OnStaticPropertyChanged("ProgramNameList");
                    return;
                }

                double wh = 150;

                for (int i = 0; i < _programNameList.Count; i++)
                {
                    TabItem item = _programNameList[i];
                    var h = (EditorHeader)item.Header;

                    if (i == 0 && h.GetWidth == 0)
                    {
                        wh += h.GetWidth + 1;
                    }
                    else if (i > 0)
                    {
                        wh += h.GetWidth + 1;
                    }

                    if (wh <= MainWindow.GetWidthGrid - 50)
                    {
                        item.Visibility = Visibility.Visible;
                        HiddenProgramButton = Visibility.Hidden;
                    }
                    else
                    {
                        item.Visibility = Visibility.Collapsed;
                        HiddenProgramButton = Visibility.Visible;
                    }
                }
                OnStaticPropertyChanged("ProgramNameList");
            }
        }

        private static TabItem _currentProgram;
        public static TabItem CurrentProgram
        {
            get { return _currentProgram; }
            set
            {
                _currentProgram = value;
                OnStaticPropertyChanged("CurrentProgram");

                if (_currentProgram != null)
                {
                    BpObjects.UpdateWork = false;
                    Project.UpdateColor();
                    var header = _currentProgram.Header as EditorHeader;
                    var name = header.HeaderName;
                    if (Project.ContainsKey(name))
                    {
                        CurrentName = name;
                        //var data = Project.GetDictionary()[name];
                        IntellisenseParser.UpdateMap(CurrentName);
                    }
                    else
                    {
                        CurrentName = "";
                    }
                }
                else
                {
                    CurrentName = "";
                }
            }
        }

        internal static string CurrentName { get; private set; }

        private static ContentControl _currentInfo;
        public static ContentControl CurrentInfo
        {
            get { return _currentInfo; }

            set
            {
                _currentInfo = value;
                OnStaticPropertyChanged("CurrentInfo");
            }
        }

        private static string _textFind = "";
        public static string TextFind
        {
            get { return _textFind; }

            set
            {
                _textFind = value;
                OnStaticPropertyChanged("TextFind");
            }
        }

        private static ObservableCollection<string> _prgName = new ObservableCollection<string>();
        public static ObservableCollection<string> ProgramName
        {
            get { return _prgName; }
            set
            {
                _prgName = value;
                OnStaticPropertyChanged("ProgramName");
            }
        }

        #region Checked button

        private static bool _showNumber;
        public static bool ShowNumber
        {
            get { return _showNumber; }
            set
            {
                _showNumber = value;
                Configurations.Get.ShowNumber = _showNumber;
                OnStaticPropertyChanged("ShowNumber");
            }
        }

        private static bool _showLine;
        public static bool ShowLine
        {
            get { return _showLine; }
            set
            {
                _showLine = value;
                Configurations.Get.ShowLine = _showLine;
                OnStaticPropertyChanged("ShowLine");
            }
        }

        private static bool _showSpace;
        public static bool ShowSpace
        {
            get { return _showSpace; }
            set
            {
                _showSpace = value;
                Configurations.Get.ShowSpace = _showSpace;
                OnStaticPropertyChanged("ShowSpace");
            }
        }

        private static bool _showWrap;
        public static bool ShowWrap
        {
            get { return _showWrap; }
            set
            {
                _showWrap = value;
                Configurations.Get.Wrap = _showWrap;
                OnStaticPropertyChanged("ShowWrap");
            }
        }

        #endregion

        #region Brick checed button

        private static bool _brickConnect;
        public static bool BrickConnect
        {
            get { return _brickConnect; }
            set
            {
                _brickConnect = value;
                OnStaticPropertyChanged("BrickConnect");
            }
        }
        #endregion

        #region Panel size

        private static double _spWidth;
        public static double SpWidth
        {
            get { return _spWidth; }
            set
            {
                _spWidth = value;
                HelperPanelVM.Get.SpWidth = value;
            }
        }

        private static double _spWidthCl;
        public static double SpWidthCl
        {
            get { return _spWidthCl; }
            set
            {
                _spWidthCl = value;
                HelperPanelVM.Get.SpWidthCl = value;
            }
        }

        private static double _widthHelpPanel;
        public static double WidthHelpPanel
        {
            get { return _widthHelpPanel; }
            set
            {
                _widthHelpPanel = value;
                HelperPanelVM.Get.WidthHelpPanel = value;
                OnStaticPropertyChanged("WidthHelpPanel");
            }
        }

        private static double _heightHelpPanel;
        public static double HeightHelpPanel
        {
            get { return _heightHelpPanel; }
            set
            {
                _heightHelpPanel = value;
                HelperPanelVM.Get.HeightHelpPanel = value;
                OnStaticPropertyChanged("HeightHelpPanel");
            }
        }

        #endregion

        public void FileNew()
        {
            MainWindow.UpdateSize();
            string name = "";
            string path = "";
            var tmp = new NewTabItem().Create(Project.GetDictionary(), out name, out path);
            if (tmp == null)
            {
                return;
            }
            var pd = new ProgramData();
            pd.OldName = name;
            pd.ClosedName = name;
            pd.Name = name;
            pd.Path = path;
            pd.FullPath = Path.Combine(path, name);
            pd.Item = tmp;
            pd.TextChange = false;
            if (Project.Add(name, pd))
            {
                var pnl = new ObservableCollection<TabItem>();
                pnl.Add(pd.Item);
                foreach (var i in ProgramNameList)
                {
                    i.Style = (System.Windows.Style)Application.Current.Resources["NextTabItem"];
                    pnl.Add(i);
                }
                ProgramNameList = pnl;

                CurrentProgram = pd.Item;
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        public static void FileNew(string path, bool start)
        {
            ProgramData pdOld = null;
            bool tcOld = false;
            if (CurrentProgram != null)
            {
                pdOld = Project.GetProgramData(CurrentProgram);
                tcOld = pdOld.TextChange;
            }

            FileInfo fi = new FileInfo(path);
            if (Project.ContainsKey(fi.Name))
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgFindName1"] + " " + fi.Name + " " + GetLocalization["prgFindName2"]);

                Topmost();
                if (start)
                {
                    StartNewServer();
                }
                return;
            }
            else if (fi.Extension != ext && fi.Extension != ".bpm" && fi.Extension != ".bpi")
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgInvalidExt1"] + " " + fi.Extension + " " + GetLocalization["prgInvalidExt2"]);
                Topmost();
                if (start)
                {
                    StartNewServer();
                }
                return;
            }

            if (Project != null && Project.Count != 0)
                MainWindow.UpdateSize();

            string text = File.ReadAllText(path);
            var tmp = new NewTabItem().Create(Project.GetDictionary(), text, fi.Name);
            var pd = new ProgramData();
            pd.OldName = fi.Name;
            pd.ClosedName = fi.Name;
            pd.Name = fi.Name;
            pd.Path = fi.DirectoryName;
            pd.FullPath = fi.FullName;
            pd.Item = tmp;
            pd.TextChange = false;
            currentPath = fi.FullName;

            if (Project.Add(pd.Name, pd))
            {
                var pnl = new ObservableCollection<TabItem>();
                pnl.Add(pd.Item);
                foreach (var i in ProgramNameList)
                {
                    i.Style = (System.Windows.Style)Application.Current.Resources["NextTabItem"];
                    pnl.Add(i);
                }
                ProgramNameList = pnl;
                CurrentProgram = pd.Item;
            }

            if (pdOld != null)
                pdOld.TextChange = tcOld;

            Topmost();

            if (start)
            {
                StartNewServer();
            }
        }

        public static void FindAndReplaceFile(string name, string path)
        {
            var tmpName = "~" + name;
            if (Project.ContainsKey(tmpName))
            {
                var pd = Project.GetDictionary()[tmpName];

                if (pd.FullPath == path + tmpName.Replace(ext, "") + "\\" + tmpName)
                {
                    string text = File.ReadAllText(pd.FullPath);
                    pd.Editor.TextArea.Text = text;
                    pd.TextChange = false;
                }
            }
            Project.GetProgramData(CurrentProgram).TextChange = false;
        }

        public void FileOpen()
        {
            try
            {
                MainWindow.UpdateSize();
                string tmpName = "";
                string tmpDirectory = "";
                string tmpFullPath = "";
                bool changeCurPrg = false;
                ProgramData tmpCurPrg;
                if (CurrentProgram != null)
                {
                    tmpCurPrg = Project.GetProgramData(CurrentProgram);
                }
                else
                {
                    tmpCurPrg = null;
                }

                if (tmpCurPrg != null)
                {
                    if (tmpCurPrg.TextChange)
                    {
                        changeCurPrg = true;
                    }
                }
                var text = new OpenFile().Open(currentPath, out tmpFullPath, out tmpDirectory, out tmpName);
                if (text != null)
                {
                    if (Project.Count > 0 && Project.ContainsKey(tmpName))
                    {
                        CommonData.Status.Clear();
                        CommonData.Status.Add(GetLocalization["prgFindName1"] + " " + tmpName + " " + GetLocalization["prgFindName2"]);
                        return;
                    }

                    if (IntellisenseParser.Data.ContainsKey(tmpName))
                    {
                        IntellisenseParser.Data.Remove(tmpName);
                    }

                    var tmp = new NewTabItem().Create(Project.GetDictionary(), text, tmpName);
                    var pd = new ProgramData();
                    pd.OldName = tmpName;
                    pd.ClosedName = tmpName;
                    pd.Name = tmpName;
                    pd.Path = tmpDirectory;
                    pd.FullPath = tmpFullPath;
                    pd.Item = tmp;
                    pd.TextChange = false;
                    if (Project.Add(pd.Name, pd))
                    {
                        var pnl = new ObservableCollection<TabItem>();
                        pnl.Add(pd.Item);
                        foreach (var i in ProgramNameList)
                        {
                            i.Style = (System.Windows.Style)Application.Current.Resources["NextTabItem"];
                            pnl.Add(i);
                        }
                        ProgramNameList = pnl;
                        CurrentProgram = pd.Item;
                        if (!changeCurPrg && tmpCurPrg != null)
                        {
                            tmpCurPrg.TextChange = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        public void FileSave()
        {
            if (ProgramNameList.Count == 0)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgNotPrograms"]);
                return;
            }
            try
            {
                var name = Project.SelectedItemName();
                if (name != "")
                {
                    var pd = Project.GetProgramData(CurrentProgram);
                    if (pd != null && pd.FullPath != "")
                    {
                        new SaveFile().Save(pd);
                        pd.TextChange = false;
                    }
                    else
                    {
                        FileSaveAsOther();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        public static void SaveAllAndClose()
        {
            if (ProgramNameList.Count == 0)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgNotPrograms"]);
                return;
            }
            try
            {
                foreach (var i in ProgramNameList)
                {
                    var pd = Project.GetProgramData(i);
                    if ((pd != null && pd.TextChange) || pd.GetHeaderChangeText != "")
                    {
                        if (pd.FullPath != "")
                        {
                            new SaveFile().Save(pd);
                        }
                        else
                        {
                            string tmpExt = "";
                            for (int a = pd.Name.IndexOf('.'); a < pd.Name.Length; a++)
                            {
                                tmpExt += pd.Name[a];
                            }
                            new SaveFile().Save(pd.Editor.TextArea.Text, pd.Name, tmpExt);
                        }
                        pd.TextChange = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
        }

        private void FileSaveAll()
        {
            if (ProgramNameList.Count == 0)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgNotPrograms"]);
                return;
            }

            try
            {
                foreach (var i in ProgramNameList)
                {
                    var pd = Project.GetProgramData(i);
                    if (pd != null)
                    {
                        if (pd.FullPath != "")
                        {
                            new SaveFile().Save(pd);
                            pd.TextChange = false;
                        }
                        else
                        {
                            string tmpExt = "";
                            for (int a = pd.Name.IndexOf('.'); a < pd.Name.Length; a++)
                            {
                                tmpExt += pd.Name[a];
                            }
                            string name = new SaveFile().Save(pd.Editor.TextArea.Text, pd.Name, tmpExt);
                            if (name != "")
                            {
                                FileInfo fi = new FileInfo(name);
                                string oldName = pd.Name;
                                Project.Remove(oldName);
                                var newPd = new ProgramData() { Item = i };
                                //
                                //newPd.Name = fi.Name.Replace(ext, "");
                                newPd.Name = fi.Name;
                                //
                                newPd.Path = fi.DirectoryName;
                                newPd.FullPath = fi.FullName;
                                newPd.TextChange = false;
                                Project.Add(newPd.Name, newPd);
                                currentPath = fi.FullName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        private void FileSaveAs()
        {
            if (ProgramNameList.Count == 0)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgNotPrograms"]);
                return;
            }

            try
            {
                var pd = Project.GetProgramData(CurrentProgram);
                string tmpExt = "";
                for (int a = pd.Name.IndexOf('.'); a < pd.Name.Length; a++)
                {
                    tmpExt += pd.Name[a];
                }
                string name = new SaveFile().Save(pd.Editor.TextArea.Text, pd.Name, "");
                if (name != "")
                {
                    FileInfo fi = new FileInfo(name);
                    string oldName = pd.Name;
                    Project.Remove(oldName);
                    var newPd = new ProgramData() { Item = CurrentProgram };
                    //
                    //newPd.Name = fi.Name.Replace(ext, "");
                    newPd.Name = fi.Name;
                    //
                    newPd.Path = fi.DirectoryName;
                    newPd.FullPath = fi.FullName;
                    newPd.TextChange = false;
                    Project.Add(newPd.Name, newPd);
                    currentPath = fi.FullName;
                    //projectData = Project.GetProgramData(CurrentProgram);
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        private void FileSaveAsOther()
        {
            if (ProgramNameList.Count == 0)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(GetLocalization["prgNotPrograms"]);
                return;
            }

            try
            {
                var pd = Project.GetProgramData(CurrentProgram);
                string tmpExt = "";
                for (int a = pd.Name.IndexOf('.'); a < pd.Name.Length; a++)
                {
                    tmpExt += pd.Name[a];
                }
                string name = new SaveFile().Save(pd.Editor.TextArea.Text, pd.Name, tmpExt);
                if (name != "")
                {
                    FileInfo fi = new FileInfo(name);
                    string oldName = pd.Name;
                    Project.Remove(oldName);
                    var newPd = new ProgramData() { Item = CurrentProgram };
                    //
                    //newPd.Name = fi.Name.Replace(ext, "");
                    newPd.Name = fi.Name;
                    //
                    newPd.Path = fi.DirectoryName;
                    newPd.FullPath = fi.FullName;
                    newPd.TextChange = false;
                    Project.Add(newPd.Name, newPd);
                    currentPath = fi.FullName;
                    //projectData = Project.GetProgramData(CurrentProgram);
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }

        public void EditCopy()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Copy();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        public void EditPaste()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Paste();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        public void EditCut()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Cut();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        public void EditUndo()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Undo();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        public void EditRedo()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Redo();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditSelectAll()
        {
            highlightAll = true;
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.SelectAll();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditFormat()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Lexer.Format();
                data.Menu.IsOpen = false;
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditShowLine()
        {
            //indent = !indent;
            if (ProgramNameList.Count > 0)
            {
                foreach (var doc in ProgramNameList)
                {
                    var data = Project.GetProgramData(doc);
                    if (data != null)
                        data.Editor.IndentationGuides = ShowLine ? IndentView.LookBoth : IndentView.None;
                }
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditShowNumber()
        {
            if (ProgramNameList.Count > 0)
            {
                foreach (var doc in ProgramNameList)
                {
                    var data = Project.GetProgramData(doc);
                    if (data != null)
                        data.Editor.TextArea.Margins[TextEditor.NUMBER_MARGIN].Width = ShowNumber ? Math.Max(50, 10 * (int)Math.Log10(data.Editor.TextArea.Lines.Count)) : 0;
                }
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditShowSpace()
        {
            if (ProgramNameList.Count > 0)
            {
                foreach (var doc in ProgramNameList)
                {
                    var data = Project.GetProgramData(doc);
                    if (data != null)
                        data.Editor.ViewWhitespace = ShowSpace ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
                }
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditWrap()
        {
            if (ProgramNameList.Count > 0)
            {
                foreach (var doc in ProgramNameList)
                {
                    var data = Project.GetProgramData(doc);
                    if (data != null)
                        data.Editor.WrapMode = ShowWrap ? WrapMode.Whitespace : WrapMode.None;
                }
            }
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void FindText()
        {
            FindNext();
            ButtonsPanel.GetEditPopup.IsOpen = false;
        }
        private void EditComment()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Comment(true);
                data.Menu.IsOpen = false;
            }
        }
        private void EditUnComment()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.Comment(false);
                data.Menu.IsOpen = false;
            }
        }
        private void EditFoldAll()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.FoldAll();
                data.Menu.IsOpen = false;
            }
        }
        private void EditUnFoldAll()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null)
            {
                data.Editor.FoldAll();
                data.Menu.IsOpen = false;
            }
        }
        public void FindNext()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null && TextFind != "")
                data.Editor.searchManager.Find(true, TextFind);
        }
        private void FindPrevious()
        {
            var data = Project.GetProgramData(CurrentProgram);
            if (data != null && TextFind != "")
                data.Editor.searchManager.Find(false, TextFind);
        }
        private void FileCloseAll()
        {
            if (ProgramNameList.Count == 0)
            {
                return;
            }

            try
            {
                int result = 0;
                foreach (var item in ProgramNameList)
                {
                    string name = Project.GetProgramData(item).Name;
                    ProgramNameList = new CloseProgram().Close(ProgramNameList, name, out result);
                    if (result == 0)
                    {
                        Project.Remove(name);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
            ButtonsPanel.GetFilePopup.IsOpen = false;
        }
        private void CloseProgram()
        {
            try
            {
                int result = 0;
                var pnl = ProgramNameList;
                var name = EditorHeader.CloseName;
                ProgramNameList = new CloseProgram().Close(pnl, name, out result);
                if (result == 0)
                {
                    Project.Remove(name);

                    /*
                    if (BpObjects.IntelliData.ContainsKey(name))
                    {
                        BpObjects.IntelliData.Remove(name);
                    }
                    */
                }
                if (ProgramNameList.Count > 0)
                {
                    CurrentProgram = ProgramNameList[0];
                    ProgramNameList[0].Style = (System.Windows.Style)Application.Current.Resources["FirstTabItem"];
                }
                else
                {
                    IntellisenseParser.Data.Clear();
                }
                EditorPanel.GetPopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add(ex.Message);
            }
        }

        

        public static Dictionary<string, string> GetLocalization
        {
            get
            {
                return GUILanguage.Items;
            }
        }

        public static Dictionary<string, string> GetErrors
        {
            get
            {
                return App.Errors;
            }
        }

        private static Visibility _hiddenProgramButton;
        public static Visibility HiddenProgramButton
        {
            get { return _hiddenProgramButton; }
            set
            {
                _hiddenProgramButton = value;
                OnStaticPropertyChanged("HiddenProgramButton");
            }
        }

        #region HomeButton

        private static Visibility _fileNewV;
        public static Visibility FileNewV
        {
            get { return _fileNewV; }
            set
            {
                _fileNewV = value;
                OnStaticPropertyChanged("FileNewV");
                FileDVisibility();
            }
        }

        private static Visibility _fileOpenV;
        public static Visibility FileOpenV
        {
            get { return _fileOpenV; }
            set
            {
                _fileOpenV = value;
                OnStaticPropertyChanged("FileOpenV");
                FileDVisibility();
            }
        }

        private static Visibility _fileCloseAllV;
        public static Visibility FileCloseAllV
        {
            get { return _fileCloseAllV; }
            set
            {
                _fileCloseAllV = value;
                OnStaticPropertyChanged("FileCloseAllV");
                FileDVisibility();
            }
        }

        private static Visibility _fileSaveV;
        public static Visibility FileSaveV
        {
            get { return _fileSaveV; }
            set
            {
                _fileSaveV = value;
                OnStaticPropertyChanged("FileSaveV");
                FileDVisibility();
            }
        }

        private static Visibility _fileSaveAllV;
        public static Visibility FileSaveAllV
        {
            get { return _fileSaveAllV; }
            set
            {
                _fileSaveAllV = value;
                OnStaticPropertyChanged("FileSaveAllV");
                FileDVisibility();
            }
        }

        private static Visibility _fileSaveAsV;
        public static Visibility FileSaveAsV
        {
            get { return _fileSaveAsV; }
            set
            {
                _fileSaveAsV = value;
                OnStaticPropertyChanged("FileSaveAsV");
                FileDVisibility();
            }
        }

        private static Visibility _fileDV;
        public static Visibility FileDV
        {
            get { return _fileDV; }
            set
            {
                _fileDV = value;
                OnStaticPropertyChanged("FileDV");
            }
        }

        private static Visibility _editCopyV;
        public static Visibility EditCopyV
        {
            get { return _editCopyV; }
            set
            {
                _editCopyV = value;
                OnStaticPropertyChanged("EditCopyV");
                EditDV1isibility();
            }
        }

        private static Visibility _editCutV;
        public static Visibility EditCutV
        {
            get { return _editCutV; }
            set
            {
                _editCutV = value;
                OnStaticPropertyChanged("EditCutV");
                EditDV1isibility();
            }
        }

        private static Visibility _editFormatV;
        public static Visibility EditFormatV
        {
            get { return _editFormatV; }
            set
            {
                _editFormatV = value;
                OnStaticPropertyChanged("EditFormatV");
                EditDV2isibility();
            }
        }

        private static Visibility _editPasteV;
        public static Visibility EditPasteV
        {
            get { return _editPasteV; }
            set
            {
                _editPasteV = value;
                OnStaticPropertyChanged("EditPasteV");
                EditDV1isibility();
            }
        }

        private static Visibility _editRedoV;
        public static Visibility EditRedoV
        {
            get { return _editRedoV; }
            set
            {
                _editRedoV = value;
                OnStaticPropertyChanged("EditRedoV");
                EditDV4isibility();
            }
        }

        private static Visibility _editSelectAllV;
        public static Visibility EditSelectAllV
        {
            get { return _editSelectAllV; }
            set
            {
                _editSelectAllV = value;
                OnStaticPropertyChanged("EditSelectAllV");
                EditDV1isibility();
            }
        }

        private static Visibility _editShowLineV;
        public static Visibility EditShowLineV
        {
            get { return _editShowLineV; }
            set
            {
                _editShowLineV = value;
                OnStaticPropertyChanged("EditShowLineV");
                EditDV2isibility();
            }
        }

        private static Visibility _editShowNumberV;
        public static Visibility EditShowNumberV
        {
            get { return _editShowNumberV; }
            set
            {
                _editShowNumberV = value;
                OnStaticPropertyChanged("EditShowNumberV");
                EditDV2isibility();
            }
        }

        private static Visibility _editShowSpaceV;
        public static Visibility EditShowSpaceV
        {
            get { return _editShowSpaceV; }
            set
            {
                _editShowSpaceV = value;
                OnStaticPropertyChanged("EditShowSpaceV");
                EditDV2isibility();
            }
        }

        private static Visibility _editUndoV;
        public static Visibility EditUndoV
        {
            get { return _editUndoV; }
            set
            {
                _editUndoV = value;
                OnStaticPropertyChanged("EditUndoV");
                EditDV4isibility();
            }
        }

        private static Visibility _editWrapV;
        public static Visibility EditWrapV
        {
            get { return _editWrapV; }
            set
            {
                _editWrapV = value;
                OnStaticPropertyChanged("EditWrapV");
                EditDV2isibility();
            }
        }

        private static Visibility _editFindV;
        public static Visibility EditFindV
        {
            get { return _editFindV; }
            set
            {
                _editFindV = value;
                OnStaticPropertyChanged("EditFindV");
                EditDV3isibility();
            }
        }

        private static Visibility _editDV1;
        public static Visibility EditDV1
        {
            get { return _editDV1; }
            set
            {
                _editDV1 = value;
                OnStaticPropertyChanged("EditDV1");
            }
        }

        private static Visibility _editDV2;
        public static Visibility EditDV2
        {
            get { return _editDV2; }
            set
            {
                _editDV2 = value;
                OnStaticPropertyChanged("EditDV2");
            }
        }

        private static Visibility _editDV3;
        public static Visibility EditDV3
        {
            get { return _editDV3; }
            set
            {
                _editDV3 = value;
                OnStaticPropertyChanged("EditDV3");
            }
        }

        private static Visibility _editDV4;
        public static Visibility EditDV4
        {
            get { return _editDV4; }
            set
            {
                _editDV4 = value;
                OnStaticPropertyChanged("EditDV4");
            }
        }

        private static Visibility _compileV;
        public static Visibility CompileV
        {
            get { return _compileV; }
            set
            {
                _compileV = value;
                OnStaticPropertyChanged("CompileV");
            }
        }

        private static Visibility _compileAndRunV;
        public static Visibility CompileAndRunV
        {
            get { return _compileAndRunV; }
            set
            {
                _compileAndRunV = value;
                OnStaticPropertyChanged("CompileAndRunV");
            }
        }

        private static Visibility _connectV;
        public static Visibility ConnectV
        {
            get { return _connectV; }
            set
            {
                _connectV = value;
                OnStaticPropertyChanged("ConnectV");
            }
        }

        private static Visibility _downloadV;
        public static Visibility DownloadV
        {
            get { return _downloadV; }
            set
            {
                _downloadV = value;
                OnStaticPropertyChanged("DownloadV");
            }
        }

        private static Visibility _stopV;
        public static Visibility StopV
        {
            get { return _stopV; }
            set
            {
                _stopV = value;
                OnStaticPropertyChanged("StopV");
            }
        }

        private static void FileDVisibility()
        {
            if (FileNewV != Visibility.Visible && FileOpenV != Visibility.Visible && FileCloseAllV != Visibility.Visible && FileSaveV != Visibility.Visible && FileSaveAllV != Visibility.Visible && FileSaveAsV != Visibility.Visible)
            {
                FileDV = Visibility.Collapsed;
            }
            else
            {
                FileDV = Visibility.Visible;
            }
        }

        private static void EditDV1isibility()
        {
            if (_editSelectAllV != Visibility.Visible && EditCopyV != Visibility.Visible && EditPasteV != Visibility.Visible && EditCutV != Visibility.Visible)
            {
                EditDV1 = Visibility.Collapsed;
            }
            else
            {
                EditDV1 = Visibility.Visible;
            }
        }

        private static void EditDV2isibility()
        {
            if (EditShowLineV != Visibility.Visible && EditShowNumberV != Visibility.Visible && EditShowSpaceV != Visibility.Visible && EditWrapV != Visibility.Visible && EditFormatV != Visibility.Visible)
            {
                EditDV2 = Visibility.Collapsed;
            }
            else
            {
                EditDV2 = Visibility.Visible;
            }
        }

        private static void EditDV3isibility()
        {
            if (EditFindV != Visibility.Visible)
            {
                EditDV3 = Visibility.Collapsed;
            }
            else
            {
                EditDV3 = Visibility.Visible;
            }
        }

        private static void EditDV4isibility()
        {
            if (EditUndoV != Visibility.Visible && EditRedoV != Visibility.Visible)
            {
                EditDV4 = Visibility.Collapsed;
            }
            else
            {
                EditDV4 = Visibility.Visible;
            }
        }

        #endregion

        private void EditShowLineMenu()
        {
            ShowLine = !ShowLine;
            EditShowLine();
        }
        private void EditShowNumberMenu()
        {
            ShowNumber = !ShowNumber;
            EditShowNumber();
        }
        private void EditShowSpaceMenu()
        {
            ShowSpace = !ShowSpace;
            EditShowSpace();
        }
        private void EditWrapMenu()
        {
            ShowWrap = !ShowWrap;
            EditWrap();
        }

        private static void SetProgramName()
        {
            var list = new StackPanel();
            var scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            for (int i = 0; i < ProgramNameList.Count; i++)
            {
                TabItem pnl = ProgramNameList[i];
                if (pnl.Visibility == Visibility.Visible)
                {
                    continue;
                }
                var item = new ProgramNamePanel();
                var cbi = Project.GetProgramData(pnl).Name;
                item.HeaderName = cbi;
                item.GetTextBlock.PreviewMouseLeftButtonDown += Cbi_PreviewMouseLeftButtonDown;

                if (i == 0)
                {
                    item.GetBorder.BorderThickness = new Thickness(0, 0, 0, 0.5);
                }
                else if (i == ProgramNameList.Count - 1)
                {
                    item.GetBorder.BorderThickness = new Thickness(0, 0.5, 0, 0);
                }

                if (ProgramNameList.Count == 1)
                {
                    item.GetBorder.BorderThickness = new Thickness(0, 0, 0, 0);
                }

                list.Children.Add(item);
            }
            scroll.Content = list;

            if (ProgramNameList.Count > 20)
            {
                EditorPanel.GetPopup.Width = 200;
                EditorPanel.GetPopup.HorizontalOffset = -200;
            }
            else
            {
                EditorPanel.GetPopup.Width = 180;
                EditorPanel.GetPopup.HorizontalOffset = -180;
            }

            EditorPanel.GetPopup.Child = scroll;
            EditorPanel.GetPopup.IsOpen = true;
        }

        private static void Cbi_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ProgramNameList.Count < 2)
                return;

            var tb = (TextBlock)sender;
            string name = tb.Text;
            int iN = ProgramNameList.IndexOf(Project.GetDictionary()[name].Item);

            var tiN = ProgramNameList[iN];
            var ti0 = ProgramNameList[0];

            var ti0v = ti0.Visibility;
            var tiNv = tiN.Visibility;

            ti0.Visibility = tiNv;
            tiN.Visibility = ti0v;

            var pnl = new ObservableCollection<TabItem>();
            tiN.Style = (System.Windows.Style)Application.Current.Resources["FirstTabItem"];

            pnl.Add(tiN);
            for (int item = 0; item < ProgramNameList.Count; item++)
            {
                TabItem i;
                if (item != iN)
                {
                    i = ProgramNameList[item];
                    i.Style = (System.Windows.Style)Application.Current.Resources["NextTabItem"];
                    pnl.Add(i);
                }
            }

            ProgramNameList = pnl;
            ProgramNameList[0].IsSelected = true;
            CurrentProgram = ProgramNameList[0];
            EditorPanel.GetPopup.IsOpen = false;
        }

        private void About()
        {
            var about = new View.Dialogs.About();
            about.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static void OnStaticPropertyChanged(string propertyName)
        {
            var handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
