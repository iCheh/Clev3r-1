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
using Clever.CommonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Management;
using System.Drawing;

namespace Clever.Brick.Communication
{
    public class ConnectionFinder
    {

        public static EV3Connection CreateConnection(bool isUIThread, bool automaticallyUseSingleUSB)
        {
            //Dictionary<string, string> btDict = null;
            //Dictionary<string, string> objDict = null;
            EV3Connection c = null;
            ConObjList.CreateList();

            //return new EV3ConnectionWiFi("10.0.0.140");

            // retry multiple times to open connection
            for (; ; )
            {
                if (c != null && c.IsOpen())
                {
                    c.Close();
                    c = null;
                }

                // check which EV3 devices are connected via USB
                int[] usbdevices = EV3ConnectionUSB.FindEV3s();

                // if there is exactly one, try to open it
                if (automaticallyUseSingleUSB && usbdevices.Length == 1)
                {
                    try
                    {
                        return TestConnection(new EV3ConnectionUSB(usbdevices[0]));
                    }
                    catch (Exception)
                    { 
                    }
                }

                //Попытка авто подключения к через COM Bluetooth
                if (ConObjList.ConCOMList.Count == 1)
                {
                    var tmpPort = ConObjList.ConCOMList[0].ComPort.ToUpper();
                    c = TestConnection(new EV3ConnectionBluetooth(tmpPort));
                    if (c != null && c.IsOpen())
                    {
                        return c;
                    }
                }
                

                // in this case also check if there are some IP addresses configured as possible connection targets
                IPAddress[] addresses = LoadPossibleIPAddresses();


                // Create and show the window to select one of the connection possibilities
                object port_or_device = DoModalConnectionTypeDialog(isUIThread, usbdevices, addresses);

                if (port_or_device == null)
                {
                    throw new Exception("User canceled connection selection");
                }
                try
                {
                    if (port_or_device is string)
                    {
                        c = TestConnection(new EV3ConnectionBluetooth((string)port_or_device));
                        return c;
                    }
                    else if (port_or_device is int)
                    {
                        c = TestConnection(new EV3ConnectionUSB((int)port_or_device));
                        return c;
                    }
                    else if (port_or_device is IPAddress)
                    {
                        IPAddress addr = (IPAddress)port_or_device;
                        c = TestConnection(new EV3ConnectionWiFi(addr));
                        if (!addresses.Contains(addr) && c.IsOpen())
                        {
                            AddPossibleIPAddress(addr);
                        }
                        return c;
                    }
                }
                catch (Exception)
                { }     // if not possible, retry
            }
        }

        private static EV3Connection TestConnection(EV3Connection con)
        {
            try
            {
                // perform a tiny direct command to check if communication works
                ByteCodeBuffer c = new ByteCodeBuffer();
                c.OP(0x30);           // Move8_8
                c.CONST(74);
                c.GLOBVAR(0);
                byte[] response = con.DirectCommand(c, 1, 0);
                if (response == null || response.Length != 1 || response[0] != 74)
                {
                    throw new Exception("Test DirectCommand delivers wrong result");
                }
                return con;
            }
            catch (Exception e)
            {
                con.Close();
                throw e;
            }
        }

        private static object DoModalConnectionTypeDialog(bool isUIThread, int[] usbdevices, IPAddress[] addresses)
        {
            Window dialog = null;
            

            // simple operation when called from an UI thread
            if (isUIThread)
            {
                dialog = new ConnectionTypeDialog(usbdevices, addresses);
                dialog.ShowDialog();
            }
            // when being called from a non-UI-thread, must create an own thread here
            else
            {
                // Create an extra thread for the dialog window
                Thread newWindowThread = new Thread(new ThreadStart(() =>
                {
                    Window window = (Window)new ConnectionTypeDialog(usbdevices, addresses);
                    // When the window closes, shut down the dispatcher
                    window.Closed += (s, e) =>
                       Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    window.Show();
                    // let other thread get hold the window to check for finish
                    dialog = window;
                    // Start the Dispatcher Processing
                    System.Windows.Threading.Dispatcher.Run();
                }));
                // Set the apartment state
                newWindowThread.SetApartmentState(ApartmentState.STA);
                // Make the thread a background thread
                newWindowThread.IsBackground = true;
                // Start the thread
                newWindowThread.Start();

                // wait here until the window actually was created and user has answered the prompt or closed the window...
                while (dialog == null || dialog.IsVisible)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            return ((ConnectionTypeDialog)dialog).GetSelectedPort();
        }


        private static IPAddress[] LoadPossibleIPAddresses()
        {
            List<IPAddress> addresses = new List<IPAddress>();

            string fileName = Configurations.Get.ConfigNameWifi;

            if (!File.Exists(fileName))
            {
                return addresses.ToArray();
            }

            try
            {
                //string fileName = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Clever"), "ipaddresses.txt");
                StreamReader file = new StreamReader(fileName);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        addresses.Add(IPAddress.Parse(line));
                    }
                    catch (Exception) { }
                }
                file.Close();
            }
            catch (Exception)
            {
                Status.Add(GUILanguage.GetItem("prepRead1") + " " + fileName + " " + GUILanguage.GetItem("prepRead2"));
            }

            return addresses.ToArray();
        }

        private static void AddPossibleIPAddress(IPAddress a)
        {
            string fileName = Configurations.Get.ConfigNameWifi;

            try
            {
                string dir = Configurations.Get.FullPath;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                StreamWriter file = new StreamWriter(fileName, true);
                file.WriteLine(a.ToString());
                file.Close();
            }
            catch (Exception)
            {
                Status.Add(GUILanguage.GetItem("prepRead1") + " " + fileName + " " + GUILanguage.GetItem("prepRead2"));
            }
        }
    }
}
