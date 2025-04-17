using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PriceLevelListDTO
    {
        public static readonly long serialVersionUID = 1L;
        public String productId { get; set; }
        public String productProfileName { get; set; }

        public String priceLevelName { get; set; }

        public double price { get; set; }

        public DateTime date { get; set; }

        public double rangeFrom { get; set; }

        public double rangeTo { get; set; }
        //todo check with server 
        //added for getting discount
        public double discount { get; set; }

        public PriceLevelListDTO()
        {

        }
        public PriceLevelListDTO(String productProfileName, String priceLevelName, double price, DateTime date,
        double rangeFrom, double rangeTo)
        {
           
            this.productProfileName = productProfileName;
            this.priceLevelName = priceLevelName;
            this.price = price;
            this.date = date;
            this.rangeFrom = rangeFrom;
            this.rangeTo = rangeTo;
        }
    }
}
