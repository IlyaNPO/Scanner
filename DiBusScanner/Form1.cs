using DiBusLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZyablikLib;
using INIFileLibrary;
using DetectorBlockViewerLibrary;
using TranslationLib;

namespace DiBusScanner
{
    public partial class Form1 : Form
    {
        Translation translation = null;
        IniFile iniFile = new IniFile("scannerSettings.ini");
        //SerialPortOperator spOperator = null;
        DevicesScanner scanner = null;
        bool flagStopThreads = false;



        SerialPortOperator serialPortOperator = null;
        
        DetectorBlockViewerLibrary.DBMaster dbMaster = null;
        DetectorBlockViewerLibrary.DBDataViewer dbViewer = null;


        int RadioChannel = -1;

        byte DeviceOldRadioChannel = 0;
        DiBusDevice currentDevice = null;

        List<DiBusDevice> devices = new List<DiBusDevice>();
        public Form1()
        {
            InitializeComponent();

            this.translation = new Translation();
            translation.SetLanguage(EnabledLanguages.Russian);

            // Программа может запускаться двумя способами:
            // 1) независимо,
            // в этом случае она берет настройки ком-порта из ini-файла,
            LoadSettingsFromINI();

            // 2) командой process.Start() как доп.модуль из ПО DoseAssistant,
            // в этом случае настройки ком-порта передает запускающая программа как аргументы коммандной строки
            try
            {
                if (!LoadSerialPortParamsFromCommandLine())
                    LoadSerialPortParamsFromINI();

                serialPortOperator = new SerialPortOperator(this.serialPort1);

                scanner = new DevicesScanner(this.serialPort1);

                DisplaySerialPortInfo();

                dbViewer = new DBDataViewer();

            }
            catch (Exception ex) { this.toolStripStatusLabelInfo.Text = ex.Message; }

            this.toolStripStatusLabelInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        }

        #region SerialPortSettings
        private void LoadSerialPortParamsFromINI()
        {
            string portName = string.Empty;
            int baudRate = 0;

            if (iniFile.KeyExists("PortName", "SerialPort"))
                portName = iniFile.ReadINI("SerialPort", "PortName");

            if (iniFile.KeyExists("BaudRate", "SerialPort"))
                baudRate = Int32.Parse(iniFile.ReadINI("SerialPort", "BaudRate"));

            SerialPortOperator.ApplySerialPortSettings(this.serialPort1, portName, baudRate);
        }

        private void SaveSerialPortParamsToINI()
        {
            iniFile.Write("SerialPort", "PortName", this.serialPort1.PortName);
            iniFile.Write("SerialPort", "BaudRate", this.serialPort1.BaudRate.ToString());
        }

        private void DisplaySerialPortInfo()
        {
            try
            {
                string msg = "Connected to";
                if (translation.Table.ContainsKey("msgConnectedTo"))
                    msg = translation.Table["msgConnectedTo"];
                this.toolStripStatusLabelInfo.Text = string.Format("{0} {1}, {2}", msg, this.serialPort1.PortName, this.serialPort1.BaudRate);
            }
            catch (Exception ex) { this.toolStripStatusLabelInfo.Text = ex.Message; }
        }

        private void LoadSettingsFromINI()
        {
            if(iniFile. KeyExists("FindFrom", ""))
            {
                this.tbxScanFrom.Text = iniFile.ReadINI("","FindFrom");
            }

            if (iniFile.KeyExists("FindTo", ""))
            {
                this.tbxScanTo.Text = iniFile.ReadINI("", "FindTo");
            }
        }

        private void SaveSettingsToINI()
        {
            //iniFile.
            iniFile.Write("", "FindFrom", this.tbxScanFrom.Text);
            iniFile.Write("", "FindTo", this.tbxScanTo.Text);
        }

        /// <summary> Метод вызывается в том случае
        /// </summary>
        /// <returns></returns>
        private bool LoadSerialPortParamsFromCommandLine()
        {
            bool result = false;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length >= 3)
            {
                try
                {
                    //this.serialPort1.PortName = args[1];ени
                    //this.serialPort1.BaudRate = int.Parse(args[2]);
                    SerialPortOperator.ApplySerialPortSettings(this.serialPort1, args[1], int.Parse(args[2]));
                    result = true;
                }
                catch (UnauthorizedAccessException uaEx) 
                { 
                    result = false;
                    this.toolStripStatusLabelInfo.Text = uaEx.Message;
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                }
            }
            return result;
        }




        #endregion SerialPortSettings


        private bool LoadLanguageFromCommandLine()
        {
            bool result = false;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 3)
            {
                try
                {
                    if (args[3].ToLower() == "en")
                        TranslateUI(EnabledLanguages.English);
                    else
                        TranslateUI(EnabledLanguages.Russian);
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                }
            }
            return result;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.DisplayNoDevice();
            this.flagStopThreads = false;
            this.toolStripProgressBar1.Visible = true;
            this.devices.Clear();
            this.lvDevices.Items.Clear();

            byte minRadioChannel = byte.Parse(this.tbxScanFrom.Text);
            byte maxRadioChannel = byte.Parse(this.tbxScanTo.Text);
            this.toolStripProgressBar1.Minimum = minRadioChannel;
            this.toolStripProgressBar1.Value = minRadioChannel;
            this.toolStripProgressBar1.Maximum = maxRadioChannel; 

            this.btnFind.Visible = false;

            SerialPortOperator.LogFileName = string.Format("log_{0}.txt", DateTime.Now.ToString("ddMMyyyy_hh-mm-ss"));

            try
            {
                Thread threadFind = new Thread(() => { 
                    for (byte i = minRadioChannel; i <= maxRadioChannel; i++)
                    {
                        if (flagStopThreads)
                            break;

                        try
                        {
                            WriteRadioChannelNum(serialPortOperator, i);
                            // 
                            var mbs2 = serialPortOperator.FindDevice(new byte[] { 0x29, 0x02, 0xff });
                            GetDevice(mbs2, i);
                            var mbs3 = serialPortOperator.FindDevice(new byte[] { 0x29, 0x05, 0xff });
                            GetDevice(mbs3, i);
                            //var BDBeta = spOperator.FindDevice(new byte[] { 0x07, 0x0F, 0xFF });
                            //GetDevice(BDBeta, i);
                            var pult = serialPortOperator.FindDevice(new byte[] { 0x03, 0xC8, 0xFF });
                            GetDevice(pult, i);
                            var device = serialPortOperator.FindDevice(new byte[] { 0xff, 0xff, 0xff });
                            GetDevice(device, i);
                            //this.SendPing(this.serialPort1, new byte[] { 0xff, 0xff, 0xff });
                            //System.Threading.Thread.Sleep(100);
                        }
                        catch(Exception ex)
                        {
                            Invoke(new MethodInvoker(delegate { toolStripStatusLabelCounter.Text = ex.Message; }));
                            break;
                        }

                        Invoke(new MethodInvoker(delegate {
                            try
                            {
                                this.toolStripProgressBar1.Value = i;
                                toolStripStatusLabelCounter.Text = "" + i;
                            }
                            catch (NullReferenceException nrEx) { }
                        }));
                    }
                    Invoke(new MethodInvoker(delegate {
                        try
                        { 
                            this.toolStripProgressBar1.Visible = false;
                            this.btnFind.Visible = true;
                            toolStripStatusLabelCounter.Text = string.Empty;
                        }
                        catch (NullReferenceException nrEx) { }
                    }));
                });
                threadFind.IsBackground = true;
                threadFind.Start();
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.Message);
                try
                {
                    this.toolStripProgressBar1.Visible = false;
                    this.btnFind.Visible = true;
                    toolStripStatusLabelCounter.Text = string.Empty;
                }
                catch (NullReferenceException nrEx) { }
            }
            
        }

        private void GetDevice(DiBusDevice device, int radiochannel)
        {
            if (device != null)
            {
                device.RadioChannel = radiochannel;

                if (device.Type != DeviceType.MBS2 || !ListHasDevice(this.devices, device))
                {
                    this.devices.Add(device);
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.lvDevices.Items.Clear();
                        foreach (var dev in this.devices)
                        {
                            ListViewItem lvItem = new ListViewItem();
                            // OLD
                            //lvItem.SubItems.Add(string.Join(".", dev.Address));
                            ////lvItem.SubItems.Add(dev.PortName);
                            //lvItem.SubItems.Add(dev.Type.ToString());
                            //lvItem.SubItems.Add(dev.Name);
                            //lvItem.SubItems.Add(dev.RadioChannel.ToString());
                            lvItem.ImageIndex = (int)dev.Type;
                            // NEW
                            lvItem.SubItems.AddRange(new string[] { string.Join(".", dev.Address), dev.RadioChannel.ToString(), dev.Type.ToString(), dev.Comment });
                            lvDevices.Items.Add(lvItem);
                        }
                    }));
                }
            }
        }


        public bool WriteRadioChannelNum(SerialPortOperator spOperator, byte channelNum)
        {
            DiBusMessage msg = new DiBusMessage();
            msg.SourceAddress = new byte[] { 0x01, 0x01, 0x01 };
            msg.ReceiverAddress = new byte[] { 0x29, 0x02, 0xFF };
            msg.PackageType = PackageType.MasterSendData;
            msg.DataType = DataType.UserType;
            msg.DataArray = new byte[] { 0x00, 0x04, 0x01, 0x01, 0x01, 0x01, 0x05, 0x0a, 0x00, 0x03 };
            msg.DataArray[6] = channelNum;

            byte[] msgByte = msg.GetMessageByteArray();
            return spOperator.SendDiBusMessageWitnConfirm(msgByte);
        }

        private bool AddDevice(byte[] data)
        {
            bool res = false;
            DiBusMessage newMsg = DiBusMessage.Parse(data);
            if (newMsg.PackageType == PackageType.CommandConfirm)
            {
                DiBusDevice device = new DiBusDevice();
                device.Address = newMsg.SourceAddress;
                device.PortName = this.serialPort1.PortName;
                this.devices.Add(device);
                this.Invoke(new MethodInvoker(delegate {
                    this.lvDevices.Items.Clear();
                    foreach (var dev in this.devices)
                    {
                        ListViewItem lvItem = new ListViewItem(string.Join(".", dev.Address));
                        lvItem.SubItems.Add(dev.PortName);
                        lvItem.SubItems.Add(dev.Type.ToString());
                        lvItem.SubItems.Add(dev.Comment);
                        lvItem.ImageIndex = (int)dev.Type;
                        lvDevices.Items.Add(lvItem);
                        res = true;
                    }
                }));
            }

            return res;
        }

        private void SendPing(System.IO.Ports.SerialPort sp, byte[] diBusAddress)
        {
            try
            {
                // request settings
                DiBusMessage msg = new DiBusMessage();
                msg.ReceiverAddress = diBusAddress; // new byte[] { 0x29, 0x02, 0xff };
                msg.SourceAddress = new byte[] { 0x01, 0x01, 0x01 };
                msg.PackageType = PackageType.Ping;
                msg.DataType = DataType.UserType;


                byte[] request = msg.GetMessageByteArray();
                sp.Write(request, 0, request.Length);

            }
            catch (Exception ex) {  }
        }

        //private void SetMBS3RadioChannel(byte radioChannel)
        //{
            
        //}

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // receive responce
            int byteRecieved = serialPort1.BytesToRead;
            if (byteRecieved >= 14)
            {
                try
                {
                    byte[] respBuffer = new byte[byteRecieved];
                    serialPort1.Read(respBuffer, 0, byteRecieved);


                    // if responce correct - return serialport name
                    DiBusMessage newMsg = DiBusMessage.Parse(respBuffer);
                    if (newMsg.PackageType == PackageType.CommandConfirm)
                    {
                        DiBusDevice device = new DiBusDevice();
                        device.Address = newMsg.SourceAddress;
                        device.PortName = this.serialPort1.PortName;
                        device.Type = DiBusDevice.GetDevTypeByDiBusAddress(device.Address);
                        device.RadioChannel = this.RadioChannel;
                        if (!ListHasDevice(this.devices, device))
                        {
                            //Invoke(new MethodInvoker(delegate {
                            //    textBox2.Text += string.Join(" ", respBuffer) + " RadioChannel=" + this.RadioChannel + Environment.NewLine;
                            //}));
                            this.devices.Add(device);
                            this.Invoke(new MethodInvoker(delegate
                            {
                                this.UpdateLVDevices();
                            }));
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void UpdateLVDevices()
        {
            this.lvDevices.Items.Clear();
            foreach (var dev in this.devices)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.SubItems.Add(string.Join(".", dev.Address));
                //lvItem.SubItems.Add(dev.PortName);
                lvItem.SubItems.Add(dev.Type.ToString());
                lvItem.SubItems.Add(dev.Comment);
                lvItem.SubItems.Add(dev.RadioChannel.ToString());
                lvItem.ImageIndex = (int)dev.Type;
                lvDevices.Items.Add(lvItem);
            }
        }

        private bool ListHasDevice(List<DiBusDevice> devList, DiBusDevice dev)
        {
            foreach (var item in devList)
            {
                if (string.Join(".",item.Address) == string.Join(".", dev.Address))
                    return true;
            }

            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {


                flagStopThreads = true;

                StopReadData();

                SaveSettingsToINI();
                SaveSerialPortParamsToINI();

                this.scanner.RestoreMBS2RadioChannel();

                if (serialPort1 != null && serialPort1.IsOpen)
                    serialPort1.Close();

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { }
        }

        //private void btnTest_Click(object sender, EventArgs e)
        //{
        //    byte radiochannelToScan = byte.Parse(tbxRadioChannel.Text);
        //    this.TestRadioChannel(radiochannelToScan); 
        //}

        //private void TestRadioChannel(byte rchannel)
        //{
        //    var serials = System.IO.Ports.SerialPort.GetPortNames();

        //    if (this.serialPort1.IsOpen)
        //        this.serialPort1.Close();
            
        //    serialPort1.BaudRate = 115200;
        //    serialPort1.WriteTimeout = 50;
        //    serialPort1.ReadTimeout = 200;
        //    serialPort1.PortName = "COM3";

        //    try
        //    {
        //        serialPort1.Open();

        //        if (!WriteRadioChannelNum(new SerialPortOperator(serialPort1), rchannel))
        //        {
        //            MessageBox.Show("Error WriteRadioChannelNum("+rchannel+")");
        //            return;
        //        }

        //        //this.SendPing(this.serialPort1, new byte[] { 0x29, 0x02, 0xff });
        //        //System.Threading.Thread.Sleep(100);
        //        this.SendPing(this.serialPort1, new byte[] { 0xff, 0xff, 0xff });
        //        System.Threading.Thread.Sleep(100);
        //    }
        //    catch(Exception ex) { MessageBox.Show(ex.Message); }
        //}

        private void btnStop_Click(object sender, EventArgs e)
        {
            flagStopThreads = true;
            //btnStop.Visible = false;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        //private void btnWrite_Click(object sender, EventArgs e)
        //{
        //    byte radioChannel = 0;
        //    if (byte.TryParse(this.tbxRadioChannelNumber.Text, out radioChannel))
        //        WriteRadioChannelNum(serialPortOperator, radioChannel);
        //    else
        //        this.tbxRadioChannelNumber.Text = radioChannel.ToString();
        //}

        private void lvDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopReadData();

            this.listBox1.Items.Clear();

            if (lvDevices.SelectedItems.Count > 0)
            {
                try
                {
                    this.currentDevice = devices[lvDevices.SelectedItems[0].Index];
                    this.tbxDeviceNewRadioChannel.Text = currentDevice.RadioChannel.ToString();
                    this.DeviceOldRadioChannel = (byte)currentDevice.RadioChannel;

                    //this.btnSetDeviceRadioChannel.Enabled = true;
                    //this.tbxDeviceNewRadioChannel.Enabled = true;

                    DisplayDevice(currentDevice);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                this.currentDevice = null;
                this.btnSetDeviceRadioChannel.Enabled = false;
                this.tbxDeviceNewRadioChannel.Enabled = false;
                this.DisplayNoDevice();
            }
        }

        private void DisplayDevice(DiBusDevice dev)
        {
            try
            {
                //this.currentDevice = devices[lvDevices.SelectedItems[0].Index];
                this.lblDevice.Text = dev.Type.ToString();
                this.lblDevDiBusAddress.Text = string.Join(".", dev.Address);
                this.lblDevRadioChannel.Text = dev.RadioChannel.ToString();
                this.lblDevDevInfo.Text = dev.Comment;
                this.pbxDevIcon.Image = imageListLarge.Images[(int)dev.Type];
                if(dev.Type == DeviceType.MBS2 ||
                    dev.Type == DeviceType.MBS3 ||
                    dev.Type == DeviceType.Pult)
                    this.btnSetDeviceRadioChannel.Enabled = true;
                else
                    this.btnSetDeviceRadioChannel.Enabled = false;
                if (dev.Type == DeviceType.BDBeta ||
                    dev.Type == DeviceType.BDGamma ||
                    dev.Type == DeviceType.BDAlpha ||
                    dev.Type == DeviceType.BDNeutron ||
                    dev.Type == DeviceType.Pult)
                    this.btnGetDataFromDevice.Enabled = true;
                else
                    this.btnGetDataFromDevice.Enabled = false;
                this.tbxDeviceNewRadioChannel.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DisplayNoDevice()
        {
            try
            {
                this.lblDevice.Text = "Device";
                this.lblDevDiBusAddress.Text = "__.__.__";
                this.lblDevRadioChannel.Text = "__";
                this.lblDevDevInfo.Text = string.Empty;
                this.pbxDevIcon.Image = null;
                this.btnSetDeviceRadioChannel.Enabled = false;
                this.btnGetDataFromDevice.Enabled = false;
                this.tbxDeviceNewRadioChannel.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSetDeviceRadioChannel_Click(object sender, EventArgs e)
        {
            if (this.currentDevice == null)
                return;

            byte deviceNewRadioChannel = this.DeviceOldRadioChannel;

            if(byte.TryParse(this.tbxDeviceNewRadioChannel.Text, out deviceNewRadioChannel))
            {
                //this.scanner.SetRadioChannel(currentDevice, deviceNewRadioChannel); 
                //return; 

                DiBusMessage msg = new DiBusMessage();
                msg.ReceiverAddress = currentDevice.Address;
                msg.SourceAddress = new byte[] { 1, 1, 1 };
                msg.PackageType = PackageType.MasterSendData;

                switch (currentDevice.Type)
                {
                    case DeviceType.MBS2:
                        if (WriteRadioChannelNum(serialPortOperator, deviceNewRadioChannel))
                        {
                            this.currentDevice.RadioChannel = deviceNewRadioChannel;
                            this.scanner.Mbs2RadioChannel = deviceNewRadioChannel;
                            this.UpdateLVDevices();
                        }
                        break;
                    case DeviceType.BDGamma:
                        break;
                    case DeviceType.Pult:
                        WriteRadioChannelNum(serialPortOperator, this.DeviceOldRadioChannel);
                        // меняем адрес текущего устройства на новый
                        msg.DataType = DataType.UserType;
                        msg.DataArray = new byte[] { 32, 2, 1, 1, deviceNewRadioChannel, 0 };

                        if (serialPortOperator.SendDiBusMessageWitnConfirm(msg.GetMessageByteArray()))
                        {
                            currentDevice.RadioChannel = deviceNewRadioChannel;
                            this.UpdateLVDevices();
                        }
                        break;
                    case DeviceType.MBS3:
                        // перестраиваем МБС2 на радиоканал текущего устройства
                        WriteRadioChannelNum(serialPortOperator, this.DeviceOldRadioChannel);
                        // меняем адрес текущего устройства на новый
                        msg.DataType = DataType.Byte;
                        msg.DataArray = new byte[] { 0xFE, deviceNewRadioChannel };
                        
                        if (serialPortOperator.SendDiBusMessageWitnConfirm(msg.GetMessageByteArray()))
                        {
                            // добавить сохранение параметров
                            DiBusMessage msgAddress = new DiBusMessage();
                            msgAddress.ReceiverAddress = currentDevice.Address;
                            msgAddress.SourceAddress = new byte[] { 1, 1, 1 };
                            msgAddress.PackageType = PackageType.MasterSendData;
                            msgAddress.DataType = DataType.DiBusAddress;
                            msgAddress.DataArray = new byte[] { 0xFD, currentDevice.Address[2], currentDevice.Address[1], currentDevice.Address[0] };
                            //msgAddress.DataArray = new byte[] { 0xFD, currentDevice.Address[0], currentDevice.Address[1], currentDevice.Address[2] };
                            if (serialPortOperator.SendDiBusMessageWitnConfirm(msgAddress.GetMessageByteArray()))
                            {
                                currentDevice.RadioChannel = deviceNewRadioChannel;
                                this.UpdateLVDevices();
                            }
                        }

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

        private void tbxScanFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            byte tmp = 0;
            if (!char.IsControl(e.KeyChar) 
                && !char.IsDigit(e.KeyChar) 
                && (e.KeyChar != '.') 
                //&& !byte.TryParse(tbxScanFrom.Text, out tmp)
                )
            {
                e.Handled = true;
            }
            else e.Handled = false;

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialPortSetingsForm spSettingsForm = new SerialPortSetingsForm(this.serialPort1);
            if(spSettingsForm.ShowDialog() == DialogResult.OK)
            {
                DisplaySerialPortInfo();
            }
        }

        private void btnGetDataFromDevice_Click(object sender, EventArgs e)
        {
           
            if (dbMaster == null)
                dbMaster = new DBMaster(serialPort1);
            
            dbMaster.DBAddress = this.currentDevice.Address;

            if (currentDevice.Type == DeviceType.BDGamma)
                dbMaster.DBDataRegister = (byte)Registers.DBGammaDataRegister;
            if (currentDevice.Type == DeviceType.BDBeta)
                dbMaster.DBDataRegister = (byte)Registers.DBBetaDataRegister;
            if (currentDevice.Type == DeviceType.BDAlpha)
                dbMaster.DBDataRegister = (byte)Registers.DBAlphaDataRegister;
            if (currentDevice.Type == DeviceType.Pult)
                dbMaster.DBDataRegister = (byte)Registers.PultGPSDataRegister;
            if (currentDevice.Type == DeviceType.BDNeutron)
                dbMaster.DBDataRegister = (byte)Registers.DBNeutronDataRegister;

            dbViewer.InitialDescriptions(dbMaster.DBDataRegister);
            
            WriteRadioChannelNum(serialPortOperator, (byte)this.currentDevice.RadioChannel);

            dbMaster.DisplayData = (s) => {
                if (s == null) return;
                if (dbMaster.DBDataRegister == (byte)Registers.PultGPSDataRegister)
                    s = DiBusDevice.ConvertToGpsData(s);
                this.Invoke(new MethodInvoker(delegate 
                {
                    try { this.dbViewer.DisplayDataOnListBox(this.listBox1, s); }
                    catch { }
                }));
            };
            dbMaster.ProcessAction = (s) => {
                //try { this.Invoke(new MethodInvoker(delegate { this.toolStripStatusLabelInfo.Text = s; })); }
                //catch { }
            };
            
            if(!dbMaster.IsStarted)
            {
                dbMaster.StartAsync();
                this.btnGetDataFromDevice.Visible = false;
                this.btnStopGetData.Visible = true;
            }
        
        }

        private void btnStopGetData_Click(object sender, EventArgs e)
        {
            StopReadData();
        }

        private void StopReadData()
        {
            if (dbMaster != null && dbMaster.IsStarted)
                dbMaster.Stop();
            this.btnStopGetData.Visible = false;
            this.btnGetDataFromDevice.Visible = true;
        }

        private void tsmItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region Translation
        private void tsmItemRus_Click(object sender, EventArgs e)
        {
            TranslateUI(EnabledLanguages.Russian);
        }

        private void tsmItemEng_Click(object sender, EventArgs e)
        {
            TranslateUI(EnabledLanguages.English);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!LoadLanguageFromCommandLine())
                TranslateUI(Translation.GetLanguageByLocalization());
        }


        private void TranslateUI(EnabledLanguages language)
        {

            translation.SetLanguage(language);

            if (language == EnabledLanguages.Russian)
            {
                tsmItemRus.Checked = true;
                tsmItemEng.Checked = false;
            }
            else
            {
                tsmItemRus.Checked = false;
                tsmItemEng.Checked = true;
            }

            //foreach (var control in this.Controls)
            //{
            //    if (translation.Table.ContainsKey((control as UserControl).T))
            //        toolStripMenuItemEdit.Text = translation.Table["toolStripMenuItemEdit"];
            //}

            if (translation.Table.ContainsKey("toolStripMenuItemEdit"))
                toolStripMenuItemEdit.Text = translation.Table["toolStripMenuItemEdit"];

            if (translation.Table.ContainsKey("tsmItemSerialPortSettings"))
                tsmItemSerialPortSettings.Text = translation.Table["tsmItemSerialPortSettings"];

            if (translation.Table.ContainsKey("tsmItemLanguage"))
                tsmItemLanguage.Text = translation.Table["tsmItemLanguage"];

            if (translation.Table.ContainsKey("tsmItemExit"))
                tsmItemExit.Text = translation.Table["tsmItemExit"];

            if (translation.Table.ContainsKey("lblRadioChannelsFrom"))
                lblRadioChannelsFrom.Text = translation.Table["lblRadioChannelsFrom"];

            if (translation.Table.ContainsKey("lblLabelDiBusAddress"))
                lblLabelDiBusAddress.Text = translation.Table["lblLabelDiBusAddress"];

            if (translation.Table.ContainsKey("lblLabelRadioChannel"))
                lblLabelRadioChannel.Text = translation.Table["lblLabelRadioChannel"];

            if (translation.Table.ContainsKey("lblLabelDevInfo"))
                lblLabelDevInfo.Text = translation.Table["lblLabelDevInfo"];

            if (translation.Table.ContainsKey("btnSetDeviceRadioChannel"))
                btnSetDeviceRadioChannel.Text = translation.Table["btnSetDeviceRadioChannel"];

            if (translation.Table.ContainsKey("btnStopGetData"))
                btnStopGetData.Text = translation.Table["btnStopGetData"];

            if (translation.Table.ContainsKey("btnGetDataFromDevice"))
                btnGetDataFromDevice.Text = translation.Table["btnGetDataFromDevice"];

            if (translation.Table.ContainsKey("columnHeaderDiBusAddress"))
                columnHeaderDiBusAddress.Text = translation.Table["columnHeaderDiBusAddress"];

            if (translation.Table.ContainsKey("columnHeaderComment"))
                columnHeaderComment.Text = translation.Table["columnHeaderComment"];

            if (translation.Table.ContainsKey("columnHeaderRadioChannel"))
                columnHeaderRadioChannel.Text = translation.Table["columnHeaderRadioChannel"];

            if (translation.Table.ContainsKey("columnHeaderType"))
                columnHeaderType.Text = translation.Table["columnHeaderType"];

            if (translation.Table.ContainsKey("lblDevice"))
                lblDevice.Text = translation.Table["lblDevice"];

            //DisplaySerialPortInfo();
            //if (translation.Table.ContainsKey(""))
            //    .Text = translation.Table[""];

        }

        #endregion Translation
    }


}