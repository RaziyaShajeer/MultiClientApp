//using SNR_ClientApp.Helpers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
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

		public Dictionary<string, object> props => lazyProps.Value;
		private Dictionary<string, string> propertiestoupdate = new Dictionary<string, string>();
		public static string selectedCompany;
		public TalllyConnect()
		{
			ApplicationProperties s = new ApplicationProperties();
			InitializeComponent();
			tallyService = new TallyService();
			companySelect.Items.Clear();
			loadDefaultDatas();

		}
		private void loadDefaultDatas()
		{
			//var host = ApplicationProperties.properties.GetValueOrDefault("tally.hostname");
			//var port = ApplicationProperties.properties.GetValueOrDefault("tally.port");
			//if (host != null && !String.IsNullOrEmpty(host.ToString()))
			//{
			//	txtHost.Text = ApplicationProperties.properties["tally.hostname"].ToString();
			//}
			//if (port != null && !String.IsNullOrEmpty(host.ToString()))
			//{
			//	txtPort.Text = ApplicationProperties.properties["tally.port"].ToString();
			//}
			//string odbcDsn = ApplicationProperties.properties["tally.odbcdsn"].ToString();

			//if (String.IsNullOrEmpty(odbcDsn))
			//    odbcDsn="TallyODBC64_"+port?.ToString()?.Trim();
			//txt_odbc_dsn.Text = odbcDsn;
			companySelect.SelectedItem = null;
			StringUtilsCustom.TALLY_COMPANY = null;
		}

		private void button1_Click(object sender, EventArgs e)

		{

			
			companySelect.SelectedItem = null;
			ParentForm.Cursor = Cursors.WaitCursor;
			try
			{
				//companySelect.DataSource="";
				string odbcDsn = txt_odbc_dsn.Text;
				if (String.IsNullOrEmpty(odbcDsn))
				{
					odbcDsn = "TallyODBC64_" + txtPort.Text?.ToString()?.Trim();
					txt_odbc_dsn.Text = odbcDsn;
				}


				if (ValidateChildren(ValidationConstraints.Enabled))
				{
					//	LogManager.WriteLog("Connecting To Tally Started...");
					bool res = tallyService.Connect(txtHost.Text, txtPort.Text, odbcDsn);

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
						MainForm mainform = new MainForm();
						mainform.Show();
						this.Hide();
						ParentForm.Hide();
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

					//ApplicationProperties.userinitialproperty["tally.company"]= companySelect.Text;

					selectedCompanyName = companySelect.Text;

					ApplicationProperties.createPropertyFile(selectedCompanyName);
					ApplicationProperties.properties["tally.company"] = companySelect.Text;
					//ApplicationProperties.userinitialproperty["tally.company"]= companySelect.Text;

					//ApplicationProperties.updatePropertiesFile();
					ApplicationProperties.updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);
					if (ApplicationProperties.properties["isFirstTimeCompanyLogin"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
					{
						ApplicationProperties.getPropertyFromServer();
					}
					StringUtilsCustom.TALLY_COMPANY = companySelect.Text;
					ApplicationProperties.setProperties(TallyService.props);
					ApplicationProperties.getAllProperties(selectedCompanyName);
					TallyProperties1_UC tallyProperties1_UC = new TallyProperties1_UC();
					tallyProperties1_UC.parentform = ParentForm;
					ParentForm.AddUserControl(tallyProperties1_UC);

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


		private void companySelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			StringUtilsCustom.TALLY_COMPANY = companySelect.Text;
			//ApplicationProperties.getAllProperties(StringUtilsCustom.TALLY_COMPANY);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			TallyUpdateform tallyUpdateform = new TallyUpdateform();

			if (tallyUpdateform.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if (TallyUpdateform.companyToupdate != null)
					{
						string fileName = $"{TallyUpdateform.companyToupdate}.resx";
						string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

						if (File.Exists(filePath))
						{

							ResXResourceReader rsr = new ResXResourceReader(filePath);
							foreach (DictionaryEntry d in rsr)
							{
								propertiestoupdate.Add(d.Key.ToString(), d.Value.ToString());
								//Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());
							}
							propertiestoupdate["tally.company"] = StringUtilsCustom.TALLY_COMPANY;
							rsr.Close();
							ApplicationProperties.createPropertyFile(StringUtilsCustom.TALLY_COMPANY);
							ApplicationProperties.setProperties(propertiestoupdate);
							ApplicationProperties.Removepropertyfile(TallyUpdateform.companyToupdate);
						}

					}
				}
				catch (Exception ex)
				{

					LogManager.WriteLog($"not found. Creating new resource file. Error: {ex.Message}");



				}
			

			}
			else
			{
				LogManager.WriteLog("Hai");
			}

		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void txtHost_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtPort_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtHost_TextChanged_1(object sender, EventArgs e)
		{
			companySelect.SelectedItem = null;
		}

		private void txtPort_TextChanged_1(object sender, EventArgs e)
		{
			companySelect.SelectedItem = null;
		}
	}
}

