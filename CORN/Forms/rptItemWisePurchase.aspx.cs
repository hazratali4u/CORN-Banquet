using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_rptItemWisePurchase : System.Web.UI.Page
{
    readonly RptInventoryController _InvCtl = new RptInventoryController();
    readonly SkuController _skuController = new SkuController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly DistributorController _dController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            LoadCategory();
            LoadItem();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        drpDistributor.Items.Clear();
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue,
            int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 15, Constants.IntNullValue);
            clsWebFormUtil.FillListBox(cblCategory, _mDt, 0, 3, true);
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }

    private void LoadItem()
    {
        
        System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        cblItem.Items.Clear();
        foreach (ListItem li in cblCategory.Items)
        {
            if(li.Selected)
            {
                sbCategory.Append(li.Value);
                sbCategory.Append(",");
            }
        }

        DataTable dt = new DataTable();
        dt = _skuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 21, Constants.IntNullValue, sbCategory.ToString());
        clsWebFormUtil.FillListBox(cblItem, dt, "SKU_ID", "SKU_NAME", true);
        if (cblItem.Items.Count > 0)
        {
            cbAllItem.Checked = true;
            for (int i = 0; i < cblItem.Items.Count; i++)
            {
                cblItem.Items[i].Selected = true;
            }
        }
    }
    private void ShowReport(int reportType)
    {
        System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        System.Text.StringBuilder sbItem = new System.Text.StringBuilder();
        System.Text.StringBuilder sbLocationIDs = new System.Text.StringBuilder();
        foreach (ListItem li in cblCategory.Items)
        {
            if (li.Selected)
            {
                sbCategory.Append(li.Value);
                sbCategory.Append(",");
            }
        }
        foreach (ListItem li in cblItem.Items)
        {
            if (li.Selected)
            {
                sbItem.Append(li.Value);
                sbItem.Append(",");
            }
        }
        if(drpDistributor.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (DevExpress.Web.ListEditItem li in drpDistributor.Items)
            {
                sbLocationIDs.Append(li.Value.ToString());
                sbLocationIDs.Append(",");
            }
        }
        else
        {
            sbLocationIDs.Append(drpDistributor.Value.ToString());
        }

        DataSet ds = _InvCtl.GetItemWisePurchase(sbLocationIDs.ToString(), sbCategory.ToString(), sbItem.ToString(), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text + " 23:59:59"));

        var crpReport = new CrpItemWisePurchase();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("fromdate",txtStartDate.Text);
        crpReport.SetParameterValue("todate", txtEndDate.Text);
        crpReport.SetParameterValue("ReportTitle", "Item Wise Purchase Report");
        crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("PritedBy", Session["UserName"].ToString());
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {

        ShowReport(1);
    }

    protected void cbCategory_CheckedChanged(object sender, EventArgs e)
    {
        if(cbCategory.Checked)
        {
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ListItem li in cblCategory.Items)
            {
                li.Selected = false;
            }
        }
        LoadItem();
    }

    protected void cbAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAllItem.Checked)
        {
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ListItem li in cblItem.Items)
            {
                li.Selected = false;
            }
        }
    }

    protected void cblCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItem();
    }
}