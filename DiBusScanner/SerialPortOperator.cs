using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using DiBusLib;
using System.Threading;
using DiBusScanner;
using System.Runtime.CompilerServices;

namespace ZyablikLib
{
    public delegate bool ReceiveMessagePOingConfirm(byte[] msg);
    /// <summary>
    /// Класс инкапсулирует работу с последовательным портом
    /// </summary>
    public class SerialPortOperator
    {
        private const int responceWaitingTimeout = 200;
        private const int repeatMaxCount = 3;
        
        public static string LogFileName = "log.txt";

        private SerialPort serialPort = null;

        public bool IsOpen
        {
            get { return this.serialPort != null && this.serialPort.IsOpen; }
        }

        public SerialPortOperator(SerialPort sp)
        {
            try
            {
                this.serialPort = sp;
                if (!serialPort.IsOpen)
                    this.serialPort.Open();
            }
            catch (Exception ex) { }
            //throw ex; }
        }

        ~SerialPortOperator()
        {
            if (serialPort == null && serialPort.IsOpen)
                this.serialPort.Close();
        }

        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public bool SendDiBusMessageWitnConfirm(byte[] command)
        {
            bool result = false;

            //if (serialPort == null ||
            //    !serialPort.IsOpen)
            //    return result;
            if (serialPort == null)
                return result;

            //if(!serialPort.IsOpen)
            //    serialPort.Open();

            try
            {
                // request command
                int repeatCount = 0;
                while (repeatCount < repeatMaxCount)
                {
                    serialPort.Write(command, 0, command.Length);

                    System.IO.File.AppendAllText(LogFileName, string.Format("Set RadioChannel ->:  {0}{1}", string.Join(" ", command), Environment.NewLine));

                    // wait responce
                    System.Threading.Thread.Sleep(responceWaitingTimeout);
                    int byteRecieved = serialPort.BytesToRead;
                    byte[] respBuffer = new byte[byteRecieved];
                    serialPort.Read(respBuffer, 0, byteRecieved);
                    System.IO.File.AppendAllText(LogFileName, string.Format("Set RadioChannel <-:  {0}{1}", string.Join(" ", command), Environment.NewLine));
                    // data processing
                    if (respBuffer.Length >= 14)
                    {
                        try
                        {
                            DiBusMessage newMsg = DiBusMessage.Parse(respBuffer);
                            if (newMsg.PackageType == PackageType.CommandConfirm)
                            {
                                result = true;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    repeatCount++;
                }
            }
            catch (Exception ex) { result = false; }
            return result;
        }


        public bool SendPing(SerialPort sp, byte[] receiverAddress)
        {
            bool result = false;

            if (serialPort == null || !
                serialPort.IsOpen)
                return result;
            //if (serialPort == null)
            //    return result;

            //if(!serialPort.IsOpen)
            //    serialPort.Open();

            DiBusMessage msg = new DiBusMessage();
            msg.ReceiverAddress = receiverAddress;
            msg.SourceAddress = new byte[] { 1, 1, 1 };
            msg.PackageType = PackageType.Ping;
            msg.DataType = DataType.UserType;

            byte[] command = msg.GetMessageByteArray();

            try
            {
                // request command
                int repeatCount = 0;
                while (repeatCount < repeatMaxCount)
                {
                    sp.Write(command, 0, command.Length);

                    int byteRecieved = serialPort.BytesToRead;
                    byte[] respBuffer = new byte[byteRecieved];
                    serialPort.Read(respBuffer, 0, byteRecieved);
                    // data processing
                    if (respBuffer.Length >= 14)
                    {
                        try
                        {
                            DiBusMessage newMsg = DiBusMessage.Parse(respBuffer);
                            if (newMsg.PackageType == PackageType.CommandConfirm)
                            {
                                result = true;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    repeatCount++;
                }
            }
            catch (Exception ex) { result = false; }
            return result;
        }


        public DiBusDevice FindDevice(byte[] receiverAddress)
        {
            DiBusDevice result = null;

            if (serialPort == null || !
                serialPort.IsOpen)
                return result;
            
            DiBusMessage msg = new DiBusMessage();
            msg.ReceiverAddress = receiverAddress;
            msg.SourceAddress = new byte[] { 1, 1, 1 };
            msg.PackageType = PackageType.Ping;
            msg.DataType = DataType.UserType;

            byte[] command = msg.GetMessageByteArray();

            try
            {
                // request command
                int repeatCount = 0;
                while (repeatCount < repeatMaxCount)
                {
                    serialPort.Write(command, 0, command.Length);
                    System.IO.File.AppendAllText(LogFileName, string.Format("command:  {0}{1}", string.Join(" ", command), Environment.NewLine));
                    Thread.Sleep(100);
                    int byteRecieved = serialPort.BytesToRead;
                    byte[] respBuffer = new byte[byteRecieved];
                    serialPort.Read(respBuffer, 0, byteRecieved);
                    System.IO.File.AppendAllText(LogFileName, string.Format("responce:  {0}{1}", string.Join(" ", respBuffer), Environment.NewLine));
                    // data processing
                    if (respBuffer.Length >= 14)
                    {
                        try
                        {
                            DiBusDevice device = new DiBusDevice();
                            DiBusMessage newMsg = DiBusMessage.Parse(respBuffer);
                            if (newMsg.PackageType == PackageType.CommandConfirm)
                            {
                                
                                device.Address = newMsg.SourceAddress;
                                device.PortName = this.serialPort.PortName;
                                device.Type = DiBusDevice.GetDevTypeByDiBusAddress(device.Address);
                                result = device;
                                //System.IO.File.AppendAllText(LogFileName, string.Format(
                                //    " !!! Device Found:  {0}{1}{2}{3}",
                                //    device.Type,
                                //    string.Join(".", device.Address),
                                //    device.RadioChannel,
                                //    Environment.NewLine));
                                break;
                            }
                        }
                        catch (DiBusLib.InvalidHeaderCRCException headerCHCEx) {
                            System.IO.File.AppendAllText(LogFileName, "InvalidHeaderCRCException: possible Collisions!!!!" + Environment.NewLine);}
                        catch (Exception ex) { }
                    }
                    repeatCount++;
                }
            }
            catch (Exception ex) { result = null; }
            return result;
        }

        public static void Read(SerialPort sp, byte[] cmd, ReceiveMessagePOingConfirm action)
        {
            bool _continue = true;

            Thread readThread = new Thread(()=> {
                while (_continue)
                {
                    try
                    {
                        int byteRecieved = sp.BytesToRead;
                        if (byteRecieved >= 14)
                        {
                            byte[] respBuffer = new byte[byteRecieved];
                            sp.Read(respBuffer, 0, byteRecieved);
                            action(respBuffer);
                        }                        
                    }
                    catch (TimeoutException) { break; }
                }
            });
            
            if(!sp.IsOpen)
                sp.Open();
            
            readThread.Start();

            sp.Write(cmd,0, cmd.Length);

            readThread.Join();
            sp.Close();
        }

        public byte[] GetData(byte[] cmd)
        {
            if (serialPort == null ||
                !serialPort.IsOpen)
                throw new Exception("Device Is Enabled!");

            int repeatCount = 0;
            int byteRecieved = 0;
            do
            {
                serialPort.Write(cmd, 0, cmd.Length);
                System.Threading.Thread.Sleep(responceWaitingTimeout);
                byteRecieved = serialPort.BytesToRead;
                if (byteRecieved >= 14)
                {
                    byte[] respBuffer = new byte[byteRecieved];
                    serialPort.Read(respBuffer, 0, byteRecieved);
                    return respBuffer;
                }
                repeatCount++;
            }
            while (repeatCount < repeatMaxCount);
            throw new Exception("Repeat Counter Overflow!");
        }

        public static bool ApplySerialPortSettings(System.IO.Ports.SerialPort sp, string name, int rate)
        {
            bool result = false;
            try
            {
                if (sp.IsOpen)
                    sp.Close();
                sp.PortName = name;
                sp.BaudRate = rate;
                sp.DataBits = 8;
                sp.Parity = System.IO.Ports.Parity.None;
                sp.StopBits = System.IO.Ports.StopBits.One;
                sp.Handshake = System.IO.Ports.Handshake.None;
                sp.WriteTimeout = 200;
                sp.ReadTimeout = 500;
                sp.ReceivedBytesThreshold = 14;
                sp.Open();
                result = true;
            }
            catch (Exception ex) 
            {
                result = false;
                //System.Windows.Forms.MessageBox.Show(ex.ToString()); 
                throw ex;
            }

            return result;
        }
    }
}
