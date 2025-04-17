using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using Microsoft.VisualBasic.Logging;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Tally;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Windows;


namespace SNR_ClientApp.Services
{

    internal class BackgroundTaskManagerService
    {

        readonly TallyCommunicator tallyCommunicator;
        private static BackgroundTaskManagerService _instance;
        CompanyService companyService;
        private static BackgroundTaskManagerService _instance1;
        private System.Threading.Timer _timer;
        // private static Dictionary<string, Object> props = new Dictionary<string, Object>();
    
        private System.Threading.Timer _timer1;
        UC_Download uC_Download;
        SalesOrderUploadService salesOrderUploadService;
        SalesOrderDownload salesOrderDownload;
        private static bool downloadStatus = false;
        private static bool uploadStatus = false;
        UC_Logger uC_Logger;
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;

     
        private string downloadtime = ApplicationProperties.properties.GetValueOrDefault("AutoDownloadTimePeriod").ToString();
        private string uploadtime = ApplicationProperties.properties.GetValueOrDefault("AutoUploadTimePeriod").ToString();
        private string isFirstTimeLogin = ApplicationProperties.properties.GetValueOrDefault("isFirstTimeLogin").ToString();
        public BackgroundTaskManagerService()
        {
           
            tallyCommunicator = new TallyCommunicator();
            companyService = new CompanyService();
            salesOrderDownload
                = new SalesOrderDownload();
            salesOrderUploadService = new SalesOrderUploadService();


            uC_Logger = new UC_Logger();

        }

        
        public static BackgroundTaskManagerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BackgroundTaskManagerService();
                }
                return _instance;
            }
        }
        public static BackgroundTaskManagerService Instance1
        {
            get
            {
                if (_instance1 == null)
                {
                    _instance1 = new BackgroundTaskManagerService();
                }
                return _instance1;
            }
        }

        public async Task StartBackgroundTasktodownloadAsync()
        {
        
            int downloadTime;
          
            if (_timer == null)
            {
                if (int.TryParse(downloadtime, out int parsedDownloadTime))
                {
                    downloadTime = parsedDownloadTime;
                    var time_interval = 60000*downloadTime;
                    //var time_interval = 300000;
                    //MessageBox.Show("hai");
                    _timer = new System.Threading.Timer(ExecuteTask, null, 0, time_interval); // Start immediately, 5 second intervals
                }
            }
        }


        private async void ExecuteTask(object state)
        {
            try
            {
                
                
                
                if (GlobalStateService.Currentstateoftask==false)
                {
                               
                if (!isFirstTimeLogin.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
     
                            GlobalStateService.Currentstateoftask = true;

                            // Execute your download task
                            await salesOrderDownload.salesOrderAutomaticDownload();

                            GlobalStateService.Currentstateoftask=false;
                        }
                    }
                }
            
            finally
            {
                // Task is complete, notify forms to enable buttons
                GlobalStateService.Currentstateoftask=false;
            }

        }
        public void appendLogMessage(string v)
        {
            uC_Logger.AppendLogMsg(v);
        }

        public void ClearLogMessage()
        {
            uC_Logger.ClearLogArea();
        }

        //private async Task<bool> checkTallyCompanyIsOpened()
        //{
        //    try
        //    {
        //        appendLogMessage("Verifying Tally and company .");
        //        List<CompanyDTO> companies = await companyService.GetCompanies();
        //        if (companies != null)
        //        {
        //            if (!checkCompanyExist(companies))
        //            {
        //                MessageBox.Show("Please ensure company is open in tally ");
        //                throw new ServiceException("Please ensure company is open in tally");
        //                return false;
        //            }
        //            else
        //            {
        //                appendLogMessage("Tally and company verified.");
        //                return true;

        //            }
        //        }
        //        appendLogMessage("Tally and company verification failed.");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        appendLogMessage("Tally and company verification failed.");
        //        LogManager.HandleException(ex);
        //        return false;
        //    }
        //}
        public async Task StartBackgroundTasktouploadAsync(List<string> syncOperationTypes)
        {
        
                int uploadTime;
                if (_timer1 == null)
                {
                    if (int.TryParse(uploadtime, out int parsedUploadTime))
                    {
                        uploadTime =parsedUploadTime;

                        var time_interval = 60000*uploadTime;
                        // var time_interval = 150000;
                        _timer1 = new System.Threading.Timer(ExecuteTaskupload, syncOperationTypes, 0, time_interval);
                    }
                }
            
            
        }
        

        private bool checkCompanyExist(List<CompanyDTO> companies)
        {
            foreach (CompanyDTO dto in companies)
            {
                if (dto.companyName.Equals(StringUtilsCustom.TALLY_COMPANY))
                {
                    return true;
                }
            }
            return false;
        }
        private async void ExecuteTaskupload(object state)
        {
            try
            {
                
                
               if (GlobalStateService.Currentstateoftask==false)
                {
                
                    var allAssignedSyncOperations = state as List<string>;
                   
                        if (!(isFirstTimeLogin.Equals("true", StringComparison.OrdinalIgnoreCase)))
                        {
                             uploadStatus=true;
                            if (allAssignedSyncOperations != null)
                            {
                                foreach (string syncOperation in allAssignedSyncOperations)
                                {
                                    if (syncOperation.Equals(SyncOperationType.PRODUCTGROUP.ToString()))
                                    {
                                        await salesOrderUploadService.ProductGroupUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.PRODUCTCATEGORY.ToString()))
                                    {
                                        await salesOrderUploadService.ProductCategoryUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.PRODUCTPROFILE.ToString()))
                                    {
                                        await salesOrderUploadService.ProductProfileUploadAsync();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.LOCATION.ToString()))
                                    {
                                        await salesOrderUploadService.LocationUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.TAX_MASTER.ToString()))
                                    {
                                        await salesOrderUploadService.TaxMasterUpload();
                                    }

                                    else if (syncOperation.Equals(SyncOperationType.ACCOUNT_PROFILE.ToString()))
                                    {
                                        await salesOrderUploadService.AccountProfileUpload();
                                    }

                                    else if (syncOperation.Equals(SyncOperationType.GST_PRODUCT_GROUP.ToString()))
                                    {
                                        await salesOrderUploadService.GroupwiseGstUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.PRODUCTGROUP_PRODUCTPROFILE.ToString()))
                                    {
                                        await salesOrderUploadService.GroupWiseItemUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.LOCATION_ACCOUNT_PROFILE.ToString()))
                                    {
                                        await salesOrderUploadService.GroupwiseAccountUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.ACCOUNT_PROFILE_CLOSING_BALANCE.ToString()))
                                    {
                                        await salesOrderUploadService.AccountClosingBalanceUpload();
                                    }

                                    else if (syncOperation.Equals(SyncOperationType.ACCOUNT_PROFILE_CLOSING_BALANCE.ToString()))
                                    {
                                        await salesOrderUploadService.AccountClosingBalanceUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.PRODUCT_WISE_DEFAULT_LEDGER.ToString()))
                                    {
                                        await salesOrderUploadService.DefaultLedgerwiseItemUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.GST_LEDGER.ToString()))
                                    {
                                        await salesOrderUploadService.GstLedgerUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.PRICE_LEVEL_LIST.ToString()))
                                    {
                                        await salesOrderUploadService.PriceLevelListUploadAsync();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.OPENING_STOCK.ToString()))
                                    {
                                        await salesOrderUploadService.OpeningStockUploadAsync();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.TEMPORARY_OPENING_STOCK.ToString()))
                                    {
                                        await salesOrderUploadService.TemporaryOpeningStockUploadAsync();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.POST_DATED_VOUCHER.ToString()))
                                    {
                                        await salesOrderUploadService.PostDatedVoucherUpload();
                                    }
                                    else if (syncOperation.Equals(SyncOperationType.RECEIVABLE_PAYABLE.ToString()))
                                    {
                                        await salesOrderUploadService.ReceivablePayableUploadAsync();
                                    }

                                }
                            
                            GlobalStateService.Currentstateoftask=false;
                        }
                        else
                        {

                            ExecuteTaskupload(state);

                        }


                    }
                }

               
            }
            finally
            {
              GlobalStateService.Currentstateoftask = false; 
            }
        }

        public void StopBackgroundTask()
        {
            _timer?.Dispose();
            _timer = null;
            _timer1?.Dispose();
            _timer1=null;

        }

    }
}


