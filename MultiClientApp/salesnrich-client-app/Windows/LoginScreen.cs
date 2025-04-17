using Microsoft.VisualBasic;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Security;
using SNR_ClientApp.Services;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static SNR_ClientApp.Services.BackgroundTaskManagerService;
using SNR_ClientApp.Config;
using System.Net.Http;

using System.Text.Json;
using dtos;
using Newtonsoft.Json;
namespace SNR_ClientApp.Windows
{
    public partial class LoginScreen : Form
    {
        static HttpClient httpclient;
        AuthenticationService authenticationService = new AuthenticationService();
        SyncOperationService syncOperationService = new SyncOperationService();
        UploadService uploadService;
        UC_Logger uC_Logger;
        CompanyService companyService;
        string autodownloadenable;
        
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;



        public LoginScreen ()
        {

            httpclient = new HttpClient();
            InitializeComponent();

            companyService = new CompanyService();

            StringUtilsCustom.TALLY_COMPANY = props.GetValueOrDefault("tally.company").ToString();
            loadRememberCredentials();
            uC_Logger = new UC_Logger();
            uploadService=new UploadService();



        }
        private void ClearLogMessage ()
        {
            uC_Logger.ClearLogArea();
        }
        private void appendLogMessage (string v)
        {
            uC_Logger.AppendLogMsg(v);
        }
        private DateTime ConvertDate (string date)
        {
            string formattedDate = "";
            string[] splitDates = date.Split('T');
            DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return dateTime;
        }
        //private async Task UploadSalesFromTally()
        //{

        //    try
        //    {
        //        string saledate = ApplicationProperties.properties["salessorderDate"].ToString();
        //        ClearLogMessage();
        //        appendLogMessage("Verifying tally and compnay.");
        //        if (!await checkTallyCompanyIsOpenedAsync())
        //        {
        //            appendLogMessage("Verifying tally and compnay failed.\n Please ensure company is open and Active in tally ");
        //            MessageBox.Show("Please ensure company is open and Active in tally ");
        //        }
        //        else
        //        {
        //            appendLogMessage("Tally and company verified.");
        //            if (saledate!=null)
        //            {
        //                DateTime converteddate = ConvertDate(saledate);

        //                string formatedDate = converteddate.ToString("d-MMM-yy");
        //                await uploadService.getFromTallyAndUploadAsync(formatedDate);

        //            }

        //            else
        //            {
        //                await uploadService.getFromTallyAndUploadAsync(DateTime.Now.ToString("d-MMM-yy"));

        //            }



        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.HandleException(ex, "exception occured while uploading datas to server ");
        //        appendLogMessage("Upload data to server failed ...");
        //    }
        //    finally
        //    {
        //        //MessageBox.Show(" Master Data Upload Completed ");
        //    }
        //}

        private void loadRememberCredentials ()
        {
            if (ApplicationProperties.properties["RememberMe"].ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                username.Text= ApplicationProperties.properties["RememberUser"].ToString();
                password.Text= ApplicationProperties.properties["RememberPass"].ToString();
                rememberMeChkBox.Checked = true;
            }
        }
        private async Task<bool> checkTallyCompanyIsOpenedAsync ()
        {
            // appendLogMessage("Tally and company verified.");

            try
            {
                string hostname = props.GetValueOrDefault("tally.hostname").ToString();
                if (!String.Equals(hostname, "localhost", StringComparison.OrdinalIgnoreCase))
                {
                    List<CompanyDTO> companies = await companyService.GetCompanies();
                    if (companies != null)
                    {
                        if (!checkCompanyExist(companies))
                        {
                            MessageBox.Show("Please ensure company is open in tally ");
                            //  throw new ServiceException("Please ensure company is open in tally");
                            return false;
                        }
                        else
                        {
                            // appendLogMessage("Tally and company verified.");
                            return true;

                        }
                    }
                    // appendLogMessage("Tally and company verification failed.");
                    return false;

                }
                else
                {

                    string activeCompany = await companyService.getCurrentActiveCompanyAsync();
                    if (activeCompany != null)
                    {
                        if (activeCompany.Equals(StringUtilsCustom.TALLY_COMPANY))
                        {
                            return true;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }


            }
            catch (Exception ex)
            {
                appendLogMessage("please ensure tally and company open");
                return false;
            }
            return false;
        }
        private bool checkCompanyExist (List<CompanyDTO> companies)
        {
            //test code : assuming that first comapny in the list willbe the active company
            //var selectedCompany= companies.First();
            //if (selectedCompany.companyName.Equals(StringUtilsCustom.TALLY_COMPANY))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            foreach (CompanyDTO dto in companies)
            {
                if (dto.companyName.Equals(StringUtilsCustom.TALLY_COMPANY))
                {
                    return true;
                }
            }
            return false;
        }
        private void UserControl1_Load (object sender, EventArgs e)
        {
            panel1.Size = new Size(panel1.Size.Width, 40);
        }

        int panel1_y = 40; int waiter = 0;

        private void timer1_Tick (object sender, EventArgs e)
        {
            waiter++;
            if (waiter > 200)
            {
                label1.Hide();
                panel1_y += 6;
                panel1.Size = new Size(panel1.Size.Width, panel1_y);
                if (panel1_y > 341)
                {
                    panel2.Hide();
                    SnrHeadding.Visible = true;
                    close_btn.Visible=true;
                    timer1.Enabled = false;
                    username.Focus();
                }
            }
        }



        private void button1_Click (object sender, EventArgs e)
        {

            try
            {
                if (ValidateChildren(ValidationConstraints.Enabled))
                {
                    button1.Enabled = false;
                    button1.Cursor = Cursors.WaitCursor;
                    LoginDto loginDto = new LoginDto();
                    loginDto.username = username.Text;
                    loginDto.password = password.Text;
                    loginDto.rememberMe = rememberMeChkBox.Checked;


                    LogManager.WriteLog("Login into SalesNrich ...");
                    var res = authenticationService.authenticateAsync(loginDto);

                    if (res != null && res.IsSuccessStatusCode)
                    {


                        var isFirstTimeLogin = ApplicationProperties.properties.GetValueOrDefault("isFirstTimeLogin").ToString();
                        if (isFirstTimeLogin.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            authenticationService.setDeviceKey();
                        }
                        else
                        {
                            try
                            {
                                TallyCommunicator tallyCommunicator = new TallyCommunicator();
                              // tallyCommunicator.GetConnection();
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show("Please Ensure Tally Is Open");
                                return;
                                //throw ex;
                            }
                        }
                        bool isAuthenticated = authenticationService.validateApplication();
                        //need to cooment this code 
                        // isAuthenticated = true;
                        if (isAuthenticated)
                        {
                            //UploadSalesFromTally();
                            bool cond = getsyncOperation();
                            if (!cond)
                            {
                                MessageBox.Show("Please configure settings");
                                return;
                            }





                            if (rememberMeChkBox.Checked)
                            {
                                ApplicationProperties.properties["RememberMe"] = "True";
                                ApplicationProperties.properties["RememberUser"] = loginDto.username;
                                ApplicationProperties.properties["RememberPass"] = loginDto.password;
                                ApplicationProperties.updatePropertiesFile();
                            }
                            else
                            {
                                ApplicationProperties.properties["RememberMe"] = "False";
                                ApplicationProperties.properties["RememberUser"] = "";
                                ApplicationProperties.properties["RememberPass"] = "";
                                ApplicationProperties.updatePropertiesFile();
                            }



                            if (isFirstTimeLogin.Equals("true", StringComparison.OrdinalIgnoreCase))

                            {


                                ApplicationProperties.getPropertyFromServer();
                                // Check Whether Company is Distributed enabled or not

                                ApplicationProperties.properties["isFirstTimeLogin"] = "False";
								if (rememberMeChkBox.Checked)
								{
									ApplicationProperties.properties["RememberMe"] = "True";
									ApplicationProperties.properties["RememberUser"] = loginDto.username;
									ApplicationProperties.properties["RememberPass"] = loginDto.password;
									ApplicationProperties.updatePropertiesFile();
								}
								ApplicationProperties.updatePropertiesFile();

                                //TallyConnect tallyConnect = new TallyConnect();
                                //tallyConnect.Show();
                                TallyConfigForm tallyConfigForm = new TallyConfigForm();
                                tallyConfigForm.Show();
                                this.Hide();
                            }
                            else
                            {


                             
                                MainForm mainform = new MainForm();
                                mainform.Show();
                                this.Hide();
                            }
							checkdistributedenabled();


							ApplicationProperties.updatePropertiesFile();


						}
                        else
                        {
                            //ErrorLabel.Text = "Validating Application Failed..";
                            //ErrorLabel.Visible=true;
                            MessageBox.Show("Validating Application Failed..");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            finally
            {
                button1.Enabled = true;
                button1.Cursor = Cursors.Default;
            }
        }
        //Code to check whether the company is distributer enabled or not... if yes get distributor code of the company
        public static void checkdistributedenabled ()
        {
            try
            {
                string isenable = ApiConstants.CHECK_DISTRIBUTED_ENABLED;
                LogManager.WriteLog("Checking whether company is distributed enabled or not."+isenable);
                LogManager.WriteLog(isenable.ToString());
                httpclient = RestClientUtil.getClient();

                var responseTask1 = httpclient.GetAsync(isenable);
                responseTask1.Wait();
                HttpResponseMessage Res1 = responseTask1.Result;

                LogManager.WriteResponseLog(Res1);
                if (Res1.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("checking distributed enabling checking from Server is Success...");
                    var response = Res1.Content.ReadAsStringAsync().Result;
                    if (!(string.IsNullOrEmpty(response)))
                    {
                        // var distributedChecking = JsonConvert.DeserializeObject<List<object>>(response);
                        var distributedChecking = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                        LogManager.WriteLog(distributedChecking.ToString());
                        foreach (var item in distributedChecking)
                        {
                           
							if (item.Key== "ENABLE_MULTI_DISTRIBUTOR_CLIENT_CONFIG")


                            {
                                if(item.Value=="true")

                               {
									string gettingDistributorCode = ApiConstants.GET_DISTRIBUTEDCODE;
                                    ApplicationProperties.properties["IsEnableDistributor"] = "True";
                                    LogManager.WriteLog("Get  company  distributed Code" + gettingDistributorCode);
                                    LogManager.WriteLog(gettingDistributorCode.ToString());
                                    httpclient = RestClientUtil.getClient();

                                    var responseTask2 = httpclient.GetAsync(gettingDistributorCode);
                                    responseTask2.Wait();
                                    HttpResponseMessage Res2 = responseTask2.Result;

                                    LogManager.WriteResponseLog(Res2);
                                    if (Res2.IsSuccessStatusCode)
                                    {
                                        LogManager.WriteLog("getting distributed code  from Server is Success...");
                                        var response2 = Res2.Content.ReadAsStringAsync().Result;
                                        if (!(string.IsNullOrEmpty(response2)))
                                        {
                                            var DistributedCode = JsonConvert.DeserializeObject<DistributedDto>(response2);
                                            ApplicationProperties.properties["DistributedCode"] = DistributedCode.code;
                                            ApplicationProperties.properties["DistributedCodeCompany"] = DistributedCode.accountName;
                                           // ApplicationProperties.updatePropertiesFile();
                                      
                                        }
                                    }
                                }
                                else
                                {
									ApplicationProperties.properties["IsEnableDistributor"] = "False";
									//  ApplicationProperties.updatePropertiesFile();
								}


							}
                            if(item.Key == "ENABLE_SCHEME_CLIENT_APP")
                                {
								if (item.Value == "true")
                                {
									ApplicationProperties.properties["SchemeDiscountEnabled"] = "True";

								}
                                else
                                {
									ApplicationProperties.properties["SchemeDiscountEnabled"] = "False";
								}

							}
                            ApplicationProperties.updatePropertiesFile();



						}
                    }
                }
            }




            catch (Exception e)
            {
                LogManager.HandleException(e);
            }




        }
        public static void showMessageToClientAppPropertyUpdate ()
        {
            ApplicationProperties.getPropertyFromServer();
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
                ApplicationProperties.getPropertyFromServer();
            }
            else if (res == DialogResult.No)
            {
                ApplicationProperties.writeInitialProperties();
            }
        }

        private bool getsyncOperation ()
        {
            bool cond = false;
            try
            {
                SyncOperationDTO syncOperationDTO = syncOperationService.getAssignedSyncOperations();
                if (syncOperationDTO.operationTypes != null)
                {
                    if (syncOperationDTO.operationTypes.Count != 0)
                    {
                        MasterDataUploadServices.setSyncOperationTypes(syncOperationDTO.operationTypes);
                        UC_Download.setSyncOperationTypes(syncOperationDTO.operationTypes);
                        cond = true;
                    }
                }
            }
            catch (ServiceException exception)
            {
                LogManager.HandleException(exception);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
            }
            return cond;
        }

        private void username_Validating (object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text))
            {
                e.Cancel = true;
                //username.Focus();
                errorProvider1.SetError(username, "Username should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(username, "");
            }
        }


        private void password_Validating (object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(password.Text))
            {
                e.Cancel = true;
                //password.Focus();
                errorProvider1.SetError(password, "Password should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(password, "");
            }
        }

        private void close_btn_Click (object sender, EventArgs e)
        {

            // Clean up other resources if necessary
            Dispose();  // Call Dispose to release form resources

            // Exit the application
            Application.Exit();
        }

        //private void button3_Click (object sender, EventArgs e)
        //{
        //    var baseUrl = System.Configuration.ConfigurationManager.AppSettings["FullURL"];

        //    // Create and open the custom input form, passing the baseUrl as a default value
        //    CustomInputForm childForm = new CustomInputForm("Change Url", "Please enter new URL:", baseUrl);


        //    // Set the form to be displayed in the center of its parent
        //    childForm.StartPosition = FormStartPosition.CenterParent;

        //    // Show the child form as a modal dialog (centered on the parent)


        //    using (childForm)
        //    {
        //        if (childForm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(childForm.UserInput))
        //        {
        //            string appSettingsPath = @"D:\SAles&RichLatest\App.config"; // Path to your App.config
        //            AppSettingsUpdater updater = new AppSettingsUpdater(appSettingsPath); // AppSettings updater instance
        //            updater.UpdateFullUrl(childForm.UserInput); // Update the FullURL in config
        //        }
        //    }

        //}

        //private void linkLabel1_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    try
        //    {
        //        var baseUrl = System.Configuration.ConfigurationManager.AppSettings["FullURL"];

        //        // Create and open the custom input form, passing the baseUrl as a default value
        //        CustomInputForm childForm = new CustomInputForm("Change Url", "Please enter new URL:", baseUrl);


        //        // Set the form to be displayed in the center of its parent
        //        childForm.StartPosition = FormStartPosition.CenterParent;

        //        // Show the child form as a modal dialog (centered on the parent)


        //        using (childForm)
        //        {
        //            if (childForm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(childForm.UserInput))
        //            {
        //                string appSettingsPath = @"D:\SAles&RichLatest\App.config"; // Path to your App.config
        //                AppSettingsUpdater updater = new AppSettingsUpdater(appSettingsPath); // AppSettings updater instance
        //                updater.UpdateFullUrl(childForm.UserInput); // Update the FullURL in config
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //        LogManager.WriteLog(ex.Message.ToString());
        //    }
            

        //}

       
    }
}

