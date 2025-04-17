namespace SNR_ClientApp.Windows.CustomControls
{
    partial class UC_Logger
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
            this.loggerArea = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // loggerArea
            // 
            this.loggerArea.BackColor = System.Drawing.Color.White;
            this.loggerArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loggerArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggerArea.Location = new System.Drawing.Point(0, 0);
            this.loggerArea.Name = "loggerArea";
            this.loggerArea.ReadOnly = true;
            this.loggerArea.Size = new System.Drawing.Size(911, 575);
            this.loggerArea.TabIndex = 0;
            this.loggerArea.Text = "";
            // 
            // UC_Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.loggerArea);
            this.Name = "UC_Logger";
            this.Size = new System.Drawing.Size(911, 575);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox loggerArea;
    }
}
