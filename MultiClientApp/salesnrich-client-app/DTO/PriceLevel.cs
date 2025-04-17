using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class PriceLevel
    {

        public String priceLevelName { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
        public string productId { get;  set; }
        //todo check with server 
        //added for getting discount
        public double discount { get; set; }
    }
}
