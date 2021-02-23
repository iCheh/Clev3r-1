using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.CommonData
{
    internal static class Status
    {
        internal static ObservableCollection<string> Items { get; set; }

        #region Utils

        internal static void Clear()
        {
            Items.Clear();
            StatusChanged(Items, new EventArgs());
        }

        internal static int Count
        {
            get { return Items.Count; }
        }

        internal static string Text
        {
            set
            {
                Items.Clear();
                var sr = new StringReader(value);
                var line = sr.ReadLine();
                while (line != null)
                {
                    Items.Add(line);
                    line = sr.ReadLine();
                }
                StatusChanged(Items, new EventArgs());
            }
        }

        internal static void Add(string text)
        {
            Items.Add(text);
            StatusChanged(Items, new EventArgs());
        }

        internal static void Update()
        {
            var obj = Items;
            Items = obj;
            StatusChanged(Items, new EventArgs());
        }

        public static event EventHandler StatusChanged;

        #endregion
    }
}
