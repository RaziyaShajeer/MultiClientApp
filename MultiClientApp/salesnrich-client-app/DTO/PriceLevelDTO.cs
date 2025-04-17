using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PriceLevelDTO
    {
        public String name;
        public bool activated = true;
        //Todo : need to very that productId is required in this dto
       // public String productId { get; set; }

        public PriceLevelDTO(String name, bool activated)
        {
            
            this.name = name;
            this.activated = activated;
        }
    }
}
