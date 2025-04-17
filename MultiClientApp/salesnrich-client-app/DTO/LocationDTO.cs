using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class LocationDTO
    {
        public long alterId { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public bool activated = true;

        public LocationDTO()
        {

        }
        public LocationDTO(String name, String description)
        {
           
            this.name = name;
            this.description = description;
            this.activated = true;
        }

        public String locationId { get; set; }
    }
}
