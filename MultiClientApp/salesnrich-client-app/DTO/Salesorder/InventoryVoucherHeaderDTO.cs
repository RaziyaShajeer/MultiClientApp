using SNR_ClientApp.DTO;
using System; 
namespace dtos{ 

    public class InventoryVoucherHeaderDTO
    {
        public String pid{get;set;}
        public String documentNumberLocal{get;set;}
        public String documentNumberServer{get;set;}
        public String documentPid{get;set;}
        public String documentName{get;set;}
        public String createdDate{get;set;}
        public String documentDate{get;set;}
        public String receiverAccountPid{get;set;}
        public String receiverAccountName{get;set;}
        public String supplierAccountPid{get;set;}
        public String supplierAccountName{get;set;}
        public String employeePid{get;set;}
        public String employeeName{get;set;}
        public String userName{get;set;}
        public double documentTotal{get;set;}
        public double? documentVolume{get;set;}
        public double? docDiscountPercentage{get;set;}
        public double? docDiscountAmount{get;set;}
        public Boolean status{get;set;}
        public String priceLevelPid{get;set;}
        public String priceLevelName{get;set;}
        public String referenceDocumentNumber{get;set;}
        public String referenceDocumentType{get;set;}
        public List<InventoryVoucherDetailDTO> inventoryVoucherDetails{get;set;}
        public String salesLedgerName{get;set;}
     
    }

}