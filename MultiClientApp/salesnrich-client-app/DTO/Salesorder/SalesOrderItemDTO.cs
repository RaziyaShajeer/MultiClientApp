using System.Collections.Generic; 
using System;
using SNR_ClientApp.DTO;

namespace dtos{ 

    public class SalesOrderItemDTO
    {
        public long itemId{get;set;}
        public String pid{get;set;}
        public String itemName{get;set;}
        public String unit{get;set;}
        public double itemDiscount{get;set;}
        public double rateOfVat{get;set;}
        public double itemRate{get;set;}
        public double itemStock{get;set;}
        public double quantity{get;set;}
        public double itemFreeQuantity{get;set;}
        public double sellingRate{get;set;}
        public double mrp{get;set;}
        public double taxPercentage{get;set;}
        public double discountPercentage{get;set;}
        public double rowTotal{get;set;}
        public double discountAmount{get;set;}
        public double? taxAmount{get;set;}
        public String batchDate{get;set;}
        public ProductProfileTaxMasterDTO productProfileDTO{get;set;}
        public List<OpeningStockDTO> openingStockDTOs{get;set;}

        public long detailId{get;set;}
        public String productPid{get;set;}
        public String productName{get;set;}
        public double? freeQuantity{get;set;}
        public double? purchaseRate{get;set;}
        public String batchNumber{get;set;}
        public String length{get;set;}
        public String width{get;set;}
        public String thickness{get;set;}
        public String size{get;set;}
        public String color{get;set;}
        public String sourceStockLocationPid{get;set;}
        public String sourceStockLocationName{get;set;}
        public String destinationStockLocationPid{get;set;}
        public String destinationStockLocationName{get;set;}
        public String referenceInventoryVoucherHeaderPid{get;set;}
        public long? referenceInventoryVoucherDetailId{get;set;}
        public String remarks{get;set;}
        public String trimChar{get;set;}
        public String stockLocationName{get;set;}

        public double cessRateOfVat { get;set;}
        public string alias { get; set; }
    }

}