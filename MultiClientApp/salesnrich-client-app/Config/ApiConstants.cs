using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Config
{
    public static class ApiConstants
    {
       
		public static readonly string prefixstatic= ApplicationProperties.properties["prefix"].ToString();
        //public static readonly string PREFIX = "/api/tp/v1";
        public static readonly string PREFIX = prefixstatic;
        public static readonly string AUTHENTICATION = "/api/authenticate";
        public static readonly String VALIDATEAPP = "/validate.json";
	public static readonly String SAVE_HARD_DISK_NO = "/api/tp/v1/save-hard-disk-client?key=";

	// product wise details
	public static readonly String PRODUCT_GROUP = "/product-group-alterid.json";
	public static readonly String PRODUCT_CATEGORY = "/product-categories-alterid.json";
	public static readonly String STOCK_ITEM = "/product-profile-alterid.json";
	public static readonly String PRICE_LEVEL = "/price-levels.json";
	public static readonly String PRICE_LIST_LEVEL_LIST = "/price-level-list.json";
	public static readonly String OPENING_STOCK = "/opening-stock.json";
	public static readonly String TEMPORARY_OPENING_STOCK = "/temporary-opening-stock.json";
	public static readonly String STOCK_LOCATION = "/stock-locations.json";
	public static readonly String PRODUCTGROUP_PRODUCTPROFILE = "/product-group_product-profile.json";
	public static readonly String PRODUCT_WISE_DEFAULT_LEDGER = "/product-wise-default-ledger.json";
	public static readonly String GST_GROUP_WISE = "/gst-product-group.json";
	public static readonly String TAX_MASTER = "/tax-master.json";
	public static readonly String PRODUCT_PROFILE_TAX_MASTER = "/product-profile-tax-master.json";
	public static readonly String GST_LEDGERS = "/gst-ledger-tally";
	public static readonly String ALTERID_MASTER ="/alterid-master.json";
	
//	idClientApp urls
	public static readonly String STOCK_ITEM_ID = "/product-profileUpdateIdNew-alterId.json";
	public static readonly String PRODUCT_GROUP_ID = "/product-groupdataUpdateIdNew-alterID.json";
	public static readonly String PRODUCT_CATEGORY_ID = "/product-categoriesalterIDUpdateIdNew.json";
	public static readonly String ACCOUNT_PROFILE_ID = "/account-profilesUpdateId-alterId.json";
	public static readonly String LOCATION_ACCOUNT_PROFILE_ID = "/location-account-alterid-profileUpdateId.json";//SNR_CLIENT_GA_RP_2
		public static readonly String LOCATION_ID = "/locationsUpdateId-alterID.json";
	public static readonly String LOCATION_HIERARCHY_ID = "/location-hierarchyUpdateIdNew.json";
	public static readonly String OPENING_STOCK_ID = "/opening-stockUpdatedIdNew.json";
	public static readonly String PRODUCTGROUP_PRODUCTPROFILE_ID = "/product-group_product-profileUpdateIdNew.json";
	public static readonly String GST_GROUP_WISE_ID = "/gst-product-groupUpdatedId.json";
	public static readonly String PRICE_LIST_LEVEL_LIST_ID = "/price-level-listUpdatedIdNew.json";
	public static readonly String RECEIVALE_PAYABLES_ID = "/receivable-payableUpdatedIdNew.json";
	public static readonly String ACCOUNT_PROFILE_CLOSING_BALANCE_ID = "/account-profiles-closing-balanceUpdateIdNew.json";//SNR_CLIENT_CBAP_2
		public static readonly String TEMPORARY_OPENING_STOCK_ID = "/temporary-opening-stockUpdatedIdNew.json";
	public static readonly String PRODUCT_WISE_DEFAULT_LEDGER_ID = "/product-wise-default-ledgerIdNem.json";
	
	
	
	// account wise details
	public static readonly String LOCATION = "/locations-alterid.json";
	public static readonly String LOCATION_HIERARCHY = "/location-hierarchy.json";
	public static readonly String ACCOUNT_PROFILE = "/account-profiles-alterid.json";
	public static readonly String ACCOUNT_PROFILE_CLOSING_BALANCE = "/account-profiles-closing-balance.json";//SNR_CLIENT_CBAP_1
		public static readonly String LOCATION_ACCOUNT_PROFILE = "/location-account-profile-alterid.json";//SNR_CLIENT_GA_RP_1
		public static readonly String RECEIVALE_PAYABLES = "/receivable-payable.json";
	public static readonly String POST_DATED_VOUCHERS = "/post-dated-vouchers";

	// download details
//	public static readonly String DOWNLOAD_ORDER = "/api/tp/get-sales-orders.json";
//	public static readonly String UPDATE_ORDER_STATUS = "/api/tp/update-order-status";
//	public static readonly String DOWNLOAD_RECEIPT = "/api/tp/get-receipts.json";
//	public static readonly String UPDATE_RECEIPT_STATUS = "/api/tp/update-receipt-status";
	public static readonly String DOWNLOAD_ORDER = "/api/tp/v2/get-sales-orders.json";
        public static readonly String DOWNLOAD_ORDER_ISOPTIMIZED = "/api/tp/optimised/get-sales-orders.json";
        public static readonly String DOWNLOAD_SALES = "/api/tp/v2/get-sales.json";
		public static readonly String DOWNLOAD_BY_VOUCHER_TYPE = "/api/tp/optimised/get-sales-by-voucher.json";
        public static readonly String DOWNLOAD_BY_VOUCHER_TYPE_OPTIMIZED_DISTRIBUTDCODE = "api/v2/vouchers/sales-orders/download-batch";
        public static readonly String DOWNLOAD_BY_VOUCHER_TYPE_OPTIMIZED_DISTRIBUTDCODE_SELECTED_DATE = "";
        public static readonly String DOWNLOAD_BY_VOUCHER_TYPE_OPTIMIZED_DISTRIBUTDCODE_ENABLEDATE = "";
        public static readonly String DOWNLOAD_BY_VOUCHER_TYPE_NOT_OPTIMIZED = "/api/tp/v2/get-sales-by-voucher.json";
        public static readonly String UPDATE_ORDER_STATUS = "/api/tp/v2/update-order-status";
		public static readonly String UPDATE_ORDER_STATUS_PENDING = "/api/tp/v2/update-order-status-pending";
		public static readonly String DOWNLOAD_RECEIPT = "/api/tp/optimised/get-receipts.json";//SNR_CLIENT_ReptDLDREciptDateByDate_1
		public static readonly String DOWNLOAD_RECEIPT_NOT_OPTIMIZED = "/api/tp/v2/get-receipts.json";//SNR_CLIENT_ReptDLDNOTOPTIMIZED_1
		public static readonly String DOWNLOAD_SALES_RETURN = "/api/tp/optimised/get-sales-return.json";
        public static readonly String DOWNLOAD_SALES_OPTIMIZED = "/api/tp/optimised/get-sales.json";
        public static readonly String DOWNLOAD_ORDER_WITHDISTRIBUTED_CODE = "/api/v2/vouchers/sales-orders/download-batch";
		 
        public static readonly String UPDATE_RECEIPT_STATUS = "/api/tp/v2/update-receipt-status";//SNR_CLIENT_ReptDLDStatusUpdate_1
		public static readonly String UPDATE_RECEIPT_STATUS_PENDING = "/api/tp//v2/update-receipt-status-pending";

		public static readonly String DOWNLOAD_LEDGER = "/api/tp/get-ledgers.json";
	public static readonly String UPDATE_LEDGER_STATUS = "/api/tp/update-ledgers-status";
	public static readonly String SYNC_OPERATION_STATUS = "/api/tp/syncOperationStatus";
	public static readonly String ASSIGNED_SYNC_OPERATIONS = "/api/tp/assigned-syncOperations";

	public static readonly String UPLOAD_INVENTORY_VOUCHERS = "/api/tp/tally-inventory-vouchers";
	public static readonly String UPLOAD_ACCOUNTING_VOUCHERS = "/api/tp/tally-accounting-vouchers";

	public static readonly String SAVE_LOG_TO_SERVER = "/save-log-files";

	public static readonly String FIRST_TIME_ASSOCIATION = "/first-time-association.json";
		public static readonly String UPDATE_STATUS = "/api/update-voucher-no";
        public static readonly String UPDATE_STATUS_OF_SALES_ORDER = "/api/update-sales-status";
        public static readonly String GET_SALESVOUCHERNUMBER_FROM_SERVER = "/api/get-all-voucher-nos";
        public static readonly String CLIENTAPP_PROPERTIES_TO_SERVER = "/api/create-properties";
        public static readonly String CLIENTAPP_PROPERTIES_FROM_SERVER = "/api/get-properties";
        public static readonly String CHECK_DISTRIBUTED_ENABLED = "/api/tally-settings";
        public static readonly String GET_DISTRIBUTEDCODE = "/api/user-distributer-details";
    }
}
