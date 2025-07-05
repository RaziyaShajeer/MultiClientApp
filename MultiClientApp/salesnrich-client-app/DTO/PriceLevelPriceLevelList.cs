using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PriceLevelPriceLevelList
    {
        public List<PriceLevelDTO> priceLevelDtos { get; set; }

        public List<PriceLevelListDTO> priceLevelListDtos { get; set; }
		public string tallyCompanyName { get; set; }
		public PriceLevelPriceLevelList()
		{
			tallyCompanyName = ApplicationProperties.properties["tally.company"].ToString();
		}
		public PriceLevelPriceLevelList(List<PriceLevelDTO> priceLevelDtos, List<PriceLevelListDTO> priceLevelListDtos)
        {
         
            this.priceLevelDtos = priceLevelDtos;
            this.priceLevelListDtos = priceLevelListDtos;
		
		}
    }
}
