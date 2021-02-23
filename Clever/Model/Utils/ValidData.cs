using System.Collections.Generic;
using System.Management;

namespace Clever.Model.Utils
{
    class ValidData
    {
        ManagementObjectSearcher searcher;
        Dictionary<string, string> ids;

        public ValidData()
        {
            ids = new Dictionary<string, string>();
        }

        public void GetData()
        {
            //процессор
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("ProcessorId", queryObj["ProcessorId"].ToString());

            //мать
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("CardID", queryObj["SerialNumber"].ToString());

            /*
            //клавиатура
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_KeyBoard");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("KeyBoardID", queryObj["DeviceId"].ToString());

            //ОС
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("OSSerialNumber", queryObj["SerialNumber"].ToString());

            //мышь
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PointingDevice");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("MouseID", queryObj["DeviceID"].ToString());

            //звук
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("SoundCardID", queryObj["DeviceID"].ToString());

            //CD-ROM
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("CDROMID", queryObj["DeviceID"].ToString());

            //UUID
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT UUID FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("UUID", queryObj["UUID"].ToString());

            */

            string data = "";
            foreach (var x in ids)
                data += x.Key + ": " + x.Value + "\r\n";

            //MessageBox.Show(data);
        }
    }
}
