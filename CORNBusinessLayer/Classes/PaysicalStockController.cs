using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Physical Stock Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Stock
    /// </item>
    /// <term>
    /// Update Stock
    /// </term>
    /// <item>
    /// Get Stock
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class PhaysicalStockController
    {

        #region Constructors

        /// <summary>
        /// Constructor For PhaysicalStockController
        /// </summary>
        public PhaysicalStockController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        #endregion

        #region Private Variables

        IDbTransaction mTransaction;
        IDbConnection mConnection;

        #endregion

        #region Public Methods

        #region Select

        /// <summary>
        /// Gets SKU Closing Stock
        /// </summary>
        /// <remarks>
        /// Returns SKU Closing Stock as Datatable
        /// </remarks>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_BatchNo">Batch</param>
        /// <param name="p_StockDate">Date</param>
        /// <returns>SKU Closing Stock as Datatable</returns>
        public DataTable SelectSKUClosingStock(int p_DISTRIBUTOR_ID, int p_SKU_ID, string p_BatchNo, DateTime p_StockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = 12;
                mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockUpdate.SKU_ID = p_SKU_ID;
                mStockUpdate.BATCHNO = p_BatchNo;
                mStockUpdate.STOCK_DATE = p_StockDate;
                DataTable dt = mStockUpdate.ExecuteTable();
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

        public DataTable SelectSKUClosingStock2(int p_DISTRIBUTOR_ID, int p_SKU_ID, string p_BatchNo, DateTime p_StockDate, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = pType;
                mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockUpdate.SKU_ID = p_SKU_ID;
                mStockUpdate.BATCHNO = p_BatchNo;
                mStockUpdate.STOCK_DATE = p_StockDate;
                DataTable dt = mStockUpdate.ExecuteTable();
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
       

        /// <summary>
        /// Gets Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Physical Stock Data as Datatable</returns>
        public DataTable SelectPysicalStock(int p_DISTRIBUTOR_ID, int p_POST, int PRINCIPAL_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectPAYSICAL_STOCK mPaysical = new spSelectPAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.PRINCIPAL_ID = PRINCIPAL_ID;
                mPaysical.POST = 0;
                DataTable dt = mPaysical.ExecuteTable();
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

        public DataTable SelectPhysicalStockTakingDetail(int p_DISTRIBUTOR_ID, long p_PhysiclaStockTaking_ID,int p_TYPE_ID)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPhysicalStockTakingDetail mPurchaseDetail = new spSelectPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseDetail.PhysiclaStockTaking_ID = p_PhysiclaStockTaking_ID;
                mPurchaseDetail.TYPE_ID = p_TYPE_ID;
                DataTable dt = mPurchaseDetail.ExecuteTable();
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

        #region Insert, Update, Delete

        /// <summary>
        /// Inserts Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string InsertPysicalStock(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE, int p_SKU_ID, int p_SQUANTITY,int p_UQUANTITY, decimal p_UNIT_RATE, int p_POST, int PRINCIPAL_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertPAYSICAL_STOCK mPaysical = new spInsertPAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.STOCK_DATE = p_STOCK_DATE;
                mPaysical.SKU_ID = p_SKU_ID;
                mPaysical.SALEABLE_QUANTITY  = p_SQUANTITY;
                mPaysical.UNSALEABLE_QUANTITY = p_UQUANTITY;  
                mPaysical.UNIT_RATE = p_UNIT_RATE;
                mPaysical.PRINCIPAL_ID = PRINCIPAL_ID;
                mPaysical.POST = 0;
                mPaysical.ExecuteQuery();
                return null;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
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
        /// Updates Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string UpdatePysicalStock(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE, int p_SKU_ID, int p_SQUANTITY,int p_UQUANTITY, decimal p_UNIT_RATE, int p_POST, int PRINCIPAL_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatePAYSICAL_STOCK mPaysical = new spUpdatePAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.STOCK_DATE = p_STOCK_DATE;
                mPaysical.SKU_ID = p_SKU_ID;
                mPaysical.SALEABLE_QUANTITY  = p_SQUANTITY;
                mPaysical.UNSALEABLE_QUANTITY = p_UQUANTITY;  
                mPaysical.UNIT_RATE = p_UNIT_RATE;
                mPaysical.PRINCIPAL_ID = PRINCIPAL_ID;
                mPaysical.POST = 0;
                mPaysical.ExecuteQuery();
                return null;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
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
        /// Updates  AND CALCULATE StockREGISTER
        /// </summary>
        /// <param name="pDistributorId">Location</param>
        /// <param name="pStockDate">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string CalculateStockRegister(int pDistributorId, DateTime pStockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspCalculateStockregister2 mPaysical = new uspCalculateStockregister2();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = pDistributorId;
                mPaysical.STOCK_DATE = pStockDate;
               
                mPaysical.ExecuteQuery();
                return null;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
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
        /// Deletes Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="SKU_ID">SKU</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool  DELETEPysicalStock(int p_DISTRIBUTOR_ID, DateTime  p_STOCK_DATE, int SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spDeletePAYSICAL_STOCK mPaysical = new spDeletePAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.SKU_ID = SKU_ID;
                mPaysical.STOCK_DATE  = p_STOCK_DATE;
                mPaysical.ExecuteQuery();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return false;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public bool InsertPhysicalStockTaking(int p_DISTRIBUTOR_ID, DateTime p_DOCUMENT_DATE, string p_REMARKS, int p_USER_ID, DataTable dtPurchaseDetail)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertPhysicalStockTaking mPurchaseMaster = new spInsertPhysicalStockTaking();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mPurchaseMaster.USER_ID = p_USER_ID;
                mPurchaseMaster.REMARKS = p_REMARKS;
                mPurchaseMaster.ExecuteQuery();

                spInsertPhysicalStockTakingDetail mPurchaseDetail = new spInsertPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.Transaction = mTransaction;

                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    mPurchaseDetail.PhysiclaStockTaking_ID = mPurchaseMaster.PhysiclaStockTaking_ID;
                    mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPurchaseDetail.STOCK_DATE = p_DOCUMENT_DATE;
                    mPurchaseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mPurchaseDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                    mPurchaseDetail.UNIT_RATE = decimal.Parse(dr["UNIT_RATE"].ToString());
                    mPurchaseDetail.ExecuteQuery();
                }
                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return false;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        public bool UpdatePhysicalStockTaking(long p_PhysiclaStockTaking_ID, int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE, int p_USER_ID,string p_REMARKS, DataTable dtPurchaseDetail)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spUpdatePhysicalStockTaking mPurchaseMaster = new spUpdatePhysicalStockTaking();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.PhysiclaStockTaking_ID = p_PhysiclaStockTaking_ID;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.USER_ID = p_USER_ID;
                mPurchaseMaster.REMARKS = p_REMARKS;
                mPurchaseMaster.ExecuteQuery();
                
                spInsertPhysicalStockTakingDetail mPurchaseDetail = new spInsertPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.Transaction = mTransaction;

                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    mPurchaseDetail.PhysiclaStockTaking_ID = mPurchaseMaster.PhysiclaStockTaking_ID;
                    mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPurchaseDetail.STOCK_DATE = p_DOCUMENT_DATE;
                    mPurchaseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mPurchaseDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                    mPurchaseDetail.UNIT_RATE = decimal.Parse(dr["UNIT_RATE"].ToString());
                    mPurchaseDetail.ExecuteQuery();
                }
                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return false;
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