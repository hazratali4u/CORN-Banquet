using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CORNDatabaseLayer.InputClasses
{
  public class spSelectSKUSNew
    {
    
private string sp_Name = "spSelectSKUSNew";
private IDbConnection m_connection;
private IDbTransaction m_transaction;

           
public string SKU_Name { get; set; }
public string Description { get; set; }
public bool IsActive { get; set; }
      

public IDbConnection Connection
{
    set
    {
        m_connection = value;
    }
    get
    {
        return m_connection;
    }
}
public IDbTransaction Transaction
{
    set
    {
        m_transaction = value;
    }
    get
    {
        return m_transaction;
    }
}
#region Constructor
public spSelectSKUSNew()
{

}
#endregion
#region public Methods
public bool ExecuteQuery()
{
    try
    {
        IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = sp_Name;
        cmd.Connection = m_connection;
        if (m_transaction != null)
        {
            cmd.Transaction = m_transaction;
        }
        GetParameterCollection(ref cmd);
        cmd.ExecuteNonQuery();
        return true;
    }
    catch (Exception e)
    {
        throw e;
    }
    finally
    {
    }
}
public IDataReader ExecuteReader()
{
    try
    {
        IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = sp_Name;
        command.Connection = m_connection;
        if (m_transaction != null)
        {
            command.Transaction = m_transaction;
        }
        GetParameterCollection(ref command);
        IDataReader dr = command.ExecuteReader();
        return dr;
    }
    catch (Exception exp)
    {
        throw exp;
    }
    finally
    {
    }
}
public DataTable ExecuteTable()
{
    try
    {
        IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = sp_Name;
        command.Connection = m_connection;
        if (m_transaction != null)
        {
            command.Transaction = m_transaction;
        }
        GetParameterCollection(ref command);
        IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
        da.SelectCommand = command;
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];
    }
    catch (Exception exp)
    {
        throw exp;
    }
    finally
    {
    }
}
public string ExecuteScalar()
{
    try
    {
        IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = sp_Name;
        command.Connection = m_connection;
        if (m_transaction != null)
        {
            command.Transaction = m_transaction;
        }
        GetParameterCollection(ref command);
        object o;
        o = command.ExecuteScalar();


        return o.ToString();
    }
    catch (Exception exp)
    {
        throw exp;
    }
    finally
    {
    }
}
public void FirstReader(IDataReader dr)
{
    if (dr.Read())
    {
        IsActive = Convert.ToBoolean(dr["IsActive"]);
    }
}
public void GetParameterCollection(ref IDbCommand cmd)
{
    IDataParameterCollection pparams = cmd.Parameters;
    IDataParameter parameter;

    parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
    parameter.ParameterName = "@SKU_Name";
    parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
    if (SKU_Name == null)
    {
        parameter.Value = DBNull.Value;
    }
    else
    {
        parameter.Value = SKU_Name;
    }
    pparams.Add(parameter);

    parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
    parameter.ParameterName = "@Description";
    parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
    if (Description == null)
    {
        parameter.Value = DBNull.Value;
    }
    else
    {
        parameter.Value = Description;
    }
    pparams.Add(parameter);

    parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
    parameter.ParameterName = "@IsActive";
    parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
    parameter.Value = IsActive;
    pparams.Add(parameter);

}
#endregion

        }
    }

