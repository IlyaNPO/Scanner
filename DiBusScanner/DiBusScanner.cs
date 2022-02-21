using DiBusLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using ZyablikLib;

namespace DiBusScanner
{

    public class DevicesScanner
    {
        SerialPort Interface = null;
        SerialPortOperator spOperator = null;

        DiBusDevice mbs2 = null;
        
        public byte Mbs2RadioChannel
        {
            set 
            {
                if (mbs2 != null)
                    mbs2.RadioChannel = value;
            }
        }

        List<DiBusDevice> devices = new List<DiBusDevice>();
        
        private bool flagStopThreads = true;

        public DevicesScanner(SerialPort sp)
        {
            this.Interface = sp;
            spOperator = new SerialPortOperator(Interface);
            
            this.mbs2 = spOperator.FindDevice(new byte[] { 0x29, 0x02, 0xff });
            mbs2.PortName = this.Interface.PortName;
            mbs2.RadioChannel = GetRadioChannelNumber(mbs2);
            this.devices.Add(mbs2);
        }


        public int GetRadioChannelNumber(DiBusDevice mbs2)
        {
            int res = -1;

            try
            {
                DiBusMessage msg = new DiBusMessage();
                msg.SourceAddress = new byte[] { 0x01, 0x01, 0x01 };
                msg.ReceiverAddress = mbs2.Address;
                msg.PackageType = PackageType.RequestDataFromSlave;
                msg.DataType = DataType.UserType;
                msg.DataArray = new byte[] { 0x00 };

                byte[] data = this.spOperator.GetData(msg.GetMessageByteArray());

                try
                {
                    DiBusMessage responceMessage = DiBusMessage.Parse(data);
                    mbs2.Address = responceMessage.SourceAddress;
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }

                var result = (new DiBusProvider()).ReseiveData(data);
                if (result[0] is byte)
                {
                    res = (byte)result[0];
                }
            }
            catch (Exception ex) { res = -1; }
            return res;
        }


        public bool WriteRadioChannelNumToMBS2(byte channelNum)
        {
            DiBusMessage msg = new DiBusMessage();
            msg.SourceAddress = new byte[] { 0x01, 0x01, 0x01 };
            msg.ReceiverAddress = this.mbs2.Address; // address; // new byte[] { 0x29, 0x02, 0xFF };
            msg.PackageType = PackageType.MasterSendData;
            msg.DataType = DataType.UserType;
            msg.DataArray = new byte[] { 0x00, 0x04, 0x01, 0x01, 0x01, 0x01, 0x05, 0x0a, 0x00, 0x03 };
            msg.DataArray[6] = channelNum;

            byte[] msgByte = msg.GetMessageByteArray();
            return spOperator.SendDiBusMessageWitnConfirm(msgByte);
        }


        private void GetDevice(DiBusDevice device, int radiochannel)
        {
            if (device != null)
            {
                device.RadioChannel = radiochannel;

                if (device.Type != DeviceType.MBS2 || !ListHasSameDevice(this.devices, device))
                {
                    this.devices.Add(device);
                }
            }
        }

        public DiBusDevice[] GetDevices(byte radiochannel)
        {
            var mbs2 = spOperator.FindDevice(new byte[] { 0x29, 0x02, 0xff });
            mbs2.PortName = this.Interface.PortName;
            mbs2.RadioChannel = radiochannel;
            GetDevice(mbs2, radiochannel);

            var mbs3 = spOperator.FindDevice(new byte[] { 0x29, 0x05, 0xff });
            mbs3.PortName = this.Interface.PortName;
            mbs3.RadioChannel = radiochannel;
            GetDevice(mbs3, radiochannel);

            var pult = spOperator.FindDevice(new byte[] { 0x03, 0xC8, 0xFF });
            pult.PortName = this.Interface.PortName;
            pult.RadioChannel = radiochannel;
            GetDevice(pult, radiochannel);

            var device = spOperator.FindDevice(new byte[] { 0xff, 0xff, 0xff });
            device.PortName = this.Interface.PortName;
            device.RadioChannel = radiochannel;
            GetDevice(device, radiochannel);
            
            return null;
        }


        private bool ListHasSameDevice(List<DiBusDevice> devList, DiBusDevice dev)
        {
            foreach (var item in devList)
            {
                if (string.Join(".", item.Address) == string.Join(".", dev.Address) && 
                    item.PortName == dev.PortName)
                    return true;
            }

            return false;
        }

        public void FindDevices(byte radioChannelFrom, byte radioChannelTo, Action<int> findProcessAction, Action findFinishAction)
        {
            flagStopThreads = false;

            for (byte i = radioChannelFrom; i <= radioChannelTo; i++)
            {
                if (flagStopThreads)
                    break;
                
                WriteRadioChannelNumToMBS2(i);

                var mbs2 = spOperator.FindDevice(new byte[] { 0x29, 0x02, 0xff });
                GetDevice(mbs2, i);
                var mbs3 = spOperator.FindDevice(new byte[] { 0x29, 0x05, 0xff });
                GetDevice(mbs3, i);
                var pult = spOperator.FindDevice(new byte[] { 0x03, 0xC8, 0xFF });
                GetDevice(pult, i);
                var device = spOperator.FindDevice(new byte[] { 0xff, 0xff, 0xff });
                GetDevice(device, i);
                if (findProcessAction != null)
                    findProcessAction(i);
            }
            if (findFinishAction != null)
                findFinishAction();
        }


        public void RestoreMBS2RadioChannel()
        {
            this.WriteRadioChannelNumToMBS2((byte)mbs2.RadioChannel);
        }

        public bool WriteRadioChannelToUPI(DiBusDevice device, byte radioChannel)
        {
            bool result = false;
            DiBusMessage msg = new DiBusMessage();
            msg.ReceiverAddress = device.Address;
            msg.SourceAddress = new byte[] { 1, 1, 1 };
            msg.PackageType = PackageType.MasterSendData;
            msg.DataType = DataType.UserType;
            msg.DataArray = new byte[] { 32, 2, 1, 1, radioChannel, 0 };

            if (this.spOperator.SendDiBusMessageWitnConfirm(msg.GetMessageByteArray()))
            {
                device.RadioChannel = radioChannel;
                result = true;
            }
            return result;
        }


        public bool WriteRadioChannelToMBS3(DiBusDevice device, byte radioChannel)
        {
            bool result = false;
            DiBusMessage msg = new DiBusMessage();
            msg.ReceiverAddress = device.Address;
            msg.SourceAddress = new byte[] { 1, 1, 1 };
            msg.PackageType = PackageType.MasterSendData;
            msg.DataType = DataType.Byte;
            msg.DataArray = new byte[] { 0xFE, radioChannel };

            if (this.spOperator.SendDiBusMessageWitnConfirm(msg.GetMessageByteArray()))
            {
                device.RadioChannel = radioChannel;
                result = true;
            }
            return result;
        }

        internal void SetRadioChannel(DiBusDevice device, byte radioChannel)
        {
            if (device == null)
                return;
            
            switch (device.Type)
            {
                case DeviceType.MBS2:
                    this.mbs2.RadioChannel = radioChannel;
                    WriteRadioChannelNumToMBS2(radioChannel);
                    break;
                case DeviceType.BDGamma:
                    break;
                case DeviceType.Pult:
                    // меняем радиоканал на МБС2
                    WriteRadioChannelNumToMBS2((byte)device.RadioChannel);
                    // меняем радиоканал текущего устройства на новый

                    WriteRadioChannelToUPI(device, radioChannel);
                    break;
                case DeviceType.MBS3:
                    // перестраиваем МБС2 на радиоканал текущего устройства
                    WriteRadioChannelNumToMBS2((byte)device.RadioChannel);
                    // меняем адрес текущего устройства на новый
                    WriteRadioChannelToMBS3(device, radioChannel);
                    break;
                case DeviceType.BDBeta:
                    break;
                case DeviceType.Other:
                    break;
                default:
                    break;
            }
        }
    }

}
