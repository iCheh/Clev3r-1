using Clever.Brick;
using Clever.Brick.Communication;
using Clever.Brick.Utils;
using Clever.CommonData;
using Clever.Model.Utils;
using Clever.View.Controls.Buttons.BrickButtons;
using Clever.View.Controls.Buttons.CompileButtons;
using Clever.View.Controls.Buttons.HomeButtons;
using Clever.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Clever.View.Controls.Helps
{
    /// <summary>
    /// Логика взаимодействия для EV3Brick.xaml
    /// </summary>
    public partial class EV3Brick : UserControl
    {
        EV3Connection connection;
        static EV3Brick _ev3Brick;
        static Popup _menuPopup;
        public static string ProjectFolder { get; set; }
        public static string ProjectPath { get; set; }

        double width = 0;
        double w1 = 0;
        double w2 = 0;
        double w3 = 0;
        string ev3path;
        byte[] ev3fileB = null;
        string ev3fileS = "";
        string ev3fileName = "";
        string downloadPath = "";

        public EV3Brick()
        {
            InitializeComponent();
            ProjectFolder = "";

            // initialize common data
            ev3path = "/home/root/lms2012/prjs/";

            EV3Path.Text = ev3path;
            connection = null;

            width = 390;
            GetNewSize();
            AdjustDisabledStates();
            _ev3Brick = this;
            _menuPopup = menuPopup;
        }

        private void DeviceName_focuslost(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                return;
            }

            if (EV3DeviceName.Text.Length > 0)
            {
                try
                {
                    SetEV3DeviceName(EV3DeviceName.Text);
                    ReadEV3DeviceName();
                }
                catch (Exception ex)
                {
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    Reconnect();
                }
            }
        }

        private void DeviceName_keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                EV3Directory.Focus();
            }
        }

        private void EV3Directory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DirectoryEntry de = (DirectoryEntry)EV3Directory.SelectedItem;
                if (de != null && de.IsDirectory)
                {
                    String newpath = ev3path + de.FileName + "/";
                    // prevent to navigate into folder that would lock up the brick
                    if (!newpath.Equals("/proc/"))
                    {
                        ev3path = newpath;
                        EV3Path.Text = newpath;
                        ReadEV3Directory(true);
                    }
                }
                AdjustDisabledStates();
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Status.Add(MainWindowVM.GetLocalization["brkRetryConnect"]);
                Reconnect();
            }
        }

        void Reconnect()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
            MainWindowVM.BrickConnect = false;

            EV3Directory.Visibility = Visibility.Hidden;
            BrickNotFound.Visibility = Visibility.Visible;

            ev3path = "/home/root/lms2012/prjs/";
            EV3Path.Text = ev3path;
            EV3Directory.Items.Clear();

            // find connected brick
            try
            {
                //connection = ConnectionFinder.CreateConnection(true, false);
                connection = ConnectionFinder.CreateConnection(true, true);
                ReadEV3Directory(true);
                ReadEV3DeviceName();
                MainWindowVM.BrickConnect = true;
            }
            catch (Exception)
            {
                //MessageBox.Show("Блок EV3 не найден");
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkNotFound"]);
                MainWindowVM.BrickConnect = false;
            }

            EV3Directory.Visibility = Visibility.Visible;
            BrickNotFound.Visibility = Visibility.Hidden;

            AdjustDisabledStates();
        }

        private void AdjustDisabledStates()
        {
            if (connection != null && connection.IsOpen())
            {
                DirectoryEntry de = (DirectoryEntry)EV3Directory.SelectedItem;
                //EV3Directory.IsEnabled = true;
                CompileAndRun.IsEnabled = true;
                EV3NavigateUp.IsEnabled = !ev3path.Equals("/");
                EV3NavigateUp2.IsEnabled = !ev3path.Equals("/");
                BrickNotFound.Visibility = Visibility.Hidden;
                DeleteFile.IsEnabled = (de != null && !de.IsDirectory) || (EV3Directory.Items.Count == 0 && ev3path.Length > 1);
                NewFolder.IsEnabled = true;
                Upload.IsEnabled = de != null && !de.IsDirectory;
                Preview.IsEnabled = de != null && !de.IsDirectory;
                Download.IsEnabled = true;
                RunFile.IsEnabled = de != null && de.IsRunable;
                EV3DeviceName.IsReadOnly = false;
                EV3NavigateUp.Visibility = Visibility.Visible;
                EV3Path.Visibility = Visibility.Visible;
                EV3DeviceName.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                StopPrg.IsEnabled = true;

                ButtonCompileAndRun.GetButton.IsEnabled = CompileAndRun.IsEnabled;
                ButtonDownload.GetButton.IsEnabled = Download.IsEnabled;
                ButtonStop.GetButton.IsEnabled = StopPrg.IsEnabled;
                hButtonCompileAndRun.GetButton.IsEnabled = CompileAndRun.IsEnabled;
                hButtonDownload.GetButton.IsEnabled = Download.IsEnabled;
                hButtonStop.GetButton.IsEnabled = StopPrg.IsEnabled;
            }
            else
            {
                //EV3Directory.IsEnabled = false;
                CompileAndRun.IsEnabled = false;
                EV3NavigateUp.IsEnabled = false;
                EV3NavigateUp2.IsEnabled = false;
                BrickNotFound.Visibility = Visibility.Visible;
                DeleteFile.IsEnabled = false;
                NewFolder.IsEnabled = false;
                Upload.IsEnabled = false;
                RunFile.IsEnabled = false;
                Preview.IsEnabled = false;
                Download.IsEnabled = false;
                Compile.IsEnabled = true;
                EV3SwitchDevice.IsEnabled = true;
                EV3DeviceName.IsReadOnly = true;
                EV3NavigateUp.Visibility = Visibility.Hidden;
                EV3Path.Visibility = Visibility.Hidden;
                EV3DeviceName.Text = MainWindowVM.GetLocalization["tBrkNotFound"];
                EV3DeviceName.Foreground = new SolidColorBrush(Color.FromRgb(0xB3, 0xB3, 0xB3));
                StopPrg.IsEnabled = false;

                ButtonCompileAndRun.GetButton.IsEnabled = CompileAndRun.IsEnabled;
                ButtonDownload.GetButton.IsEnabled = Download.IsEnabled;
                ButtonStop.GetButton.IsEnabled = StopPrg.IsEnabled;
                hButtonCompileAndRun.GetButton.IsEnabled = CompileAndRun.IsEnabled;
                hButtonDownload.GetButton.IsEnabled = Download.IsEnabled;
                hButtonStop.GetButton.IsEnabled = StopPrg.IsEnabled;
            }
        }

        public void EV3SwitchDevice_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Status.Clear();

            if (connection != null && connection.IsOpen())
            {
                connection.Close();
                connection = null;
                AdjustDisabledStates();
                MainWindowVM.BrickConnect = false;
                Status.Add(MainWindowVM.GetLocalization["brkOff"]);
                EV3Directory.Items.Clear();
                return;
            }
            // find connected brick
            try
            {
                Status.Add(MainWindowVM.GetLocalization["brkConnect"]);
                connection = ConnectionFinder.CreateConnection(true, true);
                ReadEV3Directory(true);
                ReadEV3DeviceName();
                AdjustDisabledStates();
                MainWindowVM.BrickConnect = true;
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                MainWindowVM.BrickConnect = false;
            }
            Status.Clear();
            if (connection != null)
                Status.Add(MainWindowVM.GetLocalization["brkConnectYes"]);
            else
                Status.Add(MainWindowVM.GetLocalization["brkConnectNo"]);

            ButtonsPanel.GetBrickPopup.IsOpen = false;
        }

        private void DeleteFile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            // check if the explorer sees an empty directory - will delete this instead of files
            if (EV3Directory.Items.Count == 0 && ev3path.Length > 1)  // can only remove empty directories but not the topmost element
            {
                try
                {
                    DeleteCurrentEV3Directory();

                    int idx = ev3path.LastIndexOf('/', ev3path.Length - 2);
                    if (idx >= 0)
                    {
                        ev3path = ev3path.Substring(0, idx + 1);
                        EV3Path.Text = ev3path;
                    }
                    ReadEV3Directory(true);
                    AdjustDisabledStates();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    Reconnect();
                }
                return;
            }


            // determine which things to delete
            List<DirectoryEntry> del = new List<DirectoryEntry>();
            foreach (DirectoryEntry de in EV3Directory.SelectedItems)
            {
                if (de != null && !de.IsDirectory)
                {
                    del.Add(de);
                }
            }
            if (del.Count > 0)
            {
                try
                {
                    foreach (DirectoryEntry de in del)
                    {
                        DeleteEV3File(de.FileName);
                    }
                    ReadEV3Directory(false);
                    AdjustDisabledStates();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    Reconnect();
                }
            }
        }

        private void NewFolder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            QuestionBox qb = new QuestionBox(MainWindowVM.GetLocalization["dNameNewFolder"], "");
            if (qb.ShowDialog() == true)
            {
                string dirname = qb.Answer;

                if (!checkFileName(dirname))
                {
                    return;
                }

                try
                {
                    CreateEV3Directory(dirname);
                    ReadEV3Directory(false);
                    AdjustDisabledStates();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    Reconnect();
                }
            }
        }

        private void Upload_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            // determine which things to upload
            List<DirectoryEntry> tr = new List<DirectoryEntry>();
            foreach (DirectoryEntry de in EV3Directory.SelectedItems)
            {
                if (de != null && !de.IsDirectory)
                {
                    tr.Add(de);
                }
            }
            if (tr.Count > 0)
            {
                try
                {
                    foreach (DirectoryEntry de in tr)
                    {
                        byte[] data = null;
                        data = connection.ReadEV3File(internalPath(ev3path) + de.FileName);

                        if (data != null)
                        {
                            // Выбор места для сохранения
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.FileName = de.FileName;
                            if (sfd.ShowDialog() == true)
                            {
                                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                                fs.Write(data, 0, data.Length);
                                fs.Close();
                                //View.GetEdtFileContents().Text = File.ReadAllText(sfd.FileName);
                                //tabControlTools.SelectedItem = tabFileContents;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    Reconnect();
                }
            }


        }

        public void Compile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CompileAndDownload(false);
            ButtonsPanel.GetBrickPopup.IsOpen = false;
        }

        public void CompileAndRun_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                ButtonsPanel.GetBrickPopup.IsOpen = false;
                AdjustDisabledStates();
                return;
            }

            try
            {
                if (EV3ProgramRun())
                {
                    ByteCodeBuffer c = new ByteCodeBuffer();

                    c.OP(0x02);       // opPROGRAM_STOP
                    c.CONST(1);       // slot 1 = user program slot
                    connection.DirectCommand(c, 0, 0);
                }

                CompileAndDownload(true);

            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }

            ButtonsPanel.GetBrickPopup.IsOpen = false;
        }

        private void RunFile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                if (EV3ProgramRun())
                {
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkIsStartPrg"]);
                    return;
                }

                DirectoryEntry de = (DirectoryEntry)EV3Directory.SelectedItem;
                if (de != null && de.IsRunable)
                {
                    RunEV3File(de.FileName, ev3path);
                }
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private bool EV3ProgramRun()
        {
            //===
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return false;
            }
            //===

            bool start = false;

            ByteCodeBuffer c = new ByteCodeBuffer();

            c.OP(0x0C);       // opPROGRAM_INFO
            c.CONST(0x18);    // GET_PRGRESULT
            c.CONST(1);       // slot 1 = user program slot
            c.GLOBVAR(0);
            byte[] result = connection.DirectCommand(c, 1, 0);

            if (result == null || result.Length < 1 || result[0] < 0)
            {
                start = false;
            }
            else
            {
                if (result[0] == 1)
                    start = true;
            }

            return start;
        }

        public void BrickStop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                ButtonsPanel.GetBrickPopup.IsOpen = false;
                AdjustDisabledStates();
                return;
            }

            try
            {
                if (!EV3ProgramRun())
                {
                    ButtonsPanel.GetBrickPopup.IsOpen = false;
                    return;
                }

                ByteCodeBuffer c = new ByteCodeBuffer();

                c.OP(0x02);       // opPROGRAM_STOP
                c.CONST(1);       // slot 1 = user program slot
                connection.DirectCommand(c, 0, 0);

                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkStopPrg"]);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }

            ButtonsPanel.GetBrickPopup.IsOpen = false;
        }

        /*
        private bool GetSDCard()
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return false;
            }
            bool res = false;

            ByteCodeBuffer c = new ByteCodeBuffer();
            c.OP(0x81);       // opUI_READ = 0x81
            c.CONST(0x1D);    // CMD: GET_SDCARD = 0x1D
            c.GLOBVAR(4);
            c.GLOBVAR(4);
            c.GLOBVAR(0);
            byte[] byteRes = connection.DirectCommand(c, 9, 0);

            if (byteRes == null)
            {
                //MessageBox.Show("1");
            }
            else
            {
                //MessageBox.Show("2");
            }


            ///var bbb = BitConverter.ToBoolean(byteRes, 0);
            return res;
        }
        */

        private void ReadEV3Directory(bool resetposition)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                MemoryStream data = new MemoryStream();

                //            if (!ev3path.Equals("/proc/"))       // avoid locking up the brick
                {
                    // get data from brick
                    BinaryBuffer b = new BinaryBuffer();
                    b.Append16(500);  // expect max 500 bytes per packet
                    b.AppendZeroTerminated(internalPath(ev3path));
                    byte[] response = connection.SystemCommand(EV3Connection.LIST_FILES, b);

                    if (response == null)
                    {
                        throw new Exception("No response to LIST_FILES");
                    }
                    if (response.Length < 6)
                    {
                        throw new Exception("Response too short for LIST_FILES");
                    }
                    if (response[0] != EV3Connection.SUCCESS && response[0] != EV3Connection.END_OF_FILE)
                    {
                        throw new Exception("Unexpected status at LIST_FILES: " + response[0]);
                    }
                    int handle = response[5] & 0xff;
                    data.Write(response, 6, response.Length - 6);

                    // continue reading until have total buffer
                    while (response[0] != EV3Connection.END_OF_FILE)
                    {
                        b.Clear();
                        b.Append8(handle);
                        b.Append16(500);  // expect max 500 bytes per packet
                        response = connection.SystemCommand(EV3Connection.CONTINUE_LIST_FILES, b);
                        //                    Console.WriteLine("follow-up response length: " + response.Length);

                        if (response == null)
                        {
                            throw new Exception("No response to CONTINUE_LIST_FILES");
                        }
                        if (response.Length < 2)
                        {
                            throw new Exception("Too short response to CONTINUE_LIST_FILES");
                        }
                        if (response[0] != EV3Connection.SUCCESS && response[0] != EV3Connection.END_OF_FILE)
                        {
                            throw new Exception("Unexpected status at CONTINUE_LIST_FILES: " + response[0]);
                        }
                        //                    Console.WriteLine("subsequent response length: " + response.Length);
                        data.Write(response, 2, response.Length - 2);
                    }
                }

                List<DirectoryEntry> list = new List<DirectoryEntry>();

                data.Position = 0;  // start reading at beginning
                StreamReader tr = new StreamReader(data, Encoding.GetEncoding("iso-8859-1"));
                string l;
                while ((l = tr.ReadLine()) != null)
                {
                    if (l.EndsWith("/"))
                    {
                        string n = l.Substring(0, l.Length - 1);
                        if ((!n.Equals(".")) && (!n.Equals("..")))
                        {
                            list.Add(new DirectoryEntry(n, 0, true));
                        }
                    }
                    else
                    {
                        int firstspace = l.IndexOf(' ');
                        if (firstspace < 0)
                        {
                            continue;
                        }
                        int secondspace = l.IndexOf(' ', firstspace + 1);
                        if (secondspace < 0)
                        {
                            continue;
                        }
                        int size = int.Parse(l.Substring(firstspace, secondspace - firstspace).Trim(), System.Globalization.NumberStyles.HexNumber);

                        list.Add(new DirectoryEntry(l.Substring(secondspace + 1), size, false));
                    }
                }

                // sort list
                list.Sort((x, y) => x.FileName.CompareTo(y.FileName));

                // put data into listview
                EV3Directory.Items.Clear();
                foreach (DirectoryEntry de in list)
                {
                    EV3Directory.Items.Add(de);
                }

                // let the WPF system re-calculate all column widths so everthing fits as good as possible

                foreach (var gvc in EV3DirectoryGridView.Columns)
                {

                    //gvc.Width = gvc.ActualWidth;
                    //gvc.Width = Double.NaN;
                    width = grid.DesiredSize.Width;
                    GetNewSize();
                }


                // move the controls scroller to top position
                if (resetposition)
                {
                    if (EV3Directory.Items.Count > 0)
                    {
                        EV3Directory.ScrollIntoView(EV3Directory.Items[0]);

                        //Status.Add(list[0].FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void ReadEV3DeviceName()
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                ByteCodeBuffer c = new ByteCodeBuffer();
                c.OP(0xD3);         // Com_Get
                c.CONST(0x0D);      // GET_BRICKNAME = 0x0D
                c.CONST(127);       // maximum string length
                c.GLOBVAR(0);       // where to store name
                byte[] response = connection.DirectCommand(c, 128, 0);

                string devicename = "?";
                if (response != null && response.Length > 1)
                {
                    // find the null-termination
                    for (int len = 0; len < response.Length; len++)
                    {
                        if (response[len] == 0)
                        {
                            // extract the message text
                            char[] msg = new char[len];
                            for (int i = 0; i < len; i++)
                            {
                                msg[i] = (char)response[i];
                            }
                            devicename = new string(msg);
                            break;
                        }
                    }
                }

                EV3DeviceName.Text = devicename;
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }

        }

        private void SetEV3DeviceName(string name)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                ByteCodeBuffer c = new ByteCodeBuffer();
                c.OP(0xD4);         // Com_Set
                c.CONST(0x08);      // SET_BRICKNAME = 0x08
                c.STRING(name);
                connection.DirectCommand(c, 0, 0);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void DeleteEV3File(string filename)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                BinaryBuffer b = new BinaryBuffer();
                b.AppendZeroTerminated(internalPath(ev3path) + filename);
                connection.SystemCommand(EV3Connection.DELETE_FILE, b);
                EV3RefreshList();
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void DeleteCurrentEV3Directory()
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                BinaryBuffer b = new BinaryBuffer();
                b.AppendZeroTerminated(internalPath(ev3path));
                b.AppendZeroTerminated(ev3path);
                connection.SystemCommand(EV3Connection.DELETE_FILE, b);
                //EV3RefreshList();
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void CreateEV3Directory(string directoryname)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                BinaryBuffer b = new BinaryBuffer();
                b.AppendZeroTerminated(internalPath(ev3path) + directoryname);

                connection.SystemCommand(EV3Connection.CREATE_DIR, b);
                EV3RefreshList();
            }
            catch (Exception ex)
            {
                //Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void RunEV3File(string filename, string path)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }

            try
            {
                string fullname = internalPath(path) + filename;
                //Console.WriteLine("Trying to start: " + fullname);
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkStartPrg"] + " " + fullname);
                ByteCodeBuffer c = new ByteCodeBuffer();

                // load and start it
                c.OP(0xC0);       // opFILE
                c.CONST(0x08);    // CMD: LOAD_IMAGE = 0x08
                c.CONST(1);       // slot 1 = user program slot
                c.STRING(fullname);
                c.GLOBVAR(0);
                c.GLOBVAR(4);
                c.OP(0x03);       // opPROGRAM_START
                c.CONST(1);       // slot 1 = user program slot
                c.GLOBVAR(0);
                c.GLOBVAR(4);
                c.CONST(0);

                connection.DirectCommand(c, 10, 0);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private bool checkFileName(string filename)
        {
            return new ValidName().Valid(filename);
        }

        private string internalPath(string absolutePath)
        {
            if (absolutePath.StartsWith("/home/root/lms2012/"))
            {
                return ".." + absolutePath.Substring(18);
            }
            else
            {
                return "/." + absolutePath;
            }

        }

        private void EV3NavigateUp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                Reconnect();
                return;
            }

            try
            {
                if (ev3path.Length > 1)
                {
                    int idx = ev3path.LastIndexOf('/', ev3path.Length - 2);
                    if (idx >= 0)
                    {
                        ev3path = ev3path.Substring(0, idx + 1);
                        EV3Path.Text = ev3path;

                        ReadEV3Directory(true);
                    }
                    AdjustDisabledStates();
                }
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void BrickRefresh_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Reconnect();
        }

        public void EV3RefreshList()
        {
            try
            {
                ReadEV3Directory(false);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Status.Add(MainWindowVM.GetLocalization["brkConnectRepeat"]);
                Reconnect();
            }
            if (menuPopup.IsOpen)
                menuPopup.IsOpen = false;
        }

        private void CompileAndDownload(bool run)
        {
            ProjectFolder = "";
            ProjectPath = "";
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

            Status.Clear();
            if (MainWindowVM.Project.Count == 0)
            {
                Status.Add(MainWindowVM.GetLocalization["brkFileNotFound"]);
                return;
            }

            var programs = MainWindowVM.ProgramNameList;
            var listCur = new List<ProgramData>();
            var listPrograms = new List<string>();
            var current = MainWindowVM.Project.GetProgramData(MainWindowVM.CurrentProgram);
            //Status.Add("===> " + programs.Count.ToString());

            foreach (var cur in programs)
            {
                var curPrg = MainWindowVM.Project.GetProgramData(cur);
                string tmpExt = "";
                for (int a = curPrg.Name.IndexOf('.'); a < curPrg.Name.Length; a++)
                {
                    tmpExt += curPrg.Name[a];
                }

                //Status.Add("===> " + curPrg.Name.Replace(tmpExt, ""));

                if (tmpExt == ".bp")
                {
                    listCur.Add(curPrg);
                    //Status.Add("===> " + curPrg.Name.Replace(tmpExt, ""));
                }

                if (curPrg.FullPath.Length == 0)
                {
                    curPrg.FullPath = new Model.Program.SaveFile().Save(curPrg.Editor.TextArea.Text, curPrg.Name, tmpExt);
                    if (curPrg.FullPath == "")
                    {
                        Status.Add(MainWindowVM.GetLocalization["brkFileNotSave"]);
                        return;
                    }
                    curPrg.TextChange = false;
                }
                else
                {
                    new Model.Program.SaveFile().Save(curPrg);
                    curPrg.TextChange = false;
                }

                
            }
            
            if (listCur.Count == 0)
            {
                Status.Add(MainWindowVM.GetLocalization["bpCompileNotFound"]);
                return;
            }
            else if (listCur.Count > 1)
            {
                if (current.Name.IndexOf(".bpm") != -1 || current.Name.IndexOf(".bpi") != -1)
                {
                    Status.Add(MainWindowVM.GetLocalization["bpCompileNotSelect"]);
                    return;
                }
            }
            else
            {
                current = listCur[0];
            }

            FileInfo pcfile = new FileInfo(current.FullPath);
            

            byte[] content = null;
            string targetfilename = null;

            if (pcfile.Name.EndsWith(MainWindowVM.ext, StringComparison.InvariantCultureIgnoreCase))
            {
                targetfilename = pcfile.Name.Substring(0, pcfile.Name.LastIndexOf('.')) + ".rbf";

                if (!checkFileName(targetfilename.Replace(".rbf", "")))
                {
                    return;
                }

                List<string> errors = new List<string>();

                try
                {
                    // Вставка препроцессора

                    //=============================
                    //BPInterpreter.Builder builder = new BPInterpreter.Builder();
                    Interpreter.Builder builder = new Interpreter.Builder(Configurations.Get.Language);

                    //=============================

                    Status.Add(MainWindowVM.GetLocalization["brkPreStart"]);
                    var tmpText = new Reader().ReadFile(pcfile.FullName);
                    string tmpPath = Application.ResourceAssembly.Location.Replace("Clever.exe", "");
                    string tmpLoc = Configurations.Get.Language;
                    if (tmpText == null)
                    {
                        return;
                    }
                    else if (tmpText.Length == 0)
                    {
                        return;
                    }

                    var streamI = new MemoryStream();
                    errors.Clear();

                    builder.StartInterpreter(tmpText, GetPath(pcfile.FullName), GetName(pcfile.FullName), tmpPath, ".bp", ".bpi", ".bpm", "~", tmpLoc, errors, streamI);
                    if (errors.Count > 0)
                    {
                        Status.Add(MainWindowVM.GetLocalization["brkFindErr"] + " " + errors.Count);

                        foreach (string s in errors)
                        {
                            Status.Add(s);
                            int startName = s.IndexOf("file:") + 5;
                            int endName = s.IndexOf("line:") - 1;
                            ProjectPath = s.Substring(startName, endName - startName).Trim();
                        }
                        return;
                    }

                    if (builder.GetIsFolder())
                    {
                        Keys.FolderKey = builder.GetFolderName().Trim().Replace("\"", "").ToUpper();
                        Keys.FolderName = builder.GetProjectName().Trim().Replace("\"", "");

                        ProjectFolder = Keys.FolderName;

                        Keys.ImageList = builder.GetImageList();
                        Keys.ImagePath = builder.GetImagePath();
                        Keys.ImageFullName = builder.GetImageFullName();

                        Keys.SoundList = builder.GetSoundList();
                        Keys.SoundPath = builder.GetSoundPath();
                        Keys.SoundFullName = builder.GetSoundFullName();

                        Keys.FileList = builder.GetFileList();
                        Keys.FilePath = builder.GetFilePath();
                        Keys.FileFullName = builder.GetFileFullName();

                        // Запишем файл ключей
                        new WriteKeys().Write(GetPath(pcfile.FullName), GetName(pcfile.FullName));
                        // ===========================================
                    }
                    else
                    {
                        Keys.FolderName = "";
                    }

                    // Список всех программ проекта
                    listPrograms = builder.GetPrograms();

                    // Проверим, что собранный файл ~... .bp открыт и его надо изменить
                    MainWindowVM.FindAndReplaceFile(GetName(pcfile.FullName), GetPath(pcfile.FullName));
                    // ===========================================

                    Status.Items[Status.Count - 1] = MainWindowVM.GetLocalization["brkPreStart"] + MainWindowVM.GetLocalization["brkDone"];
                    Status.Add(MainWindowVM.GetLocalization["brkCompileStart"]);
                    var streamC = new MemoryStream();
                    errors.Clear();
                    builder.StartCompiler(streamI, streamC, errors);
                    //MessageBox.Show(streamC.Length.ToString());
                    if (errors.Count == 0)
                    {
                        Status.Items[Status.Count - 1] = MainWindowVM.GetLocalization["brkCompileStart"] + MainWindowVM.GetLocalization["brkDone"];
                        Status.Add(MainWindowVM.GetLocalization["brkAssembStart"]);

                        var streamA = new MemoryStream();
                        errors.Clear();
                        builder.StartAssembler(streamC, streamA, errors);
                        if (errors.Count == 0)
                        {
                            content = streamA.ToArray();
                            Status.Items[Status.Count - 1] = MainWindowVM.GetLocalization["brkAssembStart"] + MainWindowVM.GetLocalization["brkDone"];
                            Status.Update();
                        }
                        else
                        {
                            Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + errors.Count);
                            foreach (string s in errors)
                            {
                                Status.Add(s);
                            }
                        }
                    }
                    else
                    {
                        Status.Add(MainWindowVM.GetLocalization["brkFindErr"] + " " + errors.Count);

                        //var ln = preprocessor.GetFilesTree;
                        foreach (string s in errors)
                        {
                            Status.Add(s);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                }
            }

            if (connection != null && connection.IsOpen())
            {
                //Status.Add(internalPath(ev3path) + targetfilename + " 0");
                EV3RefreshList();
                if (content != null)
                {
                    try
                    {
                        string newPath = "";
                        string pathForRun = "";

                        if (Keys.FolderName != "")
                        {
                            if (Keys.FolderKey == "SD")
                            {
                                if (!IsSDCard())
                                {
                                    Status.Add("SD Card not found");
                                    return;
                                }
                                else
                                {
                                    newPath = "/home/root/lms2012/prjs/SD_Card/" + Keys.FolderName;
                                }
                            }
                            else if (Keys.FolderKey == "PRJS")
                            {
                                newPath = "/home/root/lms2012/prjs/" + Keys.FolderName;
                            }

                            BinaryBuffer b = new BinaryBuffer();
                            b.AppendZeroTerminated(internalPath(newPath));
                            connection.SystemCommand(EV3Connection.CREATE_DIR, b);
                            connection.CreateEV3File(internalPath(newPath + "/") + targetfilename, content);

                            if (Keys.ImageList.Count != 0 || Keys.SoundList.Count != 0)
                            {
                                BinaryBuffer m = new BinaryBuffer();
                                m.AppendZeroTerminated(internalPath(newPath + "/Media/"));
                                connection.SystemCommand(EV3Connection.CREATE_DIR, m);

                                if (Keys.SoundList.Count != 0)
                                {
                                    foreach (var f in Keys.SoundList)
                                    {
                                        DownloadMedia(f, newPath + "/Media/", "Sounds\\rsf");
                                    }
                                }
                                if (Keys.ImageList.Count != 0)
                                {
                                    foreach (var f in Keys.ImageList)
                                    {
                                        DownloadMedia(f, newPath + "/Media/", "Images\\rgf");
                                    }
                                }
                            }
                            if (Keys.FileList.Count != 0)
                            {
                                BinaryBuffer f = new BinaryBuffer();
                                f.AppendZeroTerminated(internalPath(newPath + "/Files/"));
                                connection.SystemCommand(EV3Connection.CREATE_DIR, f);
                            }
                            ReadEV3Directory(false);
                            AdjustDisabledStates();

                            pathForRun = newPath + "/";
                            
                        }
                        else
                        {
                            connection.CreateEV3File(internalPath(ev3path) + targetfilename, content);
                            //Status.Add(internalPath(ev3path) + targetfilename);
                            ReadEV3Directory(false);
                            AdjustDisabledStates();
                            pathForRun = ev3path;
                        }

                        // Попытка копирования файлов программы в блок
                        BinaryBuffer sourceBF = new BinaryBuffer();
                        sourceBF.AppendZeroTerminated(internalPath(pathForRun + "/Source/"));
                        connection.SystemCommand(EV3Connection.CREATE_DIR, sourceBF);
                        DownloadSourceFiles(listPrograms, pathForRun + "/Source/");

                        // Попытка запуска программы
                        if (run)
                        {
                            RunEV3File(targetfilename, pathForRun);
                        }

                    }
                    catch (Exception ex)
                    {
                        Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                        Reconnect();
                    }
                }
                else
                {
                    Status.Add("byte code = 0");
                }
            }
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            width = grid.DesiredSize.Width;
            GetNewSize();
        }

        private void GetNewSize()
        {
            w1 = (width) * 0.6;
            w2 = (width) * 0.15;
            w3 = (width) * 0.25;

            if (w1 < 0)
                w1 = 0;
            if (w2 < 0)
                w2 = 0;
            if (w3 < 0)
                w3 = 0;

            c1.Width = w1;
            c2.Width = w2;
            c3.Width = w3;
        }

        private void Preview_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<DirectoryEntry> tr = new List<DirectoryEntry>();
            foreach (DirectoryEntry de in EV3Directory.SelectedItems)
            {
                if (de != null && !de.IsDirectory)
                {
                    tr.Add(de);
                }
            }
            if (tr.Count > 0)
            {
                try
                {
                    foreach (DirectoryEntry de in tr)
                    {
                        ev3fileB = null;
                        ev3fileB = connection.ReadEV3File(internalPath(ev3path) + de.FileName);

                        if (de.FileName.IndexOf(".rtf") == -1 && de.FileName.IndexOf(".txt") == -1 && de.FileName.IndexOf(".rcf") == -1 && de.FileName.IndexOf(".bp") == -1 && de.FileName.IndexOf(".bpm") == -1 && de.FileName.IndexOf(".bpi") == -1)
                        {
                            Status.Clear();
                            Status.Add(MainWindowVM.GetLocalization["brkViewFileExt"]);
                            return;
                        }

                        Status.Clear();

                        if (ev3fileB != null)
                        {
                            ev3fileName = de.FileName;
                            ev3fileS = "";
                            foreach (var i in ev3fileB)
                            {
                                ev3fileS += Convert.ToChar(i);
                            }

                            BrickUpload2.ev3fileName = ev3fileName;
                            BrickUpload2.ev3fileB = ev3fileB;
                            View.GetEditFileContents().Text = ev3fileS;
                            HelpPanel.GetTabControlHelps().SelectedItem = HelpPanel.GetTabItemView();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Status.Clear();
                    Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                    //Reconnect();
                }
            }
        }

        public void Download_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                ButtonsPanel.GetBrickPopup.IsOpen = false;
                AdjustDisabledStates();
                return;
            }

            try
            {
                FileInfo info;
                OpenFileDialog open = new OpenFileDialog();
                if (downloadPath != "")
                    open.InitialDirectory = downloadPath;

                if (open.ShowDialog() == true)
                {
                    info = new FileInfo(open.FileName);
                    downloadPath = info.DirectoryName;
                    byte[] content = new byte[info.Length];
                    FileStream fs = new FileStream(info.FullName, FileMode.Open, FileAccess.Read);
                    int pos = 0;
                    while (pos < content.Length)
                    {
                        int didread = fs.Read(content, pos, content.Length - pos);
                        if (didread <= 0)
                        {
                            throw new Exception("Unexpected end of file");
                        }
                        pos += didread;
                    }
                    fs.Close();

                    string tmp = "";
                    if (info.Name.IndexOf('.') != -1)
                    {
                        for (int i = 0; i < info.Name.IndexOf('.'); i++)
                        {
                            tmp += info.Name[i];
                        }
                    }

                    if (info.Name.IndexOf(".rgf") == -1 && info.Name.IndexOf(".rsf") == -1)
                    {
                        if (!checkFileName(tmp))
                        {
                            return;
                        }
                    }

                    //Вставка для флагов компиляции
                    if (info.Extension == ".rbf")
                    {
                        string keysPath = info.FullName.Replace(info.Extension, ".info");
                        if (File.Exists(keysPath))
                        {
                            new ReadKeys().Read(keysPath);
                            if (Keys.FolderName != "")
                            {
                                string newPath = "";
                                if (Keys.FolderKey == "SD")
                                {
                                    if (!IsSDCard())
                                    {
                                        Status.Add("SD Card not found");
                                        return;
                                    }
                                    else
                                    {
                                        newPath = "/home/root/lms2012/prjs/SD_Card/" + Keys.FolderName;
                                    }
                                }
                                else if (Keys.FolderKey == "PRJS")
                                {
                                    newPath = "/home/root/lms2012/prjs/" + Keys.FolderName;
                                }

                                BinaryBuffer b = new BinaryBuffer();
                                b.AppendZeroTerminated(internalPath(newPath));
                                connection.SystemCommand(EV3Connection.CREATE_DIR, b);
                                connection.CreateEV3File(internalPath(newPath + "/") + info.Name, content);

                                if (Keys.ImageList.Count != 0 || Keys.SoundList.Count != 0)
                                {
                                    BinaryBuffer m = new BinaryBuffer();
                                    m.AppendZeroTerminated(internalPath(newPath + "/Media/"));
                                    connection.SystemCommand(EV3Connection.CREATE_DIR, m);

                                    if (Keys.SoundList.Count != 0)
                                    {
                                        foreach (var f in Keys.SoundList)
                                        {
                                            DownloadMedia(f, newPath + "/Media/", "Sounds\\rsf");
                                        }
                                    }
                                    if (Keys.ImageList.Count != 0)
                                    {
                                        foreach (var f in Keys.ImageList)
                                        {
                                            DownloadMedia(f, newPath + "/Media/", "Images\\rgf");
                                        }
                                    }
                                }
                                if (Keys.FileList.Count != 0)
                                {
                                    BinaryBuffer f = new BinaryBuffer();
                                    f.AppendZeroTerminated(internalPath(newPath + "/Files/"));
                                    connection.SystemCommand(EV3Connection.CREATE_DIR, f);
                                }
                                ReadEV3Directory(false);
                                AdjustDisabledStates();
                            }
                            else
                            {
                                connection.CreateEV3File(internalPath(ev3path) + info.Name, content);
                                ReadEV3Directory(false);
                                AdjustDisabledStates();
                            }
                        }
                        else
                        {
                            connection.CreateEV3File(internalPath(ev3path) + info.Name, content);
                            ReadEV3Directory(false);
                            AdjustDisabledStates();
                        }
                    }
                    //
                    else
                    {
                        connection.CreateEV3File(internalPath(ev3path) + info.Name, content);
                        ReadEV3Directory(false);
                        EV3RefreshList();
                    }
                }
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }

            ButtonsPanel.GetBrickPopup.IsOpen = false;
        }

        private void EV3Directory_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Preview_PreviewMouseLeftButtonDown(sender, e);
        }

        public static EV3Brick GetEV3Brick
        {
            get { return _ev3Brick; }
        }

        public static Popup GetMenurPopup
        {
            get { return _menuPopup; }
        }

        private void EV3Directory_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (connection == null || !connection.IsOpen())
            {
                AdjustDisabledStates();
                return;
            }
            menuPopup.IsOpen = true;
        }

        private bool IsSDCard()
        {
            bool res = false;

            try
            {
                MemoryStream data = new MemoryStream();

                //            if (!ev3path.Equals("/proc/"))       // avoid locking up the brick
                {
                    // get data from brick
                    BinaryBuffer b = new BinaryBuffer();
                    b.Append16(500);  // expect max 500 bytes per packet
                    b.AppendZeroTerminated(internalPath("/home/root/lms2012/prjs/"));
                    byte[] response = connection.SystemCommand(EV3Connection.LIST_FILES, b);

                    if (response == null)
                    {
                        throw new Exception("No response to LIST_FILES");
                    }
                    if (response.Length < 6)
                    {
                        throw new Exception("Response too short for LIST_FILES");
                    }
                    if (response[0] != EV3Connection.SUCCESS && response[0] != EV3Connection.END_OF_FILE)
                    {
                        throw new Exception("Unexpected status at LIST_FILES: " + response[0]);
                    }
                    int handle = response[5] & 0xff;
                    data.Write(response, 6, response.Length - 6);

                    // continue reading until have total buffer
                    while (response[0] != EV3Connection.END_OF_FILE)
                    {
                        b.Clear();
                        b.Append8(handle);
                        b.Append16(500);  // expect max 500 bytes per packet
                        response = connection.SystemCommand(EV3Connection.CONTINUE_LIST_FILES, b);
                        //                    Console.WriteLine("follow-up response length: " + response.Length);

                        if (response == null)
                        {
                            throw new Exception("No response to CONTINUE_LIST_FILES");
                        }
                        if (response.Length < 2)
                        {
                            throw new Exception("Too short response to CONTINUE_LIST_FILES");
                        }
                        if (response[0] != EV3Connection.SUCCESS && response[0] != EV3Connection.END_OF_FILE)
                        {
                            throw new Exception("Unexpected status at CONTINUE_LIST_FILES: " + response[0]);
                        }
                        //                    Console.WriteLine("subsequent response length: " + response.Length);
                        data.Write(response, 2, response.Length - 2);
                    }
                }

                List<DirectoryEntry> lists = new List<DirectoryEntry>();

                data.Position = 0;  // start reading at beginning
                StreamReader tr = new StreamReader(data, Encoding.GetEncoding("iso-8859-1"));
                String l;
                while ((l = tr.ReadLine()) != null)
                {
                    if (l.EndsWith("/"))
                    {
                        String n = l.Substring(0, l.Length - 1);
                        if ((!n.Equals(".")) && (!n.Equals("..")))
                        {
                            lists.Add(new DirectoryEntry(n, 0, true));
                        }
                    }
                    else
                    {
                        int firstspace = l.IndexOf(' ');
                        if (firstspace < 0)
                        {
                            continue;
                        }
                        int secondspace = l.IndexOf(' ', firstspace + 1);
                        if (secondspace < 0)
                        {
                            continue;
                        }
                        int size = int.Parse(l.Substring(firstspace, secondspace - firstspace).Trim(), System.Globalization.NumberStyles.HexNumber);

                        lists.Add(new DirectoryEntry(l.Substring(secondspace + 1), size, false));
                    }
                }

                // sort list
                lists.Sort((x, y) => x.FileName.CompareTo(y.FileName));

                //string tmp = "";
                foreach (var list in lists)
                {
                    //tmp += list.FileName;
                    //tmp += '\n';
                    if (list.FileName == "SD_Card" && list.IsDirectory)
                    {
                        res = true;
                    }
                }
                //MessageBox.Show(tmp);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }

            return res;
        }

        private void DownloadMedia(string file, string path, string mediaType)
        {
            try
            {
                string appPath = Application.ResourceAssembly.Location.Replace("Clever.exe", "Media") + "\\" + mediaType + "\\";
                if (!File.Exists(appPath + file))
                {
                    Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + appPath + file + " " + MainWindowVM.GetLocalization["prepRead2"]);
                    return;
                }
                FileInfo fi = new FileInfo(appPath + file);
                //downloadPath = fi.DirectoryName;
                byte[] content = new byte[fi.Length];
                FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                int pos = 0;
                while (pos < content.Length)
                {
                    int didread = fs.Read(content, pos, content.Length - pos);
                    if (didread <= 0)
                    {
                        throw new Exception("Unexpected end of file");
                    }
                    pos += didread;
                }
                fs.Close();

                connection.CreateEV3File(internalPath(path) + fi.Name, content);
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private void DownloadSourceFiles(List<string> files, string path)
        {
            try
            {
                foreach (var curPrg in files)
                {
                    //Status.Add(cur.FullPath);
                    if (!File.Exists(curPrg))
                    {
                        Status.Add(MainWindowVM.GetLocalization["prepRead1"] + " " + curPrg + " " + MainWindowVM.GetLocalization["prepRead2"]);
                        return;
                    }
                    FileInfo fi = new FileInfo(curPrg);
                    byte[] content = new byte[fi.Length];
                    FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                    int pos = 0;
                    while (pos < content.Length)
                    {
                        int didread = fs.Read(content, pos, content.Length - pos);
                        if (didread <= 0)
                        {
                            throw new Exception("Unexpected end of file");
                        }
                        pos += didread;
                    }
                    fs.Close();
                    connection.CreateEV3File(internalPath(path) + fi.Name, content);

                }
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(MainWindowVM.GetLocalization["brkExcept"] + " " + ex.Message);
                Reconnect();
            }
        }

        private string GetPath(string str)
        {
            string path = "";
            int index = str.LastIndexOf('\\');
            for (int i = 0; i <= index; i++)
            {
                path += str[i];
            }
            return path;
        }
        private string GetName(string str)
        {
            string name = "";
            int index = str.LastIndexOf('\\');
            for (int i = index + 1; i < str.Length; i++)
            {
                name += str[i];
            }
            return name;
        }

    }
}
