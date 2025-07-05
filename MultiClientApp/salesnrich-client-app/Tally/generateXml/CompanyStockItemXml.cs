using Microsoft.VisualBasic;
using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
	internal class CompanyStockItemXml
	{
		public ENVELOPE getCompanyStockItemXml()
		{
			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Collection";
			header.ID = "Collection of Stock Items";
			tallyRequest.HEADER = header;

			BODY body = new BODY();
			DESC desc = new DESC();

			// Static variables
			STATICVARIABLES staticvariables = new STATICVARIABLES();
			staticvariables.EXPLODEFLAG = "Yes";
			staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
			staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
			staticvariables.IsItemWise = "Yes";
			desc.STATICVARIABLES = staticvariables;

			// Build TDL with TDLMESSAGE (Report, Form, Part, Line, Field)
			TDL tdl = new TDL();
			TDLMESSAGE tdlmessage = new TDLMESSAGE();

			// REPORT
			REPORT report = new REPORT();
			report.NAME = "List of Stock Items";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "List of Stock Items";
			tdlmessage.REPORT = report;

			// FORM
			FORM form = new FORM();
			form.NAME = "List of Stock Items";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "List of Stock Items";
			form.XMLTAG = "List of Stock Items";
			tdlmessage.FORM = form;

			// PART
			PART part = new PART();
			part.NAME = "List of Stock Items";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "List of Stock Items";
			part.REPEAT = "List of Stock Items : Collection of Stock Items";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			tdlmessage.PART = new List<PART> { part };

			// LINE
			LINE line = new LINE();
			line.NAME = "List of Stock Items";
			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "Stock Item";
			line.FIELD = "Name,FirstAlias,Parent,AlterID,Guid,Category,RateOfMrp,OpeningRate,RateOfVat,BaseUnits,AdditionalUnits,Conversion,Denominator,IsBatchWiseOn,PartNo,Description,Narration,LastSalePrice,HSNCode,IntegratedTax,StateTax,CentralTax,Cess,StandardPrice";
			tdlmessage.LINE = new List<LINE> { line };

			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Name";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			
			fieldList.Add(field);
			FIELD field1 = new FIELD();
			 
			field1.NAME = "FirstAlias";
			field1.ISMODIFY = "No";
			field1.ISFIXED = "No";
			field1.ISINITIALIZE = "No";
			field1.ISOPTION = "No";
			field1.ISINTERNAL = "No";
			field1.SET = "$_FirstAlias";
			field1.XMLTAG = "ALIAS";
	 
			fieldList.Add(field1);
			FIELD field2 = new FIELD();
			field2.NAME = "Parent";
			field2.ISMODIFY = "No";
			field2.ISFIXED = "No";
			field2.ISINITIALIZE = "No";
			field2.ISOPTION = "No";
			field2.ISINTERNAL = "No";
			field2.SET = "$Parent";
			field2.XMLTAG = "PARENT";
			
			fieldList.Add(field2);
			FIELD field3 = new FIELD();
			field3.NAME = "AlterID";
			field3.ISMODIFY = "No";
			field3.ISFIXED = "No";
			field3.ISINITIALIZE = "No";
			field3.ISOPTION = "No";
			field3.ISINTERNAL = "No";
			field3.SET = "$AlterID";
			field3.XMLTAG = "ALTERID";
			fieldList.Add(field3);
			 

		 


			FIELD field4 = new FIELD();
			field4.NAME = "Guid";
			field4.ISMODIFY = "No";
			field4.ISFIXED = "No";
			field4.ISINITIALIZE = "No";
			field4.ISOPTION = "No";
			field4.ISINTERNAL = "No";
			field4.SET = "$Guid";
			field4.XMLTAG = "GUID";
		 
			fieldList.Add(field4);
			FIELD field5 = new FIELD();
			field5.NAME = "Category";
			field5.ISMODIFY = "No";
			field5.ISFIXED = "No";
			field5.ISINITIALIZE = "No";
			field5.ISOPTION = "No";
			field5.ISINTERNAL = "No";
			field5.SET = "$Category";
			field5.XMLTAG = "CATEGORY";

			fieldList.Add(field5);
			FIELD field6 = new FIELD();
			field6.NAME = "RateOfMrp";
			field6.ISMODIFY = "No";
			field6.ISFIXED = "No";
			field6.ISINITIALIZE = "No";
			field6.ISOPTION = "No";
			field6.ISINTERNAL = "No";
			field6.SET = "$RateOfMrp";
			field6.XMLTAG = "RATEOFMRP";
					fieldList.Add(field6);
						FIELD field7 = new FIELD();
			field7.NAME = "OpeningRate";
			field7.ISMODIFY = "No";
			field7.ISFIXED = "No";
			field7.ISINITIALIZE = "No";
			field7.ISOPTION = "No";
			field7.ISINTERNAL = "No";
			field7.SET = "$OpeningRate";
			field7.XMLTAG = "OPENINGRATE";
			
			fieldList.Add(field7);
			FIELD field8 = new FIELD();
			field8.NAME = "RateOfVat";
			field8.ISMODIFY = "No";
			field8.ISFIXED = "No";
			field8.ISINITIALIZE = "No";
			field8.ISOPTION = "No";
			field8.ISINTERNAL = "No";
			field8.SET = "$RateOfVat";
			field8.XMLTAG = "RATEOFVAT";
		
			fieldList.Add(field8);
			FIELD field9 = new FIELD();
			field9.NAME = "BaseUnits";
			field9.ISMODIFY = "No";
			field9.ISFIXED = "No";
			field9.ISINITIALIZE = "No";
			field9.ISOPTION = "No";
			field9.ISINTERNAL = "No";
			field9.SET = "$BaseUnits";
			field9.XMLTAG = "BASEUNITS";

			fieldList.Add(field9);
			
			FIELD field10 = new FIELD();
			field10.NAME = "AdditionalUnits";
			field10.ISMODIFY = "No";
			field10.ISFIXED = "No";
			field10.ISINITIALIZE = "No";
			field10.ISOPTION = "No";
			field10.ISINTERNAL = "No";
			field10.SET = "$AdditionalUnits";
			field10.XMLTAG = "ADDITIONALUNITS";
						fieldList.Add(field10);
						FIELD field11 = new FIELD();
			field11.NAME = "Conversion";
			field11.ISMODIFY = "No";
			field11.ISFIXED = "No";
			field11.ISINITIALIZE = "No";
			field11.ISOPTION = "No";
			field11.ISINTERNAL = "No";
			field11.SET = "$Conversion";
			field11.XMLTAG = "CONVERSION";
					  
			fieldList.Add(field11);
			FIELD field12 = new FIELD();
			field12.NAME = "Denominator";
			field12.ISMODIFY = "No";
			field12.ISFIXED = "No";
			field12.ISINITIALIZE = "No";
			field12.ISOPTION = "No";
			field12.ISINTERNAL = "No";
			field12.SET = "$Denominator";
			field12.XMLTAG = "DENOMINATOR";
		
			fieldList.Add(field12);
			FIELD field13 = new FIELD();
			field13.NAME = "IsBatchWiseOn";
			field13.ISMODIFY = "No";
			field13.ISFIXED = "No";
			field13.ISINITIALIZE = "No";
			field13.ISOPTION = "No";
			field13.ISINTERNAL = "No";
			field13.SET = "$IsBatchWiseOn";
			field13.XMLTAG = "ISBATCHWISEON";

			fieldList.Add(field13);
			FIELD field14 = new FIELD();
			field14.NAME = "PartNo";
			field14.ISMODIFY = "No";
			field14.ISFIXED = "No";
			field14.ISINITIALIZE = "No";
			field14.ISOPTION = "No";
			field14.ISINTERNAL = "No";
			field14.SET = "$PartNo";
			field14.XMLTAG = "PARTNO";
		
			fieldList.Add(field14);
			FIELD field15 = new FIELD();
			field15.NAME = "Description";
			field15.ISMODIFY = "No";
			field15.ISFIXED = "No";
			field15.ISINITIALIZE = "No";
			field15.ISOPTION = "No";
			field15.ISINTERNAL = "No";
			field15.SET = "$Description";
			field15.XMLTAG = "DESCRIPTION";
		
			fieldList.Add(field15);
			FIELD field16 = new FIELD();
			field16.NAME = "Narration";
			field16.ISMODIFY = "No";
			field16.ISFIXED = "No";
			field16.ISINITIALIZE = "No";
			field16.ISOPTION = "No";
			field16.ISINTERNAL = "No";
			field16.SET = "$Narration";
			field16.XMLTAG = "NARRATION";
		
			fieldList.Add(field16);
			FIELD field17 = new FIELD();
			field17.NAME = "LastSalePrice";
			field17.ISMODIFY = "No";
			field17.ISFIXED = "No";
			field17.ISINITIALIZE = "No";
			field17.ISOPTION = "No";
			field17.ISINTERNAL = "No";
			field17.SET = "$_LastSalePrice";
			field17.XMLTAG = "LASTSALEPRICE";
			
			fieldList.Add(field17);
			FIELD field18 = new FIELD();
			field18.NAME = "HSNCode";
			field18.ISMODIFY = "No";
			field18.ISFIXED = "No";
			field18.ISINITIALIZE = "No";
			field18.ISOPTION = "No";
			field18.ISINTERNAL = "No";
			field18.SET = "$_HSNCode";
			field18.XMLTAG = "HSNCODE";
		
			fieldList.Add(field18);

			FIELD field19 = new FIELD();
			field19.NAME = "IntegratedTax";
			field19.ISMODIFY = "No";
			field19.ISFIXED = "No";
			field19.ISINITIALIZE = "No";
			field19.ISOPTION = "No";
			field19.ISINTERNAL = "No";
			field19.SET = "$_IntegratedTax";
			field19.XMLTAG = "INTEGRATEDTAX";
		
			fieldList.Add(field19);
			FIELD field20 = new FIELD();
			field20.NAME = "StateTax";
			field20.ISMODIFY = "No";
			field20.ISFIXED = "No";
			field20.ISINITIALIZE = "No";
			field20.ISOPTION = "No";
			field20.ISINTERNAL = "No";
			field20.SET = "$_StateTax";
			field20.XMLTAG = "STATETAX";

			fieldList.Add(field20);
			FIELD field21 = new FIELD();
			field21.NAME = "CentralTax";
			field21.ISMODIFY = "No";
			field21.ISFIXED = "No";
			field21.ISINITIALIZE = "No";
			field21.ISOPTION = "No";
			field21.ISINTERNAL = "No";
			field21.SET = "$_CentralTax";
			field21.XMLTAG = "CENTRALTAX";

			fieldList.Add(field21);
			fieldList.Add(field20);
			FIELD field22 = new FIELD();
			field22.NAME = "Cess";
			field22.ISMODIFY = "No";
			field22.ISFIXED = "No";
			field22.ISINITIALIZE = "No";
			field22.ISOPTION = "No";
			field22.ISINTERNAL = "No";
			field22.SET = "$_Cess";
			field22.XMLTAG = "CESS";

			fieldList.Add(field22);
			FIELD field23 = new FIELD();
			field23.NAME = "StandardPrice";
			field23.ISMODIFY = "No";
			field23.ISFIXED = "No";
			field23.ISINITIALIZE = "No";
			field23.ISOPTION = "No";
			field23.ISINTERNAL = "No";
			field23.SET = "$StandardPrice";
			field23.XMLTAG = "STANDARDPRICE";

			fieldList.Add(field23);
			tdlmessage.FIELD = fieldList;
			tdl.TDLMESSAGE = tdlmessage;
			desc.TDL = tdl;

			// ✅ Correct: Set collection directly under DESC (not inside TDLMESSAGE)
			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "Collection of Stock Items";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";
			collection.ISINTERNAL = "No";
			collection.TYPE = new List<string> { "Stock Item" }; // Use "Stock Item" with proper case
			collection.FETCH = "Name,$_FirstAlias,Parent,AlterID,Guid,Category,RateOfMrp,OpeningRate,RateOfVat,BaseUnits,AdditionalUnits,Conversion,Denominator,IsBatchWiseOn,PartNo,Description,Narration,$_LastSalePrice,$_HSNCode,$_IntegratedTax,$_StateTax,$_CentralTax,$_Cess,StandardPrice";
			collectionsList.Add(collection);

			// ✅ Add collection directly to DESC
			tdlmessage.COLLECTION = collectionsList;

			// Final setup
			body.DESC = desc;
			tallyRequest.BODY = body;

			// Return the final object
			return tallyRequest;
		}
	}
}
