using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class DownloadServices
    {
        private static List<String> syncOperationTypes;

        public static void setSyncOperationTypes(List<String> syncOperationTypes)
        {
            DownloadServices.syncOperationTypes = syncOperationTypes;
        }
    }
}
