using dtos;
using Newtonsoft.Json;
using OfficeOpenXml;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
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
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    public partial class TallyProperties2_UC : UserControl
    {
        public TallyConfigForm parentform { get; set; }
        TallyService tallyService;
        //Dictionary<string, Object> props = new Dictionary<string, Object>();
        HttpClient httpClient;
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;

        public TallyProperties2_UC()
        {
            InitializeComponent();
            tallyService = new TallyService();
            LoadDefaultValues();
            httpClient=new HttpClient();

        }

        private void LoadDefaultValues()
        {
            loadReceiptVoucherTypes();
            loadBankNames();
            loadRoundOffLedgers();
            bindDatasFromProperties();
            loadSalesreturnVoucherTypes();
            LoadSalesReturnLedgerNames();
        }

        private void bindDatasFromProperties()
        {
            try
            {
                var bankname = ApplicationProperties.properties["receipt.voucher.bank.name"];
                if (!bankname.Equals(""))
                {
                    BankNamesSelect.SelectedItem = bankname;
                }
                var cashVTYpe = ApplicationProperties.properties["receipt.voucher.type.cash"];
                if (!cashVTYpe.Equals(""))
                {
                    CashReceiptSelect.SelectedItem = cashVTYpe;
                }

                var bankVoucherType = ApplicationProperties.properties["receipt.voucher.type.bank"];
                if (!bankVoucherType.Equals(""))
                {
                    BankReceiptVoucherTypeSelect.SelectedItem = bankVoucherType;
                }
                var pdc = ApplicationProperties.properties["pdc.vouchertype"];
                if (!pdc.Equals(""))
                {
                    PostDatedVoucherSelect.SelectedItem = pdc;
                }
                var isRoundOffEnabled = ApplicationProperties.properties["isRoundOffEnabled"].ToString();
                chk_RoundOff.Checked = false;
                if (isRoundOffEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    var roundOff = ApplicationProperties.properties["round.off.ledger"];
                    if (!roundOff.Equals(""))
                    {
                        RoundOffLedgerSelect.SelectedItem = roundOff;
                        chk_RoundOff.Checked = true;
                        //lbl_EnabeIgst.Enabled = true;
                        RoundOffLedgerSelect.Enabled = true;
                    }
                }

                //Binding default ledger
                var isDiscountLedgerEnabled = ApplicationProperties.properties["enable.discount.ledger"].ToString();
                Chk_Discount.Checked = false;
                if (isDiscountLedgerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    var dicountLedger = ApplicationProperties.properties["discount.ledger"];
                    if (!dicountLedger.Equals(""))
                    {
                        DiscountLedgerSelect.SelectedItem = dicountLedger;
                        Chk_Discount.Checked = true;
                        //lbl_EnabeIgst.Enabled = true;
                        DiscountLedgerSelect.Enabled = true;
                    }
                }
              
              

                var salesReturnLedgerName = ApplicationProperties.properties["salesReturnLedgerName"].ToString();
                if (!salesReturnLedgerName.Equals(""))
                {
                    sales_return_name_combo.SelectedItem = salesReturnLedgerName;
                }
                var salesReturnVoucherType = ApplicationProperties.properties["salesreturnvouchertype"].ToString();
                if (!salesReturnVoucherType.Equals(""))
                {
                    SalesReturnVoucherTypeSelect.SelectedItem = salesReturnLedgerName;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("exception occured while Binding datas from properties file in TallyProperties2 .");
                LogManager.HandleException(ex);
            }
        }

        private async  void loadBankNames()
        {
            var Parent = "Bank Accounts";
            String[] row = await tallyService.getAllLedgersByParent(Parent);
            //var Parent = "Bank Accounts";
            string[] groups = await tallyService.getAllGroupsByParent(Parent);
            foreach (string group in groups)
            {
                var res = await tallyService.getAllLedgersByParent(group);
                //row.Append(res);
                row = row.Concat(res).ToArray();
            }

            BankNamesSelect.Items.Clear();
            BankNamesSelect.Items.Add("Cash");
            BankNamesSelect.Items.AddRange(row);
            if (row.Length > 0)
            {
                BankNamesSelect.SelectedItem = row[0];
            }
            else
            {
                BankNamesSelect.SelectedItem="Cash";
            }
        }
        private async void LoadSalesReturnLedgerNames()
        {
            var vtypes = await  tallyService.getAllSalesReturnLedgerNames();
            List<String> types = new List<String>();
            types.AddRange(vtypes);

            String[] row = types.ToArray();
            if (row.Length > 0)
            {
                sales_return_name_combo.Items.Clear();
                sales_return_name_combo.Items.AddRange(row);
                //binding default values
                sales_return_name_combo.SelectedItem=row[0];
                var returnLedgerName = ApplicationProperties.properties["salesReturnLedgerName"].ToString();
                if (returnLedgerName!=null)
                {
                    sales_return_name_combo.SelectedItem = returnLedgerName;
                }
                else
                {
                    string salesReturnLedgerName = "";
                    bool isSalesReturnLedgerNameExist = Array.Exists(row, (element) =>
                    {

                        if (element.ToLower().Equals("sales return"))
                        {
                            salesReturnLedgerName = element;
                            return true;
                        };
                        return false;
                    });
                    if (isSalesReturnLedgerNameExist)
                    {
                        sales_return_name_combo.SelectedItem = salesReturnLedgerName;

                    }

                }


            }
        }
        private async void loadSalesreturnVoucherTypes()
        {
            var vtypes = await tallyService.getAllSalesReturnVoucherTypes();
            List<String> types = new List<String>();
            types.AddRange(vtypes);

            String[] row = types.ToArray();
            if (row.Length > 0)
            {
                SalesReturnVoucherTypeSelect.Items.Clear();
                SalesReturnVoucherTypeSelect.Items.AddRange(row);
                //binding default values
                SalesReturnVoucherTypeSelect.SelectedItem=row[0];
                string salesreturnvoucher = "";

                var returnVoucher = ApplicationProperties.properties["salesreturnvouchertype"].ToString();
                if (returnVoucher != null)
                {
                    SalesReturnVoucherTypeSelect.SelectedItem=returnVoucher;
                }
                else
                {
                    bool isSalesReturnexist = Array.Exists(row, (element) =>
                    {

                        if (element.ToLower().Equals("credit note"))
                        {
                            salesreturnvoucher = element;
                            return true;
                        };
                        return false;
                    });
                    if (isSalesReturnexist)
                    {
                        SalesReturnVoucherTypeSelect.SelectedItem = salesreturnvoucher;

                    }

                }


            }
        }
        private async void loadReceiptVoucherTypes()
        {

            var vtypes = await tallyService.getAllCashReceiptVoucherTypes();
            List<String> types = new List<String>();
            types.AddRange(vtypes);
            types.Add("Journal");
            String[] row = types.ToArray();

            if (row.Length > 0)
            {
                CashReceiptSelect.Items.Clear();
                CashReceiptSelect.Items.AddRange(row);

                BankReceiptVoucherTypeSelect.Items.AddRange(row);

                PostDatedVoucherSelect.Items.AddRange(row);

                //binding default values

                CashReceiptSelect.SelectedItem = row[0];
                BankReceiptVoucherTypeSelect.SelectedItem = row[0];
                PostDatedVoucherSelect.SelectedItem = row[0];
                String Receipt = "";
                String CashReceipt = "";
                String BankReceipt = "";
                //  bool isCashReceiptExist = Array.Exists(row, element => element.ToLower().Equals("cash receipt"));

                bool isReceiptExist = Array.Exists(row, (element) =>
                {

                    if (element.ToLower().Equals("receipt"))
                    {
                        Receipt = element;
                        return true;
                    };
                    return false;
                });

                bool isBankReceiptExist = Array.Exists(row, (element) =>
                {

                    if (element.ToLower().Equals("bank receipt"))
                    {
                        BankReceipt = element;
                        return true;
                    };
                    return false;
                });

                bool isCashReceiptExist = Array.Exists(row, (element) =>
                {

                    if (element.ToLower().Equals("cash receipt"))
                    {
                        CashReceipt = element;
                        return true;
                    };
                    return false;
                });


                //    bool isBankReceiptExist = Array.Exists(row, element => element.ToLower().Equals("bank receipt"));


                if (isCashReceiptExist)
                {
                    CashReceiptSelect.SelectedItem = CashReceipt;
                }
                if (isReceiptExist)
                {
                    CashReceiptSelect.SelectedItem = Receipt;
                    BankReceiptVoucherTypeSelect.SelectedItem = Receipt;
                    PostDatedVoucherSelect.SelectedItem = Receipt;
                }
                if (isBankReceiptExist)
                {
                    BankReceiptVoucherTypeSelect.SelectedItem = BankReceipt;
                    PostDatedVoucherSelect.SelectedItem = BankReceipt;
                }




            }
        }

        private async void loadRoundOffLedgers()
        {
            String[] IndirectIncomesList =await  tallyService.getAllIndirectIncomes();
            String[] IndirectExpencesList = await tallyService.getAllIndirectExpences();
            String[] RoundOffLedgerSelectList = new String[IndirectIncomesList.Length + IndirectExpencesList.Length];
            IndirectIncomesList.CopyTo(RoundOffLedgerSelectList, 0);
            IndirectExpencesList.CopyTo(RoundOffLedgerSelectList, IndirectIncomesList.Length);
            RoundOffLedgerSelect.Items.Clear();
            RoundOffLedgerSelect.Items.AddRange(RoundOffLedgerSelectList);

            //binding default values
            if (RoundOffLedgerSelectList.Length > 0)
            {
                // Check if the list contains "round off" without case sensitivity
                string roundOffValue = RoundOffLedgerSelectList.FirstOrDefault(s => s.Equals("round off", StringComparison.OrdinalIgnoreCase));

                if (roundOffValue != null)
                {
                    RoundOffLedgerSelect.SelectedItem = roundOffValue;
                }
                else
                {
                    RoundOffLedgerSelect.SelectedIndex= 0;
                }


            }
            //binding discount ledger
            string[] groups = await tallyService.getAllGroupsByParent("Indirect Expenses");
            foreach (string group in groups)
            {
                var res = await tallyService.getAllLedgersByParent(group);
                //row.Append(res);
                IndirectExpencesList = IndirectExpencesList.Concat(res).ToArray();
            }
            DiscountLedgerSelect.Items.Clear();
            DiscountLedgerSelect.Items.AddRange(IndirectExpencesList);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {

                printDatasToResourceFile();
                sendPropertiesToAdmin();

                
                sendPropertiestoServer();
                ApplicationProperties.updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);
			var	resWriter = new ResXResourceWriter("ClientAppProps1.resx");
				var res = MessageBox.Show(
								"Do you want to Add More Companies..?",
								"SalesNrich",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Warning,
								MessageBoxDefaultButton.Button1
							);
				if (res == DialogResult.Yes)
				{
					TallyConfigForm tallyConfigForm = new TallyConfigForm();
					tallyConfigForm.Show();
					this.Hide();
				}
				else  
				{
					MainForm mainform = new MainForm();
					mainform.Show();
					this.Hide();
					parentform.Hide();
				}
				
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            finally
            {
                parentform.Cursor = Cursors.Default;
            }
        }

        private void sendPropertiesToAdmin()
        {
            generateXmlFile();

        }

        private void generateXmlFile()
        {
            string filename = "PropertyFile-"+DateTime.Now.ToString("dd-MMM-yyyy-HH-mm")+".xlsx";

            GenerateExcelFile(ApplicationProperties.properties, filename);
            sendEmail(filename);

        }

        public static void GenerateExcelFile(Dictionary<string, object> data, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Write headers
                worksheet.Cells[1, 1].Value = "Property";
                worksheet.Cells[1, 2].Value = "Value";
                int row = 2;
                foreach (var key in data.Keys)
                {
                    worksheet.Cells[row, 1].Value = key;
                    row++;
                }

                // Write data
                int column = 2;
                row = 2;
                foreach (var value in data.Values)
                {
                    worksheet.Cells[row, column].Value = value;
                    row++;
                }

                // Save the file
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private void sendEmail(string filename)
        {
            Thread t1 = new Thread(delegate ()
            {

                MailRequest mrequest = new MailRequest();
                string body = " <div style='display: flex; flex-direction: column; align-items: center;'>        <div style='display: flex; flex-direction: row;'>     " +
                "  <img src=\"http://salesnrich.com/resources/web-assets/img/logo1.png\" alt=\"SalesNrich\">        </div>" +
                " </div> <font style=' font-family: math;'>Client App Property Change Report</font>" +
                "<br> " +
                "  <div><b>Company : </b>"+ApplicationProperties.properties.GetValueOrDefault("tally.company", "") +"<br>  " +
                "     <b>Last Updated Date : </b>"+DateTime.Now+"<br>" +
                "         <b>Last Updated User : </b>"+RestClientUtil.getLoggedUser()+" <br>   </div> </div>";
                mrequest.Body = body;
                mrequest.Subject = "SalesNrich ClientApp Properties Update Report ";
                mrequest.ToEmail ="yadhu.aitrich@gmail.com";
                mrequest.Attachment=filename;
                MailService mailService = new MailService();
                mailService.SendEmailAsync(mrequest);

            });
            t1.Start();
        }
        private void sendPropertiestoServer()
        {
            try
            {
                 ApplicationProperties.getAllProperties();
                string ClientappPropertiestoServer = ApiConstants.CLIENTAPP_PROPERTIES_TO_SERVER;
                LogManager.WriteLog("updating  ClietappProperty Api:...."+ClientappPropertiestoServer);
                LogManager.WriteLog(ClientappPropertiestoServer.ToString());
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(props);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                var responseTask1 = httpClient.PostAsync(ClientappPropertiestoServer, inputContent);
                responseTask1.Wait();
                HttpResponseMessage Res = responseTask1.Result;

                LogManager.WriteResponseLog(Res);


                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("Posting ClientApp Properties to Server Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("Posting ClientApp Properties to Server Failed");
                    throw new ServiceException("Posting ClientApp Properties to Server Failed");
                }


            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
        private void printDatasToResourceFile()
        {
            ApplicationProperties.properties["receipt.voucher.bank.name"] = BankNamesSelect.SelectedItem.ToString();
            ApplicationProperties.properties["receipt.voucher.type.bank"] = BankReceiptVoucherTypeSelect.SelectedItem.ToString();
            ApplicationProperties.properties["receipt.voucher.type.cash"] = CashReceiptSelect.SelectedItem.ToString();
            ApplicationProperties.properties["isRoundOffEnabled"] = chk_RoundOff.Checked;
            if (chk_RoundOff.Checked)
            {
                if (RoundOffLedgerSelect.SelectedItem != null)
                {
                    ApplicationProperties.properties["round.off.ledger"] = RoundOffLedgerSelect.SelectedItem.ToString();
                }


                else
                {
                    chk_RoundOff.Checked = false;
                    ApplicationProperties.properties["round.off.ledger"] = "";
                }
            }
                //ApplicationProperties.properties["payment.mode.terms"] = txt_PaymentModeTerms.Text;
                ApplicationProperties.properties["pdc.vouchertype"] = PostDatedVoucherSelect.SelectedItem.ToString();
                ApplicationProperties.properties["isFirstTimeLogin"] = false;

                ApplicationProperties.properties["enable.discount.ledger"] = Chk_Discount.Checked;

                if (Chk_Discount.Checked)
                {
                    if (DiscountLedgerSelect.SelectedItem != null)
                    {
                        ApplicationProperties.properties["discount.ledger"] = DiscountLedgerSelect.SelectedItem.ToString();
                    }
                    else
                    {
                        Chk_Discount.Checked = false;
                        ApplicationProperties.properties["discount.ledger"] = "";
                    }

                }

                if (SalesReturnVoucherTypeSelect.SelectedItem != null && !string.IsNullOrWhiteSpace(SalesReturnVoucherTypeSelect.SelectedItem.ToString()))
                {
                    ApplicationProperties.properties["salesreturnvouchertype"] = SalesReturnVoucherTypeSelect.SelectedItem.ToString();

                    if (sales_return_name_combo.SelectedItem != null && !string.IsNullOrWhiteSpace(sales_return_name_combo.SelectedItem.ToString()))
                    {
                        ApplicationProperties.properties["salesReturnLedgerName"] = sales_return_name_combo.SelectedItem.ToString();
                    }

                }


                ApplicationProperties.updatePropertiesFile();
            }
        

        private void btn_Back_Click(object sender, EventArgs e)
        {
            //TallyProperties1 tallyProperties1 = new TallyProperties1();
            //tallyProperties1.Show();
            //this.Hide();
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                printDatasToResourceFile();
                TallyProperties1_UC tallyProperties1_UC = new TallyProperties1_UC();
                tallyProperties1_UC.parentform = parentform;
                parentform.AddUserControl(tallyProperties1_UC);
                this.Hide();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
            }
            finally
            {
                parentform.Cursor = Cursors.Default;
            }
        }

        private void chk_RoundOff_CheckedChanged(object sender, EventArgs e)
        {

            lbl_Roundoff.Enabled = chk_RoundOff.Checked;
            RoundOffLedgerSelect.Enabled = chk_RoundOff.Checked;

        }

        private void btn_Advanced_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            //AdvancedProperties advancedProperties = new AdvancedProperties();
            //advancedProperties.Show();
            //this.Hide();
            try
            {
                printDatasToResourceFile();
                AdvancedProperties1_UC nextScreen = new();

                nextScreen.parentform = parentform;
                parentform.AddUserControl(nextScreen);


                this.Hide();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            finally
            {
                parentform.Cursor = Cursors.Default;
            }

        }

        private void Chk_Discount_CheckedChanged(object sender, EventArgs e)
        {
            Lbl_Discount.Enabled = Chk_Discount.Checked;
            DiscountLedgerSelect.Enabled = Chk_Discount.Checked;
        }

       
    }
}
