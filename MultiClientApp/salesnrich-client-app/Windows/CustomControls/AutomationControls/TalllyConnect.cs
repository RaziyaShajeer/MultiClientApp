//using SNR_ClientApp.Helpers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    public partial class TalllyConnect : UserControl
    {
        public static string selectedCompanyName;
        public TallyConfigForm ParentForm { get; set; }
        TallyService tallyService;
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;
        public static string selectedCompany;
        public TalllyConnect()
        {
            ApplicationProperties s = new ApplicationProperties();
            InitializeComponent();
            tallyService = new TallyService();
            loadDefaultDatas();
        }
        private void loadDefaultDatas()
        {
            var host = ApplicationProperties.properties.GetValueOrDefault("tally.hostname");
            var port = ApplicationProperties.properties.GetValueOrDefault("tally.port");
            if (host!=null && !String.IsNullOrEmpty(host.ToString()))
            {
                txtHost.Text= ApplicationProperties.properties["tally.hostname"].ToString();
            }
            if (port!=null && !String.IsNullOrEmpty(host.ToString()))
            {
                txtPort.Text= ApplicationProperties.properties["tally.port"].ToString();
            }
            string odbcDsn = ApplicationProperties.properties["tally.odbcdsn"].ToString();
        
            //if (String.IsNullOrEmpty(odbcDsn))
            //    odbcDsn="TallyODBC64_"+port?.ToString()?.Trim();
            txt_odbc_dsn.Text= odbcDsn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParentForm.Cursor = Cursors.WaitCursor;
            try
            {
                //companySelect.DataSource="";
                string odbcDsn = txt_odbc_dsn.Text;
                if (String.IsNullOrEmpty(odbcDsn))
                {
                   odbcDsn = "TallyODBC64_"+txtPort.Text?.ToString()?.Trim();
                    txt_odbc_dsn.Text= odbcDsn;
                }
                

                if (ValidateChildren(ValidationConstraints.Enabled))
                {
                    LogManager.WriteLog("Connecting To Tally Started...");
                    bool res = tallyService.Connect(txtHost.Text, txtPort.Text,odbcDsn);

                    if (res)
                    {
                        //LogManager.WriteLog("Tally Coonected SuccessFully...");
                        //MessageBox.Show("Tally Connected Successfully");
                        GetCompany();
                        companySelect.Enabled = true;
                        button2.Enabled = true;
                    }
                    else
                    {

                        MessageBox.Show("Tally unable to connect to Tally...");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        private async void GetCompany()
        {
            try
            {
                object[] row = await tallyService.getCompanies();
                companySelect.DataSource = row;
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
                MessageBox.Show("Unable to fetch Company names");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ParentForm.Cursor = Cursors.WaitCursor;
            try
            {
                if (companySelect.Text != "")
                {
                    ApplicationProperties.properties["tally.company"] = companySelect.Text;
                    ApplicationProperties.updatePropertiesFile();
                    StringUtilsCustom.TALLY_COMPANY = companySelect.Text;

                    //new Step().Forward(ParentForm, "tallyConnectPic", "tallyPropPic", "pictureBox5");
                    TallyProperties1_UC tallyProperties1_UC = new TallyProperties1_UC();
                    tallyProperties1_UC.parentform = ParentForm;
                    ParentForm.AddUserControl(tallyProperties1_UC);
                    selectedCompanyName = companySelect.Text;   
                }
                else
                {
                    MessageBox.Show("No Company Selected");
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }

        }

        private void txtHost_Validating(object sender, CancelEventArgs e)
        {
            if (txtHost.Text == string.Empty)
            {
                errorProvider1.SetError(txtHost, "Please Fillout Hostname");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtHost, "");
                e.Cancel = false;
            }
        }

        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            if (txtPort.Text == string.Empty)
            {
                errorProvider1.SetError(txtPort, "Please Fillout Portnumber");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPort, "");
                e.Cancel = false;
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            MainForm mainform = new MainForm();
            mainform.Show();
            this.Hide();
            ParentForm.Hide();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
