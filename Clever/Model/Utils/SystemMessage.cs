using Clever.ViewModel;

namespace Clever.Model.Utils
{
    class SystemMessage
    {
        public string ToNameNull
        {
            get
            {
                return MainWindowVM.GetLocalization["dToNameNull"];
            }
        }

        public string ToNameUndersgore
        {
            get
            {
                return MainWindowVM.GetLocalization["dToNameUndersgore"];
            }
        }

        public string ToNameNumber
        {
            get
            {
                return MainWindowVM.GetLocalization["dToNameNumber"];
            }
        }

        public string ToNameRegex
        {
            get
            {
                return MainWindowVM.GetLocalization["dToNameRegex1"] + "\n" + MainWindowVM.GetLocalization["dToNameRegex2"];
            }
        }

        public string ToNameLength
        {
            get
            {
                return MainWindowVM.GetLocalization["dToNameLength1"] + "\n" + MainWindowVM.GetLocalization["dToNameLength2"];
            }
        }
    }
}
