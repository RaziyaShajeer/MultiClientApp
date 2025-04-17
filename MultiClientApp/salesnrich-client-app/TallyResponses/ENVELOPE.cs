//using SNR_ClientApp.TallyRequests;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SNR_ClientApp.TallyResponses
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(ENVELOPE));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (ENVELOPE)serializer.Deserialize(reader);
    // }


    public class TallyRequestResponse
    {

        public ENVELOPE response = new ENVELOPE();

    }

    [XmlRoot(ElementName = "HEADER")]
    public class HEADER
    {

        [XmlElement(ElementName = "VERSION")]
        public string VERSION { get; set; }

        [XmlElement(ElementName = "STATUS")]
        public string STATUS { get; set; }

        [XmlElement(ElementName = "TALLYREQUEST")]
        public string TALLYREQUEST { get; set; }

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }

        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }

    }

    [XmlRoot(ElementName = "CMPINFO")]
    public class CMPINFO
    {

        [XmlElement(ElementName = "COMPANY")]
        public string COMPANY { get; set; }

        [XmlElement(ElementName = "GROUP")]
        public string GROUP { get; set; }

        [XmlElement(ElementName = "LEDGER")]
        public string LEDGER { get; set; }

        [XmlElement(ElementName = "COSTCATEGORY")]
        public string COSTCATEGORY { get; set; }

        [XmlElement(ElementName = "COSTCENTRE")]
        public string COSTCENTRE { get; set; }

        [XmlElement(ElementName = "GODOWN")]
        public string GODOWN { get; set; }

        [XmlElement(ElementName = "STOCKGROUP")]
        public string STOCKGROUP { get; set; }

        [XmlElement(ElementName = "STOCKCATEGORY")]
        public string STOCKCATEGORY { get; set; }

        [XmlElement(ElementName = "STOCKITEM")]
        public string STOCKITEM { get; set; }

        [XmlElement(ElementName = "VOUCHERTYPE")]
        public string VOUCHERTYPE { get; set; }

        [XmlElement(ElementName = "CURRENCY")]
        public string CURRENCY { get; set; }

        [XmlElement(ElementName = "UNIT")]
        public string UNIT { get; set; }

        [XmlElement(ElementName = "BUDGET")]
        public string BUDGET { get; set; }

        [XmlElement(ElementName = "CLIENTRULE")]
        public string CLIENTRULE { get; set; }

        [XmlElement(ElementName = "SERVERRULE")]
        public string SERVERRULE { get; set; }

        [XmlElement(ElementName = "STATE")]
        public string STATE { get; set; }

        [XmlElement(ElementName = "TDSRATE")]
        public string TDSRATE { get; set; }

        [XmlElement(ElementName = "TAXCLASSIFICATION")]
        public string TAXCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "STCATEGORY")]
        public string STCATEGORY { get; set; }

        [XmlElement(ElementName = "DEDUCTEETYPE")]
        public string DEDUCTEETYPE { get; set; }

        [XmlElement(ElementName = "ATTENDANCETYPE")]
        public string ATTENDANCETYPE { get; set; }

        [XmlElement(ElementName = "FBTCATEGORY")]
        public string FBTCATEGORY { get; set; }

        [XmlElement(ElementName = "FBTASSESSEETYPE")]
        public string FBTASSESSEETYPE { get; set; }

        [XmlElement(ElementName = "TARIFFCLASSIFICATION")]
        public string TARIFFCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "EXCISEDUTYCLASSIFICATION")]
        public string EXCISEDUTYCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "SERIALNUMBER")]
        public string SERIALNUMBER { get; set; }

        [XmlElement(ElementName = "ADJUSTMENTCLASSIFICATION")]
        public string ADJUSTMENTCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "INCOMETAXSLAB")]
        public string INCOMETAXSLAB { get; set; }

        [XmlElement(ElementName = "INCOMETAXCLASSIFICATION")]
        public string INCOMETAXCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "LBTCLASSIFICATION")]
        public string LBTCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "TAXUNIT")]
        public string TAXUNIT { get; set; }

        [XmlElement(ElementName = "RETURNMASTER")]
        public string RETURNMASTER { get; set; }

        [XmlElement(ElementName = "VOUCHER")]
        public string VOUCHER { get; set; }
    }

    [XmlRoot(ElementName = "DESC")]
    public class DESC
    {

        [XmlElement(ElementName = "CMPINFO")]
        public CMPINFO CMPINFO { get; set; }

        [XmlElement(ElementName = "TDL")]
        public TDL TDL { get; set; }


        [XmlElement(ElementName = "STATICVARIABLES")]
        public STATICVARIABLES STATICVARIABLES { get; set; }
    }

    [XmlRoot(ElementName = "GUID")]
    public class GUID
    {

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "ISDELETED")]
    public class ISDELETED
    {

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }
    }

    [XmlRoot(ElementName = "NAME.LIST")]
    public class NAME_LIST {

        [XmlElement(ElementName = "NAME")]
        public List<string> NAME { get; set; }

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "LANGUAGEID")]
    public class LANGUAGEID
    {

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    //[XmlRoot(ElementName = "LANGUAGENAME.LIST")]
    //public class LANGUAGENAME_LIST
    //{

    //    [XmlElement(ElementName = "NAME.LIST")]
    //    public NAME_LIST NAME_LIST { get; set; }

    //    [XmlElement(ElementName = "LANGUAGEID")]
    //    public LANGUAGEID LANGUAGEID { get; set; }
    //}

    [XmlRoot(ElementName = "PRICELEVELLIST.LIST")]
    public class PRICELEVELLIST_LIST
    {

        [XmlElement(ElementName = "ENDINGAT")]
        public object ENDINGAT { get; set; }

        [XmlElement(ElementName = "STARTINGFROM")]
        public object STARTINGFROM { get; set; }

        [XmlElement(ElementName = "RATE")]
        public string RATE { get; set; }

        [XmlElement(ElementName = "DISCOUNT")]
        public string DISCOUNT { get; set; }
    }

    [XmlRoot(ElementName = "FULLPRICELIST.LIST")]
    public class FULLPRICELIST_LIST
    {

        [XmlElement(ElementName = "DATE")]
        public string DATE { get; set; }

        [XmlElement(ElementName = "PRICELEVEL")]
        public string PRICELEVEL { get; set; }

        [XmlElement(ElementName = "PRICELEVELLIST.LIST")]
        public PRICELEVELLIST_LIST PRICELEVELLIST_LIST { get; set; }
    }

    [XmlRoot(ElementName = "STOCKITEM")]
    public class STOCKITEM
    {

        [XmlElement(ElementName = "GUID")]
        public GUID GUID { get; set; }

        [XmlElement(ElementName = "ISDELETED")]
        public ISDELETED ISDELETED { get; set; }

        [XmlElement(ElementName = "LANGUAGENAME.LIST")]
        public LANGUAGENAMELIST LANGUAGENAME_LIST { get; set; }

        [XmlElement(ElementName = "FULLPRICELIST.LIST")]
        public List<FULLPRICELIST_LIST> FULLPRICELIST_LIST { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlElement(ElementName = "RESERVEDNAME")]
        public object RESERVEDNAME { get; set; }

        [XmlText]
        public string text { get; set; }

        [XmlElement(ElementName = "PARENT")]
        public PARENT PARENT { get; set; }

        [XmlElement(ElementName = "GSTDETAILS.LIST")]
        public List<GSTDETAILSLIST> GSTDETAILSLIST { get; set; } = new();

        //[XmlElement(ElementName = "LANGUAGENAME.LIST")]
        //public LANGUAGENAMELIST LANGUAGENAMELIST { get; set; }

        [XmlElement(ElementName = "MRPDETAILS.LIST")]
        public List<MRPDETAILSLIST> MRPDETAILSLIST { get; set; }

        [XmlElement(ElementName = "SALESLIST.LIST")]
        public SALESLISTLIST SALESLISTLIST { get; set; }


    }


    [XmlRoot(ElementName = "COLLECTION")]
    public class COLLECTION
    {
        [XmlElement(ElementName = "OBJECTS")]
        public string? OBJECTS { get; set; }

        [XmlElement(ElementName = "STOCKITEM")]
        public List<STOCKITEM> STOCKITEM { get; set; } = new();

        [XmlElement(ElementName = "ISMSTDEPTYPE")]
        public string ISMSTDEPTYPE { get; set; }

        [XmlElement(ElementName = "MSTDEPTYPE")]
        public string MSTDEPTYPE { get; set; }

        [XmlText]
        public string text { get; set; }

        [XmlElement(ElementName = "TYPE")]
        public List<string> TYPE { get; set; }

        [XmlElement(ElementName = "FETCH")]
        public string FETCH { get; set; }

        [XmlElement(ElementName = "SOURCECOLLECTION")]
        public string SOURCECOLLECTION { get; set; }

        [XmlElement(ElementName = "WALK")]
        public string WALK { get; set; }


        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlElement(ElementName = "FILTERS")]
        public List<string> FILTERS { get; set; }



        [XmlElement(ElementName = "childof")]
        public string childof { get; set; }

        //For getCompanyNettStockAvilBatchWiseXml request

        [XmlElement(ElementName = "BelongsTo")]
        public string BelongsTo { get; set; }

        [XmlElement(ElementName = "NativeMethod")]
        public List<string> NativeMethod { get; set; }


        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }


        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlElement(ElementName = "CURRENTCOMPANY")]
        public CURRENTCOMPANY CURRENTCOMPANY { get; set; }
        [XmlElement(ElementName = "VOUCHER")]
        public List<VOUCHER> VOUCHER { get; set; }

        //------------


    }


    [XmlRoot(ElementName = "DATA")]
    public class DATA
    {

        [XmlElement(ElementName = "COLLECTION")]
        public COLLECTION COLLECTION { get; set; }
    }

    [XmlRoot(ElementName = "BODY")]
    public class BODY
    {

        [XmlElement(ElementName = "DESC")]
        public DESC DESC { get; set; }

        [XmlElement(ElementName = "DATA")]
        public DATA DATA { get; set; }

        [XmlElement(ElementName = "EXPORTDATA")]
        public EXPORTDATA EXPORTDATA { get; set; }
        [XmlElement(ElementName = "IMPORTDATA")]
        public IMPORTDATA IMPORTDATA { get; set; }
        //test : for Sql query response

        [XmlElement(ElementName = "EXPORTDATARESPONSE")]
        public EXPORTDATARESPONSE EXPORTDATARESPONSE { get; set; }

    }

    [XmlRoot(ElementName = "ENVELOPE")]
    public class ENVELOPE
    {

        [XmlElement(ElementName = "HEADER")]
        public HEADER HEADER { get; set; }

        [XmlElement(ElementName = "BODY")]
        public BODY BODY { get; set; }

        [XmlElement(ElementName = "DSPACCNAME")]
        public List<DSPACCNAME> DSPACCNAME { get; set; }

        [XmlElement(ElementName = "DSPSTKINFO")]
        public List<DSPSTKINFO> DSPSTKINFO { get; set; }

        [XmlElement(ElementName = "SSBATCHNAME")]
        public List<SSBATCHNAME> SSBATCHNAME { get; set; }



        [XmlElement(ElementName = "ITEMLIST")]
        public List<ITEMLIST> ITEMLIST { get; set; }

        [XmlElement(ElementName = "VOUCHERS")]
        public List<VOUCHERS> VOUCHERS { get; set; }
   

    }


    [XmlRoot(ElementName = "DSPACCNAME")]
    public class DSPACCNAME
    {

        [XmlElement(ElementName = "DSPDISPNAME")]
        public string DSPDISPNAME { get; set; }
    }

    [XmlRoot(ElementName = "DSPSTKCL")]
    public class DSPSTKCL
    {

        [XmlElement(ElementName = "DSPCLQTY")]
        public string DSPCLQTY { get; set; }

        [XmlElement(ElementName = "DSPCLRATE")]
        public string DSPCLRATE { get; set; }

        [XmlElement(ElementName = "DSPCLAMTA")]
        public string DSPCLAMTA { get; set; }
    }

    [XmlRoot(ElementName = "DSPSTKINFO")]
    public class DSPSTKINFO
    {

        [XmlElement(ElementName = "DSPSTKCL")]
        public DSPSTKCL DSPSTKCL { get; set; }
    }

    [XmlRoot(ElementName = "SSBATCHNAME")]
    public class SSBATCHNAME
    {

        [XmlElement(ElementName = "SSBATCH")]
        public string SSBATCH { get; set; }

        [XmlElement(ElementName = "SSGODOWN")]
        public string SSGODOWN { get; set; }
    }

    [XmlRoot(ElementName = "ITEMLIST")]
    public class ITEMLIST
    {

        [XmlElement(ElementName = "ITEMNAME")]
        public string ITEMNAME { get; set; }

        [XmlElement(ElementName = "ITEMQTY")]
        public string ITEMQTY { get; set; }

        [XmlElement(ElementName = "ITEMSOQTY")]
        public string ITEMSOQTY { get; set; }
    }


 

    [XmlRoot(ElementName = "STATICVARIABLES")]
    public class STATICVARIABLES
    {

        [XmlElement(ElementName = "SVCURRENTCOMPANY")]
        public string SVCURRENTCOMPANY { get; set; }
        [XmlElement(ElementName = "EXPLODEFLAG")]
        public string EXPLODEFLAG { get; set; }

       
        [XmlElement(ElementName = "SVEXPORTFORMAT")]
        public string SVEXPORTFORMAT { get; set; }

        [XmlElement(ElementName = "IsItemWise")]
        public string IsItemWise { get; set; }


        [XmlElement(ElementName = "LEDGERNAME")]
        public string LEDGERNAME { get; set; }


    }

    //[XmlRoot(ElementName = "OLDAUDITENTRYIDS.LIST")]
    //public class OLDAUDITENTRYIDSLIST
    //{

    //    [XmlElement(ElementName = "OLDAUDITENTRYIDS")]
    //    public string OLDAUDITENTRYIDS { get; set; }

    //    [XmlAttribute(AttributeName = "TYPE")]
    //    public string TYPE { get; set; }

    //    [XmlText]
    //    public string Text { get; set; }
    //}


    [XmlRoot(ElementName = "ALLLEDGERENTRIES.LIST")]
    public class ALLLEDGERENTRIESLIST
    {

        [XmlElement(ElementName = "OLDAUDITENTRYIDS.LIST")]
        public OLDAUDITENTRYIDSLIST OLDAUDITENTRYIDSLIST { get; set; }

        [XmlElement(ElementName = "LEDGERNAME")]
        public string LEDGERNAME { get; set; }

        [XmlElement(ElementName = "GSTCLASS")]
        public object GSTCLASS { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string ISDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "LEDGERFROMITEM")]
        public string LEDGERFROMITEM { get; set; }

        [XmlElement(ElementName = "REMOVEZEROENTRIES")]
        public string REMOVEZEROENTRIES { get; set; }

        [XmlElement(ElementName = "ISPARTYLEDGER")]
        public string ISPARTYLEDGER { get; set; }

        [XmlElement(ElementName = "ISLASTDEEMEDPOSITIVE")]
        public string ISLASTDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }

        [XmlElement(ElementName = "INTERESTCOLLECTION.LIST")]
        public object INTERESTCOLLECTIONLIST { get; set; }

        [XmlElement(ElementName = "OLDAUDITENTRIES.LIST")]
        public object OLDAUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "ACCOUNTAUDITENTRIES.LIST")]
        public object ACCOUNTAUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "AUDITENTRIES.LIST")]
        public object AUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "TAXBILLALLOCATIONS.LIST")]
        public object TAXBILLALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "TAXOBJECTALLOCATIONS.LIST")]
        public object TAXOBJECTALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "TDSEXPENSEALLOCATIONS.LIST")]
        public object TDSEXPENSEALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "VATSTATUTORYDETAILS.LIST")]
        public object VATSTATUTORYDETAILSLIST { get; set; }

        [XmlElement(ElementName = "COSTTRACKALLOCATIONS.LIST")]
        public object COSTTRACKALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "BANKALLOCATIONS.LIST")]
        public BANKALLOCATIONSLIST BANKALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "BILLALLOCATIONS.LIST")]
        public BILLALLOCATIONSLIST BILLALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "CATEGORYALLOCATIONS.LIST")]
        public CATEGORYALLOCATIONSLIST CATEGORYALLOCATIONSLIST { get; set; }
    }

    [XmlRoot(ElementName = "COSTCENTREALLOCATIONS.LIST")]
    public class COSTCENTREALLOCATIONSLIST
    {

        [XmlElement(ElementName = "NAME")]
        public string? NAME { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string? AMOUNT { get; set; }
    }

    [XmlRoot(ElementName = "CATEGORYALLOCATIONS.LIST")]
    public class CATEGORYALLOCATIONSLIST
    {

        [XmlElement(ElementName = "CATEGORY")]
        public string? CATEGORY { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string? ISDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "COSTCENTREALLOCATIONS.LIST")]
        public COSTCENTREALLOCATIONSLIST COSTCENTREALLOCATIONSLIST { get; set; }
    }


    [XmlRoot(ElementName = "REQUESTDESC")]
    public class REQUESTDESC
    {

        [XmlElement(ElementName = "REPORTNAME")]
        public string REPORTNAME { get; set; }

        [XmlElement(ElementName = "STATICVARIABLES")]
        public STATICVARIABLES STATICVARIABLES { get; set; }
        [XmlElement(ElementName = "SQLREQUEST")]
        public SQLREQUEST SQLREQUEST { get; set; }
    }

    [XmlRoot(ElementName = "SQLREQUEST")]
    public class SQLREQUEST
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "METHOD")]
        public string METHOD { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "REQUESTDATA")]
    public class REQUESTDATA
    {

        [XmlElement(ElementName = "TALLYMESSAGE")]
        public List<TALLYMESSAGE> TALLYMESSAGE { get; set; }
    }

    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class TALLYMESSAGE
    {

        [XmlElement(ElementName = "VOUCHER")]
        public VOUCHER VOUCHER { get; set; }

        [XmlAttribute(AttributeName = "UDF")]
        public string UDF { get; set; }

        [XmlText]
        public string text { get; set; }

        [XmlElement(ElementName = "COMPANY")]
        public COMPANY COMPANY { get; set; }

        [XmlElement(ElementName = "LEDGER")]
        public LEDGER LEDGER { get; set; }
    }

    [XmlRoot(ElementName = "COMPANY")]
    public class COMPANY
    {

        [XmlElement(ElementName = "REMOTECMPINFO.LIST")]
        public REMOTECMPINFO_LIST REMOTECMPINFO_LIST { get; set; }
    }

    [XmlRoot(ElementName = "REMOTECMPINFO.LIST")]
    public class REMOTECMPINFO_LIST
    { 

	[XmlElement(ElementName = "NAME")]
    public string NAME { get; set; }

    [XmlElement(ElementName = "REMOTECMPNAME")]
    public string REMOTECMPNAME { get; set; }

    [XmlElement(ElementName = "REMOTECMPSTATE")]
    public string REMOTECMPSTATE { get; set; }

    [XmlAttribute(AttributeName = "MERGE")]
    public string MERGE { get; set; }

    [XmlText]
    public string text { get; set; }
}


[XmlRoot(ElementName = "VOUCHER")]
    public class VOUCHER
    {
        [XmlAttribute(AttributeName = "REMOTEID")]
        public string REMOTEID { get; set; }

        [XmlAttribute(AttributeName = "VCHTYPE")]
        public string VCHTYPE { get; set; }


        [XmlElement(ElementName = "DATE")]
        public string DATE { get; set; }

        [XmlElement(ElementName = "GUID")]
        public string GUID { get; set; }


        [XmlElement(ElementName = "VOUCHERTYPENAME")]
        public string VOUCHERTYPENAME { get; set; }

        [XmlElement(ElementName = "NARRATION")]
        public string NARRATION { get; set; }

        [XmlElement(ElementName = "PARTYLEDGERNAME")]
        public string PARTYLEDGERNAME { get; set; }

        [XmlElement(ElementName = "ALLLEDGERENTRIES.LIST")]
        public List<ALLLEDGERENTRIESLIST> ALLLEDGERENTRIES_LIST { get; set; }

        [XmlElement(ElementName = "VOUCHERNUMBER")]
        public string VOUCHERNUMBER { get; set; }

        [XmlElement(ElementName = "REFERENCE")]
        public string REFERENCE { get; set; }

		



		[XmlElement(ElementName = "OLDAUDITENTRYIDS.LIST")]
        public OLDAUDITENTRYIDSLIST OLDAUDITENTRYIDSLIST { get; set; }

        //[XmlElement(ElementName = "DATE")]
        //public int DATE { get; set; }

        //[XmlElement(ElementName = "GUID")]
        //public string GUID { get; set; }

        //[XmlElement(ElementName = "NARRATION")]
        //public string NARRATION { get; set; }

        //[XmlElement(ElementName = "VOUCHERTYPENAME")]
        //public string VOUCHERTYPENAME { get; set; }

        //[XmlElement(ElementName = "VOUCHERNUMBER")]
        //public object VOUCHERNUMBER { get; set; }

        //[XmlElement(ElementName = "PARTYLEDGERNAME")]
        //public string PARTYLEDGERNAME { get; set; }

        [XmlElement(ElementName = "CSTFORMISSUETYPE")]
        public object CSTFORMISSUETYPE { get; set; }

        [XmlElement(ElementName = "CSTFORMRECVTYPE")]
        public object CSTFORMRECVTYPE { get; set; }

        [XmlElement(ElementName = "FBTPAYMENTTYPE")]
        public string FBTPAYMENTTYPE { get; set; }

        [XmlElement(ElementName = "PERSISTEDVIEW")]
        public string PERSISTEDVIEW { get; set; }

        [XmlElement(ElementName = "VCHGSTCLASS")]
        public object VCHGSTCLASS { get; set; }

        [XmlElement(ElementName = "DIFFACTUALQTY")]
        public string DIFFACTUALQTY { get; set; }

        [XmlElement(ElementName = "AUDITED")]
        public string AUDITED { get; set; }

        [XmlElement(ElementName = "FORJOBCOSTING")]
        public string FORJOBCOSTING { get; set; }

        [XmlElement(ElementName = "ISOPTIONAL")]
        public string ISOPTIONAL { get; set; }

        [XmlElement(ElementName = "EFFECTIVEDATE")]
        public string EFFECTIVEDATE { get; set; }

        [XmlElement(ElementName = "ISFORJOBWORKIN")]
        public string ISFORJOBWORKIN { get; set; }

        [XmlElement(ElementName = "ALLOWCONSUMPTION")]
        public string ALLOWCONSUMPTION { get; set; }

        [XmlElement(ElementName = "USEFORINTEREST")]
        public string USEFORINTEREST { get; set; }

        [XmlElement(ElementName = "USEFORGAINLOSS")]
        public string USEFORGAINLOSS { get; set; }

        [XmlElement(ElementName = "USEFORGODOWNTRANSFER")]
        public string USEFORGODOWNTRANSFER { get; set; }

        [XmlElement(ElementName = "USEFORCOMPOUND")]
        public string USEFORCOMPOUND { get; set; }

        [XmlElement(ElementName = "ALTERID")]
        public int ALTERID { get; set; }

        [XmlElement(ElementName = "EXCISEOPENING")]
        public string EXCISEOPENING { get; set; }

        [XmlElement(ElementName = "USEFORFINALPRODUCTION")]
        public string USEFORFINALPRODUCTION { get; set; }

        [XmlElement(ElementName = "ISCANCELLED")]
        public string ISCANCELLED { get; set; }

        [XmlElement(ElementName = "HASCASHFLOW")]
        public string HASCASHFLOW { get; set; }

        [XmlElement(ElementName = "ISPOSTDATED")]
        public string ISPOSTDATED { get; set; }

        [XmlElement(ElementName = "USETRACKINGNUMBER")]
        public string USETRACKINGNUMBER { get; set; }

        [XmlElement(ElementName = "ISINVOICE")]
        public string ISINVOICE { get; set; }

        [XmlElement(ElementName = "MFGJOURNAL")]
        public string MFGJOURNAL { get; set; }

        [XmlElement(ElementName = "HASDISCOUNTS")]
        public string HASDISCOUNTS { get; set; }

        [XmlElement(ElementName = "ASPAYSLIP")]
        public string ASPAYSLIP { get; set; }

        [XmlElement(ElementName = "ISSTXNONREALIZEDVCH")]
        public string ISSTXNONREALIZEDVCH { get; set; }

        [XmlElement(ElementName = "ISEXCISEMANUFACTURERON")]
        public string ISEXCISEMANUFACTURERON { get; set; }

        [XmlElement(ElementName = "ISBLANKCHEQUE")]
        public string ISBLANKCHEQUE { get; set; }

        [XmlElement(ElementName = "ISVOID")]
        public string ISVOID { get; set; }

        [XmlElement(ElementName = "ISONHOLD")]
        public string ISONHOLD { get; set; }

        [XmlElement(ElementName = "ISDELETED")]
        public string ISDELETED { get; set; }

        [XmlElement(ElementName = "ASORIGINAL")]
        public string ASORIGINAL { get; set; }

        [XmlElement(ElementName = "VCHISFROMSYNC")]
        public string VCHISFROMSYNC { get; set; }

        [XmlElement(ElementName = "MASTERID")]
        public int MASTERID { get; set; }

        [XmlElement(ElementName = "VOUCHERKEY")]
        public string VOUCHERKEY { get; set; }

        [XmlElement(ElementName = "OLDAUDITENTRIES.LIST")]
        public object OLDAUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "ACCOUNTAUDITENTRIES.LIST")]
        public object ACCOUNTAUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "AUDITENTRIES.LIST")]
        public object AUDITENTRIESLIST { get; set; }

        [XmlElement(ElementName = "INVOICEDELNOTES.LIST")]
        public object INVOICEDELNOTESLIST { get; set; }

        [XmlElement(ElementName = "INVOICEORDERLIST.LIST")]
        public object INVOICEORDERLISTLIST { get; set; }

        [XmlElement(ElementName = "INVOICEINDENTLIST.LIST")]
        public object INVOICEINDENTLISTLIST { get; set; }

        [XmlElement(ElementName = "ATTENDANCEENTRIES.LIST")]
        public object ATTENDANCEENTRIESLIST { get; set; }

        [XmlElement(ElementName = "ORIGINVOICEDETAILS.LIST")]
        public object ORIGINVOICEDETAILSLIST { get; set; }

        [XmlElement(ElementName = "INVOICEEXPORTLIST.LIST")]
        public object INVOICEEXPORTLISTLIST { get; set; }

        //[XmlElement(ElementName = "ALLLEDGERENTRIES.LIST")]
        //public List<ALLLEDGERENTRIESLIST> ALLLEDGERENTRIESLIST { get; set; }

        [XmlElement(ElementName = "PAYROLLMODEOFPAYMENT.LIST")]
        public object PAYROLLMODEOFPAYMENTLIST { get; set; }

        [XmlElement(ElementName = "ATTDRECORDS.LIST")]
        public object ATTDRECORDSLIST { get; set; }


        [XmlAttribute(AttributeName = "VCHKEY")]
        public string? VCHKEY { get; set; }

        //[XmlAttribute(AttributeName = "VCHTYPE")]
        //public string VCHTYPE { get; set; }

        [XmlAttribute(AttributeName = "ACTION")]
        public string? ACTION { get; set; }

        [XmlAttribute(AttributeName = "OBJVIEW")]
        public string? OBJVIEW { get; set; }

        [XmlText]
        public string? Text { get; set; }
        [XmlElement(ElementName = "COSTCENTRENAME")]
        public string? COSTCENTRENAME { get;  set; }
        [XmlElement(ElementName = "ISCOSTCENTRE")]
        public string? ISCOSTCENTRE { get;  set; }

        [XmlElement(ElementName = "PRICELEVEL")]
        public string? PRICELEVEL { get; set; }


        [XmlElement(ElementName = "ADDRESS.LIST")]
        public ADDRESSLIST? ADDRESSLIST { get; set; }

        [XmlElement(ElementName = "BASICBUYERADDRESS.LIST")]
        public List<BASICBUYERADDRESSLIST>? BASICBUYERADDRESSLIST { get; set; }

        [XmlElement(ElementName = "PARTYNAME")]
        public string? PARTYNAME { get; set; }

    

        [XmlElement(ElementName = "VOUCHERTYPECLASS")]
        public string? VOUCHERTYPECLASS { get; set; }

       


       
        [XmlElement(ElementName = "PARTYPINCODE")]
        public object? PARTYPINCODE { get; set; }

        [XmlElement(ElementName = "BASICBASEPARTYNAME")]
        public string? BASICBASEPARTYNAME { get; set; }

      

        [XmlElement(ElementName = "BASICBUYERNAME")]
        public string? BASICBUYERNAME { get; set; }

        [XmlElement(ElementName = "STATENAME")]
        public string? STATENAME { get; set; }

        [XmlElement(ElementName = "COUNTRYOFRESIDENCE")]
        public string? COUNTRYOFRESIDENCE { get; set; }

        [XmlElement(ElementName = "PLACEOFSUPPLY")]
        public string? PLACEOFSUPPLY { get; set; }

        [XmlElement(ElementName = "CONSIGNEEMAILINGNAME")]
        public string? CONSIGNEEMAILINGNAME { get; set; }

        [XmlElement(ElementName = "CONSIGNEECOUNTRYNAME")]
        public string? CONSIGNEECOUNTRYNAME { get; set; }

        [XmlElement(ElementName = "CONSIGNEESTATENAME")]
        public string? CONSIGNEESTATENAME { get; set; }


        [XmlElement(ElementName = "CONSIGNEEPINCODE")]
        public string? CONSIGNEEPINCODE { get; set; }


        [XmlElement(ElementName = "LEDGERENTRIES.LIST")]
        public List<LEDGERENTRIESLIST>? LEDGERENTRIESLIST { get; set; }

        [XmlElement(ElementName = "ALLINVENTORYENTRIES.LIST")]
        public List<ALLINVENTORYENTRIESLIST>? ALLINVENTORYENTRIESLIST { get; set; }
        [XmlElement(ElementName = "PARTYGSTIN")]
        public string? PARTYGSTIN { get;  set; }

        [XmlElement(ElementName = "CONSIGNEEGSTIN")]
        public string? CONSIGNEEGSTIN { get; set; }
        [XmlElement(ElementName = "BASICDUEDATEOFPYMT")]
        public string BASICDUEDATEOFPYMT { get; set; }

        [XmlElement(ElementName = "PPVCHSALESMANNAME.LIST")]
        public PPVCHSALESMANNAMELIST PPVCHSALESMANNAMELIST { get; set; }
        [XmlElement(ElementName = "VATISASSESABLECALCVCH")]
        public string VATISASSESABLECALCVCH { get;  set; }

        //for vehicle details on vansales
        [XmlElement(ElementName = "EWAYBILLDETAILS.LIST")]
        public EWAYBILLDETAILSLIST EWAYBILLDETAILSLIST { get; set; }


        [XmlElement(ElementName = "SDKGWTOTALS.LIST")]
        public SDKGWTOTALSLIST SDKGWTOTALSLIST { get; set; }

        //[XmlElement(ElementName = "SALESMASTERID.LIST")]
        //public SALESMASTERIDLIST SALESMASTERIDLIST { get; set; }


        [XmlElement(ElementName = "SDKRENTVEHICLEF.LIST")]
        public SDKRENTVEHICLEFLIST SDKRENTVEHICLEFLIST { get; set; }

        [XmlElement(ElementName = "SDKOWNVEHICLEF.LIST")]
        public SDKOWNVEHICLEFLIST SDKOWNVEHICLEFLIST { get; set; }

        [XmlElement(ElementName = "SDKSALESMAN2F.LIST")]
        public SDKSALESMAN2FLIST SDKSALESMAN2FLIST { get; set; }

        [XmlElement(ElementName = "SDKDRIVERLISTF.LIST")]
        public SDKDRIVERLISTFLIST SDKDRIVERLISTFLIST { get; set; }

        [XmlElement(ElementName = "SDKOWNVEHICLEF1.LIST")]
        public SDKOWNVEHICLEF1LIST SDKOWNVEHICLEF1LIST { get; set; }


        /// <summary>
        /// 
        /// </summary>

        [XmlElement(ElementName = "SDKCREATEDDATE.LIST")]
        public SDKCREATEDDATELIST? SDKCREATEDDATELIST { get; set; }

        //[XmlElement(ElementName = "SDKOWNVEHICLEF.LIST")]
        //public SDKOWNVEHICLEFLIST SDKOWNVEHICLEFLIST { get; set; }

        //[XmlElement(ElementName = "SDKSALESMAN2F.LIST")]
        //public SDKSALESMAN2FLIST SDKSALESMAN2FLIST { get; set; }

        //[XmlElement(ElementName = "SDKDRIVERLISTF.LIST")]
        //public SDKDRIVERLISTFLIST SDKDRIVERLISTFLIST { get; set; }

        //[XmlElement(ElementName = "SDKOWNVEHICLEF1.LIST")]
        //public SDKOWNVEHICLEF1LIST SDKOWNVEHICLEF1LIST { get; set; }

        [XmlElement(ElementName = "SDKSYNCSELVOUCHERS.LIST")]
        public SDKSYNCSELVOUCHERSLIST SDKSYNCSELVOUCHERSLIST { get; set; }

        [XmlElement(ElementName = "SDKCREATEDBY.LIST")]
        public SDKCREATEDBYLIST SDKCREATEDBYLIST { get; set; }

        [XmlElement(ElementName = "SDKCREATEDTIME.LIST")]
        public SDKCREATEDTIMELIST SDKCREATEDTIMELIST { get; set; }

        [XmlElement(ElementName = "SDKCREATEDSYSNAME.LIST")]
        public SDKCREATEDSYSNAMELIST SDKCREATEDSYSNAMELIST { get; set; }

	
	}
	[XmlRoot(ElementName = "SDKBBDISCAMT.LIST", Namespace = "http://schemas.sdk")]
	public class SDKBBDISCAMT_LIST
	{
		[XmlElement("SDKBBDISCAMT", Namespace = "http://schemas.sdk")]
		public SDKBBDISCAMT_VALUE SDKBBDISCAMT { get; set; }
		[XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }
		[XmlAttribute(AttributeName = "TYPE")]
		public string TYPE { get; set; }
		[XmlAttribute(AttributeName = "ISLIST")]
     	public string ISLIST { get; set; }
		[XmlAttribute(AttributeName = "INDEX")]
		public string INDEX { get; set; }
		
	}
	public class SDKBBDISCAMT_VALUE
	{
		[XmlAttribute("DESC")]
		public string DESC { get; set; } = "`SdkBBDiscAmt`";

		[XmlText]
		public double Value { get; set; }
	}
	public class UDF_SDKVCHGROSSWEIGHT_LIST
	{
		[XmlElement("UDF:SDKVCHGROSSWEIGHT")]
		public decimal SDKVCHGROSSWEIGHT { get; set; }
	}

	[XmlRoot(ElementName = "PPVCHSALESMANNAME")]
    public class PPVCHSALESMANNAME
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "PPVCHSALESMANNAME.LIST")]
    public class PPVCHSALESMANNAMELIST
    {

        [XmlElement(ElementName = "PPVCHSALESMANNAME")]
        public PPVCHSALESMANNAME PPVCHSALESMANNAME { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }



    //[XmlRoot(ElementName = "ALLLEDGERENTRIES.LIST")]
    //public class ALLLEDGERENTRIES_LIST {
    //    [XmlElement(ElementName = "BANKALLOCATIONS.LIST")]
    //    public BANKALLOCATIONS_LIST BANKALLOCATIONS_LIST { get; set; }

    //}


    //   [XmlRoot(ElementName = "BANKALLOCATIONS.LIST")]
    //   public class BANKALLOCATIONS_LIST { 

    //[XmlElement(ElementName = "DATE")]
    //   public string DATE { get; set; }


    //       [XmlElement(ElementName = "INSTRUMENTDATE")]
    //       public string INSTRUMENTDATE { get; set; }

    //       [XmlElement(ElementName = "INSTRUMENTNUMBER")]
    //       public object INSTRUMENTNUMBER { get; set; }

    //       [XmlElement(ElementName = "PDCACTUALDATE")]
    //       public string PDCACTUALDATE { get; set; }

    //       [XmlElement(ElementName = "AMOUNT")]
    //       public double AMOUNT { get; set; }


    //       [XmlElement(ElementName = "NARRATION")]
    //       public string NARRATION { get; set; }

    //       [XmlElement(ElementName = "NAME")]
    //       public string NAME { get; set; }


    //   }




    [XmlRoot(ElementName = "REPORT")]
    public class REPORT
    {

        [XmlElement(ElementName = "FORMS")]
        public string FORMS { get; set; }
        [XmlElement(ElementName = "VARIABLE")]
        public string VARIABLE { get; set; }

        [XmlElement(ElementName = "SET")]
        public List<string> Set { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }

        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "FORM")]
    public class FORM
    {

        [XmlElement(ElementName = "TOPPARTS")]
        public string TOPPARTS { get; set; }

        [XmlElement(ElementName = "HEIGHT")]
        public string HEIGHT { get; set; }

        [XmlElement(ElementName = "WIDTH")]
        public string WIDTH { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlElement(ElementName = "PARTS")]
        public string PARTS { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }

        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlText]
        public string text { get; set; }
        [XmlElement(ElementName = "XMLTAG")]
        public string XMLTAG { get;  set; }
    }

    [XmlRoot(ElementName = "PART")]
    public class PART
    {

        [XmlElement(ElementName = "TOPLINES")]
        public string TOPLINES { get; set; }

        [XmlElement(ElementName = "REPEAT")]
        public string REPEAT { get; set; }

        [XmlElement(ElementName = "SCROLLED")]
        public string SCROLLED { get; set; }

        [XmlElement(ElementName = "VERTICAL")]
        public string VERTICAL { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlElement(ElementName = "LINES")]
        public string LINES { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }

        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "LINE")]
    public class LINE
    {

        [XmlElement(ElementName = "LEFTFIELDS")]
        public string LEFTFIELDS { get; set; }

        [XmlElement(ElementName = "RIGHTFIELDS")]
        public string RIGHTFIELDS { get; set; }

        [XmlElement(ElementName = "XMLtag")]
        public string XMLtag { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }

        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlElement(ElementName = "Fields")]
        public List<string> Fields { get; set; }
        [XmlElement(ElementName = "FIELD")]
        public string FIELD { get; set; }

        [XmlElement(ElementName  = "explode")]
        public List<string> explode { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "FIELD")]
    public class FIELD
    {

        [XmlElement(ElementName = "USE")]
        public string USE { get; set; }

        [XmlElement(ElementName = "SET")]
        public string SET { get; set; }

        [XmlElement(ElementName = "XMLTAG")]
        public string XMLTAG { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "ISMODIFY")]
        public string ISMODIFY { get; set; }

        [XmlAttribute(AttributeName = "ISFIXED")]
        public string ISFIXED { get; set; }

        [XmlAttribute(AttributeName = "ISINITIALIZE")]
        public string ISINITIALIZE { get; set; }

        [XmlAttribute(AttributeName = "ISOPTION")]
        public string ISOPTION { get; set; }

        [XmlAttribute(AttributeName = "ISINTERNAL")]
        public string ISINTERNAL { get; set; }

        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "EXPORTDATA")]
    public class EXPORTDATA
    {

        [XmlElement(ElementName = "REQUESTDESC")]
        public REQUESTDESC REQUESTDESC { get; set; }


        [XmlElement(ElementName = "REQUESTDATA")]
        public REQUESTDATA REQUESTDATA { get; set; }
    }

    [XmlRoot(ElementName = "TDLMESSAGE")]
    public class TDLMESSAGE
    {
        //

        [XmlElement(ElementName = "REPORT")]
        public REPORT REPORT { get; set; }

        [XmlElement(ElementName = "FORM")]
        public FORM FORM { get; set; }

        [XmlElement(ElementName = "PART")]
        public List<PART> PART { get; set; }

        [XmlElement(ElementName = "LINE")]
        public List<LINE> LINE { get; set; }

        [XmlElement(ElementName = "FIELD")]
        public List<FIELD> FIELD { get; set; }

        //


        [XmlElement(ElementName = "COLLECTION")]
        public List<COLLECTION> COLLECTION { get; set; }
        [XmlElement(ElementName = "SYSTEM")]
        public List<SYSTEM> SYSTEM { get; set; }
        [XmlElement(ElementName = "OBJECT")]
        public OBJECT? OBJECT { get; set; }
    }

    [XmlRoot(ElementName = "SYSTEM")]
    public class SYSTEM
    {

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "TDL")]
    public class TDL
    {

        [XmlElement(ElementName = "TDLMESSAGE")]
        public TDLMESSAGE TDLMESSAGE { get; set; }
    }

    [XmlRoot(ElementName = "IMPORTDATA")]
    public class IMPORTDATA
    {

        [XmlElement(ElementName = "REQUESTDESC")]
        public REQUESTDESC REQUESTDESC { get; set; }

        [XmlElement(ElementName = "REQUESTDATA")]
        public REQUESTDATA REQUESTDATA { get; set; }
    }


    [XmlRoot(ElementName = "BANKALLOCATIONS.LIST")]
    public class BANKALLOCATIONSLIST
    {

        [XmlElement(ElementName = "DATE")]
        public String DATE { get; set; }

        [XmlElement(ElementName = "INSTRUMENTDATE")]
        public string INSTRUMENTDATE { get; set; }

        [XmlElement(ElementName = "BANKNAME")]
        public object BANKNAME { get; set; }

        [XmlElement(ElementName = "INSTRUMENTNUMBER")]
        public object INSTRUMENTNUMBER { get; set; }

        [XmlElement(ElementName = "TRANSACTIONTYPE")]
        public string TRANSACTIONTYPE { get; set; }
 

        [XmlElement(ElementName = "PDCACTUALDATE")]
        public string PDCACTUALDATE { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }


        [XmlElement(ElementName = "NARRATION")]
        public string NARRATION { get; set; }

        [XmlElement(ElementName = "NAME")]
        public string NAME { get; set; }

        


    }

    [XmlRoot(ElementName = "PPSALESMAN")]
    public class PPSALESMAN
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "PPSALESMAN.LIST")]
    public class PPSALESMANLIST
    {

        [XmlElement(ElementName = "PPSALESMAN")]
        public PPSALESMAN PPSALESMAN { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "BANKALLOCATIONS.LIST")]
    public class BILLALLOCATIONSLIST
    {

        //[XmlElement(ElementName = "DATE")]
        //public int DATE { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string AMOUNT { get; set; }

        [XmlElement(ElementName = "NAME")]
        public String NAME { get; set; }

        [XmlElement(ElementName = "TDSDEDUCTEEISSPECIALRATE")]
        public string TDSDEDUCTEEISSPECIALRATE { get; set; }

        [XmlElement(ElementName = "BILLTYPE")]
        public string BILLTYPE { get; set; }


        [XmlElement(ElementName = "PPSIVCHSALESMAN.LIST")]
        public PPSIVCHSALESMANLIST PPSIVCHSALESMANLIST { get; set; }

      

        [XmlText]
        public string Text { get; set; }

 

        [XmlElement(ElementName = "PPSALESMAN.LIST")]
        public PPSALESMANLIST PPSALESMANLIST { get; set; }

    }


    [XmlRoot(ElementName = "PPSIVCHSALESMAN")]
    public class PPSIVCHSALESMAN
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "PPSIVCHSALESMAN.LIST")]
    public class PPSIVCHSALESMANLIST
    {

        [XmlElement(ElementName = "PPSIVCHSALESMAN")]
        public PPSIVCHSALESMAN PPSIVCHSALESMAN { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }



    [XmlRoot(ElementName = "ADDRESS.LIST")]
    public class ADDRESSLIST
    {

        [XmlElement(ElementName = "ADDRESS")]
        public List<string> ADDRESS { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }
        
        
    }


    [XmlRoot(ElementName = "BASICBUYERADDRESS.LIST")]
    public class BASICBUYERADDRESSLIST
    {

        [XmlElement(ElementName = "BASICBUYERADDRESS")]
        public List<string> BASICBUYERADDRESS { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "LEDGERENTRIES.LIST")]
    public class LEDGERENTRIESLIST
    {

        [XmlElement(ElementName = "LEDGERNAME")]
        public string LEDGERNAME { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string ISDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "LEDGERFROMITEM")]
        public string LEDGERFROMITEM { get; set; }

        [XmlElement(ElementName = "REMOVEZEROENTRIES")]
        public string REMOVEZEROENTRIES { get; set; }

        [XmlElement(ElementName = "ISPARTYLEDGER")]
        public string ISPARTYLEDGER { get; set; }

        [XmlElement(ElementName = "ISLASTDEEMEDPOSITIVE")]
        public string ISLASTDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }

        [XmlElement(ElementName = "OLDAUDITENTRYIDS.LIST")]
        public OLDAUDITENTRYIDSLIST OLDAUDITENTRYIDSLIST { get; set; }

        [XmlElement(ElementName = "BASICRATEOFINVOICETAX.LIST")]
        public BASICRATEOFINVOICETAXLIST BASICRATEOFINVOICETAXLIST { get; set; }

        [XmlElement(ElementName = "ROUNDTYPE")]
        public string ROUNDTYPE { get; set; }

        [XmlElement(ElementName = "GSTCLASS")]
        public object GSTCLASS { get; set; }

        [XmlElement(ElementName = "VATEXPAMOUNT")]
        public double VATEXPAMOUNT { get; set; }
        [XmlElement(ElementName = "CATEGORYALLOCATIONS.LIST")]
        public List<CATEGORYALLOCATIONSLIST> CATEGORYALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "BILLALLOCATIONS.LIST")]
        public BILLALLOCATIONSLIST BILLALLOCATIONSLIST { get; set; }
    }

    [XmlRoot(ElementName = "ATCSTKITEMBATCHNETRATE")]
    public class ATCSTKITEMBATCHNETRATE
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ATCSTKITEMBATCHNETRATE.LIST")]
    public class ATCSTKITEMBATCHNETRATELIST
    {

        [XmlElement(ElementName = "ATCSTKITEMBATCHNETRATE")]
        public ATCSTKITEMBATCHNETRATE ATCSTKITEMBATCHNETRATE { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "BATCHALLOCATIONS.LIST")]
    public class BATCHALLOCATIONSLIST
    {

        [XmlElement(ElementName = "GODOWNNAME")]
        public string GODOWNNAME { get; set; }

        [XmlElement(ElementName = "BATCHNAME")]
        public string BATCHNAME { get; set; }

        [XmlElement(ElementName = "DESTINATIONGODOWNNAME")]
        public string DESTINATIONGODOWNNAME { get; set; }

        [XmlElement(ElementName = "INDENTNO")]
        public object INDENTNO { get; set; }

        [XmlElement(ElementName = "ORDERNO")]
        public string ORDERNO { get; set; }

        [XmlElement(ElementName = "DYNAMICCSTISCLEARED")]
        public string DYNAMICCSTISCLEARED { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }

        [XmlElement(ElementName = "ACTUALQTY")]
        public string ACTUALQTY { get; set; }

        [XmlElement(ElementName = "BILLEDQTY")]
        public string BILLEDQTY { get; set; }

        [XmlElement(ElementName = "ORDERDUEDATE")]
        public String ORDERDUEDATE { get; set; }

        [XmlElement(ElementName = "ATCSTKITEMBATCHNETRATE.LIST")]
        public ATCSTKITEMBATCHNETRATELIST ATCSTKITEMBATCHNETRATELIST { get; set; }
    }


    [XmlRoot(ElementName = "ACCOUNTINGALLOCATIONS.LIST")]
    public class ACCOUNTINGALLOCATIONSLIST
    {

        [XmlElement(ElementName = "LEDGERNAME")]
        public string LEDGERNAME { get; set; }

        [XmlElement(ElementName = "GSTCLASS")]
        public object GSTCLASS { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string ISDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "LEDGERFROMITEM")]
        public string LEDGERFROMITEM { get; set; }

        [XmlElement(ElementName = "REMOVEZEROENTRIES")]
        public string REMOVEZEROENTRIES { get; set; }

        [XmlElement(ElementName = "ISPARTYLEDGER")]
        public string ISPARTYLEDGER { get; set; }

        [XmlElement(ElementName = "ISLASTDEEMEDPOSITIVE")]
        public string ISLASTDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }
        [XmlElement(ElementName = "CATEGORYALLOCATIONS.LIST")]
        public CATEGORYALLOCATIONSLIST CATEGORYALLOCATIONSLIST { get;  set; }
    }

    [XmlRoot(ElementName = "ALLINVENTORYENTRIES.LIST")]
    public class ALLINVENTORYENTRIESLIST
    {

        [XmlElement(ElementName = "STOCKITEMNAME")]
        public string STOCKITEMNAME { get; set; }

        [XmlElement(ElementName = "SUBCATEGORY")]
        public string SUBCATEGORY { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string ISDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "ISLASTDEEMEDPOSITIVE")]
        public string ISLASTDEEMEDPOSITIVE { get; set; }

        [XmlElement(ElementName = "ISAUTONEGATE")]
        public string ISAUTONEGATE { get; set; }

        [XmlElement(ElementName = "ISCUSTOMSCLEARANCE")]
        public string ISCUSTOMSCLEARANCE { get; set; }

        [XmlElement(ElementName = "ISTRACKCOMPONENT")]
        public string ISTRACKCOMPONENT { get; set; }

        [XmlElement(ElementName = "ISTRACKPRODUCTION")]
        public string ISTRACKPRODUCTION { get; set; }

        [XmlElement(ElementName = "ISPRIMARYITEM")]
        public string ISPRIMARYITEM { get; set; }

        [XmlElement(ElementName = "ISSCRAP")]
        public string ISSCRAP { get; set; }

        [XmlElement(ElementName = "RATE")]
        public string RATE { get; set; }

        [XmlElement(ElementName = "DISCOUNT")]
        public double DISCOUNT { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public double AMOUNT { get; set; }

        [XmlElement(ElementName = "ACTUALQTY")]
        public string ACTUALQTY { get; set; }

        [XmlElement(ElementName = "BILLEDQTY")]
        public string BILLEDQTY { get; set; }

        [XmlElement(ElementName = "BATCHALLOCATIONS.LIST")]
        public BATCHALLOCATIONSLIST BATCHALLOCATIONSLIST { get; set; }

        [XmlElement(ElementName = "ACCOUNTINGALLOCATIONS.LIST")]
        public ACCOUNTINGALLOCATIONSLIST ACCOUNTINGALLOCATIONSLIST { get; set; }
        [XmlElement(ElementName = "BASICUSERDESCRIPTION.LIST")]
        public BASICUSERDESCRIPTIONLIST BASICUSERDESCRIPTIONLIST { get; set; }
	
		[XmlElement("SDKBBDISCAMT.LIST", Namespace = "http://schemas.sdk")]
		public SDKBBDISCAMT_LIST SDKBBDISCAMT_LIST { get; set; }

	

	}

    [XmlRoot(ElementName = "BASICUSERDESCRIPTION.LIST")]
    public class BASICUSERDESCRIPTIONLIST
    {

        [XmlElement(ElementName = "BASICUSERDESCRIPTION")]
        public string BASICUSERDESCRIPTION { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "OLDAUDITENTRYIDS.LIST")]
    public class OLDAUDITENTRYIDSLIST
    {

        [XmlElement(ElementName = "OLDAUDITENTRYIDS")]
        public string OLDAUDITENTRYIDS { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "BASICRATEOFINVOICETAX.LIST")]
    public class BASICRATEOFINVOICETAXLIST
    {

        [XmlElement(ElementName = "BASICRATEOFINVOICETAX")]
        public object BASICRATEOFINVOICETAX { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }
    }

  

    [XmlRoot(ElementName = "VAT")]
    public class VAT
    {

        [XmlElement(ElementName = "VATLEDGERNAME")]
        public string VATLEDGERNAME { get; set; }

        [XmlElement(ElementName = "VATAMOUNT")]
        public string VATAMOUNT { get; set; }
        [XmlElement(ElementName = "INVNAME")]
        public string INVNAME { get; set; }

        [XmlElement(ElementName = "INVAMOUNT")]
        public string INVAMOUNT { get; set; }

        [XmlElement(ElementName = "ACTUALQTY")]
        public string ACTUALQTY { get; set; }

        [XmlElement(ElementName = "BILLEDQTY")]
        public string BILLEDQTY { get; set; }

        [XmlElement(ElementName = "INVRATE")]
        public string INVRATE { get; set; }

        [XmlElement(ElementName = "SALESLEDGER")]
        public string SALESLEDGER { get; set; }
      
        



    }
	[XmlRoot(ElementName = "LEDMAILINGDETAILS.LIST")]
    public class LEDMAILINGDETAILS
    {

    }
	[XmlRoot(ElementName = "VOUCHERS")]
    public class VOUCHERS
    {

        [XmlElement(ElementName = "VMASTERID")]
        public string VMASTERID { get; set; }

        [XmlElement(ElementName = "VOUCHERDATE")]
        public string VOUCHERDATE { get; set; }

        [XmlElement(ElementName = "VTYPENAME")]
        public string VTYPENAME { get; set; }

        [XmlElement(ElementName = "VOUCHERNUMBER")]
        public string VOUCHERNUMBER { get; set; }


        //[XmlElement(ElementName = "VAT")]

        //public VAT VAT { get; set; }


        [XmlElement(ElementName = "VAT")]
        public List<VAT> VAT { get; set; }
        [XmlElement(ElementName = "REFERENCE")]
        public String REFERENCE { get; set; }


        
    }



    [XmlRoot(ElementName = "MAILINGNAME.LIST")]
    public class MAILINGNAMELIST
    {

        [XmlElement(ElementName = "MAILINGNAME")]
        public string MAILINGNAME { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "LEDGER")]
    public class LEDGER
    {

        [XmlElement(ElementName = "ADDRESS.LIST")]
        public ADDRESSLIST ADDRESSLIST { get; set; }

        [XmlElement(ElementName = "MAILINGNAME.LIST")]
        public MAILINGNAMELIST MAILINGNAMELIST { get; set; }

        [XmlElement(ElementName = "OLDAUDITENTRYIDS.LIST")]
        public OLDAUDITENTRYIDSLIST OLDAUDITENTRYIDSLIST { get; set; }

		[XmlElement(ElementName = "LEDMAILINGDETAILS.LIST")]
        public LEDMAILINGDETAILSLIST LEDMAILINGDETAILSLIST { get; set; }

		[XmlElement(ElementName = "STARTINGFROM")]
        public string STARTINGFROM { get; set; }

        [XmlElement(ElementName = "GUID")]
        public string GUID { get; set; }

        [XmlElement(ElementName = "COUNTRYNAME")]
        public string COUNTRYNAME { get; set; }

        [XmlElement(ElementName = "STATENAME")]
        public string STATENAME { get; set; }

        [XmlElement(ElementName = "INTERSTATESTNUMBER")]
        public object INTERSTATESTNUMBER { get; set; }

        [XmlElement(ElementName = "VATTINNUMBER")]
        public object VATTINNUMBER { get; set; }

        [XmlElement(ElementName = "VATDEALERTYPE")]
        public string VATDEALERTYPE { get; set; }

        [XmlElement(ElementName = "PARENT")]
        public string PARENT { get; set; }

        [XmlElement(ElementName = "TAXCLASSIFICATIONNAME")]
        public object TAXCLASSIFICATIONNAME { get; set; }

        [XmlElement(ElementName = "TAXTYPE")]
        public string TAXTYPE { get; set; }

        [XmlElement(ElementName = "COUNTRYOFRESIDENCE")]
        public string COUNTRYOFRESIDENCE { get; set; }

        [XmlElement(ElementName = "GSTTYPE")]
        public object GSTTYPE { get; set; }

        [XmlElement(ElementName = "APPROPRIATEFOR")]
        public object APPROPRIATEFOR { get; set; }

        [XmlElement(ElementName = "SERVICECATEGORY")]
        public string SERVICECATEGORY { get; set; }

        [XmlElement(ElementName = "EXCISELEDGERCLASSIFICATION")]
        public object EXCISELEDGERCLASSIFICATION { get; set; }

        [XmlElement(ElementName = "EXCISEDUTYTYPE")]
        public object EXCISEDUTYTYPE { get; set; }

        [XmlElement(ElementName = "EXCISENATUREOFPURCHASE")]
        public object EXCISENATUREOFPURCHASE { get; set; }

        [XmlElement(ElementName = "LEDGERFBTCATEGORY")]
        public object LEDGERFBTCATEGORY { get; set; }

        [XmlElement(ElementName = "LEDSTATENAME")]
        public string LEDSTATENAME { get; set; }

        [XmlElement(ElementName = "ISBILLWISEON")]
        public string ISBILLWISEON { get; set; }

        [XmlElement(ElementName = "ISCOSTCENTRESON")]
        public string ISCOSTCENTRESON { get; set; }

        [XmlElement(ElementName = "ISINTERESTON")]
        public string ISINTERESTON { get; set; }

        [XmlElement(ElementName = "ALLOWINMOBILE")]
        public string ALLOWINMOBILE { get; set; }

        [XmlElement(ElementName = "ISCOSTTRACKINGON")]
        public string ISCOSTTRACKINGON { get; set; }

        [XmlElement(ElementName = "ISBENEFICIARYCODEON")]
        public string ISBENEFICIARYCODEON { get; set; }

        [XmlElement(ElementName = "ISUPDATINGTARGETID")]
        public string ISUPDATINGTARGETID { get; set; }

        [XmlElement(ElementName = "ASORIGINAL")]
        public string ASORIGINAL { get; set; }

        [XmlElement(ElementName = "ISCONDENSED")]
        public string ISCONDENSED { get; set; }

        [XmlElement(ElementName = "AFFECTSSTOCK")]
        public string AFFECTSSTOCK { get; set; }

        [XmlElement(ElementName = "ISRATEINCLUSIVEVAT")]
        public string ISRATEINCLUSIVEVAT { get; set; }

        [XmlElement(ElementName = "FORPAYROLL")]
        public string FORPAYROLL { get; set; }

        [XmlElement(ElementName = "ISABCENABLED")]
        public string ISABCENABLED { get; set; }

        [XmlElement(ElementName = "ISCREDITDAYSCHKON")]
        public string ISCREDITDAYSCHKON { get; set; }

        [XmlElement(ElementName = "INTERESTONBILLWISE")]
        public string INTERESTONBILLWISE { get; set; }

        [XmlElement(ElementName = "OVERRIDEINTEREST")]
        public string OVERRIDEINTEREST { get; set; }

        [XmlElement(ElementName = "OVERRIDEADVINTEREST")]
        public string OVERRIDEADVINTEREST { get; set; }

        [XmlElement(ElementName = "USEFORVAT")]
        public string USEFORVAT { get; set; }

        [XmlElement(ElementName = "IGNORETDSEXEMPT")]
        public string IGNORETDSEXEMPT { get; set; }

        [XmlElement(ElementName = "ISTCSAPPLICABLE")]
        public string ISTCSAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISTDSAPPLICABLE")]
        public string ISTDSAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISFBTAPPLICABLE")]
        public string ISFBTAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISGSTAPPLICABLE")]
        public string ISGSTAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISEXCISEAPPLICABLE")]
        public string ISEXCISEAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISTDSEXPENSE")]
        public string ISTDSEXPENSE { get; set; }

        [XmlElement(ElementName = "ISEDLIAPPLICABLE")]
        public string ISEDLIAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISRELATEDPARTY")]
        public string ISRELATEDPARTY { get; set; }

        [XmlElement(ElementName = "USEFORESIELIGIBILITY")]
        public string USEFORESIELIGIBILITY { get; set; }

        [XmlElement(ElementName = "ISINTERESTINCLLASTDAY")]
        public string ISINTERESTINCLLASTDAY { get; set; }

        [XmlElement(ElementName = "APPROPRIATETAXVALUE")]
        public string APPROPRIATETAXVALUE { get; set; }

        [XmlElement(ElementName = "ISBEHAVEASDUTY")]
        public string ISBEHAVEASDUTY { get; set; }

        [XmlElement(ElementName = "INTERESTINCLDAYOFADDITION")]
        public string INTERESTINCLDAYOFADDITION { get; set; }

        [XmlElement(ElementName = "INTERESTINCLDAYOFDEDUCTION")]
        public string INTERESTINCLDAYOFDEDUCTION { get; set; }

        [XmlElement(ElementName = "OVERRIDECREDITLIMIT")]
        public string OVERRIDECREDITLIMIT { get; set; }

        [XmlElement(ElementName = "ISAGAINSTFORMC")]
        public string ISAGAINSTFORMC { get; set; }

        [XmlElement(ElementName = "ISCHEQUEPRINTINGENABLED")]
        public string ISCHEQUEPRINTINGENABLED { get; set; }

        [XmlElement(ElementName = "ISPAYUPLOAD")]
        public string ISPAYUPLOAD { get; set; }

        [XmlElement(ElementName = "ISPAYBATCHONLYSAL")]
        public string ISPAYBATCHONLYSAL { get; set; }

        [XmlElement(ElementName = "ISBNFCODESUPPORTED")]
        public string ISBNFCODESUPPORTED { get; set; }

        [XmlElement(ElementName = "ALLOWEXPORTWITHERRORS")]
        public string ALLOWEXPORTWITHERRORS { get; set; }

        [XmlElement(ElementName = "USEFORNOTIONALITC")]
        public string USEFORNOTIONALITC { get; set; }

        [XmlElement(ElementName = "SHOWINPAYSLIP")]
        public string SHOWINPAYSLIP { get; set; }

        [XmlElement(ElementName = "USEFORGRATUITY")]
        public string USEFORGRATUITY { get; set; }

        [XmlElement(ElementName = "ISTDSPROJECTED")]
        public string ISTDSPROJECTED { get; set; }

        [XmlElement(ElementName = "FORSERVICETAX")]
        public string FORSERVICETAX { get; set; }

        [XmlElement(ElementName = "ISINPUTCREDIT")]
        public string ISINPUTCREDIT { get; set; }

        [XmlElement(ElementName = "ISEXEMPTED")]
        public string ISEXEMPTED { get; set; }

        [XmlElement(ElementName = "ISABATEMENTAPPLICABLE")]
        public string ISABATEMENTAPPLICABLE { get; set; }

        [XmlElement(ElementName = "ISSTXPARTY")]
        public string ISSTXPARTY { get; set; }

        [XmlElement(ElementName = "ISSTXNONREALIZEDTYPE")]
        public string ISSTXNONREALIZEDTYPE { get; set; }

        [XmlElement(ElementName = "ISUSEDFORCVD")]
        public string ISUSEDFORCVD { get; set; }

        [XmlElement(ElementName = "LEDBELONGSTONONTAXABLE")]
        public string LEDBELONGSTONONTAXABLE { get; set; }

        [XmlElement(ElementName = "ISEXCISEMERCHANTEXPORTER")]
        public string ISEXCISEMERCHANTEXPORTER { get; set; }

        [XmlElement(ElementName = "ISPARTYEXEMPTED")]
        public string ISPARTYEXEMPTED { get; set; }

        [XmlElement(ElementName = "ISSEZPARTY")]
        public string ISSEZPARTY { get; set; }

        [XmlElement(ElementName = "TDSDEDUCTEEISSPECIALRATE")]
        public string TDSDEDUCTEEISSPECIALRATE { get; set; }

        [XmlElement(ElementName = "ISECHEQUESUPPORTED")]
        public string ISECHEQUESUPPORTED { get; set; }

        [XmlElement(ElementName = "ISEDDSUPPORTED")]
        public string ISEDDSUPPORTED { get; set; }

        [XmlElement(ElementName = "HASECHEQUEDELIVERYMODE")]
        public string HASECHEQUEDELIVERYMODE { get; set; }

        [XmlElement(ElementName = "HASECHEQUEDELIVERYTO")]
        public string HASECHEQUEDELIVERYTO { get; set; }

        [XmlElement(ElementName = "HASECHEQUEPRINTLOCATION")]
        public string HASECHEQUEPRINTLOCATION { get; set; }

        [XmlElement(ElementName = "HASECHEQUEPAYABLELOCATION")]
        public string HASECHEQUEPAYABLELOCATION { get; set; }

        [XmlElement(ElementName = "HASECHEQUEBANKLOCATION")]
        public string HASECHEQUEBANKLOCATION { get; set; }

        [XmlElement(ElementName = "HASEDDDELIVERYMODE")]
        public string HASEDDDELIVERYMODE { get; set; }

        [XmlElement(ElementName = "HASEDDDELIVERYTO")]
        public string HASEDDDELIVERYTO { get; set; }

        [XmlElement(ElementName = "HASEDDPRINTLOCATION")]
        public string HASEDDPRINTLOCATION { get; set; }

        [XmlElement(ElementName = "HASEDDPAYABLELOCATION")]
        public string HASEDDPAYABLELOCATION { get; set; }

        [XmlElement(ElementName = "HASEDDBANKLOCATION")]
        public string HASEDDBANKLOCATION { get; set; }

        [XmlElement(ElementName = "ISEBANKINGENABLED")]
        public string ISEBANKINGENABLED { get; set; }

        [XmlElement(ElementName = "ISEXPORTFILEENCRYPTED")]
        public string ISEXPORTFILEENCRYPTED { get; set; }

        [XmlElement(ElementName = "ISBATCHENABLED")]
        public string ISBATCHENABLED { get; set; }

        [XmlElement(ElementName = "ISPRODUCTCODEBASED")]
        public string ISPRODUCTCODEBASED { get; set; }

        [XmlElement(ElementName = "HASEDDCITY")]
        public string HASEDDCITY { get; set; }

        [XmlElement(ElementName = "HASECHEQUECITY")]
        public string HASECHEQUECITY { get; set; }

        [XmlElement(ElementName = "ISFILENAMEFORMATSUPPORTED")]
        public string ISFILENAMEFORMATSUPPORTED { get; set; }

        [XmlElement(ElementName = "HASCLIENTCODE")]
        public string HASCLIENTCODE { get; set; }

        [XmlElement(ElementName = "PAYINSISBATCHAPPLICABLE")]
        public string PAYINSISBATCHAPPLICABLE { get; set; }

        [XmlElement(ElementName = "PAYINSISFILENUMAPP")]
        public string PAYINSISFILENUMAPP { get; set; }

        [XmlElement(ElementName = "ISSALARYTRANSGROUPEDFORBRS")]
        public string ISSALARYTRANSGROUPEDFORBRS { get; set; }

        [XmlElement(ElementName = "ISEBANKINGSUPPORTED")]
        public string ISEBANKINGSUPPORTED { get; set; }

        [XmlElement(ElementName = "ISSCBUAE")]
        public string ISSCBUAE { get; set; }

        [XmlElement(ElementName = "ISBANKSTATUSAPP")]
        public string ISBANKSTATUSAPP { get; set; }

        [XmlElement(ElementName = "ISSALARYGROUPED")]
        public string ISSALARYGROUPED { get; set; }

        [XmlElement(ElementName = "USEFORPURCHASETAX")]
        public string USEFORPURCHASETAX { get; set; }

        [XmlElement(ElementName = "AUDITED")]
        public string AUDITED { get; set; }

        [XmlElement(ElementName = "SORTPOSITION")]
        public string SORTPOSITION { get; set; }

        [XmlElement(ElementName = "SERVICETAXDETAILS.LIST")]
        public object SERVICETAXDETAILSLIST { get; set; }

        [XmlElement(ElementName = "LBTREGNDETAILS.LIST")]
        public object LBTREGNDETAILSLIST { get; set; }

        [XmlElement(ElementName = "VATDETAILS.LIST")]
        public object VATDETAILSLIST { get; set; }

        [XmlElement(ElementName = "LANGUAGENAME.LIST")]
        public LANGUAGENAMELIST LANGUAGENAMELIST { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "RESERVEDNAME")]
        public string RESERVEDNAME { get; set; }

        [XmlElement(ElementName = "PARTYGSTIN")]
        public string PARTYGSTIN { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "LANGUAGENAME.LIST")]
    public class LANGUAGENAMELIST
    {

        [XmlElement(ElementName = "NAME.LIST")]
        public NAMELIST NAMELIST { get; set; }

        [XmlElement(ElementName = "LANGUAGEID")]
        public int LANGUAGEID { get; set; }
    }

    [XmlRoot(ElementName = "NAME.LIST")]
    public class NAMELIST
    {

        [XmlElement(ElementName = "NAME")]
        public List<string> NAME { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    //test : for sql query execution response

    [XmlRoot(ElementName = "COL")]
    public class COL
    {

        [XmlElement(ElementName = "NAME")]
        public string NAME { get; set; }

        [XmlElement(ElementName = "ALIAS")]
        public string ALIAS { get; set; }

        [XmlElement(ElementName = "TYPE")]
        public string TYPE { get; set; }

        [XmlElement(ElementName = "LENGTH")]
        public int LENGTH { get; set; }

        [XmlElement(ElementName = "PRECISION")]
        public int PRECISION { get; set; }

        [XmlElement(ElementName = "NULLABLE")]
        public string NULLABLE { get; set; }
    }

    [XmlRoot(ElementName = "ROWDESC")]
    public class ROWDESC
    {

        [XmlElement(ElementName = "COL")]
        public List<COL> COL { get; set; }
    }

    [XmlRoot(ElementName = "RESULTDESC")]
    public class RESULTDESC
    {

        [XmlElement(ElementName = "ROWDESC")]
        public ROWDESC ROWDESC { get; set; }
    }

    [XmlRoot(ElementName = "ROW")]
    public class ROW
    {

        [XmlElement(ElementName = "COL")]
        public List<string> COL { get; set; }
    }

    [XmlRoot(ElementName = "RESULTDATA")]
    public class RESULTDATA
    {

        [XmlElement(ElementName = "ROW")]
        public List<ROW> ROW { get; set; }
    }

    [XmlRoot(ElementName = "EXPORTDATARESPONSE")]
    public class EXPORTDATARESPONSE
    {

        [XmlElement(ElementName = "RESULTDESC")]
        public RESULTDESC RESULTDESC { get; set; }

        [XmlElement(ElementName = "RESULTDATA")]
        public RESULTDATA RESULTDATA { get; set; }

        [XmlAttribute(AttributeName = "ResultType")]
        public string ResultType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    //-----------------------------------------------------------------------------------------









    [XmlRoot(ElementName = "PARENT")]
    public class PARENT
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "APPLICABLEFROM")]
    public class APPLICABLEFROM
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "CALCULATIONTYPE")]
    public class CALCULATIONTYPE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "HSNCODE")]
    public class HSNCODE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "HSNMASTERNAME")]
    public class HSNMASTERNAME
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "TAXABILITY")]
    public class TAXABILITY
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "RATEDETAILS.LIST")]
    public class RATEDETAILSLIST
    {

        [XmlElement(ElementName = "GSTRATEDUTYHEAD")]
        public string GSTRATEDUTYHEAD { get; set; }

        [XmlElement(ElementName = "GSTRATEVALUATIONTYPE")]
        public string GSTRATEVALUATIONTYPE { get; set; }

        [XmlElement(ElementName = "GSTRATE")]
        public string GSTRATE { get; set; }

        [XmlElement(ElementName = "GSTRATEPERUNIT")]
        public string GSTRATEPERUNIT { get; set; }
    }

    [XmlRoot(ElementName = "STATEWISEDETAILS.LIST")]
    public class STATEWISEDETAILSLIST
    {

        [XmlElement(ElementName = "STATENAME")]
        public string STATENAME { get; set; }

        [XmlElement(ElementName = "RATEDETAILS.LIST")]
        public List<RATEDETAILSLIST> RATEDETAILSLIST { get; set; } = new();

        [XmlElement(ElementName = "GSTSLABRATES.LIST")]
        public object GSTSLABRATESLIST { get; set; }
    }

    [XmlRoot(ElementName = "GSTDETAILS.LIST")]
    public class GSTDETAILSLIST
    {

        [XmlElement(ElementName = "APPLICABLEFROM")]
        public APPLICABLEFROM APPLICABLEFROM { get; set; }

        [XmlElement(ElementName = "CALCULATIONTYPE")]
        public CALCULATIONTYPE CALCULATIONTYPE { get; set; }

        [XmlElement(ElementName = "HSNCODE")]
        public HSNCODE HSNCODE { get; set; }

        [XmlElement(ElementName = "HSNMASTERNAME")]
        public HSNMASTERNAME HSNMASTERNAME { get; set; }

        [XmlElement(ElementName = "TAXABILITY")]
        public TAXABILITY TAXABILITY { get; set; }

        [XmlElement(ElementName = "STATEWISEDETAILS.LIST")]
        public STATEWISEDETAILSLIST STATEWISEDETAILSLIST { get; set; } = new();
    }


    //for stockitem mrp getting along with getCompanyStockItemGSTWithCessRateXml request

    [XmlRoot(ElementName = "FROMDATE")]
    public class FROMDATE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "STATENAME")]
    public class STATENAME
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MRPRATE")]
    public class MRPRATE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MRPRATEDETAILS.LIST")]
    public class MRPRATEDETAILSLIST
    {

        [XmlElement(ElementName = "STATENAME")]
        public STATENAME STATENAME { get; set; }

        [XmlElement(ElementName = "MRPRATE")]
        public MRPRATE MRPRATE { get; set; } = new();

        [XmlElement(ElementName = "PRIORSTATENAME")]
        public PRIORSTATENAME PRIORSTATENAME { get; set; }
    }

    [XmlRoot(ElementName = "MRPDETAILS.LIST")]
    public class MRPDETAILSLIST
    {

        [XmlElement(ElementName = "FROMDATE")]
        public FROMDATE FROMDATE { get; set; }

        [XmlElement(ElementName = "MRPRATEDETAILS.LIST")]
        public MRPRATEDETAILSLIST MRPRATEDETAILSLIST { get; set; }=new();
    }

    [XmlRoot(ElementName = "PRIORSTATENAME")]
    public class PRIORSTATENAME
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "OBJECT")]
    public class OBJECT
    {

        [XmlElement(ElementName = "LOCALFORMULA")]
        public string LOCALFORMULA { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CURRENTCOMPANY")]
    public class CURRENTCOMPANY
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "CURRENTCOMPANY")]
        public CURRENTCOMPANY CurrentCompany { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }
    }

    

    [XmlRoot(ElementName = "NAME")]
    public class NAME
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CLASSRATE")]
    public class CLASSRATE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public double Text { get; set; }
    }

    [XmlRoot(ElementName = "GSTCLASSIFICATIONNATURE")]
    public class GSTCLASSIFICATIONNATURE
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "LEDGERFROMITEM")]
    public class LEDGERFROMITEM
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "REMOVEZEROENTRIES")]
    public class REMOVEZEROENTRIES
    {

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SALESLIST.LIST")]
    public class SALESLISTLIST
    {

        [XmlElement(ElementName = "NAME")]
        public NAME NAME { get; set; }

        [XmlElement(ElementName = "CLASSRATE")]
        public CLASSRATE CLASSRATE { get; set; }

        [XmlElement(ElementName = "GSTCLASSIFICATIONNATURE")]
        public GSTCLASSIFICATIONNATURE GSTCLASSIFICATIONNATURE { get; set; }

        [XmlElement(ElementName = "LEDGERFROMITEM")]
        public LEDGERFROMITEM LEDGERFROMITEM { get; set; }

        [XmlElement(ElementName = "REMOVEZEROENTRIES")]
        public REMOVEZEROENTRIES REMOVEZEROENTRIES { get; set; }

        [XmlElement(ElementName = "DUTYHEADDETAILS.LIST")]
        public object DUTYHEADDETAILSLIST { get; set; }
    }

    //for vehicle details on vansales
    [XmlRoot(ElementName = "EWAYBILLDETAILS.LIST")]
    public class EWAYBILLDETAILSLIST
    {

        [XmlElement(ElementName = "CONSIGNORADDRESS.LIST")]
        public CONSIGNORADDRESSLIST CONSIGNORADDRESSLIST { get; set; }

        [XmlElement(ElementName = "CONSIGNEEADDRESS.LIST")]
        public CONSIGNEEADDRESSLIST CONSIGNEEADDRESSLIST { get; set; }

        [XmlElement(ElementName = "BILLDATE")]
        public string BILLDATE { get; set; }

        [XmlElement(ElementName = "DOCUMENTTYPE")]
        public string DOCUMENTTYPE { get; set; }

        [XmlElement(ElementName = "CONSIGNEEPINCODE")]
        public string CONSIGNEEPINCODE { get; set; }

        [XmlElement(ElementName = "BILLNUMBER")]
        public string BILLNUMBER { get; set; }

        [XmlElement(ElementName = "SUBTYPE")]
        public string SUBTYPE { get; set; }

        [XmlElement(ElementName = "CONSIGNORPLACE")]
        public string CONSIGNORPLACE { get; set; }

        [XmlElement(ElementName = "CONSIGNORPINCODE")]
        public string CONSIGNORPINCODE { get; set; }

        [XmlElement(ElementName = "CONSIGNEEPLACE")]
        public string CONSIGNEEPLACE { get; set; }

        [XmlElement(ElementName = "SHIPPEDFROMSTATE")]
        public string SHIPPEDFROMSTATE { get; set; }

        [XmlElement(ElementName = "SHIPPEDTOSTATE")]
        public string SHIPPEDTOSTATE { get; set; }

        [XmlElement(ElementName = "IGNOREGSTINVALIDATION")]
        public string IGNOREGSTINVALIDATION { get; set; }

        [XmlElement(ElementName = "ISCANCELLED")]
        public string ISCANCELLED { get; set; }

        [XmlElement(ElementName = "ISCANCELPENDING")]
        public string ISCANCELPENDING { get; set; }

        [XmlElement(ElementName = "IGNOREGENERATIONVALIDATION")]
        public string IGNOREGENERATIONVALIDATION { get; set; }

        [XmlElement(ElementName = "ISEXPORTEDFORGENERATION")]
        public string ISEXPORTEDFORGENERATION { get; set; }

        [XmlElement(ElementName = "VALIDUPTO")]
        public string VALIDUPTO { get; set; }

        [XmlElement(ElementName = "UPDATEDDATE")]
        public string UPDATEDDATE { get; set; }

        [XmlElement(ElementName = "TRANSPORTDETAILS.LIST")]
        public TRANSPORTDETAILSLIST TRANSPORTDETAILSLIST { get; set; }

        [XmlElement(ElementName = "EXTENSIONDETAILS.LIST")]
        public object EXTENSIONDETAILSLIST { get; set; }
    }


    [XmlRoot(ElementName = "TRANSPORTDETAILS.LIST")]
    public class TRANSPORTDETAILSLIST
    {

        [XmlElement(ElementName = "TRANSPORTMODE")]
        public string TRANSPORTMODE { get; set; }

        [XmlElement(ElementName = "VEHICLENUMBER")]
        public string VEHICLENUMBER { get; set; }

        [XmlElement(ElementName = "VEHICLETYPE")]
        public string VEHICLETYPE { get; set; }

        [XmlElement(ElementName = "IGNOREVEHICLENOVALIDATION")]
        public string IGNOREVEHICLENOVALIDATION { get; set; }

        [XmlElement(ElementName = "ISTRANSIDPENDING")]
        public string ISTRANSIDPENDING { get; set; }

        [XmlElement(ElementName = "ISTRANSIDUPDATED")]
        public string ISTRANSIDUPDATED { get; set; }

        [XmlElement(ElementName = "IGNORETRANSIDVALIDATION")]
        public string IGNORETRANSIDVALIDATION { get; set; }

        [XmlElement(ElementName = "ISEXPORTEDFORTRANSPORTERID")]
        public string ISEXPORTEDFORTRANSPORTERID { get; set; }

        [XmlElement(ElementName = "ISPARTBPENDING")]
        public string ISPARTBPENDING { get; set; }

        [XmlElement(ElementName = "ISPARTBUPDATED")]
        public string ISPARTBUPDATED { get; set; }

        [XmlElement(ElementName = "IGNOREPARTBVALIDATION")]
        public string IGNOREPARTBVALIDATION { get; set; }

        [XmlElement(ElementName = "ISEXPORTEDFORPARTB")]
        public string ISEXPORTEDFORPARTB { get; set; }

        [XmlElement(ElementName = "DISTANCE")]
        public string DISTANCE { get; set; }
    }


    [XmlRoot(ElementName = "CONSIGNEEADDRESS.LIST")]
    public class CONSIGNEEADDRESSLIST
    {

        [XmlElement(ElementName = "CONSIGNEEADDRESS")]
        public string CONSIGNEEADDRESS { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "CONSIGNORADDRESS.LIST")]
    public class CONSIGNORADDRESSLIST
    {

        [XmlElement(ElementName = "CONSIGNORADDRESS")]
        public string CONSIGNORADDRESS { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "SDKGWTOTALS.LIST")]
    public class SDKGWTOTALSLIST
    {

        [XmlElement(ElementName = "SDKGWTOTALS")]
        public SDKGWTOTALS SDKGWTOTALS { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SALESMASTERID")]
    public class SALESMASTERID
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SALESMASTERID.LIST")]
    public class SALESMASTERIDLIST
    {

        [XmlElement(ElementName = "SALESMASTERID")]
        public SALESMASTERID? SALESMASTERID { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string INDEX { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDDATE")]
    public class SDKCREATEDDATE
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDDATE.LIST")]
    public class SDKCREATEDDATELIST
    {

        [XmlElement(ElementName = "SDKCREATEDDATE")]
        public SDKCREATEDDATE SDKCREATEDDATE { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public string? INDEX { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKOWNVEHICLEF")]
    public class SDKOWNVEHICLEF
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKOWNVEHICLEF.LIST")]
    public class SDKOWNVEHICLEFLIST
    {

        [XmlElement(ElementName = "SDKOWNVEHICLEF")]
        public SDKOWNVEHICLEF SDKOWNVEHICLEF { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKSALESMAN2F")]
    public class SDKSALESMAN2F
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKSALESMAN2F.LIST")]
    public class SDKSALESMAN2FLIST
    {

        [XmlElement(ElementName = "SDKSALESMAN2F")]
        public SDKSALESMAN2F SDKSALESMAN2F { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKDRIVERLISTF")]
    public class SDKDRIVERLISTF
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKDRIVERLISTF.LIST")]
    public class SDKDRIVERLISTFLIST
    {

        [XmlElement(ElementName = "SDKDRIVERLISTF")]
        public SDKDRIVERLISTF SDKDRIVERLISTF { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKOWNVEHICLEF1")]
    public class SDKOWNVEHICLEF1
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKOWNVEHICLEF1.LIST")]
    public class SDKOWNVEHICLEF1LIST
    {

        [XmlElement(ElementName = "SDKOWNVEHICLEF1")]
        public SDKOWNVEHICLEF1 SDKOWNVEHICLEF1 { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKSYNCSELVOUCHERS")]
    public class SDKSYNCSELVOUCHERS
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKSYNCSELVOUCHERS.LIST")]
    public class SDKSYNCSELVOUCHERSLIST
    {

        [XmlElement(ElementName = "SDKSYNCSELVOUCHERS")]
        public SDKSYNCSELVOUCHERS SDKSYNCSELVOUCHERS { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDBY")]
    public class SDKCREATEDBY
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDBY.LIST")]
    public class SDKCREATEDBYLIST
    {

        [XmlElement(ElementName = "SDKCREATEDBY")]
        public SDKCREATEDBY SDKCREATEDBY { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDTIME")]
    public class SDKCREATEDTIME
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDTIME.LIST")]
    public class SDKCREATEDTIMELIST
    {

        [XmlElement(ElementName = "SDKCREATEDTIME")]
        public SDKCREATEDTIME SDKCREATEDTIME { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDSYSNAME")]
    public class SDKCREATEDSYSNAME
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKCREATEDSYSNAME.LIST")]
    public class SDKCREATEDSYSNAMELIST
    {

        [XmlElement(ElementName = "SDKCREATEDSYSNAME")]
        public SDKCREATEDSYSNAME SDKCREATEDSYSNAME { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKGWTOTALS")]
    public class SDKGWTOTALS
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKRENTVEHICLEF")]
    public class SDKRENTVEHICLEF
    {

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SDKRENTVEHICLEF.LIST")]
    public class SDKRENTVEHICLEFLIST
    {

        [XmlElement(ElementName = "SDKRENTVEHICLEF")]
        public SDKRENTVEHICLEF SDKRENTVEHICLEF { get; set; }

        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }

        [XmlAttribute(AttributeName = "ISLIST")]
        public string ISLIST { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        [XmlAttribute(AttributeName = "INDEX")]
        public int INDEX { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
	[XmlRoot(ElementName = "ADDRESS.LIST")]
	public class ADDRESSLIST1
	{

		[XmlElement(ElementName = "ADDRESS")]
		public List<string> ADDRESS { get; set; }

		[XmlAttribute(AttributeName = "TYPE")]
		public string TYPE { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "LEDMAILINGDETAILS.LIST")]
	public class LEDMAILINGDETAILSLIST
	{

		[XmlElement(ElementName = "ADDRESS.LIST")]
		public ADDRESSLIST ADDRESSLIST { get; set; }

		[XmlElement(ElementName = "APPLICABLEFROM")]
		public string APPLICABLEFROM { get; set; }

		[XmlElement(ElementName = "PINCODE")]
		public string PINCODE { get; set; }

		[XmlElement(ElementName = "MAILINGNAME")]
		public string MAILINGNAME { get; set; }

		[XmlElement(ElementName = "STATE")]
		public string STATE { get; set; }

		[XmlElement(ElementName = "COUNTRY")]
		public string COUNTRY { get; set; }
	}




}
