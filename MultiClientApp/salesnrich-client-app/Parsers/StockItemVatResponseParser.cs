using SNR_ClientApp.DTO;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Parsers
{
    public class StockItemVatResponseParser
    {
        public List<ProductProfileDTO> parseProductWiseDefaultLedgerListXml(TallyRequestResponse data)
        {
            List<ProductProfileDTO> productProfileDTOs = new List<ProductProfileDTO>();
           var stockitems=data.response?.BODY?.DATA?.COLLECTION?.STOCKITEM;
            foreach (var item in stockitems)
            {
                ProductProfileDTO productProfileDTO = new ProductProfileDTO();
                productProfileDTO.name=item?.NAME;
                productProfileDTO.defaultLedger=item?.SALESLISTLIST?.NAME?.Text;
                productProfileDTO.productId=item?.GUID?.text;
                productProfileDTO.description=item?.PARENT?.Text;
                productProfileDTOs.Add(productProfileDTO);
            }

            return productProfileDTOs;
        }
    }
}
