namespace SNR_ClientApp.Windows
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panel1 = new Panel();
            pictureBox5 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            pictureBox4 = new PictureBox();
            btn_upload_sales_receipt = new Button();
            btn_download = new Button();
            btn_Upload = new Button();
            parentContainer = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(6, 96, 137);
            panel1.Controls.Add(pictureBox5);
            panel1.Controls.Add(pictureBox3);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(820, 37);
            panel1.TabIndex = 0;
            panel1.Visible = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox5.BackColor = Color.FromArgb(6, 96, 137);
            pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            pictureBox5.Cursor = Cursors.Hand;
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(744, 8);
            pictureBox5.Margin = new Padding(10);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(20, 20);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 4;
            pictureBox5.TabStop = false;
            pictureBox5.Visible = false;
            pictureBox5.Click += pictureBox5_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = Properties.Resources.close_red;
            pictureBox3.Location = new Point(781, 8);
            pictureBox3.Margin = new Padding(10);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(20, 20);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            pictureBox3.Visible = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.salesnrich;
            pictureBox2.Location = new Point(51, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(131, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(12, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 25);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(33, 141, 170);
            panel2.Controls.Add(pictureBox4);
            panel2.Controls.Add(btn_upload_sales_receipt);
            panel2.Controls.Add(btn_download);
            panel2.Controls.Add(btn_Upload);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 37);
            panel2.Name = "panel2";
            panel2.Size = new Size(820, 33);
            panel2.TabIndex = 1;
            // 
            // pictureBox4
            // 
            pictureBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(781, 6);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(20, 21);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click;
            // 
            // btn_upload_sales_receipt
            // 
            btn_upload_sales_receipt.AutoSize = true;
            btn_upload_sales_receipt.FlatAppearance.BorderSize = 0;
            btn_upload_sales_receipt.FlatAppearance.MouseDownBackColor = Color.FromArgb(75, 162, 201);
            btn_upload_sales_receipt.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
            btn_upload_sales_receipt.FlatStyle = FlatStyle.Flat;
            btn_upload_sales_receipt.Font = new Font("Nirmala UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btn_upload_sales_receipt.ForeColor = Color.White;
            btn_upload_sales_receipt.Location = new Point(303, 0);
            btn_upload_sales_receipt.Name = "btn_upload_sales_receipt";
            btn_upload_sales_receipt.Size = new Size(183, 33);
            btn_upload_sales_receipt.TabIndex = 2;
            btn_upload_sales_receipt.Text = "Upload Sales/Receipt";
            btn_upload_sales_receipt.UseVisualStyleBackColor = true;
            btn_upload_sales_receipt.Click += btn_upload_sales_receipt_Click;
            // 
            // btn_download
            // 
            btn_download.FlatAppearance.BorderSize = 0;
            btn_download.FlatAppearance.MouseDownBackColor = Color.FromArgb(75, 162, 201);
            btn_download.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
            btn_download.FlatStyle = FlatStyle.Flat;
            btn_download.Font = new Font("Nirmala UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btn_download.ForeColor = Color.White;
            btn_download.Location = new Point(150, 0);
            btn_download.Name = "btn_download";
            btn_download.Size = new Size(157, 33);
            btn_download.TabIndex = 1;
            btn_download.Text = "Download";
            btn_download.UseVisualStyleBackColor = true;
            btn_download.Click += btn_download_Click;
            // 
            // btn_Upload
            // 
            btn_Upload.FlatAppearance.BorderSize = 0;
            btn_Upload.FlatAppearance.MouseDownBackColor = Color.FromArgb(75, 162, 201);
            btn_Upload.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 96, 137);
            btn_Upload.FlatStyle = FlatStyle.Flat;
            btn_Upload.Font = new Font("Nirmala UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btn_Upload.ForeColor = Color.White;
            btn_Upload.Location = new Point(-1, 0);
            btn_Upload.Name = "btn_Upload";
            btn_Upload.Size = new Size(157, 33);
            btn_Upload.TabIndex = 0;
            btn_Upload.Text = "Upload";
            btn_Upload.UseVisualStyleBackColor = true;
            btn_Upload.Click += btn_Upload_Click;
            // 
            // parentContainer
            // 
            parentContainer.Dock = DockStyle.Fill;
            parentContainer.Location = new Point(0, 70);
            parentContainer.Margin = new Padding(5);
            parentContainer.Name = "parentContainer";
            parentContainer.Size = new Size(820, 632);
            parentContainer.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(820, 702);
            Controls.Add(parentContainer);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SalesNrich";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private Panel panel2;
        //private Guna.UI2.WinForms.Guna2Button guna2Button2;
        //private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Panel parentContainer;
        private Button btn_Upload;
        private Button btn_download;
        private Button btn_upload_sales_receipt;
        private Panel panel3;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
    }
}