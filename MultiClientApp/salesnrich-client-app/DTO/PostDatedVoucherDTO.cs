using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PostDatedVoucherDTO
    {
		public long id { get; set; }

		public String pid { get; set; }

		public String accountProfilePid { get; set; }

		public String accountProfileName { get; set; }

		public String referenceVoucher { get; set; }

		public String referenceDocumentNumber { get; set; }

		public String referenceDocumentDate { get; set; }

		public double referenceDocumentAmount { get; set; }

		public String remark { get; set; }

		public List<PostDatedVoucherAllocationDTO> postDatedVoucherAllocationList { get; set; }

		public String instrumentNumber { get; set; }

		public String instrumentDate { get; set; }

		public String pdcReceiptDate { get; set; }
	}
}
