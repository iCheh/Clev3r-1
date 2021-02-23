using System.Windows.Controls;

namespace Clever.View.Controls.Intellisense
{
    /// <summary>
    /// Логика взаимодействия для HeaderObject.xaml
    /// </summary>
    public partial class HeaderObject : UserControl
    {
        public HeaderObject()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return hoText.Text; }
            set { hoText.Text = value; }
        }
    }
}
