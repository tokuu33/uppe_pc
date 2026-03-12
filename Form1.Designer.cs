namespace SerialUpgrader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxFirmware = new TextBox();
            buttonSelectFirmware = new Button();
            label2 = new Label();
            comboBoxSerialPort = new ComboBox();
            textBoxLog = new TextBox();
            progressBar = new ProgressBar();
            buttonUpgrade = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(27, 20);
            label1.Name = "label1";
            label1.Size = new Size(96, 28);
            label1.TabIndex = 0;
            label1.Text = "固件文件";
            // 
            // textBoxFirmware
            // 
            textBoxFirmware.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFirmware.BackColor = SystemColors.Window;
            textBoxFirmware.Location = new Point(122, 16);
            textBoxFirmware.Name = "textBoxFirmware";
            textBoxFirmware.ReadOnly = true;
            textBoxFirmware.Size = new Size(624, 30);
            textBoxFirmware.TabIndex = 1;
            textBoxFirmware.TextChanged += textBox1_TextChanged;
            // 
            // buttonSelectFirmware
            // 
            buttonSelectFirmware.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSelectFirmware.Location = new Point(752, 16);
            buttonSelectFirmware.Name = "buttonSelectFirmware";
            buttonSelectFirmware.Size = new Size(46, 30);
            buttonSelectFirmware.TabIndex = 2;
            buttonSelectFirmware.Text = "...";
            buttonSelectFirmware.UseVisualStyleBackColor = true;
            buttonSelectFirmware.Click += buttonSlectFirmware_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(63, 56);
            label2.Name = "label2";
            label2.Size = new Size(54, 28);
            label2.TabIndex = 0;
            label2.Text = "端口";
            label2.Click += label2_Click;
            // 
            // comboBoxSerialPort
            // 
            comboBoxSerialPort.FormattingEnabled = true;
            comboBoxSerialPort.Location = new Point(122, 53);
            comboBoxSerialPort.Name = "comboBoxSerialPort";
            comboBoxSerialPort.Size = new Size(132, 32);
            comboBoxSerialPort.TabIndex = 3;
            // 
            // textBoxLog
            // 
            textBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxLog.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBoxLog.Location = new Point(27, 91);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.Size = new Size(771, 406);
            textBoxLog.TabIndex = 1;
            textBoxLog.TextChanged += textBox1_TextChanged;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(27, 503);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(771, 46);
            progressBar.TabIndex = 4;
            progressBar.Click += progressBar_Click;
            // 
            // buttonUpgrade
            // 
            buttonUpgrade.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonUpgrade.Font = new Font("微软雅黑", 18F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonUpgrade.Location = new Point(27, 555);
            buttonUpgrade.Name = "buttonUpgrade";
            buttonUpgrade.Size = new Size(771, 82);
            buttonUpgrade.TabIndex = 2;
            buttonUpgrade.Text = "升 级";
            buttonUpgrade.UseVisualStyleBackColor = true;
            buttonUpgrade.Click += buttonUpgrade_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(822, 644);
            Controls.Add(progressBar);
            Controls.Add(comboBoxSerialPort);
            Controls.Add(buttonUpgrade);
            Controls.Add(buttonSelectFirmware);
            Controls.Add(textBoxLog);
            Controls.Add(textBoxFirmware);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "串行升级工具";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxFirmware;
        private Button buttonSelectFirmware;
        private Label label2;
        private ComboBox comboBoxSerialPort;
        private TextBox textBoxLog;
        private ProgressBar progressBar;
        private Button buttonUpgrade;
    }
}
