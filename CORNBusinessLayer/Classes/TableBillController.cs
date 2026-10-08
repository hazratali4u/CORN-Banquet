using System;
using System.Data;
using System.IO;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using CORNBusinessLayer.Classes;
using System.Collections.Generic;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For SKU Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU
    /// </item>
    /// <term>
    /// Update SKU
    /// </term>
    /// <item>
    /// Get SKU
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class TableBillController
    {
        #region Constructors

        /// <summary>
        /// Constructor For TableBillController
        /// </summary>
        public TableBillController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        public TableInvoiceResponse GetOccupiedTablesForBilling(TableInvoiceRequest request)
        {
            TableInvoiceResponse response = new TableInvoiceResponse();
            response.TableInfoList = new List<TableInfo>();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.SALE_GetOccupiedTablesForBilling";
                command.Connection = mConnection;

                IDataParameterCollection pparams = command.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@DocumentDate";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                parameter.Value = request.DocumentDate;
                pparams.Add(parameter);

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@DistributorId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                parameter.Value = request.DistributorId;
                pparams.Add(parameter);

                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    response.TableInfoList = ds.Tables[0].ToCollection<TableInfo>();
                }
                response.IsException = false;

                return response;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                response.IsException = true;
                response.Message = exp.Message;
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public TableInvoiceResponse GetOccupiedTableBill(TableInvoiceRequest request)
        {
            TableInvoiceResponse response = new TableInvoiceResponse();
            response.TableInfoList = new List<TableInfo>();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.SALE_GetOccupiedTableBill";
                command.Connection = mConnection;

                IDataParameterCollection pparams = command.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@InvoiceId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
                parameter.Value = request.InvoiceId;
                pparams.Add(parameter);

                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    response.OrderBookerInfo = ds.Tables[0].ToObject<OrderBookerInfo>();
                    response.OrderBookerInfo.TableInfo = new TableInfo();
                    response.OrderBookerInfo.TableInfo = ds.Tables[0].ToObject<TableInfo>();

                    response.TableInvoiceDetailList = ds.Tables[1].ToCollection<TableInvoiceDetail>();
                }
                response.IsException = false;

                return response;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                response.IsException = true;
                response.Message = exp.Message;
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public TableInvoiceResponse UnholdTableByBillPaid(TableInvoiceRequest request)
        {
            TableInvoiceResponse response = new TableInvoiceResponse();
            response.TableInfoList = new List<TableInfo>();

            IDbConnection mConnection = null;
            IDbTransaction mtransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.SALE_UnholdTableByBillPaid";
                command.Connection = mConnection;

                IDataParameterCollection pparams = command.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@InvoiceId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
                parameter.Value = request.InvoiceId;
                pparams.Add(parameter);

                mtransaction = ProviderFactory.GetTransaction(mConnection);
                command.Transaction = mtransaction;

                command.ExecuteNonQuery();
                mtransaction.Commit();

                response.IsException = false;

                return response;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                response.IsException = true;
                response.Message = exp.Message;
                if (mtransaction != null)
                    mtransaction.Rollback();
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

    }

    public class TableInfo
    {
        public int TableId { get; set; }
        public string TableNo { get; set; }
        public long InvoiceId { get; set; }
    }

    public class OrderBookerInfo
    {
        public int OrderBookerId { get; set; }
        public string OrderBookerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }
        public string OrderDateTime { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public decimal GST { get; set; }

        public TableInfo TableInfo { get; set; }
    }

    public class TableInvoiceDetail
    {
        public long InvoiceId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
    }

    public class TableInvoiceResponse
    {
        public List<TableInfo> TableInfoList { get; set; }
        public OrderBookerInfo OrderBookerInfo { get; set; }
        public List<TableInvoiceDetail> TableInvoiceDetailList { get; set; }

        public bool IsException { get; set; }
        public string Message { get; set; }
    }

    public class TableInvoiceRequest
    {
        public DateTime DocumentDate { get; set; }
        public int DistributorId { get; set; }
        public long InvoiceId { get; set; }
    }

}