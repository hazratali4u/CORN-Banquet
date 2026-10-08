using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace CORNBusinessLayer.Classes

{
    /// <summary>
    /// Class For Account Head Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Account Head
    /// </item>
    /// <term>
    /// Update Account Head
    /// </term>
    /// <item>
    /// Get Account Head
    /// </item>
    /// <item>
    /// Assigns/UnAssings Account Head To Principal
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class EventBookingController
    {
        IDbTransaction mTransaction;
        IDbConnection mConnection;

        #region Constructor

        /// <summary>
        /// Constructor for AccountHeadController
        /// </summary>
        public EventBookingController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Methods

        public DataTable GetEventTypes()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectEVENT_TYPE mDistributorInfo = new spSelectEVENT_TYPE();
                mDistributorInfo.Connection = mConnection;
                DataTable dt = mDistributorInfo.ExecuteTable();
                return dt;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataTable SelectDineTimes(DateTime eventDate, int p_HALL_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectDine_TIME mDistributorInfo = new spSelectDine_TIME();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.Event_Date = eventDate;
                mDistributorInfo.HALL_ID = p_HALL_ID;
                mDistributorInfo.TYPE_ID = p_TYPE_ID;
                DataTable dt = mDistributorInfo.ExecuteTable();
                return dt;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataTable Select_Event_Booking_Lookup(long? p_EVENT_BOOKING_ID, int p_USER_ID)
        {
            try
            {
                spSelectEVENT_BOOKING mEVENT_Booking = new spSelectEVENT_BOOKING();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mEVENT_Booking.Connection = mConnection;
                mEVENT_Booking.EVENT_BOOKING_ID = p_EVENT_BOOKING_ID;
                mEVENT_Booking.USER_ID = p_USER_ID;
                DataTable dt = mEVENT_Booking.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        public DataTable Select_Event_Booking_Details(long? p_EVENT_BOOKING_ID)
        {
            try
            {
                spSelectEVENT_BOOKING_DETAIL mEVENT_Booking = new spSelectEVENT_BOOKING_DETAIL();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mEVENT_Booking.Connection = mConnection;
                mEVENT_Booking.EVENT_BOOKING_ID = p_EVENT_BOOKING_ID;
                DataTable dt = mEVENT_Booking.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        #region Insert, Update

        public string InsertEventBooking(int p_customer_Id, DateTime p_event_Date, long p_dine_time, Int16 p_dine_type,
            DateTime p_booking_Date, int p_Ladies, int p_Gents, int p_user_Id, int p_distributor_Id, int p_hall_Id,
            DataTable Items, decimal p_TOTAL_AMOUNT, decimal p_ADVANCE_AMOUNT, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                //decimal remainingAmount = p_TOTAL_AMOUNT - (!string.IsNullOrEmpty(p_ADVANCE_AMOUNT) ? Convert.ToDecimal(p_ADVANCE_AMOUNT) : 0);

                decimal Remaining_Amount = p_TOTAL_AMOUNT - p_ADVANCE_AMOUNT;
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertEVENT_BOOKING_Master mISom = new spInsertEVENT_BOOKING_Master
                {
                    Connection = mConnection,
                    Transaction = mTransaction,
                    EVENT_DATE = p_event_Date,
                    CUSTOMER_ID = p_customer_Id,
                    EVENT_TIME_ID = p_dine_time,
                    DOCUMENT_DATE = DateTime.Now,
                    EVENT_TYPE_ID = p_dine_type,
                    BOOKING_DATE = p_booking_Date,
                    TableDefination_ID = p_hall_Id,
                    USER_ID = p_user_Id,
                    LADIES = p_Ladies,
                    GENTS = p_Gents,
                    DISTRIBUTOR_ID = p_distributor_Id,
                    TOTAL_AMOUNT = p_TOTAL_AMOUNT,
                    ADVANCE_AMOUNT = p_ADVANCE_AMOUNT,
                    BALANCE_AMOUNT = Remaining_Amount,
                    CREDIT_AMOUNT = Remaining_Amount,
                    STATUS_ID = 1,
                    FORCE_CLOSED = 0
                };
                mISom.ExecuteQuery();

                if (Items.Rows.Count > 0)
                {
                    //----------------Insert into sale order detail-------------
                    spInsertEVENT_BOOKING_DETAIL mDetail = new spInsertEVENT_BOOKING_DETAIL
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in Items.Rows)
                    {
                        mDetail.EVENT_BOOKING_ID = mISom.EVENT_BOOKING_ID;
                        mDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mDetail.Category_Type_ID = int.Parse(dr["Category_Type_ID"].ToString());
                        mDetail.RATE = decimal.Parse(dr["PRICE"].ToString());
                        mDetail.QTY = decimal.Parse(dr["QTY"].ToString());
                        mDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mDetail.UOM_ID = int.Parse(dr["UOM_ID"].ToString());
                        mDetail.ExecuteQuery();
                    }
                }

                #region Credit Invoice
                DataRow[] drConfig2 = null;

                LedgerController LController2 = new LedgerController();
                string VoucherNo2 = LController2.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_distributor_Id, 0);

                drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, p_TOTAL_AMOUNT, 0, DateTime.Now,
                    "Credit Sale Default", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, 0, p_ADVANCE_AMOUNT, DateTime.Now,
                    "Credit Sale Default Advance Amount Received", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, 0, Remaining_Amount, DateTime.Now,
                    "Credit Sale Default", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                #endregion

                #region Commented Code
                //if (IsFinanceIntegrate)

                //{
                //    #region GL Master, Detail

                //    LedgerController LController = new LedgerController();

                //    string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_distributor_Id,
                //        DateTime.Now);

                //    if (LController.PostingGLMaster(p_distributor_Id, 0, VoucherNo, Constants.Journal_Voucher,
                //        DateTime.Now, Constants.Document_SaleInvoice, Convert.ToString(mISom.EVENT_BOOKING_ID),
                //        "Sale Voucher, Inv#: " + mISom.EVENT_BOOKING_ID.ToString(), p_user_Id, "EventBooking",
                //        Constants.Document_SaleInvoice, mISom.EVENT_BOOKING_ID, mTransaction, mConnection))
                //    {
                //        DataRow[] drConfig = null;

                //        //Dr  Credit Sale Receivable
                //        //Cr  Credit Sales

                //        drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                //        LController.PostingGLDetail(p_distributor_Id, 0, Constants.Journal_Voucher, VoucherNo,
                //            Convert.ToInt64(drConfig[0]["VALUE"].ToString()), p_TOTAL_AMOUNT, 0,
                //            "Credit Card Sale Voucher", mTransaction, mConnection);
                //        drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                //        LController.PostingGLDetail(p_distributor_Id, 0, Constants.Journal_Voucher, VoucherNo,
                //            Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, p_TOTAL_AMOUNT,
                //            "Credit Card Sale Voucher", mTransaction, mConnection);

                //    }

                //    #endregion
                //}
                #endregion
                mTransaction.Commit();
                return mISom.EVENT_BOOKING_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        public string UpdateEventBooking(long p_event_Booking_Id, int p_customer_Id, DateTime p_event_Date, long p_dine_time, Int16 p_dine_type,
                   DateTime p_booking_Date, int p_Ladies, int p_Gents, int p_user_Id, int p_distributor_Id, int p_hall_Id,
                   DataTable Items, decimal p_TOTAL_AMOUNT, decimal p_ADVANCE_AMOUNT, decimal p_CREDIT_AMOUNT,
                   bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                //decimal remainingAmount = totalAmount - (!string.IsNullOrEmpty(advanceAmount) ? Convert.ToDecimal(advanceAmount) : 0);
                decimal Remaining_Amount = p_TOTAL_AMOUNT - p_ADVANCE_AMOUNT;
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spUpdateEVENT_BOOKING_Master mISom = new spUpdateEVENT_BOOKING_Master
                {
                    Connection = mConnection,
                    Transaction = mTransaction,
                    EVENT_DATE = p_event_Date,
                    CUSTOMER_ID = p_customer_Id,
                    EVENT_TIME_ID = p_dine_time,
                    DOCUMENT_DATE = DateTime.Now,
                    EVENT_TYPE_ID = p_dine_type,
                    BOOKING_DATE = p_booking_Date,
                    TableDefination_ID = p_hall_Id,
                    EVENT_BOOKING_ID = p_event_Booking_Id,
                    USER_ID = p_user_Id,
                    LADIES = p_Ladies,
                    GENTS = p_Gents,
                    DISTRIBUTOR_ID = p_distributor_Id,
                    TOTAL_AMOUNT = p_TOTAL_AMOUNT,
                    ADVANCE_AMOUNT = p_ADVANCE_AMOUNT,
                    BALANCE_AMOUNT = Remaining_Amount,
                    CREDIT_AMOUNT = p_CREDIT_AMOUNT,
                    STATUS_ID = 1,
                    FORCE_CLOSED = 0
                };
                mISom.ExecuteQuery();

                if (Items.Rows.Count > 0)
                {
                    //----------------Insert into sale order detail-------------
                    spInsertEVENT_BOOKING_DETAIL mDetail = new spInsertEVENT_BOOKING_DETAIL
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in Items.Rows)
                    {
                        mDetail.EVENT_BOOKING_ID = mISom.EVENT_ID;
                        mDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mDetail.Category_Type_ID = int.Parse(dr["Category_Type_ID"].ToString());
                        mDetail.RATE = decimal.Parse(dr["PRICE"].ToString());
                        mDetail.QTY = decimal.Parse(dr["QTY"].ToString());
                        mDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mDetail.UOM_ID = int.Parse(dr["UOM_ID"].ToString());
                        mDetail.ExecuteQuery();
                    }
                }

                #region Credit Invoice
                DataRow[] drConfig2 = null;

                LedgerController LController2 = new LedgerController();
                string VoucherNo2 = LController2.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_distributor_Id, 0);

                drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, p_TOTAL_AMOUNT, 0, DateTime.Now,
                    "Credit Sale Default", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, 0, p_ADVANCE_AMOUNT, DateTime.Now,
                    "Credit Sale Default Advance Amount Received", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), p_distributor_Id, 0, Remaining_Amount, DateTime.Now,
                    "Credit Sale Default", DateTime.Now, 0, int.Parse(p_customer_Id.ToString()), mISom.EVENT_BOOKING_ID, "", Constants.Document_SaleInvoice, p_user_Id, mTransaction,
                    mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                #endregion
                #region Commented Code
                //if (IsFinanceIntegrate)

                //{
                //    #region GL Master, Detail

                //    //LedgerController LController = new LedgerController();

                //    //string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_distributor_Id,
                //    //    DateTime.Now);

                //    //if (LController.PostingGLMaster(p_distributor_Id, 0, VoucherNo, Constants.Journal_Voucher,
                //    //    DateTime.Now, Constants.Document_SaleInvoice, Convert.ToString(mISom.EVENT_BOOKING_ID),
                //    //    "Sale Voucher, Inv#: " + mISom.EVENT_BOOKING_ID.ToString(), p_user_Id, "EventBooking",
                //    //    Constants.Document_SaleInvoice, mISom.EVENT_BOOKING_ID, mTransaction, mConnection))
                //    //{
                //    //    DataRow[] drConfig = null;

                //    //    //Dr  Credit Sale Receivable
                //    //    //Cr  Credit Sales

                //    //    drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                //    //    LController.PostingGLDetail(p_distributor_Id, 0, Constants.Journal_Voucher, VoucherNo,
                //    //        Convert.ToInt64(drConfig[0]["VALUE"].ToString()), totalAmount, 0,
                //    //        "Credit Card Sale Voucher", mTransaction, mConnection);
                //    //    drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                //    //    LController.PostingGLDetail(p_distributor_Id, 0, Constants.Journal_Voucher, VoucherNo,
                //    //        Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, totalAmount,
                //    //        "Credit Card Sale Voucher", mTransaction, mConnection);

                //    //}

                //    #endregion
                //}
                #endregion
                mTransaction.Commit();
                return mISom.EVENT_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        #endregion

        #endregion
    }
}
