/*  EV3-Basic: A basic compiler to target the Lego EV3 brick
    Copyright (C) 2015 Reinhard Grafl

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Clever.Brick.Communication
{
    /// <summary>
    /// Interaction logic for ConnectionTypeDialog.xaml
    /// </summary>
    public partial class ConnectionTypeDialog : Window
    {
        private int[] usbdevices;
        private IPAddress[] addresses;
        private object selected;
        private Dictionary<int, int> ports;

        public ConnectionTypeDialog(int[] usbdevices, IPAddress[] addresses)
        {
            this.usbdevices = usbdevices;
            this.addresses = addresses;
            this.selected = null;
            this.ports = new Dictionary<int, int>();

            InitializeComponent();

            headerText.Text = MainWindowVM.GetLocalization["dHeaderConnection"];
            butCancel.Text = MainWindowVM.GetLocalization["dButCancel"];
            butRetry.Text = MainWindowVM.GetLocalization["dButRetry"];

            int globInd = 0;
            int curComInd = 0;
            int curUsbInd = 0;
            int curWifiInd = 0;

            foreach (var p in ConObjList.ConCOMList)
            {
                PortList.Items.Add(p.Name);
                ports.Add(globInd, curComInd);
                globInd++;
                curComInd++;
            }

            foreach (int i in usbdevices)
            {
                string txt = "USB " + i;
                PortList.Items.Add(txt);
                ports.Add(globInd, curUsbInd);
                globInd++;
                curUsbInd++;
            }
            
            foreach (IPAddress a in addresses)
            {
                string txt = a.ToString();
                PortList.Items.Add(txt);
                ports.Add(globInd, curWifiInd);
                globInd++;
                curWifiInd++;
            }

            if (PortList.Items.Count > 0)
            {
                PortList.Focus();
                PortList.SelectedIndex = 0;
            }
        }

        public object GetSelectedPort()
        {
            //MessageBox.Show("return - " + selected);
            return selected;
        }

        private void CancelButton_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            selected = null;
            Close();
        }

        private void RetryButton_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            SetSelected();
        }

        private void WiFiButton_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            IPAddressDialog dialog = new IPAddressDialog();
            dialog.ShowDialog();
            IPAddress a = dialog.GetAddress();
            if (a != null)
            {
                selected = a;
                Close();
            }
        }


        private void PortList_selected(Object sender, EventArgs e)
        {
            SetSelected();
        }

        private void PortList_keydown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetSelected();
            }
        }

        private void SetSelected()
        {
            int idx = PortList.SelectedIndex;

            selected = null;

            if (idx <= ports.Count)
            {
                if (idx >= 0 && idx < ConObjList.ConCOMList.Count)
                {
                    selected = ConObjList.ConCOMList[ports[idx]].ComPort;
                }
                else if (idx >= ConObjList.ConCOMList.Count && idx < usbdevices.Length + ConObjList.ConCOMList.Count)
                {
                    selected = usbdevices[ports[idx]];
                }
                else if (idx >= usbdevices.Length + ConObjList.ConCOMList.Count && idx < usbdevices.Length + ConObjList.ConCOMList.Count + addresses.Length)
                {
                    selected = addresses[ports[idx]];
                }
            }

            Close();
            
        }
    }
}
