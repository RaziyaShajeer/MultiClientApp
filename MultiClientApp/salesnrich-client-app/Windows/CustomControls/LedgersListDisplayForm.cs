using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows.CustomControls
{
	public partial class LedgersListDisplayForm : Form
	{
		private string message;

		public LedgersListDisplayForm()
		{
			InitializeComponent();

		}

		public LedgersListDisplayForm(string message)
		{
			this.message = message;
		}

		private void LedgersListDisplayForm_Load(object sender, EventArgs e)
		{
			// Center the TextBox
			// Center the TextBox

			textBox1.Text = message;
		}

		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
