using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json.Linq;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows.CustomControls
{
    public partial class UC_UserHome : UserControl
    {
        TallyService tallyService;
        private bool isOptimise = false;
        CompanyService companyService;
        SyncOperationService syncOperationService;
        //Dictionary<string, Object> props = new Dictionary<string, Object>();
        FileManagerService fileManagerService;
        ProductGroupService productGroupService;
        ProductCategoryService productCategoryService;
        ProductProfileService productProfileService;
        LocationService locationService;
        private TaxMasterService taxMasterService;
        AccountProfileService accountProfileService;
        GSTProductGroupWiseService gstProductGroupWiseService;
        ProductGroupProductProfileService productGroupProductProfileService;
        LocationAccountProfileService locationAccountProfileService;
        private LedgerClosingBalanceUploadService ledgerClosingBalanceUploadService;
        private ProductWiseDefaultLedgerService productWiseDefaultLedgerService;
        private GstLedgersService gstLedgersService;
        PriceLevelListService priceLevelListService;
        OpeningStockService openingStockService;
        PostDatedVoucherService postDatedVoucherService;
        UploadService uploadService;
        TemporaryOpeningStockService temporaryOpeningStockService;
        ReceivablePayableservice receivablePayableservice;
        UC_Logger uC_Logger;
        LocationHierarchyservice locationHierarchyservice;
        List<string> syncOperationTypes = new();
        string IsFirstTimeUpload;
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;
        static List<string> allAssignedSyncOperations;
        static List<CheckBox> selectedBoxes;
        private static bool downloadStatus=false;
        private static bool uploadStatus=false;
        public UC_UserHome()
        {
            InitializeComponent();

          
            companyService = new CompanyService();
            syncOperationService = new SyncOperationService();
            fileManagerService = new FileManagerService();
            productGroupService = new ProductGroupService();
            productCategoryService = new ProductCategoryService();
            productProfileService = new ProductProfileService();
            locationService = new LocationService();
            taxMasterService = new TaxMasterService();
            accountProfileService = new AccountProfileService();
            gstProductGroupWiseService = new GSTProductGroupWiseService();
            productGroupProductProfileService = new ProductGroupProductProfileService();
            locationAccountProfileService = new LocationAccountProfileService();
            ledgerClosingBalanceUploadService = new LedgerClosingBalanceUploadService();
            productWiseDefaultLedgerService = new ProductWiseDefaultLedgerService();
            gstLedgersService = new GstLedgersService();
            tallyService = new TallyService();
            priceLevelListService = new PriceLevelListService();
            openingStockService = new OpeningStockService();
            postDatedVoucherService = new PostDatedVoucherService();
            uploadService=new UploadService();
            temporaryOpeningStockService = new TemporaryOpeningStockService();
            receivablePayableservice = new ReceivablePayableservice();
            locationHierarchyservice = new LocationHierarchyservice();
            StringUtilsCustom.TALLY_COMPANY = props.GetValueOrDefault("tally.company").ToString();
            IsFirstTimeUpload= props.GetValueOrDefault("isFirstTimeUpload").ToString();

            uC_Logger = new UC_Logger();
            loggerPanel.Controls.Clear();
            loggerPanel.Controls.Add(uC_Logger);
            uC_Logger.BringToFront();
            uC_Logger.Dock=DockStyle.Top;

            uC_Logger.ClearLogArea();
            //  uC_Logge r.SetBounds(10, 10, 100, 100);

            loadCompanyNames();
           
            //sync operationtypes
            var syncOperationTypes = MasterDataUploadServices.getSyncOperationTypes();
            List<SyncOperationType> allSyncOperationTypes = Enum.GetValues(typeof(SyncOperationType)).Cast<SyncOperationType>().ToList();
            removeNotAssynedCheckBoxes(allSyncOperationTypes);

            // checkAllCheckboxes(true);
            // this.Load += new EventHandler(MyForm_Shown);                                                        

            
    
           
           // UploadSalesFromTally();
            automatupdate();


        }
        private async Task automatupdate()
        {
            List<SyncOperationType> allSyncOperationTypes = Enum.GetValues(typeof(SyncOperationType)).Cast<SyncOperationType>().ToList();
            var syncOperationTypes = MasterDataUploadServices.getSyncOperationTypes();
            allAssignedSyncOperations = allSyncOperationTypes.Select(a => a.ToString()).ToList();
            var autodownloadenable = ApplicationProperties.properties.GetValueOrDefault("AutoDownload").ToString();
            if (autodownloadenable.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
               
           
                await BackgroundTaskManagerService.Instance.StartBackgroundTasktodownloadAsync();
               
               
            }
            var autouploadenable = ApplicationProperties.properties.GetValueOrDefault("AutoUpload").ToString();
            if (autouploadenable.Equals("true", StringComparison.OrdinalIgnoreCase))
            {


                await BackgroundTaskManagerService.Instance1.StartBackgroundTasktouploadAsync(syncOperationTypes);


            }
        }



        private void MyForm_Shown(object sender, EventArgs e)
        {
            // Code to be executed when the form is loaded
            if (IsFirstTimeUpload.Equals("true", StringComparison.OrdinalIgnoreCase))
            {

                DialogResult result = MessageBox.Show("SalesNrich Application Is Ready For Initial Upload ,\n Do You Want To Continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Code to perform the action
                }
                //initialUpload();
                //ApplicationProperties.properties["isFirstTimeUpload"] = "false";
                //ApplicationProperties.updatePropertiesFile();
            };
        }


        private void removeNotAssynedCheckBoxes(List<SyncOperationType> allSyncOperationTypes)
        {
            //Log.Info("request for removing not assigned checkboxes");
         selectedBoxes = new List<CheckBox>();
            var syncOperationTypes = MasterDataUploadServices.getSyncOperationTypes();
              allAssignedSyncOperations = allSyncOperationTypes.Select(a => a.ToString()).ToList();

            //allAssignedSyncOperations.RemoveAll(syncOperationTypes);
            List<string> allNotAssignedSyncOperations = allAssignedSyncOperations.Except(syncOperationTypes).ToList();

            foreach (string syncOperationType in allNotAssignedSyncOperations)
            {
                if (syncOperationType.Equals(SyncOperationType.PRODUCTCATEGORY.ToString()))

                {
                    selectedBoxes.Add(chk_productCategory);
                    chk_productCategory.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.PRODUCTGROUP.ToString()))
                {
                    selectedBoxes.Add(chk_productGroup);
                    chk_productGroup.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.PRODUCTPROFILE.ToString()))
                {
                    selectedBoxes.Add(chk_productProfile);
                    chk_productProfile.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.OPENING_STOCK.ToString()))
                {
                    selectedBoxes.Add(chk_openingStock);
                    chk_openingStock.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.TEMPORARY_OPENING_STOCK.ToString()))
                {
                    selectedBoxes.Add(chk_temporary_openingStock_id);
                    chk_temporary_openingStock_id.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.PRODUCTGROUP_PRODUCTPROFILE.ToString()))
                {
                    selectedBoxes.Add(chk_groupWiseItem);
                    chk_groupWiseItem.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.GST_PRODUCT_GROUP.ToString()))
                {
                    selectedBoxes.Add(chk_groupWiseGST);
                    chk_groupWiseGST.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.TAX_MASTER.ToString()))
                {
                    selectedBoxes.Add(chk_taxMaster);
                    chk_taxMaster.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.LOCATION.ToString()))
                {
                    selectedBoxes.Add(chk_location);
                    chk_location.Visible = false;
                }
                else if (syncOperationType.Equals(SyncOperationType.ACCOUNT_PROFILE.ToString()))
                {
                    selectedBoxes.Add(chk_accountProfile);
                    chk_accountProfile.Visible=false;
                }
                else if (syncOperationType.Equals(SyncOperationType.PRICE_LEVEL_LIST.ToString()))
                {
                    selectedBoxes.Add(chk_priceLevelList_id);
                    chk_priceLevelList_id.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.RECEIVABLE_PAYABLE.ToString()))
                {
                    selectedBoxes.Add(chk_receiveblePayeble_id);
                    chk_receiveblePayeble_id.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.LOCATION_ACCOUNT_PROFILE.ToString()))
                {
                    selectedBoxes.Add(chk_groupWiseAccount);
                    chk_groupWiseAccount.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.ACCOUNT_PROFILE_CLOSING_BALANCE.ToString()))
                {
                    selectedBoxes.Add(chk_Account_ClosingBalance);
                    chk_Account_ClosingBalance.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.POST_DATED_VOUCHER.ToString()))
                {
                    selectedBoxes.Add(chk_post_dated_voucher);
                    chk_post_dated_voucher.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.GST_LEDGER.ToString()))
                {
                    selectedBoxes.Add(chk_gst_ledgers);
                    chk_gst_ledgers.Visible=(false);
                }
                else if (syncOperationType.Equals(SyncOperationType.PRODUCT_WISE_DEFAULT_LEDGER.ToString()))
                {
                    selectedBoxes.Add(chk_defultLedgerWiseItem);
                    chk_defultLedgerWiseItem.Visible=(false);
                }


            }
            foreach (var sync in syncOperationTypes)
            {

                if (sync.Equals(SyncOperationType.LOCATION_HIRARCHY.ToString()))
                {
                    uploadLocationHeirarchyButton.Visible=(true);
                }
            }

        }

        private void checkAllCheckboxes(bool isCheck)
        {
            chk_selectAll.Checked = (chk_selectAll.Visible==true) ? isCheck : false;
            //  chk_fullUpdate.Checked=(chk_fullUpdate.Visible==true)?isCheck:false;
            chk_gst_ledgers.Checked =(chk_gst_ledgers.Visible) ? isCheck : false;
            chk_post_dated_voucher.Checked = (chk_post_dated_voucher.Visible==true) ? isCheck : false;
            chk_Account_ClosingBalance.Checked = (chk_Account_ClosingBalance.Visible) ? isCheck : false;
            chk_groupWiseAccount.Checked = (chk_groupWiseAccount.Visible) ? isCheck : false;
            chk_receiveblePayeble_id.Checked = (chk_receiveblePayeble_id.Visible) ? isCheck : false;
            chk_accountProfile.Checked = (chk_accountProfile.Visible) ? isCheck : false;
            chk_location.Checked = (chk_location.Visible) ? isCheck : false;
            chk_taxMaster.Checked = (chk_taxMaster.Visible) ? isCheck : false;
            chk_groupWiseGST.Checked = (chk_groupWiseGST.Visible) ? isCheck : false;
            chk_groupWiseItem.Checked = (chk_groupWiseItem.Visible) ? isCheck : false;
            chk_priceLevelList_id.Checked = (chk_priceLevelList_id.Visible) ? isCheck : false;
            chk_temporary_openingStock_id.Checked = (chk_temporary_openingStock_id.Visible) ? isCheck : false;
            chk_openingStock.Checked = (chk_openingStock.Visible) ? isCheck : false;
            chk_productProfile.Checked = (chk_productProfile.Visible) ? isCheck : false;
            chk_productCategory.Checked=(chk_productCategory.Visible) ? isCheck : false;
            chk_productGroup.Checked=(chk_productGroup.Visible) ? isCheck : false;
            chk_defultLedgerWiseItem.Checked=(chk_defultLedgerWiseItem.Visible) ? isCheck : false;
            // chk_fullUpdate.Checked=(chk_fullUpdate.Visible) ? isCheck:false;
            

    }

    private void LoadLoggerArea()
        {
            uC_Logger = new UC_Logger();
            AddUserControl(uC_Logger);
        }
        private void AddUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            loggerPanel.Controls.Clear();
            loggerPanel.Controls.Add(userControl);

            userControl.BringToFront();
        }

        private void loadCompanyNames()
        {
            try
            {
                // object[] row = tallyService.getCompanies();
                List<string> names = new List<string>();
                string company = CompanyService.getCompanyName();
                names.Add(company);
                companySelect.DataSource = names.ToArray();
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
                MessageBox.Show("Unable to fetch Company names \n"+e.Message);
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            ParentForm.Cursor = Cursors.WaitCursor;
            uC_Logger.AppendLogMsg("Uploading Datas To Server Started.....");
            try
            {
                isOptimisedEnable();
             
                UploadDatasToServerAsync();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                uC_Logger.AppendLogMsg("Uploading Datas To Server Failed .....");
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        private async Task<bool> checkTallyCompanyIsOpenedAsync()
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
        private bool checkCompanyExist(List<CompanyDTO> companies)
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
        private async Task UploadSalesFromTally()
        {
           
            try
            {
                //string saledate = ApplicationProperties.properties["salessorderDate"].ToString();
                ClearLogMessage();
                appendLogMessage("Verifying tally and compnay.");
                if (!await checkTallyCompanyIsOpenedAsync())
                {
                    appendLogMessage("Verifying tally and compnay failed.\n Please ensure company is open and Active in tally ");
                    MessageBox.Show("Please ensure company is open and Active in tally ");
                }
                else
                {
                    appendLogMessage("Tally and company verified.");
                    //if(!string.IsNullOrEmpty(saledate))
                    //{
                        //DateTime converteddate = ConvertDate(saledate);

                        //string formatedDate = converteddate.ToString("d-MMM-yy");
                        await uploadService.getFromTallyAndUploadAsync();

                    //}

                    //else
                    //{
                    //    await uploadService.getFromTallyAndUploadAsync();

                    //}


                  
                }

            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "exception occured while uploading datas to server ");
                appendLogMessage("Upload data to server failed ...");
            }
            finally
            {
                //MessageBox.Show(" Master Data Upload Completed ");
            }
        }
        private async Task UploadDatasToServerAsync()
        {
            try
            {
                ClearLogMessage();
                appendLogMessage("Verifying tally and compnay.");
                if (!await checkTallyCompanyIsOpenedAsync())
                {
                    appendLogMessage("Verifying tally and compnay failed.\n Please ensure company is open and Active in tally ");
                    MessageBox.Show("Please ensure company is open and Active in tally ");
                }
                else
                {
                    appendLogMessage("Tally and company verified.");

                    //List<SyncOperationTimeDTO> syncOperationTimeDTOs=syncOperationService.getSyncOperations();
                    //if (syncOperationTimeDTOs != null && syncOperationTimeDTOs.Count > 0)
                    //{
                    //   // deleteStatusFalseFiles(syncOperationTimeDTOs);
                    //}

                    if (chk_productGroup.Checked)
                    {
                        ProductGroupUpload();

                    }
                    if (chk_productCategory.Checked)
                    {
                        ProductCategoryUpload();

                    }

                    if (chk_productProfile.Checked)
                    {
                        ProductProfileUploadAsync();

                    }
                    if (chk_location.Checked)
                    {
                        LocationUpload();

                    }

                    if (chk_taxMaster.Checked)
                    {
                        TaxMasterUpload();

                    }

                    if (chk_accountProfile.Checked)
                    {
                        AccountProfileUpload();

                    }

                    if (chk_groupWiseGST.Checked)
                    {
                        GroupwiseGstUpload();

                    }

                    if (chk_groupWiseItem.Checked)
                    {
                        GroupWiseItemUpload();

                    }

                    if (chk_groupWiseAccount.Checked)
                    {
                        GroupwiseAccountUpload();

                    }

                    if (chk_Account_ClosingBalance.Checked)
                    {
                        AccountClosingBalanceUpload();

                    }

                    if (chk_defultLedgerWiseItem.Checked)
                    {
                        DefaultLedgerwiseItemUpload();

                    }


                    if (chk_gst_ledgers.Checked)
                    {
                        GstLedgerUpload();

                    }

                    // working async in server
                    if (chk_priceLevelList_id.Checked)
                    {
                        await PriceLevelListUploadAsync();

                    }

                    // working async in server
                    if (chk_openingStock.Checked)
                    {
                        await OpeningStockUploadAsync();

                    }

                    if (chk_temporary_openingStock_id.Checked)
                    {
                        await TemporaryOpeningStockUploadAsync();

                    }

                    if (chk_post_dated_voucher.Checked)
                    {
                        PostDatedVoucherUpload();

                    }

                    if (chk_receiveblePayeble_id.Checked)
                    {
                        await ReceivablePayableUploadAsync();

                    }

                    MessageBox.Show(" Master Data Upload Completed ");
                }

            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "exception occured while uploading datas to server ");
                appendLogMessage("Upload data to server failed ...");
            }
            finally
            {
                //MessageBox.Show(" Master Data Upload Completed ");
            }
        }
        private DateTime ConvertDate(string date)
        {
            string formattedDate = "";
            string[] splitDates = date.Split('T');
            DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
           
            return dateTime;
        }
        private async Task ReceivablePayableUploadAsync()
        {
            try
            {
                appendLogMessage(" receivable-Payables upload started.");
                var res = await receivablePayableservice.getFromTallyAndUploadAsync();
                //  if (res)
                appendLogMessage(" receivable-Payables upload completed.");
                //else
                //    appendLogMessage("  receivable-Payables upload failed.");
            }
            catch (Exception ex)
            {
                appendLogMessage(" receivable-Payables upload failed.");
                LogManager.HandleException(ex);
            }
        }

        private void PostDatedVoucherUpload()
        {
            try
            {
                appendLogMessage("post dated voucher upload started.");
                postDatedVoucherService.getFromTallyAndUploadAsync();
                appendLogMessage("post dated voucher upload completed.");
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("post dated voucher upload failed.");

            }
        }

        private async Task TemporaryOpeningStockUploadAsync()
        {
            appendLogMessage("temporary opening stock upload started.");
            var res = await temporaryOpeningStockService.getFromTallyAndUploadAsync();
            if (res)
                appendLogMessage("temporary opening stock upload completed.");
            else
                appendLogMessage(" temporary opening stock upload failed.");
        }

        private async Task OpeningStockUploadAsync()
        {
            appendLogMessage("opening stock upload started.");
            var res = await openingStockService.getFromTallyAndUploadAsync();
            if (res)
                appendLogMessage("opening stock upload completed.");
            else
                appendLogMessage("opening stock upload failed.");
        }

        private async Task PriceLevelListUploadAsync()
        {
            try
            {
                appendLogMessage("price level list upload started.");
                await priceLevelListService.getFromTallyAndUploadAsync();
                appendLogMessage("price level list upload completed.");
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("price level list upload failed.");
            }
        }

        private void GstLedgerUpload()
        {
            try
            {
                appendLogMessage("GST ledgers upload started.");
                gstLedgersService.getFromTallyAndUpload();
                appendLogMessage("GST ledgers upload completed.");
            }
            catch (Exception e)
            {
                appendLogMessage("GST ledgers upload Failed.");
                LogManager.HandleException(e);
            }
        }

        private async void DefaultLedgerwiseItemUpload()
        {
            try
            {
                appendLogMessage("Default Ledger Wise profile upload started.");
                await productWiseDefaultLedgerService.getFromTallyAndUploadAsync();
                appendLogMessage("Default Ledger Wise profile upload completed.");
            }
            catch (Exception ex)
            {
                appendLogMessage("Default Ledger Wise profile upload Failed.");
                LogManager.HandleException(ex);
            }
        }

        private void AccountClosingBalanceUpload()
        {
            try
            {
                appendLogMessage("account closing balance upload started.");
                ledgerClosingBalanceUploadService.getFromTallyAndUpload();
                appendLogMessage("account closing balance upload completed.");
            }
            catch (Exception ex)
            {
                appendLogMessage("account closing balance upload failed.");
                LogManager.HandleException(ex);
            }
        }

        private void GroupwiseAccountUpload()
        {
            try
            {
                appendLogMessage("location accountProfiles upload started.");
                locationAccountProfileService.getFromTallyAndUpload(isOptimise);
            }
            catch (Exception e)
            {
                appendLogMessage("location accountProfiles upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("location accountProfiles upload completed.");

            }
        }

        private void GroupWiseItemUpload()
        {
            appendLogMessage("product group product profile upload started.");
            try
            {


                productGroupProductProfileService.getFromTallyAndUpload();
            }
            catch (Exception e)
            {
                appendLogMessage("product group product profile upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("product group product profile upload completed.");

            }
        }

        private void GroupwiseGstUpload()
        {
            try
            {
                appendLogMessage("group wise gst upload started.");
                gstProductGroupWiseService.getFromTallyAndUpload();
            }
            catch (Exception e)
            {
                appendLogMessage("group wise gst upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("group wise gst upload completed.");
            }
        }

        private void AccountProfileUpload()
        {
            try
            {
                appendLogMessage("account profile upload started.");
                accountProfileService.getFromTallyAndUpload(isOptimise);
                appendLogMessage("account profile upload completed.");
            }
            catch (Exception e)
            {
                appendLogMessage("account profile upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {

            }
        }

        private void TaxMasterUpload()
        {
            try
            {
                appendLogMessage("tax master upload started.");
                taxMasterService.getFromTallyAndUpload();
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
                appendLogMessage("tax master upload Failed.");
            }
            finally
            {
                appendLogMessage("tax master upload completed.");
            }
        }

        private void LocationUpload()
        {
            try
            {
                appendLogMessage("location upload started.");
                locationService.getFromTallyAndUpload(isOptimise);
            }
            catch (Exception e)
            {
                appendLogMessage("location upload Failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("location upload completed.");
            }
        }

        private async Task ProductProfileUploadAsync()
        {
            try
            {
                appendLogMessage("stock item upload started.");
                await productProfileService.getFromTallyAndUploadAsync(isOptimise);
            }
            catch (Exception e)
            {
                appendLogMessage("stock item upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("stock item upload completed.");
            }
        }

        private void ProductCategoryUpload()
        {
            try
            {
                appendLogMessage("product category upload started.");
                productCategoryService.getFromTallyAndUpload(isOptimise);
            }
            catch (Exception e)
            {
                appendLogMessage("product category upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("product category upload completed.");
            }
        }

        private void ProductGroupUpload()
        {
            try
            {
                appendLogMessage("product group upload started.");
                productGroupService.getFromTallyAndUpload(isOptimise);

            }
            catch (Exception e)
            {
                appendLogMessage("product group upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("product group upload completed.");
            }
        }

        private void appendLogMessage(string v)
        {
            uC_Logger.AppendLogMsg(v);
        }

        private void ClearLogMessage()
        {
            uC_Logger.ClearLogArea();
        }

        private void deleteStatusFalseFiles(List<SyncOperationTimeDTO>? syncOperationTimeDTOs)
        {
            foreach (SyncOperationTimeDTO syncOperationTimeDTO in syncOperationTimeDTOs)
            {
                if (syncOperationTimeDTO.operationType.Equals(SyncOperationType.ACCOUNT_PROFILE))
                {
                    // loggerTabController.logMessage(SyncOperationType.ACCOUNT_PROFILE + "-- delete start--");
                    fileManagerService.deleteFile(AccountProfileService.FILE_NAME);
                    //  loggerTabController.logMessage(SyncOperationType.ACCOUNT_PROFILE + "-- delete finish--");
                }
                else if (syncOperationTimeDTO.operationType.Equals(SyncOperationType.LOCATION_ACCOUNT_PROFILE))
                {
                    // loggerTabController.logMessage(SyncOperationType.LOCATION_ACCOUNT_PROFILE + "-- delete start--");
                    fileManagerService.deleteFile(LocationAccountProfileService.FILE_NAME);
                    //  loggerTabController.logMessage(SyncOperationType.LOCATION_ACCOUNT_PROFILE + "-- delete finish--");
                }
                else if (syncOperationTimeDTO.operationType.Equals(SyncOperationType.PRODUCTPROFILE))
                {
                    // loggerTabController.logMessage(SyncOperationType.PRODUCTPROFILE + "-- delete start--");
                    fileManagerService.deleteFile(ProductProfileService.FILE_NAME);
                    // loggerTabController.logMessage(SyncOperationType.PRODUCTPROFILE + "-- delete finish--");
                }
                else if (syncOperationTimeDTO.operationType.Equals(SyncOperationType.PRODUCTGROUP_PRODUCTPROFILE))
                {
                    //loggerTabController.logMessage(SyncOperationType.PRODUCTGROUP_PRODUCTPROFILE + "-- delete start--");
                    fileManagerService.deleteFile(ProductGroupProductProfileService.FILE_NAME);
                    // loggerTabController.logMessage(SyncOperationType.PRODUCTGROUP_PRODUCTPROFILE + "-- delete finish--");
                }
                else if (syncOperationTimeDTO.operationType.Equals(SyncOperationType.PRICE_LEVEL_LIST))
                {
                    // loggerTabController.logMessage(SyncOperationType.PRICE_LEVEL_LIST + "-- delete start--");
                    fileManagerService.deleteFile(PriceLevelListService.FILE_NAME);
                    //  loggerTabController.logMessage(SyncOperationType.PRICE_LEVEL_LIST + "-- delete finish--");
                }
            }
        }

        private void isOptimisedEnable()
        {
            LogManager.WriteLog("List optimised....");
            if (chk_fullUpdate.Checked)
            {
                isOptimise = false;
            }
            else
            {
                isOptimise = true;
            }
            LogManager.WriteLog("List optimised...." + isOptimise);
        }

        private void initialUpload()
        {
            try
            {
                checkAllCheckboxes(true);
                chk_fullUpdate.Checked = true;
                UploadDatasToServerAsync();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("Initial upload failed...");
            }
            finally
            {
                checkAllCheckboxes(true);
                chk_fullUpdate.Checked = false;
            }
        }

        private void chk_selectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool ischeck = chk_selectAll.Checked;
            //chk_productGroup.Checked = ischeck;
            //chk_productCategory.Checked = ischeck;
            //chk_productProfile.Checked = ischeck;
            //chk_location.Checked = ischeck;
            //chk_taxMaster.Checked = ischeck;
            //chk_accountProfile.Checked = ischeck;
            //chk_groupWiseGST.Checked = ischeck;
            //chk_groupWiseItem.Checked = ischeck;
            //chk_groupWiseAccount.Checked = ischeck;
            //chk_Account_ClosingBalance.Checked = ischeck;
            //chk_defultLedgerWiseItem.Checked = ischeck;
            //chk_gst_ledgers.Checked = ischeck;
            //chk_priceLevelList_id.Checked = ischeck;
            //chk_openingStock.Checked = ischeck;
            //chk_temporary_openingStock_id.Checked = ischeck;
            //chk_receiveblePayeble_id.Checked = ischeck;
            //chk_post_dated_voucher.Checked = ischeck;
            checkAllCheckboxes(ischeck);
        }

        private async Task uploadLocationHeirarchyAsync()
        {
            if (await checkTallyCompanyIsOpenedAsync())
            {

                appendLogMessage("location-hierarchy upload started.");
                locationHierarchyservice.getFromTallyAndUpload();
                appendLogMessage("location-hierarchy  upload completed.");

            }
            else
            {
                appendLogMessage("Check whether tally is open and company is active.");
            }

        }

        private void uploadLocationHeirarchyButton_Click(object sender, EventArgs e)
        {
            try
            {
                uploadLocationHeirarchyAsync();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Upload Location Heirarchy Failed..");
                appendLogMessage("location-hierarchy  upload Failed...");
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        private void companySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringUtilsCustom.TALLY_COMPANY = companySelect.Text;
            ApplicationProperties.properties["tally.company"] = companySelect.Text;
            ApplicationProperties.updatePropertiesFile();
        }

        public void LedgerMasterUpload()
        {
            chk_productProfile.Checked = true;
            chk_accountProfile.Checked = true;
            UploadDatasToServerAsync();
        }

       
    }
}
