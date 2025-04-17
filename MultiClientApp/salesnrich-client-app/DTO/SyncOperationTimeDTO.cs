using SNR_ClientApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class SyncOperationTimeDTO
    {
        public SyncOperationType operationType { get; set; }
        public String lastSyncStartedDate { get; set; } 
        public String lastSyncCompletedDate  { get; set; }
        public double lastSyncTime { get; set; }
        public bool completed { get; set; }
    }
}
