using dtos;

using Newtonsoft.Json.Linq;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNR_ClientApp.Parsers
{
    public class SalesVoucherTallyMasterResponceMaster
    {
        internal List<InventoryVoucherHeaderDTO> parseSalesVoucherListXml(TallyRequestResponse res2)
        {
            try
            {
                int count = 0;
                double totalAmount = 0;
                List<InventoryVoucherHeaderDTO> inventoryVoucherHeaderDTOs = new List<InventoryVoucherHeaderDTO>();
                List<InventoryVoucherDetailDTO> inventoryVoucherDetailDTOs = new List<InventoryVoucherDetailDTO>();
                InventoryVoucherHeaderDTO inventoryVoucherHeaderDTO = new InventoryVoucherHeaderDTO();
           
                foreach (var voucher in res2.response.VOUCHERS)
                {
                    count++;
                   
                    string vdate = voucher.VOUCHERDATE;
                    DateTime vDateTime = StringUtilsCustom.ConvertFormat(vdate);
                    inventoryVoucherHeaderDTO.documentDate = vDateTime.ToString("yyyy-MM-ddTHH:mm"); 
                    inventoryVoucherHeaderDTO.documentNumberLocal = voucher.VOUCHERNUMBER.ToString();
                    inventoryVoucherHeaderDTO.documentNumberServer = voucher.VOUCHERNUMBER.ToString();
                    inventoryVoucherHeaderDTO.referenceDocumentType = voucher.VTYPENAME.ToString();
               
                  
                    List<VAT> VatList = voucher.VAT;
                    VatList.ForEach(vat =>
                    {

                        InventoryVoucherDetailDTO inventoryVoucherDetailDTO = new InventoryVoucherDetailDTO();
                        inventoryVoucherDetailDTO.productName = vat.INVNAME;
                        Double spliyValueBtDot = (!String.IsNullOrEmpty(vat.INVAMOUNT)) ? StringUtilsCustom.ExtractDoubleValue(vat.INVAMOUNT) : 0;
                        inventoryVoucherDetailDTO.rowTotal = spliyValueBtDot;
                        totalAmount += spliyValueBtDot;
                        decimal actualQty;

                        if (!String.IsNullOrEmpty(vat.ACTUALQTY))
                        {
                            MatchCollection matches = Regex.Matches(vat.ACTUALQTY, @"\d+(\.\d+)?");
                            inventoryVoucherDetailDTO.quantity = (matches.Count > 0) ? StringUtilsCustom.ExtractDoubleValue(matches[0].Value) : 0;

                        }
                      

                        //String[] sellingRate = vat.ACTUALQTY.Split("/");

                        //String[] actualSellingRate = vat.INVRATE.Split("/");
                        if (vat.VATLEDGERNAME != null)
                        {
                            inventoryVoucherHeaderDTO.receiverAccountName = vat.VATLEDGERNAME.ToString();

                        }
                        

                        inventoryVoucherHeaderDTO.salesLedgerName = (!String.IsNullOrEmpty(vat.SALESLEDGER)) ? vat.SALESLEDGER : inventoryVoucherHeaderDTO.salesLedgerName;

                        if (!String.IsNullOrEmpty(vat.VATAMOUNT))
                        {
                            if (count == 1)
                            {
                                inventoryVoucherHeaderDTOs = new List<InventoryVoucherHeaderDTO>();
                                if (inventoryVoucherDetailDTOs.Count > 0)
                                {
                                    inventoryVoucherHeaderDTO.inventoryVoucherDetails = inventoryVoucherDetailDTOs;
                                }

                                inventoryVoucherHeaderDTOs.Add(inventoryVoucherHeaderDTO);
                                String spliyValueBtDot1 = vat.VATAMOUNT.Replace(",", "");
                                double toatalAmount = StringUtilsCustom.ExtractDoubleValue(spliyValueBtDot1);
                                double documentTotal = inventoryVoucherHeaderDTOs[0].documentTotal;
                                inventoryVoucherHeaderDTOs[0].documentTotal = (documentTotal + toatalAmount);
                                inventoryVoucherHeaderDTOs[0].documentTotal = totalAmount;

                            }
                        }

                        if (!String.IsNullOrEmpty(vat.INVRATE))
                        {
                            MatchCollection actualSellingRate = Regex.Matches(vat.INVRATE, @"\d+(\.\d+)?");
                            inventoryVoucherDetailDTO.sellingRate = (actualSellingRate.Count > 0) ? StringUtilsCustom.ExtractDoubleValue(actualSellingRate[0].Value) : 0;
                            if (count == 1)
                            {
                              

                                inventoryVoucherDetailDTOs.Add(inventoryVoucherDetailDTO);
                                inventoryVoucherDetailDTO = new InventoryVoucherDetailDTO();
                            }
                        }
                   


                    });
                    //  inventoryVoucherHeaderDTOs.Add(inventoryVoucherHeaderDTO);
                    // String spliyValueBtDot = res2.response.VOUCHERS.VAT.ElementAtOrDefault(0).VATAMOUNT.Replace(",", "");
                    //  double toatalAmount = StringUtilsCustom.ExtractDoubleValue(spliyValueBtDot);
                    //double documentTotal = accountingVoucherHeaderDTOs[0].totalAmount;
                    //  accountingVoucherHeaderDTOs[0].totalAmount = (documentTotal + toatalAmount);

                }


                    return inventoryVoucherHeaderDTOs;
                
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("Exception occured while parsing SalesVoucherList :" + ex.Message);
                LogManager.WriteLog(ex.StackTrace);
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
