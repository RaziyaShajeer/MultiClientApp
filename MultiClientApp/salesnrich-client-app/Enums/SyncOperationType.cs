﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Enums
{
    internal enum SyncOperationType
    {
        PRODUCTCATEGORY, PRODUCTGROUP, PRODUCTPROFILE, PRODUCTGROUP_PRODUCTPROFILE, PRICE_LEVEL, PRICE_LEVEL_LIST,
        STOCK_LOCATION, OPENING_STOCK, ACCOUNT_PROFILE, LOCATION, LOCATION_HIRARCHY, LOCATION_ACCOUNT_PROFILE,
        RECEIVABLE_PAYABLE, PRODUCT_WISE_DEFAULT_LEDGER, ACCOUNT_PROFILE_CLOSING_BALANCE, GST_PRODUCT_GROUP, TAX_MASTER,
        PRODUCT_PROFILE_TAX_MASTER, EXECUTIVE_TASK_EXECUTION, SALES_ORDER, SALES_VOUCHER, RECEIPT, DAILY_RECEIPT,
        DOWNLOAD_ACCOUNT_PROFILE, POST_DATED_VOUCHER, GST_LEDGER, SALES_BY_VOUCHER, JOURNAL, TEMPORARY_OPENING_STOCK,
        SALES_JOURNAL, PRICELEVEL_ACCOUNT_PRODUCTGROUP, TRIAL_RUN,SALES_RETURN
    }
}
