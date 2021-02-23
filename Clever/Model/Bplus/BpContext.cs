//The following Copyright applies to SB-Prime for Small Basic and files in the namespace EV3BasicPlus. 
//Copyright (C) <2017> litdev@hotmail.co.uk 
//This file is part of SB-Prime for Small Basic. 

//SB-Prime for Small Basic is free software: you can redistribute it and/or modify 
//it under the terms of the GNU General Public License as published by 
//the Free Software Foundation, either version 3 of the License, or 
//(at your option) any later version. 

//SB-Prime for Small Basic is distributed in the hope that it will be useful, 
//but WITHOUT ANY WARRANTY; without even the implied warranty of 
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
//GNU General Public License for more details.  

//You should have received a copy of the GNU General Public License 
//along with SB-Prime for Small Basic.  If not, see <http://www.gnu.org/licenses/>. 

using ScintillaNET;
using System.Windows.Forms;

namespace Clever.Model.Bplus
{
    public class BpContext
    {
        private TextEditor sbDocument;
        private Scintilla textArea;

        public BpContext(TextEditor sbDocument)
        {
            this.sbDocument = sbDocument;
            textArea = sbDocument.TextArea;
        }

        public void SetMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Closed += new ToolStripDropDownClosedEventHandler(OnClosed);
            textArea.ContextMenuStrip = menu;

            menu.Items.Add(new ToolStripMenuItem("Отменить Ctrl+Z", null, (s, ea) => textArea.Undo()) { Enabled = textArea.CanUndo });
            menu.Items.Add(new ToolStripMenuItem("Повторить Ctrl+Y", null, (s, ea) => textArea.Redo()) { Enabled = textArea.CanRedo });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Вырезать Ctrl+X", null, (s, ea) => textArea.Cut()) { Enabled = textArea.SelectedText.Length > 0 });
            menu.Items.Add(new ToolStripMenuItem("Копировать Ctrl+C", null, (s, ea) => textArea.Copy()) { Enabled = textArea.SelectedText.Length > 0 });
            menu.Items.Add(new ToolStripMenuItem("Вставить Ctrl+V", null, (s, ea) => textArea.Paste()) { Enabled = textArea.CanPaste });
            menu.Items.Add(new ToolStripMenuItem("Удалить", null, (s, ea) => textArea.DeleteRange(textArea.SelectionStart, textArea.SelectionEnd - textArea.SelectionStart)) { Enabled = textArea.SelectedText.Length > 0 });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Выбрать всё Ctrl+A", null, (s, ea) => textArea.SelectAll()));
            //menu.Items.Add(new ToolStripSeparator());
            //menu.Items.Add(new ToolStripMenuItem("Поиск Ctrl+F", null, OpenFindDialog) { Enabled = null != sbDocument.Tab });
            //menu.Items.Add(new ToolStripMenuItem("Поиск и замена Ctrl+H", null, OpenReplaceDialog) { Enabled = null != sbDocument.Tab });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Закомментировать выбранное", null, (s, ea) => sbDocument.Comment(true)));
            menu.Items.Add(new ToolStripMenuItem("Раскомментировать выбранное", null, (s, ea) => sbDocument.Comment(false)));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Свернуть всё", null, (s, ea) => sbDocument.FoldAll()));
            menu.Items.Add(new ToolStripMenuItem("Развернуть всё", null, (s, ea) => sbDocument.FoldAll()));
            //menu.Items.Add(new ToolStripSeparator());
            //menu.Items.Add(new ToolStripMenuItem("Навигация назад Ctrl+B", null, (s, ea) => sbDocument.GoBackwards()) { Enabled = sbDocument.lineStack.backwards.Count > 1 });
            //menu.Items.Add(new ToolStripMenuItem("Навигация вперёд Ctrl+Shift+B", null, (s, ea) => sbDocument.GoForwards()) { Enabled = sbDocument.lineStack.forwards.Count > 0 });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Форматировать код", null, (s, ea) => sbDocument.Lexer.Format()));
        }

        private void OnClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            sbDocument.lineStack.bActive = false;
        }

        /*
        private void OpenFindDialog(object sender, EventArgs e)
        {
            if (textArea.SelectedText != "") MainWindow.THIS.tbFind.Text = textArea.SelectedText;
            MainWindow.THIS.tbFind.Focus();
            MainWindow.THIS.tbFind.SelectAll();
            MainWindow.THIS.FindNext();
        }

        private void OpenReplaceDialog(object sender, EventArgs e)
        {
            if (FindAndReplace.Active) return;

            FindAndReplace far = new FindAndReplace(MainWindow.THIS);
            far.Show();
        }
        */
        /*
        private void Insert(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string value = MainWindow.hexColors && null != menuItem.Tag ? menuItem.Tag.ToString() : menuItem.Text;
            if (MainWindow.quoteInserts)
                textArea.ReplaceSelection("\"" + value + "\"");
            else
                textArea.ReplaceSelection(value);
            textArea.SelectionStart = textArea.CurrentPosition;
            textArea.SelectionEnd = textArea.CurrentPosition;
        }
        */
    }
}
