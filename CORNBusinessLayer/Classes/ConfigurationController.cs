using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System;
using System.Data;

namespace CORNBusinessLayer.Classes
{
    public class ConfigurationController
    {
        public DataTable SelectAppConfiguration(int Typeid, int Code, string ColumnValue, int DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spAppConfiguration mdtConfig = new spAppConfiguration();
                mdtConfig.Connection = mConnection;
                mdtConfig.TYPEID = Typeid;
                mdtConfig.CODE_MASTER = Code;
                mdtConfig.CODE_MASTER2 = Code;
                mdtConfig.CODE_DETAIL = Code;
                mdtConfig.COLUMN_VALUE = ColumnValue;
                mdtConfig.COMPANY_ID = Constants.IntNullValue;
                mdtConfig.DISTRIBUTOR_ID = DistributorID;
                DataTable dt = mdtConfig.ExecuteTable();
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
        public DataTable ChekAppConfigurationExistance(int Typeid, int DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spAppConfiguration mdtConfig = new spAppConfiguration();
                mdtConfig.Connection = mConnection;
                mdtConfig.TYPEID = Typeid;
                mdtConfig.CODE_MASTER = Constants.IntNullValue;
                mdtConfig.CODE_MASTER2 = Constants.IntNullValue;
                mdtConfig.CODE_DETAIL = Constants.IntNullValue;
                mdtConfig.COLUMN_VALUE = null;
                mdtConfig.COMPANY_ID = Constants.IntNullValue;
                mdtConfig.DISTRIBUTOR_ID = DistributorID;

                return mdtConfig.ExecuteTable();
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
    }
}