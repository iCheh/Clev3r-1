using Clever.CommonData;
using Clever.Model.BaseVM;
using Clever.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Clever.Model.Colorizer
{
    internal class ColorItem : BaseModel
    {
        private bool _change;
        
        private System.Drawing.Color _defaultColor;

        internal ColorType ColorType { get; private set; }

        internal ColorItem(System.Drawing.Color color, ColorType type, string name)
        {
            ColorType = type;
            VisibleName = name;

            _change = false;

            ItemBrush = new SolidColorBrush(ToSWMC(color));

            ValueR = Convert.ToDouble(ItemBrush.Color.R);
            ValueG = Convert.ToDouble(ItemBrush.Color.G);
            ValueB = Convert.ToDouble(ItemBrush.Color.B);

            _change = true;
        }

        #region Bindings

        public string VisibleName { get; private set; }

        private double _valueR;
        public double ValueR
        {
            get { return _valueR; }
            set
            {
                _valueR = value;
                OnPropertyChanged("ValueR");
                SetNewColor();
            }
        }

        private double _valueG;
        public double ValueG
        {
            get { return _valueG; }
            set
            {
                _valueG = value;
                OnPropertyChanged("ValueG");
                SetNewColor();
            }
        }

        private double _valueB;
        public double ValueB
        {
            get { return _valueB; }
            set
            {
                _valueB = value;
                OnPropertyChanged("ValueR");
                SetNewColor();
            }
        }

        private SolidColorBrush _itemBrush;
        public SolidColorBrush ItemBrush
        {
            get { return _itemBrush; }
            set
            {
                _itemBrush = value;
                OnPropertyChanged("ItemBrush");
                if (_change)
                    ColorChanged(this, new EventArgs());
            }
        }

        #endregion

        #region Event

        public event EventHandler ColorChanged;

        #endregion

        #region Utils

        private void SetNewColor()
        {
            if (!_change)
                return;

            var r = Convert.ToByte(ValueR);
            var g = Convert.ToByte(ValueG);
            var b = Convert.ToByte(ValueB);
            var color = Color.FromRgb(r, g, b);
            ItemBrush = new SolidColorBrush(color);
            ChangeConfigurationColor(color);
        }

        internal void SetDefaultColor()
        {
            switch (ColorType)
            {
                case ColorType.FOREGROUND_COLOR:
                    _defaultColor = Configurations.Get.Foreground_Color;
                    break;
                case ColorType.BACK_MARGIN_COLOR:
                    _defaultColor = Configurations.Get.Back_Margin_Color;
                    break;
                case ColorType.FORE_MARGIN_COLOR:
                    _defaultColor = Configurations.Get.Fore_Margin_Color;
                    break;
                case ColorType.BACK_FOLDING_COLOR:                   
                    _defaultColor = Configurations.Get.Back_Folding_Color;
                    break;
                case ColorType.FORE_FOLDING_COLOR:
                    _defaultColor = Configurations.Get.Fore_Folding_Color;
                    break;
                case ColorType.SELECT_COLOR:
                    _defaultColor = Configurations.Get.Select_Color;
                    break;
                case ColorType.FIND_HIGHLIGHT_COLOR:
                    _defaultColor = Configurations.Get.Find_Highlight_Color;
                    break;
                case ColorType.BACK_CALLTIP_COLOR:
                    _defaultColor = Configurations.Get.Back_Calltip_Color;
                    break;
                case ColorType.FORE_CALLTIP_COLOR:
                    _defaultColor = Configurations.Get.Fore_Calltip_Color;
                    break;
                case ColorType.CARRET_LINE_COLOR:
                    _defaultColor = Configurations.Get.Carret_Line_Color;
                    break;


                case ColorType.FORE_COLOR:
                    _defaultColor = Configurations.Get.Fore_Color;
                    break;
                case ColorType.BACK_COLOR:
                    _defaultColor = Configurations.Get.Back_Color;
                    break;
                case ColorType.COMMENT_COLOR:
                    _defaultColor = Configurations.Get.Comment_Color;
                    break;
                case ColorType.STRING_COLOR:
                    _defaultColor = Configurations.Get.String_Color;
                    break;
                case ColorType.OPERATOR_COLOR:
                    _defaultColor = Configurations.Get.Operator_Color;
                    break;
                case ColorType.KEYWORD_1_COLOR:
                    _defaultColor = Configurations.Get.Keyword_1_Color;
                    break;
                case ColorType.KEYWORD_2_COLOR:
                    _defaultColor = Configurations.Get.Keyword_2_Color;
                    break;
                case ColorType.KEYWORD_3_COLOR:
                    _defaultColor = Configurations.Get.Keyword_3_Color;
                    break;
                case ColorType.KEYWORD_4_COLOR:
                    _defaultColor = Configurations.Get.Keyword_4_Color;
                    break;
                case ColorType.OBJECT_COLOR:
                    _defaultColor = Configurations.Get.Object_Color;
                    break;
                case ColorType.METHOD_COLOR:
                    _defaultColor = Configurations.Get.Method_Color;
                    break;
                case ColorType.LITERAL_COLOR:
                    _defaultColor = Configurations.Get.Literal_Color;
                    break;
                case ColorType.NUMBER_COLOR:
                    _defaultColor = Configurations.Get.Number_Color;
                    break;
                case ColorType.SUB_COLOR:
                    _defaultColor = Configurations.Get.Sub_Color;
                    break;
                case ColorType.VAR_COLOR:
                    _defaultColor = Configurations.Get.Var_Color;
                    break;
                case ColorType.LABEL_COLOR:
                    _defaultColor = Configurations.Get.Label_Color;
                    break;
                case ColorType.MODULE_COLOR:
                    _defaultColor = Configurations.Get.Module_Color;
                    break;
                case ColorType.REGION_OPEN_COLOR:
                    _defaultColor = Configurations.Get.Region_Open_Color;
                    _defaultColor = Configurations.Get.Region_Close_Color;
                    break;
                case ColorType.REGION_CLOSE_COLOR:
                    _defaultColor = Configurations.Get.Region_Close_Color;
                    _defaultColor = Configurations.Get.Region_Open_Color;
                    break;
            }

            _change = false;

            ItemBrush = new SolidColorBrush(ToSWMC(_defaultColor));
            ValueR = Convert.ToDouble(ItemBrush.Color.R);
            ValueG = Convert.ToDouble(ItemBrush.Color.G);
            ValueB = Convert.ToDouble(ItemBrush.Color.B);

            _change = true;

            ColorChanged(this, new EventArgs());
        }

        private void ChangeConfigurationColor(Color color)
        {
            switch (ColorType)
            {
                case ColorType.FOREGROUND_COLOR:
                    Configurations.Get.Foreground_Color = ToSDC(color);
                    break;
                case ColorType.BACK_MARGIN_COLOR:
                    Configurations.Get.Back_Margin_Color = ToSDC(color);
                    break;
                case ColorType.FORE_MARGIN_COLOR:
                    Configurations.Get.Fore_Margin_Color = ToSDC(color);
                    break;
                case ColorType.BACK_FOLDING_COLOR:
                    Configurations.Get.Back_Color = ToSDC(color);
                    Configurations.Get.Back_Folding_Color = ToSDC(color);
                    break;
                case ColorType.FORE_FOLDING_COLOR:
                    Configurations.Get.Fore_Folding_Color = ToSDC(color);
                    break;
                case ColorType.SELECT_COLOR:
                    Configurations.Get.Select_Color = ToSDC(color);
                    break;
                case ColorType.FIND_HIGHLIGHT_COLOR:
                    Configurations.Get.Find_Highlight_Color = ToSDC(color);
                    break;
                case ColorType.BACK_CALLTIP_COLOR:
                    Configurations.Get.Back_Calltip_Color = ToSDC(color);
                    break;
                case ColorType.FORE_CALLTIP_COLOR:
                    Configurations.Get.Fore_Calltip_Color = ToSDC(color);
                    break;
                case ColorType.CARRET_LINE_COLOR:
                    Configurations.Get.Carret_Line_Color = ToSDC(color);
                    break;

                case ColorType.FORE_COLOR:
                    Configurations.Get.Fore_Color = ToSDC(color);
                    break;
                case ColorType.BACK_COLOR:
                    Configurations.Get.Back_Color = ToSDC(color);
                    Configurations.Get.Back_Folding_Color = ToSDC(color);
                    break;
                case ColorType.COMMENT_COLOR:
                    Configurations.Get.Comment_Color = ToSDC(color);
                    break;
                case ColorType.STRING_COLOR:
                    Configurations.Get.String_Color = ToSDC(color);
                    break;
                case ColorType.OPERATOR_COLOR:
                    Configurations.Get.Operator_Color = ToSDC(color);
                    break;
                case ColorType.KEYWORD_1_COLOR:
                    Configurations.Get.Keyword_1_Color = ToSDC(color);
                    Configurations.Get.Keyword_3_Color = ToSDC(color);
                    Configurations.Get.Keyword_4_Color = ToSDC(color);
                    break;
                case ColorType.KEYWORD_2_COLOR:
                    Configurations.Get.Keyword_2_Color = ToSDC(color);
                    break;
                case ColorType.KEYWORD_3_COLOR:
                    Configurations.Get.Keyword_3_Color = ToSDC(color);
                    Configurations.Get.Keyword_1_Color = ToSDC(color);
                    Configurations.Get.Keyword_4_Color = ToSDC(color);
                    break;
                case ColorType.KEYWORD_4_COLOR:
                    Configurations.Get.Keyword_4_Color = ToSDC(color);
                    Configurations.Get.Keyword_1_Color = ToSDC(color);
                    Configurations.Get.Keyword_3_Color = ToSDC(color);
                    break;
                case ColorType.OBJECT_COLOR:
                    Configurations.Get.Object_Color = ToSDC(color);
                    break;
                case ColorType.METHOD_COLOR:
                    Configurations.Get.Method_Color = ToSDC(color);
                    break;
                case ColorType.LITERAL_COLOR:
                    Configurations.Get.Literal_Color = ToSDC(color);
                    break;
                case ColorType.NUMBER_COLOR:
                    Configurations.Get.Number_Color = ToSDC(color);
                    break;
                case ColorType.SUB_COLOR:
                    Configurations.Get.Sub_Color = ToSDC(color);
                    break;
                case ColorType.VAR_COLOR:
                    Configurations.Get.Var_Color = ToSDC(color);
                    break;
                case ColorType.LABEL_COLOR:
                    Configurations.Get.Label_Color = ToSDC(color);
                    break;
                case ColorType.MODULE_COLOR:
                    Configurations.Get.Module_Color = ToSDC(color);
                    break;
                case ColorType.REGION_OPEN_COLOR:
                    Configurations.Get.Region_Open_Color = ToSDC(color);
                    break;
                case ColorType.REGION_CLOSE_COLOR:
                    Configurations.Get.Region_Close_Color = ToSDC(color);
                    break;
            }
        }

        private System.Drawing.Color ToSDC(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private Color ToSWMC(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion
    }
}
