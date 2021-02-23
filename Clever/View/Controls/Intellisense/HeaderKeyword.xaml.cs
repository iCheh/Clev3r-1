using System.Windows.Controls;

namespace Clever.View.Controls.Intellisense
{
    /// <summary>
    /// Логика взаимодействия для HeaderKeyWord.xaml
    /// </summary>
    public partial class HeaderKeyword : UserControl
    {
        public HeaderKeyword()
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
