using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clever.ViewModel.BaseVM;
using Clever.CommonData;
using System.Windows.Media;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using ScintillaNET;
using System.Windows.Input;

namespace Clever.ViewModel
{
    internal class ConfigWindowVM : BaseViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand DefaultColorCommand { get; set; }
        internal bool ChangeColor;

        internal ConfigWindowVM()
        {
            SaveCommand = new RelayCommand(param => SaveClick(), true);
            DefaultColorCommand = new RelayCommand(param => DefaultColoreClick(), true);

            ChangeColor = false;
            
            SetSettingColor();

            ChangeColor = true;
        }

        #region METHODS FOR COMMAND

        private void SaveClick()
        {
            Configurations.Save();
        }

        private void DefaultColoreClick()
        {
            Configurations.SetDefaultColor();
            ChangeColor = false;
            SetSettingColor();
            ChangeColor = true;
        }

        #endregion

        #region COLOR BINDING

        #region Editor background

        private SolidColorBrush _editorBGD_Color;
        public SolidColorBrush EditorBGD_Color
        {
            get { return _editorBGD_Color; }
            set
            {
                _editorBGD_Color = value;
                OnPropertyChanged("EditorBGD_Color");
            }
        }

        private double _editorBGDR;
        public double EditorBGDR
        {
            get { return _editorBGDR; }
            set
            {
                _editorBGDR = value;
                OnPropertyChanged("EditorBGDR");
                SetEditorBGDColor();
            }
        }

        private double _editorBGDG;
        public double EditorBGDG
        {
            get { return _editorBGDG; }
            set
            {
                _editorBGDG = value;
                OnPropertyChanged("EditorBGDG");
                SetEditorBGDColor();
            }
        }

        private double _editorBGDB;
        public double EditorBGDB
        {
            get { return _editorBGDB; }
            set
            {
                _editorBGDB = value;
                OnPropertyChanged("EditorBGDB");
                SetEditorBGDColor();
            }
        }

        internal void SetEditorBGDColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(EditorBGDR);
            var g = Convert.ToByte(EditorBGDG);
            var b = Convert.ToByte(EditorBGDB);
            var color = Color.FromRgb(r, g, b);
            EditorBGD_Color = new SolidColorBrush(color);
            Configurations.Get.Back_Color = ToSDC(color);
            Configurations.Get.Back_Folding_Color = ToSDC(color);
        }

        #endregion

        #region Line Number Backgroun

        private SolidColorBrush _lineNumberBGD_Color;
        public SolidColorBrush LineNumberBGD_Color
        {
            get { return _lineNumberBGD_Color; }
            set
            {
                _lineNumberBGD_Color = value;
                OnPropertyChanged("LineNumberBGD_Color");
            }
        }

        private double _lineNumberBGDR;
        public double LineNumberBGDR
        {
            get { return _lineNumberBGDR; }
            set
            {
                _lineNumberBGDR = value;
                OnPropertyChanged("LineNumberBGDR");
                SetLineNumberBGDColor();
            }
        }

        private double _lineNumberBGDG;
        public double LineNumberBGDG
        {
            get { return _lineNumberBGDG; }
            set
            {
                _lineNumberBGDG = value;
                OnPropertyChanged("LineNumberBGDG");
                SetLineNumberBGDColor();
            }
        }

        private double _lineNumberBGDB;
        public double LineNumberBGDB
        {
            get { return _lineNumberBGDB; }
            set
            {
                _lineNumberBGDB = value;
                OnPropertyChanged("LineNumberBGDB");
                SetLineNumberBGDColor();
            }
        }

        internal void SetLineNumberBGDColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(LineNumberBGDR);
            var g = Convert.ToByte(LineNumberBGDG);
            var b = Convert.ToByte(LineNumberBGDB);
            var color = Color.FromRgb(r, g, b);
            LineNumberBGD_Color = new SolidColorBrush(color);
            Configurations.Get.Back_Margin_Color = ToSDC(color);
        }

        #endregion

        #region Line Number Foreground

        private SolidColorBrush _lineNumberFORE_Color;
        public SolidColorBrush LineNumberFORE_Color
        {
            get { return _lineNumberFORE_Color; }
            set
            {
                _lineNumberFORE_Color = value;
                OnPropertyChanged("LineNumberFORE_Color");
            }
        }

        private double _lineNumberFORER;
        public double LineNumberFORER
        {
            get { return _lineNumberFORER; }
            set
            {
                _lineNumberFORER = value;
                OnPropertyChanged("LineNumberFORER");
                SetLineNumberFOREColor();
            }
        }

        private double _lineNumberFOREG;
        public double LineNumberFOREG
        {
            get { return _lineNumberFOREG; }
            set
            {
                _lineNumberFOREG = value;
                OnPropertyChanged("LineNumberFOREG");
                SetLineNumberFOREColor();
            }
        }

        private double _lineNumberFOREB;
        public double LineNumberFOREB
        {
            get { return _lineNumberFOREB; }
            set
            {
                _lineNumberFOREB = value;
                OnPropertyChanged("LineNumberFOREB");
                SetLineNumberFOREColor();
            }
        }

        internal void SetLineNumberFOREColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(LineNumberFORER);
            var g = Convert.ToByte(LineNumberFOREG);
            var b = Convert.ToByte(LineNumberFOREB);
            var color = Color.FromRgb(r, g, b);
            LineNumberFORE_Color = new SolidColorBrush(color);
            Configurations.Get.Fore_Margin_Color = ToSDC(color);
        }

        #endregion

        #region Selection Line

        private SolidColorBrush _selectionLine_Color;
        public SolidColorBrush SelectionLine_Color
        {
            get { return _selectionLine_Color; }
            set
            {
                _selectionLine_Color = value;
                OnPropertyChanged("SelectionLine_Color");
            }
        }

        private double _selectionLineR;
        public double SelectionLineR
        {
            get { return _selectionLineR; }
            set
            {
                _selectionLineR = value;
                OnPropertyChanged("SelectionLineR");
                SetSelectionLineColor();
            }
        }

        private double _selectionLineG;
        public double SelectionLineG
        {
            get { return _selectionLineG; }
            set
            {
                _selectionLineG = value;
                OnPropertyChanged("SelectionLineG");
                SetSelectionLineColor();
            }
        }

        private double _selectionLineB;
        public double SelectionLineB
        {
            get { return _selectionLineB; }
            set
            {
                _selectionLineB = value;
                OnPropertyChanged("SelectionLineB");
                SetSelectionLineColor();
            }
        }

        internal void SetSelectionLineColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(SelectionLineR);
            var g = Convert.ToByte(SelectionLineG);
            var b = Convert.ToByte(SelectionLineB);
            var color = Color.FromRgb(r, g, b);
            SelectionLine_Color = new SolidColorBrush(color);
            Configurations.Get.Carret_Line_Color = ToSDC(color);
        }

        #endregion

        #region Selection

        private SolidColorBrush _selection_Color;
        public SolidColorBrush Selection_Color
        {
            get { return _selection_Color; }
            set
            {
                _selection_Color = value;
                OnPropertyChanged("Selection_Color");
            }
        }

        private double _selectionR;
        public double SelectionR
        {
            get { return _selectionR; }
            set
            {
                _selectionR = value;
                OnPropertyChanged("SelectionR");
                SetSelectionColor();
            }
        }

        private double _selectionG;
        public double SelectionG
        {
            get { return _selectionG; }
            set
            {
                _selectionG = value;
                OnPropertyChanged("SelectionG");
                SetSelectionColor();
            }
        }

        private double _selectionB;
        public double SelectionB
        {
            get { return _selectionB; }
            set
            {
                _selectionB = value;
                OnPropertyChanged("SelectionB");
                SetSelectionColor();
            }
        }

        internal void SetSelectionColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(SelectionR);
            var g = Convert.ToByte(SelectionG);
            var b = Convert.ToByte(SelectionB);
            var color = Color.FromRgb(r, g, b);
            Selection_Color = new SolidColorBrush(color);
            Configurations.Get.Select_Color = ToSDC(color);
        }

        #endregion

        #region Comment

        private SolidColorBrush _comment_Color;
        public SolidColorBrush Comment_Color
        {
            get { return _comment_Color; }
            set
            {
                _comment_Color = value;
                OnPropertyChanged("Comment_Color");
            }
        }

        private double _commentR;
        public double CommentR
        {
            get { return _commentR; }
            set
            {
                _commentR = value;
                OnPropertyChanged("CommentR");
                SetCommentColor();
            }
        }

        private double _commentG;
        public double CommentG
        {
            get { return _commentG; }
            set
            {
                _commentG = value;
                OnPropertyChanged("CommentG");
                SetCommentColor();
            }
        }

        private double _commentB;
        public double CommentB
        {
            get { return _commentB; }
            set
            {
                _commentB = value;
                OnPropertyChanged("CommentB");
                SetCommentColor();
            }
        }

        internal void SetCommentColor()
        {
            if (!ChangeColor)
                return;

            var r = Convert.ToByte(CommentR);
            var g = Convert.ToByte(CommentG);
            var b = Convert.ToByte(CommentB);
            var color = Color.FromRgb(r, g, b);
            Comment_Color = new SolidColorBrush(color);
            Configurations.Get.Comment_Color = ToSDC(color);
        }

        #endregion

        #endregion

        internal void SetSettingColor()
        {
            LineNumberBGD_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Back_Margin_Color));
            LineNumberBGDR = Convert.ToDouble(LineNumberBGD_Color.Color.R);
            LineNumberBGDG = Convert.ToDouble(LineNumberBGD_Color.Color.G);
            LineNumberBGDB = Convert.ToDouble(LineNumberBGD_Color.Color.B);

            LineNumberFORE_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Fore_Margin_Color));
            LineNumberFORER = Convert.ToDouble(LineNumberFORE_Color.Color.R);
            LineNumberFOREG = Convert.ToDouble(LineNumberFORE_Color.Color.G);
            LineNumberFOREB = Convert.ToDouble(LineNumberFORE_Color.Color.B);

            EditorBGD_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Back_Color));
            EditorBGDR = Convert.ToDouble(EditorBGD_Color.Color.R);
            EditorBGDG = Convert.ToDouble(EditorBGD_Color.Color.G);
            EditorBGDB = Convert.ToDouble(EditorBGD_Color.Color.B);

            SelectionLine_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Carret_Line_Color));
            SelectionLineR = Convert.ToDouble(SelectionLine_Color.Color.R);
            SelectionLineG = Convert.ToDouble(SelectionLine_Color.Color.G);
            SelectionLineB = Convert.ToDouble(SelectionLine_Color.Color.B);

            Selection_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Select_Color));
            SelectionR = Convert.ToDouble(Selection_Color.Color.R);
            SelectionG = Convert.ToDouble(Selection_Color.Color.G);
            SelectionB = Convert.ToDouble(Selection_Color.Color.B);

            Comment_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Comment_Color));
            CommentR = Convert.ToDouble(Comment_Color.Color.R);
            CommentG = Convert.ToDouble(Comment_Color.Color.G);
            CommentB = Convert.ToDouble(Comment_Color.Color.B);
        }

        #region Utils

        private System.Drawing.Color ToSDC(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private Color ToSWMC (System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion
    }
}
