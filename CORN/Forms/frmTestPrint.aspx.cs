using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Forms_frmTestPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            // Create an instance of SqlDataAdapter. Spcify the command and the connection
            //SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from tblProductInventory", connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductInventory", connection);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Create an instance of DataSet, which is an in-memory datastore for storing tables
            DataSet dataset = new DataSet();
            // Call the Fill() methods, which automatically opens the connection, executes the command 
            // and fills the dataset with data, and finally closes the connection.
            dataAdapter.Fill(dataset);

            //GridView1.DataSource = dataset;
            //GridView1.DataBind();
        }
        //-----------------------------------------------------------
        // string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            // Create an instance of SqlDataAdapter, specifying the stored procedure 
            // and the connection object to use
            SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductInventoryById", connection);
            // Specify the command type is an SP
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            // Associate the parameter with the stored procedure
            dataAdapter.SelectCommand.Parameters.AddWithValue("@ProductId", 1);

            DataSet dataset = new DataSet();
            dataAdapter.Fill(dataset);

            //GridView1.DataSource = dataset;
            //GridView1.DataBind();
        }

        //-------------------------------------------
        //string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductAndCategoriesData", connection);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dataset = new DataSet();
            dataAdapter.Fill(dataset);

            //GridViewProducts.DataSource = dataset.Tables[0];
            //GridViewProducts.DataBind();

            //GridViewCategories.DataSource = dataset.Tables[1];
            //GridViewCategories.DataBind();
            //---------------OR------------------------
            dataset.Tables[0].TableName = "Products";
            dataset.Tables[1].TableName = "Categories";

            //GridViewProducts.DataSource = dataset.Tables["Products"];
            //GridViewProducts.DataBind();

            //GridViewCategories.DataSource = dataset.Tables["Categories"];
            //GridViewCategories.DataBind();

        }
    }
    //----------------------------------------

    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        // Check if the DataSet is present in the cache
        if (Cache["Data"] == null)
        {
            // If the dataset is not in the cache load data from the database into the DataSet
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(CS))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from tblProductInventory", connection);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);

                gvProducts.DataSource = dataset;
                gvProducts.DataBind();

                // Store the DataSet in the Cache
                Cache["Data"] = dataset;
                lblMessage.Text = "Data loaded from the Database";
            }
        }
        // If the DataSet is in the Cache
        else
        {
            // Retrieve the DataSet from the Cache and type cast to DataSet
            gvProducts.DataSource = (DataSet)Cache["Data"];
            gvProducts.DataBind();
            lblMessage.Text = "Data loaded from the Cache";
        }
    }

    protected void btnClearnCache_Click(object sender, EventArgs e)
    {
        // Check if the DataSet is present in the cache
        if (Cache["Data"] != null)
        {
            // Remove the DataSet from the Cache
            Cache.Remove("Data");
            lblMessage.Text = "DataSet removed from the cache";
        }
        // If the DataSet is not in the Cache
        else
        {
            lblMessage.Text = "There is nothing in the cache to remove";
        }
    }





}
