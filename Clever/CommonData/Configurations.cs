using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clever.CommonData
{
    internal class Configurations
    {
        private static int ConfigCount = 40;
        public static Configurations Get { get; private set; }

        #region Bindings

        public WindowState Window_State { get; set; }
        public double Window_Top { get; set; }
        public double Window_Left { get; set; }
        public double Window_Height { get; set; }
        public double Window_Width { get; set; }
        public GridLength Grid_Row1 { get; set; }
        public GridLength Grid_Row3 { get; set; }
        public GridLength Grid_Column0 { get; set; }
        public GridLength Grid_Column2 { get; set; }
        public bool Association { get; set; }
        public bool Association_NotShow { get; set; }
        public string Language { get; set; }
        internal bool ShowNumber { get; set; }
        internal bool ShowLine { get; set; }
        internal bool ShowSpace { get; set; }
        internal bool Wrap { get; set; }

        internal bool File_New { get; set; }
        internal bool File_Open { get; set; }
        internal bool File_CloseAll { get; set; }
        internal bool File_Save { get; set; }
        internal bool File_SaveAll { get; set; }
        internal bool File_SaveAs { get; set; }
        internal bool Edit_Copy { get; set; }
        internal bool Edit_Paste { get; set; }
        internal bool Edit_Cut { get; set; }
        internal bool Edit_Undo { get; set; }
        internal bool Edit_Redo { get; set; }
        internal bool Edit_SelectAll { get; set; }
        internal bool Edit_Format { get; set; }
        internal bool Edit_ShowLine { get; set; }
        internal bool Edit_ShowNumber { get; set; }
        internal bool Edit_ShowSpace { get; set; }
        internal bool Edit_Wrap { get; set; }
        internal bool Edit_Find { get; set; }

        internal bool Brick_Compile { get; set; }
        internal bool Brick_CompileAndRun { get; set; }
        internal bool Brick_Connect { get; set; }
        internal bool Brick_Download { get; set; }
        internal bool Brick_Stop { get; set; }
        internal int Zoom { get; set; }

        #endregion

        #region Install New Configurations

        internal static void Install()
        {
            Get = new Configurations();
            var list = Get.ReadFile(Get.FullName);
            //var list = new List<string>();
            if (list == null || list.Count < ConfigCount)
            {
                SetDefault();
                return;
            }

            try
            {
                switch (list[0])
                {
                    case "0":
                        Get.Window_State = WindowState.Normal;
                        break;
                    case "1":
                        Get.Window_State = WindowState.Minimized;
                        break;
                    case "2":
                        Get.Window_State = WindowState.Maximized;
                        break;
                    default:
                        Get.Window_State = WindowState.Normal;
                        break;
                }
                Get.Window_Top = Convert.ToDouble(list[1]);
                Get.Window_Left = Convert.ToDouble(list[2]);
                Get.Window_Height = Convert.ToDouble(list[3]);
                Get.Window_Width = Convert.ToDouble(list[4]);
                Get.Grid_Row1 = new GridLength(Convert.ToDouble(list[5]), GridUnitType.Star);
                Get.Grid_Row3 = new GridLength(Convert.ToDouble(list[6]), GridUnitType.Star);
                Get.Grid_Column0 = new GridLength(Convert.ToDouble(list[7]), GridUnitType.Star);
                Get.Grid_Column2 = new GridLength(Convert.ToDouble(list[8]), GridUnitType.Star);
                Get.Association = Convert.ToBoolean(list[9]);
                Get.Association = Convert.ToBoolean(list[10]);
                Get.Language = list[11];
                Get.ShowNumber = Convert.ToBoolean(list[12]);
                Get.ShowLine = Convert.ToBoolean(list[13]);
                Get.ShowSpace = Convert.ToBoolean(list[14]);
                Get.Wrap = Convert.ToBoolean(list[15]);
                Get.File_New = Convert.ToBoolean(list[16]);
                Get.File_Open = Convert.ToBoolean(list[17]);
                Get.File_CloseAll = Convert.ToBoolean(list[18]);
                Get.File_Save = Convert.ToBoolean(list[19]);
                Get.File_SaveAll = Convert.ToBoolean(list[20]);
                Get.File_SaveAs = Convert.ToBoolean(list[21]);
                Get.Edit_Copy = Convert.ToBoolean(list[22]);
                Get.Edit_Paste = Convert.ToBoolean(list[23]);
                Get.Edit_Cut = Convert.ToBoolean(list[24]);
                Get.Edit_Undo = Convert.ToBoolean(list[25]);
                Get.Edit_Redo = Convert.ToBoolean(list[26]);
                Get.Edit_SelectAll = Convert.ToBoolean(list[27]);
                Get.Edit_Format = Convert.ToBoolean(list[28]);
                Get.Edit_ShowLine = Convert.ToBoolean(list[29]);
                Get.Edit_ShowNumber = Convert.ToBoolean(list[30]); 
                Get.Edit_ShowSpace = Convert.ToBoolean(list[31]);
                Get.Edit_Wrap = Convert.ToBoolean(list[32]);
                Get.Edit_Find = Convert.ToBoolean(list[33]);
                Get.Brick_Compile = Convert.ToBoolean(list[34]);
                Get.Brick_CompileAndRun = Convert.ToBoolean(list[35]);
                Get.Brick_Connect = Convert.ToBoolean(list[36]);
                Get.Brick_Download = Convert.ToBoolean(list[37]);
                Get.Brick_Stop = Convert.ToBoolean(list[38]);
                Get.Zoom = Convert.ToInt16(list[39]);
            }
            catch
            {
                SetDefault();
                return;
            }
        }

        #endregion

        #region Save Current Configurations

        internal static void Save()
        {
            var list = new List<string>();

            // 1) WindowState
            switch (Get.Window_State)
            {
                case WindowState.Normal:
                    list.Add("0");
                    break;
                case WindowState.Minimized:
                    list.Add("1");
                    break;
                case WindowState.Maximized:
                    list.Add("2");
                    break;
            }
            // 2) WindowTop
            list.Add(Get.Window_Top.ToString());
            // 3) WindowLeft
            list.Add(Get.Window_Left.ToString());
            // 4) WindowHeight
            list.Add(Get.Window_Height.ToString());
            // 5) WindowWidth
            list.Add(Get.Window_Width.ToString());

            list.Add(Get.Grid_Row1.Value.ToString());
            list.Add(Get.Grid_Row3.Value.ToString());
            list.Add(Get.Grid_Column0.Value.ToString());
            list.Add(Get.Grid_Column2.Value.ToString());
            list.Add(Get.Association.ToString());
            list.Add(Get.Association.ToString());
            list.Add(Get.Language);
            list.Add(Get.ShowNumber.ToString());
            list.Add(Get.ShowLine.ToString());
            list.Add(Get.ShowSpace.ToString());
            list.Add(Get.Wrap.ToString());
            list.Add(Get.File_New.ToString());
            list.Add(Get.File_Open.ToString());
            list.Add(Get.File_CloseAll.ToString());
            list.Add(Get.File_Save.ToString());
            list.Add(Get.File_SaveAll.ToString());
            list.Add(Get.File_SaveAs.ToString());
            list.Add(Get.Edit_Copy.ToString());
            list.Add(Get.Edit_Paste.ToString());
            list.Add(Get.Edit_Cut.ToString());
            list.Add(Get.Edit_Undo.ToString());
            list.Add(Get.Edit_Redo.ToString());
            list.Add(Get.Edit_SelectAll.ToString());
            list.Add(Get.Edit_Format.ToString());
            list.Add(Get.Edit_ShowLine.ToString());
            list.Add(Get.Edit_ShowNumber.ToString());
            list.Add(Get.Edit_ShowSpace.ToString());
            list.Add(Get.Edit_Wrap.ToString());
            list.Add(Get.Edit_Find.ToString());
            list.Add(Get.Brick_Compile.ToString());
            list.Add(Get.Brick_CompileAndRun.ToString());
            list.Add(Get.Brick_Connect.ToString());
            list.Add(Get.Brick_Download.ToString());
            list.Add(Get.Brick_Stop.ToString());
            list.Add(Get.Zoom.ToString());

            WriteFile(Get.FullPath, Get.FileName, list);
        }

        #endregion

        #region Default Configurations

        private static void SetDefault()
        {
            Get.Window_State = WindowState.Normal;
            Get.Window_Top = 40;
            Get.Window_Left = 40;
            Get.Window_Height = 600;
            Get.Window_Width = 1000;
            Get.Grid_Row1 = new GridLength(450, GridUnitType.Star);
            Get.Grid_Row3 = new GridLength(80, GridUnitType.Star);
            Get.Grid_Column0 = new GridLength(700, GridUnitType.Star);
            Get.Grid_Column2 = new GridLength(300, GridUnitType.Star);
            Get.Association = false;
            Get.Association_NotShow = false;
            Get.Language = "en";
            Get.ShowNumber = true;
            Get.ShowLine = true;
            Get.ShowSpace = false;
            Get.Wrap = true;
            Get.File_New = true;
            Get.File_Open = true;
            Get.File_CloseAll = true;
            Get.File_Save = true;
            Get.File_SaveAll = false;
            Get.File_SaveAs = false;
            Get.Edit_Copy = true;
            Get.Edit_Paste = true;
            Get.Edit_Cut = false;
            Get.Edit_Undo = false;
            Get.Edit_Redo = false;
            Get.Edit_SelectAll = false;
            Get.Edit_Format = false;
            Get.Edit_ShowLine = false;
            Get.Edit_ShowNumber = false;
            Get.Edit_ShowSpace = false;
            Get.Edit_Wrap = false;
            Get.Edit_Find = true;
            Get.Brick_Compile = true;
            Get.Brick_CompileAndRun = false;
            Get.Brick_Connect = true;
            Get.Brick_Download = false;
            Get.Brick_Stop = true;
            Get.Zoom = 0;
        }

        #endregion

        #region Utils

        private static void WriteFile(string path, string name, List<string> list)
        {
            DirectoryInfo df = new DirectoryInfo(path);
            if (!df.Exists)
            {
                df.Create();
            }

            string tmpText = "";
            for (int i = 0; i < list.Count; i++)
            {
                tmpText += list[i];
                if (i < list.Count - 1)
                {
                    tmpText += '\n';
                }
            }
            File.WriteAllText(df.FullName + "\\" + name, tmpText);
        }

        private List<string> ReadFile(string fuulPath)
        {
            if (!File.Exists(fuulPath))
            {
                return null;
            }
            return File.ReadAllLines(fuulPath).ToList();
        }

        #endregion

        #region File Name

        private string FullPath
        {
            get
            {
                return Path.Combine(SpecialFolder, AppName, AppVersion);
            }
        }

        private string FullName
        {
            get
            {
                return Path.Combine(SpecialFolder, AppName, AppVersion, FileName);
            }
        }

        private string SpecialFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); }
        }

        private string AppName
        {
            get { return Assembly.GetExecutingAssembly().GetName().Name; }
        }

        private string AppVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        private string FileName
        {
            get { return "Setting.ini"; }
        }

        #endregion
    }
}
