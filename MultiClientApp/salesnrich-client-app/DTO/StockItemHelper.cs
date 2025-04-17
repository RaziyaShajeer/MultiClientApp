using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class StockItemHelper
    {
        public StockItemHelper() { }
        public string mrpRate {  get; set; }
        public List<RATEDETAILSLIST> RATEDETAILSLIST { get; set; } = new();

        public StockItemHelper(List<RATEDETAILSLIST> _RATEDETAILSLIST,string _mrpRate)
        {
            RATEDETAILSLIST=_RATEDETAILSLIST;
            mrpRate= _mrpRate;
        }
    }
}
