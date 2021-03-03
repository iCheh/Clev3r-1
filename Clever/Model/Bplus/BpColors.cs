using Clever.CommonData;
using System.Drawing;

namespace Clever.Model.Bplus
{
    class BpColors
    {
        // HELP COLORS
        internal static int HELP_OBJECT_NAME = 0x969696;
        internal static int HELP_METHOD_NAME = 0x0C37CD;
        internal static int HELP_TEXT = 0x000000;

        internal static int HELP_COMMENT_COLOR = 0x008020;
        internal static int HELP_STRING_COLOR = 0xCC6633;

        internal static int HELP_KEYWORD_COLOR = 0x7777FF;
        internal static int HELP_OBJECT_COLOR = 0x006060;
        internal static int HELP_METHOD_COLOR = 0x802020;

        // EDITOR
        internal static Color Foreground_Color { get; set; }
        internal static Color Back_Margin_Color { get; set; }
        internal static Color Fore_Margin_Color { get; set; }
        internal static Color Back_Folding_Color { get; set; }
        internal static Color Fore_Folding_Color { get; set; }
        internal static Color Select_Color { get; set; }
        internal static Color Find_Highlight_Color { get; set; }
        internal static Color Back_Calltip_Color { get; set; }
        internal static Color Fore_Calltip_Color { get; set; }
        internal static Color Carret_Line_Color { get; set; }

        // LEXER
        internal static Color Back_Color { get; set; }
        internal static Color Fore_Color { get; set; }
        internal static Color Comment_Color { get; set; }
        internal static Color String_Color { get; set; }
        internal static Color Operator_Color { get; set; }
        internal static Color Keyword_1_Color { get; set; }
        internal static Color Keyword_2_Color { get; set; }
        internal static Color Keyword_3_Color { get; set; }
        internal static Color Keyword_4_Color { get; set; }
        internal static Color Object_Color { get; set; }
        internal static Color Method_Color { get; set; }
        internal static Color Literal_Color { get; set; }
        internal static Color Number_Color { get; set; }
        internal static Color Sub_Color { get; set; }
        internal static Color Var_Color { get; set; }
        internal static Color Label_Color { get; set; }
        internal static Color Module_Color { get; set; }
        internal static Color Region_Open_Color { get; set; }
        internal static Color Region_Close_Color { get; set; }

        internal static void Install()
        {
            Foreground_Color = Configurations.Get.Foreground_Color;
            Back_Margin_Color = Configurations.Get.Back_Margin_Color;
            Fore_Margin_Color = Configurations.Get.Fore_Margin_Color;
            Back_Folding_Color = Configurations.Get.Back_Folding_Color;
            Fore_Folding_Color = Configurations.Get.Fore_Folding_Color;
            Select_Color = Configurations.Get.Select_Color;
            Find_Highlight_Color = Configurations.Get.Find_Highlight_Color;
            Back_Calltip_Color = Configurations.Get.Back_Calltip_Color;
            Fore_Calltip_Color = Configurations.Get.Fore_Calltip_Color;
            Carret_Line_Color = Configurations.Get.Carret_Line_Color;

            Back_Color = Configurations.Get.Back_Color;
            Fore_Color = Configurations.Get.Fore_Color;
            Comment_Color = Configurations.Get.Comment_Color;
            String_Color = Configurations.Get.String_Color;
            Operator_Color = Configurations.Get.Operator_Color;
            Keyword_1_Color = Configurations.Get.Keyword_1_Color;
            Keyword_2_Color = Configurations.Get.Keyword_2_Color;
            Keyword_3_Color = Configurations.Get.Keyword_3_Color;
            Keyword_4_Color = Configurations.Get.Keyword_4_Color;
            Object_Color = Configurations.Get.Object_Color;
            Method_Color = Configurations.Get.Method_Color;
            Literal_Color = Configurations.Get.Literal_Color;
            Number_Color = Configurations.Get.Number_Color;
            Sub_Color = Configurations.Get.Sub_Color;
            Var_Color = Configurations.Get.Var_Color;
            Label_Color = Configurations.Get.Label_Color;
            Module_Color = Configurations.Get.Module_Color;
            Region_Open_Color = Configurations.Get.Region_Open_Color;
            Region_Close_Color = Configurations.Get.Region_Close_Color;
        }
    }
}
