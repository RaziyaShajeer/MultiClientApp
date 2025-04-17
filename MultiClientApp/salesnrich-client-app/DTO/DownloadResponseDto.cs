using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class DownloadResponseDto
    {
        public DownloadResponseDto() { }
        public DownloadResponseDto(List<String> successOrders, List<String> failedOrders)
        {
            SuccessOrders=successOrders;
            FailedOrders=failedOrders;
        }
        public int SuccessCount { get;set; }
        public int FailedCount { get; set; }
        public int TotalCount { get; set; }
        public List<String> SuccessOrders { get; set; } = new();
        public List<String> FailedOrders { get; set; } = new();
		public bool isLedgerMissmatch { get; set; } = false;
		public List<String> failedOrdersLineErrors { get; set; } = new List<String>();


	}
}
