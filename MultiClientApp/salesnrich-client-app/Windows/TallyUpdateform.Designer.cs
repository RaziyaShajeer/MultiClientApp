namespace SNR_ClientApp.Windows
{
	partial class TallyUpdateform
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
		private void InitializeComponent()
		{
			label2 = new Label();
			combo_ConnnectCompanies = new ComboBox();
			button1 = new Button();
			button2 = new Button();
			SuspendLayout();
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			label2.Location = new Point(51, 30);
			label2.Name = "label2";
			label2.Size = new Size(297, 15);
			label2.TabIndex = 5;
			label2.Text = "Select Configured Company Names want to Update?";
			// 
			// combo_ConnnectCompanies
			// 
			combo_ConnnectCompanies.FormattingEnabled = true;
			combo_ConnnectCompanies.Location = new Point(51, 62);
			combo_ConnnectCompanies.Name = "combo_ConnnectCompanies";
			combo_ConnnectCompanies.Size = new Size(287, 23);
			combo_ConnnectCompanies.TabIndex = 6;
			combo_ConnnectCompanies.SelectedIndexChanged += combo_ConnnectCompanies_SelectedIndexChanged;
			// 
			// button1
			// 
			button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			button1.Location = new Point(76, 115);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 7;
			button1.Text = "OK";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			button2.Location = new Point(206, 115);
			button2.Name = "button2";
			button2.Size = new Size(75, 23);
			button2.TabIndex = 8;
			button2.Text = "CANCEL";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// TallyUpdateform
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(406, 174);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(combo_ConnnectCompanies);
			Controls.Add(label2);
			Name = "TallyUpdateform";
			Text = "TallyUpdateform";
			Load += TallyUpdateform_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label2;
		private ComboBox combo_ConnnectCompanies;
		private Button button1;
		private Button button2;
	}
}