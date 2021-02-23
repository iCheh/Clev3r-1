using Clever.CommonData;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Clever.Model.Utils
{
    class ProjectData
    {
        private Dictionary<string, ProgramData> data;
        public ProjectData()
        {
            data = new Dictionary<string, ProgramData>();
            ProjectName = "";
        }

        public string ProjectName { get; set; }

        public bool Add(string key, ProgramData value)
        {
            if (!data.ContainsKey(key))
            {
                try
                {
                    data.Add(key, value);
                    return true;
                }
                catch (Exception ex)
                {
                    Status.Clear();
                    //MainWindowVM.Status.Add(MainWindowVM.GetLocalization["brkFindErr"] + " " + errors.Count);
                    Status.Add(MainWindowVM.GetErrors["ePrjDataAdd"] + ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Remove(string name)
        {
            if (data.ContainsKey(name))
            {
                try
                {
                    return data.Remove(name);
                }
                catch (Exception ex)
                {
                    Status.Clear();
                    Status.Add(MainWindowVM.GetErrors["ePrjDataRemove"] + ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string, ProgramData> GetDictionary()
        {
            return data;
        }

        public string SelectedItemName()
        {
            string name = "";
            if (data.Count > 0)
            {
                var values = data.Values;
                foreach (var pd in values)
                {
                    if (pd.Item.IsSelected)
                    {
                        name = pd.Name;
                        break;
                    }
                }
            }
            return name;
        }

        public ProgramData GetProgramData(TabItem item)
        {
            if (item == null)
            {
                return null;
            }
            var pd = new ProgramData();
            var elem = new Elements();
            var name = elem.GetTabItemHeaderName(item);

            if (data.ContainsKey(name))
            {
                pd = data[name];
            }
            else
            {
                pd = null;
            }
            return pd;
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void UpdateColor()
        {
            if (data.Count > 0)
            {
                var values = data.Values;
                foreach (var pd in values)
                {
                    pd.SetHeaderColorText();
                }
            }
        }
    }
}
