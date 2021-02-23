using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clever.View.Panels
{
    /// <summary>
    /// Логика взаимодействия для MediaPanelItemHeader.xaml
    /// </summary>
    public partial class MediaPanelItemHeader : UserControl
    {
        public MediaPanelItemHeader()
        {
            InitializeComponent();
        }

        internal ImageSource Image
        {
            get { return _image.Source; }
            set { _image.Source = value; }
        }

        internal string Text
        {
            get { return _text.Text; }
            set { _text.Text = value; }
        }
    }
}
