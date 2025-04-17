using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class DistributedDto
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("accountName")]
        public string accountName { get; set; } 
    }
}
