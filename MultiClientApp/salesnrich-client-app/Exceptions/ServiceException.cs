using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Exceptions
{
    public class ServiceException: Exception
    {
        private static readonly long serialVersionUID = 1L;

        public ServiceException(string message) : base(message)
        {

        }
        public ServiceException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
