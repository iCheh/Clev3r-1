using Clever.CommonData;
using Clever.Model.Bplus;
using Clever.Model.Utils;
using Clever.View.Controls.Helps;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Clever.Model.Intellisense
{
    internal class IntellisenseInfo
    {
        internal void GetInfo()
        {
            var treeView = Help.GetHelpTree;
            treeView.FontFamily = new FontFamily("Calibri");

            CreateOperators(treeView);
            

            foreach (var obj in App.IntellisenseObjects)
            {
                var tvi1 = new TreeViewItem() { Name = obj.Name, Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpOne"] };
                treeView.Items.Add(tvi1);

                if (obj.Class.Type == IntellisenseType.Keyword)
                {
                    //imghead = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Intellisense/IntellisenseKeyword.png")), Width = 14, Height = 14, ClipToBounds = true };
                    tvi1.Header = new View.Controls.Intellisense.HeaderKeyword() { Text = obj.Name };
                }

                else
                {
                    //imghead = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Intellisense/IntellisenseObject.png")), Width = 14, Height = 14, ClipToBounds = true };
                    tvi1.Header = new View.Controls.Intellisense.HeaderObject() { Text = obj.Name };
                }

                var sp1 = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
                sp1.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidthCl") });

                foreach (var w in obj.Class.Summary)
                {
                    var tb = new TextBlock() { Margin = new System.Windows.Thickness(2) };
                    tb.TextWrapping = System.Windows.TextWrapping.Wrap;
                    tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb.Text = w;
                    sp1.Children.Add(tb);
                }
                tvi1.Items.Add(sp1);

                if (obj.Methods.Count > 0)
                {
                    foreach (var method in obj.Methods)
                    {
                        var tvi2 = new TreeViewItem() { Name = method.Name.Replace("_", "robomaks"), Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpTwo"] };
                        tvi2.Header = new View.Controls.Intellisense.HeaderMethod() { Text = method.Name };
                        tvi1.Items.Add(tvi2);
                        var sp2 = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
                        sp2.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidth") });
                        sp2.Orientation = Orientation.Vertical;
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                        tvi2.Items.Add(sp2);
                        if (method.Summary.Count > 0)
                        {
                            foreach (var w in method.Summary)
                            {
                                var tbs = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbs.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbs.Text = w;
                                tbs.FontSize = 14;
                                sp2.Children.Add(tbs);
                            }
                            var tbh = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                            tbh.TextWrapping = System.Windows.TextWrapping.Wrap;
                            tbh.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            tbh.FontSize = 14;
                            sp2.Children.Add(tbh);
                            var tbh1 = new Run();
                            tbh1.Text = obj.Class.Name + ".";
                            tbh1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_OBJECT_NAME));
                            tbh1.FontStyle = FontStyles.Normal;
                            var tbh2 = new Run();
                            tbh2.Text = method.Name;
                            tbh2.FontWeight = FontWeights.Bold;
                            tbh2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            var tbh3 = new Run();
                            tbh3.Text = " (";
                            tbh3.FontStyle = FontStyles.Normal;
                            tbh3.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            var tbh4 = new Run();
                            tbh4.FontStyle = FontStyles.Italic;
                            tbh4.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            if (method.ParamName.Count > 0)
                            {
                                for (int i = 0; i < method.ParamName.Count; i++)
                                {
                                    tbh4.Text += method.ParamName[i];
                                    if (i < method.ParamName.Count - 1)
                                    {
                                        tbh4.Text += ", ";
                                    }
                                }
                            }
                            var tbh5 = new Run();
                            tbh5.Text = ")";
                            tbh5.FontStyle = FontStyles.Normal;
                            tbh5.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            tbh.Inlines.Add(tbh1);
                            tbh.Inlines.Add(tbh2);
                            tbh.Inlines.Add(tbh3);
                            tbh.Inlines.Add(tbh4);
                            tbh.Inlines.Add(tbh5);
                            if (method.ParamName.Count > 0)
                            {
                                for (int i = 0; i < method.ParamName.Count; i++)
                                {
                                    var tbpn = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                    tbpn.TextWrapping = System.Windows.TextWrapping.Wrap;
                                    tbpn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                    var tbpn1 = new Run();
                                    tbpn1.Text = method.ParamName[i];
                                    tbpn1.FontStyle = FontStyles.Italic;
                                    tbpn1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                                    var tbpn2 = new Run();
                                    tbpn2.Text = " - " + method.ParamSummary[i];
                                    tbpn2.FontStyle = FontStyles.Normal;
                                    tbpn2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                    tbpn.Inlines.Add(tbpn1);
                                    tbpn.Inlines.Add(tbpn2);
                                    sp2.Children.Add(tbpn);
                                }
                            }
                            if (method.Return != "")
                            {
                                var tbr = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbr.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                var tbr1 = new Run();
                                tbr1.Text = MainWindowVM.GetLocalization["hpReturn"] + ": ";
                                tbr1.FontWeight = FontWeights.Bold;
                                tbr1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                var tbr2 = new Run();
                                tbr2.Text = method.Return;
                                tbr2.FontStyle = FontStyles.Normal;
                                tbr2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                tbr.Inlines.Add(tbr1);
                                tbr.Inlines.Add(tbr2);
                                sp2.Children.Add(tbr);
                            }

                            if (method.Example != "")
                            {
                                var tbe = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbe.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbe.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbe.Text = MainWindowVM.GetLocalization["hpExample"] + ":";
                                tbe.FontWeight = FontWeights.Bold;
                                tbe.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                sp2.Children.Add(tbe);
                                var tbes = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbes.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbes.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbes.Text = "' " + method.Example;
                                tbes.FontStyle = FontStyles.Italic;
                                tbes.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_COMMENT_COLOR));
                                sp2.Children.Add(tbes);
                                if (method.Code.Count > 0)
                                {
                                    var tbl = GetColorCode(FormatCode(method.Code));
                                    sp2.Children.Add(tbl);
                                }
                            }
                        }
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                    }
                }
                if (obj.Event.Count > 0)
                {
                    foreach (var ev in obj.Event)
                    {
                        var tvi2 = new TreeViewItem() { Name = ev.Name.Replace("_", "robomaks"), Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpTwo"] };
                        tvi2.Header = new View.Controls.Intellisense.HeaderEvent() { Text = ev.Name };
                        tvi1.Items.Add(tvi2);
                        var sp2 = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
                        sp2.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidth") });
                        sp2.Orientation = Orientation.Vertical;
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                        tvi2.Items.Add(sp2);
                        if (ev.Summary.Count > 0)
                        {
                            foreach (var w in ev.Summary)
                            {
                                var tbs = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbs.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbs.Text = w;
                                tbs.FontSize = 14;
                                sp2.Children.Add(tbs);
                            }
                            var tbh = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                            tbh.TextWrapping = System.Windows.TextWrapping.Wrap;
                            tbh.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            tbh.FontSize = 14;
                            sp2.Children.Add(tbh);
                            var tbh1 = new Run();
                            tbh1.Text = obj.Class.Name + ".";
                            tbh1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_OBJECT_NAME));
                            tbh1.FontStyle = FontStyles.Normal;
                            var tbh2 = new Run();
                            tbh2.Text = ev.Name;
                            tbh2.FontWeight = FontWeights.Bold;
                            tbh2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            tbh.Inlines.Add(tbh1);
                            tbh.Inlines.Add(tbh2);
                            if (ev.Return != "")
                            {
                                var tbr = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbr.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                var tbr1 = new Run();
                                tbr1.Text = MainWindowVM.GetLocalization["hpReturn"] + ": ";
                                tbr1.FontWeight = FontWeights.Bold;
                                tbr1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                var tbr2 = new Run();
                                tbr2.Text = ev.Return;
                                tbr2.FontStyle = FontStyles.Normal;
                                tbr2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                tbr.Inlines.Add(tbr1);
                                tbr.Inlines.Add(tbr2);
                                sp2.Children.Add(tbr);
                            }
                            if (ev.Example != "")
                            {
                                var tbe = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbe.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbe.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbe.Text = MainWindowVM.GetLocalization["hpExample"] + ":";
                                tbe.FontWeight = FontWeights.Bold;
                                tbe.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                sp2.Children.Add(tbe);
                                var tbes = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbes.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbes.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbes.Text = "' " + ev.Example;
                                tbes.FontStyle = FontStyles.Italic;
                                tbes.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_COMMENT_COLOR));
                                sp2.Children.Add(tbes);
                                if (ev.Code.Count > 0)
                                {
                                    var tbl = GetColorCode(FormatCode(ev.Code));
                                    sp2.Children.Add(tbl);
                                }
                            }
                        }
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                    }
                }
                if (obj.Property.Count > 0)
                {
                    foreach (var prop in obj.Property)
                    {
                        var tvi2 = new TreeViewItem() { Name = prop.Name.Replace("_", "robomaks"), Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpTwo"] };
                        tvi2.Header = new View.Controls.Intellisense.HeaderProperty() { Text = prop.Name };
                        tvi1.Items.Add(tvi2);
                        var sp2 = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
                        sp2.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidth") });
                        sp2.Orientation = Orientation.Vertical;
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                        tvi2.Items.Add(sp2);
                        if (prop.Summary.Count > 0)
                        {
                            foreach (var w in prop.Summary)
                            {
                                var tbs = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbs.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbs.Text = w;
                                tbs.FontSize = 14;
                                sp2.Children.Add(tbs);
                            }
                            var tbh = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                            tbh.TextWrapping = System.Windows.TextWrapping.Wrap;
                            tbh.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            tbh.FontSize = 14;
                            sp2.Children.Add(tbh);
                            var tbh1 = new Run();
                            tbh1.Text = obj.Class.Name + ".";
                            tbh1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_OBJECT_NAME));
                            tbh1.FontStyle = FontStyles.Normal;
                            var tbh2 = new Run();
                            tbh2.Text = prop.Name;
                            tbh2.FontWeight = FontWeights.Bold;
                            tbh2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            tbh.Inlines.Add(tbh1);
                            tbh.Inlines.Add(tbh2);
                            if (prop.Return != "")
                            {
                                var tbr = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbr.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                var tbr1 = new Run();
                                tbr1.Text = MainWindowVM.GetLocalization["hpReturn"] + ": ";
                                tbr1.FontWeight = FontWeights.Bold;
                                tbr1.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                var tbr2 = new Run();
                                tbr2.Text = prop.Return;
                                tbr2.FontStyle = FontStyles.Normal;
                                tbr2.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                tbr.Inlines.Add(tbr1);
                                tbr.Inlines.Add(tbr2);
                                sp2.Children.Add(tbr);
                            }
                            if (prop.Example != "")
                            {
                                var tbe = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbe.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbe.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbe.Text = MainWindowVM.GetLocalization["hpExample"] + ":";
                                tbe.FontWeight = FontWeights.Bold;
                                tbe.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                sp2.Children.Add(tbe);
                                var tbes = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbes.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbes.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbes.Text = "' " + prop.Example;
                                tbes.FontStyle = FontStyles.Italic;
                                tbes.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_COMMENT_COLOR));
                                sp2.Children.Add(tbes);
                                if (prop.Code.Count > 0)
                                {
                                    var tbl = GetColorCode(FormatCode(prop.Code));
                                    sp2.Children.Add(tbl);
                                }
                            }
                        }
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                    }
                }
                if (obj.Keywords.Count > 0)
                {
                    foreach (var key in obj.Keywords)
                    {
                        var tvi2 = new TreeViewItem() { Name = key.Name.Replace("_", "robomaks"), Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpTwo"] };
                        tvi2.Header = new View.Controls.Intellisense.HeaderKeyword() { Text = key.Name };
                        tvi1.Items.Add(tvi2);
                        var sp2 = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
                        sp2.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidth") });
                        sp2.Orientation = Orientation.Vertical;
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                        tvi2.Items.Add(sp2);
                        if (key.Summary.Count > 0)
                        {
                            var tbh = new TextBlock() { Margin = new System.Windows.Thickness(2) };
                            tbh.TextWrapping = System.Windows.TextWrapping.Wrap;
                            tbh.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            tbh.FontWeight = FontWeights.Bold;
                            tbh.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_METHOD_NAME));
                            tbh.Text = key.Name;
                            sp2.Children.Add(tbh);
                            var tbs = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                            tbs.TextWrapping = System.Windows.TextWrapping.Wrap;
                            tbs.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            foreach (var w in key.Summary)
                            {
                                var tbsr = new Run();
                                tbsr.FontWeight = FontWeights.Normal;
                                tbsr.FontStyle = FontStyles.Normal;
                                tbsr.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                tbsr.Text += w;
                                tbs.Inlines.Add(tbsr);
                            }
                            sp2.Children.Add(tbs);
                            if (key.Example != "")
                            {
                                var tbe = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbe.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbe.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbe.Text = MainWindowVM.GetLocalization["hpExample"] + ":";
                                tbe.FontWeight = FontWeights.Bold;
                                tbe.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_TEXT));
                                sp2.Children.Add(tbe);
                                var tbes = new TextBlock() { Margin = new System.Windows.Thickness(2, 2, 2, 2) };
                                tbes.TextWrapping = System.Windows.TextWrapping.Wrap;
                                tbes.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                tbes.Text = "' " + key.Example;
                                tbes.FontStyle = FontStyles.Italic;
                                tbes.Foreground = new SolidColorBrush(IntToColor(BpColors.HELP_COMMENT_COLOR));
                                sp2.Children.Add(tbes);
                                if (key.Code.Count > 0)
                                {
                                    var tbl = GetColorCode(FormatCode(key.Code));
                                    sp2.Children.Add(tbl);
                                }
                            }
                        }
                        sp2.Children.Add(new Separator() { Margin = new Thickness(2), Height = 6 });
                    }
                }
            }
        }

        private Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private Color IntToColor(string word)
        {
            if (word == "" || word == " ")
            {
                return Color.FromArgb(255, 0, 0, 0);
            }

            Regex regex;
            MatchCollection matches;
            regex = new Regex("[a-zA-Z]");
            matches = regex.Matches(word);
            if (matches.Count == word.Length || matches.Count + 1 == word.Length)
            {
                foreach (var obj in App.IntellisenseObjects)
                {
                    if (obj.Name == word)
                    {
                        return IntToColor(BpColors.HELP_OBJECT_COLOR);
                    }
                    foreach (var k in obj.Keywords)
                    {
                        if (k.Name == word)
                        {
                            return IntToColor(BpColors.HELP_KEYWORD_COLOR);
                        }
                    }
                    foreach (var m in obj.Methods)
                    {
                        if (m.Name == word)
                        {
                            return IntToColor(BpColors.HELP_METHOD_COLOR);
                        }
                    }
                    foreach (var p in obj.Property)
                    {
                        if (p.Name == word)
                        {
                            return IntToColor(BpColors.HELP_METHOD_COLOR);
                        }
                    }
                    foreach (var e in obj.Event)
                    {
                        if (e.Name == word)
                        {
                            return IntToColor(BpColors.HELP_METHOD_COLOR);
                        }
                    }
                }
            }
            regex = new Regex("[0-9]");
            matches = regex.Matches(word);
            if (matches.Count == word.Length)
            {
                return IntToColor(BpColors.HELP_OBJECT_COLOR);
            }
            else if (matches.Count == word.Length + 1 && word.IndexOf('.') != -1)
            {
                return IntToColor(BpColors.HELP_OBJECT_COLOR);
            }
            else if (word.IndexOf('"') != -1)
            {
                return IntToColor(BpColors.HELP_STRING_COLOR);
            }
            return Color.FromArgb(255, 0, 0, 0);
        }

        private List<string> FormatCode(List<string> code)
        {
            var outcode = new List<string>();
            int stop = 0;
            foreach (var w in code)
            {
                string tmp = "";
                if (w.ToUpper().IndexOf("IF") != -1 && w.ToUpper().IndexOf("ELSE") == -1 && w.ToUpper().IndexOf("ENDIF") == -1)
                {
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                    stop++;
                }
                else if (w.ToUpper().IndexOf("ELSE") != -1)
                {
                    stop--;
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                    stop++;
                }
                else if (w.ToUpper().IndexOf("FOR") != -1 && w.ToUpper().IndexOf("ENDFOR") == -1)
                {
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                    stop++;
                }
                else if (w.ToUpper().IndexOf("WHILE") != -1 && w.ToUpper().IndexOf("ENDWHILE") == -1)
                {
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                    stop++;
                }
                else if (w.ToUpper().IndexOf("SUB") != -1 && w.ToUpper().IndexOf("ENDSUB") == -1)
                {
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                    stop++;
                }
                else if (w.ToUpper().IndexOf("ENDIF") != -1 || w.ToUpper().IndexOf("ENDSUB") != -1 || w.ToUpper().IndexOf("ENDFOR") != -1 || w.ToUpper().IndexOf("ENDWHILE") != -1)
                {
                    stop--;
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                }
                else
                {
                    for (int i = 0; i < stop; i++)
                    {
                        tmp += "  ";
                    }
                    tmp += w;
                }
                outcode.Add(tmp);
            }
            return outcode;
        }

        private RichTextBox GetColorCode(List<string> code)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.PreviewMouseDoubleClick += Rtb_PreviewMouseDoubleClick;
            rtb.IsReadOnly = true;
            rtb.ContextMenu = null;
            FlowDocument fd = new FlowDocument();
            rtb.Document = fd;
            rtb.Margin = new System.Windows.Thickness(2, 4, 2, 4);
            rtb.BorderThickness = new System.Windows.Thickness(0);
            rtb.BorderBrush = new SolidColorBrush(IntToColor(0x00FFFFFF));
            rtb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            rtb.FontStyle = FontStyles.Normal;
            rtb.FontSize = 14;
            Paragraph prgf = new Paragraph();
            prgf.LineHeight = 10;
            fd.Blocks.Add(prgf);
            int item = 0;

            foreach (var ln in code)
            {
                string word = "";
                for (int i = 0; i < ln.Length; i++)
                {
                    if (ln[i] == '+' || ln[i] == '-' || ln[i] == '*' || ln[i] == '/' || ln[i] == '(' || ln[i] == ')' || ln[i] == '[' || ln[i] == ']' || ln[i] == '=' || ln[i] == ' ' || ln[i] == '<' || ln[i] == '>')
                    {
                        var r1 = new Run();
                        r1.Text = word;
                        r1.Foreground = new SolidColorBrush(IntToColor(word));
                        prgf.Inlines.Add(r1);
                        word = ln[i].ToString();
                        var r2 = new Run();
                        r2.Text = word;
                        r2.Foreground = new SolidColorBrush(IntToColor(word));
                        prgf.Inlines.Add(r2);
                        word = "";
                    }
                    else if (ln[i] == '.')
                    {
                        if (word.Length > 0)
                        {
                            char ch = word[word.Length - 1];
                            if ((ch == '0' || ch == '1' || ch == '2' || ch == '3' || ch == '4' || ch == '5' || ch == '6' || ch == '7' || ch == '8' || ch == '9') && word.IndexOf("EV3") == -1 && word.IndexOf("Sensor") == -1)
                            {
                                word += ln[i];
                            }
                            else
                            {
                                var r1 = new Run();
                                r1.Text = word;
                                r1.Foreground = new SolidColorBrush(IntToColor(word));
                                prgf.Inlines.Add(r1);
                                word = ln[i].ToString();
                                var r2 = new Run();
                                r2.Text = word;
                                r2.Foreground = new SolidColorBrush(IntToColor(word));
                                prgf.Inlines.Add(r2);
                                word = "";
                            }
                        }
                    }
                    else
                    {
                        word += ln[i];
                    }
                }
                var r = new Run();
                r.Text = word;
                r.Foreground = new SolidColorBrush(IntToColor(word));
                if (item < code.Count - 1)
                {
                    r.Text += "\n";
                }
                item++;
                prgf.Inlines.Add(r);
            }
            return rtb;
        }

        private void Rtb_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MainWindowVM.CurrentProgram != null)
            {
                var rtb = (RichTextBox)sender;
                rtb.SelectAll();
                string text = rtb.Selection.Text;

                Clipboard.SetData(DataFormats.Text, (Object)text);

                Elements elements = new Elements();

                elements.GetTextEditor(MainWindowVM.CurrentProgram).Paste();
            }
        }

        private void CreateOperators(TreeView tree)
        {
            var tvi = new TreeViewItem() { Style = (System.Windows.Style)Application.Current.Resources["TreeViewItemHelpOne"] };
            tree.Items.Add(tvi);
            string tmpHeaderName = "";           

            tvi.Name = "operators";

            if (Configurations.Get.Language == "ru")
            {
                tmpHeaderName = "Операторы";
            }
            else if (Configurations.Get.Language == "en")
            {
                tmpHeaderName = "Operators";
            }
            else if (Configurations.Get.Language == "ua")
            {
                tmpHeaderName = "Оператори";
            }

            tvi.Header = new View.Controls.Intellisense.HeaderOperators() { Text = tmpHeaderName };

            var sp = new StackPanel() { Margin = new Thickness(0, 2, 0, 4) };
            sp.SetBinding(StackPanel.WidthProperty, new Binding() { Path = new System.Windows.PropertyPath("SpWidthCl") });

            var tb = new TextBlock() { Margin = new System.Windows.Thickness(2) };
            tb.TextWrapping = System.Windows.TextWrapping.Wrap;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            tb.Text = IntellisenseOperators.Text;
            sp.Children.Add(tb);

            tvi.Items.Add(sp);
        }
    }
}
