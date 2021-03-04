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
using System.Collections.ObjectModel;
using Clever.Model.Colorizer;
using Clever.Model.Enums;

namespace Clever.ViewModel
{
    internal class ConfigWindowVM : BaseViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand DefaultColorCommand { get; set; }

        internal ConfigWindowVM()
        {
            Install();
            SaveCommand = new RelayCommand(param => SaveClick(), true);
            DefaultColorCommand = new RelayCommand(param => DefaultColoreClick(), true);
        }

        #region Bindings

        public ObservableCollection<ColorItem> ColorItems { get; private set; }

        #region EDITOR

        private SolidColorBrush _marginBackground_Brush;
        public SolidColorBrush MarginBackground_Brush
        {
            get { return _marginBackground_Brush; }
            set
            {
                _marginBackground_Brush = value;
                OnPropertyChanged("MarginBackground_Brush");
            }
        }

        private SolidColorBrush _marginForeground_Brush;
        public SolidColorBrush MarginForeground_Brush
        {
            get { return _marginForeground_Brush; }
            set
            {
                _marginForeground_Brush = value;
                OnPropertyChanged("MarginForeground_Brush");
            }
        }

        private SolidColorBrush _editorBackground_Brush;
        public SolidColorBrush EditorBackground_Brush
        {
            get { return _editorBackground_Brush; }
            set
            {
                _editorBackground_Brush = value;
                OnPropertyChanged("EditorBackground_Brush");
            }
        }

        private SolidColorBrush _carretLine_Brush;
        public SolidColorBrush CarretLine_Brush
        {
            get { return _carretLine_Brush; }
            set
            {
                _carretLine_Brush = value;
                OnPropertyChanged("CarretLine_Brush");
            }
        }

        private SolidColorBrush _selection_Brush;
        public SolidColorBrush Selection_Brush
        {
            get { return _selection_Brush; }
            set
            {
                _selection_Brush = value;
                OnPropertyChanged("Selection_Brush");
            }
        }

        private SolidColorBrush _calltipBackground_Brush;
        public SolidColorBrush CalltipBackground_Brush
        {
            get { return _calltipBackground_Brush; }
            set
            {
                _calltipBackground_Brush = value;
                OnPropertyChanged("CalltipBackground_Brush");
            }
        }

        private SolidColorBrush _calltipForeground_Brush;
        public SolidColorBrush CalltipForeground_Brush
        {
            get { return _calltipForeground_Brush; }
            set
            {
                _calltipForeground_Brush = value;
                OnPropertyChanged("CalltipForeground_Brush");
            }
        }

        private SolidColorBrush _foldingForeground_Brush;
        public SolidColorBrush FoldingForeground_Brush
        {
            get { return _foldingForeground_Brush; }
            set
            {
                _foldingForeground_Brush = value;
                OnPropertyChanged("FoldingForeground_Brush");
            }
        }

        #endregion

        #region LEXER

        private SolidColorBrush _editorForeground_Brush;
        public SolidColorBrush EditorForeground_Brush
        {
            get { return _editorForeground_Brush; }
            set
            {
                _editorForeground_Brush = value;
                OnPropertyChanged("EditorForeground_Brush");
            }
        }

        private SolidColorBrush _comment_Brush;
        public SolidColorBrush Comment_Brush
        {
            get { return _comment_Brush; }
            set
            {
                _comment_Brush = value;
                OnPropertyChanged("Comment_Brush");
            }
        }

        private SolidColorBrush _string_Brush;
        public SolidColorBrush String_Brush
        {
            get { return _string_Brush; }
            set
            {
                _string_Brush = value;
                OnPropertyChanged("String_Brush");
            }
        }

        private SolidColorBrush _operator_Brush;
        public SolidColorBrush Operator_Brush
        {
            get { return _operator_Brush; }
            set
            {
                _operator_Brush = value;
                OnPropertyChanged("Operator_Brush");
            }
        }

        private SolidColorBrush _keywords1_Brush;
        public SolidColorBrush Keywords1_Brush
        {
            get { return _keywords1_Brush; }
            set
            {
                _keywords1_Brush = value;
                OnPropertyChanged("Keywords1_Brush");
            }
        }

        private SolidColorBrush _keywords2_Brush;
        public SolidColorBrush Keywords2_Brush
        {
            get { return _keywords2_Brush; }
            set
            {
                _keywords2_Brush = value;
                OnPropertyChanged("Keywords2_Brush");
            }
        }

        private SolidColorBrush _object_Brush;
        public SolidColorBrush Object_Brush
        {
            get { return _object_Brush; }
            set
            {
                _object_Brush = value;
                OnPropertyChanged("Object_Brush");
            }
        }

        private SolidColorBrush _module_Brush;
        public SolidColorBrush Module_Brush
        {
            get { return _module_Brush; }
            set
            {
                _module_Brush = value;
                OnPropertyChanged("Module_Brush");
            }
        }

        private SolidColorBrush _method_Brush;
        public SolidColorBrush Method_Brush
        {
            get { return _method_Brush; }
            set
            {
                _method_Brush = value;
                OnPropertyChanged("Method_Brush");
            }
        }

        private SolidColorBrush _number_Brush;
        public SolidColorBrush Number_Brush
        {
            get { return _number_Brush; }
            set
            {
                _number_Brush = value;
                OnPropertyChanged("Number_Brush");
            }
        }

        private SolidColorBrush _sub_Brush;
        public SolidColorBrush Sub_Brush
        {
            get { return _sub_Brush; }
            set
            {
                _sub_Brush = value;
                OnPropertyChanged("Sub_Brush");
            }
        }

        private SolidColorBrush _label_Brush;
        public SolidColorBrush Label_Brush
        {
            get { return _label_Brush; }
            set
            {
                _label_Brush = value;
                OnPropertyChanged("Label_Brush");
            }
        }

        private SolidColorBrush _region_Brush;
        public SolidColorBrush Region_Brush
        {
            get { return _region_Brush; }
            set
            {
                _region_Brush = value;
                OnPropertyChanged("Region_Brush");
            }
        }

        #endregion

        #endregion

        #region Methods For Command

        private void SaveClick()
        {
            Configurations.Save();
        }

        private void DefaultColoreClick()
        {
            Configurations.SetDefaultColor();

            foreach (var item in ColorItems)
            {
                item.SetDefaultColor();
            }
        }

        #endregion

        #region Events

        private void Item_ColorChanged(object sender, EventArgs e)
        {
            var item = sender as ColorItem;
            SetColor(item);
        }

        #endregion

        #region Configurations

        private void Install()
        {
            var items = new ObservableCollection<ColorItem>();
            
            items.Add(new ColorItem(Configurations.Get.Back_Margin_Color, ColorType.BACK_MARGIN_COLOR, "Line number background:"));
            items.Add(new ColorItem(Configurations.Get.Fore_Margin_Color, ColorType.FORE_MARGIN_COLOR, "Line number foreground:"));
            items.Add(new ColorItem(Configurations.Get.Back_Color, ColorType.BACK_COLOR, "Editor background:"));
            items.Add(new ColorItem(Configurations.Get.Fore_Color, ColorType.FORE_COLOR, "Editor foreground:"));
            items.Add(new ColorItem(Configurations.Get.Carret_Line_Color, ColorType.CARRET_LINE_COLOR, "Carret line background:"));
            items.Add(new ColorItem(Configurations.Get.Select_Color, ColorType.SELECT_COLOR, "Selection background:"));

            items.Add(new ColorItem(Configurations.Get.Fore_Folding_Color, ColorType.FORE_FOLDING_COLOR, "Folding foreground:"));
            items.Add(new ColorItem(Configurations.Get.Comment_Color, ColorType.COMMENT_COLOR, "Comment:"));
            items.Add(new ColorItem(Configurations.Get.String_Color, ColorType.STRING_COLOR, "String:"));
            items.Add(new ColorItem(Configurations.Get.Operator_Color, ColorType.OPERATOR_COLOR, "Operator:"));
            items.Add(new ColorItem(Configurations.Get.Keyword_1_Color, ColorType.KEYWORD_1_COLOR, "Keywords 1:"));
            items.Add(new ColorItem(Configurations.Get.Keyword_2_Color, ColorType.KEYWORD_2_COLOR, "Keywords 2:"));

            items.Add(new ColorItem(Configurations.Get.Object_Color, ColorType.OBJECT_COLOR, "Basic object name:"));
            items.Add(new ColorItem(Configurations.Get.Module_Color, ColorType.MODULE_COLOR, "Module name:"));
            items.Add(new ColorItem(Configurations.Get.Method_Color, ColorType.METHOD_COLOR, "Method name:"));

            items.Add(new ColorItem(Configurations.Get.Literal_Color, ColorType.LITERAL_COLOR, "Numbers:"));
            items.Add(new ColorItem(Configurations.Get.Sub_Color, ColorType.SUB_COLOR, "Function and Sub names:"));
            items.Add(new ColorItem(Configurations.Get.Label_Color, ColorType.LABEL_COLOR, "Labels name:"));
            items.Add(new ColorItem(Configurations.Get.Region_Open_Color, ColorType.REGION_OPEN_COLOR, "#region:"));

            items.Add(new ColorItem(Configurations.Get.Back_Calltip_Color, ColorType.BACK_CALLTIP_COLOR, "Calltip background:"));
            items.Add(new ColorItem(Configurations.Get.Fore_Calltip_Color, ColorType.FORE_CALLTIP_COLOR, "Calltip foreground:"));

            foreach (var item in items)
            {
                SetColor(item);
                item.ColorChanged += Item_ColorChanged;
            }

            ColorItems = items;
        }

        #endregion

        #region Utils

        internal void RemoveEvents()
        {
            foreach (var item in ColorItems)
            {
                item.ColorChanged -= Item_ColorChanged;
            }
        }
        //ggggggggggggggggggggggg
        
        private void SetColor(ColorItem item)
        {
            switch (item.ColorType)
            {
                case ColorType.FOREGROUND_COLOR:
                    break;
                case ColorType.BACK_MARGIN_COLOR:
                    MarginBackground_Brush = item.ItemBrush;
                    break;
                case ColorType.FORE_MARGIN_COLOR:
                    MarginForeground_Brush = item.ItemBrush;
                    break;
                case ColorType.BACK_FOLDING_COLOR:
                    break;
                case ColorType.FORE_FOLDING_COLOR:
                    FoldingForeground_Brush = item.ItemBrush;
                    break;
                case ColorType.SELECT_COLOR:
                    Selection_Brush = item.ItemBrush;
                    break;
                case ColorType.FIND_HIGHLIGHT_COLOR:
                    break;
                case ColorType.BACK_CALLTIP_COLOR:
                    CalltipBackground_Brush = item.ItemBrush;
                    break;
                case ColorType.FORE_CALLTIP_COLOR:
                    CalltipForeground_Brush = item.ItemBrush;
                    break;
                case ColorType.CARRET_LINE_COLOR:
                    CarretLine_Brush = item.ItemBrush;
                    break;


                case ColorType.FORE_COLOR:
                    EditorForeground_Brush = item.ItemBrush;
                    break;
                case ColorType.BACK_COLOR:
                    EditorBackground_Brush = item.ItemBrush;
                    break;
                case ColorType.COMMENT_COLOR:
                    Comment_Brush = item.ItemBrush;
                    break;
                case ColorType.STRING_COLOR:
                    String_Brush = item.ItemBrush;
                    break;
                case ColorType.OPERATOR_COLOR:
                    Operator_Brush = item.ItemBrush;
                    break;
                case ColorType.KEYWORD_1_COLOR:
                    Keywords1_Brush = item.ItemBrush;
                    break;
                case ColorType.KEYWORD_2_COLOR:
                    Keywords2_Brush = item.ItemBrush;
                    break;
                case ColorType.KEYWORD_3_COLOR:
                    break;
                case ColorType.KEYWORD_4_COLOR:
                    break;
                case ColorType.OBJECT_COLOR:
                    Object_Brush = item.ItemBrush;
                    break;
                case ColorType.METHOD_COLOR:
                    Method_Brush = item.ItemBrush;
                    break;
                case ColorType.LITERAL_COLOR:
                    Number_Brush = item.ItemBrush;
                    break;
                case ColorType.NUMBER_COLOR:
                    break;
                case ColorType.SUB_COLOR:
                    Sub_Brush = item.ItemBrush;
                    break;
                case ColorType.VAR_COLOR:
                    break;
                case ColorType.LABEL_COLOR:
                    Label_Brush = item.ItemBrush;
                    break;
                case ColorType.MODULE_COLOR:
                    Module_Brush = item.ItemBrush;
                    break;
                case ColorType.REGION_OPEN_COLOR:
                    Region_Brush = item.ItemBrush;
                    break;
                case ColorType.REGION_CLOSE_COLOR:
                    break;
            }
        }

        #endregion
    }
}
