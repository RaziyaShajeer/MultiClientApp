
namespace SNR_ClientApp.Windows
{
    partial class LoginScreen
    {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginScreen));
			panel1 = new Panel();
			close_btn = new PictureBox();
			ErrorLabel = new Label();
			rememberMeChkBox = new CheckBox();
			SnrHeadding = new PictureBox();
			button1 = new Button();
			password = new TextBox();
			username = new TextBox();
			label1 = new Label();
			panel2 = new Panel();
			pictureBox1 = new PictureBox();
			pictureBox2 = new PictureBox();
			timer1 = new System.Windows.Forms.Timer(components);
			errorProvider1 = new ErrorProvider(components);
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)close_btn).BeginInit();
			((System.ComponentModel.ISupportInitialize)SnrHeadding).BeginInit();
			panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.BackColor = Color.Transparent;
			panel1.Controls.Add(close_btn);
			panel1.Controls.Add(ErrorLabel);
			panel1.Controls.Add(rememberMeChkBox);
			panel1.Controls.Add(SnrHeadding);
			panel1.Controls.Add(button1);
			panel1.Controls.Add(password);
			panel1.Controls.Add(username);
			panel1.Controls.Add(label1);
			panel1.Dock = DockStyle.Bottom;
			panel1.Location = new Point(0, -2);
			panel1.Margin = new Padding(4, 1, 4, 3);
			panel1.Name = "panel1";
			panel1.Size = new Size(488, 343);
			panel1.TabIndex = 0;
			
			// 
			// close_btn
			// 
			close_btn.Cursor = Cursors.Hand;
			close_btn.Image = Properties.Resources.close_red;
			close_btn.Location = new Point(458, 5);
			close_btn.Name = "close_btn";
			close_btn.Size = new Size(15, 17);
			close_btn.SizeMode = PictureBoxSizeMode.StretchImage;
			close_btn.TabIndex = 6;
			close_btn.TabStop = false;
			close_btn.Visible = false;
			close_btn.Click += close_btn_Click;
			// 
			// ErrorLabel
			// 
			ErrorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			ErrorLabel.AutoSize = true;
			ErrorLabel.ForeColor = Color.FromArgb(192, 0, 0);
			ErrorLabel.Location = new Point(155, 60);
			ErrorLabel.Name = "ErrorLabel";
			ErrorLabel.RightToLeft = RightToLeft.No;
			ErrorLabel.Size = new Size(0, 15);
			ErrorLabel.TabIndex = 5;
			ErrorLabel.TextAlign = ContentAlignment.TopCenter;
			ErrorLabel.Visible = false;
			// 
			// rememberMeChkBox
			// 
			rememberMeChkBox.AutoSize = true;
			rememberMeChkBox.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			rememberMeChkBox.Location = new Point(129, 241);
			rememberMeChkBox.Name = "rememberMeChkBox";
			rememberMeChkBox.Size = new Size(114, 21);
			rememberMeChkBox.TabIndex = 4;
			rememberMeChkBox.Text = "Remember Me";
			rememberMeChkBox.UseVisualStyleBackColor = true;
			// 
			// SnrHeadding
			// 
			SnrHeadding.BackColor = Color.Transparent;
			SnrHeadding.Image = Properties.Resources.salesnrich;
			SnrHeadding.Location = new Point(99, 31);
			SnrHeadding.Name = "SnrHeadding";
			SnrHeadding.Size = new Size(292, 55);
			SnrHeadding.SizeMode = PictureBoxSizeMode.Zoom;
			SnrHeadding.TabIndex = 3;
			SnrHeadding.TabStop = false;
			SnrHeadding.Visible = false;
			// 
			// button1
			// 
			button1.BackColor = Color.FromArgb(6, 96, 137);
			button1.Cursor = Cursors.Hand;
			button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(6, 96, 137);
			button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(33, 141, 170);
			button1.FlatStyle = FlatStyle.Flat;
			button1.Font = new Font("Comic Sans MS", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
			button1.ForeColor = Color.White;
			button1.Location = new Point(129, 205);
			button1.Name = "button1";
			button1.Size = new Size(227, 30);
			button1.TabIndex = 0;
			button1.Text = "Login";
			button1.UseVisualStyleBackColor = false;
			button1.Click += button1_Click;
			// 
			// password
			// 
			password.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
			password.Location = new Point(129, 155);
			password.Name = "password";
			password.PasswordChar = '*';
			password.PlaceholderText = "Password";
			password.Size = new Size(227, 26);
			password.TabIndex = 2;
			password.Validating += password_Validating;
			// 
			// username
			// 
			username.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
			username.ForeColor = Color.Black;
			username.Location = new Point(129, 114);
			username.Name = "username";
			username.PlaceholderText = "Username";
			username.Size = new Size(227, 26);
			username.TabIndex = 1;
			username.Validating += username_Validating;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label1.ForeColor = SystemColors.MenuHighlight;
			label1.Location = new Point(155, 3);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(169, 16);
			label1.TabIndex = 0;
			label1.Text = "Connecting To SalesNrich..";
			// 
			// panel2
			// 
			panel2.BackColor = Color.Transparent;
			panel2.BackgroundImageLayout = ImageLayout.None;
			panel2.Controls.Add(pictureBox1);
			panel2.Controls.Add(pictureBox2);
			panel2.Dock = DockStyle.Bottom;
			panel2.Location = new Point(0, -314);
			panel2.Margin = new Padding(4, 3, 4, 3);
			panel2.Name = "panel2";
			panel2.Size = new Size(488, 312);
			panel2.TabIndex = 1;
			// 
			// pictureBox1
			// 
			pictureBox1.Image = Properties.Resources.salesnrich;
			pictureBox1.Location = new Point(95, 176);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(305, 58);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			pictureBox2.Image = Properties.Resources.logo;
			pictureBox2.Location = new Point(181, 52);
			pictureBox2.Margin = new Padding(4, 3, 4, 3);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new Size(122, 115);
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox2.TabIndex = 0;
			pictureBox2.TabStop = false;
			// 
			// timer1
			// 
			timer1.Enabled = true;
			timer1.Interval = 10;
			timer1.Tick += timer1_Tick;
			// 
			// errorProvider1
			// 
			errorProvider1.ContainerControl = this;
			// 
			// LoginScreen
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(488, 341);
			ControlBox = false;
			Controls.Add(panel2);
			Controls.Add(panel1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			Name = "LoginScreen";
			StartPosition = FormStartPosition.CenterScreen;
			Load += UserControl1_Load;
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)close_btn).EndInit();
			((System.ComponentModel.ISupportInitialize)SnrHeadding).EndInit();
			panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
			ResumeLayout(false);
		}

	

		#endregion

		private Panel panel1;
        private Label label1;
        private Panel panel2;
        private PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private Button button1;
        private TextBox password;
        private TextBox username;
        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
        private Panel panel3;
        private Button button2;
        private CheckBox rememberMeChkBox;
        private ErrorProvider errorProvider1;
        private PictureBox SnrHeadding;
        private Label ErrorLabel;
        private PictureBox pictureBox4;
        private PictureBox close_btn;
        //private LinkLabel linkLabel1;
    }
}