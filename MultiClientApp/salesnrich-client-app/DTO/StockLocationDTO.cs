using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{ 
    public class StockLocationDTO
    {
        public String name { get; set; }
      
   
        public StockLocationType stockLocationType { get; set; }
        public string displayName { get; set; }

        public bool activated  = true;
        public string distributorCode { get; set; }
        public string distributorName { get;set; }
        public StockLocationDTO(String name)
        {
      this.name= DistributedCodeAppend.appendDistributedCode(name);
       this.displayName=name;                         
       this.stockLocationType = StockLocationType.ACTUAL;
       this.activated = true;
       this.distributorCode=ApplicationProperties.properties["DistributedCode"].ToString();
      this.distributorName=ApplicationProperties.properties["DistributedCodeCompany"].ToString();
        }
        public StockLocationDTO ()
        {

        }

       
          

    }
}
