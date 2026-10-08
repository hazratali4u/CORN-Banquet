using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using CORNDatabaseLayer.InputClasses;
using CORNDatabaseLayer.InputClasses.EventBooking;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Inventory Reports
    /// </summary>
    public class RptInventoryController
    {
        #region Constructor

        /// <summary>
        /// Constructor for RptInventoryController
        /// </summary>
        public RptInventoryController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets Data For Stock Reconciliation Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_USER_ID">User</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPrincipalStockReconcilation(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID, int p_UOM_ID, int p_PRICE_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockRegister ObjPrint = new uspRptSelectStockRegister();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.Company_Id = p_Principal_Id;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.UOM_ID = p_UOM_ID;
                ObjPrint.PRICE_TYPE = p_PRICE_TYPE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["StockRegister"].ImportRow(dr);
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

        public DataSet SelectSkuStockStatus(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID, int p_UOM_ID, int p_PRICE_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockStatus ObjPrint = new uspRptSelectStockStatus();
                CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.Company_Id = p_Principal_Id;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.UOM_ID = p_UOM_ID;
                ObjPrint.PRICE_TYPE = p_PRICE_TYPE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspRptSelectStockStatus"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Date Wise Stock Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_TypeId">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPurchaseTransferStock(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId, int p_RATE_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspDailyPurchaseTransfer ObjPrint = new UspDailyPurchaseTransfer();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.TYPEID = p_TypeId;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.RATE_TYPE = p_RATE_TYPE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["DailyPurchaseTransferReport"].ImportRow(dr);
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



        public DataSet SelectDailyTopSellerReport(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_type, int Upto)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspDailyTopSellerReport ObjPrint = new UspDailyTopSellerReport();
                CORNBusinessLayer.Reports.LatestDataSet ds = new CORNBusinessLayer.Reports.LatestDataSet();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;

                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                if (p_type == 0)
                {

                    DataView dv = dt.DefaultView;
                    dv.Sort = "UnitsSold desc";
                    DataTable sortedDT = dv.ToTable();

                    DataTable dt1 = SelectTopDataRow(sortedDT, Upto);


                    foreach (DataRow dr in dt1.Rows)
                    {
                        ds.Tables["UspDailyTopSellerReport"].ImportRow(dr);
                    }
                }
                else
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "UnitsSold ASC";
                    DataTable sortedDT = dv.ToTable();
                    DataTable dt1 = SelectTopDataRow(sortedDT, Upto);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        ds.Tables["UspDailyTopSellerReport"].ImportRow(dr);
                    }
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
        public DataTable SelectTopDataRow(DataTable dt, int countt)
        {



            DataTable dtn = dt.Clone();

            if (countt > 0)
            {
                if (dt.Rows.Count > countt)
                {

                    for (int i = 0; i < countt; i++)
                    {
                        dtn.ImportRow(dt.Rows[i]);
                    }
                    return dtn;
                }
                else
                {
                    return dt;
                }
            }
            else
            {
                return dt;
            }

        }
        #region Transfer In/Out Report

        /// <summary>
        /// Gets Data For Transfer In/Out Report (In Value)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_FromTime">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_TransferType">Type</param>
        /// <param name="p_type">ReportTyp</param>
        /// <returns>DataSet</returns>
        public DataSet TransferInOutValue(int p_Principal_ID, int p_Distributor_ID, DateTime p_FromTime, DateTime p_ToDate, string p_TransferType, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspTransferInOutValue mTransferIn = new UspTransferInOutValue();
                mTransferIn.Connection = mConnection;

                mTransferIn.PRINCIPAL_ID = p_Principal_ID;
                mTransferIn.DISTRIBUTOR_ID = p_Distributor_ID;
                mTransferIn.FromDate = p_FromTime;
                mTransferIn.ToDate = p_ToDate;
                mTransferIn.TransferType = p_TransferType;
                mTransferIn.ReportType = p_type;
                DataTable DT = mTransferIn.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptTransferInOutValueWise"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Transfer In/Out Report (In Quantity And Carton)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_FromTime">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_TransferType">Type</param>
        /// <param name="p_type">ReportType</param>
        /// <returns>DataSet</returns>
        public DataSet TransferIn(int p_Principal_ID, int p_Distributor_ID, DateTime p_FromTime, DateTime p_ToDate, string p_TransferType, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspTransferInrpt mTransferIn = new UspTransferInrpt();
                mTransferIn.Connection = mConnection;

                mTransferIn.PRINCIPAL_ID = p_Principal_ID;
                mTransferIn.DISTRIBUTOR_ID = p_Distributor_ID;
                mTransferIn.FromDate = p_FromTime;
                mTransferIn.ToDate = p_ToDate;
                mTransferIn.TransferType = p_TransferType;
                mTransferIn.ReportType = p_type;
                DataTable DT = mTransferIn.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["TransferIn"].ImportRow(dr);
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

        #region  Physical Stock Report

        /// <summary>
        /// Gets Data For  Physical Stock Report (SKU Wise)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Date">Date</param>
        /// <returns>DataSet</returns>
        public DataSet PhysicalStockTaking(int p_Principal_ID, int p_Distributor_ID, DateTime p_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                RptPhysicalStockTaking mStockTaking = new RptPhysicalStockTaking();

                mStockTaking.Connection = mConnection;
                mStockTaking.PRINCIPAL_ID = p_Principal_ID;
                mStockTaking.DISTRIBUTOR_ID = p_Distributor_ID;
                mStockTaking.Date = p_Date;

                DataTable DT = mStockTaking.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["PhysicalStockTaking"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For  Physical Stock Report (Value Wise)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Date">Date</param>
        /// <param name="p_UserId">User</param>
        /// <returns>DataSet</returns>
        public DataSet PhysicalStockTakingValueWise(int p_Principal_ID, int p_Distributor_ID, DateTime p_Date, int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                RptPhysicalStockTakingSummary mStockTaking = new RptPhysicalStockTakingSummary();

                mStockTaking.Connection = mConnection;
                mStockTaking.PRINCIPAL_ID = p_Principal_ID;
                mStockTaking.DISTRIBUTOR_ID = p_Distributor_ID;
                mStockTaking.Date = p_Date;
                mStockTaking.USER_ID = p_UserId;

                DataTable DT = mStockTaking.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptPhysicalStockValue"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Purchase Document Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_TypeId">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPurchaseDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchaseDocument"].ImportRow(dr);
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

        public DataSet selectSupllierWisePurchase(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSupllierWisePurchase ObjPrint = new spSupllierWisePurchase();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.USER_ID = p_UserId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSupllierWisePurchase"].ImportRow(dr);
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

        public DataSet SelectPurchaseDocumentPopUp(int p_Purchase_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocumentPopUp ObjPrint = new GetPurchasedocumentPopUp();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DOCUMENT_ID = p_Purchase_ID;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchasedocument"].ImportRow(dr);
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

        public DataSet SelectTransferDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptTransferocument"].ImportRow(dr);
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

        public DataSet SelectTransferDocumentPopUp(int p_Purchase_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocumentPopUp ObjPrint = new GetPurchasedocumentPopUp();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DOCUMENT_ID = p_Purchase_ID;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptTransferocument"].ImportRow(dr);
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

        public DataSet SelectIssueDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId, long pPurchaseId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                ObjPrint.DOCUMENT_ID = pPurchaseId;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchaseDocument"].ImportRow(dr);
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

        public DataSet SelectRawIssuenceReport(int DistributorId, DateTime FromDate, DateTime ToDate, string SKU_ID, int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectRawMaterialIssuence ObjPrint = new spSelectRawMaterialIssuence();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = SKU_ID;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectRawMaterialIssuence"].ImportRow(dr);
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

        public DataSet SelectDateWiseStokReport(int DistributorId, DateTime FromDate, DateTime ToDate, string SKU_ID, int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectdatewisestocktaking ObjPrint = new uspSelectdatewisestocktaking();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = SKU_ID;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspSelectdatewisestocktaking"].ImportRow(dr);
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


        public DataSet selectStockDemandReport(string  DistributorId, DateTime FromDate, DateTime ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandReprot ObjPrint = new spSelectStockDemandReprot();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandReprot"].ImportRow(dr);
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

        public DataSet StockDemandPrint(int p_DemandId,int p_DISTRIBUTOR_ID,DateTime p_FROM_DATE,DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandForPrint ObjPrint = new spSelectStockDemandForPrint();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DEMAND_ID = p_DemandId;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.FROM_DATE = p_FROM_DATE;
                ObjPrint.TO_DATE = p_TO_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandForPrint"].ImportRow(dr);
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

        public DataSet StockDemandPrint(int p_DemandId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandForPrint ObjPrint = new spSelectStockDemandForPrint();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DEMAND_ID = p_DemandId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandForPrint"].ImportRow(dr);
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

        public DataSet GetPhysicalStockTaking(int DistributorId, DateTime FromDate, DateTime ToDate,int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectdatewisestocktaking ObjPrint = new uspSelectdatewisestocktaking();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = null;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["PhysicalStockTaking"].ImportRow(dr);
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

        #region Added By Hazrat Ali

        /// <summary>
        /// Gets Data For Stock Valuation Report
        /// </summary>
        /// <param name="p_StockDate">Date</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_USER_ID">User</param>
        /// <param name="p_ReportType">ReportType</param>
        /// <returns>DataSet</returns>
        public DataSet SelectStockValuation(DateTime p_StockDate, int p_Distributor_ID, int p_Principal_ID, int p_USER_ID, int p_ReportType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetStockValuation ObjPrint = new uspGetStockValuation();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DATE = p_StockDate;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_ID;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.TYPE = p_ReportType;
                DataTable dt = ObjPrint.ExecuteTable();
                if (p_ReportType == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["rptStockValuationDetail"].ImportRow(dr);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["rptStockValuationSummary"].ImportRow(dr);
                    }
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

        public DataSet GetStockVariation(int p_DISTRIBUTOR_ID,DateTime p_STOCK_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetStockVariation ObjPrint = new uspGetStockVariation();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.STOCK_DATE = p_STOCK_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetStockVariation"].ImportRow(dr);
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

        public DataSet GetStockValuation2(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE,string p_cetegoryIDs,int p_TypeId,int p_userID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetStockValuation2 ObjPrint = new uspGetStockValuation2();
                Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.STOCK_DATE = p_STOCK_DATE;
                ObjPrint.CETEGORYIDS = p_cetegoryIDs;
                ObjPrint.TYPE = p_TypeId;
                ObjPrint.USER_ID = p_userID;
                ObjPrint.PRINCIPAL_ID = Constants.IntNullValue;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetStockValuation2"].ImportRow(dr);
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

        public DataSet GetItemWisePurchase(string p_DISTRIBUTOR_ID, string p_CATEGORY_ID,string p_SKU_ID,DateTime p_FROM_DATE,DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemWisePurchase ObjPrint = new uspGetItemWisePurchase();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.CATEGORY_ID = p_CATEGORY_ID;
                ObjPrint.SKU_ID = p_SKU_ID;
                ObjPrint.FROM_DATE = p_FROM_DATE;
                ObjPrint.TO_DATE = p_TO_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetItemWisePurchase"].ImportRow(dr);
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
        public DataSet GetPhysicalStockTaking(string p_DISTRIBUTOR_IDs,DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetPhysicalStockTaking mLedger = new uspGetPhysicalStockTaking();

                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.FROM_DATE = p_FROM_DATE;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetPhysicalStockTaking"].ImportRow(dr);

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

        public DataSet SelectEventPopUp(long p_EVENT_BOOKING_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetEventBookingPopUp ObjPrint = new GetEventBookingPopUp();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.EVENT_BOOKING_ID = p_EVENT_BOOKING_ID;
                ObjPrint.TYPE_ID = p_TYPE_ID;
                DataSet dt = ObjPrint.ExecuteDatSet();

                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    ds.Tables["GetEventBookingPopUp"].ImportRow(dr);
                }

                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    ds.Tables["GetEventBookingPopUpDetail"].ImportRow(dr);
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

        public DataTable SelectEventBooking(long p_EVENT_BOOKING_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetEventBooking ObjPrint = new GetEventBooking();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.EVENT_BOOKING_ID = p_EVENT_BOOKING_ID;
                ObjPrint.TYPE_ID = p_TYPE_ID;
                DataTable dt = ObjPrint.ExecuteTable();
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

        #endregion
    }
}
