using SNR_ClientApp.Services;
using SNR_ClientApp.Windows.CustomControls;
using SNR_ClientApp.Windows.CustomControls.AutomationControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace SNR_ClientApp.Windows
{
    public partial class TallyConfigForm : Form
    {
        public static bool isSettings { get; set; } = false;
        public TallyConfigForm ()
        {
            InitializeComponent();

            loadContent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void loadContent ()
        {
            if (!isSettings)
            {
                TalllyConnect talllyConnect = new TalllyConnect();
                talllyConnect.ParentForm = this;
                AddUserControl(talllyConnect);
            }
            else
            {
                //TallyProperties1_UC tallyProperties1_UC = new TallyProperties1_UC();
                //tallyProperties1_UC.parentform = this;
                //AddUserControl(tallyProperties1_UC);

                UC_Settings_login nextform = new UC_Settings_login();
                nextform.ParentForm = this;
                AddUserControl(nextform);

            }
        }

        public void AddUserControl (UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void pictureBox3_Click (object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click (object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TallyConfigForm_Load (object sender, EventArgs e)
        {

        }

        private void TallyConfigForm_FormClosed (object sender, FormClosedEventArgs e)
        {
            // Clean up other resources if necessary
            Dispose();  // Call Dispose to release form resources
            BackgroundTaskManagerService.Instance.StopBackgroundTask();
            // Exit the application
            Application.Exit();
        }
    }
}
