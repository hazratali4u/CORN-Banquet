using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.IO.Ports;
using CORNBusinessLayer.Reports;

/// <summary>
/// From To Adjust Stock
/// </summary>
public partial class Forms_frmPysicalStockteken : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    readonly PhaysicalStockController PhyscalStock = new PhaysicalStockController();
    readonly DataControl _dc = new DataControl();
    DataTable _purchaseSku;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            //if (IsDayClosed())
            //{
            //    UserController UserCtl = new UserController();

            //    UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            //    Session.Clear();
            //    System.Web.Security.FormsAuthentication.SignOut();
            //    Response.Redirect("../Login.aspx");
            //}
            CreatTable();
            LoadDistributor();
            GetDocumentNo();
            LoadSkuDetail();
        }
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_Code", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSku.Columns.Add("UNIT_RATE", typeof(decimal));
        _purchaseSku.Columns.Add("Quantity", typeof(decimal));
        _purchaseSku.Columns.Add("DateTime", typeof(DateTime));
        Session.Add("PurchaseSKU", _purchaseSku);
    }

    private void GetDocumentNo()
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
            {
                if (dr["MaxDayClose"].ToString().Length > 0)
                {
                    CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                    break;
                }
            }
        }
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            drpDocumentNo.Items.Clear();
            PurchaseController mPurchase = new PurchaseController();
            DataTable dt = mPurchase.SelectPurchaseDocumentNo(10, Constants.IntNullValue, Convert.ToInt32(Session["UserID"]), CurrentWorkDate);
            drpDocumentNo.Items.Add(new DevExpress.Web.ListEditItem("New", Constants.LongNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, 0, 0, false);
            drpDocumentNo.SelectedIndex = 0;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.Value.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            LoadGird();
            drpDistributor.Enabled = true;
            ClearAll();
            txtRemarks.Text = "";
            DisAbaleOption(false);
        }
        else
        {
            drpDistributor.Enabled = false;
            LoadDocumentDetail();
            LoadSkuDetail();
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
    }

    private void LoadSkuDetail()
    {
        hfInventoryType.Value = "0";
        SkuController SKUCtl = new SkuController();
        DataTable dtskuPrice = SKUCtl.SelectSkuInfo(Convert.ToInt32(drpDistributor.Value), Constants.IntNullValue, Constants.IntNullValue, 18, int.Parse(Session["CompanyId"].ToString()), 0);
        clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);

        if (dtskuPrice.Rows.Count > 0)
        {
            txtUOM.Text = dtskuPrice.Rows[0]["UOM_DESC"].ToString();
            ddlSkus.SelectedIndex = 0;
            if (dtskuPrice.Rows[0]["IsInventoryWeight"].ToString() != "")
            {
                if (Convert.ToBoolean(dtskuPrice.Rows[0]["IsInventoryWeight"]))
                {
                    //txtQuantity.Enabled = false;
                    hfInventoryType.Value = "1";
                }
            }
        }
        Session.Add("Dtsku_Price", dtskuPrice);

    }

    private void LoadGird()
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        GrdPurchase.DataSource = _purchaseSku;
        GrdPurchase.DataBind();
    }

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        }
        txtQuantity.Focus();
        btnSave.Text = "Update";
    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        if (_purchaseSku.Rows.Count > 0)
        {
            _purchaseSku.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
        }
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(10, Constants.IntNullValue, long.Parse(drpDocumentNo.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.Value = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
            txtRemarks.Text = dt.Rows[0]["REMARKS"].ToString();
            _purchaseSku = PhyscalStock.SelectPhysicalStockTakingDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()),1);
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
        }
    }

    private bool CheckDublicateSku()
    {
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        DataRow[] foundRows = _purchaseSku.Select("SKU_ID  = '" + ddlSkus.Value + "'");
        decimal Qty = 0;
        if (foundRows.Length > 0)
        {
            foreach (DataRow dr in _purchaseSku.Rows)
            {
                if (dr["SKU_ID"].ToString() == ddlSkus.Value.ToString())
                {
                    Qty = Convert.ToDecimal(dr["Quantity"]);
                    dr["Quantity"] = Qty + decimal.Parse(txtQuantity.Text);
                    dr["DateTime"] = System.DateTime.Now;
                    break;
                }
            }
            DataView dv = new DataView(_purchaseSku);
            dv.Sort = "DateTime DESC";
            _purchaseSku = dv.ToTable();
            Session.Add("PurchaseSKU", _purchaseSku);
            ClearAll();
            LoadGird();
            DisAbaleOption(true);
            ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Adds Document Detail To Document Detail Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) > 0 || drpDocumentNo.Value.ToString() != Constants.LongNullValue.ToString())
        {
            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.Value + "'");
            if (foundRows.Length > 0)
            {
                _purchaseSku = (DataTable)Session["PurchaseSKU"];
                if (btnSave.Text == "Add")
                {
                    if (CheckDublicateSku())
                    {
                        DataRow dr = _purchaseSku.NewRow();
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["UNIT_RATE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                        dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        dr["UOM_DESC"] = txtUOM.Text;
                        dr["DateTime"] = System.DateTime.Now;
                        _purchaseSku.Rows.InsertAt(dr,0);
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    DataRow dr = _purchaseSku.Rows[Convert.ToInt32(_rowNo.Value)];
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["UNIT_RATE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                    dr["Quantity"] = decimal.Parse(txtQuantity.Text);
                    dr["UOM_DESC"] = txtUOM.Text;
                    dr["DateTime"] = System.DateTime.Now;
                }
                DataView dv = new DataView(_purchaseSku);
                dv.Sort = "DateTime DESC";
                _purchaseSku = dv.ToTable();

                Session.Add("PurchaseSKU", _purchaseSku);
                ClearAll();
                LoadGird();
                DisAbaleOption(true);
                ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong item please check in list');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity');", true);
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
    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        //if (IsDayClosed())
        //{
        //    UserController UserCtl = new UserController();

        //    UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
        //    Session.Clear();
        //    System.Web.Security.FormsAuthentication.SignOut();
        //    Response.Redirect("../Login.aspx");
        //}

        var mDayClose = new DistributorController();
        DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.Value.ToString()));
        if (dt.Rows.Count > 0)
        {
            DateTime mWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());
            DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];            
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = PhyscalStock.InsertPhysicalStockTaking(int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate, txtRemarks.Text, int.Parse(Session["UserId"].ToString()), dtPurchaseDetail);
                if(mResult)
                {
                    ShowReport(mWorkDate);
                }
            }
            else
            {
                bool mResult = PhyscalStock.UpdatePhysicalStockTaking(long.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), mWorkDate, int.Parse(Session["UserId"].ToString()), txtRemarks.Text, dtPurchaseDetail);
                if(mResult)
                {
                    ShowReport(mWorkDate);
                }
            }

            lblErrorMsg.Text = "Record Upated";
            _purchaseSku = (DataTable)Session["PurchaseSKU"];
            _purchaseSku.Rows.Clear();
            Session.Add("PurchaseSKU", _purchaseSku);
            LoadGird();
            GetDocumentNo();
            ClearAll();
            txtRemarks.Text = "";
            DisAbaleOption(false);

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CreatTable();
        LoadGird();
        drpDistributor.Enabled = true;

        ClearAll();
        txtRemarks.Text = "";
        DisAbaleOption(false);
    }

    /// <summary>
    /// Enables/Disables Controls
    /// </summary>
    /// <param name="IsDisable">bool</param>
    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;

        }
        else
        {
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    private void ClearAll()
    {

        txtQuantity.Text = "1";

        ddlSkus.Enabled = true;
        btnSave.Text = "Add";
        lblErrorMsg.Text = "";

    }

    private bool IsDayClosed()
    {
        DistributorController DistrCtl = new DistributorController();
        try
        {
            DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), 3);
            if (dtDayClose.Rows.Count > 0)
            {
                if (Convert.ToDateTime(Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtQuantity.Enabled = true;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
            if (foundRows[0]["IsInventoryWeight"].ToString() != "")
            {
                if (Convert.ToBoolean(foundRows[0]["IsInventoryWeight"]))
                {
                    //txtQuantity.Enabled = false;
                    hfInventoryType.Value = "1";
                }
                else
                {
                    txtQuantity.Text = string.Empty;
                    hfInventoryType.Value = "0";
                }
            }
            else
            {
                txtQuantity.Text = string.Empty;
            }
        }
        txtQuantity.Focus();
    }

    private void ShowReport(DateTime CurrentWorkDate)
    {
        try
        {
            DsReport ds = new DsReport();
            DataTable dt = new DataTable();
            dt.Columns.Add("PhysiclaStockTaking_ID", typeof(long));
            dt.Columns.Add("REMARKS", typeof(string));
            dt.Columns.Add("SKU_ID", typeof(int));
            dt.Columns.Add("SKU_NAME", typeof(string));
            dt.Columns.Add("QUANTITY", typeof(decimal));
            dt.Columns.Add("UOM", typeof(string));
            dt.Columns.Add("DATE", typeof(DateTime));
            int i = 0;
            DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];
            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                dt.Rows.Add();
                dt.Rows[i]["PhysiclaStockTaking_ID"] = 0;
                dt.Rows[i]["REMARKS"] = txtRemarks.Text;
                dt.Rows[i]["SKU_ID"] = Convert.ToInt32(dr["SKU_ID"]);
                dt.Rows[i]["SKU_NAME"] = dr["SKU_NAME"];
                dt.Rows[i]["QUANTITY"] = Convert.ToDecimal(dr["QUANTITY"]);
                dt.Rows[i]["UOM"] = dr["UOM_DESC"];
                dt.Rows[i]["DATE"] = CurrentWorkDate;
                i += 1;
            }
            foreach (DataRow dr in dt.Rows)
            {
                ds.Tables["PhysicalStockTaking"].ImportRow(dr);
            }
            var crpReport = new CrpPhysicalStockTaking();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow1", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
}