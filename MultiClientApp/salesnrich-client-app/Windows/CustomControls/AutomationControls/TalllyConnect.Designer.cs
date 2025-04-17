namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    partial class TalllyConnect
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TalllyConnect));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel4 = new Panel();
            infoHostName = new PictureBox();
            pictureBox5 = new PictureBox();
            txt_odbc_dsn = new TextBox();
            label1 = new Label();
            btn_back = new Button();
            pictureBox4 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel5 = new Panel();
            panel3 = new Panel();
            companySelect = new ComboBox();
            button2 = new Button();
            button1 = new Button();
            label4 = new Label();
            txtPort = new TextBox();
            label3 = new Label();
            txtHost = new TextBox();
            panel2 = new Panel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            errorProvider1 = new ErrorProvider(components);
            toolTip1 = new ToolTip(components);
            label2 = new Label();
            panel6 = new Panel();
            tableLayoutPanel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)infoHostName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.5F));
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(800, 413);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(33, 141, 170);
            panel4.BorderStyle = BorderStyle.Fixed3D;
            panel4.Controls.Add(infoHostName);
            panel4.Controls.Add(pictureBox5);
            panel4.Controls.Add(txt_odbc_dsn);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(btn_back);
            panel4.Controls.Add(pictureBox4);
            panel4.Controls.Add(pictureBox2);
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(panel3);
            panel4.Controls.Add(companySelect);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button1);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(txtPort);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(txtHost);
            panel4.Controls.Add(panel6);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(388, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Size = new Size(412, 413);
            panel4.TabIndex = 8;
            panel4.Paint += panel4_Paint;
            // 
            // infoHostName
            // 
            infoHostName.Image = (Image)resources.GetObject("infoHostName.Image");
            infoHostName.Location = new Point(344, 67);
            infoHostName.Name = "infoHostName";
            infoHostName.Size = new Size(15, 15);
            infoHostName.SizeMode = PictureBoxSizeMode.Zoom;
            infoHostName.TabIndex = 16;
            infoHostName.TabStop = false;
            toolTip1.SetToolTip(infoHostName, "Please specify the IP address/Network address of ");
            infoHostName.Visible = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(342, 191);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(15, 15);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 21;
            pictureBox5.TabStop = false;
            toolTip1.SetToolTip(pictureBox5, "Please specify the ODBC Data Source Name of Tally");
            pictureBox5.Visible = false;
            // 
            // txt_odbc_dsn
            // 
            txt_odbc_dsn.Anchor = AnchorStyles.None;
            txt_odbc_dsn.BackColor = Color.FromArgb(6, 96, 137);
            txt_odbc_dsn.BorderStyle = BorderStyle.None;
            txt_odbc_dsn.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txt_odbc_dsn.ForeColor = Color.White;
            txt_odbc_dsn.Location = new Point(70, 188);
            txt_odbc_dsn.Multiline = true;
            txt_odbc_dsn.Name = "txt_odbc_dsn";
            txt_odbc_dsn.Size = new Size(267, 23);
            txt_odbc_dsn.TabIndex = 19;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(162, 165);
            label1.Name = "label1";
            label1.Size = new Size(89, 19);
            label1.TabIndex = 20;
            label1.Text = "ODBC DSN";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_back
            // 
            btn_back.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_back.BackColor = Color.SpringGreen;
            btn_back.FlatAppearance.BorderColor = Color.SpringGreen;
            btn_back.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, 255, 170);
            btn_back.FlatStyle = FlatStyle.Flat;
            btn_back.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_back.Location = new Point(68, 350);
            btn_back.Name = "btn_back";
            btn_back.Size = new Size(95, 37);
            btn_back.TabIndex = 17;
            btn_back.Text = "Back";
            btn_back.UseVisualStyleBackColor = false;
            btn_back.Visible = false;
            btn_back.Click += btn_back_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(347, 296);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(15, 15);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            toolTip1.SetToolTip(pictureBox4, "Select the company name to be integrated with salesNrich.");
            pictureBox4.Visible = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(344, 131);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(15, 15);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            toolTip1.SetToolTip(pictureBox2, "Please specify the port number where Tally is running.");
            pictureBox2.Visible = false;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.None;
            panel5.BackColor = Color.DeepSkyBlue;
            panel5.Location = new Point(74, 149);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(267, 3);
            panel5.TabIndex = 13;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.None;
            panel3.BackColor = Color.DeepSkyBlue;
            panel3.Location = new Point(73, 84);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(267, 3);
            panel3.TabIndex = 12;
            // 
            // companySelect
            // 
            companySelect.Anchor = AnchorStyles.None;
            companySelect.BackColor = Color.FromArgb(6, 96, 137);
            companySelect.DropDownStyle = ComboBoxStyle.DropDownList;
            companySelect.Enabled = false;
            companySelect.FlatStyle = FlatStyle.Flat;
            companySelect.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            companySelect.ForeColor = Color.White;
            companySelect.FormattingEnabled = true;
            companySelect.ImeMode = ImeMode.Hiragana;
            companySelect.Location = new Point(74, 292);
            companySelect.Name = "companySelect";
            companySelect.Size = new Size(267, 28);
            companySelect.TabIndex = 10;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.BackColor = Color.SpringGreen;
            button2.Enabled = false;
            button2.FlatAppearance.BorderColor = Color.SpringGreen;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, 255, 170);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(264, 350);
            button2.Name = "button2";
            button2.Size = new Size(95, 37);
            button2.TabIndex = 9;
            button2.Text = "Next";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click_1;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.BackColor = Color.Green;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 64, 0);
            button1.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Nirmala UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.Location = new Point(163, 223);
            button1.Name = "button1";
            button1.Size = new Size(84, 28);
            button1.TabIndex = 9;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = SystemColors.ButtonFace;
            label4.Location = new Point(166, 266);
            label4.Name = "label4";
            label4.Size = new Size(68, 19);
            label4.TabIndex = 11;
            label4.Text = "Company";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtPort
            // 
            txtPort.Anchor = AnchorStyles.None;
            txtPort.BackColor = Color.FromArgb(6, 96, 137);
            txtPort.BorderStyle = BorderStyle.None;
            txtPort.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtPort.ForeColor = Color.White;
            txtPort.Location = new Point(74, 126);
            txtPort.Multiline = true;
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(267, 23);
            txtPort.TabIndex = 8;
            txtPort.Text = "9000";
            txtPort.Validating += txtPort_Validating;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(183, 98);
            label3.Name = "label3";
            label3.Size = new Size(35, 19);
            label3.TabIndex = 10;
            label3.Text = "Port";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtHost
            // 
            txtHost.Anchor = AnchorStyles.None;
            txtHost.BackColor = Color.FromArgb(6, 96, 137);
            txtHost.BorderStyle = BorderStyle.None;
            txtHost.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtHost.ForeColor = Color.White;
            txtHost.Location = new Point(74, 63);
            txtHost.Margin = new Padding(0);
            txtHost.Multiline = true;
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(267, 23);
            txtHost.TabIndex = 7;
            txtHost.Text = "localhost";
            txtHost.Validating += txtHost_Validating;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(pictureBox3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(382, 407);
            panel2.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 148);
            panel1.Name = "panel1";
            panel1.Size = new Size(382, 259);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(104, 24);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(157, 129);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.White;
            pictureBox3.Dock = DockStyle.Top;
            pictureBox3.Image = Properties.Resources.salesnrich;
            pictureBox3.Location = new Point(0, 0);
            pictureBox3.Margin = new Padding(10, 10, 10, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Padding = new Padding(10);
            pictureBox3.Size = new Size(382, 148);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 1;
            pictureBox3.TabStop = false;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(58, 5);
            label2.Name = "label2";
            label2.Size = new Size(70, 19);
            label2.TabIndex = 15;
            label2.Text = "Hostname";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            panel6.Controls.Add(label2);
            panel6.Location = new Point(108, 35);
            panel6.Name = "panel6";
            panel6.Size = new Size(200, 29);
            panel6.TabIndex = 18;
            // 
            // TalllyConnect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "TalllyConnect";
            Size = new Size(800, 413);
            tableLayoutPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)infoHostName).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel4;
        private ComboBox companySelect;
        private Button button2;
        private Button button1;
        private Label label4;
        private TextBox txtPort;
        private Label label3;
        private TextBox txtHost;
        private Panel panel2;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Panel panel5;
        private ErrorProvider errorProvider1;
        private ToolTip toolTip1;
        private PictureBox pictureBox2;
        private Button btn_back;
        private Panel panel6;
        private PictureBox infoHostName;
        private Label label2;
        private Panel panel7;
        private Panel panel8;
        private PictureBox pictureBox5;
        private TextBox txt_odbc_dsn;
        private Label label1;
    }
}
