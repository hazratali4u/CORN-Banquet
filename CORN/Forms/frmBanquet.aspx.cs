using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.IO;
using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web.Services;

public partial class Forms_frmBanquet : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly SkuController _mSkuController = new SkuController();
    readonly CustomerDataController _cutsomerController = new CustomerDataController();
    readonly EventBookingController _eventBookingController = new EventBookingController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadEventType();
            LoadCustomer();
            LoadHall();

            SetInitialRow();
            btnSaveDocument.Attributes.Add("onclick", "return ValidateForm()");

            LoadLookupGrid("");
        }
    }

    #region Page Load
    private void LoadEventType()
    {
        DataTable dt = _eventBookingController.GetEventTypes();
        clsWebFormUtil.FillDxComboBoxList(ddlEventType, dt, "EVENT_TYPE_ID", "EVENT_TYPE_DESC");

        if (dt.Rows.Count > 0)
        {
            ddlEventType.SelectedIndex = 0;
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
        GeoHierarchyController _hierarchyController = new GeoHierarchyController();

        DataTable dt = _hierarchyController.GetTableDefination(Constants.IntNullValue, Constants.IntNullValue, true, int.Parse(Session["UserID"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(txtHallNo, dt, "TableDefination_ID", "TableDefination_No");

        if (dt.Rows.Count > 0)
        {
            ddlEventType.SelectedIndex = 0;
        }
    }
    #endregion

    #region add button Pop UP
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        //clearMaster();
        //ClearAll();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
        //ClearAll();
        btnSaveDocument.Text = "Save";
        //CreatTable();
        mPopUpLocation.Hide();

    }
    #endregion

    #region Lookup Grid
    protected void Grid_users_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "1")
            {
                e.Row.Visible = false;
            }
        }
    }

    protected void Grid_users_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserController _mUController = new UserController();
        mPopUpLocation.Show();
        try
        {
            GridViewRow gvr = Grid_users.Rows[e.NewEditIndex];
            hfMaster_ID.Value = gvr.Cells[1].Text;
            ddlCustomer.Value = gvr.Cells[2].Text;
            ddlCustomer_SelectedIndexChanged(null, null);
            txtEventDate.Text = gvr.Cells[4].Text;
            txtEventDate_SelectedIndexChanged(null, null);

            DataTable dineDt = new DataTable();

            dineDt.Columns.Add(new DataColumn("EVENT_TIME_ID", typeof(string)));
            dineDt.Columns.Add(new DataColumn("EVENT_TIME_SPAN", typeof(string)));

            DataRow dr1 = dineDt.NewRow();
            dr1["EVENT_TIME_ID"] = gvr.Cells[6].Text;
            dr1["EVENT_TIME_SPAN"] = gvr.Cells[5].Text;

            dineDt.Rows.Add(dr1);

            clsWebFormUtil.FillDxComboBoxList(ddDineType, dineDt, "EVENT_TIME_ID", "EVENT_TIME_SPAN", false);

            ddDineType.Items.FindByValue(gvr.Cells[6].Text).Selected = true;

            //ddDineType.Value = gvr.Cells[6].Text;
            ddlEventType.Value = gvr.Cells[8].Text;
            txtHallNo.Value = gvr.Cells[10].Text;
            txtLadies.Text = gvr.Cells[11].Text;
            txtGents.Text = gvr.Cells[12].Text;
            txtBookingDate.Text = gvr.Cells[13].Text;

            var details = _eventBookingController.Select_Event_Booking_Details(Convert.ToInt64(hfMaster_ID.Value));

            DataTable items = (DataTable)Session["CurrentTable"];

            for (int i=0; i < items.Rows.Count; i++)
            {
                items.Rows.RemoveAt(i);
            }

            foreach (DataRow item in details.Rows)
            {
                DataRow dr = items.NewRow();
                dr["ddlSKU"] = item["SKU_ID"].ToString();
                dr["txtPax"] = item["QTY"].ToString();
                dr["txtRate"] = item["RATE"].ToString();
                dr["txtAmount"] = item["AMOUNT"].ToString();
                //dr["Amount_Value"] = item["AMOUNT"].ToString();
                dr["txtUOM"] = item["UOM_DESC"].ToString();
                dr["UOM_ID"] = item["UOM_ID"].ToString();

                items.Rows.Add(dr);
            }
            Gridview1.DataSource = items;
            Gridview1.DataBind();


            Session.Add("CurrentTable", items);
            LoadItemsGird();

            btnSaveDocument.Text = "Update";
        }
        catch (Exception ex)
        {
            //btnSave.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
            ex.Message.ToString();
        }
    }

    private void LoadItemsGird()
    {
        if (Session["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)Session["CurrentTable"];

            if (dt.Rows.Count > 0)
            {
                var itemsData = _mSkuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Set the Previous Selected Items on Each DropDownList on Postbacks
                    ASPxComboBox items = (ASPxComboBox)Gridview1.Rows[i].Cells[1].FindControl("ddlSKU");

                    items.DataSource = itemsData;
                    items.TextField = "SKU_NAME";
                    items.ValueField = "SKU_ID";
                    items.DataBind();

                    TextBox T1 = (TextBox)Gridview1.Rows[i].Cells[5].FindControl("txtRate");
                    TextBox T2 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
                    TextBox T3 = (TextBox)Gridview1.Rows[i].Cells[4].FindControl("txtPax");

                    TextBox T4 = (TextBox)Gridview1.Rows[i].Cells[3].FindControl("txtUOM");

                    items.Items.FindByValue(dt.Rows[i]["ddlSKU"].ToString()).Selected = true;
                    T1.Text = dt.Rows[i]["txtRate"].ToString();
                    T2.Text = dt.Rows[i]["txtAmount"].ToString();
                    T3.Text = dt.Rows[i]["txtPax"].ToString();
                    T4.Text = dt.Rows[i]["txtUOM"].ToString();

                    Gridview1.Rows[i].Cells[2].Text = dt.Rows[i]["UOM_ID"].ToString();
                    //Gridview1.Rows[i].Cells[7].Text = dt.Rows[i]["Amount_Value"].ToString();

                    Gridview1.Rows[i].Cells[0].Text = (i + 1).ToString();
                }
                GrandTotal();
            }
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadLookupGrid("");
    }

    protected void LoadLookupGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();

        DataTable dt = new DataTable();

        dt = _eventBookingController.Select_Event_Booking_Lookup(null, Constants.IntNullValue);

        if (pType == "")
        {
            //if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            //{
            //    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            //}
            //Grid_users.DataSource = dt;
            //Grid_users.DataBind();
            if (dt.Rows.Count > 0)
            {
                Grid_users.PageIndex = 0;
            }
            Grid_users.DataSource = dt;
            Grid_users.DataBind();
        }
        else
        {
            //if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            //{
            //    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            //}
            if (dt.Rows.Count > 0)
            {
                Grid_users.PageIndex = 0;
            }
            Grid_users.DataSource = dt;
            Grid_users.DataBind();
        }
    }
    #endregion


    #region Selected Index Change
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = _cutsomerController.GetCustomerByID(Convert.ToInt32(ddlCustomer.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            txtIDCard.Text =  dt.Rows[0]["CNIC"].ToString();
            txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
            txtCustomerNo.Text = dt.Rows[0]["CONTACT_NUMBER"].ToString();
            txtOtherNo.Text = dt.Rows[0]["CONTACT2"].ToString();
        }
        mPopUpLocation.Show();
    }

    protected void txtEventDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = _eventBookingController.SelectDineTimes(Convert.ToDateTime(txtEventDate.Text), Constants.IntNullValue,1);
        clsWebFormUtil.FillDxComboBoxList(ddDineType, dt, "EVENT_TIME_ID", "EVENT_TIME_SPAN", true);

        if (dt.Rows.Count > 0)
        {
            ddDineType.SelectedIndex = 0;
        }
        mPopUpLocation.Show();

        GrandTotal();
    }
    #endregion


    #region Bulk Add Items
    protected void btnBulkClose_Click(object sender, EventArgs e)
    {
        Session["CurrentTable"] = null;
        Gridview1.DataSource = null;
        Gridview1.DataBind();
        SetInitialRow();
        //mPOPBulkAdd.Hide();
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ddlSKU", typeof(string)));
        dt.Columns.Add(new DataColumn("txtPax", typeof(string)));
        dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
        dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("UOM_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("txtUOM", typeof(string)));
       // dt.Columns.Add(new DataColumn("Amount_Value", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["ddlSKU"] = string.Empty;
        dr["txtRate"] = string.Empty;
        dr["txtAmount"] = string.Empty;
        dr["txtPax"] = string.Empty;
        dr["UOM_ID"] = string.Empty;
        dr["txtUOM"] = string.Empty;
        //dr["Amount_Value"] = string.Empty;

        dt.Rows.Add(dr);

        //dr = dt.NewRow();
        //Store the DataTable in Session

        Session["CurrentTable"] = dt;
        Gridview1.DataSource = dt;
        Gridview1.DataBind();

        ASPxComboBox type = (ASPxComboBox)Gridview1.Rows[0].Cells[1].FindControl("ddlSKU");
        type.Focus();
    }

    private void AddNewRowToGrid()
    {
        if (Session["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)Session["CurrentTable"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                try
                {
                    Session["CurrentTable"] = dtCurrentTable;
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                    {

                        //extract the TextBox values
                        ASPxComboBox box1 = (ASPxComboBox)Gridview1.Rows[i].Cells[1].FindControl("ddlSKU");
                        TextBox box5 = (TextBox)Gridview1.Rows[i].Cells[5].FindControl("txtRate");
                        TextBox box6 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
                        TextBox box4 = (TextBox)Gridview1.Rows[i].Cells[4].FindControl("txtPax");

                        TextBox box3 = (TextBox)Gridview1.Rows[i].Cells[3].FindControl("txtUOM");
                        dtCurrentTable.Rows[i]["UOM_ID"] = Gridview1.Rows[i].Cells[2].Text;
                        //dtCurrentTable.Rows[i]["Amount_Value"] = Gridview1.Rows[i].Cells[7].Text;


                        dtCurrentTable.Rows[i]["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i]["ddlSKU"] = box1.SelectedItem.Value;
                        dtCurrentTable.Rows[i]["txtRate"] = box5.Text;
                        dtCurrentTable.Rows[i]["txtAmount"] = box6.Text;
                        dtCurrentTable.Rows[i]["txtPax"] = box4.Text;
                        dtCurrentTable.Rows[i]["txtUOM"] = box3.Text;
                    }

                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                    //UpdatePanel4.Update();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        else
        {
            Response.Write("Session is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }

    private void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (Session["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)Session["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    var itemsData = _mSkuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (i == dt.Rows.Count - 1 && (dt.Rows[i]["ddlSKU"].ToString() == null ||
                            dt.Rows[i]["ddlSKU"].ToString() == ""))
                        {
                            rowIndex = rowIndex - 1;
                        }

                        //Set the Previous Selected Items on Each DropDownList on Postbacks
                        ASPxComboBox items = (ASPxComboBox)Gridview1.Rows[i].Cells[1].FindControl("ddlSKU");

                        items.DataSource = itemsData;
                        items.TextField = "SKU_NAME";
                        items.ValueField = "SKU_ID";
                        items.DataBind();

                        TextBox T1 = (TextBox)Gridview1.Rows[i].Cells[5].FindControl("txtRate");
                        TextBox T2 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
                        TextBox T3 = (TextBox)Gridview1.Rows[i].Cells[4].FindControl("txtPax");

                        TextBox T4 = (TextBox)Gridview1.Rows[i].Cells[3].FindControl("txtUOM");

                        if (i < dt.Rows.Count - 1)
                        {
                            items.Items.FindByValue(dt.Rows[i]["ddlSKU"].ToString()).Selected = true;

                            T1.Text = dt.Rows[i]["txtRate"].ToString();
                            T2.Text = dt.Rows[i]["txtAmount"].ToString();
                            T3.Text = dt.Rows[i]["txtPax"].ToString();
                            T4.Text = dt.Rows[i]["txtUOM"].ToString();
                            Gridview1.Rows[i].Cells[2].Text = dt.Rows[i]["UOM_ID"].ToString();
                            //Gridview1.Rows[i].Cells[7].Text = dt.Rows[i]["Amount_Value"].ToString();

                        }
                        else
                        {
                            items.Items.FindByValue(dt.Rows[rowIndex]["ddlSKU"].ToString()).Selected = true;

                            ASPxComboBox type = (ASPxComboBox)Gridview1.Rows[i].Cells[1].FindControl("ddlSKU");
                            type.Focus();

                            T1.Text = dt.Rows[rowIndex]["txtRate"].ToString();
                            T3.Text = dt.Rows[rowIndex]["txtPax"].ToString();
                            T4.Text = dt.Rows[rowIndex]["txtUOM"].ToString();
                            Gridview1.Rows[i].Cells[2].Text = dt.Rows[rowIndex]["UOM_ID"].ToString();

                            if (T1.Text == "" || T1.Text == null)
                                T1.Text = "0";
                            if (T3.Text == "" || T3.Text == null)
                                T3.Text = "0";

                            var totalAmount = Convert.ToDecimal(T1.Text) * Convert.ToDecimal(T3.Text);
                            T2.Text = totalAmount.ToString();

                            //Gridview1.Rows[i].Cells[7].Text = totalAmount.ToString();

                            //T2.Text = dt.Rows[rowIndex]["txtAmount"].ToString();
                            Gridview1.Rows[i].Cells[0].Text = (i + 1).ToString();
                        }
                        rowIndex++;
                    }
                }
            }
            GrandTotal();
            mPopUpLocation.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        mPopUpLocation.Show();
    }

    protected void ddlSKU_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //string id = dropDownList.ID;


        ASPxComboBox ddl1 = (ASPxComboBox)sender;
        GridViewRow row = (GridViewRow)ddl1.NamingContainer;
        //GridViewRow row = (GridViewRow)((ASPxComboBox)sender).Parent.Parent;
        if (row != null)
        {

            DataTable _mDt = _mSkuController.SelectSkuData(Convert.ToInt32(ddl1.SelectedItem.Value), Constants.IntNullValue);

            TextBox rate = (TextBox)row.FindControl("txtRate");
            TextBox uom = (TextBox)row.FindControl("txtUOM");
            TextBox qty = (TextBox)row.FindControl("txtPax");
            TextBox amount = (TextBox)row.FindControl("txtAmount");

            if (_mDt.Rows.Count > 0)
            {
                _mDt.Rows[0]["TRADE_PRICE"] = !string.IsNullOrEmpty(_mDt.Rows[0]["TRADE_PRICE"].ToString()) ? _mDt.Rows[0]["TRADE_PRICE"] : 0;
                rate.Text = Convert.ToDecimal(_mDt.Rows[0]["TRADE_PRICE"]).ToString("F");

                uom.Text = _mDt.Rows[0]["UOM_DESC"].ToString();
                row.Cells[2].Text = _mDt.Rows[0]["intSaleMUnitCode"].ToString();

                var qtyValue = !string.IsNullOrEmpty(qty.Text) ? qty.Text : "0";
                var rowAmount = Convert.ToDecimal(qtyValue) * Convert.ToDecimal(rate.Text);
                amount.Text = rowAmount.ToString("F");

                //row.Cells[7].Text = amount.Text;
            }

            GrandTotal();
        }

        //DataTable dt = DptTpe.GetUOM(0, Convert.ToInt32(ddl1.SelectedItem.Value), Constants.IntNullValue);

        //ASPxComboBox ddl = (ASPxComboBox)row.FindControl("ddlUOM1");
        //ddl.DataSource = dt;
        //ddl.TextField = "UOM_DESC";
        //ddl.ValueField = "UOM_ID";
        //ddl.DataBind();

        //if (dt.Rows.Count > 0)
        //{
        //    ddl.SelectedIndex = 0;
        //}

        mPopUpLocation.Show();
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        ASPxComboBox btnAdd = (ASPxComboBox)e.Row.Cells[0].FindControl("ddlSKU");
        if (btnAdd != null)
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
        }
    }
    protected void BulkAdd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                LinkButton imgBtnDelete = (LinkButton)e.Row.FindControl("bulkDeletebtn");
                if (e.Row.DataItemIndex == 0)
                    imgBtnDelete.Visible = false;
            }

            DataTable itemsdtSKU = _mSkuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);

            ASPxComboBox ddlCategoryType = (ASPxComboBox)e.Row.FindControl("ddlSKU");
            ddlCategoryType.DataSource = itemsdtSKU;
            ddlCategoryType.TextField = "SKU_NAME";
            ddlCategoryType.ValueField = "SKU_ID";
            ddlCategoryType.DataBind();

            if (itemsdtSKU.Rows.Count > 0)
            {
                TextBox uom = (TextBox)e.Row.FindControl("txtUOM");

                if (ddlCategoryType.SelectedIndex == -1)
                {
                    ddlCategoryType.SelectedIndex = 0;
                    uom.Text = itemsdtSKU.Rows[0]["UOM_DESC"].ToString();
                    e.Row.Cells[2].Text = itemsdtSKU.Rows[0]["intSaleMUnitCode"].ToString();
                }

                DataTable _mDt = _mSkuController.SelectSkuData(Convert.ToInt32(ddlCategoryType.SelectedItem.Value), Constants.IntNullValue);
                TextBox rate = (TextBox)e.Row.FindControl("txtRate");

                if (_mDt.Rows.Count > 0)
                {
                    _mDt.Rows[0]["TRADE_PRICE"] = !string.IsNullOrEmpty(_mDt.Rows[0]["TRADE_PRICE"].ToString()) ? _mDt.Rows[0]["TRADE_PRICE"] : 0;
                    rate.Text = Convert.ToDecimal(_mDt.Rows[0]["TRADE_PRICE"]).ToString("F");
                }
            }

            GrandTotal();
        }
    }

    private void GrandTotal()
    {
        decimal GTotal = 0;
        for (int i = 0; i < Gridview1.Rows.Count; i++)
        {
            TextBox T1 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
            if (T1.Text == "" || T1.Text == null)
                T1.Text = "0";
            GTotal += Convert.ToDecimal(T1.Text);
        }
        itemsGrandTotal.Text = GTotal.ToString("0.00");

        txtallTotal.Text = (Convert.ToDecimal(itemsGrandTotal.Text)).ToString("0.00");
    }

    public void btnCancelBulk_Click(object sender, EventArgs args)
    {
        Session["CurrentTable"] = null;
        Gridview1.DataSource = null;
        Gridview1.DataBind();
        SetInitialRow();
    }
    protected void Gridview1_RowDeleting(object sender, EventArgs e)
    {
        LinkButton lnkbtndel = sender as LinkButton;
        GridViewRow gdrow = lnkbtndel.NamingContainer as GridViewRow;

        if (gdrow.RowIndex != 0)
        {
            int index = Convert.ToInt32(gdrow.RowIndex);
            DataTable dt = Session["CurrentTable"] as DataTable;
            dt.Rows[index].Delete();
            Session["CurrentTable"] = dt;
            Gridview1.DataSource = dt;
            Gridview1.DataBind();
        }
        mPopUpLocation.Show();
    }
    #endregion


    #region SAVE Document
    protected void btnSave_Document(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                decimal totalAmount = 0;
                for (int i = 0; i < Gridview1.Rows.Count; i++)
                {
                    TextBox box3 = (TextBox)Gridview1.Rows[i].Cells[6].FindControl("txtAmount");
                    totalAmount += !string.IsNullOrEmpty(box3.Text) ? Convert.ToDecimal(box3.Text) : 0;
                }

                DataTable dtConfig = GetCOAConfiguration();
                bool IsFinanceSetting = GetFinanceConfig();

                bool flag = true;
                if (btnSaveDocument.Text == "Save")
                {
                    var saved = _eventBookingController.InsertEventBooking(
                        Convert.ToInt32(ddlCustomer.SelectedItem.Value),
                        Convert.ToDateTime(txtEventDate.Text), Convert.ToInt64(ddDineType.SelectedItem.Value), Convert.ToInt16(ddlEventType.SelectedItem.Value),
                         Convert.ToDateTime(txtBookingDate.Text), Convert.ToInt32(txtLadies.Text), Convert.ToInt32(txtGents.Text),
                    Convert.ToInt32(Session["UserID"].ToString()), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()),
                    Convert.ToInt32(txtHallNo.SelectedItem.Value),null, totalAmount, decimal.Parse(_dc.chkNull_0(txtAdvanceAmount.Text)),
                    IsFinanceSetting, dtConfig);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                    flag = true;
                }
                else if (btnSaveDocument.Text == "Update")
                {
                    var saved = _eventBookingController.UpdateEventBooking(Convert.ToInt64(hfMaster_ID.Value),
                        Convert.ToInt32(ddlCustomer.SelectedItem.Value),
                        Convert.ToDateTime(txtEventDate.Text), Convert.ToInt64(ddDineType.SelectedItem.Value), Convert.ToInt16(ddlEventType.SelectedItem.Value),
                         Convert.ToDateTime(txtBookingDate.Text), Convert.ToInt32(txtLadies.Text), Convert.ToInt32(txtGents.Text),
                    Convert.ToInt32(Session["UserID"].ToString()), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()),
                    Convert.ToInt32(txtHallNo.SelectedItem.Value), null, totalAmount, decimal.Parse(_dc.chkNull_0(txtAdvanceAmount.Text)),0,
                    IsFinanceSetting, dtConfig);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                    flag = true;
                    //mPopUpLocation.Hide();
                }
                if (flag)
                {
                    //mPopUpLocation.Show();
                    //LoadLookupGrid("");
                    //clearMaster();
                    //ClearAll();
                    btnSaveDocument.Text = "Save";
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                //mPopUpLocation.Show();
                //LoadApprovalBy();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
    #endregion
}