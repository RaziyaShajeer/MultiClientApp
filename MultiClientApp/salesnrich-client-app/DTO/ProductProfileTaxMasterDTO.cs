using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ProductProfileTaxMasterDTO
    {
        private static readonly long serialVersionUID = 1L;

        public String name {get;set;}

        public String alias {get;set;}

        public String description {get;set;}

        public double? price {get;set;}

        public String sku {get;set;}

        public double? unitQty {get;set;}

        public double? taxRate  {get;set;}

        public String productCategoryName {get;set;}

        public String divisionName {get;set;}

        public String size {get;set;}

        public bool activated = true;

        public double? mrp { get;set;}

        public String defaultLedger { get;set;}

        public List<TaxMasterDTO>? productProfileTaxMasterDTOs { get; set;}

        public ProductProfileTaxMasterDTO(ProductProfileDTO dto)
        {
            if (dto != null)
            {
                this.name = dto.name;
                this.alias = dto.alias;
                this.description = dto.description;
                this.price = dto.price;
                this.sku = dto.sku;
                this.unitQty = dto.unitQty;
                this.taxRate = dto.taxRate;
                this.productCategoryName = dto.productCategoryName;
                this.divisionName = dto.divisionName;
                this.size = dto.size;
                this.activated = dto.activated;
                this.mrp = dto.mrp;
                this.defaultLedger = dto.defaultLedger;
            }
        }
    }
}
