using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiBusScanner
{
    public partial class SerialPortSetingsForm : Form
    {
        SerialPort currentSerialPort = null;
        public SerialPortSetingsForm()
        {
            InitializeComponent();
            this.InitialSerianPortSettingsControls();
            this.SetDefaultSettings();
        }

        public SerialPortSetingsForm(SerialPort sp)
        {
            InitializeComponent();
            this.currentSerialPort = sp;
            this.InitialSerianPortSettingsControls();
            this.DisplaySerialPortSettings(this.currentSerialPort);
        }

        private void InitialSerianPortSettingsControls()
        {
            try
            {
                var serialPorts = SerialPort.GetPortNames();
                cbxPort.DataSource = serialPorts;
                List<int> baudRates = new List<int>() { 9600, 19200, 115200 };
                cbxRate.DataSource = baudRates;
                List<int> dataBits = new List<int>() { 5, 6, 7, 8 };
                cbxDataBits.DataSource = dataBits;
                cbxParity.DataSource = Enum.GetValues(typeof(Parity));
                cbxStopBits.DataSource = Enum.GetValues(typeof(StopBits));
                cbxFlowControl.DataSource = Enum.GetValues(typeof(Handshake));
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void SetDefaultSettings()
        {
            try
            {
                cbxParity.SelectedIndex = 0;
                cbxStopBits.SelectedIndex = 1;
                cbxFlowControl.SelectedIndex = 0;
                cbxDataBits.SelectedIndex = 3;
                cbxRate.SelectedIndex = 2;
                cbxPort.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void DisplaySerialPortSettings(SerialPort sp)
        {
            try
            {
                cbxParity.SelectedItem = sp.Parity;
                cbxDataBits.SelectedItem = sp.DataBits;
                cbxStopBits.SelectedItem = sp.StopBits;
                cbxFlowControl.SelectedItem = sp.Handshake;
                cbxRate.SelectedItem = sp.BaudRate;
                cbxPort.SelectedItem = sp.PortName;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ApplySerialPortSettings(SerialPort serial)
        {
            try
            {
                serial.PortName = cbxPort.Text;
                serial.BaudRate = int.Parse(cbxRate.Text);
                serial.DataBits = int.Parse(cbxDataBits.Text);
                serial.Parity = (Parity)cbxParity.SelectedItem;
                serial.StopBits = (StopBits)cbxStopBits.SelectedItem;
                serial.Handshake = (Handshake)cbxFlowControl.SelectedItem;
                // Additionally
                serial.WriteTimeout = 100;
                serial.ReadTimeout = 500;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (currentSerialPort != null)
            {
                if (currentSerialPort.IsOpen)
                    currentSerialPort.Close();
                ApplySerialPortSettings(this.currentSerialPort);
            }
        }
    }
}
