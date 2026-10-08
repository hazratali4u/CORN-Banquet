using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

/// <summary>
/// Form For Chart of Account Report
/// </summary>
public partial class Forms_frmAssignCOA : System.Web.UI.Page
{
    readonly AccountHeadController MController = new AccountHeadController();
    readonly RoleManagementController mController = new RoleManagementController();

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
            this.GetAccountType();
            this.GetSubAccountType();
            this.GetDetailAccountType();
            this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
            this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        }
    }

    /// <summary>
    /// Loads Account Types To AccountType Combo
    /// </summary>
    private void GetAccountType()
    {
        DrpMainType.Items.Clear();

        DataTable dt = MController.GetAccountHead(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, DrpAccountCategory.SelectedIndex, Constants.AC_MainTypeId);
        clsWebFormUtil.FillDxComboBoxList(DrpMainType, dt, 0, 1);

        if (dt.Rows.Count > 0)
        {
            DrpMainType.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Loads Account Sub Types To SubType Combo
    /// </summary>
    private void GetSubAccountType()
    {
        DrpSubType.Items.Clear();
        if (DrpMainType.Items.Count > 0)
        {
            DataTable dt = MController.GetAccountHead(int.Parse(DrpMainType.SelectedItem.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue, DrpAccountCategory.SelectedIndex, Constants.AC_SubTypeId);
            clsWebFormUtil.FillDxComboBoxList(DrpSubType, dt, 0, 1);

            if (dt.Rows.Count > 0)
            {
                DrpSubType.SelectedIndex = 0;
            }
        }
    }

    /// <summary>
    /// Loads Account Detail Types To Detail Type Combo
    /// </summary>
    private void GetDetailAccountType()
    {
        DrpDetailType.Items.Clear();
        if (DrpSubType.Items.Count > 0)
        {
            DataTable dt = MController.GetAccountHead(int.Parse(DrpMainType.SelectedItem.Value.ToString()), int.Parse(DrpSubType.SelectedItem.Value.ToString()), Constants.IntNullValue, DrpAccountCategory.SelectedIndex, Constants.AC_DetailTypeId);
            clsWebFormUtil.FillDxComboBoxList(DrpDetailType, dt, 0, 1);

            if (dt.Rows.Count > 0)
            {
                DrpDetailType.SelectedIndex = 0;
            }
        }
    }
    private void GetUnAssingAccountModule(int ModuleID)
    {
        DataTable dt = mController.GetAccountHeadModule(ModuleID, Convert.ToInt32(DrpDetailType.SelectedItem.Value), DrpAccountCategory.SelectedIndex, 1);
        dt.DefaultView.Sort = "ACCOUNT_NAME";
        lstUnAssignModule.DataSource = dt.DefaultView;
        lstUnAssignModule.DataTextField = "ACCOUNT_NAME";
        lstUnAssignModule.DataValueField = "ACCOUNT_HEAD_ID";
        lstUnAssignModule.DataBind();
    }

    /// <summary>
    /// Loads Fourth Layers Modules Assigned To Role To ListBox
    /// </summary>
    /// <param name="RoleID"></param>
    private void GetAssingAccountModule(int ModuleID)
    {
        DataTable dt = mController.GetAccountHeadModule(ModuleID, Convert.ToInt32(DrpDetailType.SelectedItem.Value), DrpAccountCategory.SelectedIndex, 2);
        dt.DefaultView.Sort = "ACCOUNT_NAME";
        lstAssignModule.DataSource = dt.DefaultView;
        lstAssignModule.DataTextField = "ACCOUNT_NAME";
        lstAssignModule.DataValueField = "ACCOUNT_HEAD_ID";
        lstAssignModule.DataBind();
    }

    /// <summary>
    /// Shows Chart of Account in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();

        DataSet ds = RptAccountCtl.SelectRptChartofAccount(DrpAccountCategory.SelectedIndex, int.Parse(DrpMainType.SelectedItem.Value.ToString()), int.Parse(DrpSubType.SelectedItem.Value.ToString()), int.Parse(DrpDetailType.SelectedItem.Value.ToString()));
        DataTable dt = DPrint.SelectReportTitle(Constants.IntNullValue);

        CORNBusinessLayer.Reports.CrpchartOfAccunt CrpReport = new CORNBusinessLayer.Reports.CrpchartOfAccunt();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    /// <summary>
    /// Loads Account Sub Types And Detail Types
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetSubAccountType();
        this.GetDetailAccountType();
        this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
    }

    /// <summary>
    /// Loads Account Detail Types
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetDetailAccountType();
        this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
    }

    /// <summary>
    /// Loads Account Types, Sub Types And Detail Types
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpAccountCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetAccountType();
        GetSubAccountType();
        GetDetailAccountType();
        this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        if (lstUnAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstUnAssignModule.Items.Count; i++)
            {

                if (lstUnAssignModule.Items[i].Selected == true)
                {
                    mController.InsertAccountHeadModule(int.Parse(ddlReport.SelectedItem.Value.ToString()), Convert.ToInt32(lstUnAssignModule.SelectedValue.ToString()), Convert.ToInt32(Session["UserID"]),1);

                }
            }
            this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
            this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        }
    }

    protected void btnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (lstUnAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstUnAssignModule.Items.Count; i++)
            {
                mController.InsertAccountHeadModule(int.Parse(ddlReport.SelectedItem.Value.ToString()), Convert.ToInt32(lstUnAssignModule.Items[i].Value), Convert.ToInt32(Session["UserID"]), 1);
            }
            this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
            this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        }
    }

    protected void btnUnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (this.lstAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstAssignModule.Items.Count; i++)
            {
                mController.InsertAccountHeadModule(int.Parse(ddlReport.SelectedItem.Value.ToString()), Convert.ToInt32(lstAssignModule.Items[i].Value), Convert.ToInt32(Session["UserID"]), 2);
            }
            this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
            this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        }
    }

    protected void btnUnAssign_Click(object sender, EventArgs e)
    {
        if (this.lstAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstAssignModule.Items.Count; i++)
            {
                if (lstAssignModule.Items[i].Selected == true)
                {
                    mController.InsertAccountHeadModule(int.Parse(ddlReport.SelectedItem.Value.ToString()), Convert.ToInt32(lstAssignModule.SelectedValue.ToString()), Convert.ToInt32(Session["UserID"]), 2);
                }
            }
            this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
            this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        }
    }

    protected void DrpDetailType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
    }

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetAccountType();
        GetSubAccountType();
        GetDetailAccountType();
        this.GetUnAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
        this.GetAssingAccountModule(int.Parse(ddlReport.SelectedItem.Value.ToString()));
    }
}