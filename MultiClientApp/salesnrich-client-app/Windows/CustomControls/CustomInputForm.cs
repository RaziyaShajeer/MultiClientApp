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
    public partial class CustomInputForm : Form
    {
        public string UserInput { get; private set; } // Property to store user input

        // Constructor to accept title, prompt, and default value
        public CustomInputForm (string title, string prompt, string defaultValue)
        {
            InitializeComponent();
            this.Text = title; // Set form title
            lblPrompt.Text = prompt; // Set label text
            txtInput.Text = defaultValue; // Set default value for textbox
        }

        public CustomInputForm ()
        {
        }




        // OK button click event handler
        private void btnOk_Click (object sender, EventArgs e)
        {
            this.UserInput = txtInput.Text; // Store the user input
            this.DialogResult = DialogResult.OK; // Set result to OK
            this.Close(); // Close the form
        }

        // Cancel button click event handler
        private void btnCancel_Click (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Set result to Cancel
            this.Close(); // Close the form
        }

        private void CustomInputForm_Load (object sender, EventArgs e)
        {

        }
    }
}
