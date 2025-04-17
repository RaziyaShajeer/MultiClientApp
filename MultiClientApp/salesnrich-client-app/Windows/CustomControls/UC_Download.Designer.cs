namespace SNR_ClientApp.Windows.CustomControls
{
    partial class UC_Download
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
			DownloadTabMainPanel = new Panel();
			LoggerArea = new Panel();
			flowLayoutPanel1 = new FlowLayoutPanel();
			panel3 = new Panel();
			label1 = new Label();
			panel2 = new Panel();
			companySelect = new ComboBox();
			btn_downloadReceipt = new Button();
			btn_Journal = new Button();
			btn_optionalReceipt = new Button();
			btn_salesOrder = new Button();
			btn_sales_download = new Button();
			btn_vansales = new Button();
			btn_SalesJournal = new Button();
			btn_accountProfiles = new Button();
			btn_Sales_Retrun = new Button();
			employeeList = new ComboBox();
			panel1 = new Panel();
			salesDate = new DateTimePicker();
			DownloadTabMainPanel.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			panel3.SuspendLayout();
			panel2.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// DownloadTabMainPanel
			// 
			DownloadTabMainPanel.Controls.Add(LoggerArea);
			DownloadTabMainPanel.Controls.Add(flowLayoutPanel1);
			DownloadTabMainPanel.Dock = DockStyle.Fill;
			DownloadTabMainPanel.Location = new Point(0, 0);
			DownloadTabMainPanel.Name = "DownloadTabMainPanel";
			DownloadTabMainPanel.Size = new Size(800, 575);
			DownloadTabMainPanel.TabIndex = 0;
			// 
			// LoggerArea
			// 
			LoggerArea.AutoScroll = true;
			LoggerArea.AutoScrollMargin = new Size(2, 2);
			LoggerArea.Dock = DockStyle.Fill;
			LoggerArea.Location = new Point(299, 0);
			LoggerArea.Name = "LoggerArea";
			LoggerArea.Size = new Size(501, 575);
			LoggerArea.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.AutoScroll = true;
			flowLayoutPanel1.BackColor = Color.FromArgb(143, 184, 202);
			flowLayoutPanel1.Controls.Add(panel3);
			flowLayoutPanel1.Controls.Add(panel2);
			flowLayoutPanel1.Controls.Add(btn_downloadReceipt);
			flowLayoutPanel1.Controls.Add(btn_Journal);
			flowLayoutPanel1.Controls.Add(btn_optionalReceipt);
			flowLayoutPanel1.Controls.Add(btn_salesOrder);
			flowLayoutPanel1.Controls.Add(btn_sales_download);
			flowLayoutPanel1.Controls.Add(btn_vansales);
			flowLayoutPanel1.Controls.Add(btn_SalesJournal);
			flowLayoutPanel1.Controls.Add(btn_accountProfiles);
			flowLayoutPanel1.Controls.Add(btn_Sales_Retrun);
			flowLayoutPanel1.Controls.Add(employeeList);
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Dock = DockStyle.Left;
			flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
			flowLayoutPanel1.ForeColor = Color.White;
			flowLayoutPanel1.Location = new Point(0, 0);
			flowLayoutPanel1.Margin = new Padding(5);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Padding = new Padding(5);
			flowLayoutPanel1.Size = new Size(299, 575);
			flowLayoutPanel1.TabIndex = 3;
			flowLayoutPanel1.WrapContents = false;
			
			// 
			// panel3
			// 
			panel3.Controls.Add(label1);
			panel3.Dock = DockStyle.Fill;
			panel3.Location = new Point(8, 8);
			panel3.Name = "panel3";
			panel3.Size = new Size(260, 77);
			panel3.TabIndex = 54;
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Top;
			label1.AutoSize = true;
			label1.Font = new Font("Palatino Linotype", 12F, FontStyle.Bold, GraphicsUnit.Point);
			label1.Location = new Point(54, 29);
			label1.Name = "label1";
			label1.Size = new Size(0, 22);
			label1.TabIndex = 0;
			// 
			// panel2
			// 
			panel2.Controls.Add(companySelect);
			panel2.Location = new Point(8, 91);
			panel2.Name = "panel2";
			panel2.Padding = new Padding(5, 10, 5, 5);
			panel2.Size = new Size(260, 45);
			panel2.TabIndex = 52;
			// 
			// companySelect
			// 
			companySelect.BackColor = Color.FromArgb(224, 224, 224);
			companySelect.DropDownStyle = ComboBoxStyle.DropDownList;
			companySelect.FlatStyle = FlatStyle.Popup;
			companySelect.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
			companySelect.FormattingEnabled = true;
			companySelect.ImeMode = ImeMode.Hiragana;
			companySelect.Location = new Point(12, 10);
			companySelect.Margin = new Padding(15, 3, 3, 3);
			companySelect.Name = "companySelect";
			companySelect.Size = new Size(248, 27);
			companySelect.TabIndex = 51;
			// 
			// btn_downloadReceipt
			// 
			btn_downloadReceipt.BackColor = Color.FromArgb(33, 141, 170);
			btn_downloadReceipt.Dock = DockStyle.Fill;
			btn_downloadReceipt.FlatAppearance.BorderColor = Color.FromArgb(6, 96, 137);
			btn_downloadReceipt.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_downloadReceipt.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_downloadReceipt.FlatStyle = FlatStyle.Flat;
			btn_downloadReceipt.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_downloadReceipt.Location = new Point(20, 142);
			btn_downloadReceipt.Margin = new Padding(15, 3, 3, 3);
			btn_downloadReceipt.Name = "btn_downloadReceipt";
			btn_downloadReceipt.Size = new Size(248, 28);
			btn_downloadReceipt.TabIndex = 55;
			btn_downloadReceipt.Text = "Receipts";
			btn_downloadReceipt.UseVisualStyleBackColor = false;
			btn_downloadReceipt.Click += button1_Click;
			// 
			// btn_Journal
			// 
			btn_Journal.BackColor = Color.FromArgb(33, 141, 170);
			btn_Journal.Dock = DockStyle.Fill;
			btn_Journal.FlatAppearance.BorderColor = Color.FromArgb(6, 96, 137);
			btn_Journal.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_Journal.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_Journal.FlatStyle = FlatStyle.Flat;
			btn_Journal.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_Journal.Location = new Point(20, 176);
			btn_Journal.Margin = new Padding(15, 3, 3, 3);
			btn_Journal.Name = "btn_Journal";
			btn_Journal.Size = new Size(248, 28);
			btn_Journal.TabIndex = 56;
			btn_Journal.Text = "Journal";
			btn_Journal.UseVisualStyleBackColor = false;
			btn_Journal.Click += btn_Journal_Click;
			// 
			// btn_optionalReceipt
			// 
			btn_optionalReceipt.BackColor = Color.FromArgb(33, 141, 170);
			btn_optionalReceipt.Dock = DockStyle.Fill;
			btn_optionalReceipt.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
			btn_optionalReceipt.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_optionalReceipt.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_optionalReceipt.FlatStyle = FlatStyle.Flat;
			btn_optionalReceipt.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_optionalReceipt.Location = new Point(20, 210);
			btn_optionalReceipt.Margin = new Padding(15, 3, 3, 3);
			btn_optionalReceipt.Name = "btn_optionalReceipt";
			btn_optionalReceipt.Size = new Size(248, 28);
			btn_optionalReceipt.TabIndex = 57;
			btn_optionalReceipt.Text = "Optional Receipt (Daily)";
			btn_optionalReceipt.UseVisualStyleBackColor = false;
			btn_optionalReceipt.Click += btn_optionalReceipt_Click;
			// 
			// btn_salesOrder
			// 
			btn_salesOrder.BackColor = Color.FromArgb(33, 141, 170);
			btn_salesOrder.Dock = DockStyle.Fill;
			btn_salesOrder.FlatAppearance.BorderColor = Color.FromArgb(6, 96, 137);
			btn_salesOrder.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_salesOrder.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_salesOrder.FlatStyle = FlatStyle.Flat;
			btn_salesOrder.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_salesOrder.Location = new Point(20, 244);
			btn_salesOrder.Margin = new Padding(15, 3, 3, 3);
			btn_salesOrder.Name = "btn_salesOrder";
			btn_salesOrder.Size = new Size(248, 28);
			btn_salesOrder.TabIndex = 58;
			btn_salesOrder.Text = "Sales Order";
			btn_salesOrder.UseVisualStyleBackColor = false;
			btn_salesOrder.Click += btn_salesOrder_Click;
			// 
			// btn_sales_download
			// 
			btn_sales_download.BackColor = Color.FromArgb(33, 141, 170);
			btn_sales_download.Dock = DockStyle.Fill;
			btn_sales_download.FlatAppearance.BorderColor = Color.FromArgb(6, 96, 137);
			btn_sales_download.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_sales_download.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_sales_download.FlatStyle = FlatStyle.Flat;
			btn_sales_download.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_sales_download.Location = new Point(20, 278);
			btn_sales_download.Margin = new Padding(15, 3, 3, 3);
			btn_sales_download.Name = "btn_sales_download";
			btn_sales_download.Size = new Size(248, 28);
			btn_sales_download.TabIndex = 59;
			btn_sales_download.Text = "Sales";
			btn_sales_download.UseVisualStyleBackColor = false;
			btn_sales_download.Click += btn_sales_download_Click;
			// 
			// btn_vansales
			// 
			btn_vansales.BackColor = Color.FromArgb(33, 141, 170);
			btn_vansales.Dock = DockStyle.Fill;
			btn_vansales.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
			btn_vansales.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_vansales.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_vansales.FlatStyle = FlatStyle.Flat;
			btn_vansales.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_vansales.Location = new Point(20, 312);
			btn_vansales.Margin = new Padding(15, 3, 3, 3);
			btn_vansales.Name = "btn_vansales";
			btn_vansales.Size = new Size(248, 28);
			btn_vansales.TabIndex = 60;
			btn_vansales.Text = "Vansales";
			btn_vansales.UseVisualStyleBackColor = false;
			btn_vansales.Click += btn_vansales_Click;
			// 
			// btn_SalesJournal
			// 
			btn_SalesJournal.BackColor = Color.FromArgb(33, 141, 170);
			btn_SalesJournal.Dock = DockStyle.Fill;
			btn_SalesJournal.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
			btn_SalesJournal.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_SalesJournal.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_SalesJournal.FlatStyle = FlatStyle.Flat;
			btn_SalesJournal.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_SalesJournal.Location = new Point(20, 346);
			btn_SalesJournal.Margin = new Padding(15, 3, 3, 3);
			btn_SalesJournal.Name = "btn_SalesJournal";
			btn_SalesJournal.Size = new Size(248, 28);
			btn_SalesJournal.TabIndex = 61;
			btn_SalesJournal.Text = "Sales Journal";
			btn_SalesJournal.UseVisualStyleBackColor = false;
			btn_SalesJournal.Click += btn_SalesJournal_Click;
			// 
			// btn_accountProfiles
			// 
			btn_accountProfiles.BackColor = Color.FromArgb(33, 141, 170);
			btn_accountProfiles.Dock = DockStyle.Fill;
			btn_accountProfiles.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
			btn_accountProfiles.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_accountProfiles.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_accountProfiles.FlatStyle = FlatStyle.Flat;
			btn_accountProfiles.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_accountProfiles.Location = new Point(20, 380);
			btn_accountProfiles.Margin = new Padding(15, 3, 3, 3);
			btn_accountProfiles.Name = "btn_accountProfiles";
			btn_accountProfiles.Size = new Size(248, 28);
			btn_accountProfiles.TabIndex = 62;
			btn_accountProfiles.Text = "Account Profiles";
			btn_accountProfiles.UseVisualStyleBackColor = false;
			btn_accountProfiles.Click += btn_accountProfiles_Click;
			// 
			// btn_Sales_Retrun
			// 
			btn_Sales_Retrun.BackColor = Color.FromArgb(33, 141, 170);
			btn_Sales_Retrun.Dock = DockStyle.Fill;
			btn_Sales_Retrun.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
			btn_Sales_Retrun.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
			btn_Sales_Retrun.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
			btn_Sales_Retrun.FlatStyle = FlatStyle.Flat;
			btn_Sales_Retrun.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
			btn_Sales_Retrun.Location = new Point(20, 414);
			btn_Sales_Retrun.Margin = new Padding(15, 3, 3, 3);
			btn_Sales_Retrun.Name = "btn_Sales_Retrun";
			btn_Sales_Retrun.Size = new Size(248, 28);
			btn_Sales_Retrun.TabIndex = 63;
			btn_Sales_Retrun.Text = "Sales Return";
			btn_Sales_Retrun.UseVisualStyleBackColor = false;
			btn_Sales_Retrun.Click += btn_Sales_Retrun_Click;
			// 
			// employeeList
			// 
			employeeList.BackColor = Color.FromArgb(224, 224, 224);
			employeeList.Dock = DockStyle.Fill;
			employeeList.DropDownStyle = ComboBoxStyle.DropDownList;
			employeeList.FlatStyle = FlatStyle.Popup;
			employeeList.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
			employeeList.FormattingEnabled = true;
			employeeList.ImeMode = ImeMode.Hiragana;
			employeeList.Location = new Point(20, 448);
			employeeList.Margin = new Padding(15, 3, 3, 3);
			employeeList.Name = "employeeList";
			employeeList.Size = new Size(248, 27);
			employeeList.TabIndex = 64;
			employeeList.Visible = false;
			// 
			// panel1
			// 
			panel1.Controls.Add(salesDate);
			panel1.Location = new Point(8, 481);
			panel1.Name = "panel1";
			panel1.Size = new Size(260, 76);
			panel1.TabIndex = 53;
			// 
			// salesDate
			// 
			salesDate.Location = new Point(12, 30);
			salesDate.Name = "salesDate";
			salesDate.Size = new Size(248, 23);
			salesDate.TabIndex = 0;
			// 
			// UC_Download
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(DownloadTabMainPanel);
			Name = "UC_Download";
			Size = new Size(800, 575);
			DownloadTabMainPanel.ResumeLayout(false);
			flowLayoutPanel1.ResumeLayout(false);
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panel2.ResumeLayout(false);
			panel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel DownloadTabMainPanel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel3;
        private Panel panel2;
        private ComboBox companySelect;
        private Button button1;
        private Panel panel1;
        private Panel LoggerArea;
        private Label label1;
        private Button btn_Journal;
        private Button btn_optionalReceipt;
        private Button btn_salesOrder;
        private Button btn_sales_download;
        private Button btn_vansales;
        private Button btn_SalesJournal;
        private Button button8;
        private DateTimePicker salesDate;
        private ComboBox employeeList;
        private Button btn_downloadReceipt;
        private Button btn_accountProfiles;
        private Button button2;
       private Button btn_Sales_Retrun;
    }
}
