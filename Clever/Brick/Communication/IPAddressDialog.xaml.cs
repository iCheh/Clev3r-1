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
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Clever.Brick.Communication
{
    /// <summary>
    /// Interaction logic for NoBrickFoundDialog.xaml
    /// </summary>
    public partial class IPAddressDialog : Window
    {
        private IPAddress ipaddress;

        public IPAddressDialog()
        {
            ipaddress = null;
            InitializeComponent();
            address.Focus();
            butCancel.Text = MainWindowVM.GetLocalization["dButCancel"];
            butConnect.Text = MainWindowVM.GetLocalization["dButIpConnect"];
            headerText.Text = MainWindowVM.GetLocalization["dHeaderIp"];
        }

        public IPAddress GetAddress()
        {
            return ipaddress;
        }

        private void CancelButton_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void RetryButton_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ipaddress = IPAddress.Parse(address.Text);
                Close();
            }
            catch (Exception) { }
        }

        private void address_keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    ipaddress = IPAddress.Parse(address.Text);
                    Close();
                }
                catch (Exception) { }
            }
        }
    }
}
