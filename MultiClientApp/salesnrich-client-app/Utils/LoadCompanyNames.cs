using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
	public class LoadCompanyNames
	{
		public static List<string> LoadCompanyNamesoftally()
		{
			List<string> companyNames = new List<string>();
			try
			{
				// object[] row = tallyService.getCompanies();
				string resxDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string targetKey = "tally.company";
				string fileToExclude = "ClientAppProps.resx";
		

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
			return companyNames;
			}
			catch (Exception e)
			{
				LogManager.HandleException(e);
				MessageBox.Show("Unable to fetch Company names \n" + e.Message);
				return companyNames;
			}
			
		}

	}
}

