
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class SalesOrderUploadService
    {
        readonly TallyCommunicator tallyCommunicator;
        ReceivablePayableservice receivablePayableservice; 
        CompanyService companyService;

        UC_Logger uC_Logger;
        PostDatedVoucherService postDatedVoucherService;
        TemporaryOpeningStockService temporaryOpeningStockService; 
        PriceLevelListService priceLevelListService; 
        OpeningStockService openingStockService;
        private GstLedgersService gstLedgersService;
        private ProductWiseDefaultLedgerService productWiseDefaultLedgerService;
        private LedgerClosingBalanceUploadService ledgerClosingBalanceUploadService;
        ProductGroupProductProfileService productGroupProductProfileService;
        ProductGroupService productGroupService;
        ProductCategoryService productCategoryService;
        GSTProductGroupWiseService gstProductGroupWiseService;
        ProductProfileService productProfileService; 
        LocationService locationService;
        AccountProfileService accountProfileService; 
        private TaxMasterService taxMasterService; 
        LocationAccountProfileService locationAccountProfileService;
        bool isOptimise = false;
        public SalesOrderUploadService()
        {
            companyService = new CompanyService();
            tallyCommunicator = new TallyCommunicator();    
            ledgerClosingBalanceUploadService = new LedgerClosingBalanceUploadService();
            openingStockService = new OpeningStockService();
            priceLevelListService = new PriceLevelListService();
            receivablePayableservice = new ReceivablePayableservice();
            temporaryOpeningStockService = new TemporaryOpeningStockService();
            postDatedVoucherService = new PostDatedVoucherService();
            taxMasterService = new TaxMasterService();
            locationService = new LocationService();
            productProfileService = new ProductProfileService();
            gstProductGroupWiseService = new GSTProductGroupWiseService();
            productCategoryService = new ProductCategoryService();
            productGroupService = new ProductGroupService();
            accountProfileService = new AccountProfileService();
         
            productGroupProductProfileService = new ProductGroupProductProfileService();
            locationService = new LocationService();
            locationAccountProfileService = new LocationAccountProfileService();
            gstLedgersService = new GstLedgersService(); 
            productWiseDefaultLedgerService = new ProductWiseDefaultLedgerService(); 
            uC_Logger = new UC_Logger(); uC_Logger.BringToFront();
            uC_Logger.Dock=DockStyle.Top;

            uC_Logger.ClearLogArea();
        }
       

        public async Task ReceivablePayableUploadAsync()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage(" receivable-Payables upload started.");
                    var res = await receivablePayableservice.getFromTallyAndUploadAsync();
                    //  if (res)
                    appendLogMessage(" receivable-Payables upload completed.");
                    //else
                    //    appendLogMessage("  receivable-Payables upload failed.");
                }

            }
            catch (Exception ex)
            {
                appendLogMessage(" receivable-Payables upload failed.");
                LogManager.HandleException(ex);
            }
            finally
            {
                GlobalStateService.Currentstateoftask
                    = false;    
            }
         
        }
        public async Task PostDatedVoucherUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("post dated voucher upload started.");
                    postDatedVoucherService.getFromTallyAndUploadAsync();
                    appendLogMessage("post dated voucher upload completed.");
                }
                
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("post dated voucher upload failed.");

            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }
            
        }

        public async Task TemporaryOpeningStockUploadAsync()
        {
            try
            {
                GlobalStateService.Currentstateoftask = true;       
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }

                else
                {
                    appendLogMessage("temporary opening stock upload started.");
                    var res = await temporaryOpeningStockService.getFromTallyAndUploadAsync();
                    if (res)
                        appendLogMessage("temporary opening stock upload completed.");
                    else
                        appendLogMessage(" temporary opening stock upload failed.");

                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("temporary opening stock  failed.");

            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }


        }

        public async Task OpeningStockUploadAsync()

        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                 
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("opening stock upload started.");
                    var res = await openingStockService.getFromTallyAndUploadAsync();
                    if (res)
                        appendLogMessage("opening stock upload completed.");
                    else

                        appendLogMessage("opening stock upload failed.");
                }
             
                
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("Opening stock upload failed.");

            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }

        }

        public async Task PriceLevelListUploadAsync()
        {
            try
            {

                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("price level list upload started.");
                    await priceLevelListService.getFromTallyAndUploadAsync();
                    appendLogMessage("price level list upload completed.");
                }

                
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("price level list upload failed.");
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }
            
        }

        public async Task GstLedgerUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                   
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("GST ledgers upload started.");
                    gstLedgersService.getFromTallyAndUpload();
                    appendLogMessage("GST ledgers upload completed.");
                }
                
            }
            catch (Exception e)
            {
                appendLogMessage("GST ledgers upload Failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }
           
        }

        public async Task DefaultLedgerwiseItemUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                   
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("Default Ledger Wise profile upload started.");
                    await productWiseDefaultLedgerService.getFromTallyAndUploadAsync();
                    appendLogMessage("Default Ledger Wise profile upload completed.");
                }
               
            }
            catch (Exception ex)
            {
                appendLogMessage("Default Ledger Wise profile upload Failed.");
                LogManager.HandleException(ex);
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }
           
        }

        public async Task AccountClosingBalanceUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                   
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("account closing balance upload started.");
                    ledgerClosingBalanceUploadService.getFromTallyAndUpload();
                    appendLogMessage("account closing balance upload completed.");
                }
                
            }
            catch (Exception ex)
            {
                appendLogMessage("account closing balance upload failed.");
                LogManager.HandleException(ex);
            }
            finally
                {
                GlobalStateService.Currentstateoftask=false;
            }
        }

        public async Task GroupwiseAccountUpload()
        {
            try

               
            {
                GlobalStateService.Currentstateoftask = true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("location accountProfiles upload started.");
                    locationAccountProfileService.getFromTallyAndUpload(isOptimise);
                }
                
            } 
            catch (Exception e)
            {
                appendLogMessage("location accountProfiles upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {GlobalStateService.Currentstateoftask = false;
                appendLogMessage("location accountProfiles upload completed.");

            }
         
        }

        public async Task GroupWiseItemUpload()
        {
            try
            {
            GlobalStateService.Currentstateoftask = true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                   
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("product group product profile upload started.");
                    productGroupProductProfileService.getFromTallyAndUpload();
                }
                
            }
            catch (Exception e)
            {
                appendLogMessage("product group product profile upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask = false; 
                appendLogMessage("product group product profile upload completed.");

            }
         
        }

        public async Task GroupwiseGstUpload()
        {
            try
            {
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("group wise gst upload started.");
                    gstProductGroupWiseService.getFromTallyAndUpload();
                }
              
            }
            catch (Exception e)
            {
                appendLogMessage("group wise gst upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {GlobalStateService.Currentstateoftask = false;
                appendLogMessage("group wise gst upload completed.");
            }
        
        }

        public async Task AccountProfileUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("account profile upload started.");
                    accountProfileService.getFromTallyAndUpload(isOptimise);
                    appendLogMessage("account profile upload completed.");
                }
                
            }
            catch (Exception e)
            {
                appendLogMessage("account profile upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
            }
          
        }

        public async Task TaxMasterUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("tax master upload started.");
                    taxMasterService.getFromTallyAndUpload();
                }
               
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
                appendLogMessage("tax master upload Failed.");
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;

                appendLogMessage("tax master upload completed.");
            }
           
        }

        public async Task LocationUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask =true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("location upload started.");
                    locationService.getFromTallyAndUpload(isOptimise);
                }
                
            }
            catch (Exception e)
            {
                appendLogMessage("location upload Failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask = false;
                appendLogMessage("location upload completed.");
            }
            

        }

        public async Task ProductProfileUploadAsync()
        {
            try
            {
                GlobalStateService.Currentstateoftask = true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                 
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("stock item upload started.");
                    await productProfileService.getFromTallyAndUploadAsync(isOptimise);
                }
              
            }
            catch (Exception e)
            {
                appendLogMessage("stock item upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
                appendLogMessage("stock item upload completed.");
            }
    
        }
        // To check tally connection
        private async Task<bool> checkTallyCompanyIsOpened()
        {
            try
            {
                appendLogMessage("Verifying Tally and company .");
                List<CompanyDTO> companies = await companyService.GetCompanies();
                if (companies != null)
                {
                    if (!checkCompanyExist(companies))
                    {
                        MessageBox.Show("Please ensure company is open in tally ");
                        throw new ServiceException("Please ensure company is open in tally");
                        return false;
                    }
                    else
                    {
                        appendLogMessage("Tally and company verified.");
                        return true;

                    }
                }
                appendLogMessage("Tally and company verification failed.");
                return false;
            }
            catch (Exception ex)
            {
                appendLogMessage("Tally and company verification failed.");
                LogManager.HandleException(ex);
                return false;
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
        public async Task ProductCategoryUpload()
        {

            try
            {
                GlobalStateService.Currentstateoftask=true;
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                   
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("product category upload started.");
                    productCategoryService.getFromTallyAndUpload(isOptimise);
                }

                
            }
            catch (Exception e)
            {
                appendLogMessage("product category upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                appendLogMessage("product category upload completed.");
                GlobalStateService.Currentstateoftask=false;

            }
          
        }

        public async Task ProductGroupUpload()
        {
            try
            {
                GlobalStateService.Currentstateoftask = true;   
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    appendLogMessage("product group upload started.");
                    productGroupService.getFromTallyAndUpload(isOptimise);
                }
                

            }
            catch (Exception e)
            {
                appendLogMessage("product group upload failed.");
                LogManager.HandleException(e);
            }
            finally
            {
                GlobalStateService.Currentstateoftask=false;
                appendLogMessage("product group upload completed.");
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

    }
}
