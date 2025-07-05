using SNR_ClientApp.Config;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Services;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.Utils;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SNR_ClientApp.Properties
{
    public class ApplicationProperties
    {
        public static Dictionary<String, Object> properties = new Dictionary<String, Object>();
		public static Dictionary<String, Object> propertiesofTalllyCompany = new Dictionary<String, Object>();
		public static Dictionary<String, Object> userinitialproperty = new Dictionary<String, Object>();
		static ResXResourceWriter resWriter;
        static HttpClient httpclient;
        //Properties props=new Properties();
        public ApplicationProperties()
        {
            resWriter = new ResXResourceWriter("ClientAppProps.resx");
            httpclient = new HttpClient();
        }
        public static void Removepropertyfile(string CompanyNameToRemove)
        {
			string resxDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string targetFileName = $"{CompanyNameToRemove}.resx"; // Replace with your .resx file name
			string fullPath = Path.Combine(resxDirectory, targetFileName);
			if (File.Exists(fullPath))
			{
				try
				{
					File.Delete(fullPath);
					
				}
				catch (Exception ex)
				{

					LogManager.WriteLog("ClientAppProps.resx not found exception");
				}
			}
			else
			{
				Console.WriteLine("File not found.");
			}

		}
        public static Dictionary<string, Object> Userinitialproperty()
        {
            try
            {
                userinitialproperty.Clear();
                ResXResourceReader rsr = new ResXResourceReader(@".\ClientAppProps.resx");
                foreach (DictionaryEntry d in rsr)
                {
                    userinitialproperty.Add(d.Key.ToString(), d.Value.ToString());
                    //Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());
                }
	rsr.Close();

                // Properties someObject = properties.ToObject<Properties>();

                //IDictionary<string, object> objectBackToDictionary = someObject.AsDictionary();
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("ClientAppProps.resx not found exception");
                resWriter = new ResXResourceWriter("ClientAppProps.resx");
                writeUserInitialProperties();

            }

            return userinitialproperty ;
		}
        public static void updatePropertiesFile(string companyname)
        {
            try
            {
				string resxFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{companyname}.resx");

					using (ResXResourceWriter resWriter = new ResXResourceWriter(resxFilePath))
					{
						

						foreach (KeyValuePair<string, object> d in properties)
						{
							resWriter.AddResource(d.Key, d.Value);
						}

						resWriter.Generate(); // Ensure the data is flushed to file
                        resWriter.Close();
					}

					
				
				

			}
            catch (Exception e)
            {
                LogManager.HandleException(e);
            }
   

        }

		public static Dictionary<string, Object> getAllProperties(string? companyName)
		{
			try
			{
	
				string fileName = $"{companyName}.resx";
				string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

				if (File.Exists(filePath))
				{
					properties.Clear();
					ResXResourceReader rsr = new ResXResourceReader(filePath);
					foreach (DictionaryEntry d in rsr)
					{
						properties.Add(d.Key.ToString(), d.Value.ToString());
						//Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());
					}
				

					rsr.Close();
				

				}
             
		
			}
			catch (Exception ex)
			{
				
				LogManager.WriteLog($"not found. Creating new resource file. Error: {ex.Message}");

				

			}

			return properties;
		}

		public static Dictionary<string, Object> getAllProperties()
        {
            try
            {
				userinitialproperty.Clear();
                ResXResourceReader rsr = new ResXResourceReader(@".\ClientAppProps.resx");
                foreach (DictionaryEntry d in rsr)
                {
                    userinitialproperty.Add(d.Key.ToString(), d.Value.ToString());
                    //Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());
                }
                rsr.Close();

                // Properties someObject = properties.ToObject<Properties>();

                //IDictionary<string, object> objectBackToDictionary = someObject.AsDictionary();
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("ClientAppProps.resx not found exception");
                resWriter = new ResXResourceWriter("ClientAppProps.resx");
                writeUserInitialProperties();

            }

            return userinitialproperty;
        }
        public static void writeUserInitialProperties()
        {
			ApplicationProperties.userinitialproperty["prefix"] = "/api/tp/v1";
			ApplicationProperties.userinitialproperty["RememberMe"] = "False";
			ApplicationProperties.userinitialproperty["RememberUser"] = "";
			ApplicationProperties.userinitialproperty["RememberPass"] = "";
			ApplicationProperties.userinitialproperty["isFirstTimeLogin"] = "True";
			ApplicationProperties.userinitialproperty["idclientapp"] = "True";
			ApplicationProperties.userinitialproperty["snrich.dir"] = "C\\:\\\\salesNrich\\\\SNR\\\\";

			updatePropertiesFile();
		}


		public static void writeInitialProperties()
        {
			ApplicationProperties.properties["SchemeDiscountEnabled"] = "False";
			ApplicationProperties.properties["DistributedCodeCompany"]=" ";
            ApplicationProperties.properties["IsEnableDistributor"] = "False";
            ApplicationProperties.properties["AutoUpload"] = "";
            ApplicationProperties.properties["DistributedCode"] = "";
            ApplicationProperties.properties["AutoDownload"] = "";
            ApplicationProperties.properties["RememberMe"] = "False";
            ApplicationProperties.properties["RememberUser"] = "";
            ApplicationProperties.properties["RememberPass"] = "";
            ApplicationProperties.properties["isFirstTimeCompanyLogin"] = "True";
            ApplicationProperties.properties["round.off.ledger"] = "";
            ApplicationProperties.properties["isIgstEnabled"] = "False";
            ApplicationProperties.properties["tally.company"] = "";
            ApplicationProperties.properties["tally.ledger.parent"] = "Sundry Debtors";
            ApplicationProperties.properties["sales.ledger"] = "";
             ApplicationProperties.properties["gstParentGroup"] = "";
            ApplicationProperties.properties["salesLedgerParentGroup"] = "";
            ApplicationProperties.properties["tally.gst"] = "";
            ApplicationProperties.properties["tally.taxs"] = "";
            ApplicationProperties.properties["isFirstTimeUpload"] = "True";
            ApplicationProperties.properties["download.by.employees"] = "All,";
            ApplicationProperties.properties["download.by.employee.voucher"] = "";
            ApplicationProperties.properties["item.remarks.enabled"] = "";
            ApplicationProperties.properties["show.provisional.no"] = "";
            ApplicationProperties.properties["enable.receipt.voucherType"] = "";
            ApplicationProperties.properties["enable.cash.only.ledger.entry"] = "";
            ApplicationProperties.properties["isCashonlyLedgerEnabled"] = "";
            ApplicationProperties.properties["gst.ledger.calculation"] = "";
            ApplicationProperties.properties["reduce.tax"] = "";
            ApplicationProperties.properties["enable.cost.centre"] = "";
            ApplicationProperties.properties["enable.cost.centre.cash.receipts"] = "False";
            ApplicationProperties.properties["enable.date"] = "";
            ApplicationProperties.properties["enable.Selecteddate"] = "";
            ApplicationProperties.properties["payment.mode.terms"] = "CD Bills";

            ApplicationProperties.properties["download.disable.chequeue.entry"]="";
            ApplicationProperties.properties["salesreturnvouchertype"]="";
            ApplicationProperties.properties["salesReturnLedgerName"]="";

            ApplicationProperties.properties["pdc.vouchertype"]="";

            ApplicationProperties.properties["Isoptimized"] = "True";
            ApplicationProperties.properties["batch.fixed"] = "";
            ApplicationProperties.properties["godown.fixed"] = "";
            ApplicationProperties.properties["is.optional.receipt"] = "True";
            ApplicationProperties.properties["product.rate.including.tax"] = "";
            ApplicationProperties.properties["tally.company.prefix"] = "";
            ApplicationProperties.properties["enable.receipt.employeewise.ledger"] = "";
            ApplicationProperties.properties["actual.billed.status"] = "";
            ApplicationProperties.properties["order.employee.name"] = "True";

            ApplicationProperties.properties["actual.billed.status"] = "False";
            ApplicationProperties.properties["batch.fixed"] = "False";
            ApplicationProperties.properties["company.state"] = "Kerala";
            ApplicationProperties.properties["user.stockLocation"] = "True";
            ApplicationProperties.properties["transaction.type"] = "";
            ApplicationProperties.properties["idclientapp"] = "True";
            ApplicationProperties.properties["tally.productSGST"] = "State Tax";
            ApplicationProperties.properties["tally.igst.index"] = "";
            ApplicationProperties.properties["snrich.dir"] = "C\\:\\\\salesNrich\\\\SNR\\\\";
            ApplicationProperties.properties["tally.gst.index"] = "";
            ApplicationProperties.properties["tally.igst.index"] = "";
            ApplicationProperties.properties["stocklocationMethode"] = "1";
            ApplicationProperties.properties["netStockAvilable"] = "False";
            ApplicationProperties.properties["receipt.voucher.type"] = "";
            ApplicationProperties.properties["godown.in.sales.receipt"] = "False";
            ApplicationProperties.properties["is.optional.salesOrder"] = "False";
            ApplicationProperties.properties["kfc.ledger.name"] = "";
            ApplicationProperties.properties["gst.ledger.calculation"] = "False";
            ApplicationProperties.properties["is.multi.tax"] = "false";
            ApplicationProperties.properties["sales.order.activity.remarks"] = "True";
            ApplicationProperties.properties["salessorderDate"] = "";
            ApplicationProperties.properties["receiptdate"] = "";
            ApplicationProperties.properties["service.full.url"] = "";
            ApplicationProperties.properties["godownName"] = "";
            ApplicationProperties.properties["batchName"] = "";
            ApplicationProperties.properties["tax.required"] = "True";
            ApplicationProperties.properties["invoice.number.as.reference"] = "True";
            ApplicationProperties.properties["salesorder.number.as.reference"] = "False";
            ApplicationProperties.properties["tally.productSGST"] = "State Tax";
            ApplicationProperties.properties["tally.productCGST"] = "Central Tax";
            ApplicationProperties.properties["tally.productIGST"] = "Integrated Tax";
            ApplicationProperties.properties["tally.productCESS"] = "Cess";
            ApplicationProperties.properties["Cess.ledger.name"] = "";
            ApplicationProperties.properties["case.value"] = "False";
            ApplicationProperties.properties["IsCessEnabled"] = "False";
            ApplicationProperties.properties["receiptVoucherType"] = "Receipt";
            ApplicationProperties.properties["salesVoucherType"] = "Sales";
            ApplicationProperties.properties["isRoundOffEnabled"] = "";
            ApplicationProperties.properties["receipt.voucher.bank.name"] = "";
            ApplicationProperties.properties["isDownloadByEmployeesEnabled"] = "False";
            ApplicationProperties.properties["receipt.voucher.type.bank"] = "";
            ApplicationProperties.properties["receipt.voucher.type.cash"] = "";

            ApplicationProperties.properties["DocumentNoAsVoucher"] = "False";
            ApplicationProperties.properties["AutoDownload"] ="Fasle";
            ApplicationProperties.properties["AutoUpload"] = "False";
            ApplicationProperties.properties["AutoDownloadTimePeriod"] = "";
            ApplicationProperties.properties["AutoUploadTimePeriod"] = "";
            ApplicationProperties.properties["prefix"] = "/api/tp/v1";
            ApplicationProperties.properties["tally.odbcdsn"]="";

            //for vansale vehicle details
            ApplicationProperties.properties["downloadVehicleDetails"]="False";
            ApplicationProperties.properties["enable.discount.ledger"]="False";
            ApplicationProperties.properties["discount.ledger"]="";
            ApplicationProperties.properties["PaymentModeRemarks"]="";

            ApplicationProperties.properties["salesreturnvouchertype"]="";
            ApplicationProperties.properties["salesReturnLedgerName"]="";
			ApplicationProperties.properties["LedgerDate"] = "";


			ApplicationProperties.updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);
        }

		public static void setProperties(Dictionary<string, string> props)
		{

            if(StringUtilsCustom.TALLY_COMPANY!=null)
            {
                getAllProperties(StringUtilsCustom.TALLY_COMPANY);
				foreach (KeyValuePair<string, string> d in props)
				{
					if (properties.ContainsKey(d.Key))
						properties[d.Key] = d.Value.ToString();
					else
						properties.Add(d.Key, d.Value.ToString());

				}

                updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);
			}
            else
            {
				properties = getAllProperties();
				foreach (KeyValuePair<string, string> d in props)
				{
					if (userinitialproperty.ContainsKey(d.Key))
						userinitialproperty[d.Key] = d.Value.ToString();
					else
						userinitialproperty.Add(d.Key, d.Value.ToString());

				}
                updatePropertiesFile();
			
			}
		
			
		


		}
		public static void setPropertieswithdataFromServer(Dictionary<string, string> props)
        {

            getAllProperties(StringUtilsCustom.TALLY_COMPANY);
            foreach (KeyValuePair<string, string> d in props)
            {
                if (properties.ContainsKey(d.Key))
                    properties[d.Key] = d.Value.ToString();
              
               

            }
            ApplicationProperties.updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);   
            //resWriter = new ResXResourceWriter("ClientAppProps.resx");
            //foreach (KeyValuePair<string, Object> d in properties)
            //{
            //    resWriter.AddResource(d.Key, d.Value);

            //}
            //resWriter.Close();


        }
        public static void createPropertyFile(string companyName)
        {
			try
			{
				string resxFilePath = $"{companyName}.resx";

				// Check if the resx file exists
				if (File.Exists(resxFilePath))
				{
					// Read existing properties from the .resx file
					using (ResXResourceReader resReader = new ResXResourceReader(resxFilePath))
					{
						resReader.UseResXDataNodes = true;

						foreach (DictionaryEntry entry in resReader)
						{
							string key = entry.Key.ToString();
							ResXDataNode node = (ResXDataNode)entry.Value;
							object value = node.GetValue((ITypeResolutionService)null);

							if (properties.ContainsKey(key))
								properties[key] = value;
							else
								properties.Add(key, value);
						}
					}
				}
				else
				{
					writeInitialProperties();
					// File doesn't exist, create a new one from current properties
					

				}
			}
			catch (Exception e)
			{
				LogManager.HandleException(e);
			}

		}
		
        public static void updatePropertiesFile()
        {
            try
            {
                //resWriter.Close();
                resWriter = new ResXResourceWriter("ClientAppProps.resx");
                foreach (KeyValuePair<string, Object> d in userinitialproperty)
                {
                    resWriter.AddResource(d.Key, d.Value);

                }
                
                
                
                
                resWriter.Close();

            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
            }

        }
        

        //Getting Properties from Server
        public static async void getPropertyFromServer()
        {
			try
            {

			
				string getPropertiesfromServer = ApiConstants.CLIENTAPP_PROPERTIES_FROM_SERVER + "?tallyCompanyName=" + StringUtilsCustom.TALLY_COMPANY; 
                LogManager.WriteLog("getting  ClientApp Prperty from Server:...."+getPropertiesfromServer);
                LogManager.WriteLog(getPropertiesfromServer.ToString());
                httpclient = RestClientUtil.getClient();

                var responseTask1 = httpclient.GetAsync(getPropertiesfromServer);
                responseTask1.Wait();
                HttpResponseMessage Res1 = responseTask1.Result;

                LogManager.WriteResponseLog(Res1);


                if (Res1.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("getting  ClientApp Prperty from Server ids Success...");
                    if (Res1.Content!=null)
					{
					
						var jsonString = await Res1.Content.ReadAsStringAsync();
                        LogManager.WriteLog(jsonString.ToString());
						var jsonElements = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
						properties = jsonElements.ToDictionary(
				   kvp => kvp.Key,
				   kvp => ConvertJsonElement(kvp.Value)
			   );
						var props = ConvertToDictionaryString(properties);
                        if(props.Count!=0)
                        {
							MessageBoxIcon icon = MessageBoxIcon.Warning;
							var res = MessageBox.Show(
								"Do you want to update the Property file with existing properties?",
								"SalesNrich",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Warning,
								MessageBoxDefaultButton.Button1
							);
							if (res == DialogResult.Yes)
							{
								ApplicationProperties.setPropertieswithdataFromServer(props);
							}
							else if (res == DialogResult.No)
							{

							}
						}
                        else
                        {
                            
                        }
                       

                    }
                }
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
            }
           
        }
        public static Dictionary<string, string> ConvertToDictionaryString(Dictionary<string, object> originalDictionary)
        {
            var stringDictionary = new Dictionary<string, string>();

            foreach (var kvp in originalDictionary)
            {
                stringDictionary[kvp.Key] = kvp.Value?.ToString(); // Convert the object to string (handling nulls)
            }

            return stringDictionary;
        }

        private static object ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => element.ToString(), // Convert JSON objects to their string representation
                JsonValueKind.Array => element.ToString(),  // Convert JSON arrays to their string representation
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(), // Handle integer and floating-point numbers
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.ToString() // Fallback
            };
        }
    }
}
