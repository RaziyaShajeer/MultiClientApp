namespace SNR_ClientApp.Windows.CustomControls
{
    partial class UC_UploadSalesReceipt
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
            MainPanel1 = new Panel();
            LoggerArea = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel3 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            DatePicker = new DateTimePicker();
            Btn_Receipt = new Button();
            Btn_Sales = new Button();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            MainPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // MainPanel1
            // 
            MainPanel1.Controls.Add(LoggerArea);
            MainPanel1.Controls.Add(flowLayoutPanel1);
            MainPanel1.Dock = DockStyle.Fill;
            MainPanel1.Location = new Point(0, 0);
            MainPanel1.Name = "MainPanel1";
            MainPanel1.Size = new Size(800, 575);
            MainPanel1.TabIndex = 0;
            // 
            // LoggerArea
            // 
            LoggerArea.Dock = DockStyle.Fill;
            LoggerArea.Location = new Point(260, 0);
            LoggerArea.Name = "LoggerArea";
            LoggerArea.Size = new Size(540, 575);
            LoggerArea.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = Color.FromArgb(143, 184, 202);
            flowLayoutPanel1.Controls.Add(panel3);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Controls.Add(Btn_Receipt);
            flowLayoutPanel1.Controls.Add(Btn_Sales);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(pictureBox1);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.ForeColor = Color.White;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(260, 575);
            flowLayoutPanel1.TabIndex = 4;
            flowLayoutPanel1.WrapContents = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(label1);
            panel3.Location = new Point(8, 8);
            panel3.Name = "panel3";
            panel3.Size = new Size(244, 88);
            panel3.TabIndex = 54;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Palatino Linotype", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(9, 27);
            label1.Margin = new Padding(10, 0, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(221, 22);
            label1.TabIndex = 0;
            label1.Text = "UPLOAD SALES/RECEIPTS";
            // 
            // panel2
            // 
            panel2.Controls.Add(DatePicker);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(8, 102);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5, 10, 5, 5);
            panel2.Size = new Size(244, 64);
            panel2.TabIndex = 52;
            // 
            // DatePicker
            // 
            DatePicker.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DatePicker.Location = new Point(15, 23);
            DatePicker.Margin = new Padding(10, 3, 3, 3);
            DatePicker.Name = "DatePicker";
            DatePicker.Size = new Size(215, 23);
            DatePicker.TabIndex = 0;
            // 
            // Btn_Receipt
            // 
            Btn_Receipt.BackColor = Color.FromArgb(33, 141, 170);
            Btn_Receipt.Dock = DockStyle.Fill;
            Btn_Receipt.FlatAppearance.BorderColor = Color.FromArgb(7, 58, 82);
            Btn_Receipt.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
            Btn_Receipt.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
            Btn_Receipt.FlatStyle = FlatStyle.Flat;
            Btn_Receipt.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_Receipt.Location = new Point(8, 172);
            Btn_Receipt.Name = "Btn_Receipt";
            Btn_Receipt.Size = new Size(244, 28);
            Btn_Receipt.TabIndex = 55;
            Btn_Receipt.Text = "Receipts";
            Btn_Receipt.UseVisualStyleBackColor = false;
            Btn_Receipt.Click += Btn_Receipt_Click;
            // 
            // Btn_Sales
            // 
            Btn_Sales.BackColor = Color.FromArgb(33, 141, 170);
            Btn_Sales.Dock = DockStyle.Fill;
            Btn_Sales.FlatAppearance.BorderColor = Color.FromArgb(6, 96, 137);
            Btn_Sales.FlatAppearance.MouseDownBackColor = Color.FromArgb(7, 58, 82);
            Btn_Sales.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
            Btn_Sales.FlatStyle = FlatStyle.Flat;
            Btn_Sales.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_Sales.Location = new Point(8, 206);
            Btn_Sales.Name = "Btn_Sales";
            Btn_Sales.Size = new Size(244, 28);
            Btn_Sales.TabIndex = 59;
            Btn_Sales.Text = "Sales";
            Btn_Sales.UseVisualStyleBackColor = false;
            Btn_Sales.Click += Btn_Sales_ClickAsync;
            // 
            // panel1
            // 
            panel1.Location = new Point(8, 240);
            panel1.Name = "panel1";
            panel1.Size = new Size(242, 58);
            panel1.TabIndex = 53;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom;
            pictureBox1.ErrorImage = Properties.Resources.preloader;
            flowLayoutPanel1.SetFlowBreak(pictureBox1, true);
            pictureBox1.ImageLocation = "center";
            pictureBox1.InitialImage = Properties.Resources.preloader;
            pictureBox1.Location = new Point(49, 304);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(162, 87);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 60;
            pictureBox1.TabStop = false;
            // 
            // UC_UploadSalesReceipt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel1);
            Name = "UC_UploadSalesReceipt";
            Size = new Size(800, 575);
            MainPanel1.ResumeLayout(false);
            MainPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel3;
        private Label label1;
        private Panel panel2;
        private Button Btn_Receipt;
        private Button Btn_Sales;
        private Panel panel1;
        private DateTimePicker DatePicker;
        private Panel LoggerArea;
        private PictureBox pictureBox1;
    }
}
