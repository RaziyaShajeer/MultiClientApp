using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PostDatedVoucherAllocationDTO
    {
        public String allocReferenceVoucher { get; set; }

        public double allocReferenceVoucherAmount { get; set; }

        public String voucherNumber { get; set; }

        public PostDatedVoucherAllocationDTO()
        {

        }

        public PostDatedVoucherAllocationDTO(PostDatedVoucherAllocationDTO dto)
        {
            this.allocReferenceVoucher = dto.allocReferenceVoucher;
            this.allocReferenceVoucherAmount = dto.allocReferenceVoucherAmount;
            this.voucherNumber = dto.voucherNumber;
        }

    }
}
