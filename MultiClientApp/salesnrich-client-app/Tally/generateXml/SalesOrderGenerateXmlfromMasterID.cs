using dtos;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
    public class SalesOrderGenerateXmlfromMasterID
    {
        public async Task<ENVELOPE> GenerateSalesOrderVoucherNumberfromMastrerId(string masterId)
        {
            
                string prefix = "";

                ENVELOPE envelope = new ENVELOPE();
                HEADER header = new HEADER();
                header.VERSION = "1";
                header.TALLYREQUEST = "Export";
                header.TYPE = "Collection";
                header.ID = "Vch Collection";
                envelope.HEADER = header;

                BODY body = new BODY();
                DESC desc = new DESC();
                STATICVARIABLES staticVariables = new STATICVARIABLES();

                staticVariables.SVEXPORTFORMAT = "$$SysName:XML";
                desc.STATICVARIABLES = staticVariables;
                TDL tdl = new TDL();
                TDLMESSAGE tdlmessage = new TDLMESSAGE();
                COLLECTION collection = new();
                collection.NAME = "Vch Collection";
                collection.ISMODIFY = "No";
                collection.ISFIXED = "No";
                collection.ISINITIALIZE = "No";
                collection.ISOPTION = "No";
                collection.ISINTERNAL = "No";

                List<String> types1 = new();
                types1.Add("Vouchers");

                collection.TYPE = types1;

                // collection.TYPE = "Voucher";
              
                collection.FETCH = "REFERENCE";
                collection.FETCH="VOUCHERNUMBER";
            
                List<String> filters = new List<string>();
               
                filters.Add("voucheNumberFilterfromMasterId");
                collection.FILTERS= filters;
                List<COLLECTION> cOLLECTIONSlist = new List<COLLECTION>();
                cOLLECTIONSlist.Add(collection);

                tdlmessage.COLLECTION = cOLLECTIONSlist;
                List<SYSTEM> systemsList = new List<SYSTEM>();
                SYSTEM system = new SYSTEM();
                system.NAME = "voucheNumberFilterfromMasterId";

                system.TYPE = "Formulae";
                //system.Text = StringUtilsCustom.ConvertFormat(date);
                system.Text = "$$String:$MASTERID=$$String:\""+masterId+"\" ";

                systemsList.Add(system);
                //SYSTEM system2 = new SYSTEM();
                //system2.NAME = "ofSpecificVchs";
                //system2.TYPE = "Formulae";
                //system2.Text= "$$String:$VOUCHERTYPENAME=SALES ORDER";
                //systemsList.Add(system2);
                tdlmessage.SYSTEM = systemsList;
                tdl.TDLMESSAGE = tdlmessage;
                desc.TDL = tdl;
                body.DESC = desc;
                envelope.BODY = body;
                return envelope;



            }
        }
    }

