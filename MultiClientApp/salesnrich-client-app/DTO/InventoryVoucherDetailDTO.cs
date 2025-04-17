using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class InventoryVoucherDetailDTO
    {
        public long detailId{get;set;}

        public String productPid{get;set;}

        public String productName{get;set;}

        public double quantity{get;set;}

        public double freeQuantity{get;set;}

        public double sellingRate{get;set;}

        public double mrp{get;set;}

        public double purchaseRate{get;set;}

        public double taxPercentage{get;set;}

        public double discountPercentage{get;set;}

        public String batchNumber{get;set;}

        public DateTime? batchDate { get; set; } = null;

        public double rowTotal{get;set;}

        public double discountAmount{get;set;}

        public double taxAmount{get;set;}

        public String length{get;set;}

        public String width{get;set;}

        public String thickness{get;set;}

        public String size{get;set;}

        public String color{get;set;}
        public string productdate { get;set;}

        public String sourceStockLocationPid{get;set;}

        public String sourceStockLocationName{get;set;}

        public String destinationStockLocationPid{get;set;}

        public String destinationStockLocationName{get;set;}

        public String referenceInventoryVoucherHeaderPid{get;set;}

        public long referenceInventoryVoucherDetailId{get;set;}

        public String remarks{get;set;}


        public List<InventoryVoucherBatchDetailDTO> inventoryVoucherBatchDetails{get;set;}

    }
}
