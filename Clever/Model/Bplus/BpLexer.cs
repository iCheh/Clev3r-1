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

using Clever.Model.Bplus.BPInterpreter;
using Clever.Model.Utils;
using Clever.ViewModel;
using Clever.ViewModel.PanelsVM;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace Clever.Model.Bplus
{
    public class BpLexer : IDisposable
    {
        int ppp = 0;
        private TextEditor sbDocument;
        private Scintilla textArea;
        private int linesCount = 999;

        private int STYLE_UNKNOWN_NEW = 0;
        private int STYLE_COMMENT_NEW = 1;
        private int STYLE_STRING_NEW = 2;
        private int STYLE_OPERATOR_NEW = 3;
        private int STYLE_KEYWORD_1_NEW = 4;
        private int STYLE_KEYWORD_2_NEW = 5;
        private int STYLE_KEYWORD_3_NEW = 6;
        private int STYLE_KEYWORD_4_NEW = 7;
        private int STYLE_OBJECT_NEW = 8;
        private int STYLE_METHOD_NEW = 9;
        private int STYLE_VARIABLE_NEW = 10;
        private int STYLE_SUBROUTINE_NEW = 11;
        private int STYLE_LABEL_NEW = 12;
        private int STYLE_LITERAL_NEW = 13;
        private int STYLE_NUMBER_NEW = 14;
        private int STYLE_MODULE_NEW = 15;
        private int STYLE_REGION_OPEN = 16;
        private int STYLE_REGION_CLOSE = 17;

        private HashSet<string> _keywords1;
        private HashSet<string> _keywords2;
        private HashSet<string> _keywords3;
        private HashSet<string> _keywords4;
        private HashSet<string> _bpObject;
        //private HashSet<string> _keywords;

        private Dictionary<int, int> _indent;

        int LastLineCount = 0;
        string lastObject = "";
        int AutoCMode = 0;
        string AutoCData = "";
        string spaces = "";
        Timer AutoCTimer;
        internal BpObjects bpObjects = new BpObjects();
        internal int toolTipPosition = 0;
        string keywords = "Sub|EndSub|For|To|Step|EndFor|If|Then|Else|ElseIf|EndIf|While|EndWhile|Goto|include|import|Or|And|folder|Function|EndFunction|in|out|number|string";
        Regex keyword1 = new Regex("^[\\W](IF|SUB|WHILE|FOR|FUNCTION)[\\W]");
        Regex keyword2 = new Regex("^[\\W](ENDIF|ENDSUB|ENDWHILE|ENDFOR|ENDFUNCTION)[\\W]");
        Regex keyword3 = new Regex("^[\\W](ELSE|ELSEIF)[\\W]");
        Regex keyword4 = new Regex("^[\\W](#REGION)[\\W]");
        Regex keyword5 = new Regex("^[\\W](#ENDREGION)[\\W]");
        int pos = 0;
        
        internal BpLexer(TextEditor sbDocument, Scintilla textArea)
        {
            _indent = new Dictionary<int, int>();

            this.sbDocument = sbDocument;
            this.textArea = textArea;
            SetMethods();

            // STYLING
            InitSyntaxColoring();
            InitAutoComplete();
        }

        internal void EventsRemove()
        {
            //MessageBox.Show("20");
            if (textArea != null)
            {
                textArea.CharAdded -= (this.OnCharAdded);
                textArea.StyleNeeded -= OnStyleNeeded;
                textArea.TextChanged -= OnTextChanged;
                textArea.DwellStart -= OnDwellStart;
                textArea.DwellEnd -= OnDwellEnd;
                textArea.AutoCSelection -= OnAutoCSelection;
                textArea.AutoCCompleted -= OnAutoCCompleted;
                textArea.UpdateUI -= OnUpdateUI;
                textArea.MouseDoubleClick -= TextArea_MouseDoubleClick;
                textArea.MarginClick -= TextArea_MarginClick;
                textArea.MouseClick -= TextArea_MouseClick;
            }
        }

        internal void EventsAdd()
        {
            textArea.CharAdded += (this.OnCharAdded);
            textArea.StyleNeeded += OnStyleNeeded;
            textArea.TextChanged += OnTextChanged;
            textArea.MouseDwellTime = 100;
            textArea.DwellStart += OnDwellStart;
            textArea.DwellEnd += OnDwellEnd;
            textArea.AutoCSelection += OnAutoCSelection;
            textArea.AutoCCompleted += OnAutoCCompleted;
            textArea.UpdateUI += OnUpdateUI;
            textArea.MouseDoubleClick += TextArea_MouseDoubleClick;
            textArea.MarginClick += TextArea_MarginClick;
            textArea.MouseClick += TextArea_MouseClick;

            AutoCTimer = new Timer();
            AutoCTimer.Enabled = false;
            AutoCTimer.Interval = 100;
            AutoCTimer.Tick += new EventHandler(AutoCTimerCallback);
        }

        private void TextArea_MouseClick(object sender, MouseEventArgs e)
        {
            HelperPanelVM.Get.OpenHelp();
        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            var line = textArea.LineFromPosition(e.Position);
            var text = textArea.Lines[line].Text;
            var start = textArea.Lines[line].Position;
            var end = textArea.Lines[line].EndPosition;

            if (text.Trim().ToLower().IndexOf("#region") == 0)
            {
                var tmpText = " " + text.Trim().Replace("#region ", "").Replace("#", "") + " ";
                if (tmpText.Trim() == "")
                {
                    tmpText = " Region ";
                }

                if (textArea.Lines[line].Expanded)
                {
                    textArea.StartStyling(start);
                    textArea.SetStyling(end - start, STYLE_REGION_CLOSE);
                }
                else
                {
                    textArea.StartStyling(start);
                    textArea.SetStyling(end - start, STYLE_REGION_OPEN);
                }

                textArea.Lines[line].ToggleFoldShowText(tmpText);
            }
            else
            {
                textArea.Lines[line].ToggleFoldShowText("...");
            }
        }

        private void OnUpdateUI(object sender, UpdateUIEventArgs e)
        {
            pos = textArea.CurrentPosition;
            if ((e.Change & UpdateChange.Selection) > 0)
            {
                if (MainWindowVM.highlightAll) sbDocument.searchManager.HighLight(textArea.SelectedText);
            }
        }

        private void InitSyntaxColoring()
        {
            Color foreColor = BpColors.Fore_Color;
            Color backColor = BpColors.Back_Color;

            textArea.StyleResetDefault();
            
            textArea.Styles[Style.Default].Font = "Consolas";
            textArea.Styles[Style.Default].Size = 12;
            textArea.Styles[Style.Default].BackColor = backColor;
            textArea.Styles[Style.Default].ForeColor = foreColor;
            textArea.Styles[Style.LineNumber].Font = "Consolas";
            textArea.Styles[Style.LineNumber].Bold = false;
            textArea.Styles[Style.LineNumber].Italic = false;
            textArea.Styles[Style.LineNumber].Size = 12;
            textArea.Styles[Style.CallTip].Font = "Consolas";
            textArea.Styles[Style.CallTip].Size = 8;

            textArea.CaretForeColor = foreColor;
            textArea.TabWidth = 2;
            spaces = "";

            for (int i = 0; i < textArea.TabWidth; i++) spaces += " ";
            textArea.StyleClearAll();

            textArea.Styles[Style.LineNumber].ForeColor = BpColors.Fore_Margin_Color;
            textArea.Styles[Style.LineNumber].BackColor = BpColors.Back_Margin_Color;
            textArea.Styles[Style.IndentGuide].ForeColor = BpColors.Fore_Folding_Color;
            textArea.Styles[Style.IndentGuide].BackColor = BpColors.Back_Folding_Color;

            textArea.Styles[STYLE_UNKNOWN_NEW].ForeColor = BpColors.Fore_Color;
            textArea.Styles[STYLE_COMMENT_NEW].ForeColor = BpColors.Comment_Color;
            textArea.Styles[STYLE_STRING_NEW].ForeColor = BpColors.String_Color;
            textArea.Styles[STYLE_OPERATOR_NEW].ForeColor = BpColors.Operator_Color;
            textArea.Styles[STYLE_KEYWORD_1_NEW].ForeColor = BpColors.Keyword_1_Color;
            textArea.Styles[STYLE_KEYWORD_2_NEW].ForeColor = BpColors.Keyword_2_Color;
            textArea.Styles[STYLE_KEYWORD_3_NEW].ForeColor = BpColors.Keyword_3_Color;
            textArea.Styles[STYLE_KEYWORD_4_NEW].ForeColor = BpColors.Keyword_4_Color;
            textArea.Styles[STYLE_OBJECT_NEW].ForeColor = BpColors.Object_Color;
            textArea.Styles[STYLE_METHOD_NEW].ForeColor = BpColors.Method_Color;
            textArea.Styles[STYLE_SUBROUTINE_NEW].ForeColor = BpColors.Sub_Color;
            textArea.Styles[STYLE_LABEL_NEW].ForeColor = BpColors.Label_Color;
            textArea.Styles[STYLE_VARIABLE_NEW].ForeColor = BpColors.Var_Color;
            textArea.Styles[STYLE_LITERAL_NEW].ForeColor = BpColors.Literal_Color;
            textArea.Styles[STYLE_NUMBER_NEW].ForeColor = BpColors.Number_Color;
            textArea.Styles[STYLE_MODULE_NEW].ForeColor = BpColors.Module_Color;
            textArea.Styles[STYLE_REGION_OPEN].ForeColor = BpColors.Region_Open_Color;
            textArea.Styles[STYLE_REGION_CLOSE].ForeColor = BpColors.Region_Close_Color;

            textArea.Styles[STYLE_REGION_CLOSE].Visible = false;

            textArea.Styles[STYLE_COMMENT_NEW].Italic = true;
            textArea.Styles[STYLE_KEYWORD_1_NEW].Bold = true;
            textArea.Styles[STYLE_KEYWORD_2_NEW].Bold = true;
            textArea.Styles[STYLE_KEYWORD_3_NEW].Bold = true;
            textArea.Styles[STYLE_KEYWORD_4_NEW].Bold = true;
            textArea.Styles[STYLE_SUBROUTINE_NEW].Bold = true;
            textArea.Styles[STYLE_LABEL_NEW].Bold = true;

            textArea.Styles[STYLE_OBJECT_NEW].Bold = true;
            textArea.Styles[STYLE_METHOD_NEW].Bold = true;
            textArea.Styles[STYLE_MODULE_NEW].Bold = true;

            _keywords1 = new HashSet<string>() { "SUB", "ENDSUB", "FUNCTION", "ENDFUNCTION", "GOTO", "IN", "OUT" };
            _keywords2 = new HashSet<string>() { "NUMBER", "STRING", "NUMBER[]", "STRING[]" };
            _keywords3 = new HashSet<string>() { "FOLDER", "INCLUDE", "IMPORT", "PRIVATE" };
            _keywords4 = new HashSet<string>() { "IF", "THEN", "ELSE", "ELSEIF", "ENDIF", "FOR", "TO", "STEP", "ENDFOR", "OR", "AND", "WHILE", "ENDWHILE" };

            // Configure the lexer styles
            textArea.Lexer = Lexer.Container;

            const int SCI_CALLTIPSETBACK = 2205;
            const int SCI_CALLTIPSETFORE = 2206;
            textArea.DirectMessage(SCI_CALLTIPSETBACK, new IntPtr(ColorTranslator.ToWin32(BpColors.Back_Calltip_Color)), IntPtr.Zero);
            textArea.DirectMessage(SCI_CALLTIPSETFORE, new IntPtr(ColorTranslator.ToWin32(BpColors.Fore_Calltip_Color)), IntPtr.Zero);
            
            textArea.Styles[Style.CallTip].BackColor = BpColors.Back_Calltip_Color;
            textArea.Styles[Style.CallTip].ForeColor = BpColors.Fore_Calltip_Color;
            textArea.CallTipSetForeHlt(BpColors.Fore_Calltip_Color);
        }

        private void InitAutoComplete()
        {
            textArea.AutoCIgnoreCase = true;
            textArea.AutoCMaxHeight = 10;
            textArea.AutoCMaxWidth = 0;
            textArea.AutoCOrder = Order.Presorted;
            textArea.AutoCAutoHide = true;
            textArea.AutoCCancelAtStart = true;
            textArea.AutoCChooseSingle = false;
            textArea.AutoCDropRestOfWord = false;

            textArea.RegisterRgbaImage(0, new Bitmap(Properties.Resources.IntellisenseKeyword));
            textArea.RegisterRgbaImage(1, new Bitmap(Properties.Resources.IntellisenseObject));
            textArea.RegisterRgbaImage(2, new Bitmap(Properties.Resources.IntellisenseMethod));
            textArea.RegisterRgbaImage(3, new Bitmap(Properties.Resources.IntellisenseProperty));
            textArea.RegisterRgbaImage(4, new Bitmap(Properties.Resources.IntellisenseEvent));
            textArea.RegisterRgbaImage(5, new Bitmap(Properties.Resources.IntellisenseVariable));
            textArea.RegisterRgbaImage(6, new Bitmap(Properties.Resources.IntellisenseSubroutine));
            textArea.RegisterRgbaImage(7, new Bitmap(Properties.Resources.IntellisenseLabel));
            textArea.RegisterRgbaImage(8, new Bitmap(Properties.Resources.IntellisenseObject_yellow));
        }

        private void OnCharAdded(object sender, CharAddedEventArgs e)
        {
            // Auto Indent
            if (AutoCMode == 0)
                HelperPanelVM.Get.OpenHelp();

            if (e.Char == '\n')
            {
                int foldBase = textArea.Lines[0].FoldLevel;
                int lineCur = textArea.CurrentLine;
                int foldCur = _indent[lineCur] - foldBase;
                int foldPrev = _indent[lineCur - 1] - foldBase;               

                string indents = "";

                for (int i = 0; i < foldCur; i++)
                {
                    indents += spaces;
                }
                    
                textArea.AddText(indents);

                indents = "";

                for (int i = 0; i < foldPrev; i++)
                {
                    indents += spaces;
                }
                    
                int iStart = textArea.Lines[lineCur - 1].Position;
                string linePrev = textArea.Lines[lineCur - 1].Text;
                int iLen = 0;
                while (iLen < linePrev.Length && char.IsWhiteSpace(linePrev[iLen]) && linePrev[iLen] != '\r' && linePrev[iLen] != '\n')
                {
                    iLen++;
                }
                textArea.SetTargetRange(iStart, iStart + iLen);
                textArea.ReplaceTarget(indents); 
            }
            else if (e.Char == '(')
            {
                textArea.AddText(")");
                textArea.GotoPosition(textArea.CurrentPosition - 1);
            }
            else if (e.Char == '[')
            {
                textArea.AddText("]");
                textArea.GotoPosition(textArea.CurrentPosition - 1);
            }
            else if (e.Char == '\"')
            {
                textArea.AddText("\"");
                textArea.GotoPosition(textArea.CurrentPosition - 1);
            }

            // Display the autocompletion list
            int currentPos = textArea.CurrentPosition;
            var currentLine = textArea.Lines[textArea.LineFromPosition(currentPos)];
            var tmpLineText = currentLine.Text;
            var isComment = false;
            var isString = false;
            
            if (!string.IsNullOrEmpty(tmpLineText))
            {
                tmpLineText = tmpLineText.Substring(0, currentPos - currentLine.Position);
                foreach (var ch in tmpLineText)
                {
                    if (ch == '\'')
                    {
                        isComment = true;
                        break;
                    }
                    else if (ch == '"')
                    {
                        isString = !isString;
                    }
                }
            }
            

            if (isString || isComment)
            {
                return;
            }

            int wordStartPos = textArea.WordStartPosition(currentPos, true);
            string currentWord = textArea.GetWordFromPosition(wordStartPos);
            int lenEntered = currentPos - wordStartPos;
            textArea.AutoCSetFillUps("");
            textArea.AutoCStops("");

            if (wordStartPos > 1 && textArea.GetCharAt(wordStartPos - 1) == '.') //method
            {
                textArea.AutoCSetFillUps("(");
                textArea.AutoCStops(" ");
                currentPos = wordStartPos - 2;
                wordStartPos = textArea.WordStartPosition(currentPos, true);
                lastObject = textArea.GetWordFromPosition(wordStartPos);
                if (_bpObject.Contains(lastObject.ToLower()))
                {
                    AutoCData = bpObjects.GetMembers(lastObject, currentWord).Trim();
                }
                else
                {
                    AutoCData = bpObjects.GetModuleMembers(lastObject, currentWord, sbDocument.ProgramName).Trim();
                }
                textArea.AutoCShow(lenEntered, AutoCData);
                AutoCMode = 2;
                AutoCTimer.Enabled = true;
            }
            else if (lenEntered > 0)
            {
                BpObjects.UpdateWork = true;

                int index = textArea.Lines[textArea.CurrentLine].DisplayIndex;
                string name = sbDocument.ProgramName;
                
                var tmpVariables = bpObjects.GetVariables(currentWord, index, name);
                var tmpLabels = bpObjects.GetLabels(currentWord, index, name);
                var tmpModules = bpObjects.GetModuleObjects(currentWord, name);
                var tmpSubs = bpObjects.GetSubroutines(currentWord, name);

                BpObjects.UpdateWork = false;

                textArea.AutoCSetFillUps(".");
                textArea.AutoCStops(" ");
                AutoCData = (bpObjects.GetKeywords(currentWord) + bpObjects.GetObjects(currentWord) + tmpModules + tmpSubs + tmpVariables + tmpLabels).Trim();
                textArea.AutoCShow(lenEntered, AutoCData);

                lastObject = "";
                AutoCMode = 1;
                AutoCTimer.Enabled = true;
            }
        }

        private void OnStyleNeeded(object sender, StyleNeededEventArgs e)
        {
            int startPos = textArea.GetEndStyled();
            int endPos = e.Position;
            SetStyle(startPos, endPos, textArea);
        }

        internal void OnTextChanged(object sender, EventArgs e)
        {
            //HelpPanelVM.Get.OpenHelp();
            TextEditorEvents.Editor_Change(sender, new System.Windows.RoutedEventArgs());
            if (textArea.Lines.Count != LastLineCount)
            {
                SetFolding();
            }

            //textArea.Margins[TextEditor.NUMBER_MARGIN].Width = MainWindowVM.ShowNumber ? Math.Max(50, 10 * (int)Math.Log10(textArea.Lines.Count)) : 0;
            var count = textArea.Lines.Count;
            if (count > 1000 && count > linesCount)
            {
                textArea.Margins[TextEditor.NUMBER_MARGIN].Width = textArea.TextWidth(Style.LineNumber, new string('9', count.ToString().Length + 1)) + 2;
                linesCount = count;
            }
            
            //TextEditorEvents.Editor_Change(sender, new System.Windows.RoutedEventArgs());
        }

        public void Format()
        {
            System.Windows.Input.Cursor cursor = System.Windows.Input.Mouse.OverrideCursor;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            //textArea.BeginUndoAction();

            for (int i = 0; i < textArea.Lines.Count; i++)
            {
                string text = textArea.Lines[i].Text.TrimEnd();

                if (i < textArea.Lines.Count - 1)
                {
                    text += "\r\n";
                }
                    
                if (text != textArea.Lines[i].Text)
                {
                    textArea.SetTargetRange(textArea.Lines[i].Position, textArea.Lines[i].EndPosition);
                    textArea.ReplaceTarget(text);
                }
            }

            for (int i = textArea.Lines.Count - 1; i > 0; i--)
            {
                if (string.IsNullOrWhiteSpace(textArea.Lines[i].Text) && string.IsNullOrWhiteSpace(textArea.Lines[i - 1].Text))
                {
                    textArea.SetTargetRange(textArea.Lines[i - 1].Position, textArea.Lines[i - 1].EndPosition);
                    textArea.ReplaceTarget("");
                }
            }

            int foldBase = textArea.Lines[0].FoldLevel;
            int fold = foldBase;
            int region = 0;
            _indent.Clear();

            for (int lineCur = 0; lineCur < textArea.Lines.Count; lineCur++)
            {
                textArea.Lines[lineCur].FoldLevel = fold;
                textArea.Lines[lineCur].FoldLevelFlags = 0;

                if (region > 0)
                {
                    _indent.Add(lineCur, fold - region);
                }
                else
                {
                    _indent.Add(lineCur, fold);
                }

                string text = textArea.Lines[lineCur].Text.Trim().ToUpper();

                if (keyword1.Match((' ' + text + ' ').ToUpper()).Value.Length > 0)
                {
                    fold++;
                    textArea.Lines[lineCur].FoldLevelFlags = FoldLevelFlags.Header;
                }
                else if (keyword2.Match((' ' + text + ' ').ToUpper()).Value.Length > 0)
                {
                    fold--;
                    if (fold < foldBase) fold = foldBase;
                    textArea.Lines[lineCur].FoldLevel = fold;
                    textArea.Lines[lineCur].FoldLevelFlags = FoldLevelFlags.White;
                    if (region > 0)
                    {
                        _indent[lineCur] = fold - region;
                    }
                    else
                    {
                        _indent[lineCur] = fold;
                    }
                }
                else if (keyword3.Match((' ' + text + ' ').ToUpper()).Value.Length > 0)
                {
                    textArea.Lines[lineCur].FoldLevel--;
                    textArea.Lines[lineCur].FoldLevelFlags = FoldLevelFlags.White;
                    if (region > 0)
                    {
                        _indent[lineCur] = textArea.Lines[lineCur].FoldLevel - region;
                    }
                    else
                    {
                        _indent[lineCur] = textArea.Lines[lineCur].FoldLevel;
                    }
                }
                else if (keyword4.Match((' ' + text + ' ').ToUpper()).Value.Length > 0)
                {
                    fold++;
                    textArea.Lines[lineCur].FoldLevelFlags = FoldLevelFlags.Header;

                    region++;
                }
                else if (keyword5.Match((' ' + text + ' ').ToUpper()).Value.Length > 0)
                {
                    fold--;
                    if (fold < foldBase) fold = foldBase;
                    textArea.Lines[lineCur].FoldLevel = fold;
                    textArea.Lines[lineCur].FoldLevelFlags = FoldLevelFlags.White;

                    region--;

                    if (region > 0)
                    {
                        _indent[lineCur] = fold - region;
                    }
                    else
                    {
                        _indent[lineCur] = fold;
                    }
                }

                int foldCur = _indent[lineCur] - foldBase;

                string indents = "";

                for (int i = 0; i < foldCur; i++)
                {
                    indents += spaces;
                }

                int iStart = textArea.Lines[lineCur].Position;
                string lineText = textArea.Lines[lineCur].Text;
                int iLen = 0;

                while (iLen < lineText.Length && char.IsWhiteSpace(lineText[iLen]) && lineText[iLen] != '\r' && lineText[iLen] != '\n')
                {
                    iLen++;
                }
                    
                textArea.SetTargetRange(iStart, iStart + iLen);
                textArea.ReplaceTarget(indents);
            }

            foreach (string keyword in keywords.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int pos = 0;
                Match match = Regex.Match(' ' + textArea.Text.Substring(pos).ToUpper() + ' ', "[\\W](" + keyword.ToUpper() + ")[\\W]");
                while (match.Success)
                {
                    int start = Math.Max(0, pos + match.Index);
                    int len = match.Length - 2;
                    int style = sbDocument.TextArea.GetStyleAt(start);

                    if (style == STYLE_KEYWORD_1_NEW || style == STYLE_KEYWORD_2_NEW || style == STYLE_KEYWORD_3_NEW || style == STYLE_KEYWORD_4_NEW)
                    {
                        sbDocument.TextArea.SetTargetRange(start, start + len);
                        sbDocument.TextArea.ReplaceTarget(keyword);
                    }

                    pos += match.Index + len;
                    if (pos >= textArea.Text.Length) break;
                    match = Regex.Match(' ' + textArea.Text.Substring(pos).ToUpper() + ' ', "[\\W](" + keyword.ToUpper() + ")[\\W]");
                }
            }

            System.Windows.Input.Mouse.OverrideCursor = cursor;
        }

        private void OnDwellStart(object sender, DwellEventArgs e)
        {
            int currentPos = e.Position;
            
            if (currentPos < 0) return;

            if (AutoCMode == 0)
                HelperPanelVM.Get.OpenHelp();

            int wordStartPos = textArea.WordStartPosition(currentPos, true);
            string currentWord = textArea.GetWordFromPosition(wordStartPos);
            string lastWord = "";
            string lineText = textArea.Lines[textArea.LineFromPosition(currentPos)].Text;

            if (textArea.GetCharAt(wordStartPos - 1) == '.')
            {
                lastWord = textArea.GetWordFromPosition(textArea.WordStartPosition(wordStartPos - 2, true));
            }
            if (currentPos > wordStartPos)
            {
                lastObject = textArea.GetWordFromPosition(wordStartPos);
            }
            if (currentWord != "" && bpObjects.GetObjects(currentWord) != "")
            {
                //showObjectData(currentWord, false, e.Position);
            }
            else if (lastWord != "" && currentWord != "" && bpObjects.GetMembers(lastWord, currentWord) != "")
            {
                //showMethodData(lastWord, currentWord, false, true, e.Position);
            }
            else if (currentWord != "" && bpObjects.GetKeywords(currentWord) != "")
            {
                //showObjectData(currentWord, false, e.Position);
            }
            else if (currentWord != "")
            {
                var str = "";
                BpObjects.UpdateWork = true;

                if (lastWord == "")
                {
                    str = bpObjects.GetModuleSymmary(currentWord);
                    
                    if (str == "")
                    {
                        str = bpObjects.GetSubSymmary(currentWord, sbDocument.ProgramName, lineText, true);

                        if (str == "")
                        {
                            str = bpObjects.GetVarSummary(currentWord, sbDocument.ProgramName);
                        }
                    }
                }                   
                else
                {
                    str = bpObjects.GetSubSymmary(currentWord, lastWord + ".bpm", lineText, true);
                    if (str == "")
                    {
                        str = bpObjects.GetVarSummary(currentWord, lastWord + ".bpm");
                    }
                }

                BpObjects.UpdateWork = false;

                if (str != "")
                {
                    if (e.Position >= 0)
                    {
                        sbDocument.TextArea.CallTipShow(e.Position, CallTipFormat(str));
                        sbDocument.TextArea.CallTipSetHlt(0, str.Length);
                    }
                    else
                    {
                        HelpToolTipModel.Summary = str;
                        HelperPanelVM.Get.OpenToolTip();
                    }
                }
                
            }
        }

        private void TextArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HelperPanelVM.Get.OpenHelp();
            int currentPos = textArea.SelectionStart;
            if (currentPos < 0) return;
            int wordStartPos = textArea.WordStartPosition(currentPos, true);
            string currentWord = textArea.GetWordFromPosition(wordStartPos);
            string lastWord = "";
            if (textArea.GetCharAt(wordStartPos - 1) == '.')
            {
                lastWord = textArea.GetWordFromPosition(textArea.WordStartPosition(wordStartPos - 2, true));
            }
            if (lastWord == "")
            {
                ViewTreeInfo(currentWord, "");
            }
            else
            {
                ViewTreeInfo(lastWord, currentWord);
            }
        }

        private void showObjectData(string currentWord, bool showViewInfo, int Position = -1)
        {
            int stop = 0;
            foreach (Member member in BpObjects.keywords)
            {
                if (member.name.ToUpper() == currentWord.ToUpper())
                {
                    stop = 1;
                    if (showViewInfo) { ViewTreeInfo(member.name, ""); }
                    
                    if (Position >= 0)
                    {
                        sbDocument.TextArea.CallTipShow(Position, CallTipFormat(member.summary));
                        sbDocument.TextArea.CallTipSetHlt(0, member.summary.Length);
                    }
                    else
                    {
                        HelpToolTipModel.Summary = member.name + '\n' + member.summary;
                        HelperPanelVM.Get.OpenToolTip();
                    }
                    break;
                }
            }
            if (stop == 0)
            {
                foreach (BPObject obj in BpObjects.objects)
                {
                    if (obj.name.ToUpper() == currentWord.ToUpper())
                    {
                        stop = 1;
                        if (showViewInfo) { ViewTreeInfo(obj.name, ""); }

                        if (Position >= 0)
                        {
                            sbDocument.TextArea.CallTipShow(Position, CallTipFormat(obj.summary));
                            sbDocument.TextArea.CallTipSetHlt(0, obj.summary.Length);
                        }
                        else
                        {
                            HelpToolTipModel.Summary = obj.name + '\n' + obj.summary;
                            HelperPanelVM.Get.OpenToolTip();
                        }
                        break;
                    }
                }
            }
            if (stop == 0)
            {
                if (currentWord != "")
                {
                    int currentPos = textArea.CurrentPosition;
                    string lineText = textArea.Lines[textArea.LineFromPosition(currentPos)].Text;
                    var str = "";

                    str = bpObjects.GetModuleSymmary(currentWord);

                    
                    if (str == "")
                    {
                        str = bpObjects.GetSubSymmary(currentWord, sbDocument.ProgramName, lineText, true);
                    }
                    

                    if (str != "")
                    {
                        stop = 1;
                        if (Position >= 0)
                        {
                            sbDocument.TextArea.CallTipShow(Position, CallTipFormat(str));
                            sbDocument.TextArea.CallTipSetHlt(0, str.Length);
                        }
                        else
                        {
                            HelpToolTipModel.Summary = str;
                            HelperPanelVM.Get.OpenToolTip();
                        }
                    }
                }
            }

            if (stop == 0)
            {
                HelperPanelVM.Get.OpenHelp();
            }
        }

        private void showMethodData(string lastWord, string currentWord, bool showViewInfo, bool parseLine, int Position = -1)
        {
            int stop = 0;
            foreach (BPObject obj in BpObjects.objects)
            {
                if (obj.name.ToUpper() == lastWord.ToUpper())
                {
                    foreach (Member member in obj.members)
                    {
                        if (member.name.ToUpper() == currentWord.ToUpper())
                        {
                            stop = 1;
                            if (showViewInfo) { ViewTreeInfo(obj.name, member.name); }

                            string name = "";
                            switch (member.type)
                            {
                                case MemberTypes.Custom:
                                    name = member.name;
                                    break;
                                case MemberTypes.Method:
                                    name = member.name;
                                    if (member.arguments.Count > 0)
                                    {
                                        name += "(";
                                        for (int i = 0; i < member.arguments.Count; i++)
                                        {
                                            name += member.arguments.Keys.ElementAt(i);
                                            if (i < member.arguments.Count - 1) name += ',';
                                        }
                                        name += ")";
                                    }
                                    else
                                    {
                                        name += "()";
                                    }
                                    break;
                                case MemberTypes.Property:
                                    name = member.name;
                                    break;
                                case MemberTypes.Event:
                                    name = member.name;
                                    break;
                            }
                            
                            if (Position >= 0)
                            {
                                sbDocument.TextArea.CallTipShow(Position, name + "\n" + CallTipFormat(member.summary));
                                sbDocument.TextArea.CallTipSetHlt(0, name.Length);
                            }
                            else
                            {
                                HelpToolTipModel.Summary = name + '\n' + member.summary;
                                HelperPanelVM.Get.OpenToolTip();
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            if (stop == 0)
            {
                if (currentWord != "")
                {
                    int currentPos = textArea.CurrentPosition;
                    string lineText = textArea.Lines[textArea.LineFromPosition(currentPos)].Text;
                    var str = "";

                    if (lastWord == "")
                    {
                        str = bpObjects.GetModuleSymmary(currentWord);

                        if (str == "")
                        {
                            str = bpObjects.GetSubSymmary(currentWord, sbDocument.ProgramName, lineText, parseLine);
                        }
                    }
                    else
                    {
                        str = bpObjects.GetSubSymmary(currentWord, lastWord + ".bpm", lineText, parseLine);
                        if (str == "")
                        {
                            str = bpObjects.GetVarSummary(currentWord, lastWord + ".bpm");
                        }
                    }

                    if (str != "")
                    {
                        stop = 1;
                        HelpToolTipModel.Summary = str;
                        HelperPanelVM.Get.OpenToolTip();
                    }
                }
            }

            if (stop == 0)
            {
                HelperPanelVM.Get.OpenHelp();
            }
        }

        private string CallTipFormat(string text)
        {
            if (text.Contains('\n')) return text;
            string result = "";
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.None);
            string line = "";
            foreach (string word in words)
            {
                line += word + ' ';
                if (line.Length > 50)
                {
                    result += line + '\n';
                    line = "";
                }
            }
            result += line;
            return result.Trim();
        }

        private void OnDwellEnd(object sender, DwellEventArgs e)
        {
            textArea.CallTipCancel();
            //HelpPanelVM.Get.OpenHelp();
        }

        private void AutoCTimerCallback(object sender, EventArgs e)
        {
            try
            {
                if (AutoCMode == 0)
                {
                    HelperPanelVM.Get.OpenHelp();
                    return;
                }
                int index = textArea.AutoCCurrent;
                if (index < 0)
                {
                    return;
                }
                string value = AutoCData.Split(' ')[index];
                value = value.Substring(0, value.IndexOf('?'));
                //CommonData.Status.Clear();
                if (AutoCMode == 1)
                {
                    showObjectData(value, false);
                    //CommonData.Status.Add(value);
                }
                else if (AutoCMode == 2)
                {
                    showMethodData(lastObject, value, false, false);
                    //CommonData.Status.Add(lastObject + "  " +value);
                }
            }
            catch
            {

            }
        }

        private void OnAutoCSelection(object sender, AutoCSelectionEventArgs e)
        {
            
            AutoCTimer.Enabled = false;
            AutoCMode = 0;
            HelperPanelVM.Get.OpenHelp();
        }

        private void OnAutoCCompleted(object sender, AutoCSelectionEventArgs e)
        {
            AutoCTimer.Enabled = false;
            AutoCMode = 0;
            HelperPanelVM.Get.OpenHelp();
        }

        private static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.AutoCTimer != null)
            {
                try
                {
                    AutoCTimer.Dispose();
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

        private void ViewTreeInfo(string nameO, string nameM)
        {
            var tree = (System.Windows.Controls.TreeView)View.Controls.Helps.Help.GetHelpTree;
            string kw = "Sub|EndSub|For|To|Step|EndFor|If|Then|Else|ElseIf|EndIf|While|EndWhile|Goto|include|import|And|Or|folder|function|endfunction|in|out|number|string|private";
            if (kw.ToUpper().IndexOf(nameO.ToUpper()) != -1 && nameM == "")
            {
                nameM = nameO;
                nameO = "Keywords";
            }

            foreach (System.Windows.Controls.TreeViewItem to in tree.Items)
            {
                if (to.Name == nameO)
                {
                    to.IsExpanded = true;
                    double h = MainWindowVM.HeightHelpPanel - 100;
                    if (h < 0)
                        h = 0;
                    to.BringIntoView(new System.Windows.Rect(new System.Windows.Size() { Width = MainWindowVM.WidthHelpPanel, Height = h }));
                    if (nameM != "")
                    {
                        foreach (object tm in to.Items)
                        {
                            if (tm.GetType() == to.GetType())
                            {
                                var tmi = tm as System.Windows.Controls.TreeViewItem;
                                if (tmi.Name == nameM.Replace("_", "robomaks"))
                                {
                                    tmi.IsExpanded = true;
                                    tmi.BringIntoView(new System.Windows.Rect(tmi.DesiredSize));
                                }
                                else
                                {
                                    tmi.IsExpanded = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    to.IsExpanded = false;
                }
            }
        }

        internal void SetFolding()
        {
            int position = textArea.CurrentPosition;

            int region = 0;
            _indent.Clear();

            int foldBase = textArea.Lines[0].FoldLevel;
            int fold = foldBase;

            for (int i = 0; i < textArea.Lines.Count; i++)
            {
                textArea.Lines[i].FoldLevel = fold;
                textArea.Lines[i].FoldLevelFlags = 0;
                string text = textArea.Lines[i].Text.Trim().ToUpper();

                if (region > 0)
                {
                    _indent.Add(i, fold - region);
                }
                else
                {
                    _indent.Add(i, fold);
                }

                if (keyword1.Match(('\n' + text + '\n').ToUpper()).Value.Length > 0)
                {
                    fold++;
                    textArea.Lines[i].FoldLevelFlags = FoldLevelFlags.Header;
                }
                else if (keyword2.Match(('\n' + text + '\n').ToUpper()).Value.Length > 0)
                {
                    fold--;
                    if (fold < foldBase) fold = foldBase;
                    textArea.Lines[i].FoldLevel = fold;
                    textArea.Lines[i].FoldLevelFlags = FoldLevelFlags.White;
                    if (region > 0)
                    {
                        _indent[i] = fold - region;
                    }
                    else
                    {
                        _indent[i] = fold;
                    }
                }
                else if (keyword3.Match(('\n' + text + '\n').ToUpper()).Value.Length > 0)
                {
                    textArea.Lines[i].FoldLevel--;
                    textArea.Lines[i].FoldLevelFlags = FoldLevelFlags.White;
                    if (region > 0)
                    {
                        _indent[i] =  textArea.Lines[i].FoldLevel - region;
                    }
                    else
                    {
                        _indent[i] =  textArea.Lines[i].FoldLevel;
                    }
                }
                else if (keyword4.Match(('\n' + text + '\n').ToUpper()).Value.Length > 0)
                {
                    fold++;
                    textArea.Lines[i].FoldLevelFlags = FoldLevelFlags.Header;
                    region++;
                }
                else if (keyword5.Match(('\n' + text + '\n').ToUpper()).Value.Length > 0)
                {
                    fold--;
                    if (fold < foldBase) fold = foldBase;
                    textArea.Lines[i].FoldLevel = fold;
                    textArea.Lines[i].FoldLevelFlags = FoldLevelFlags.White;

                    region--;

                    if (region > 0)
                    {
                        _indent[i] = fold - region;
                    }
                    else
                    {
                        _indent[i] = fold;
                    }
                }
            }
            textArea.CurrentPosition = position;
            LastLineCount = textArea.Lines.Count;
        }

        private void SetMethods()
        {
            _bpObject = new HashSet<string>();
            _bpObject.Add("assert");
            _bpObject.Add("buttons");
            _bpObject.Add("byte");
            _bpObject.Add("ev3");
            _bpObject.Add("ev3file");
            _bpObject.Add("lcd");
            _bpObject.Add("mailbox");
            _bpObject.Add("math");
            _bpObject.Add("motor");
            _bpObject.Add("motora");
            _bpObject.Add("motorab");
            _bpObject.Add("motorac");
            _bpObject.Add("motorad");
            _bpObject.Add("motorb");
            _bpObject.Add("motorbc");
            _bpObject.Add("motorbd");
            _bpObject.Add("motorc");
            _bpObject.Add("motorcd");
            _bpObject.Add("motord");
            _bpObject.Add("program");
            _bpObject.Add("row");
            _bpObject.Add("sensor");
            _bpObject.Add("sensor1");
            _bpObject.Add("sensor2");
            _bpObject.Add("sensor3");
            _bpObject.Add("sensor4");
            _bpObject.Add("speaker");
            _bpObject.Add("text");
            _bpObject.Add("thread");
            _bpObject.Add("time");
            _bpObject.Add("vector");
        }

        internal void SetSnippets()
        {
            textArea.AutoCCancel();
            var line = textArea.CurrentLine;
            var text = textArea.Lines[line].Text.TrimEnd().Replace("\r", "").Replace("\n", "").Replace("\r\n", "").Replace("\n\r", "");
            var textToFind = text.Trim().ToLower();
            var foldBase = textArea.Lines[0].FoldLevel;
            var foldCur = textArea.Lines[line].FoldLevel - foldBase;
            var count = textArea.Lines.Count;

            var indentsRegion = "";
            for (int i = 0; i < foldCur; i++)
            {
                indentsRegion += spaces;
            }
            var indents = indentsRegion + spaces;
            var indentsClose = indentsRegion;

            if (textToFind.IndexOf("function") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndFunction" + "\r\n");
                }
                else
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndFunction");
                    
                }
                textArea.GotoPosition(tmpPos + indents.Length);
            }
            else if (textToFind.IndexOf("sub") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndSub" + "\r\n");
                }
                else
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndSub");
                }
                textArea.GotoPosition(tmpPos + indents.Length);
            }
            else if (textToFind.IndexOf("if") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndIf" + "\r\n");
                }
                else
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndIf");
                }
                textArea.GotoPosition(tmpPos + indents.Length);
            }
            else if (textToFind.IndexOf("for") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndFor" + "\r\n");
                }
                else
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndFor");
                }
                textArea.GotoPosition(tmpPos + indents.Length);
            }
            else if (textToFind.IndexOf("while") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndWhile" + "\r\n");
                }
                else
                {
                    textArea.AddText(indents + "\r\n" + indentsClose + "EndWhile");
                }
                textArea.GotoPosition(tmpPos + indents.Length);
            }
            else if (textToFind.IndexOf("#region") == 0)
            {
                textArea.SetTargetRange(textArea.Lines[line].Position, textArea.Lines[line].EndPosition);
                textArea.ReplaceTarget(text + "\r\n");

                var tmpPos = textArea.Lines[line].EndPosition;
                textArea.GotoPosition(tmpPos);

                if (line < count - 1)
                {
                    textArea.AddText(indentsRegion + "\r\n" + indentsRegion + "#endregion" + "\r\n");
                }
                else
                {
                    textArea.AddText(indentsRegion + "\r\n" + indentsRegion + "#endregion");
                }
                textArea.GotoPosition(tmpPos + indentsRegion.Length);
            }
        }

        internal void FoldLine(int position)
        {
            var line = textArea.LineFromPosition(position);
            var text = textArea.Lines[line].Text;
            var start = textArea.Lines[line].Position;
            var end = textArea.Lines[line].EndPosition;

            if (text.Trim().ToLower().IndexOf("#region") == 0)
            {
                var tmpText = " " + text.Trim().Replace("#region ", "").Replace("#", "") + " ";
                if (tmpText.Trim() == "")
                {
                    tmpText = " Region ";
                }

                if (textArea.Lines[line].Expanded)
                {
                    textArea.StartStyling(start);
                    textArea.SetStyling(end - start, STYLE_REGION_CLOSE);
                }
                else
                {
                    textArea.StartStyling(start);
                    textArea.SetStyling(end - start, STYLE_REGION_OPEN);
                }

                textArea.Lines[line].ToggleFoldShowText(tmpText);
            }
            else
            {
                textArea.Lines[line].ToggleFoldShowText("...");
            }
        }

        private void SetStyle(int startStyling, int endStyling, Scintilla textEditor)
        {
            var startLine = textEditor.LineFromPosition(startStyling);
            var endLine = textEditor.LineFromPosition(endStyling);

            for (int i = startLine; i <= endLine; i++)
            {
                var style = STYLE_UNKNOWN_NEW;
                var text = textEditor.Lines[i].Text + " ";               
                var word = "";
                var tmpPos = textEditor.Lines[i].Position;
                var startPos = textEditor.Lines[i].Position;
                var tmpEnd = textEditor.Lines[i].EndPosition;
                textEditor.StartStyling(tmpPos);
                textEditor.SetStyling(tmpEnd - tmpPos, style);
                textEditor.StartStyling(tmpPos);
                var digits = new Regex("[0-9]");

                var commentValue = false;
                var regionValue = false;
                var stringValue = false;

                try
                {
                    if (text == null || text.Trim().Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = 0; j < text.Length; j++)
                        {
                            char ch = text[j];
                            if (char.IsLetterOrDigit(ch) || ch == '.' || ch == '_')
                            {
                                if (j > 0 && text[j - 1] == ' ' && !regionValue && !commentValue && !stringValue)
                                {
                                    word = "";
                                }

                                word += ch;                               

                                if (!commentValue && !regionValue && !stringValue)
                                {
                                    var match = digits.Matches(word);

                                    if (word.Length == 1)
                                    {
                                        tmpPos = startPos + j;
                                    }

                                    if (match.Count == word.Length)
                                    {
                                        style = STYLE_LITERAL_NEW;
                                    }
                                    else if (_keywords1.Contains(word.ToUpper()))
                                    {
                                        style = STYLE_KEYWORD_1_NEW;
                                    }
                                    else if (_keywords2.Contains(word.ToUpper()))
                                    {
                                        style = STYLE_KEYWORD_2_NEW;
                                    }
                                    else if (_keywords3.Contains(word.ToUpper()))
                                    {
                                        style = STYLE_KEYWORD_3_NEW;
                                    }
                                    else if (_keywords4.Contains(word.ToUpper()))
                                    {
                                        style = STYLE_KEYWORD_4_NEW;
                                    }
                                    
                                    else if (text.Trim().ToLower().IndexOf("goto ") == 0 && text.Length > 5)
                                    {
                                        style = STYLE_LABEL_NEW;
                                    }
                                    else if (text.Trim().ToLower().IndexOf("sub ") == 0 && text.Length > 4)
                                    {
                                        style = STYLE_SUBROUTINE_NEW;
                                    }
                                    else if (text.Trim().ToLower().IndexOf("function ") == 0 && text.Length > 9)
                                    {
                                        var ind = text.Trim().IndexOf("(");

                                        if (ind >= tmpPos)
                                        {
                                            style = STYLE_SUBROUTINE_NEW;
                                        }
                                        else
                                        {
                                            style = STYLE_UNKNOWN_NEW;
                                        }
                                    }
                                    else if (word.IndexOf(".") != -1)
                                    {
                                        if (word.Length > 1 && match.Count == word.Length - 1)
                                        {
                                            style = STYLE_LITERAL_NEW;
                                        }
                                        else
                                        {
                                            var ind = word.IndexOf(".");
                                            var firstWord = word.Substring(0, ind);
                                            var doubleWord = "";

                                            if (ind < word.Length)
                                                doubleWord = word.Substring(ind + 1);

                                            textEditor.StartStyling(tmpPos);

                                            if (_bpObject.Contains(firstWord.ToLower()))
                                                textEditor.SetStyling(firstWord.Length, STYLE_OBJECT_NEW);
                                            else
                                                textEditor.SetStyling(firstWord.Length, STYLE_MODULE_NEW);

                                            textEditor.SetStyling(1, STYLE_UNKNOWN_NEW);

                                            if (doubleWord.Length > 0)
                                            {
                                                textEditor.SetStyling(doubleWord.Length, STYLE_METHOD_NEW);
                                            }

                                            continue;
                                        }
                                    }
                                    else if (text.Trim().ToLower().IndexOf("thread.run") == 0 && text.IndexOf("=") > 0)
                                    {
                                        style = STYLE_SUBROUTINE_NEW;
                                    }
                                    else
                                    {
                                        style = STYLE_UNKNOWN_NEW;
                                    }

                                    if (word.Length > 0)
                                    {
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(word.Length, style);
                                    }
                                }
                                else
                                {
                                    if (commentValue)
                                    {
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos, style);
                                    }
                                    if (stringValue && !commentValue && !regionValue)
                                    {
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos, style);
                                    }
                                    if (regionValue && !commentValue && !stringValue)
                                    {
                                        if (textEditor.Lines[i].Expanded)
                                        {
                                            style = STYLE_REGION_OPEN;
                                            textEditor.StartStyling(tmpPos);
                                            textEditor.SetStyling(startPos + j - tmpPos, style);
                                        }
                                        else
                                        {
                                            style = STYLE_REGION_CLOSE;
                                            textEditor.StartStyling(tmpPos);
                                            textEditor.SetStyling(startPos + j - tmpPos, style);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ch == '\'' && !commentValue)
                                {
                                    style = STYLE_COMMENT_NEW;
                                    tmpPos = startPos + j;
                                    textEditor.StartStyling(tmpPos);
                                    textEditor.SetStyling(startPos + j - tmpPos, style);
                                    textEditor.StartStyling(tmpPos);
                                    commentValue = true;
                                }
                                else if (ch == '"' && !commentValue && !regionValue)
                                {
                                    if (!stringValue)
                                    {
                                        style = STYLE_STRING_NEW;
                                        tmpPos = startPos + j;
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos, style);
                                        textEditor.StartStyling(tmpPos);
                                        stringValue = true;
                                    }
                                    else
                                    {
                                        style = STYLE_STRING_NEW;
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos + 1, style);
                                        stringValue = false;
                                    }
                                }
                                else if (ch == '#' && !commentValue && !stringValue)
                                {
                                    regionValue = true;
                                    tmpPos = startPos + j;

                                    if (text.Trim().ToLower().IndexOf("#region") == 0)
                                    {
                                        if (textEditor.Lines[i].Expanded)
                                        {
                                            style = STYLE_REGION_OPEN;
                                            textEditor.StartStyling(tmpPos);
                                            textEditor.SetStyling(startPos + j - tmpPos, style);
                                        }
                                        else
                                        {
                                            style = STYLE_REGION_CLOSE;
                                            textEditor.StartStyling(tmpPos);
                                            textEditor.SetStyling(startPos + j - tmpPos, style);
                                        }
                                    }
                                    else if (text.Trim().ToLower().IndexOf("#endregion") == 0)
                                    {
                                        style = STYLE_REGION_OPEN;
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos, style);
                                        continue;
                                    }
                                    else
                                    {
                                        style = STYLE_REGION_OPEN;
                                        tmpPos = startPos + j;
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(startPos + j - tmpPos, style);
                                        continue;
                                    }
                                }
                                else if (ch == '(' && !commentValue && !stringValue && !stringValue)
                                {
                                    if (word.IndexOf(".") == -1 && !_keywords4.Contains(word.Trim().ToUpper()))
                                    {
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(word.Length, STYLE_SUBROUTINE_NEW);
                                    }
                                }
                                else if (ch == ':' && !commentValue && !stringValue && !stringValue)
                                {
                                    if (word.IndexOf(".") == -1)
                                    {
                                        textEditor.StartStyling(tmpPos);
                                        textEditor.SetStyling(word.Length, STYLE_LABEL_NEW);
                                    }
                                }
                                else if ((ch == '+' || ch == '-' || ch == '/' || ch == '*' || ch == '=' || ch == '<' || ch == '>') && !commentValue && !stringValue && !stringValue)
                                {
                                    tmpPos = startPos + j;
                                    textEditor.StartStyling(tmpPos);
                                    textEditor.SetStyling(1, STYLE_OPERATOR_NEW);
                                }

                                if (ch != ' ')
                                {
                                    word = "";
                                }
                            }

                            if (commentValue)
                            {
                                textEditor.StartStyling(tmpPos);
                                textEditor.SetStyling(startPos + j - tmpPos, style);
                            }

                            if (stringValue && !commentValue && !regionValue)
                            {
                                textEditor.StartStyling(tmpPos);
                                textEditor.SetStyling(startPos + j - tmpPos, style);
                            }

                            if (regionValue && !commentValue && !stringValue)
                            {
                                if (textEditor.Lines[i].Expanded)
                                {
                                    style = STYLE_REGION_OPEN;
                                    textEditor.StartStyling(tmpPos);
                                    textEditor.SetStyling(startPos + j - tmpPos, style);
                                }
                                else
                                {
                                    style = STYLE_REGION_CLOSE;
                                    textEditor.StartStyling(tmpPos);
                                    textEditor.SetStyling(startPos + j - tmpPos, style);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    CommonData.Status.Text = ex.Message;
                }
            }
        }
    }
}
