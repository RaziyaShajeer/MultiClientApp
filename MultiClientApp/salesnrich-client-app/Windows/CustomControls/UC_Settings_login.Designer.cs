using SNR_ClientApp.Windows.CustomControls.AutomationControls;

namespace SNR_ClientApp.Windows.CustomControls
{
    partial class UC_Settings_login
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel4 = new Panel();
            btn_back = new Button();
            btn_login = new Button();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            infoHostName = new PictureBox();
            panel5 = new Panel();
            panel3 = new Panel();
            txt_password = new TextBox();
            label3 = new Label();
            txt_username = new TextBox();
            label2 = new Label();
            panel2 = new Panel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)infoHostName).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
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
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(914, 551);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(33, 141, 170);
            panel4.BorderStyle = BorderStyle.Fixed3D;
            panel4.Controls.Add(btn_back);
            panel4.Controls.Add(btn_login);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(pictureBox2);
            panel4.Controls.Add(infoHostName);
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(panel3);
            panel4.Controls.Add(txt_password);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(txt_username);
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(443, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Size = new Size(471, 551);
            panel4.TabIndex = 8;
            // 
            // btn_back
            // 
            btn_back.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_back.BackColor = Color.SpringGreen;
            btn_back.Cursor = Cursors.Hand;
            btn_back.FlatAppearance.BorderColor = Color.SpringGreen;
            btn_back.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, 255, 170);
            btn_back.FlatStyle = FlatStyle.Flat;
            btn_back.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_back.Location = new Point(91, 463);
            btn_back.Margin = new Padding(3, 4, 3, 4);
            btn_back.Name = "btn_back";
            btn_back.Size = new Size(109, 49);
            btn_back.TabIndex = 16;
            btn_back.Text = "Back";
            btn_back.UseVisualStyleBackColor = false;
            btn_back.Click += btn_back_Click;
            // 
            // btn_login
            // 
            btn_login.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_login.BackColor = Color.SpringGreen;
            btn_login.Cursor = Cursors.Hand;
            btn_login.FlatAppearance.BorderColor = Color.SpringGreen;
            btn_login.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, 255, 170);
            btn_login.FlatStyle = FlatStyle.Flat;
            btn_login.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_login.Location = new Point(288, 463);
            btn_login.Margin = new Padding(3, 4, 3, 4);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(109, 49);
            btn_login.TabIndex = 9;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Nirmala UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(155, 61);
            label1.Name = "label1";
            label1.Size = new Size(157, 37);
            label1.TabIndex = 17;
            label1.Text = "Verify User";
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(302, 252);
            pictureBox2.Margin = new Padding(3, 4, 3, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(17, 20);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            toolTip1.SetToolTip(pictureBox2, "Please specify the port number where Tally is running.");
            // 
            // infoHostName
            // 
            infoHostName.Location = new Point(302, 148);
            infoHostName.Margin = new Padding(3, 4, 3, 4);
            infoHostName.Name = "infoHostName";
            infoHostName.Size = new Size(17, 20);
            infoHostName.SizeMode = PictureBoxSizeMode.Zoom;
            infoHostName.TabIndex = 14;
            infoHostName.TabStop = false;
            toolTip1.SetToolTip(infoHostName, "Please specify the IP address/Network address of ");
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.None;
            panel5.BackColor = Color.DeepSkyBlue;
            panel5.Location = new Point(90, 316);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(305, 4);
            panel5.TabIndex = 13;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.None;
            panel3.BackColor = Color.DeepSkyBlue;
            panel3.Location = new Point(90, 224);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(305, 4);
            panel3.TabIndex = 12;
            // 
            // txt_password
            // 
            txt_password.Anchor = AnchorStyles.None;
            txt_password.BackColor = Color.FromArgb(6, 96, 137);
            txt_password.BorderStyle = BorderStyle.None;
            txt_password.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txt_password.ForeColor = Color.White;
            txt_password.Location = new Point(90, 284);
            txt_password.Margin = new Padding(3, 4, 3, 4);
            txt_password.Multiline = true;
            txt_password.Name = "txt_password";
            txt_password.PasswordChar = '*';
            txt_password.PlaceholderText = "Enter Password";
            txt_password.Size = new Size(305, 31);
            txt_password.TabIndex = 8;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(189, 248);
            label3.Name = "label3";
            label3.Size = new Size(88, 22);
            label3.TabIndex = 10;
            label3.Text = "Password";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txt_username
            // 
            txt_username.Anchor = AnchorStyles.None;
            txt_username.BackColor = Color.FromArgb(6, 96, 137);
            txt_username.BorderStyle = BorderStyle.None;
            txt_username.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txt_username.ForeColor = Color.White;
            txt_username.Location = new Point(90, 189);
            txt_username.Margin = new Padding(0);
            txt_username.Multiline = true;
            txt_username.Name = "txt_username";
            txt_username.PlaceholderText = "Enter Username";
            txt_username.Size = new Size(305, 31);
            txt_username.TabIndex = 7;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(186, 144);
            label2.Name = "label2";
            label2.Size = new Size(88, 22);
            label2.TabIndex = 9;
            label2.Text = "Username";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(pictureBox3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 4);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(437, 543);
            panel2.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 197);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(437, 346);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(119, 32);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(179, 172);
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
            pictureBox3.Margin = new Padding(11, 13, 11, 4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Padding = new Padding(11, 13, 11, 13);
            pictureBox3.Size = new Size(437, 197);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 1;
            pictureBox3.TabStop = false;
            // 
            // UC_Settings_login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "UC_Settings_login";
            Size = new Size(914, 551);
            tableLayoutPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)infoHostName).EndInit();
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel4;
        private Button btn_login;
        private TextBox txt_password;
        private Label label3;
        private TextBox txt_username;
        private Label label2;
        private Panel panel2;
        private PictureBox pictureBox3;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Panel panel5;
        private ErrorProvider errorProvider1;
        private PictureBox infoHostName;
        private ToolTip toolTip1;
        private PictureBox pictureBox2;
        private Button btn_back;
        private Label label1;
    }
}
