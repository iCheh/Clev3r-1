using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clever.Model.Utils
{
    internal static class Converter
    {
        internal static Visibility BoolToVisibility(bool value)
        {
            if (value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        internal static bool VisibilityToBool(Visibility value)
        {
            if (value == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
