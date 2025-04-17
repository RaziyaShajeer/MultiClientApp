using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SNR_ClientApp.Tally.generateXml
{
    internal class CompanyXml
    {
        public CompanyXml() { }

        public string getActiveCompanyRequestXml() {
        ENVELOPE envelope=new ENVELOPE();
            HEADER header = new();
            header.VERSION = "1";
            header.TALLYREQUEST="Export";
            header.TYPE ="Collection";
            header.ID="CompanyInfo";
            envelope.HEADER = header;

            BODY body = new();
            DESC dESC = new DESC();
            TDL tdl=new TDL();
            TDLMESSAGE tDLMESSAGE = new TDLMESSAGE();
            OBJECT obj = new OBJECT();
            obj.NAME="CurrentCompany";
            obj.LOCALFORMULA="CurrentCompany:##SVCURRENTCOMPANY";
            tDLMESSAGE.OBJECT = obj;
            COLLECTION collection=new COLLECTION();
            collection.NAME="CompanyInfo";
            collection.OBJECTS="CurrentCompany";
            tDLMESSAGE.COLLECTION=new();
            tDLMESSAGE.COLLECTION.Add(collection);
            tdl.TDLMESSAGE=tDLMESSAGE;
            dESC.TDL=tdl;
            body.DESC=dESC;
            envelope.BODY = body;

            //return envelope;

            var stringwriter2 = new System.IO.StringWriter();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(envelope.GetType());
            serializer.Serialize(stringwriter2, envelope);

            return stringwriter2.ToString();

        }
    }
}
