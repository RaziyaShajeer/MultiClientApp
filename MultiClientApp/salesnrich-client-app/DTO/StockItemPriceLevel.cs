using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class StockItemPriceLevel
    {
        public String productName { get; set; }
        public List<PriceLevel> priceLevels = new List<PriceLevel>();
    }
}
