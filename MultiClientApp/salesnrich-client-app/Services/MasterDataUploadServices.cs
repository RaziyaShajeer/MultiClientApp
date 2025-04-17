using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class MasterDataUploadServices
    {
        private static List<String> syncOperationTypes;

        public static void setSyncOperationTypes(List<String> _syncOperationTypes)
        {
            MasterDataUploadServices.syncOperationTypes = _syncOperationTypes;
        }
        public static List<string> getSyncOperationTypes()
        {
           return syncOperationTypes;
        }
    }
}
