using SerialUpgrader;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialUpgrader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxSerialPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                comboBoxSerialPort.Items.AddRange(ports);
                comboBoxSerialPort.SelectedIndex = 0;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSlectFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new() // 创建打开文件对话框
            {
                Filter = "firmware|*.xbin"
            };
            if (ofd.ShowDialog() == DialogResult.OK) // 如果点击了确定按钮
            {
                textBoxFirmware.Text = ofd.FileName;
            }
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private async void buttonUpgrade_Click(object sender, EventArgs e)
        {
            string firmwarePath = textBoxFirmware.Text;
            if (string.IsNullOrEmpty(firmwarePath) || !File.Exists(firmwarePath))
            {
                MessageBox.Show("请选择有效的固件文件");
                return;
            }

            string? port = comboBoxSerialPort.SelectedItem as string;
            if (string.IsNullOrEmpty(port))
            {
                MessageBox.Show("请选择串口");
                return;
            }

            textBoxLog.Clear();

            FileInfo xbinFile = new(firmwarePath);
            byte[] xbin = File.ReadAllBytes(firmwarePath);

            if (xbin.Length != xbinFile.Length)
            {
                textBoxLog.AppendText($"文件大小与文件信息大小不符{xbin.Length} != {xbinFile.Length}\r\n");
                return;
            }

            textBoxLog.AppendText($"固件文件: {xbinFile.Name} {xbinFile.Length / 1024.0:F2} KB\r\n");

            MagicHeader? header = MagicHeader.Parse(xbin);
            if (header == null)
            {
                textBoxLog.AppendText("无效的固件文件\r\n");
                return;
            }

            // 提取明文固件数据部分
            byte[] databin = xbin[(int)header.DataOffset..];
            if (databin.Length != header.DataLength)
            {
                textBoxLog.AppendText($"固件数据长度与头部信息不匹配: {databin.Length} != {header.DataLength}\r\n");
                return;
            }

            // 对明文固件进行 AES-128 加密
            byte[] encryptedDatabin = FirmwareSecurity.EncryptFirmware(databin);

            textBoxLog.AppendText($"头部地址: 0x{header.ThisAddress:X8}\r\n");
            textBoxLog.AppendText($"固件版本: {header.Version}\r\n");
            textBoxLog.AppendText($"固件类型: {header.DataType}\r\n");
            textBoxLog.AppendText($"明文大小