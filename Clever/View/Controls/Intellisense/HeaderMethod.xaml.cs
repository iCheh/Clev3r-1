using System.Windows.Controls;

namespace Clever.View.Controls.Intellisense
{
    /// <summary>
    /// Логика взаимодействия для HeaderMethod.xaml
    /// </summary>
    public partial class HeaderMethod : UserControl
    {
        public HeaderMethod()
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
