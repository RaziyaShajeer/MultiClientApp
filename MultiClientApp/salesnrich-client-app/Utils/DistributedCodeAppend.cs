using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    public static class DistributedCodeAppend
    {
        public static string appendDistributedCode(string name)
        {
            if (ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (ApplicationProperties.properties["IsEnableDistributor"].ToString()!=null)
                {
                    LogManager.WriteLog("Append String:"+ApplicationProperties.properties["IsEnableDistributor"].ToString());
                    string appendedCode = name+"-"+  ApplicationProperties.properties["DistributedCode"].ToString();
                    return appendedCode;
                }
                else
                {
                    return name;
                }
          
            }
            else
            {
                return name;
            }
            
        }
    }
}
