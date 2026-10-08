using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Globalization;
using System.Web;
using System.IO.Ports;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmPurchaseEntry : System.Web.UI.Page
{
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    readonly PurchaseController _mPurchaseCtrl = new PurchaseController();
    readonly SkuController SKUCtl = new SkuController();
    readonly DataControl _dc = new DataControl();
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    DataTable _purchaseSkus;

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

            DrpDocumentType.Focus();

            LoadPrincipal();
            LoadDistributor();
            LoadSkuDetail();
            CreatTable();
            GetDocumentNo();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            drpPrincipal_SelectedIndexChanged(null,null);
            ddlSkus_SelectedIndexChanged(null, null);
            LoadVoucher();
        }
    }

    private void CreatTable()
    {
        _purchaseSkus = new DataTable();
        _purchaseSkus.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        _purchaseSkus.Columns.Add("SKU_ID", typeof(int));
        _purchaseSkus.Columns.Add("SKU_Code", typeof(string));
        _purchaseSkus.Columns.Add("SKU_Name", typeof(string));
        _purchaseSkus.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSkus.Columns.Add("PRICE", typeof(decimal));
        _purchaseSkus.Columns.Add("Quantity", typeof(decimal));
        _purchaseSkus.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSkus.Columns.Add("UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_UOM_ID", typeof(int));
        _purchaseSkus.Columns.Add("S_Quantity", typeof(decimal));

        Session.Add("PurchaseSKUS", _purchaseSkus);

    }

    #region Load
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

    private void GetDocumentNo()
    {
        int TypeID = Convert.ToInt32(DrpDocumentType.Value);
        if (DrpDocumentType.Value.ToString() == "20")
        {
            TypeID = 22;
        }
        drpDocumentNo.Items.Clear();

        DataTable dt = _mPurchaseCtrl.SelectPurchaseDocumentNo(TypeID, Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);

        drpDocumentNo.Items.Add(new DevExpress.Web.ListEditItem("New", Constants.LongNullValue.ToString()));

        clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, 0, 0, false);

        drpDocumentNo.SelectedIndex = 0;
    }

    private void LoadPrincipal()
    {
        try
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable dtVendor = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, Constants.DateNullValue);
            clsWebFormUtil.FillDxComboBoxList(drpPrincipal, dtVendor, 0, 1);
            if (dtVendor.Rows.Count > 0)
            {
                drpPrincipal.SelectedIndex = 0;
            }


            //DataTable dt = _skuHierarchyController.SelectDropdownCategoryItems(Constants.IntNullValue, null, true, 3);
            //clsWebFormUtil.FillDxComboBoxList(drpPrincipal, dt, "SKU_HIE_ID", "SKU_HIE_NAME", false);
            //if (dt.Rows.Count > 0)
            //{
            //    drpPrincipal.SelectedIndex = 0;
            //}
            //else
            //{
            //    drpPrincipal.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }

    }
    protected void drpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
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

    private void LoadToDistributor()
    {
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorInfo(Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(DrpTransferFor, dt, 0, 2, true);

        if (dt.Rows.Count > 0)
        {
            DrpTransferFor.SelectedIndex = 0;
        }
    }

    private void LoadDocumentDetail()
    {
        DataTable dt = _mPurchaseCtrl.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            if (DrpDocumentType.SelectedIndex == 0)
            {
                drpDistributor.Value = dt.Rows[0]["SOLD_TO"].ToString();
                drpPrincipal.Value = dt.Rows[0]["SOLD_FROM"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
                txtGstAmount.Text = dt.Rows[0]["GST_AMOUNT"].ToString();
                txtDiscount.Text = dt.Rows[0]["DISCOUNT"].ToString();
            }
            else if (DrpDocumentType.SelectedIndex == 1)
            {
                drpDistributor.Value = dt.Rows[0]["SOLD_FROM"].ToString();
                DrpTransferFor.Value = dt.Rows[0]["SOLD_TO"].ToString();
                txtAmount.Text = dt.Rows[0]["TOTAL_AMOUNT"].ToString();
                txtGstAmount.Text = dt.Rows[0]["GST_AMOUNT"].ToString();
                txtNetAmount.Text = dt.Rows[0]["NET_AMOUNT"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            else if (DrpDocumentType.SelectedIndex == 2)
            {
                drpPrincipal.Value = dt.Rows[0]["SOLD_TO"].ToString();
                drpDistributor.Value = dt.Rows[0]["SOLD_FROM"].ToString();
                txtAmount.Text = dt.Rows[0]["TOTAL_AMOUNT"].ToString();
                txtGstAmount.Text = dt.Rows[0]["GST_AMOUNT"].ToString();
                DrpTransferFor.Text = dt.Rows[0]["DISCOUNT"].ToString();
                txtNetAmount.Text = dt.Rows[0]["NET_AMOUNT"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            else
            {
                DrpTransferFor.Value = dt.Rows[0]["SOLD_FROM"].ToString();
                drpDistributor.Value = dt.Rows[0]["SOLD_TO"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            _purchaseSkus = _mPurchaseCtrl.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
        }
    }

    private void LoadGird()
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

        if (_purchaseSkus != null)
        {
            GrdPurchase.DataSource = _purchaseSkus;
            GrdPurchase.DataBind();
            decimal totalValue = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["Quantity"].ToString()));
            decimal totalAmount = _purchaseSkus.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));
            txtTotalQuantity.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalValue);
            txtTotalAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalAmount);
            txtNetAmount.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalAmount + Convert.ToDecimal(_dc.chkNull_0(txtGstAmount.Text)) - Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text)));
        }
    }

    private void LoadSkuDetail()
    {
        ddlSkus.Items.Clear();
        int mVendor_ID = int.Parse(drpPrincipal.SelectedItem.Value.ToString());
        DataTable dt = _skuHierarchyController.SelectDropdownCategoryItems(mVendor_ID, null, true, 4);
        clsWebFormUtil.FillDxComboBoxList(ddlSkus, dt, "SKU_ID", "SKU_NAME", false);
        if (dt.Rows.Count > 0)
        {
            ddlSkus.SelectedIndex = 0;
        }
        else
        {
            ddlSkus.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
        }
        Session.Add("Dtsku_Price", dt);
        #region Code
        //    hfInventoryType.Value = "0";
        //    if (drpPrincipal.Items.Count > 0)
        //    {
        //        DataTable dtskuPrice = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 23, int.Parse(Session["CompanyId"].ToString())
        //            , Convert.ToInt32(DrpDocumentType.Value));
        //        //DataTable dtskuPrice2 = _pController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(drpDistributor.Value), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 5, Convert.ToDateTime(Session["CurrentWorkDate"]));
        //        clsWebFormUtil.FillDxComboBoxList(ddlSkus, dtskuPrice, "SKU_ID", "SKU_NAME", true);
        //        //if (dtskuPrice.Rows.Count > 0)
        //        //{
        //        //    ddlSkus.SelectedIndex = 0;
        //        //    if (dtskuPrice.Rows[0]["IsInventoryWeight"].ToString() != "")
        //        //    {
        //        //        if (Convert.ToBoolean(dtskuPrice.Rows[0]["IsInventoryWeight"]))
        //        //        {
        //        //            hfInventoryType.Value = "1";
        //        //        }
        //        //    }
        //        //}
        //        Session.Add("Dtsku_Price", dtskuPrice);
        //        //Session.Add("dtskuPrice2", dtskuPrice2);
        //    }
        #endregion
    }

    #endregion

    #region IndexChnage

    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblInvoice.Text = "INV/DC  No";
        divPrice.Visible = true;
        divPrice2.Visible = true;
        divAmount.Visible = true;
        divAmount2.Visible = true;
        divGrossAmount.Visible = true;
        divGSTAmount.Visible = true;
        divDiscount.Visible = true;
        divNetAmount.Visible = true;
        lblStock.Visible = false;
        lblLastPrice.Visible = false;
        if (DrpDocumentType.SelectedIndex == 0)
        {
            lblStock.Visible = true;
            lblLastPrice.Visible = true;
            lbltoLocation.Text = "<span class='fa fa-caret-right rgt_cart'></span>Supplier";
            lblfromLocation.Text = "Purchase For";
            drpDistributor.Enabled = true;
            drpPrincipal.Enabled = true;
            DrpTransferFor.Visible = false;
            Label4.Visible = false;

            txtPrice.Text = "";
            GetDocumentNo();
            drpPrincipal.Visible = true;
            lbltoLocation.Visible = true;
            ddlSkus_SelectedIndexChanged(null, null);
        }
        else if (DrpDocumentType.SelectedIndex == 1)
        {
            lblStock.Visible = true;
            lblLastPrice.Visible = true;
            lblfromLocation.Text = "Transfer From";
            drpDistributor.Enabled = true;
            drpPrincipal.Visible = false;
            lbltoLocation.Visible = false;
            LoadToDistributor();
            DrpTransferFor.Visible = true;
            lblInvoice.Text = "Driver Name";
            Label4.Visible = true;
            Label4.Text = "<span class='fa fa-caret-right rgt_cart'></span>Transfer To";
            txtPrice.Text = "";
            divPrice.Visible = false;
            divPrice2.Visible = false;
            divAmount.Visible = false;
            divAmount2.Visible = false;
            divGrossAmount.Visible = false;
            divGSTAmount.Visible = false;
            divDiscount.Visible = false;
            divNetAmount.Visible = false;

            GetDocumentNo();
        }
        else if (DrpDocumentType.SelectedIndex == 2)
        {
            lbltoLocation.Text = "<span class='fa fa-caret-right rgt_cart'></span>Supplier";
            lblfromLocation.Text = "Return From";
            drpDistributor.Enabled = true;
            drpPrincipal.Enabled = true;
            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            drpPrincipal.Visible = true;
            lbltoLocation.Visible = true;
            txtPrice.Text = "";
            GetDocumentNo();
        }
        else if (DrpDocumentType.SelectedIndex == 4)
        {
            lblfromLocation.Text = "Location";
            drpDistributor.Enabled = true;
            drpPrincipal.Visible = false;
            lbltoLocation.Visible = false;
            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            txtPrice.Text = "";
            divAmount.Visible = false;
            divAmount2.Visible = false;
            divGrossAmount.Visible = false;
            divGSTAmount.Visible = false;
            divDiscount.Visible = false;
            divNetAmount.Visible = false;
            GetDocumentNo();
            ddlSkus_SelectedIndexChanged(null,null);
        }
        else
        {
            lblfromLocation.Text = "Location";
            drpDistributor.Enabled = true;
            drpPrincipal.Visible = false;
            lbltoLocation.Visible = false;
            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            txtPrice.Text = "";
            divPrice.Visible = false;
            divPrice2.Visible = false;
            divAmount.Visible = false;
            divAmount2.Visible = false;
            divGrossAmount.Visible = false;
            divGSTAmount.Visible = false;
            divDiscount.Visible = false;
            divNetAmount.Visible = false;

            GetDocumentNo();
        }
        DrpDocumentType.Focus();
        txtQuantity.Text = "";        
        txtDiscount.Text = string.Empty;
        txtGstAmount.Text = string.Empty;
        txtAmount.Text = "0";
        btnSave.Text = "Add";
        _privouseQty.Value = "0";
        txtNetAmount.Text = "0";
        txtDocumentNo.Text = "";
        txtBuiltyNo.Text = "";
    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
            ClearAll();
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            DrpDocumentType.Enabled = true;
        }
        else
        {
            txtBuiltyNo.Text = "";
            txtDocumentNo.Text = "";
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            DrpDocumentType.Enabled = false;
            LoadDocumentDetail();
            LoadSkuDetail();
        }
        drpDocumentNo.Focus();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkuDetail();
        drpDistributor.Focus();
    }

    protected void ddlSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPrice.Text = "";
        txtQuantity.Enabled = true;
        lblStock.Text = "Stock: 0";
        lblLastPrice.Text = "Last Purchase Price: 0";
        #region Start
        try
        {
            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length > 0)
            {
                #region UOM_DESC
                //txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
                //if (foundRows[0]["IsInventoryWeight"].ToString() != "")
                //{
                //    if (Convert.ToBoolean(foundRows[0]["IsInventoryWeight"]))
                //    {
                //        hfInventoryType.Value = "1";
                //    }
                //    else
                //    {
                //        txtQuantity.Text = string.Empty;
                //        hfInventoryType.Value = "0";
                //    }
                //}
                //else
                //{
                //    txtQuantity.Text = string.Empty;
                //}
                #endregion

                if (ddlSkus.Items.Count > 0 && drpDistributor.Items.Count > 0)
                {
                    DataSet dsClosing = SKUCtl.GetSKUClosingStockLastPrice(Convert.ToInt32(ddlSkus.Value), Convert.ToInt32(drpDistributor.Value));
                    if (dsClosing.Tables[0].Rows.Count > 0)
                    {
                        lblStock.Text = "Stock: " + String.Format("{0:0.00}", dsClosing.Tables[0].Rows[0]["CLOSING_STOCK"]);
                    }
                    if (dsClosing.Tables[1].Rows.Count > 0)
                    {
                        lblLastPrice.Text = "Last Purchase Price: " + String.Format("{0:0.00}", dsClosing.Tables[1].Rows[0]["PRICE"]);
                    }
                }
                if (DrpDocumentType.SelectedItem.Value.ToString() == "20")
                {
                    txtPrice.Text = String.Format("{0:0.00}", foundRows[0]["ProductionInPrice"]);
                }
            }
            txtQuantity.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
        #endregion
    }
    #endregion

    #region Grid Operations

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _rowNo.Value = e.NewEditIndex.ToString();
        ddlSkus.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        _privouseQty.Value = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
        #region UOM_DESC
        //if (foundRows.Length > 0)
        //{
        //    txtUOM.Text = foundRows[0]["UOM_DESC"].ToString();
        //}
        #endregion
        ddlSkus.Enabled = false;
        txtQuantity.Focus();
        btnSave.Text = "Update";
    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus.Rows.Count > 0)
        {
            _purchaseSkus.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKUS", _purchaseSkus);
            LoadGird();
        }
    }

    #endregion

    private bool CheckDublicateSku()
    {
        try
        {
            _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

            DataRow[] foundRows = _purchaseSkus.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length == 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Click OPerations

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (drpDocumentNo.Value.ToString() == Constants.LongNullValue.ToString())
            {
                if (decimal.Parse(_dc.chkNull_0(txtQuantity.Text)) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Quantity.');", true);
                    txtQuantity.Focus();
                    return;
                }
                if (DrpDocumentType.Value.ToString() == "2" || DrpDocumentType.Value.ToString() == "3")
                {
                    if (decimal.Parse(_dc.chkNull_0(txtPrice.Text)) <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Price.');", true);
                        txtPrice.Focus();
                        return;
                    }
                }
            }

            DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            //DataTable dtskuPrice2 = (DataTable)Session["dtskuPrice2"];
            DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            //DataRow[] foundRows2 = dtskuPrice2.Select("SKU_ID  = '" + ddlSkus.SelectedItem.Value + "'");
            if (foundRows.Length >0)
            {
                _purchaseSkus = (DataTable)Session["PurchaseSKUS"];

                decimal currentStock = CheckStockStatus(int.Parse(_dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
                if (Convert.ToBoolean(Session["VALIDATE_STOCK"]) == false)
                {
                    currentStock = -1;
                }
                if (btnSave.Text == "Add")
                {
                    if (CheckDublicateSku())
                    {
                        if (currentStock == -1)
                        {
                            DataRow dr = _purchaseSkus.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                          
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            
                            dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            #region UOM_IDUOM_DESC
                            //dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                            //dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            //dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];
                            //dr["UOM_DESC"] = txtUOM.Text;
                            //if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                            //{
                            //    dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                            //}
                            //else
                            //{
                            //    dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            //}
                            #endregion
                            if (DrpDocumentType.SelectedItem.Value.ToString() == "2" || DrpDocumentType.SelectedItem.Value.ToString() == "3" || DrpDocumentType.SelectedItem.Value.ToString() == "20")
                            {
                                dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                                dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text)) * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            }
                            else
                            {
                                //if (foundRows2.Length > 0)
                                //{
                                //    dr["PRICE"] = foundRows2[0]["DISTRIBUTOR_PRICE"];
                                //    dr["AMOUNT"] = decimal.Parse(foundRows2[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                                //}
                                //else
                                //{
                                    dr["PRICE"] = 0;
                                    dr["AMOUNT"] = 0;
                                //}
                            }
                            _purchaseSkus.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Current closing Stock is " + currentStock + "');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + " Already Exists ');", true);
                        return;
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    if (currentStock == -1)
                    {
                        DataRow dr = _purchaseSkus.Rows[Convert.ToInt32(_rowNo.Value)];
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        //dr["UOM_ID"] = foundRows[0]["UOM_ID"];
                        dr["Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        //dr["UOM_DESC"] = txtUOM.Text;
                        //dr["S_UOM_ID"] = foundRows[0]["S_UOM_ID"];

                        if (decimal.Parse(_dc.chkNull_0(foundRows[0]["UOM_ID"].ToString())) != decimal.Parse(_dc.chkNull_0(foundRows[0]["S_UOM_ID"].ToString())))
                        {
                            dr["S_Quantity"] = DataControl.QuantityConversion(Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["DEFAULT_QTY"].ToString())), foundRows[0]["PS_OPERATOR"].ToString(), Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["PS_FACTOR"].ToString())), decimal.Parse(_dc.chkNull_0(txtQuantity.Text)), Constants.DecimalNullValue, "");
                        }
                        else
                        {
                            dr["S_Quantity"] = decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }

                        if (DrpDocumentType.SelectedItem.Value.ToString() == "2" || DrpDocumentType.SelectedItem.Value.ToString() == "20")
                        {
                            dr["PRICE"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text));
                            dr["AMOUNT"] = decimal.Parse(_dc.chkNull_0(txtPrice.Text)) * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                        }
                        else
                        {
                            //if (foundRows2.Length > 0)
                            //{
                            //    dr["PRICE"] = foundRows2[0]["DISTRIBUTOR_PRICE"];
                            //    dr["AMOUNT"] = decimal.Parse(foundRows2[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));
                            //}
                            //else
                            //{
                                dr["PRICE"] = 0;
                                dr["AMOUNT"] = 0;
                            //}
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSkus.SelectedItem.Text + "Current closing Stock is " + currentStock.ToString() + "');", true);
                        return;
                    }
                }
                Session.Add("PurchaseSKUS", _purchaseSkus);
                ClearAll();
                LoadGird();
                DisAbaleOption(true);
                ScriptManager.GetCurrent(Page).SetFocus(ddlSkus);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Item Select');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedIndex == 1)
        {
            if (drpDistributor.SelectedItem.Value.ToString() == DrpTransferFor.SelectedItem.Value.ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Transfer to Location must be different ');", true);
                return;
            }
        }
        _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
        if (_purchaseSkus.Rows.Count > 0)
        {
            var mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, Convert.ToInt32(drpDistributor.SelectedItem.Value));
            if (dt.Rows.Count > 0)
            {
                if (CalculatePurchase(DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString())))
                {
                    _purchaseSkus = (DataTable)Session["PurchaseSKUS"];
                    _purchaseSkus.Rows.Clear();
                    Session.Add("PurchaseSKUS", _purchaseSkus);
                    ClearAll();
                    txtBuiltyNo.Text = "";
                    txtDocumentNo.Text = "";
                    txtTotalQuantity.Text = "";
                    txtDiscount.Text = string.Empty;
                    txtGstAmount.Text = string.Empty;
                    LoadGird();
                    GetDocumentNo();
                    drpDistributor.Enabled = true;
                    drpPrincipal.Enabled = true;
                    DrpDocumentType.Enabled = true;
                    DisAbaleOption(false);
                    this.LoadSkuDetail();
                    ddlSkus_SelectedIndexChanged(null, null);
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save ');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' At least one Item must enter');", true);

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisAbaleOption(false);
        CreatTable();
        LoadGird();
        ClearAll();
        txtDocumentNo.Text = "";
        txtBuiltyNo.Text = "";
        txtDocumentNo.Text = "";
    }

    #endregion

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
    
    private bool CalculatePurchase(DateTime mWorkDate)
    {
        var dtPurchaseDetail = (DataTable)Session["PurchaseSKUS"];

        decimal mTotalAmount = dtPurchaseDetail.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(dr["AMOUNT"].ToString()));
        decimal mNetAmount = mTotalAmount + Convert.ToDecimal(_dc.chkNull_0(txtGstAmount.Text)) - Convert.ToDecimal(_dc.chkNull_0(txtDiscount.Text));
        bool mResult = false;
        long PurchaseID = Constants.LongNullValue;

        DataTable dtConfig = GetCOAConfiguration();
        bool IsFinanceSetting = GetFinanceConfig();
        if (DrpDocumentType.SelectedIndex == 0) //Purchase
        {
            if (drpDocumentNo.Value.ToString() == Constants.LongNullValue.ToString())
            {
                PurchaseID = _mPurchaseCtrl.InsertPurchase(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , decimal.Parse(_dc.chkNull_0(txtGstAmount.Text)),
               decimal.Parse(_dc.chkNull_0(txtDiscount.Text)), mNetAmount,drpPrincipal.SelectedItem.Text,
               dtConfig, IsFinanceSetting, int.Parse(DrpVoucher.SelectedItem.Value.ToString()));
                //ShowReportPopUp(0);
                if (PurchaseID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                mResult = _mPurchaseCtrl.UpdatePurchase(long.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , decimal.Parse(_dc.chkNull_0(txtGstAmount.Text)), decimal.Parse(_dc.chkNull_0(txtDiscount.Text)),
               mNetAmount,drpPrincipal.SelectedItem.Text, dtConfig, IsFinanceSetting, int.Parse(DrpVoucher.SelectedItem.Value.ToString()));
                ShowReportPopUp(1);
                if (mResult)
                {
                }
                return mResult;
            }
        }
        else if (DrpDocumentType.SelectedIndex == 1)    // Transfer Out
        {
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                mResult = _mPurchaseCtrl.InsertPurchaseDocument2(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                , mWorkDate, int.Parse(DrpTransferFor.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);
                ShowTransferOutPopUp(0);
            }
            else
            {
                mResult = _mPurchaseCtrl.UpdatePurchaseDocument2(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                , mWorkDate, int.Parse(DrpTransferFor.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);
                ShowTransferOutPopUp(1);
            }

            return mResult;
        }
        else if (DrpDocumentType.SelectedIndex == 2)    //Purchase Return
        {
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                PurchaseID = _mPurchaseCtrl.InsertPurchaseNew(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
                , mWorkDate, int.Parse(drpPrincipal.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
                , decimal.Parse(_dc.chkNull_0(txtGstAmount.Text)), decimal.Parse(_dc.chkNull_0(txtDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)),drpPrincipal.SelectedItem.Text, dtConfig, IsFinanceSetting);
                ShowReportReturnPopUp(0);
                if (PurchaseID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                mResult = _mPurchaseCtrl.UpdatePurchaseNew(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpPrincipal.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString())
               , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , decimal.Parse(_dc.chkNull_0(txtGstAmount.Text)), decimal.Parse(_dc.chkNull_0(txtDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)),drpPrincipal.SelectedItem.Text, dtConfig, IsFinanceSetting);
                ShowReportReturnPopUp(1);
                if (mResult)
                {
                }
                return mResult;
            }
        }
        else if (DrpDocumentType.SelectedIndex == 3)           // Damage
        {
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                mResult = _mPurchaseCtrl.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()),dtConfig, IsFinanceSetting);
                ShowDamagePopUp(0);
                return mResult;
            }
            else
            {
                mResult = _mPurchaseCtrl.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()),dtConfig, IsFinanceSetting);
                ShowDamagePopUp(1);
                return mResult;
            }
        }
        else// Production In
        {
            if (drpDocumentNo.SelectedItem.Value.ToString() == Constants.LongNullValue.ToString())
            {
                mResult = _mPurchaseCtrl.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);
                ShowProductionPopUp(0);
                return mResult;
            }
            else
            {
                mResult = _mPurchaseCtrl.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedItem.Value.ToString())
               , mWorkDate, int.Parse(drpDistributor.SelectedItem.Value.ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString())
               , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedItem.Value.ToString()), dtConfig, IsFinanceSetting);
                ShowProductionPopUp (1);
                return mResult;
            }
        }
    }

    private decimal CheckStockStatus(int skuId)
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
            if (DrpDocumentType.SelectedIndex == 0 || DrpDocumentType.SelectedItem.Value.ToString() == "20")
            {
                return -1;
            }
            else
            {
                var mController = new PhaysicalStockController();
                DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.SelectedItem.Value.ToString()), skuId, "N/A", CurrentWorkDate, 15);
                if (dt.Rows.Count > 0)
                {
                    if (decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value) >= decimal.Parse(txtQuantity.Text))
                    {
                        return -1;
                    }
                    else
                    {
                        return decimal.Parse(dt.Rows[0][0].ToString()) + Convert.ToDecimal(_privouseQty.Value);
                    }
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
            return 0;
        }
        return 0;
    }

    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            DrpDocumentType.Enabled = false;
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
        }
        else
        {
            DrpDocumentType.Enabled = true;
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    private void ClearAll()
    {

        txtQuantity.Text = "";
        ddlSkus.Enabled = true;
        txtPrice.Text = "";

        txtAmount.Text = "0";
        btnSave.Text = "Add";
        _privouseQty.Value = "0";
        txtNetAmount.Text = "0";
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
    
    public void ShowReportPopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        CORNBusinessLayer.Reports.CrpPurchaseDocument CrpReport = new CORNBusinessLayer.Reports.CrpPurchaseDocument();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectPurchaseDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 2);
        }
        else
        {
            ds = RptInventoryCtl.SelectPurchaseDocumentPopUp(Constants.IntNullValue, 1);
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Purchase Document");
        //CrpReport.SetParameterValue("Principal", drpPrincipal.SelectedItem.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    public void ShowReportReturnPopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        CORNBusinessLayer.Reports.CrpPurchaseDocument CrpReport = new CORNBusinessLayer.Reports.CrpPurchaseDocument();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectPurchaseDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 2);
        }
        else
        {
            ds = RptInventoryCtl.SelectPurchaseDocumentPopUp(Constants.IntNullValue, 1);
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Purchase Return Document");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    public void ShowTransferOutPopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrpTransferDocument CrpReport = new CrpTransferDocument();
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 3);
        }
        else
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(Constants.IntNullValue, 4);
        }
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("DocumentType", "Transfer Out Document");
        CrpReport.SetParameterValue("Principal", "");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("user", this.Session["UserName"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    public void ShowDamagePopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrpDamageDocument CrpReport = new CrpDamageDocument();
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 5);
        }
        else
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(Constants.IntNullValue, 6);
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Damage Document");
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

    public void ShowProductionPopUp(int type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        CrpDamageDocument CrpReport = new CrpDamageDocument();
        DataSet ds = null;
        if (type != 0)
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 5);
        }
        else
        {
            ds = RptInventoryCtl.SelectTransferDocumentPopUp(Constants.IntNullValue, 6);
        }

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", "Production In Document");
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

   
}