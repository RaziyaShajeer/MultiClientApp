using System.Collections.Generic; 
using System;

using SNR_ClientApp.DTO;

namespace dtos{ 

    public class SalesOrderDTO
    {
        public int id { get; set; }
        public string inventoryVoucherHeaderPid { get; set; }
        public string ledgerName { get; set; }
        public string ledgerAddress { get; set; }
        public string ledgerState { get; set; }
        public string ledgerCountry { get; set; }
        public String ledgerGstType { get; set; }
        public string ledgerPinCode { get; set; }
        public string priceLevel { get; set; }
        public String narration { get; set; }
        public string selectedVats { get; set; }
        public string tabOrderNumber { get; set; }
        public string documentName { get; set; }
        public string documentAlias { get; set; }
        public double? docDiscountAmount { get; set; }
        public double? docDiscountPercentage { get; set; }
        public string trimChar { get; set; }
        public string employeeAlias { get; set; }
        public string mailingName { get; set; }

        public bool isVat { get; set; }
        public bool isDiscount { get; set; }
        public bool isFreeQuantity { get; set; }
        public bool isDiffQuantity { get; set; }
        public bool isSplitByVat { get; set; }
        public bool isPriceEditable { get; set; }

        public AccountProfileDTO accountProfileDTO { get; set; }
        public List<SalesOrderItemDTO> salesOrderItemDTOs { get; set; }
        public List<VatLedgerDTO> vatLedgerDTOs { get; set; }
        public string date { get; set; }
        public List<DynamicDocumentHeaderDTO> dynamicDocumentHeaderDTOs { get; set; }
        public InventoryVoucherHeaderDTO inventoryVoucherHeaderDTO { get; set; }
        public List<GstLedgerDTO> gstLedgerDtos { get; set; }
        public string activityRemarks { get; set; }
        public string godownName { get; set; }

        //for vansales
        //test code :13/3/2024
        public string vehicleType { get; set; }
        public string vehicleNo { get; set; }
        public string vehicleDriver { get; set; }
        public string salesOrderRefNumber { get; set; }    
        public decimal discountamout { get; set; }  
    }

}