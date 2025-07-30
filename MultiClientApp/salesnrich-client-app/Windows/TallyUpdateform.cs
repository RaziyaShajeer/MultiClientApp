using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows
{
	public partial class TallyUpdateform : Form
	{
		public static string companyToupdate;
		public static string oldCompanyname=null;
		public TallyUpdateform()
		{
			InitializeComponent();
		}

		private void TallyUpdateform_Load(object sender, EventArgs e)
		{
			loadCompanyNames();
		}
		private void loadCompanyNames()
		{
			try
			{
				// object[] row = tallyService.getCompanies();
				string resxDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string targetKey = "tally.company";
				string fileToExclude = "ClientAppProps.resx";
				List<string> companyNames = new List<string>();

				foreach (string resxFile in Directory.GetFiles(resxDirectory, "*.resx", SearchOption.AllDirectories))
				{
					if (Path.GetFileName(resxFile).Equals(fileToExclude, StringComparison.OrdinalIgnoreCase))
						continue;
					using (ResXResourceReader reader = new ResXResourceReader(resxFile))
					{

						foreach (DictionaryEntry entry in reader)
						{
							if (entry.Key.ToString() == targetKey)
							{
								string value = entry.Value?.ToString();
								if (!string.IsNullOrEmpty(value))
								{
									companyNames.Add(value);
								}
							}
						}

					}

				}
				combo_ConnnectCompanies.DataSource = companyNames;
			}
			catch (Exception e)
			{
				LogManager.HandleException(e);
				MessageBox.Show("Unable to fetch Company names \n" + e.Message);
			}
		}
		private void combo_ConnnectCompanies_SelectedIndexChanged(object sender, EventArgs e)
		{


		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (combo_ConnnectCompanies.Items.Count == 0)
			{
				MessageBox.Show("No companies available to select.");
				return;
			}

			if (combo_ConnnectCompanies.SelectedItem == null)
			{
				MessageBox.Show("Please select a company.");
				return;
			}

			
			companyToupdate = combo_ConnnectCompanies.SelectedItem.ToString();
			oldCompanyname = companyToupdate;
			this.DialogResult = DialogResult.OK;
			this.Close(); // Closes ChildForm

		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();	
		}
	}
}
