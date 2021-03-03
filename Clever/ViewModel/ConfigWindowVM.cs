﻿using System;
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

namespace Clever.ViewModel
{
    internal class ConfigWindowVM : BaseViewModel
    {

        internal bool ChangeColor;

        internal ConfigWindowVM()
        {
            ChangeColor = false;
            
            SetSettingColor();

            ChangeColor = true;
        }

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

            Comment_Color = new SolidColorBrush(ToSWMC(Configurations.Get.Comment_Color));
            CommentR = Convert.ToDouble(Comment_Color.Color.R);
            CommentG = Convert.ToDouble(Comment_Color.Color.G);
            CommentB = Convert.ToDouble(Comment_Color.Color.B);
        }

        private System.Drawing.Color ToSDC(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private Color ToSWMC (System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
