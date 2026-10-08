using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_LookUpFInalBudgetForm : System.Web.UI.Page
{
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadGrid("");
        if (Request.QueryString.ToString().Contains("VoucherID") && 
            !string.IsNullOrEmpty(Request.QueryString["VoucherID"].ToString()))
        {
            string VoucherID = Request.QueryString["VoucherID"].ToString();
            ShowReportPopUp(Convert.ToInt32(VoucherID));
        }
    }
    public void ShowReportPopUp(int savedID)
    {
        try
        {
            DocumentPrintController DPrint = new DocumentPrintController();

            DsReport2 ds = new DsReport2();
            DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));

            DataControl dc = new DataControl();
            DataTable rowsDt = _mSkuController.SelectFinalBudget
            (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, savedID, 2);

            foreach (DataRow dr in rowsDt.Rows)
            {
                ds.Tables["RptFinalBudget"].ImportRow(dr);
            }

            ReportDocument CrpReport = new ReportDocument();
            CrpReport = new CrpFinalBudget();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            //const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            //Type cstype = this.GetType();
            //ClientScriptManager cs = Page.ClientScript;
            //cs.RegisterStartupScript(cstype, "OpenWindow", script);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void LoadGrid(string pType)
    {
        grdFinal_Budget.DataSource = null;
        grdFinal_Budget.DataBind();
        DataTable dt = new DataTable();
        dt = _mSkuController.SelectFinalBudget
            (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Constants.IntNullValue,6);

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)//In case after  Filter
            {
                dt.DefaultView.RowFilter = "Voucher_no LIKE '%" + txtSearch.Text + "%' OR Voucher_no LIKE '%" + txtSearch.Text + "%' OR Venue LIKE '%" + txtSearch.Text + "%' OR Event_Type LIKE '%" + txtSearch.Text + "%' OR Event_Type LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '" + txtSearch.Text + "%'";
            }
            grdFinal_Budget.DataSource = dt;
            grdFinal_Budget.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "Voucher_no LIKE '%" + txtSearch.Text + "%' OR Voucher_no LIKE '%" + txtSearch.Text + "%' OR Venue LIKE '%" + txtSearch.Text + "%' OR Event_Type LIKE '%" + txtSearch.Text + "%' OR Event_Type LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '%" + txtSearch.Text + "%' OR Client_Name LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                grdFinal_Budget.PageIndex = 1;
            }   
            grdFinal_Budget.DataSource = dt;
            grdFinal_Budget.DataBind();
        }
    }
    protected void grdFinalBudgetChanging(object sender, GridViewPageEventArgs e)
    {
        grdFinal_Budget.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void del_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
            _mSkuController.DeleteFinalBudget(Constants.IntNullValue, int.Parse(Row.Cells[5].Text), 7);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');",true);
        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert(Record Deleted);",true);
        LoadGrid("");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        Session.Add("Voucher_ID", Row.Cells[5].Text);
        Response.Redirect("~/Forms/AddFinal_Budjet.aspx?LevelType=3&LevelID=314");
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        Session["Voucher_ID"] = null;
        Response.Redirect("~/Forms/AddFinal_Budjet.aspx?LevelType=3&LevelID=314");
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
}