using System;
using System.Data;
using CORNCommon.Classes;
using CORNDatabaseLayer.Classes;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using CORNDataAccessLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    public class VenderEntryController
    {


        #region Constructor

        /// <summary>
        /// Constructor for OrderEntryController
        /// </summary>
        public VenderEntryController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Select

        #region Vendoer Ledger

        public DataSet GetVendorLedger(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();

                uspGetVendorLedgerReport mLedger = new uspGetVendorLedgerReport();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TO_DATE = p_TO_DATE;

                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptCustomerLedgerView"].ImportRow(dr);

                }

                spSelectCHEQUE_PROCESS2 mLedgerSub = new spSelectCHEQUE_PROCESS2();

                mLedgerSub.Connection = mConnection;
                mLedgerSub.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedgerSub.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedgerSub.CUSTOMER_ID = Convert.ToInt32(p_PRINCIPAL_ID);
                mLedgerSub.STATUS_ID = 527528;// Recieved and deposit Cheques

                DataTable dtPro = mLedgerSub.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["spSelectCHEQUE_PROCESS2"].ImportRow(dr);
                }

                return ds;
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

        public DataTable GetVendorOpening(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();


                uspGetVendorLedgerOpening mLedger = new uspGetVendorLedgerOpening();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;

                DataTable DT = mLedger.ExecuteTable();
                return DT;
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

        public DataSet GetSupplierClosingSummary(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetSupplierClosingSummary mLedger = new uspGetSupplierClosingSummary();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.TO_DATE = p_TO_DATE;

                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierClosingSummary"].ImportRow(dr);

                }
                return ds;
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

        public DataSet GetSupplierStatments(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();

                uspGetSupplierStatmentReport mLedger = new uspGetSupplierStatmentReport();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TO_DATE = p_TO_DATE;

                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierStatmentReport"].ImportRow(dr);
                }
                return ds;
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

        #endregion

        #region Supplier Payable Ageing
        public DataSet GetSupplierAgeing(string p_PRINCIPAL_IDs, string p_DISTRIBUTOR_IDs,DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetSupplierAging mLedger = new uspGetSupplierAging();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_IDs = p_PRINCIPAL_IDs;
                mLedger.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                mLedger.TO_DATE = p_TO_DATE;

                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierAging"].ImportRow(dr);

                }
                return ds;
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

        #endregion
        #endregion
    }
}
