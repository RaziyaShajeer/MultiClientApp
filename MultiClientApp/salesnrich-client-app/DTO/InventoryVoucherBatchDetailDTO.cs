using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class InventoryVoucherBatchDetailDTO
    {

        public long inventoryVoucherDetailId{get;set;}

        public String productProfilePid{get;set;}

        public String productProfileName{get;set;}

        public String batchNumber{get;set;}

        public String batchDate{get;set;}

        public String remarks{get;set;}

        public double quantity{get;set;}

        public String stockLocationPid{get;set;}

        public String stockLocationName{get;set;}
    }
}
