using Newtonsoft.Json.Linq;
using SNR_ClientApp.DTO;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Parsers
{
    public class ReceiptVoucherTallyMasterResponceMaster
    {
        internal List<AccountingVoucherHeaderDTO> parseSalesVoucherListXml(TallyRequestResponse res2)
        {
            try
            {
                if (res2.response.VOUCHERS.Count > 0)
                {


                    int count = 0;

                    List<AccountingVoucherHeaderDTO> accountingVoucherHeaderDTOs = new List<AccountingVoucherHeaderDTO>();
                    List<AccountingVoucherDetailDTO> accountingVoucherDetailDTOs = new List<AccountingVoucherDetailDTO>();
                    AccountingVoucherHeaderDTO accountingVoucherHeaderDTO = new AccountingVoucherHeaderDTO();
                    AccountingVoucherDetailDTO accountingVoucherDetailDTO = new AccountingVoucherDetailDTO();
                    string vdate = res2.response.VOUCHERS.ElementAtOrDefault(0).VOUCHERDATE;
                    DateTime vDateTime = StringUtilsCustom.ConvertFormat(vdate);
                    accountingVoucherHeaderDTO.documentDate = vDateTime.ToString("yyyy-MM-ddTHH:mm");
                    accountingVoucherHeaderDTO.documentNumberLocal = res2.response.VOUCHERS.ElementAtOrDefault(0).VOUCHERNUMBER.ToString();
                    accountingVoucherHeaderDTO.documentNumberServer = res2.response.VOUCHERS.ElementAtOrDefault(0).VOUCHERNUMBER.ToString();
                    accountingVoucherHeaderDTO.accountProfileName = res2.response.VOUCHERS.ElementAtOrDefault(0).VAT.ElementAtOrDefault(0).VATLEDGERNAME.ToString();

                    accountingVoucherHeaderDTOs.Add(accountingVoucherHeaderDTO);
                    String spliyValueBtDot = res2.response.VOUCHERS.ElementAtOrDefault(0).VAT.ElementAtOrDefault(0).VATAMOUNT.Replace(",", "");
                    double toatalAmount = StringUtilsCustom.ExtractDoubleValue(spliyValueBtDot);
                    double documentTotal = accountingVoucherHeaderDTOs[0].totalAmount;
                    accountingVoucherHeaderDTOs[0].totalAmount = (documentTotal + toatalAmount);



                    return accountingVoucherHeaderDTOs;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("Exception occured while parsing SalesVoucherList :"+ ex.Message);
                LogManager.WriteLog(ex.StackTrace);
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
