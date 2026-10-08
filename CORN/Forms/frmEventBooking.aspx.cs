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

public partial class Forms_frmEventBooking : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly SkuController _mSkuController = new SkuController();
    readonly CustomerDataController _cutsomerController = new CustomerDataController();
    readonly EventBookingController _eventBookingController = new EventBookingController();
    readonly GeoHierarchyController _hierarchyController = new GeoHierarchyController();
    readonly RptInventoryController RptInventoryCtl = new RptInventoryController();
    DataTable SKU = new DataTable();
    private static int RowNo;
    private static string EVENT_BOOKING_ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!IsPostBack)
        {
            //itemsGrandTotal.Attributes.Add("readonly", "readonly");
            txtAmount.Attributes.Add("readonly", "readonly");
            txtRate.Attributes.Add("readonly", "readonly");
            txtallTotal.Attributes.Add("readonly", "readonly");
            txtRemainingAmount.Attributes.Add("readonly", "readonly");
            txtReceiptsAmount.Attributes.Add("readonly", "readonly");
            CreateTable();
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtEventDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtBookingDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            LoadEventType();
            LoadCustomer();
            if (Session["EVENT_BOOKING_ID"] != null)
            {
                LoadEvent(Convert.ToInt64(Session["EVENT_BOOKING_ID"]));
            }
            LoadSKU();
            LoadHall();
            //btnSaveDocument.Attributes.Add("onclick", "return ValidateForm()");
            txtEventDate_TextChanged(null, null);
        }
    }

    #region Page Load
    private void CreateTable()
    {
        SKU = new DataTable();
        SKU.Columns.Add("SKU_ID", typeof(int));
        SKU.Columns.Add("SKU_CODE", typeof(string));
        SKU.Columns.Add("SKU_NAME", typeof(string));
        SKU.Columns.Add("UOM_DESC", typeof(string));
        SKU.Columns.Add("PRICE", typeof(decimal));
        SKU.Columns.Add("QTY", typeof(int));
        SKU.Columns.Add("AMOUNT", typeof(decimal));
        SKU.Columns.Add("NET_AMOUNT", typeof(decimal));
        SKU.Columns.Add("UOM_ID", typeof(decimal));
        SKU.Columns.Add("Category_Type_ID", typeof(int));
        Session.Add("SKU", SKU);
    }
    private void LoadEventType()
    {
        DataTable dt = _eventBookingController.GetEventTypes();
        clsWebFormUtil.FillDxComboBoxList(ddlEventType, dt, "EVENT_TYPE_ID", "EVENT_TYPE_DESC");
        if (dt.Rows.Count > 0)
        {
            ddlEventType.SelectedIndex = 0;
        }
    }
    private void LoadEventTime(int p_TYPE_ID)
    {
        DataTable dt = new DataTable();
        ddDineType.Items.Clear();
        if (p_TYPE_ID == 1)
        {
            dt = _eventBookingController.SelectDineTimes(Convert.ToDateTime(txtEventDate.Text), int.Parse(ddlHallNo.SelectedItem.Value.ToString()), p_TYPE_ID);
        }
        else
        {
            dt = _eventBookingController.SelectDineTimes(Convert.ToDateTime(txtEventDate.Text), Constants.IntNullValue, p_TYPE_ID);
        }
        clsWebFormUtil.FillDxComboBoxList(ddDineType, dt, "EVENT_TIME_ID", "EVENT_TIME_SPAN", true);
        if (dt.Rows.Count > 0)
        {
            ddDineType.SelectedIndex = 0;
        }
    }
    private void LoadCustomer()
    {
        DataTable dt = _cutsomerController.SelectAllCustomer(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlCustomer.SelectedIndex = 0;
            txtIDCard.Text = dt.Rows[0]["CNIC"].ToString();
            txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
            txtCustomerNo.Text = dt.Rows[0]["CONTACT_NUMBER"].ToString();
            txtOtherNo.Text = dt.Rows[0]["CONTACT2"].ToString();
        }
    }
    private void LoadHall()
    {
        DataTable dt = _hierarchyController.GetTableDefination(Constants.IntNullValue, Constants.IntNullValue, true, int.Parse(Session["UserID"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlHallNo, dt, "TableDefination_ID", "TableDefination_No");
        if (dt.Rows.Count > 0)
        {
            ddlHallNo.SelectedIndex = 0;
        }
    }
    private void LoadSKU()
    {
        SkuController SKUCtl = new SkuController();
        if (ddlCustomer.Items.Count > 0)
        {
            DataTable itemsData = _mSkuController.SelectActiveSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);
            if (itemsData.Rows.Count > 0)
            {
                ddlSKU.DataSource = itemsData;
                ddlSKU.TextField = "SKU_NAME";
                ddlSKU.ValueField = "SKU_ID";
                ddlSKU.DataBind();
                ddlSKU.SelectedIndex = 0;

                txtUOM.Text = itemsData.Rows[0]["UOM_DESC"].ToString();
                decimal SKU_PRICE = Math.Round(decimal.Parse(itemsData.Rows[0]["TRADE_PRICE"].ToString()), 2);
                txtRate.Text = SKU_PRICE.ToString();
            }
            Session.Add("Dtsku_Price", itemsData);
        }
    }
    private bool CheckDublicateSKU()
    {
        SKU = (DataTable)Session["SKU"];
        DataRow[] foundRows = SKU.Select("SKU_ID  = '" + ddlSKU.SelectedItem.Value + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }
    private void LoadGird()
    {
        decimal TotalAmount = 0;
        decimal AdvanceAmount = 0;
        decimal ReceiptsAmount = 0;
        decimal RemainingAmount = 0;
        SKU = (DataTable)Session["SKU"];
        GrdItems.DataSource = SKU;
        GrdItems.DataBind();
        foreach (DataRow dr in SKU.Rows)
        {
            TotalAmount += Convert.ToDecimal(dr["AMOUNT"]);
        }
        txtallTotal.Text = string.Format("{0:N2}", TotalAmount);
        if (txtAdvanceAmount.Text.Equals(""))
        {
            txtAdvanceAmount.Text = string.Format("{0:N2}", AdvanceAmount);
        }
        else
        {
            AdvanceAmount = decimal.Parse(txtAdvanceAmount.Text);
        }
        ReceiptsAmount = decimal.Parse(_dc.chkNull_0(txtReceiptsAmount.Text));
        RemainingAmount = TotalAmount - AdvanceAmount - ReceiptsAmount;
        txtReceiptsAmount.Text = string.Format("{0:N2}", ReceiptsAmount);
        txtRemainingAmount.Text = string.Format("{0:N2}", RemainingAmount);
    }
    private DataTable GetCOAConfiguration()
    {
        try
        {
            COAMappingController _cController = new COAMappingController();
            DataTable dt = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            return null;
        }
    }
    private bool GetFinanceConfig()
    {
        try
        {
            ConfigurationController _cController = new ConfigurationController();
            DataTable dt = _cController.SelectAppConfiguration(1, (int)Enums.AppSettingMaster.FinanceSetting, null, Constants.IntNullValue);
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("CODE = '" + (int)Enums.AppSetting.IsFinanceIntegrate + "'");
                if (dr.Length > 0)
                {
                    return Convert.ToInt32(dr[0][2]) == 1 ? true : false;
                }
            }
            return false;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Error in Financial Setting!');", true);
            throw;
        }
    }
    private void GrandTotal()
    {
        //decimal GTotal = 0;
        //for (int i = 0; i < Gridview1.Rows.Count; i++)
        //{
        //    TextBox T1 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
        //    if (T1.Text == "" || T1.Text == null)
        //        T1.Text = "0";
        //    GTotal += Convert.ToDecimal(T1.Text);
        //}
        //itemsGrandTotal.Text = GTotal.ToString("0.00");
        //txtallTotal.Text = (Convert.ToDecimal(itemsGrandTotal.Text)).ToString("0.00");
    }
    private void ClearMaster()
    {
        btnSaveDocument.Text = "Save";
        txtallTotal.Text = "";
        txtAdvanceAmount.Text = "";
        txtReceiptsAmount.Text = "";
        txtRemainingAmount.Text = "";
        txtLadies.Text = "";
        txtGents.Text = "";
        CreateTable();
        CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
        txtEventDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        txtBookingDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
    }
    private void LoadEvent(long p_EVENT_BOOKING_ID)
    {
        decimal TotalAmount = 0;
        decimal AdvanceAmount = 0;
        decimal ReceiptsAmount = 0;
        decimal RemainingAmount = 0;
        DataTable dtEB = RptInventoryCtl.SelectEventBooking(p_EVENT_BOOKING_ID, 1);
        if(dtEB.Rows.Count > 0)
        {
            ddlCustomer.Value = dtEB.Rows[0]["CUSTOMER_ID"].ToString();
            DataTable dt = _cutsomerController.GetCustomerByID(Convert.ToInt32(dtEB.Rows[0]["CUSTOMER_ID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtIDCard.Text = dt.Rows[0]["CNIC"].ToString();
                txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                txtCustomerNo.Text = dt.Rows[0]["CONTACT_NUMBER"].ToString();
                txtOtherNo.Text = dt.Rows[0]["CONTACT2"].ToString();
            }
            DateTime EventDate = DateTime.Parse(dtEB.Rows[0]["EVENT_DATE"].ToString());
            txtEventDate.Text = EventDate.ToString("dd-MMM-yyyy");
            DateTime BookingDate = DateTime.Parse(dtEB.Rows[0]["BOOKING_DATE"].ToString());
            txtBookingDate.Text = BookingDate.ToString("dd-MMM-yyyy");
            txtLadies.Text = dtEB.Rows[0]["LADIES"].ToString();
            txtGents.Text = dtEB.Rows[0]["GENTS"].ToString();
            ddlEventType.Value = dtEB.Rows[0]["EVENT_TYPE_ID"].ToString();
            LoadEventTime(2);
            ddDineType.Value = dtEB.Rows[0]["EVENT_TIME_ID"].ToString();
            TotalAmount = decimal.Parse(dtEB.Rows[0]["TOTAL_AMOUNT"].ToString());
            AdvanceAmount = decimal.Parse(dtEB.Rows[0]["ADVANCE_AMOUNT"].ToString());
            ReceiptsAmount = decimal.Parse(dtEB.Rows[0]["RECEIPTS_AMOUNT"].ToString());
            RemainingAmount = decimal.Parse(dtEB.Rows[0]["CREDIT_AMOUNT"].ToString());
            txtallTotal.Text = string.Format("{0:N2}", Math.Round(TotalAmount, 2));
            txtAdvanceAmount.Text = string.Format("{0:N2}", Math.Round(AdvanceAmount, 2));
            txtReceiptsAmount.Text = string.Format("{0:N2}", Math.Round(ReceiptsAmount, 2));
            txtRemainingAmount.Text = string.Format("{0:N2}", Math.Round(RemainingAmount, 2));
            PanelReceipts.Visible = true;
            ExistenEventDetail(p_EVENT_BOOKING_ID);
            btnSaveDocument.Text = "Update";
        }
        
    }
    private void ExistenEventDetail(long p_EVENT_BOOKING_ID)
    {
        SKU = RptInventoryCtl.SelectEventBooking(p_EVENT_BOOKING_ID, 2);
        Session.Add("SKU", SKU);
        LoadGird();
    }

    #endregion

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = _cutsomerController.GetCustomerByID(Convert.ToInt32(ddlCustomer.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            txtIDCard.Text = dt.Rows[0]["CNIC"].ToString();
            txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
            txtCustomerNo.Text = dt.Rows[0]["CONTACT_NUMBER"].ToString();
            txtOtherNo.Text = dt.Rows[0]["CONTACT2"].ToString();
        }
    }

    protected void txtEventDate_TextChanged(object sender, EventArgs e)
    {
        if (Session["EVENT_BOOKING_ID"] == null)
        {
            LoadEventTime(1);
        }
        else
        {
            LoadEventTime(2);
        }
    }

    protected void ddlSKU_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dtsku_Detail = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = Dtsku_Detail.Select("SKU_ID  = '" + ddlSKU.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            decimal SKU_PRICE = Math.Round(decimal.Parse(foundRows[0]["TRADE_PRICE"].ToString()), 2);
            if (SKU_PRICE == Convert.ToDecimal(0.00))
            {
                txtRate.Text = "0.00";
            }
            else
            {
                txtRate.Text = SKU_PRICE.ToString();
            }
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        else
        {
            txtUOM.Text = "";
            txtRate.Text = "0.00";
        }
        txtQuantity.Focus();
    }

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKU.SelectedItem.Value + "'");
        string UOM_DESC = foundRows[0]["UOM_DESC"].ToString();
        decimal Price = decimal.Parse(_dc.chkNull_0(foundRows[0]["TRADE_PRICE"].ToString()));
        if (BtnAdd.Text == "Add")
        {
            if (CheckDublicateSKU())
            {
                DataRow dr = SKU.NewRow();
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
                dr["SKU_NAME"] = foundRows[0]["SKU_NAME"];
                dr["UOM_DESC"] = UOM_DESC;
                dr["PRICE"] = Price;
                dr["QTY"] = int.Parse(_dc.chkNull_0(txtQuantity.Text));
                decimal Amount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                dr["AMOUNT"] = Amount;
                dr["UOM_ID"] = foundRows[0]["SKU_ID"];
                dr["NET_AMOUNT"] = Convert.ToDecimal(Amount);
                dr["Category_Type_ID"] = foundRows[0]["Category_Type_ID"];
                SKU.Rows.Add(dr);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSKU.SelectedItem.Text + " Already Exists ');", true);
                return;
            }
        }
        else if (BtnAdd.Text == "Update")
        {
            SKU = (DataTable)Session["SKU"];
            DataRow dr = SKU.Rows[Convert.ToInt32(RowNo)];
            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
            dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
            dr["SKU_NAME"] = foundRows[0]["SKU_NAME"];
            dr["UOM_DESC"] = UOM_DESC;
            dr["PRICE"] = Price;
            dr["QTY"] = int.Parse(_dc.chkNull_0(txtQuantity.Text));
            decimal Amount = Price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
            dr["AMOUNT"] = Amount;
            dr["UOM_ID"] = foundRows[0]["SKU_ID"];
            dr["NET_AMOUNT"] = Convert.ToDecimal(Amount);
            dr["Category_Type_ID"] = foundRows[0]["Category_Type_ID"];
            BtnAdd.Text = "Add";
        }
        Session.Add("SKU", SKU);
        txtQuantity.Text = "";
        txtAmount.Text = "";
        LoadGird();
        ScriptManager.GetCurrent(Page).SetFocus(ddlSKU);
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        try
        {
            decimal TOTAL_AMOUNT = 0;
            SKU = (DataTable)Session["SKU"];
            foreach (DataRow dr in SKU.Rows)
            {
                TOTAL_AMOUNT += Convert.ToDecimal(dr["AMOUNT"]);
            }

            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();

            bool flag = true;
            if (btnSaveDocument.Text == "Save")
            {
                EVENT_BOOKING_ID = _eventBookingController.InsertEventBooking(
                    Convert.ToInt32(ddlCustomer.SelectedItem.Value),
                    Convert.ToDateTime(txtEventDate.Text), Convert.ToInt64(ddDineType.SelectedItem.Value), Convert.ToInt16(ddlEventType.SelectedItem.Value),
                     Convert.ToDateTime(txtBookingDate.Text), Convert.ToInt32(txtLadies.Text), Convert.ToInt32(txtGents.Text),
                Convert.ToInt32(Session["UserID"].ToString()), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()),
                Convert.ToInt32(ddlHallNo.SelectedItem.Value), SKU, TOTAL_AMOUNT, decimal.Parse(_dc.chkNull_0(txtAdvanceAmount.Text)),
                IsFinanceSetting, dtConfig);

                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                flag = true;
            }
            else if (btnSaveDocument.Text == "Update")
            {
                decimal CREDIT_AMOUNT = TOTAL_AMOUNT - decimal.Parse(_dc.chkNull_0(txtAdvanceAmount.Text)) - decimal.Parse(_dc.chkNull_0(txtReceiptsAmount.Text));
                EVENT_BOOKING_ID = _eventBookingController.UpdateEventBooking(Convert.ToInt64(Session["EVENT_BOOKING_ID"]),
                    Convert.ToInt32(ddlCustomer.SelectedItem.Value),
                    Convert.ToDateTime(txtEventDate.Text), Convert.ToInt64(ddDineType.SelectedItem.Value), Convert.ToInt16(ddlEventType.SelectedItem.Value),
                     Convert.ToDateTime(txtBookingDate.Text), Convert.ToInt32(txtLadies.Text), Convert.ToInt32(txtGents.Text),
                Convert.ToInt32(Session["UserID"].ToString()), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()),
                Convert.ToInt32(ddlHallNo.SelectedItem.Value), SKU, TOTAL_AMOUNT, decimal.Parse(_dc.chkNull_0(txtAdvanceAmount.Text)), CREDIT_AMOUNT,
                IsFinanceSetting, dtConfig);

                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                flag = true;
            }
            if (flag)
            {
                Session["EVENT_BOOKING_ID"] = null;
                Session["PRINT_ID"] = EVENT_BOOKING_ID;
                Session.Remove("SKU");
                PanelReceipts.Visible = true;
                ClearMaster();
                Response.Redirect("frmEventBookingView.aspx?LevelID=" + Request.QueryString["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=274");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearMaster();
        SKU = (DataTable)Session["SKU"];
        GrdItems.DataSource = SKU;
        GrdItems.DataBind();
        Response.Redirect("frmEventBookingView.aspx?LevelID=" + Request.QueryString["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=274");
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow Rows = (GridViewRow)(sender as LinkButton).NamingContainer;
        RowNo = Rows.RowIndex;
        ddlSKU.Value = Rows.Cells[0].Text;
        txtUOM.Text = Rows.Cells[3].Text;
        txtQuantity.Text = Rows.Cells[4].Text;
        txtRate.Text = Rows.Cells[5].Text;
        txtAmount.Text = Rows.Cells[6].Text;
        BtnAdd.Text = "Update";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow Rows = (GridViewRow)(sender as LinkButton).NamingContainer;
        SKU = (DataTable)Session["SKU"];
        if (SKU.Rows.Count > 0)
        {
            SKU.Rows.RemoveAt(Rows.RowIndex);
            Session.Add("SKU", SKU);
            LoadGird();
        }
    }

    public void ShowPopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        DataTable dt = mController.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
        ReportDocument CrpReport = new ReportDocument();
        CrpReport = new CrpEventBooking();
        DataSet ds = null;
        ds = RptInventoryCtl.SelectEventPopUp(long.Parse(Session["PRINT_ID"].ToString()), 2);
        ReportDocument subReport = CrpReport.OpenSubreport("Subreport");
        CrpReport.SetDataSource(ds);
        subReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("DocumentType", "Event Booking");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void ddlHallNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEventTime(1);
    }
}