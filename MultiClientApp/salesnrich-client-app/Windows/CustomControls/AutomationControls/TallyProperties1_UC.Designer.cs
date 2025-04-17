namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    partial class TallyProperties1_UC
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TallyProperties1_UC));
			panel1 = new Panel();
			IgstCheckedListbox = new CheckedListBox();
			Txt_DistributedCode = new TextBox();
			label1 = new Label();
			txt_uploadtime = new TextBox();
			chk_upload = new CheckBox();
			txt_downloadtime = new TextBox();
			chk_download = new CheckBox();
			btn_Back = new Button();
			pictureBox6 = new PictureBox();
			pictureBox5 = new PictureBox();
			pictureBox4 = new PictureBox();
			pictureBox3 = new PictureBox();
			pictureBox2 = new PictureBox();
			infoHostName = new PictureBox();
			GstCheckedListBox = new CheckedListBox();
			SalesLedgerSelect = new ComboBox();
			label9 = new Label();
			label4 = new Label();
			SalesLedgerParentSelect = new ComboBox();
			Chk_EnalbeIgst = new CheckBox();
			tallyLedgerParentSelect = new ComboBox();
			label8 = new Label();
			lbl_EnabeIgst = new Label();
			label7 = new Label();
			label6 = new Label();
			button2 = new Button();
			AccountGroupsSelect = new ComboBox();
			label3 = new Label();
			label2 = new Label();
			errorProvider1 = new ErrorProvider(components);
			toolTip1 = new ToolTip(components);
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)infoHostName).BeginInit();
			((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.Anchor = AnchorStyles.None;
			panel1.BackColor = Color.White;
			panel1.BackgroundImageLayout = ImageLayout.Center;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add(IgstCheckedListbox);
			panel1.Controls.Add(Txt_DistributedCode);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(txt_uploadtime);
			panel1.Controls.Add(chk_upload);
			panel1.Controls.Add(txt_downloadtime);
			panel1.Controls.Add(chk_download);
			panel1.Controls.Add(btn_Back);
			panel1.Controls.Add(pictureBox6);
			panel1.Controls.Add(pictureBox5);
			panel1.Controls.Add(pictureBox4);
			panel1.Controls.Add(pictureBox3);
			panel1.Controls.Add(pictureBox2);
			panel1.Controls.Add(infoHostName);
			panel1.Controls.Add(GstCheckedListBox);
			panel1.Controls.Add(SalesLedgerSelect);
			panel1.Controls.Add(label9);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(SalesLedgerParentSelect);
			panel1.Controls.Add(Chk_EnalbeIgst);
			panel1.Controls.Add(tallyLedgerParentSelect);
			panel1.Controls.Add(label8);
			panel1.Controls.Add(lbl_EnabeIgst);
			panel1.Controls.Add(label7);
			panel1.Controls.Add(label6);
			panel1.Controls.Add(button2);
			panel1.Controls.Add(AccountGroupsSelect);
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label2);
			panel1.Location = new Point(48, 29);
			panel1.Name = "panel1";
			panel1.Size = new Size(691, 344);
			panel1.TabIndex = 15;
			// 
			// IgstCheckedListbox
			// 
			IgstCheckedListbox.Enabled = false;
			IgstCheckedListbox.FormattingEnabled = true;
			IgstCheckedListbox.Location = new Point(38, 109);
			IgstCheckedListbox.Name = "IgstCheckedListbox";
			IgstCheckedListbox.Size = new Size(271, 58);
			IgstCheckedListbox.TabIndex = 29;
			// 
			// Txt_DistributedCode
			// 
			Txt_DistributedCode.AutoCompleteMode = AutoCompleteMode.Suggest;
			Txt_DistributedCode.Location = new Point(189, 246);
			Txt_DistributedCode.Name = "Txt_DistributedCode";
			Txt_DistributedCode.Size = new Size(117, 23);
			Txt_DistributedCode.TabIndex = 43;
			toolTip1.SetToolTip(Txt_DistributedCode, "Please Enter in Minute");
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(38, 252);
			label1.Name = "label1";
			label1.Size = new Size(96, 15);
			label1.TabIndex = 42;
			label1.Text = "Distributed Code";
			// 
			// txt_uploadtime
			// 
			txt_uploadtime.Location = new Point(520, 248);
			txt_uploadtime.Name = "txt_uploadtime";
			txt_uploadtime.Size = new Size(117, 23);
			txt_uploadtime.TabIndex = 41;
			toolTip1.SetToolTip(txt_uploadtime, "Please Enter in Minute");
			txt_uploadtime.TextChanged += txt_uploadtime_TextChanged;
			// 
			// chk_upload
			// 
			chk_upload.AutoSize = true;
			chk_upload.Location = new Point(372, 248);
			chk_upload.Name = "chk_upload";
			chk_upload.Size = new Size(93, 19);
			chk_upload.TabIndex = 40;
			chk_upload.Text = "Auto Upload";
			chk_upload.UseVisualStyleBackColor = true;
			chk_upload.CheckedChanged += chk_upload_CheckedChanged;
			// 
			// txt_downloadtime
			// 
			txt_downloadtime.AutoCompleteMode = AutoCompleteMode.Suggest;
			txt_downloadtime.Location = new Point(520, 203);
			txt_downloadtime.Name = "txt_downloadtime";
			txt_downloadtime.Size = new Size(117, 23);
			txt_downloadtime.TabIndex = 39;
			toolTip1.SetToolTip(txt_downloadtime, "Please Enter in Minute");
			txt_downloadtime.TextChanged += txt_downloadtime_TextChanged;
			// 
			// chk_download
			// 
			chk_download.AutoSize = true;
			chk_download.Location = new Point(372, 205);
			chk_download.Name = "chk_download";
			chk_download.Size = new Size(109, 19);
			chk_download.TabIndex = 38;
			chk_download.Text = "Auto Download";
			chk_download.UseVisualStyleBackColor = true;
			chk_download.CheckedChanged += chk_download_CheckedChanged;
			// 
			// btn_Back
			// 
			btn_Back.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btn_Back.BackColor = Color.Black;
			btn_Back.FlatAppearance.BorderSize = 0;
			btn_Back.FlatAppearance.MouseDownBackColor = Color.Gray;
			btn_Back.FlatAppearance.MouseOverBackColor = Color.DimGray;
			btn_Back.FlatStyle = FlatStyle.Flat;
			btn_Back.Font = new Font("Nirmala UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
			btn_Back.ForeColor = Color.White;
			btn_Back.Location = new Point(39, 631);
			btn_Back.Name = "btn_Back";
			btn_Back.Size = new Size(89, 33);
			btn_Back.TabIndex = 37;
			btn_Back.Text = "Back";
			btn_Back.UseVisualStyleBackColor = false;
			btn_Back.Click += btn_Back_Click;
			// 
			// pictureBox6
			// 
			pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
			pictureBox6.Location = new Point(622, 155);
			pictureBox6.Name = "pictureBox6";
			pictureBox6.Size = new Size(15, 15);
			pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox6.TabIndex = 36;
			pictureBox6.TabStop = false;
			toolTip1.SetToolTip(pictureBox6, "Select  Account Group Related to Sales");
			// 
			// pictureBox5
			// 
			pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
			pictureBox5.Location = new Point(622, 105);
			pictureBox5.Name = "pictureBox5";
			pictureBox5.Size = new Size(15, 15);
			pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox5.TabIndex = 35;
			pictureBox5.TabStop = false;
			toolTip1.SetToolTip(pictureBox5, "Select the Account Group related to Customer Ledgers");
			// 
			// pictureBox4
			// 
			pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
			pictureBox4.Location = new Point(622, 20);
			pictureBox4.Name = "pictureBox4";
			pictureBox4.Size = new Size(15, 15);
			pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox4.TabIndex = 34;
			pictureBox4.TabStop = false;
			toolTip1.SetToolTip(pictureBox4, "Select the ledgers related to CGST and SGST ");
			// 
			// pictureBox3
			// 
			pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
			pictureBox3.Location = new Point(291, 178);
			pictureBox3.Name = "pictureBox3";
			pictureBox3.Size = new Size(15, 15);
			pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox3.TabIndex = 33;
			pictureBox3.TabStop = false;
			toolTip1.SetToolTip(pictureBox3, "Select the ledgers related to Sales");
			// 
			// pictureBox2
			// 
			pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
			pictureBox2.Location = new Point(294, 67);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new Size(15, 15);
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.TabIndex = 32;
			pictureBox2.TabStop = false;
			toolTip1.SetToolTip(pictureBox2, "Select the ledgers related to IGST");
			// 
			// infoHostName
			// 
			infoHostName.Image = (Image)resources.GetObject("infoHostName.Image");
			infoHostName.Location = new Point(294, 20);
			infoHostName.Name = "infoHostName";
			infoHostName.Size = new Size(15, 15);
			infoHostName.SizeMode = PictureBoxSizeMode.Zoom;
			infoHostName.TabIndex = 30;
			infoHostName.TabStop = false;
			toolTip1.SetToolTip(infoHostName, "Select the Account Group related to Tax Ledgers ");
			// 
			// GstCheckedListBox
			// 
			GstCheckedListBox.CheckOnClick = true;
			GstCheckedListBox.FormattingEnabled = true;
			GstCheckedListBox.HorizontalScrollbar = true;
			GstCheckedListBox.Location = new Point(372, 41);
			GstCheckedListBox.Name = "GstCheckedListBox";
			GstCheckedListBox.Size = new Size(265, 58);
			GstCheckedListBox.TabIndex = 28;
			GstCheckedListBox.Validating += GstCheckedListBox_Validating;
			// 
			// SalesLedgerSelect
			// 
			SalesLedgerSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			SalesLedgerSelect.FormattingEnabled = true;
			SalesLedgerSelect.ImeMode = ImeMode.Hiragana;
			SalesLedgerSelect.Location = new Point(372, 170);
			SalesLedgerSelect.Name = "SalesLedgerSelect";
			SalesLedgerSelect.Size = new Size(265, 23);
			SalesLedgerSelect.TabIndex = 26;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new Point(372, 152);
			label9.Name = "label9";
			label9.Size = new Size(75, 15);
			label9.TabIndex = 25;
			label9.Text = "Sales Ledger ";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(38, 178);
			label4.Name = "label4";
			label4.Size = new Size(145, 15);
			label4.TabIndex = 24;
			label4.Text = "Sales Ledger Parent Group";
			// 
			// SalesLedgerParentSelect
			// 
			SalesLedgerParentSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			SalesLedgerParentSelect.FormattingEnabled = true;
			SalesLedgerParentSelect.ImeMode = ImeMode.Hiragana;
			SalesLedgerParentSelect.Location = new Point(38, 205);
			SalesLedgerParentSelect.Name = "SalesLedgerParentSelect";
			SalesLedgerParentSelect.Size = new Size(268, 23);
			SalesLedgerParentSelect.TabIndex = 23;
			SalesLedgerParentSelect.SelectedIndexChanged += SalesLedgerParentSelect_SelectedIndexChanged;
			// 
			// Chk_EnalbeIgst
			// 
			Chk_EnalbeIgst.AutoSize = true;
			Chk_EnalbeIgst.Location = new Point(40, 67);
			Chk_EnalbeIgst.Name = "Chk_EnalbeIgst";
			Chk_EnalbeIgst.Size = new Size(174, 19);
			Chk_EnalbeIgst.TabIndex = 22;
			Chk_EnalbeIgst.Text = "Do You Want To Enable IGST";
			Chk_EnalbeIgst.UseVisualStyleBackColor = true;
			Chk_EnalbeIgst.CheckedChanged += Chk_EnaleIgst_CheckedChanged;
			// 
			// tallyLedgerParentSelect
			// 
			tallyLedgerParentSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			tallyLedgerParentSelect.FormattingEnabled = true;
			tallyLedgerParentSelect.ImeMode = ImeMode.Hiragana;
			tallyLedgerParentSelect.Location = new Point(372, 126);
			tallyLedgerParentSelect.Name = "tallyLedgerParentSelect";
			tallyLedgerParentSelect.Size = new Size(265, 23);
			tallyLedgerParentSelect.TabIndex = 21;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new Point(372, 102);
			label8.Name = "label8";
			label8.Size = new Size(135, 15);
			label8.TabIndex = 20;
			label8.Text = "Customer Parent  Group";
			// 
			// lbl_EnabeIgst
			// 
			lbl_EnabeIgst.AutoSize = true;
			lbl_EnabeIgst.Enabled = false;
			lbl_EnabeIgst.Location = new Point(40, 91);
			lbl_EnabeIgst.Name = "lbl_EnabeIgst";
			lbl_EnabeIgst.Size = new Size(75, 15);
			lbl_EnabeIgst.TabIndex = 19;
			lbl_EnabeIgst.Text = " IGST Ledger ";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(372, 20);
			label7.Name = "label7";
			label7.Size = new Size(132, 15);
			label7.TabIndex = 16;
			label7.Text = " CGST and SGST Ledger ";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(38, 20);
			label6.Name = "label6";
			label6.Size = new Size(136, 15);
			label6.TabIndex = 15;
			label6.Text = "Tax Ledger Parent Group";
			// 
			// button2
			// 
			button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			button2.BackColor = Color.SpringGreen;
			button2.FlatAppearance.BorderColor = Color.FromArgb(0, 64, 0);
			button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 64, 0);
			button2.FlatAppearance.MouseOverBackColor = Color.Green;
			button2.FlatStyle = FlatStyle.Flat;
			button2.Font = new Font("Nirmala UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
			button2.Location = new Point(548, 288);
			button2.Name = "button2";
			button2.Size = new Size(89, 33);
			button2.TabIndex = 9;
			button2.Text = "Next";
			button2.UseVisualStyleBackColor = false;
			button2.Click += button2_Click;
			// 
			// AccountGroupsSelect
			// 
			AccountGroupsSelect.BackColor = Color.FromArgb(224, 224, 224);
			AccountGroupsSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			AccountGroupsSelect.Font = new Font("Nirmala UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			AccountGroupsSelect.FormattingEnabled = true;
			AccountGroupsSelect.ImeMode = ImeMode.Hiragana;
			AccountGroupsSelect.Location = new Point(40, 41);
			AccountGroupsSelect.Name = "AccountGroupsSelect";
			AccountGroupsSelect.Size = new Size(269, 23);
			AccountGroupsSelect.TabIndex = 13;
			AccountGroupsSelect.SelectedIndexChanged += AccountGroupsSelect_SelectedIndexChanged;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
			label3.ForeColor = SystemColors.ButtonFace;
			label3.Location = new Point(178, 87);
			label3.Name = "label3";
			label3.Size = new Size(35, 19);
			label3.TabIndex = 10;
			label3.Text = "Port";
			label3.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
			label2.ForeColor = SystemColors.ButtonFace;
			label2.Location = new Point(165, 28);
			label2.Name = "label2";
			label2.Size = new Size(70, 19);
			label2.TabIndex = 9;
			label2.Text = "Hostname";
			label2.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// errorProvider1
			// 
			errorProvider1.ContainerControl = this;
			// 
			// toolTip1
			// 
			toolTip1.ToolTipIcon = ToolTipIcon.Info;
			toolTip1.ToolTipTitle = "Please Enter in Minute";
			// 
			// TallyProperties1_UC
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(33, 141, 170);
			Controls.Add(panel1);
			Name = "TallyProperties1_UC";
			Size = new Size(800, 403);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)infoHostName).EndInit();
			((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
        private CheckedListBox IgstCheckedListbox;
        private CheckedListBox GstCheckedListBox;
        private ComboBox SalesLedgerSelect;
        private Label label9;
        private Label label4;
        private ComboBox SalesLedgerParentSelect;
        private CheckBox Chk_EnalbeIgst;
        private ComboBox tallyLedgerParentSelect;
        private Label label8;
        private Label lbl_EnabeIgst;
        private Label label7;
        private Label label6;
        private Button button2;
        private ComboBox AccountGroupsSelect;
        private Label label3;
        private Label label2;
        private ErrorProvider errorProvider1;
        private PictureBox pictureBox6;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private PictureBox infoHostName;
        private ToolTip toolTip1;
        private Button btn_Back;
        private CheckBox chk_download;
        private TextBox txt_uploadtime;
        private CheckBox chk_upload;
        private TextBox txt_downloadtime;
        private TextBox textBox1;
        private Label label1;
        private TextBox Txt_DistributedCode;
    }
}
