using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Clever.CommonData;
using Clever.ViewModel;

namespace Clever.Model.Utils
{
    internal class WaveSoundPayer
    {
        private SoundPlayer player = new SoundPlayer();
        internal void Play(string name)
        {
            try
            {
                string uri = "pack://application:,,,/Resources/Media/wav/" + name + ".wav";
                player.Stream = Application.GetResourceStream(new Uri(uri)).Stream;
                player.Play();
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add(ex.Message);
            }
        }
    }
}
