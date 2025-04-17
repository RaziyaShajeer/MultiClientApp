using System; 
namespace dtos{ 

    public class AccountProfileDTO2
    {
        public object alterId { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public object alias { get; set; }
        public string userPid { get; set; }
        public string userName { get; set; }
        public object description { get; set; }
        public string accountTypePid { get; set; }
        public string accountTypeName { get; set; }
        public object gstRegistrationType { get; set; }
        public double locationRadius { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public object location { get; set; }
        public object pin { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public object phone1 { get; set; }
        public object phone2 { get; set; }
        public object email1 { get; set; }
        public object email2 { get; set; }
        public object whatsAppNo { get; set; }
        public object defaultPriceLevelPid { get; set; }
        public object defaultPriceLevelName { get; set; }
        public string accountStatus { get; set; }
        public bool importStatus { get; set; }
        public int creditDays { get; set; }
        public int creditLimit { get; set; }
        public object contactPerson { get; set; }
        public bool activated { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public DateTime createdDate { get; set; }
        public object leadToCashStage { get; set; }
        public string tinNo { get; set; }
        public double closingBalance { get; set; }
        public double defaultDiscountPercentage { get; set; }
        public object trimChar { get; set; }
        public bool hasDefaultAccountInventory { get; set; }
        public bool promptStockLocationInventory { get; set; }
        public string dataSourceType { get; set; }
        public object stateName { get; set; }
        public object countryName { get; set; }
        public object districtName { get; set; }
        public object geoTaggingType { get; set; }
        public object geoTaggedTime { get; set; }
        public object geoTaggedUserName { get; set; }
        public object geoTaggedUserPid { get; set; }
        public object geoTaggedUserLogin { get; set; }
        public string customerId { get; set; }
        public object countryId { get; set; }
        public object customerCode { get; set; }
        public object stateId { get; set; }
        public object districtId { get; set; }
        public object mailingName { get; set; }
        public object employeeName { get; set; }
        public object aitrichCode { get; set; }
        public bool isImportStatus { get; set; }
    }

}