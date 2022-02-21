using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiBusScanner
{
    public class DiBusDevice
    {
        public DeviceType Type = DeviceType.Other;
        public byte[] Address = new byte[3];
        public string Comment = string.Empty;
        public string PortName = string.Empty;
        public int RadioChannel = -1;

        public static DeviceType GetDevTypeByDiBusAddress(byte[] address)
        {
            DeviceType devType = DeviceType.Other;
            if (address[0] == 0x29 && address[1] == 0x02)
                devType = DeviceType.MBS2;
            else if (address[0] == 0x03 && address[1] == 0xC8)
                devType = DeviceType.Pult;
            else if (address[0] == 0x33 && address[1] == 0xC8)
                devType = DeviceType.BDGamma;
            //else if (address[0] == 0x03 && address[1] == 0xC8 && address[2] == 0xFE)
            //    devType = DeviceType.MBS3;
            else if (address[0] == 0x29 && address[1] == 0x05)
                devType = DeviceType.MBS3;
            else if (address[2] == 0x29 && address[1] == 0x05)
                devType = DeviceType.MBS3;
            else if (address[0] == 0x2 && address[1] == 0x05)
                devType = DeviceType.MBS3;
            else if (address[0] == 0x07 && address[1] == 0x0F)
                devType = DeviceType.BDBeta;
            else if (address[0] == 0x07 && address[1] == 0x16)
                devType = DeviceType.BDAlpha;
            else if (address[0] == 17 && address[1] == 2)
                devType = DeviceType.BDNeutron;
            else
                devType = DeviceType.Other;

            return devType;
        }

        internal static List<object> ConvertToGpsData(List<object> s)
        {
            List<object> result = new List<object>();

            try
            {
                DateTime dt = new DateTime(2008, 1, 1);
                dt = dt.AddSeconds((double)s[0]);

                result.Add(dt);
                result.Add((double)s[1] * 57.29577951308232);
                result.Add((double)s[2] * 57.29577951308232);
                result.Add(s[3]);
                result.Add(s[4]);

                UInt32 ssp = (UInt32)s[5];

                bool flagNavigationTask = BitStatus(ssp, 19);
                bool flagReceiverState = BitStatus(ssp, 23);
                bool flagTestReserveOZU = BitStatus(ssp, 0);
                bool flagTestRST = BitStatus(ssp, 1);
                bool flagTelemetriyaPLL = BitStatus(ssp, 2);
                bool flagSettingsFromFlashToOZU = BitStatus(ssp, 3);
                bool flagTelemetriyaARU_GPS = BitStatus(ssp, 4);
                bool flagTelemetriyaARU_GLONASS = BitStatus(ssp, 5);

                result.Add(flagNavigationTask ? 1 : 0);
                result.Add(flagReceiverState ? 1 : 0);
                result.Add(flagTestReserveOZU ? 1 : 0);
                result.Add(flagTestRST ? 1 : 0);
                result.Add(flagTelemetriyaPLL ? 1 : 0);
                result.Add(flagSettingsFromFlashToOZU ? 1 : 0);
                result.Add(flagTelemetriyaARU_GPS ? 1 : 0);
                result.Add(flagTelemetriyaARU_GLONASS ? 1 : 0);

                //result.Add(s[6]);
                result.Add(s[7]);
                result.Add(s[8]);
                result.Add(s[9]);
            }
            catch (Exception ex) { return s; }
            /*
            "Время приемника, с (UTC). Нулевое значение соотв. 01.01.2008",
            "Широта",
            "Долгота",
            "Высота над эллипсоидом, м",
            "Количество спутников",
            "ССП",
            "Флаг достоверности решения НЗ: 0 - недостоверно, иначе - достоверно",
            "Количество достоверных решений подряд",
            "Плановая скорость, м/с",
            "Курс, рад
             */


            return result;
        }

        public static bool BitStatus(byte reg, byte bit)
        {
            return ((reg & (1 << bit)) >> (bit)) == 1;
        }

        public static bool BitStatus(UInt32 reg, byte bit)
        {
            return ((reg & (1 << bit)) >> (bit)) == 1;
        }
    }

    public enum DeviceType
    {
        MBS2 = 0,
        BDGamma = 1,
        Pult = 2,
        MBS3 = 3,
        BDBeta = 4,
        BDAlpha = 5,
        BDNeutron = 6,
        Other = 7
    }

    enum Registers
    {
        DBGammaDataRegister = 0x03,
        DBBetaDataRegister = 0x60,
        DBAlphaDataRegister = 0x40,
        PultGPSDataRegister = 0x83,
        DBNeutronDataRegister = 0x90,
    }

    interface IZyablickSubdevice
    {
        bool SetRadioChannel(byte radioChannel);
        List<object> GetData();
        bool SetDiBusAddress(byte[] address);
        bool GetDiBusAddress(byte[] address);
    }
}
