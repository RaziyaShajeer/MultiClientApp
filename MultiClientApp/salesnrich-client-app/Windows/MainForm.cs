using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows
{
    public partial class MainForm : Form
    {
        static List<string> allAssignedSyncOperations;
        private static bool downloadStatus;
        BackgroundTaskManagerService _BackgroundTaskManagerService;
        public MainForm ()
        {

            InitializeComponent();
            _BackgroundTaskManagerService=new BackgroundTaskManagerService();
            UC_UserHome home = new UC_UserHome();
            AddUserControl(home);

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

        private void AddUserControl (UserControl userControl)
        {
            userControl.Dock=DockStyle.Fill;
            parentContainer.Controls.Clear();
            parentContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void btn_Upload_Click (object sender, EventArgs e)
        {
            changeBtnBackgroundToDefault();
            btn_Upload.BackColor =System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(96)))), ((int)(((byte)(137)))));

            UC_UserHome home = new UC_UserHome();
            AddUserControl(home);

        }

        private void changeBtnBackgroundToDefault ()
        {
            btn_Upload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(141)))), ((int)(((byte)(170)))));
            btn_download.BackColor =  System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(141)))), ((int)(((byte)(170)))));
            btn_upload_sales_receipt.BackColor =  System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(141)))), ((int)(((byte)(170)))));

        }

        private void btn_download_Click (object sender, EventArgs e)
        {
            changeBtnBackgroundToDefault();
            btn_download.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(96)))), ((int)(((byte)(137)))));

            UC_Download uC_Download = new UC_Download();
            AddUserControl(uC_Download);
        }

        private void btn_upload_sales_receipt_Click (object sender, EventArgs e)
        {
            changeBtnBackgroundToDefault();
            btn_upload_sales_receipt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(96)))), ((int)(((byte)(137)))));

            UC_UploadSalesReceipt uC_UploadSalesReceipt = new UC_UploadSalesReceipt();
            AddUserControl(uC_UploadSalesReceipt);
        }
      
        private void pictureBox3_Click (object sender, EventArgs e)
        {
            BackgroundTaskManagerService.Instance.StopBackgroundTask();
            Application.Exit();

        }

        private void pictureBox4_Click (object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                TallyConfigForm.isSettings = true;
                TallyConfigForm tallyConfigForm = new TallyConfigForm();

                tallyConfigForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void pictureBox5_Click (object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Load (object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed (object sender, FormClosedEventArgs e)
        {
            // Clean up other resources if necessary
            Dispose();  // Call Dispose to release form resources
            BackgroundTaskManagerService.Instance.StopBackgroundTask();
            // Exit the application
            Application.Exit();
        }
    }
}
