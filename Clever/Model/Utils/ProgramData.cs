using Clever.Model.Bplus;
using Clever.Model.Bplus.BPInterpreter;
using Clever.View.Controls.Editors;
using Clever.Model.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Clever.ViewModel;
using Label = Clever.Model.Bplus.BPInterpreter.Label;
using Clever.Model.Intellisense;
using System.Diagnostics;
using Clever.Model.Program;

namespace Clever.Model.Utils
{
    class ProgramData
    {
        private Elements elements;       

        internal List<string> Subroutines = new List<string>();
        internal ProgramMap Map { get; set; }
        internal bool AddToObjects = true;
        public ProgramData()
        {
            ClosedName = "";
            Map = new ProgramMap(BPType.PROGRAM);
            elements = new Elements();
            Path = "";
            FullPath = "";
            ToolTip = "";
            OldName = "";
        }

        internal string ParseName { get; set; }

        internal string ClosedName { get; set; }
        public string Name
        {
            get
            {
                if (header != null)
                    return header.HeaderName;
                else
                    return ClosedName;
            }
            set
            {
                if (header != null)
                {
                    header.HeaderName = value;
                    ParseName = value;
                    if (value != OldName)
                    {
                        if (AddToObjects && IntellisenseParser.Get.Data.ContainsKey(OldName))
                        {
                            IntellisenseParser.Get.Data.Remove(OldName);
                        }
                        
                        if (AddToObjects && !IntellisenseParser.Get.Data.ContainsKey(value))
                        {
                            IntellisenseParser.Get.Data.Add(value, this);
                        }

                        OldName = value;
                    }

                    if (Editor != null)
                    {
                        Editor.ProgramName = value;
                    }

                    Map.Type = BPType.PROGRAM;

                    if (value.IndexOf(".bpi") != -1)
                    {
                        Map.Type = BPType.INCLUDE;
                    }
                    else if (value.IndexOf(".bpm") != -1)
                    {
                        Map.Type = BPType.MODULE;
                    }
                }
                else
                {
                    ClosedName = value;

                    Map.Type = BPType.PROGRAM;

                    if (value.IndexOf(".bpi") != -1)
                    {
                        Map.Type = BPType.INCLUDE;
                    }
                    else if (value.IndexOf(".bpm") != -1)
                    {
                        Map.Type = BPType.MODULE;
                    }
                }
            }
        }
        internal string OldName { get; set; }
        public string Path { get; set; }


        private string _fullPath;
        public string FullPath
        {
            get
            {
                return _fullPath;
            }
            set
            {
                _fullPath = value;
                if (_fullPath != "")
                {
                    ToolTip = _fullPath;
                    if (header != null)
                    {
                        header.MyToolTip = ToolTip;
                    }
                }
            }
        }

        internal string ToolTip { get; set; }
        public TextEditor Editor { get; set; }
        private EditorHeader header { get; set; }
        public Popup Menu { get; set; }

        private TabItem _item;
        public TabItem Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                if (_item != null)
                {                    
                    header = _item.Header as EditorHeader;
                    Editor editor = _item.Content as Editor;
                    Editor = editor.GetTextEditor;
                    Name = elements.GetTabItemHeaderName(_item);
                    Editor.ProgramName = Name;
                    Menu = editor.GetTextEditorPopup;
                    TextChange = false;

                    if (AddToObjects && !IntellisenseParser.Get.Data.ContainsKey(Name))
                        IntellisenseParser.Get.Data.Add(Name, this);
                }
            }
        }

        private bool _textChange = false;
        public bool TextChange
        {
            get
            {
                return _textChange;
            }
            set
            {
                _textChange = value;
                if (_textChange)
                {
                    SetChangeToHeaderYes();
                    if (AddToObjects)
                    {
                        IntellisenseParser.Get.UpdateMap(this);
                    }  
                }                   
                else
                    SetChangeToHeaderNo();
            }
        }

        public string GetHeaderChangeText
        {
            get { return header.HeaderChange; }
            set { header.HeaderChange = value; }
        }

        private void SetChangeToHeaderYes()
        {
            header.HeaderChange = "●";
            SetHeaderColorText();
        }

        private void SetChangeToHeaderNo()
        {
            header.HeaderChange = "";
            SetHeaderColorText();
        }

        public void SetHeaderColorText()
        {
            if (_item != null)
            {
                if (_item.IsSelected)
                {
                    header.HeaderColor = new SolidColorBrush(Color.FromRgb(0xEE, 0x37, 0x24));
                }
                else
                {
                    header.HeaderColor = new SolidColorBrush(Color.FromRgb(0x60, 0x69, 0x85));
                }
            }
        }
    }
}
