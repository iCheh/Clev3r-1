using Clever.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clever.Brick.Communication
{
    internal class ConnectionObject
    {
        internal ConnectionObject(ManagementObject obj)
        {
            this.IsEmpty = true;

            // Разбор объекта
            var strDevice = GetProrertyString(obj, "DeviceID");

            if (strDevice.StartsWith("usb"))
            {
                var tmpInd = strDevice.LastIndexOf("\\");

                if (tmpInd > 0 && tmpInd + 12 < strDevice.Length)
                {
                    var tmpMac = strDevice.Substring(tmpInd + 1, 12);
                    foreach (var ch in tmpMac)
                    {
                        if (ch != '0')
                        {
                            this.Type = ConnectionObjectType.USB;
                            this.Name = "";
                            this.ComPort = "USB1";
                            this.MacAdress = tmpMac;
                            this.IsEmpty = false;
                            break;
                        }
                    }
                }
            }

            obj.Dispose();
        }

        internal ConnectionObject(ManagementObject obj1, ManagementObject obj2)
        {
            this.IsEmpty = true;

            // Разбор объекта
            var strDevice1 = GetProrertyString(obj1, "Name");
            var strDevice2 = GetProrertyString(obj2, "DeviceID");

            if (strDevice1.Contains("bluetooth") && strDevice2.Contains("bluetooth"))
            {
                
                var tmpInd1start = strDevice1.LastIndexOf("(");
                var tmpInd1stop = strDevice1.LastIndexOf(")");
                var tmpInd2 = strDevice2.LastIndexOf("_");

                if ((tmpInd2 > 0 && tmpInd2 + 12 < strDevice2.Length)
                    && (tmpInd1start > 0 && tmpInd1stop > 0 && tmpInd1stop > tmpInd1start
                    && tmpInd1stop - tmpInd1start >= 5))
                {
                    var tmpCom = strDevice1.Substring(tmpInd1start + 1, tmpInd1stop - (tmpInd1start + 1));
                    var tmpMac = strDevice2.Substring(tmpInd2 + 1, 12);

                    if (tmpCom.Contains("com") && tmpCom.Length > 3)
                    {
                        foreach (var ch in tmpMac)
                        {
                            if (ch != '0')
                            {
                                this.Type = ConnectionObjectType.COM;
                                this.Name = GetProrertyString(obj2, "Name", true);
                                this.ComPort = tmpCom.ToUpper();
                                this.MacAdress = tmpMac;
                                this.IsEmpty = false;
                                break;
                            }
                        }
                    }
                }
            }

            obj1.Dispose();
            obj2.Dispose();
        }

        internal ConnectionObjectType Type { get; private set; }

        internal string Name { get; private set; }

        internal string MacAdress { get; private set; }

        internal string ComPort { get; private set; }

        internal bool IsEmpty { get; private set; }


        private string GetProrertyString(ManagementObject obj, string propName, bool origin = false)
        {
            if (!origin)
            {
                return obj.Properties[propName].Value.ToString().ToLower().Trim();
            }

            return obj.Properties[propName].Value.ToString();
        }
    }
}
