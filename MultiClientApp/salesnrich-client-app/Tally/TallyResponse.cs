using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally
{
    public class TallyResponse
    {
        public TallyResponse()
        {

        }
		public TallyResponse(string v1, string v2,Object DownloadResponse)
		{
			this.status = v1;
			this.description = v2;
			this.body = DownloadResponse;
		}


		public TallyResponse(string v1, string v2, List<string> successReceipts)
        {
            this.status = v1;
            this.description = v2;
            this.body = successReceipts;
        }

        public String status { get; set; }

        public String description { get; set; }

        public Object body { get; set; }

    }
}
