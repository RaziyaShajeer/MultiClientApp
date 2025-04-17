using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class DynamicDocumentHeaderDTO
    {
        public String pid{get;set;}

        public String documentNumberLocal{get;set;}

        public String documentNumberServer{get;set;}

        public String documentPid{get;set;}

        public String documentName{get;set;}

        public DateTime createdDate{get;set;}

        public DateTime documentDate{get;set;}

        public String employeePid{get;set;}

        public String employeeName{get;set;}

        public String userName{get;set;}

        public String userPid{get;set;}

        public String activityName{get;set;}

        public String accountName{get;set;}

        public List<DynamicDocumentHeaderDTO> history{get;set;}

        public String emplyeePhone{get;set;}

        public String accountAddress{get;set;}

        public String accountPhone{get;set;}

        public String accountEmail{get;set;}

        //only used to store comma separated names for print file path.
        public String printEmailDocumentNames{get;set;}
    }
}
