using Clever.Model.Intellisense;
using Clever.Model.Utils;
using Clever.View.Content;
using Clever.View.Controls.Helps;
using Clever.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Clever.ViewModel.PanelsVM
{
    internal class HelperPanelVM : BaseViewModel
    {
        internal static HelperPanelVM Get { get; set; }
        private Help HelpContent;
        private HelpToolTip HelpToolTip;

        internal HelperPanelVM()
        {
            Install();
        }

        #region Bindings

        private double _spWidth;
        public double SpWidth
        {
            get { return _spWidth; }
            set
            {
                _spWidth = value;
                OnPropertyChanged("SpWidth");
            }
        }

        private double _spWidthCl;
        public double SpWidthCl
        {
            get { return _spWidthCl; }
            set
            {
                _spWidthCl = value;
                OnPropertyChanged("SpWidthCl");
            }
        }

        private double _widthHelpPanel;
        public double WidthHelpPanel
        {
            get { return _widthHelpPanel; }
            set
            {
                _widthHelpPanel = value;
                OnPropertyChanged("WidthHelpPanel");
            }
        }

        private double _heightHelpPanel;
        public double HeightHelpPanel
        {
            get { return _heightHelpPanel; }
            set
            {
                _heightHelpPanel = value;
                OnPropertyChanged("HeightHelpPanel");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                OnPropertyChanged("Summary");
            }
        }

        private object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        #endregion

        #region Configurations

        private void Install()
        {
            HelpContent = new Help();
            HelpToolTip = new HelpToolTip();

            HelpToolTipModel.Name = "";
            HelpToolTipModel.Summary = "";

            OpenHelp();
        }

        #endregion

        #region Utils

        internal void OpenHelp()
        {
            Content = HelpContent;
        }

        internal void OpenToolTip()
        {
            Name = HelpToolTipModel.Name;
            Summary = HelpToolTipModel.Summary;
            Content = HelpToolTip;
        }

        #endregion
    }
}
