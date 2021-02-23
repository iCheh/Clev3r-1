using Clever.View.Controls.Editors;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Clever.ViewModel.PanelsVM;
using TabControl = System.Windows.Controls.TabControl;

namespace Clever.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для EditorPanel.xaml
    /// </summary>
    public partial class EditorPanel : System.Windows.Controls.UserControl
    {
        private static TabControl _tabControl;
        static Popup _popup;

        public EditorPanel()
        {
            InitializeComponent();
            _tabControl = EditorTabControl;
            _popup = PopUp;
        }

        public static TabItem GetSelectedItem
        {
            get
            {
                if (_tabControl.Items.Count > 0)
                {
                    return (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                }
                return null;
            }
        }

        public static TabControl GetTabControl
        {
            get { return _tabControl; }
        }

        public static string SelectedItemHeaderName
        {
            get
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    return h.HeaderName;
                }
                return null;
            }
            set
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    h.HeaderName = value;
                }
            }
        }

        public static Brush SelectedItemHeaderChangeColor
        {
            get
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    return h.HeaderColor;
                }
                return null;
            }
            set
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    h.HeaderColor = value;
                }
            }
        }

        public static string SelectedItemHeaderChange
        {
            get
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    return h.HeaderChange;
                }
                return null;
            }
            set
            {
                if (_tabControl.Items.Count > 0)
                {
                    var ti = (TabItem)_tabControl.Items[_tabControl.SelectedIndex];
                    var h = (EditorHeader)ti.Header;
                    h.HeaderChange = value;
                }
            }
        }

        public static void AddItem(TabItem item)
        {
            _tabControl.Items.Add(item);
            _tabControl.SelectedIndex = _tabControl.Items.Count - 1;
        }

        public static Popup GetPopup
        {
            get { return _popup; }
        }
    }
}
