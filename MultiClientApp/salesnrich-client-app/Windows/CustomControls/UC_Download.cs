using dtos;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows.CustomControls
{
	public partial class UC_Download : UserControl
	{
		TallyService tallyService;

		CompanyService companyService;
		//Dictionary<string, Object> props = new Dictionary<string, Object>();
		UC_Logger uC_Logger;
		DownloadReceiptService downloadReceiptService;
		DownloadSalesReturnService downloadSalesReturnService;
		DownloadByVoucherTypeService downloadByVoucherTypeService;
		private String enableDateWise;
		private String enableselectedDateWise;
		private String userStockLocation;
		DownloadSalesOrderService downloadSalesOrderService;
		DownloadSalesService downloadSalesService;
		DownloadSalesJournalService downloadSalesJournalService;
		DownloadJournalService downloadJournalService;
		DownloadLedgerService downloadLedgerService;
		private String companyString;
		private String salesreturnvouchertype;
		private String apiUrl;
		private String byEmpVoucher;




		private String employeeString;
		private static List<String> syncOperationTypes = new List<String>();
		public UC_Download()
		{
			InitializeComponent();


			companyService = new CompanyService();
			tallyService = new TallyService();
			uC_Logger = new UC_Logger();
			downloadReceiptService = new DownloadReceiptService();
			downloadSalesReturnService = new DownloadSalesReturnService();
			enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
			enableselectedDateWise = ApplicationProperties.properties["enable.Selecteddate"].ToString();
			companyString = ApplicationProperties.properties["tally.company"].ToString();
			// apiUrl = ApplicationProperties.properties["service.full.url"].ToString();
			byEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"].ToString();
			userStockLocation = ApplicationProperties.properties["user.stockLocation"].ToString();
			employeeString = ApplicationProperties.properties["download.by.employees"].ToString();
			salesreturnvouchertype = ApplicationProperties.properties["salesreturnvouchertype"].ToString();
			downloadByVoucherTypeService = new DownloadByVoucherTypeService();
			downloadSalesOrderService = new DownloadSalesOrderService();
			downloadSalesService = new DownloadSalesService();
			downloadSalesJournalService = new DownloadSalesJournalService();
			downloadJournalService = new DownloadJournalService();
			downloadLedgerService = new DownloadLedgerService();


			LoadLoggerArea();
			List<SyncOperationType> allSyncOperationTypes = Enum.GetValues(typeof(SyncOperationType)).Cast<SyncOperationType>().ToList();
			removeNotAssynedCheckBoxes(allSyncOperationTypes);

			// Now you can start the background task
			loadInitialValues();

		}

		private void loadInitialValues()
		{
			loadCompanyNames();
			if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase))
			{
				List<String> employees = new List<string>();
				employees = employeeString.Split(",").ToList();
				employeeList.Items.Clear();
				String[] employeesArray = employees.ToArray();
				employeeList.DataSource = employeesArray;
				// employeeList.Items.Add(employees.ToArray());
				employeeList.Visible = true;
			}
			else
			{
				employeeList.Visible = false;
			}
			if (!("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase)) && !("true".Equals(enableselectedDateWise, StringComparison.OrdinalIgnoreCase)))
			{
				salesDate.Visible = false;
			}
		}

		private void loadCompanyNames()
		{
			try
			{
				//object[] row = tallyService.getCompanies();
				//companySelect.DataSource = row;
				List<string> names = new List<string>();
				string company = CompanyService.getCompanyName();
				names.Add(company);
				companySelect.DataSource = names.ToArray();
			}
			catch (Exception e)
			{
				LogManager.HandleException(e);
				MessageBox.Show("Unable to fetch Company names");
			}
		}
		private void LoadLoggerArea()
		{
			uC_Logger = new UC_Logger();
			AddUserControl(uC_Logger);
		}
		private void AddUserControl(UserControl userControl)
		{
			userControl.Dock = DockStyle.Fill;
			LoggerArea.Controls.Clear();
			LoggerArea.Controls.Add(userControl);

			userControl.BringToFront();
		}


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
		private void appendLogMessage(string v)
		{
			uC_Logger.AppendLogMsg(v);
		}
		private void ClearLoggerArea()
		{
			uC_Logger.ClearLogArea();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadReceipt();
			GlobalStateService.Currentstateoftask = false;
			//MessageBox.Show("Process Completed ");
		}

		private async void downloadReceipt()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				//appendLogMessage("Verifying tally and compnay.");
				if (enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download  Reciept without selecting  date");
						return;
					}
				}
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download receipt started.");
					//List<ReceiptDTO> receiptDTOs = new List<ReceiptDTO>();
					downloadReceiptService.getFromServerAndDownloadToTallyAsync(uC_Logger, salesDate.Value);
					//if (receiptDTOs!=null)
					//{
					//    appendLogMessage("Download completed ");
					//    if (receiptDTOs.SuccessOrders.Count > 0)
					//        appendLogMessage(receiptDTOs.SuccessOrders.Count+" receipt Downloaded Successfully.");
					//    if (receiptDTOs.FailedOrders.Count > 0)
					//    {
					//        appendLogMessage(receiptDTOs.FailedOrders.Count +" receipt Downloaded failed..Try again..");
					//        if (receiptDTOs.failedOrdersLineErrors.Count > 0)
					//        {
					//            string joinedString = string.Join(" \n", receiptDTOs.failedOrdersLineErrors);
					//            appendLogMessage(joinedString);
					//        }
					//    }
					//}
					//else
					//{
					//    appendLogMessage("No receipts found for downloading");
					//}
				}
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Download receipt failed \n " + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
			}
		}

		private async void btn_salesOrder_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			await downloadSalesOrderAsync();
			GlobalStateService.Currentstateoftask = false;
			MessageBox.Show("Process Completed ");
		}

		public async Task downloadSalesOrderAsync()
		{
			try
			{

				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;

				//DateTime dd = salesDate.Value.Date;
				//DateOnly dateOnly = DateOnly.FromDateTime(dd);
				if (enableDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download Sales Order without selecting  date");
						return;
					}
				}
				if (enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download Sales Order without selecting  date");
						return;
					}
				}
				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download Sales Order started.");
					DownloadResponseDto res = new();
					if (userStockLocation.Equals("true", StringComparison.OrdinalIgnoreCase))
					{
						GlobalStateService.Currentstateoftask = true;
						if (ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
						{
							await downloadByVoucherTypeService
							  .getFromServerAndDownloadToTally(VoucherType.SECONDARY_SALES_ORDER, salesDate.Value, uC_Logger);
							GlobalStateService.Currentstateoftask = false;
						}
						else
						{
							await downloadByVoucherTypeService
							   .getFromServerAndDownloadToTally(VoucherType.PRIMARY_SALES_ORDER, salesDate.Value, uC_Logger);
							GlobalStateService.Currentstateoftask = false;
						}

					}
					else
					{
						GlobalStateService.Currentstateoftask = true;

						await downloadSalesOrderService
									.getFromServerAndDownloadToTallyAsync(salesDate.Value, uC_Logger);
						GlobalStateService.Currentstateoftask = false;
					}
					//if (res.SuccessCount > 0)
					//{
					//    appendLogMessage(res.SuccessCount+" sales order downloaded .");
					//}
					//if (res.FailedCount > 0)
					//{
					//    appendLogMessage(res.FailedCount+" sales order failed to downloaded .");
					//}
					//if (res.TotalCount <= 0)
					//{
					//    appendLogMessage("No sales found for downloading.");
					//}
					//else
					//{
					//	appendLogMessage("No sales order found for downloading.");
					//}
					//List<ReceiptDTO> receiptDTOs = await downloadReceiptService.getFromServerAndDownloadToTallyAsync();
					//if (receiptDTOs.Count > 0)
					//{
					//    appendLogMessage("Download receipt completed.");
					//}
					//else
					//{
					//    appendLogMessage("No receipts found for downloading");
					//}
				}
			}
			catch (ServiceException ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("downloading sales order failed .\n " + ex.Message);
				//appendLogMessage(ex.Message);
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("downloading sales order failed .");
				//appendLogMessage(ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}

		private void btn_Journal_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadJournal();
			GlobalStateService.Currentstateoftask = false;
			MessageBox.Show("Process Completed ");
		}

		private async void downloadJournal()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;
				if (enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download Journal  without selecting  date");
						return;
					}
				}

				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{

					appendLogMessage("Download journal started.");
					await downloadJournalService.getFromServerAndDownloadToTallyAsync(salesDate.Value, uC_Logger);

					//if (receiptDTOs != null)
					//{
					//    appendLogMessage("Download completed ");
					//    if (receiptDTOs.SuccessOrders.Count > 0)
					//        appendLogMessage(receiptDTOs.SuccessOrders.Count + " receipt Downloaded Successfully.");
					//    if (receiptDTOs.FailedOrders.Count > 0)
					//        appendLogMessage(receiptDTOs.FailedOrders.Count + " receipt Downloaded failed..Try again..");
					//}
					//else
					//{
					//    appendLogMessage("No receipts found for downloading");
					//}
					//if (receiptDTOs.Count > 0)
					//{
					//    appendLogMessage("Download journal completed.");
					//}
					//else
					//{
					//    appendLogMessage("No journal found for downloading");
					//}
				}



			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Download  journal failed... \n" + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}

		private void btn_sales_download_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadSalesAsync();
			GlobalStateService.Currentstateoftask = false;

		}

		private async void downloadSalesAsync()
		{

			//LogManager.WriteLog("download optional sales started.");
			//appendLogMessage("download optional sales started.");
			try
			{

				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;
				if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase) && "true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null || employeeList.SelectedItem == null)
					{
						appendLogMessage("Cannot download Sales without selecting Employee and date");
						return;
					}
				}
				if (enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download  Sales without selecting  date");
						return;
					}
				}


				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					List<SalesOrderDTO> salesOrderDTOs = new List<SalesOrderDTO>();
					DownloadResponseDto reponseDto = new();
					appendLogMessage("Download  sales started.");
					if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase) && "true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase))
					{
						await downloadSalesService.getFromServerAndDownloadToTallyWithDateAsync(salesDate.Value, employeeList.SelectedItem.ToString(), uC_Logger);
					}
					if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase) && "true".Equals(enableselectedDateWise, StringComparison.OrdinalIgnoreCase))
					{
						await downloadSalesService.getFromServerAndDownloadToTallyWithDateAsync(salesDate.Value, employeeList.SelectedItem.ToString(), uC_Logger);
					}
					//appendLogMessage("Download  sales completed.");
					//if (reponseDto.SuccessCount > 0)
					//{
					//    appendLogMessage(reponseDto.SuccessCount + " Sales downloaded successfully...");
					//}
					//if (reponseDto.FailedCount > 0)
					//{
					//    appendLogMessage(reponseDto.FailedCount + " Sales download failed...");
					//}
					//if (reponseDto.TotalCount <= 0)
					//{
					//    appendLogMessage("No sales found for downloading.");
					//}

				}
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				MessageBox.Show(ex.Message);
				appendLogMessage("Download  sales failed... \n" + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}

		}

		private void btn_optionalReceipt_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadOptionalReceipt();
			GlobalStateService.Currentstateoftask = false;
		}

		private async void downloadOptionalReceipt()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;

				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
					//appendLogMessage("Tally and company verified.");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download receipt started.");
					DownloadResponseDto receiptDTOs = await downloadReceiptService.getFromServerAndDownloadToTallyDailyReceipt(salesDate.Value);
					if (receiptDTOs != null)
					{
						if (receiptDTOs.SuccessOrders.Count > 0)
						{
							appendLogMessage($"{receiptDTOs.SuccessOrders.Count}  receipt Downloaded successfully.");
						}
						if (receiptDTOs.FailedOrders.Count > 0)
						{
							appendLogMessage($"{receiptDTOs.FailedOrders.Count}  receipt Download failed.");
							if (receiptDTOs.failedOrdersLineErrors.Count > 0)
							{
								string joinedString = string.Join(" \n", receiptDTOs.failedOrdersLineErrors);
								appendLogMessage(joinedString);
							}

						}
						if (receiptDTOs.TotalCount <= 0)
						{
							appendLogMessage("No receipt found for downloading.");
						}
					}
					else
					{
						appendLogMessage("No receipt found for downloading.");
					}
				}

			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Download Optional Receipt Failed...\n " + ex.Message);

			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}

		private void btn_vansales_Click(object sender, EventArgs e)

		{

			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadByVoucherTypeAsync();
			GlobalStateService.Currentstateoftask = false;
			MessageBox.Show("Process Completed ");
		}

		private async Task downloadByVoucherTypeAsync()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;
				if (enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download Sales  without selecting  date");
						return;
					}
				}
				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
					//appendLogMessage("Tally and company verified.");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download  sales started.");
					await downloadByVoucherTypeService
							   .getFromServerAndDownloadToTally(VoucherType.PRIMARY_SALES, salesDate.Value, uC_Logger);
					//if (reponseDto.SuccessCount > 0)
					//{

					//    appendLogMessage(reponseDto.SuccessCount +" Sales downloaded successfully...");
					//}
					//if (reponseDto.FailedCount > 0)
					//{
					//    appendLogMessage(reponseDto.FailedCount +" Sales download failed...");
					//}
					//if (reponseDto.TotalCount <= 0)
					//{
					//    appendLogMessage("No sales found for downloading.");
					//}

					//if (salesOrderDTOs.Count > 0)
					//{
					//	appendLogMessage("Download sales completed.");
					//}
					//else
					//{
					//	appendLogMessage("No sales found for downloading.");
					//}
				}
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Downloading Vansales failed..\n " + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}
		private void btn_Sales_Retrun_Click(object sender, EventArgs e)

		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadSalesReturnAsync();
			GlobalStateService.Currentstateoftask = false;

		}
		private async Task downloadSalesReturnAsync()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;
				if ((enableselectedDateWise.Equals("true", StringComparison.OrdinalIgnoreCase)) || (enableDateWise.Equals("true", StringComparison.OrdinalIgnoreCase)))


				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download  Sales without selecting  date");
						return;
					}
				}
				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{

					appendLogMessage("Download Sales Return started.");
					downloadSalesReturnService.getFromServerAndDownloadToTallyAsync(VoucherType.PRIMARY_SALES_RETURN, salesDate.Value, uC_Logger);
				}
			}

			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Downloading Sales Return failed..\n " + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}

		}

		private void btn_SalesJournal_Click(object sender, EventArgs e)
		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadSalesJournalAsync();
			GlobalStateService.Currentstateoftask = false;
		}

		private async Task downloadSalesJournalAsync()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;
				if (enableDateWise.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					if (salesDate.Value == null)
					{
						appendLogMessage(" Cannot download Sales Order without selecting  date");
						return;
					}
				}
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
					//appendLogMessage("Tally and company verified.");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download  sales started.");
					List<SalesOrderDTO> salesOrderDTOs = await downloadSalesJournalService
							.getFromServerAndDownloadToTally(VoucherType.PRIMARY_SALES, salesDate.Value);
					if (salesOrderDTOs.Count > 0)
					{
						appendLogMessage("Download sales completed.");
					}
					else
					{
						appendLogMessage("No sales found for downloading.");
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("Downloading Sales Journal failed..\n " + ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}

		public static void setSyncOperationTypes(List<String> sotypes)
		{
			syncOperationTypes = sotypes;
		}

		private void removeNotAssynedCheckBoxes(List<SyncOperationType> allSyncOperationTypes)
		{
			//Log.Info("request for removing not assigned checkboxes");
			List<Button> selectedButtons = new List<Button>();
			var syncOperationTypes = MasterDataUploadServices.getSyncOperationTypes();
			List<string> allAssignedSyncOperations = allSyncOperationTypes.Select(a => a.ToString()).ToList();

			//allAssignedSyncOperations.RemoveAll(syncOperationTypes);
			List<string> allNotAssignedSyncOperations = allAssignedSyncOperations.Except(syncOperationTypes).ToList();

			foreach (string syncOperationType in allNotAssignedSyncOperations)
			{
				if (syncOperationType.Equals(SyncOperationType.RECEIPT.ToString()))
				{
					selectedButtons.Add(btn_downloadReceipt);
					btn_downloadReceipt.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.SALES_ORDER.ToString()))
				{
					selectedButtons.Add(btn_salesOrder);
					btn_salesOrder.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.SALES_VOUCHER.ToString()))
				{
					selectedButtons.Add(btn_sales_download);
					btn_sales_download.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.DAILY_RECEIPT.ToString()))
				{
					selectedButtons.Add(btn_optionalReceipt);
					btn_optionalReceipt.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.DOWNLOAD_ACCOUNT_PROFILE.ToString()))
				{
					selectedButtons.Add(btn_accountProfiles);
					btn_accountProfiles.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.SALES_BY_VOUCHER.ToString()))
				{
					selectedButtons.Add(btn_vansales);
					btn_vansales.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.JOURNAL.ToString()))
				{
					selectedButtons.Add(btn_Journal);
					btn_Journal.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.SALES_JOURNAL.ToString()))
				{
					selectedButtons.Add(btn_SalesJournal);
					btn_SalesJournal.Visible = false;
				}
				else if (syncOperationType.Equals(SyncOperationType.SALES_RETURN.ToString()))
				{
					selectedButtons.Add(btn_Sales_Retrun);
					btn_Sales_Retrun.Visible = false;
				}

			}


		}

		private void btn_accountProfiles_Click(object sender, EventArgs e)







		{
			ClearLoggerArea();
			GlobalStateService.Currentstateoftask = true;
			downloadLedgersAsync();
			GlobalStateService.Currentstateoftask = false;
		}

		private async Task downloadLedgersAsync()
		{
			try
			{
				ParentForm.Cursor = Cursors.WaitCursor;
				this.Cursor = Cursors.WaitCursor;

				//appendLogMessage("Verifying tally and compnay...");
				if (!await checkTallyCompanyIsOpened())
				{
					MessageBox.Show("Please ensure company is open in tally ");
				}
				else
				{
					//appendLogMessage("Tally and company verified.");
					appendLogMessage("Download Account Profiles started.");
					List<AccountProfileDTO> accountProfileDTOs = new List<AccountProfileDTO>();

					accountProfileDTOs = await downloadLedgerService
								.SendAllLedgerToTally();

					if (accountProfileDTOs.Count > 0)
					{
						appendLogMessage("Download Ledger completed.");
					}
					else
					{
						appendLogMessage("No Ledger found for downloading.");
					}

				}
			}
			catch (ServiceException ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("downloading sales order failed .\n " + ex.Message);
				//appendLogMessage(ex.Message);
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				appendLogMessage("downloading sales order failed .");
				//appendLogMessage(ex.Message);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
				this.Cursor = Cursors.Default;
			}
		}

		public static void showMessage(string message)
		{
			MessageBoxIcon icon = MessageBoxIcon.Information;
			MessageBox.Show(message, "SalesNrich", 0, MessageBoxIcon.Information);
		}
		public static void showMessageToMasterUpdate(string message)
		{
			//LedgersListDisplayForm ledgersListDisplayForm = new LedgersListDisplayForm(message);
			//ledgersListDisplayForm.ShowDialog();

			MessageBoxIcon icon = MessageBoxIcon.Warning;
			var res = MessageBox.Show(message + "\n Do you want to update master data and continue .",
				"SalesNrich", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			if (res == DialogResult.Yes)
			{
				UC_UserHome home = new UC_UserHome();
				home.LedgerMasterUpload();

			}
		}

		private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
