using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ProductProfileDTO
    {

        public double alterId { get; set; }

        public String name{ get; set; }

        public String alias{ get; set; }

        public String description{ get; set; }

        public double price { get; set; }

        public String sku{ get; set; }

        public Double unitQty{ get; set; }

        public double taxRate{ get; set; }

        public String productCategoryName{ get; set; }

        public String divisionName{ get; set; }

        public String size{ get; set; }

        public bool activated = true;

        public double mrp{ get; set; }

        public String defaultLedger{ get; set; }

        public List<TaxMasterDTO> productProfileTaxMasterDTOs{ get; set; }

        public String trimChar{ get; set; }

        public String hsnCode{ get; set; }

        public String productDescription{ get; set; }

        public String barcode{ get; set; }

        public String remarks{ get; set; }

        public String productId{ get; set; }
        public double cessTaxRate { get; set; }

    }
}
