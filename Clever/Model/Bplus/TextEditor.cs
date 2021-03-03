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

using Clever.Model.Utils;
using Clever.ViewModel;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace Clever.Model.Bplus
{
    public class TextEditor : IDisposable
    {
        private Scintilla textArea;
        private BpLexer lexer;
        private string filepath = "";
        public SearchManager searchManager = new SearchManager();
        public LineStack lineStack = new LineStack();
        public Popup Menu;
        internal Popup ToolsWindow;
        public TextEditor()
        {
            // BASIC CONFIG
            textArea = new Scintilla();
            textArea.Lexer = ScintillaNET.Lexer.Null;
            lexer = new BpLexer(this, textArea);
            textArea.Dock = DockStyle.Fill;
            textArea.BorderStyle = BorderStyle.None;
            textArea.UsePopup(false);
            ProgramName = "";

            // INITIAL VIEW CONFIG
            textArea.WrapMode = WrapMode.None;
            //textArea.VirtualSpaceOptions = VirtualSpace.RectangularSelection;

            textArea.IndentationGuides = MainWindowVM.ShowLine ? IndentView.LookBoth : IndentView.None;
            textArea.ViewWhitespace = MainWindowVM.ShowSpace ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
            textArea.WrapMode = MainWindowVM.ShowWrap ? WrapMode.Whitespace : WrapMode.None;

            //textArea.Margins[NUMBER_MARGIN].Width = MainWindowVM.ShowNumber ? Math.Max(50, 10 * (int)Math.Log10(textArea.Lines.Count)) : 0;
            textArea.Margins[NUMBER_MARGIN].Width = textArea.TextWidth(Style.LineNumber, new string('9', 4)) + 2;
            textArea.ScrollWidth = 1;
            textArea.WhitespaceSize = 2;
            textArea.Zoom = CommonData.Configurations.Get.Zoom;

            // STYLING
            InitColors();

            // NUMBER MARGIN
            InitNumberMargin();

            // CODE FOLDING MARGIN
            InitCodeFolding();

            // DRAG DROP
            InitDragDropFile();

            // INIT HOTKEYS
            InitHotkeys();

            // SEARCH
            searchManager.TextArea = textArea;

            textArea.CaretLineBackColor = System.Drawing.Color.FromArgb(0x00, 0xF8, 0xF8, 0xF8);
            textArea.CaretLineVisible = true;

            //CommonData.Status.Add(textArea.Zoom.ToString());
        }

        internal void EventsRemove()
        {
            lexer.EventsRemove();
            if (textArea != null)
            {
                textArea.PreviewKeyDown -= TextArea_PreviewKeyDown;
                textArea.KeyPress -= TextArea_KeyPress;
                textArea.MouseDown -= TextArea_MouseDown;
                textArea.ZoomChanged -= TextArea_ZoomChanged;
            }
        }

        internal void EventsAdd()
        {
            lexer.EventsAdd();
            textArea.PreviewKeyDown += TextArea_PreviewKeyDown;
            textArea.KeyPress += TextArea_KeyPress;
            textArea.MouseDown += TextArea_MouseDown;
            textArea.ZoomChanged += TextArea_ZoomChanged;
        }

        private void TextArea_ZoomChanged(object sender, EventArgs e)
        {
            var z = textArea.Zoom;
            //CommonData.Status.Add(z.ToString());
            CommonData.Configurations.Get.Zoom = z;
            var count = textArea.Lines.Count;
            textArea.Margins[TextEditor.NUMBER_MARGIN].Width = textArea.TextWidth(Style.LineNumber, new string('9', count.ToString().Length + 1)) + 2;
            textArea.Styles[Style.CallTip].Size = 8;
        }

        private void TextArea_KeyPress(object sender, KeyPressEventArgs e)  
        {
            if (e.KeyChar == 11 || e.KeyChar == 14 || e.KeyChar == 15 || e.KeyChar == 16 || e.KeyChar == 18 || e.KeyChar == 19 || e.KeyChar == 22)
            {
                e.Handled = true;
                return;
            }
        }

        private void TextArea_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            MainWindow.WindowForm_PreviewKeyDown(sender, e);
        }

        internal string ProgramName { get; set; }

        public Scintilla TextArea
        {
            get { return textArea; }
        }

        public BpLexer Lexer
        {
            get { return lexer; }
        }

        public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        /*////////
        public void ClearHighlights()
        {
            foreach (Line line in textArea.Lines)
            {
                line.MarkerDelete(HIGHLIGHT_MARKER);
            }
        }

        public void HighlightLine(Line line)
        {
            Marker marker = textArea.Markers[TextEditor.HIGHLIGHT_MARKER];
            marker.Symbol = MarkerSymbol.Background;
            marker.SetBackColor(IntToColor(BpColors.DEBUG_HIGHLIGHT_COLOR));
            line.MarkerAdd(HIGHLIGHT_MARKER);
        }
        *////////
        private void InitColors()
        {
            textArea.SetSelectionBackColor(true, BpColors.Select_Color);
        }

        private void InitHotkeys()
        {
            var context = MainWindow.GetContext.DataContext as MainWindowVM;

            HotKeyManager.AddHotKey(textArea, ClearSelection, Keys.Escape);
            HotKeyManager.AddHotKey(textArea, GoBackwards, Keys.B, true);
            HotKeyManager.AddHotKey(textArea, GoForwards, Keys.B, true, true);
            HotKeyManager.AddHotKey(textArea, Lexer.Format, Keys.F, true);
            HotKeyManager.AddHotKey(textArea, SelectAll, Keys.A, true);
            HotKeyManager.AddHotKey(textArea, CommentFile, Keys.Q, true);
            HotKeyManager.AddHotKey(textArea, UnCommentFile, Keys.W, true);
            HotKeyManager.AddHotKey(textArea, FoldAll, Keys.G, true);
            HotKeyManager.AddHotKey(textArea, context.FileNew, Keys.N, true);
            HotKeyManager.AddHotKey(textArea, context.FileOpen, Keys.O, true);
            HotKeyManager.AddHotKey(textArea, context.FileSave, Keys.S, true);

            HotKeyManager.AddHotKey(textArea, lexer.SetSnippets, Keys.Enter, true);

            // remove conflicting hotkeys from scintilla
            textArea.ClearCmdKey(Keys.Control | Keys.N);
            textArea.ClearCmdKey(Keys.Control | Keys.O);
            textArea.ClearCmdKey(Keys.Control | Keys.S);
            textArea.ClearCmdKey(Keys.Control | Keys.F);
            textArea.ClearCmdKey(Keys.Control | Keys.H);
            textArea.ClearCmdKey(Keys.Control | Keys.Shift | Keys.W);
            textArea.ClearCmdKey(Keys.Control | Keys.W);
            textArea.ClearCmdKey(Keys.Escape);
            textArea.ClearCmdKey(Keys.Control | Keys.B);
            textArea.ClearCmdKey(Keys.Control | Keys.Shift | Keys.B);
            textArea.ClearCmdKey(Keys.Control | Keys.E);
            textArea.ClearCmdKey(Keys.Control | Keys.G);
            textArea.ClearCmdKey(Keys.Control | Keys.A);
            textArea.ClearCmdKey(Keys.Control | Keys.Q);
            textArea.ClearCmdKey(Keys.Control | Keys.U);
            textArea.ClearCmdKey(Keys.Control | Keys.Enter);
        }

        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        public const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        ////////public const int HIGHLIGHT_MARKER = 0;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 2;

        private void InitNumberMargin()
        {
            var nums = textArea.Margins[NUMBER_MARGIN];
            nums.Width = MainWindowVM.ShowNumber ? 50 : 0;
            nums.Type = MarginType.Number;
            nums.Sensitive = false;
            nums.Mask = 0;
        }

        private void InitCodeFolding()
        {
            textArea.SetFoldMarginColor(true, BpColors.Back_Folding_Color);
            textArea.SetFoldMarginHighlightColor(true, BpColors.Back_Folding_Color);
            textArea.FoldDisplayTextSetStyle(FoldDisplayText.Boxed);

            // Enable code folding
            textArea.SetProperty("fold", "1");
            textArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            textArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            textArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            textArea.Margins[FOLDING_MARGIN].Sensitive = true;
            textArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                textArea.Markers[i].SetForeColor(BpColors.Back_Folding_Color); // styles for [+] and [-]
                textArea.Markers[i].SetBackColor(BpColors.Fore_Folding_Color); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            textArea.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            textArea.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            textArea.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            textArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            textArea.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            textArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            textArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            //textArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void TextArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lineStack.bActive = true;
                Menu.IsOpen = true;
            }
        }

        #endregion

        #region Drag & Drop File

        public void InitDragDropFile()
        {
            textArea.AllowDrop = true;
            textArea.DragEnter += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            textArea.DragDrop += delegate (object sender, DragEventArgs e)
            {
                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {
                        string path = a.GetValue(0).ToString();
                        MainWindowVM.FileNew(path, false);
                    }
                }
            };
        }

        #endregion

        #region Selection Copy Cut Paste

        public void Cut()
        {
            textArea.Cut();
        }

        public void Copy()
        {
            textArea.Copy();
        }

        public void Paste()
        {
            System.Windows.Input.Cursor cursor = System.Windows.Input.Mouse.OverrideCursor;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            textArea.Paste();

            System.Windows.Input.Mouse.OverrideCursor = cursor;
        }

        public void Delete()
        {
            textArea.DeleteRange(textArea.SelectionStart, textArea.SelectionEnd - textArea.SelectionStart);
        }

        public void Undo()
        {
            textArea.Undo();
        }

        public void Redo()
        {
            textArea.Redo();
        }

        public void SelectAll()
        {
            textArea.SelectAll();
        }

        public void SelectLine()
        {
            Line line = textArea.Lines[textArea.CurrentLine];
            textArea.SetSelection(line.Position, line.Position + line.Length);
            textArea.ScrollCaret();
        }

        public void SelectLine(int iLine)
        {
            Line line = textArea.Lines[iLine];
            textArea.SetSelection(line.Position, line.Position + line.Length);
            textArea.ScrollCaret();
        }

        public void SetEmptySelection()
        {
            textArea.SetEmptySelection(0);
        }

        #endregion

        #region Wrap and Fold

        public WrapMode WrapMode
        {
            set { textArea.WrapMode = value; }
        }

        public IndentView IndentationGuides
        {
            set { textArea.IndentationGuides = value; }
        }

        public WhitespaceMode ViewWhitespace
        {
            set { textArea.ViewWhitespace = value; }
        }

        public void FoldAll()
        {
            foreach (var line in textArea.Lines)
            {
                if (line.FoldLevelFlags == FoldLevelFlags.Header)
                {
                    Lexer.FoldLine(line.Position);
                }
            }
        }

        private void CommentFile()
        {
            Comment(true);
        }

        private void UnCommentFile()
        {
            Comment(false);
        }
        public void Comment(bool bComment)
        {
            int lineA = textArea.LineFromPosition(textArea.SelectionStart);
            int lineB = textArea.LineFromPosition(textArea.SelectionEnd);
            if (lineB < lineA)
            {
                int iTemp = lineA;
                lineA = lineB;
                lineB = iTemp;
            }
            if (lineB > lineA && textArea.SelectionEnd == textArea.Lines[lineB - 1].EndPosition) lineB--;
            int iStart = textArea.Lines[lineA].Position;
            int iEnd = textArea.Lines[lineB].EndPosition;

            string selected = "";
            for (int i = lineA; i <= lineB; i++)
            {
                Line line = textArea.Lines[i];
                string text = line.Text;
                int pos = text.TakeWhile(c => char.IsWhiteSpace(c)).Count();
                if (pos < text.Length)
                {

                    if (bComment)
                    {
                        text = text.Insert(pos, "'");
                    }

                    else if (!bComment && text[pos] == '\'')
                    {
                        text = text.Remove(pos, 1);
                    }
                }
                selected += text;
            }

            textArea.SetTargetRange(iStart, iEnd);
            textArea.ReplaceTarget(selected);
        }

        #endregion

        #region Zoom

        public void ClearSelection()
        {
            textArea.ClearSelections();
        }

        public void GoBackwards()
        {
            int iLine = lineStack.GetBackwards();
            if (iLine < 0 || iLine >= textArea.Lines.Count) return;
            textArea.GotoPosition(textArea.Lines[iLine].Position);
            SelectLine();
        }

        public void GoForwards()
        {
            int iLine = lineStack.GetForwards();
            if (iLine < 0 || iLine >= textArea.Lines.Count) return;
            textArea.GotoPosition(textArea.Lines[iLine].Position);
            SelectLine();
        }

        #endregion       

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //this.EventsRemove();
                lexer.Dispose();
                try
                {
                    if (textArea != null)
                    {
                        textArea.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    CommonData.Status.Clear();
                    CommonData.Status.Add(ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
    }

    public class LineStack
    {
        public int MaxItems { get; set; } = 100;

        public List<int> backwards = new List<int>();
        public List<int> forwards = new List<int>();
        public bool bActive = false;

        public void PushBackwards(int iLine)
        {
            if (bActive) return;
            if (backwards.Count > 0 && iLine == backwards[backwards.Count - 1]) return;
            backwards.Add(iLine);
            while (backwards.Count > MaxItems) backwards.RemoveAt(0);
        }

        public void PushForwards(int iLine)
        {
            if (forwards.Count > 0 && iLine == forwards[0]) return;
            forwards.Insert(0, iLine);
            while (forwards.Count > MaxItems) forwards.RemoveAt(forwards.Count - 1);
        }

        public int GetBackwards()
        {
            if (backwards.Count < 2) return -1;
            PushForwards(backwards[backwards.Count - 1]);
            backwards.RemoveAt(backwards.Count - 1);
            return backwards[backwards.Count - 1];
        }

        public int GetForwards()
        {
            if (forwards.Count == 0) return -1;
            int iLine = forwards[0];
            forwards.RemoveAt(0);
            return iLine;
        }
    }
}
