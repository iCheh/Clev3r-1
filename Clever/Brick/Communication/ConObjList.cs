using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows;

namespace Clever.Brick.Communication
{
    internal static class ConObjList
    {

        internal static List<ConnectionObject> ConUSBList { get; private set; }

        internal static List<ConnectionObject> ConCOMList { get; private set; }

        internal static void CreateList()
        {
            ConUSBList = new List<ConnectionObject>();
            ConCOMList = new List<ConnectionObject>();

            SetManagementObjects();

            //ShowLists();
        }


        private static void SetManagementObjects()
        {

            // Найденные порты
            ManagementObjectCollection Ports;
            // Поисковик данных
            ManagementObjectSearcher Ports_Found;
            // Находим порты по запросу
            //Ports_Found = new ManagementObjectSearcher("Select * from Win32_SerialPort");
            Ports_Found = new ManagementObjectSearcher("Select * from Win32_PnPEntity");
            // Записываем полученные данные
            Ports = Ports_Found.Get();


            // Первый проход
            foreach (ManagementObject Port in Ports)
            {
                try
                {
                    var prop = Port.Properties;

                    if (prop.Count == 0)
                    {
                        break;
                    }

                    var tmpName = prop["Name"].Value.ToString().ToLower();

                    if (!tmpName.Contains("usb") && !tmpName.Contains("com"))
                    {
                        continue;
                    }

                    var tmpDevID = prop["DeviceID"].Value.ToString().ToLower();

                    if (tmpDevID.Contains("usb") && tmpDevID.Contains("0694") && tmpDevID.Contains("0005"))
                    {
                        ConUSBList.Add(new ConnectionObject(Port));
                    }
                    else if (tmpName.Contains("(com"))
                    {
                        var tmpInd = tmpDevID.LastIndexOf("&");
                        var tmpMAC = "";

                        if (tmpInd > 0 && tmpInd + 13 < tmpDevID.Length)
                        {
                            tmpMAC = tmpDevID.Substring(tmpInd + 1, 12);
                        }
                        else
                        {
                            continue;
                        }

                        // Второй проход для COM
                        foreach (ManagementObject prt in Ports)
                        {
                            var prop2 = prt.Properties;
                            var tmpDevID2 = prop2["DeviceID"].Value.ToString().ToLower();

                            if (!tmpDevID2.Contains("usb") && tmpDevID2 != tmpDevID && tmpDevID2.Contains(tmpMAC))
                            {
                                ConCOMList.Add(new ConnectionObject(Port, prt));
                            }
                        }

                    }
                }
                catch
                {
                    continue;
                }

            }

        }
    
        private static void ShowLists()
        {
            string str = "";

            if (ConUSBList.Count > 0)
            {
                str += "USB" + "\n";
            }

            foreach(var l in ConUSBList)
            {
                str += l.ComPort + " => " + l.MacAdress + "\n";
            }

            if (ConCOMList.Count > 0)
            {
                str += "COM" + "\n";
            }

            foreach (var l in ConCOMList)
            {
                str += l.ComPort + " => " + l.Name + " => " + l.MacAdress + "\n";
            }

            MessageBox.Show("LIST PORTS" + "\n" + "\n" + str);
        }
    }
}
