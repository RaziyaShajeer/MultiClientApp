using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class LocationHierarchyDTO
    {
        public String locationName { get; set; }

        public String parentName { get; set; }

        public LocationHierarchyDTO(LocationDTO locationDTO)
        {
            this.locationName = locationDTO.name;
            this.parentName = locationDTO.description;
        }
    }
}
