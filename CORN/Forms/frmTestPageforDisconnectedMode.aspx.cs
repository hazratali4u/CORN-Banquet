using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public class Student
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public int TotalMarks { get; set; }
}

public partial class Forms_frmTestPageforDisconnectedMode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //    //1.You create an instance of SqlDataAdapter by specifying a select command and a connection object
        //    string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string selectQuery = "Select * from tblStudents";
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

        //    //2.When SqlDataAdapter.Fill() method is invoked, SqlDataAdapter opens the connection to the database, executes the select command, 
        //    //and the DataSet is populated with the data that is retrieved.The SqlDataAdapter automatically closes the connection.

        //    DataSet dataSet = new DataSet();
        //    dataAdapter.Fill(dataSet, "Students");

        //    //3.You now have data in the DataSet and there is no active connection to the database.At this point you can make any changes(insert, update, delete) 
        //    //to the data in the DataSet.Only the data in the DataSet is changed, the underlying database table data is not changed.


        //    // 4.To update the underlying database table, invoke SqlDataAdapter.Update() method.Make sure there is an UPDATE, DELETE and INSERT command are associated 
        //    // with SqlDataAdapter object when Update() method is called, otherwise there would be a runtime exception.

        //    dataAdapter.Update(dataSet, "Students");

        ////---------------------22----------------
        //if (!IsPostBack)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string selectQuery = "Select * from tblStudents";
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

        //    DataSet dataSet = new DataSet();
        //    dataAdapter.Fill(dataSet, "Students");

        //    Session["DATASET"] = dataSet;
        //    GridView2.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
        //                           select new Student
        //                           {
        //                               ID = Convert.ToInt32(dataRow["Id"]),
        //                               Name = dataRow["Name"].ToString(),
        //                               Gender = dataRow["Gender"].ToString(),
        //                               TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])
        //                           };
        //    GridView2.DataBind();
        //}   

        ////------------------------33-------when we add strongly typed dataset-----------
        //if (!IsPostBack)
        //{
        //    StudentDataSetTableAdapters.StudentsTableAdapter studentsTableAdapter =  new StudentDataSetTableAdapters.StudentsTableAdapter();
        //    StudentDataSet.StudentsDataTable studentsDataTable = new StudentDataSet.StudentsDataTable();
        //    studentsTableAdapter.Fill(studentsDataTable);

        //    Session["DATATABLE"] = studentsDataTable;

        //    GridView1.DataSource = from student in studentsDataTable
        //                           select new { student.ID, student.Name, student.Gender, student.TotalMarks };
        //    GridView1.DataBind();
        //}

    }

    private void GetDataFromDB()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        string selectQuery = "Select * from tblStudents";
        SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

        DataSet dataSet = new DataSet();
        dataAdapter.Fill(dataSet, "Students");

        // Set ID column as the primary key
        dataSet.Tables["Students"].PrimaryKey =  new DataColumn[] { dataSet.Tables["Students"].Columns["ID"] };
        
        // Store the dataset in Cache
        Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);

        GridView1.DataSource = dataSet;
        GridView1.DataBind();

        lblStatus.Text = "Data loded from Database";
    }

    private void GetDataFromCache()
    {
        if (Cache["DATASET"] != null)
        {
            GridView1.DataSource = (DataSet)Cache["DATASET"];
            GridView1.DataBind();
        }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        // Set row in editing mode
        GridView1.EditIndex = e.NewEditIndex;
        GetDataFromCache();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        GetDataFromCache();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // Retrieve dataset from cache
        DataSet dataSet = (DataSet)Cache["DATASET"];

        // Find datarow to edit using primay key
        DataRow dataRow = dataSet.Tables["Students"].Rows.Find(e.Keys["ID"]);

        // Update datarow values
        dataRow["Name"] = e.NewValues["Name"];
        dataRow["Gender"] = e.NewValues["Gender"];
        dataRow["TotalMarks"] = e.NewValues["TotalMarks"];

        // Overwrite the dataset in cache
        Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);

        // Remove the row from edit mode
        GridView1.EditIndex = -1;

        // Reload data to gridview from cache
        GetDataFromCache();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet dataSet = (DataSet)Cache["DATASET"];
        dataSet.Tables["Students"].Rows.Find(e.Keys["ID"]).Delete();
        Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
        GetDataFromCache();
    }

    protected void btnGetDataFromDB_Click(object sender, EventArgs e)
    {
        GetDataFromDB();
    }

    protected void btnUpdateDatabaseTable_Click(object sender, EventArgs e)
    {
        if (Cache["DATASET"] != null)
        {
            string connectionString =
            ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            string selectQuery = "Select * from tblStudents";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

            // Update command to update database table
            string strUpdateCommand = "Update tblStudents set Name = @Name, Gender = @Gender, TotalMarks = @TotalMarks where Id = @Id";

            // Create an instance of SqlCommand using the update command created above
            SqlCommand updateCommand = new SqlCommand(strUpdateCommand, connection);

            // Specify the parameters of the update command
            updateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            updateCommand.Parameters.Add("@Gender", SqlDbType.NVarChar, 20, "Gender");
            updateCommand.Parameters.Add("@TotalMarks", SqlDbType.Int, 0, "TotalMarks");
            updateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            // Associate update command with SqlDataAdapter instance
            dataAdapter.UpdateCommand = updateCommand;

            // Delete command to delete data from database table
            string strDeleteCommand = "Delete from tblStudents where Id = @Id";

            // Create an instance of SqlCommand using the delete command created above
            SqlCommand deleteCommand = new SqlCommand(strDeleteCommand, connection);

            // Specify the parameters of the delete command
            deleteCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            // Associate delete command with SqlDataAdapter instance
            dataAdapter.DeleteCommand = deleteCommand;

            // Update the underlying database table
            dataAdapter.Update((DataSet)Cache["DATASET"], "Students");

            lblStatus.Text = "Database table updated";
        }
    }

    //-----------------22---------------
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    DataSet dataSet = (DataSet)Session["DATASET"];
    //    if (string.IsNullOrEmpty(TextBox1.Text))
    //    {
    //        GridView2.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
    //                               select new Student
    //                               {
    //                                   ID = Convert.ToInt32(dataRow["Id"]),
    //                                   Name = dataRow["Name"].ToString(),
    //                                   Gender = dataRow["Gender"].ToString(),
    //                                   TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])
    //                               };
    //        GridView2.DataBind();
    //    }
    //    else
    //    {
    //        GridView2.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
    //                               where dataRow["Name"].ToString().ToUpper().StartsWith(TextBox1.Text.ToUpper())
    //                               select new Student
    //                               {
    //                                   ID = Convert.ToInt32(dataRow["Id"]),
    //                                   Name = dataRow["Name"].ToString(),
    //                                   Gender = dataRow["Gender"].ToString(),
    //                                   TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])
    //                               };
    //        GridView2.DataBind();
    //    }
    //}


    ////-----------------33-----------when we add strongly typed dataset-------
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    StudentDataSet.StudentsDataTable studentsDataTable = (StudentDataSet.StudentsDataTable)Session["DATATABLE"];

    //    if (string.IsNullOrEmpty(TextBox1.Text))
    //    {
    //        GridView1.DataSource = from student in studentsDataTable
    //                               select new { student.ID, student.Name, student.Gender, student.TotalMarks };
    //        GridView1.DataBind();
    //    }
    //    else
    //    {
    //        GridView1.DataSource = from student in studentsDataTable
    //                               where student.Name.ToUpper().StartsWith(TextBox1.Text.ToUpper())
    //                               select new { student.ID, student.Name, student.Gender, student.TotalMarks };
    //        GridView1.DataBind();
    //    }
    //}

}