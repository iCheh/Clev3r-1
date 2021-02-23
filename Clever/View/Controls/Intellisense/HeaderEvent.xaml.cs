using System.Windows.Controls;

namespace Clever.View.Controls.Intellisense
{
    /// <summary>
    /// Логика взаимодействия для HeaderEvent.xaml
    /// </summary>
    public partial class HeaderEvent : UserControl
    {
        public HeaderEvent()
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
