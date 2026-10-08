using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
/// <summary>
/// Form For Voucher View Report
/// </summary>
public partial class Forms_RptItemVerification : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    /// 
    readonly SkuController _mSkuController = new SkuController();
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadVoucher();
            btnViewPDF.Attributes.Add("onclick", "return ValidateForm();");
            btnViewExcel.Attributes.Add("onclick", "return ValidateForm();");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    protected void LoadVoucher()
    {
        try
        {
            DrpVoucher.Items.Add("--Select--", Constants.IntNullValue.ToString());
            DataTable dt = _skuHierarchyController.DDLVoucherNumber(Constants.IntNullValue, null, 1);
            clsWebFormUtil.FillDxComboBoxList(DrpVoucher, dt, "id", "Voucher_no", false);
            DrpVoucher.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            DrpVoucher.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
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
        try
        {
            DocumentPrintController DPrint = new DocumentPrintController();

            DsReport2 ds = new DsReport2();
            DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));

            DataControl dc = new DataControl();
            DataTable rowsDt = _mSkuController.SelectFinalBudget
            (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue,
            true, int.Parse(DrpVoucher.SelectedItem.Value.ToString()), 2);

            foreach (DataRow dr in rowsDt.Rows)
            {
                ds.Tables["RptFinalBudget"].ImportRow(dr);
            }

            ReportDocument CrpReport = new ReportDocument();
            CrpReport = new CrpEventItemVerification();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", ReportType);
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
}
