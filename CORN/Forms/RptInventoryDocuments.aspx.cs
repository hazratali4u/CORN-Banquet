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

public partial class Forms_RptInventoryDocuments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!IsPostBack)
        {
            this.LoadDistributor();
            LoadPrincipal();
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
            PanelSupplier.Visible = true;
        }
    }
   
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
    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add("All", Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(this.DrpPrincipal, m_dt, 0, 1);
        if (m_dt.Rows.Count > 0)
        {
            DrpPrincipal.SelectedIndex = 0;
        }
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 2)
        {
            ShowPurchaseReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 3)
        {
            ShowPurchaseReturnReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 4)
        {
            ShowTransferInReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 5)
        {
            ShowTransferOutReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 6)
        {
            ShowDamageReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 8)
        {
            ShowShortReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 9)
        {
            ShowExcessReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 19 || int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 14)
        {
            ShowStockIssuanceReport(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 15)
        {
            ShowPhysicalStockTaking(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 30)
        {
            ShowStockIssuencSummary(0);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 32)
        {
            ShowPurchaseReportSummary(0);
        }
        else if(Convert.ToInt32(DrpDocumentType.Value) == 33)
        {
            ShowStockDemanReport(0);
        }

    }
    public void DailPurchaseTransfer()
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));

        DataControl dc = new DataControl();
        ds = RptInventoryCtl.SelectPurchaseTransferStock(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue,
            DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(DrpDocumentType.SelectedItem.Value.ToString()), Convert.ToInt32(rblRate.SelectedValue));

        CORNBusinessLayer.Reports.CrpDailyPurchaseTransfer CrpReport = new CORNBusinessLayer.Reports.CrpDailyPurchaseTransfer();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("PRINCIPAL", "");
        CrpReport.SetParameterValue("LOCATION", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("ReportTitle", "Date Wise " + DrpDocumentType.SelectedItem.Text + " Report");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("Price", rblRate.SelectedItem.Text);

        this.Session.Add("ReportType", 0);
        this.Session.Add("CrpReport", CrpReport);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    } 
   
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 2)
        {
            ShowPurchaseReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 3)
        {
            ShowPurchaseReturnReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 4)
        {
            ShowTransferInReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 5)
        {
            ShowTransferOutReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 6)
        {
            ShowDamageReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 8)
        {
            ShowShortReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 9)
        {
            ShowExcessReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 13 || int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 14)
        {
            ShowStockIssuanceReport(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 15)
        {
            ShowPhysicalStockTaking(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 30)
        {
            ShowStockIssuencSummary(1);
        }
        else if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 32)
        {
            ShowPurchaseReportSummary(1);
        }
        else if (Convert.ToInt32(DrpDocumentType.Value) == 33)
        {
            ShowStockDemanReport(1);
        }
    }


    public void ShowPurchaseReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        CORNBusinessLayer.Reports.CrpPurchaseDocument CrpReport = new CORNBusinessLayer.Reports.CrpPurchaseDocument();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;

        ds = RptInventoryCtl.SelectPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpPrincipal.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 2);

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Purchase Document");
        //CrpReport.SetParameterValue("Principal", drpPrincipal.SelectedItem.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    public void ShowPurchaseReturnReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        CORNBusinessLayer.Reports.CrpPurchaseDocument CrpReport = new CORNBusinessLayer.Reports.CrpPurchaseDocument();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;
        ds = RptInventoryCtl.SelectPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpPrincipal.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 3);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Purchase Return Document");
        //CrpReport.SetParameterValue("Principal", drpPrincipal.SelectedItem.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    public void ShowTransferInReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 4);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Transfer In Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    public void ShowTransferOutReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 5);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Transfer Out Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    public void ShowDamageReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpDamageDocument();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 6);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Damage Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    public void ShowShortReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpShortDocument();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 8);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Short Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    public void ShowExcessReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CORNBusinessLayer.Reports.CrpShortDocument();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 9);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Excess Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    private void ShowStockIssuanceReport(int type)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            RptInventoryController RptInventoryCtl = new RptInventoryController();
            CrpIssueDocument CrpReport = new CrpIssueDocument();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.Value.ToString()));
            DataSet ds = RptInventoryCtl.SelectIssueDocument(int.Parse(drpDistributor.Value.ToString()),
                Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(DrpDocumentType.Value.ToString()), Constants.LongNullValue);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            if (DrpDocumentType.SelectedItem.Value.ToString() == "19")
            {
                CrpReport.SetParameterValue("DocumentType", "Issue To");
                CrpReport.SetParameterValue("ReportName", "Stock Issuance Document");
                CrpReport.SetParameterValue("IssuedBy", "Issue By");
            }
            else
            {
                CrpReport.SetParameterValue("DocumentType", "Return From");
                CrpReport.SetParameterValue("ReportName", "Stock Return Document");
                CrpReport.SetParameterValue("IssuedBy", "Return By");
            }
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", type);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private void ShowPhysicalStockTaking(int type)
    {
        try
        {
            RptInventoryController RptInventoryCtl = new RptInventoryController();
            CrpPhysicalStockTaking CrpReport = new CrpPhysicalStockTaking();
            DataSet ds = RptInventoryCtl.GetPhysicalStockTaking(int.Parse(drpDistributor.SelectedItem.Value.ToString()),
            DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 2);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", type);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private void ShowStockIssuencSummary(int type)
    {
        try
        {
            DocumentPrintController mController = new DocumentPrintController();
            RptInventoryController RptInventoryCtl = new RptInventoryController();
            CrpIssueDocumentSummary CrpReport = new CrpIssueDocumentSummary();
            DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.Value.ToString()));
            DataSet ds = RptInventoryCtl.SelectIssueDocument(int.Parse(drpDistributor.Value.ToString()),
                Constants.IntNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 00:00:00"), 30, Constants.LongNullValue);

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();


            CrpReport.SetParameterValue("DocumentType", "Issue To");
            CrpReport.SetParameterValue("ReportName", "Stock Issuance Summary");
            CrpReport.SetParameterValue("IssuedBy", "Issue By");
            CrpReport.SetParameterValue("date", DateTime.Parse(txtStartDate.Text + " 00:00:00"));
            CrpReport.SetParameterValue("fromDate", DateTime.Parse(txtEndDate.Text + " 00:00:00"));
            CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
            CrpReport.SetParameterValue("location", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", type);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void ShowPurchaseReportSummary(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        CORNBusinessLayer.Reports.CrpPurchaseDocumentSummry CrpReport = new CORNBusinessLayer.Reports.CrpPurchaseDocumentSummry();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;

        ds = RptInventoryCtl.SelectPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(DrpPrincipal.SelectedItem.Value.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 32);

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
      
       // CrpReport.SetParameterValue("ReportName", "Purchase Document Summary");
      
        CrpReport.SetParameterValue("date", DateTime.Parse(txtStartDate.Text + " 00:00:00"));
        CrpReport.SetParameterValue("fromDate", DateTime.Parse(txtEndDate.Text + " 00:00:00"));
        CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());
        CrpReport.SetParameterValue("DocumentType", "Purchase Document Summary");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("supplier",DrpPrincipal.SelectedItem.Text);


        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    public void ShowStockDemanReport(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;

        ds = RptInventoryCtl.StockDemandPrint(Constants.IntNullValue,Convert.ToInt32(drpDistributor.Value), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        var crpReport = new CrpStockDemandPrint();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 2 || int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 3 || int.Parse(DrpDocumentType.SelectedItem.Value.ToString()) == 32)
        {
            PanelSupplier.Visible = true;
        }
        else
        {
            PanelSupplier.Visible = false;
        }
    }
}