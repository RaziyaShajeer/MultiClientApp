using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SNR_ClientApp.DTO
{
    public class DropdownDTO
    {
        public String name { get; set; }
        public double value { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
