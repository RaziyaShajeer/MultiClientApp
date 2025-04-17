using SNR_ClientApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class BackgroundServiceforupload
    {
        private static BackgroundServiceforupload _instance;
        private System.Threading.Timer _timer;
        public static BackgroundServiceforupload Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BackgroundServiceforupload();
                }
                return _instance;
            }
        }
        
      
        public void StopBackgroundTask()
        {
            _timer?.Dispose();

            _timer = null;

        }


    }
}
