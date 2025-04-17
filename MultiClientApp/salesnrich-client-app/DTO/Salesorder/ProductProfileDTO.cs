using System.Collections.Generic; 
using System; 
namespace dtos{ 

    public class ProductProfileDTO
    {
        public string pid { get; set; }
        public object alterId { get; set; }
        public string name { get; set; }
        public object alias { get; set; }
        public object description { get; set; }
        public double price { get; set; }
        public double mrp { get; set; }
        public string sku { get; set; }
        public double unitQty { get; set; }
        public double compoundUnitQty { get; set; }
        public double taxRate { get; set; }
        public double discountPercentage { get; set; }
        public string productCategoryPid { get; set; }
        public string productCategoryName { get; set; }
        public string divisionPid { get; set; }
        public string divisionName { get; set; }
        public string colorImage { get; set; }
        public string colorImageContentType { get; set; }
        public object size { get; set; }
        public string filesPid { get; set; }
        public bool activated { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public string defaultLedger { get; set; }
        public string unitsPid { get; set; }
        public string unitsName { get; set; }
        public List<object> productProfileTaxMasterDTOs { get; set; }
        public string stockAvailabilityStatus { get; set; }
        public object trimChar { get; set; }
        public string hsnCode { get; set; }
        public object productDescription { get; set; }
        public object barcode { get; set; }
        public object remarks { get; set; }
        public string productId { get; set; }
        public object productCode { get; set; }
        public object createdDate { get; set; }
        public object stockLocationName { get; set; }
        public double stockQty { get; set; }
        public object productGroup { get; set; }
        public double purchaseCost { get; set; }
        public object uploadSource { get; set; }
        public double cessTaxRate { get; set; }
        public double itemWidth { get; set; }
        public double sellingRate { get; set; }
        public object baseUnits { get; set; }
        public double igst { get; set; }
        public double rateConversion { get; set; }
    }

}