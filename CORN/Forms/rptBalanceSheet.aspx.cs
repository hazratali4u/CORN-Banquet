using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_rptBalanceSheet : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.LoadLevel();
            btnViewPDF.Attributes.Add("onclick", "return ValidateForm();");
            btnViewExcel.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadLevel()
    {
        DataTable dtLevel = new DataTable();
        dtLevel.Columns.Add("LevelID",typeof(int));
        dtLevel.Columns.Add("LevelName", typeof(string));

        DataRow dr4 = dtLevel.NewRow();
        dr4["LevelID"] = 4;
        dr4["LevelName"] = "Level 4";
        dtLevel.Rows.Add(dr4);

        DataRow dr3 = dtLevel.NewRow();
        dr3["LevelID"] = 3;
        dr3["LevelName"] = "Level 3";
        dtLevel.Rows.Add(dr3);

        DataRow dr2 = dtLevel.NewRow();
        dr2["LevelID"] = 2;
        dr2["LevelName"] = "Level 2";
        dtLevel.Rows.Add(dr2);

        DataRow dr1 = dtLevel.NewRow();
        dr1["LevelID"] = 1;
        dr1["LevelName"] = "Level 1";
        dtLevel.Rows.Add(dr1);
        
        clsWebFormUtil.FillDxComboBoxList(DrpLevel, dtLevel, 0, 1);

        if (dtLevel.Rows.Count > 0)
        {
            DrpLevel.SelectedIndex = 0;
        }
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
    private void ShowReport(int ReportType)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();

        DataSet ds = null;

        DataTable dt = null;
        if (drpDistributor.SelectedIndex == 0)
        { dt = DPrint.SelectReportTitle(Constants.IntNullValue); }
        else
        { dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.Value.ToString())); }

        ds = RptAccountCtl.GetBalanceSheet(0, Convert.ToInt32(drpDistributor.Value), DateTime.Parse(txtEndDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        CrystalDecisions.CrystalReports.Engine.ReportDocument CrpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srAssets = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srLiabilities = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        if (DrpLevel.Value.ToString() == "4")
        {
            CrpReport = new CrpBalanceSheet4();
        }
        else if (DrpLevel.Value.ToString() == "3")
        {
            CrpReport = new CrpBalanceSheet3();
        }
        else if (DrpLevel.Value.ToString() == "2")
        {
            CrpReport = new CrpBalanceSheet2();
        }
        else if (DrpLevel.Value.ToString() == "1")
        {
            CrpReport = new CrpBalanceSheet1();
        }

        srAssets = CrpReport.OpenSubreport("srAssets");
        srLiabilities = CrpReport.OpenSubreport("srLiabilities");
        srAssets.SetDataSource(ds);
        srLiabilities.SetDataSource(ds);

        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("FromDate", Convert.ToDateTime(txtEndDate.Text));
        CrpReport.SetParameterValue("ToDate", Convert.ToDateTime(txtEndDate.Text));

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", ReportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
