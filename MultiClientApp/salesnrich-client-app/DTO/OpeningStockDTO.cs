using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class OpeningStockDTO
    {
        public String productId { get;set; }
        public String batchNumber { get; set; }
        public String Displayname { get; set; }
        public String stockLocationName { get; set; }
        public string distributorCode { get; set; }
        public String productProfileName { get; set; }

        public int? quantity { get; set; }

        public DateTime createdDate { get; set; }

        public DateTime openingStockDate { get; set; }
    }
}
