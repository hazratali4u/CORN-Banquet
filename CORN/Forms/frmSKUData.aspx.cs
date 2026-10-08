using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// From To Add, Edit, Delete SKU
/// used PACK_SIZE for UOM
/// used BRAND_ID for CAEGORY_TYPE
/// used UNITS_IN_CASE=1 for ACTIVE, IN_ACTIVE FUNCTION 
/// used DIVISION_ID FOR PRODUCT_TYPE
/// </summary>

public partial class Forms_frmSKUData : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuController _mSkuController = new SkuController();

    readonly DataControl dc = new DataControl();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly GeoHierarchyController DptTpe = new GeoHierarchyController();

    #region Load

    List<string> Operators = new List<string>() { "+", "-", "/", "*", "%" };

    private void LoadOperators()
    {
        drpSaletoPurchaseOperator.Items.Clear();
        drpPurchaseToSaleOperator.Items.Clear();
        drpSaleToStockOperator.Items.Clear();
        drpPurchaseToStockOperator.Items.Clear();
        drpStockToSaleOperator.Items.Clear();
        drpStockToPurchaseOperator.Items.Clear();

        drpSaletoPurchaseOperator.DataSource = Operators;
        drpSaletoPurchaseOperator.DataBind();

        drpPurchaseToSaleOperator.DataSource = Operators;
        drpPurchaseToSaleOperator.DataBind();

        drpSaleToStockOperator.DataSource = Operators;
        drpSaleToStockOperator.DataBind();

        drpPurchaseToStockOperator.DataSource = Operators;
        drpPurchaseToStockOperator.DataBind();

        drpStockToSaleOperator.DataSource = Operators;
        drpStockToSaleOperator.DataBind();

        drpStockToPurchaseOperator.DataSource = Operators;
        drpStockToPurchaseOperator.DataBind();

        drpSaletoPurchaseOperator.SelectedIndex = 3;
        drpPurchaseToSaleOperator.SelectedIndex = 3;
        drpSaleToStockOperator.SelectedIndex = 3;
        drpPurchaseToStockOperator.SelectedIndex = 3;
        drpStockToSaleOperator.SelectedIndex = 3;
        drpStockToPurchaseOperator.SelectedIndex = 3;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            ceModifier.EnableCustomColors = true;
            Session.Remove("OldSKUName");
            Session.Remove("OldERPCode");
            GetConfiguration();
            LoadCategoryType();
            LoadCategory();
            LoadGrid("");
            LoadUOM();
            LoadSection();
            LoadCheckBoxCategory();
            LoadCheckBoxVendors();
            if (Session["Division"] != null)
            {
                if (Session["Division"].ToString() == "0")
                {
                    trItemType.Visible = true;
                }
            }
            LoadOperators();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            //ddlType_SelectedIndexChanged(null, null);
        }

    }
    private void LoadCategoryType()
    {
        DataTable dt = _mSkuController.SelectCategoryType();
        

    }
    private void GetConfiguration()
    {
        try
        {
            ConfigurationController _cController = new ConfigurationController();
            DataTable dt;
            dt = _cController.SelectAppConfiguration(1, (int)Enums.AppSettingMaster.InventorySetting, null, Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("CODE = '" + (int)Enums.AppSetting.NegativeAllowed + "'");
                _OversaleAllowed.Value = dr[0]["VALUE"].ToString();
                dr = dt.Select("CODE = '" + (int)Enums.AppSetting.ExpiryAllowed + "'");
                _ExpiryAllowed.Value = dr[0]["VALUE"].ToString();
            }
            if (Convert.ToBoolean(Convert.ToInt32(_OversaleAllowed.Value)))
            {
                chkIsOverSaleAllowed.Enabled = true;
            }
            else
            {
                chkIsOverSaleAllowed.Enabled = false;
            }
            if (Convert.ToBoolean(Convert.ToInt32(_ExpiryAllowed.Value)))
            {
                chkIsExpiryAllowed.Enabled = true;
            }
            else
            {
                chkIsExpiryAllowed.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            throw;
        }
    }
    private void LoadSection()
    {
        DataTable dt = _mSkuController.SelectProductSection(0, null, null);
    }

    
    private void LoadCheckBoxCategory()
    {
        DataTable _mDt = _mHerController.CheckBoxCategory(Constants.IntNullValue , null);
        clsWebFormUtil.FillDxListBox(ChCategoryList, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
    }

    private void LoadCheckBoxVendors()
    {
        DataTable _mDt = _mHerController.CheckBoxVendor(Constants.IntNullValue, null);
        clsWebFormUtil.FillDxListBox(ChVendorList, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
    }

    private void LoadCategory()
    {
        DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, 15, Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, _mDt, 0, 3, true);
        if (_mDt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
        }
    }

    private void LoadAllItems()
    {

        DataTable _mSkuDt = _mSkuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);

        DataView dv = _mSkuDt.DefaultView;
        dv.Sort = "Category asc";
        DataTable sortedDt = dv.ToTable();

        grdSKUData.DataSource = sortedDt;
        grdSKUData.DataBind();
    }

    private void LoadGrid(string pType)
    {

        grdSKUData.DataSource = null;
        grdSKUData.DataBind();

        DataTable dt = new DataTable();
        dt = _mSkuController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), null);

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)//In case after  Filter
            {
                dt.DefaultView.RowFilter = "Category LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR SECTION_NAME LIKE '%" + txtSearch.Text + "%' OR PACKSIZE LIKE '%" + txtSearch.Text + "%' OR ISACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            grdSKUData.DataSource = dt;
            grdSKUData.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "Category LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR SECTION_NAME LIKE '%" + txtSearch.Text + "%' OR PACKSIZE LIKE '%" + txtSearch.Text + "%' OR ISACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                grdSKUData.PageIndex = 0;
            }
            grdSKUData.DataSource = dt;
            grdSKUData.DataBind();
        }
    }
    private void LoadUOM()
    {
        DataTable dt = DptTpe.GetUOM(0, Constants.IntNullValue, Constants.IntNullValue);

        
        clsWebFormUtil.FillDxComboBoxList(drpSaleUnit, dt, 0, 1, true);
        clsWebFormUtil.FillDxComboBoxList(drpPurchaseUnit, dt, 0, 1, true);
        clsWebFormUtil.FillDxComboBoxList(drpStockUnit, dt, 0, 1, true);
        clsWebFormUtil.FillDxComboBoxList(drpUnitLifeCode, dt, 0, 1, true);

        if (dt.Rows.Count > 0)
        {
            drpSaleUnit.SelectedIndex = 0;
            drpPurchaseUnit.SelectedIndex = 0;
            drpStockUnit.SelectedIndex = 0;
            drpUnitLifeCode.SelectedIndex = 0;
        }
    }

    #endregion

    #region Grid


    protected void grdSKUData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        cbKOT.Checked = false;
        TabContainer1.ActiveTabIndex = 0;
        btnSave.Text = "Update";

        try
        {
            if (Convert.ToInt32(grdSKUData.Rows[e.NewEditIndex].Cells[62].Text) != 0)
            {
                ToggleActivationControles(false);
            }
            if (Convert.ToInt32(grdSKUData.Rows[e.NewEditIndex].Cells[63].Text) != 0)
            {
                chkIsRecipe.Enabled = false;
            }
            //ddlType.Value = grdSKUData.Rows[e.NewEditIndex].Cells[4].Text;
            //ddlType_SelectedIndexChanged(null, null);

            //hfSkuId.Value = grdSKUData.Rows[e.NewEditIndex].Cells[5].Text;
            txtskucode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");

            StringWriter myWriter = new StringWriter();
            HttpUtility.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", ""), myWriter);            
            txtskuname.Text = myWriter.ToString();
            Session.Add("OldSKUName", txtskuname.Text);
            txtunitincase.Text = grdSKUData.Rows[e.NewEditIndex].Cells[14].Text.Replace("&nbsp;", "");

            txtDescription.Text = string.Empty;
            if (grdSKUData.Rows[e.NewEditIndex].Cells[12].Text != "")
            {
                txtDescription.Text = grdSKUData.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
            }
            try
            {
                if (int.Parse(grdSKUData.Rows[e.NewEditIndex].Cells[16].Text) > 0)
                {
                    //ddlSection.Value = grdSKUData.Rows[e.NewEditIndex].Cells[16].Text;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Assigned Section is inactive');", true);
            }
            chkIsDesc.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[17].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[17].Text);
            chkIsInventory.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[18].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[18].Text);
            chkIsDeal.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[20].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[20].Text);
            chkIsModifier.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[21].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[21].Text);            
            hfStatus.Value = grdSKUData.Rows[e.NewEditIndex].Cells[19].Text.Replace("&nbsp;", "");
            txtMinLevel.Text = grdSKUData.Rows[e.NewEditIndex].Cells[22].Text.Replace("&nbsp;", "");
            txtReorderLevel.Text = grdSKUData.Rows[e.NewEditIndex].Cells[23].Text.Replace("&nbsp;", "");
            chkIsRecipe.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[24].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[24].Text);
            chkIsHasModifier.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[25].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[25].Text);
            txtMaxLevel.Text = grdSKUData.Rows[e.NewEditIndex].Cells[26].Text.Replace("&nbsp;", "");
            drpSaleUnit.Value = grdSKUData.Rows[e.NewEditIndex].Cells[27].Text.Replace("&nbsp;", "");
            drpSaletoPurchaseOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[28].Text.Replace("&nbsp;", "");
            txtSaleToPurchaseFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[29].Text.Replace("&nbsp;", "");
            drpPurchaseToSaleOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[30].Text.Replace("&nbsp;", "");
            txtPurchaseToSaleFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[31].Text.Replace("&nbsp;", "");
            drpPurchaseUnit.Value = grdSKUData.Rows[e.NewEditIndex].Cells[32].Text.Replace("&nbsp;", "");
            drpSaleToStockOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[33].Text.Replace("&nbsp;", "");
            txtSaleToStockFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[34].Text.Replace("&nbsp;", "");
            txtPurchaseToStockFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[35].Text.Replace("&nbsp;", "");
            drpStockUnit.Value = grdSKUData.Rows[e.NewEditIndex].Cells[36].Text.Replace("&nbsp;", "");
            drpStockToSaleOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[38].Text.Replace("&nbsp;", "");
            txtStockToSaleFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[39].Text.Replace("&nbsp;", "");
            drpStockToPurchaseOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[40].Text.Replace("&nbsp;", "");
            txtStockToPurchaseFactor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[41].Text.Replace("&nbsp;", "");
            txtDescription2.Text = grdSKUData.Rows[e.NewEditIndex].Cells[42].Text.Replace("&nbsp;", "");
            chkIsSerialized.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[43].Text);
            txtSerialCode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[44].Text.Replace("&nbsp;", "");
            chkIsHazardous.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[45].Text);
            drpStatus.Value = grdSKUData.Rows[e.NewEditIndex].Cells[46].Text.Replace("&nbsp;", "");
            chkIsBatchItem.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[47].Text);
            txtERPCode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[48].Text.Replace("&nbsp;", "");
            Session.Add("OldERPCode", txtERPCode.Text);
            chkIsOverSaleAllowed.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[49].Text);
            chkIsExpiryAllowed.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[50].Text);
            chkIsWarehouseItem.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[51].Text);
            chkIsMarketItem.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[52].Text);
            chkIsReplaceable.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[53].Text);
            chkIsFEDItem.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[54].Text);
            txtFEDPercentage.Text = grdSKUData.Rows[e.NewEditIndex].Cells[55].Text.Replace("&nbsp;", "");
            chkIsWHTItem.Checked = Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[56].Text);
            txtWHTPercentage.Text = grdSKUData.Rows[e.NewEditIndex].Cells[57].Text.Replace("&nbsp;", "");
            txtAgeInDays.Text = grdSKUData.Rows[e.NewEditIndex].Cells[58].Text.Replace("&nbsp;", "");
            txtShelfAgeInDays.Text = grdSKUData.Rows[e.NewEditIndex].Cells[59].Text.Replace("&nbsp;", "");
            drpUnitLifeCode.Value = grdSKUData.Rows[e.NewEditIndex].Cells[60].Text.Replace("&nbsp;", "");
            drpPurchaseToStockOperator.Value = grdSKUData.Rows[e.NewEditIndex].Cells[61].Text.Replace("&nbsp;", "");
            chkIsGroup.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[68].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[68].Text);
            chIsPackage.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[69].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[69].Text);
            hidSKUImageName.Value = (grdSKUData.Rows[e.NewEditIndex].FindControl("hidSKUImageName") as HiddenField).Value;

            if (skuImageUploadArea.Visible)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
            }
            mPopUpLocation.Show();
            try
            {
                ddlCategory.Value = grdSKUData.Rows[e.NewEditIndex].Cells[3].Text;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Assigned category is inactive');", true);
            }
          

            chkIsInventoryWeight.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[64].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[64].Text);
            chkIsSaleWeight.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[67].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[67].Text);
            cbKOT.Checked = (grdSKUData.Rows[e.NewEditIndex].Cells[65].Text == "&nbsp;") ? false : Convert.ToBoolean(grdSKUData.Rows[e.NewEditIndex].Cells[65].Text);
            divColorPicker.Attributes.Add("style", "visibility:hidden");
            if (chkIsModifier.Checked)
            {
                ceModifier.Text = grdSKUData.Rows[e.NewEditIndex].Cells[66].Text.Replace("&nbsp;", "");
                divColorPicker.Attributes.Add("style", "visibility:visible");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    #endregion

    public bool Validation()
    {
        if (hfStockSale.Value == "1" && txtStockToSaleFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg1", "Message('Stock to sale factor is required');", true);
            return false;
        }
        if (hfStockPurchase.Value == "1" && txtStockToPurchaseFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg2", "Message('Stock to purchase factor is required');", true);
            return false;
        }
        if (hfSalePurchase.Value == "1" && txtSaleToPurchaseFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "Message('Sale to purchase factor is required');", true);
            return false;
        }
        if (hfSaleStock.Value == "1" && txtSaleToStockFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg4", "Message('Sale to stock factor is required');", true);
            return false;
        }
        if (hfPurchaseSale.Value == "1" && txtPurchaseToSaleFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg5", "Message('Purchase to sale factor is required');", true);
            return false;
        }
        if (hfPurchaseStock.Value == "1" && txtPurchaseToStockFactor.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg6", "Message('Purchase to stock factor is required');", true);
            return false;
        }
        if (chkIsSerialized.Checked == true && txtSerialCode.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg7", "Message('Serial code is required');", true);
            return false;
        }
        if (chkIsWHTItem.Checked == true && txtWHTPercentage.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg8", "Message('WHT percentage is required');", true);
            return false;
        }
        if (chkIsFEDItem.Checked == true && txtFEDPercentage.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg9", "Message('FED percentage is required');", true);
            return false;
        }
        return true;
    }

    #region Click 
    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        if (!Validation())
        {
            return;
        }
        int sectionId = 0;

        try
        {
            //if (ddlSection.Items.Count > 0)
            //{
            //    sectionId = int.Parse(ddlSection.SelectedItem.Value.ToString());
            //}
            string ButtonColor = null;
            divColorPicker.Attributes.Add("style", "visibility:hidden");
            if (chkIsModifier.Checked)
            {
                divColorPicker.Attributes.Add("style", "visibility:visible");
                ButtonColor = ceModifier.Text;
            }
            if (btnSave.Text == "Save")
            {
                if (NameIsDuplicate())
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('This name already exist for this Category Type and Category.');", true);
                    return;
                }
                if (txtERPCode.Text.Trim().Length > 0)
                {
                    if (ERPCodeIsDuplicate())
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('This ERPCode " + txtERPCode.Text + " already exist.');", true);
                        return;
                    }
                }
                UploadImage();
                _mSkuController.InsertSKUS(chkIsDesc.Checked, chkIsInventory.Checked, true, char.Parse("E"), 1, Convert.ToInt32(drpProductType.SelectedItem.Value), Convert.ToInt32(ddlCategory.SelectedItem.Value),
                  Constants.IntNullValue, 1, 0, 0, short.Parse(dc.chkNull_0(txtunitincase.Text)),
                  txtskucode.Text.Trim().ToUpper(), txtskuname.Text.Trim(), null,null, int.Parse(Session["UserId"].ToString())
                  , int.Parse(Session["CompanyId"].ToString()), txtDescription.Text.Trim(), sectionId, chkIsDeal.Checked, chkIsModifier.Checked
                  , decimal.Parse(dc.chkNull_0(txtMinLevel.Text)), decimal.Parse(dc.chkNull_0(txtReorderLevel.Text)), chkIsRecipe.Checked,
                   chkIsHasModifier.Checked, Convert.ToInt32(drpSaleUnit.SelectedItem.Value), Convert.ToInt32(drpPurchaseUnit.SelectedItem.Value), Convert.ToInt32(drpStockUnit.SelectedItem.Value),
                   float.Parse(dc.chkNull_0(txtFEDPercentage.Text)), float.Parse(dc.chkNull_0(txtWHTPercentage.Text)), float.Parse(dc.chkNull_0(txtAgeInDays.Text)), chkIsMarketItem.Checked,
                   chkIsReplaceable.Checked, chkIsFEDItem.Checked, chkIsWHTItem.Checked, chkIsSerialized.Checked,
                   chkIsHazardous.Checked, chkIsBatchItem.Checked, chkIsOverSaleAllowed.Checked, chkIsExpiryAllowed.Checked, chkIsWarehouseItem.Checked,
                   decimal.Parse(dc.chkNull_0(txtMaxLevel.Text)), decimal.Parse(dc.chkNull_0(txtSaleToPurchaseFactor.Text)), decimal.Parse(dc.chkNull_0(txtPurchaseToSaleFactor.Text)), decimal.Parse(dc.chkNull_0(txtSaleToStockFactor.Text)),
                   decimal.Parse(dc.chkNull_0(txtPurchaseToStockFactor.Text)),Constants.DecimalNullValue, decimal.Parse(dc.chkNull_0(txtStockToSaleFactor.Text)), decimal.Parse(dc.chkNull_0(txtStockToPurchaseFactor.Text)),
                   drpSaletoPurchaseOperator.SelectedItem.Text, drpPurchaseToSaleOperator.SelectedItem.Text, drpSaleToStockOperator.SelectedItem.Text,
                   drpPurchaseToStockOperator.SelectedItem.Text, drpStockToSaleOperator.SelectedItem.Text,
                   drpStockToPurchaseOperator.SelectedItem.Text, txtDescription2.Text, txtSerialCode.Text, drpStatus.Value.ToString(), txtERPCode.Text, float.Parse(dc.chkNull_0(txtShelfAgeInDays.Text)), Convert.ToInt32(drpUnitLifeCode.SelectedItem.Value)
                   , chkIsInventoryWeight.Checked, cbKOT.Checked, ButtonColor, hidSKUImageName.Value,chkIsSaleWeight.Checked,chkIsGroup.Checked,chIsPackage.Checked);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                mPopUpLocation.Show();
            }
            else if (btnSave.Text == "Update")
            {
                if (txtskuname.Text.Trim() != Session["OldSKUName"].ToString())
                {
                    if (NameIsDuplicate())
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('This name already exist for this Category Type and Category.');", true);
                        return;
                    }
                }

                if (txtERPCode.Text.Trim() != Session["OldERPCode"].ToString())
                {
                    if (ERPCodeIsDuplicate())
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('This ERPCode " + txtERPCode.Text + " already exist.');", true);
                        return;
                    }
                }
                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }
                UploadImage();
                _mSkuController.UpdateSKUS(chkIsDesc.Checked, chkIsInventory.Checked, status, char.Parse("E"), 1, Convert.ToInt32(drpProductType.SelectedItem.Value),
                int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue, 1, 0, 0, short.Parse(dc.chkNull_0(txtunitincase.Text))
                , Constants.IntNullValue, txtskucode.Text.Trim().ToUpper(), txtskuname.Text.Trim(), null, null
                , int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()), txtDescription.Text.Trim(), sectionId
                , chkIsDeal.Checked, chkIsModifier.Checked, DataControl.chkDecimalNull(txtMinLevel.Text), DataControl.chkDecimalNull(txtReorderLevel.Text), chkIsRecipe.Checked,
                 chkIsHasModifier.Checked, Convert.ToInt32(drpSaleUnit.SelectedItem.Value), Convert.ToInt32(drpPurchaseUnit.SelectedItem.Value), Convert.ToInt32(drpStockUnit.SelectedItem.Value),
                 float.Parse(dc.chkNull_0(txtFEDPercentage.Text)), float.Parse(dc.chkNull_0(txtWHTPercentage.Text)), float.Parse(dc.chkNull_0(txtAgeInDays.Text)), chkIsMarketItem.Checked,
                 chkIsReplaceable.Checked, chkIsFEDItem.Checked, chkIsWHTItem.Checked, chkIsSerialized.Checked,
                 chkIsHazardous.Checked, chkIsBatchItem.Checked, chkIsOverSaleAllowed.Checked, chkIsExpiryAllowed.Checked, chkIsWarehouseItem.Checked,
                 decimal.Parse(dc.chkNull_0(txtMaxLevel.Text)), decimal.Parse(dc.chkNull_0(txtSaleToPurchaseFactor.Text)), decimal.Parse(dc.chkNull_0(txtPurchaseToSaleFactor.Text)), decimal.Parse(dc.chkNull_0(txtSaleToStockFactor.Text)),
                 decimal.Parse(dc.chkNull_0(txtPurchaseToStockFactor.Text)), Constants.DecimalNullValue, decimal.Parse(dc.chkNull_0(txtStockToSaleFactor.Text)), decimal.Parse(dc.chkNull_0(txtStockToPurchaseFactor.Text)),
                 drpSaletoPurchaseOperator.SelectedItem.Text, drpPurchaseToSaleOperator.SelectedItem.Text, drpSaleToStockOperator.SelectedItem.Text,
                 drpPurchaseToStockOperator.SelectedItem.Text, drpStockToSaleOperator.SelectedItem.Text, drpStockToPurchaseOperator.SelectedItem.Text, txtDescription2.Text, txtSerialCode.Text, drpStatus.Value.ToString()
                 , txtERPCode.Text, float.Parse(dc.chkNull_0(txtShelfAgeInDays.Text)), Convert.ToInt32(drpUnitLifeCode.SelectedItem.Value), chkIsInventoryWeight.Checked, cbKOT.Checked, ButtonColor, hidSKUImageName.Value
                 ,chkIsSaleWeight.Checked,chkIsGroup.Checked,chIsPackage.Checked);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                mPopUpLocation.Hide();
            }
            CLearAll();
            LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        LoadAllItems();

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        try
        {
            cbKOT.Checked = false;
            chkIsGroup.Checked = false;
            chIsPackage.Checked = false;
            txtskuname.Text = "";
            txtDescription.Text = "";
            ddlCategory.SelectedIndex = 0;
            ToggleActivationControles(true);
            chkIsRecipe.Enabled = true;
        }
        catch (Exception)
        {

        }
        mPopUpLocation.Show();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            mPopUpLocation.Show();
            CLearAll();
            
            ddlCategory.SelectedIndex = 0;
            LoadCategory();
            LoadUOM();
        }
        catch (Exception)
        {
        }
    }

    protected void btnActive_Click(object sender, EventArgs e)
    {
        UserController _mUController = new UserController();
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in grdSKUData.Rows)
            {
                var chRelized2 = (CheckBox)dr2.Cells[0].FindControl("chkRow");

                if (chRelized2.Checked)
                {
                    check = true;
                    break;
                }

            }
            if (!check)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No Record Selected');", true);
                return;
            }
            bool flag = false;
            foreach (GridViewRow dr in grdSKUData.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("chkRow");

                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[19].Text) == "Active")
                    {
                        _mUController.ActiveInactive(false, Convert.ToInt32(dr.Cells[5].Text), int.Parse(Session["UserId"].ToString()), 1);

                        flag = true;
                    }
                    else
                    {
                        _mUController.ActiveInactive(true, Convert.ToInt32(dr.Cells[5].Text), int.Parse(Session["UserId"].ToString()), 1);

                        flag = true;
                    }

                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Hide();
        CLearAll();
      
        ddlCategory.SelectedIndex = 0;
        LoadCategory();
        LoadUOM();
    }
    #endregion

    #region Index Change

    protected void grdSKUData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSKUData.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
   

    #endregion


    /// <summary>
    /// Clears Form Controls on Save and Update
    /// </summary>
    private void CLearAll()
    {
        try
        {
            Session.Remove("OldSKUName");
            Session.Remove("OldERPCode");
            LoadGrid("");
            txtskucode.Text = "";
            txtunitincase.Text = "";
            txtskuname.Text = "";
            txtDescription.Text = "";
            btnSave.Text = "Save";
            txtMinLevel.Text = "";
            txtReorderLevel.Text = "";
            txtDescription2.Text = "";
            txtMaxLevel.Text = "";
            txtPurchaseToStockFactor.Text = "1";
            txtPurchaseToSaleFactor.Text = "1";
            txtSaleToStockFactor.Text = "1";
            txtSaleToPurchaseFactor.Text = "1";
            txtStockToPurchaseFactor.Text = "1";
            txtStockToSaleFactor.Text = "1";
            txtSerialCode.Text = "";
            txtFEDPercentage.Text = "";
            txtWHTPercentage.Text = "";
            txtERPCode.Text = "";
            txtAgeInDays.Text = "";
            txtShelfAgeInDays.Text = "";

            chkIsOverSaleAllowed.Checked = true;
            chkIsRecipe.Checked = true;
            chkIsDeal.Checked = false;
            chkIsModifier.Checked = false;
            divColorPicker.Attributes.Add("style", "visibility:hidden");
            chkIsRecipe.Enabled = true;
            chkIsSerialized.Checked = false;
            chkIsFEDItem.Checked = false;
            chkIsWHTItem.Checked = false;
            chkIsHazardous.Checked = false;
            chkIsBatchItem.Checked = false;
            chkIsExpiryAllowed.Checked = false;
            chkIsWarehouseItem.Checked = false;
            chkIsMarketItem.Checked = false;
            chkIsReplaceable.Checked = false;
            ToggleActivationControles(true);

            drpStockToSaleOperator.SelectedIndex = 3;
            drpStockToPurchaseOperator.SelectedIndex = 3;
            drpSaletoPurchaseOperator.SelectedIndex = 3;
            drpSaleToStockOperator.SelectedIndex = 3;
            drpPurchaseToSaleOperator.SelectedIndex = 3;
            drpPurchaseToStockOperator.SelectedIndex = 3;
            TabContainer1.ActiveTabIndex = 0;
            drpStatus.SelectedIndex = 0;
            hidSKUImageName.Value = "";
            hidSKUImageSource.Value = "";
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
        }
        catch (Exception)
        {
        }
    }

    private void ToggleActivationControles(bool TrueFalse)
    {
        //drpStockUnit.Enabled = TrueFalse;
        drpStockToSaleOperator.Enabled = TrueFalse;
        txtStockToSaleFactor.Enabled = TrueFalse;
        drpStockToPurchaseOperator.Enabled = TrueFalse;
        txtStockToPurchaseFactor.Enabled = TrueFalse;

        //drpSaleUnit.Enabled = TrueFalse;
        drpSaletoPurchaseOperator.Enabled = TrueFalse;
        txtSaleToPurchaseFactor.Enabled = TrueFalse;
        drpSaleToStockOperator.Enabled = TrueFalse;
        txtSaleToStockFactor.Enabled = TrueFalse;

        //drpPurchaseUnit.Enabled = TrueFalse;
        drpPurchaseToSaleOperator.Enabled = TrueFalse;
        txtPurchaseToSaleFactor.Enabled = TrueFalse;
        drpPurchaseToStockOperator.Enabled = TrueFalse;
        txtPurchaseToStockFactor.Enabled = TrueFalse;
    }

    /// <summary>
    /// Working on Cancel and Close Click
    /// </summary>

    protected void drpStockUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpSaleUnit.SelectedIndex = drpStockUnit.SelectedIndex;
        drpPurchaseUnit.SelectedIndex = drpStockUnit.SelectedIndex;
        mPopUpLocation.Show();
        hfStockSale.Value = "0";
        hfStockPurchase.Value = "0";
        hfSalePurchase.Value = "0";
        hfSaleStock.Value = "0";
        hfPurchaseSale.Value = "0";
        hfPurchaseStock.Value = "0";

        WhiteColor(txtStockToSaleFactor);
        WhiteColor(txtStockToPurchaseFactor);
        WhiteColor(txtSaleToPurchaseFactor);
        WhiteColor(txtSaleToStockFactor);
        WhiteColor(txtPurchaseToSaleFactor);
        WhiteColor(txtPurchaseToStockFactor);
    }

    protected void drpSaleUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        // If Sale unit is differ from stock
        if (drpSaleUnit.SelectedIndex != drpStockUnit.SelectedIndex)
        {
            hfStockSale.Value = "1";
            hfSaleStock.Value = "1";

            RedColor(txtStockToSaleFactor);
            RedColor(txtSaleToStockFactor);
        }
        else
        {
            hfStockSale.Value = "0";
            hfSaleStock.Value = "0";

            WhiteColor(txtStockToSaleFactor);
            WhiteColor(txtSaleToStockFactor);
        }
        // If Sale unit is differ from Purchase
        if (drpSaleUnit.SelectedIndex != drpPurchaseUnit.SelectedIndex)
        {
            hfSalePurchase.Value = "1";
            hfPurchaseSale.Value = "1";

            RedColor(txtSaleToPurchaseFactor);
            RedColor(txtPurchaseToSaleFactor);
        }
        else
        {
            hfSalePurchase.Value = "0";
            hfPurchaseSale.Value = "0";

            WhiteColor(txtSaleToPurchaseFactor);
            WhiteColor(txtPurchaseToSaleFactor);
        }
    }

    protected void drpPurchaseUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        // If Purchase unit is differ from stock
        if (drpPurchaseUnit.SelectedIndex != drpStockUnit.SelectedIndex)
        {
            hfStockPurchase.Value = "1";
            hfPurchaseStock.Value = "1";

            RedColor(txtStockToPurchaseFactor);
            RedColor(txtPurchaseToStockFactor);
        }
        else
        {
            hfStockPurchase.Value = "0";
            hfPurchaseStock.Value = "0";

            WhiteColor(txtStockToPurchaseFactor);
            WhiteColor(txtPurchaseToStockFactor);
        }
        // If Sale unit is differ from Purchase
        if (drpSaleUnit.SelectedIndex != drpPurchaseUnit.SelectedIndex)
        {
            hfSalePurchase.Value = "1";
            hfPurchaseSale.Value = "1";

            RedColor(txtSaleToPurchaseFactor);
            RedColor(txtPurchaseToSaleFactor);
        }
        else
        {
            hfSalePurchase.Value = "0";
            hfPurchaseSale.Value = "0";

            WhiteColor(txtSaleToPurchaseFactor);
            WhiteColor(txtPurchaseToSaleFactor);
        }
    }
    public void RedColor(TextBox txtbox)
    {
        //txtbox.BackColor = System.Drawing.Color.Red;
    }
    public void WhiteColor(TextBox txtbox)
    {
        //txtbox.BackColor = System.Drawing.Color.White;
    }

    public bool IsSKUEditable(int SKUID)
    {
        bool flag = true;
        DataTable dtSKU = _mSkuController.CheckSKU(SKUID);
        if(dtSKU != null)
        {
            if(dtSKU.Rows.Count > 0)
            {
                flag = false;
            }
        }
        return flag;
    }

    public bool NameIsDuplicate()
    {
        bool flag = false;
        DataTable dtSKUName = _mSkuController.GetSKUByName(txtskuname.Text.Trim(), Convert.ToInt32(ddlCategory.Value), Constants.IntNullValue);
        if(dtSKUName != null)
        {
            if(dtSKUName.Rows.Count > 0)
            {
                flag = true;
            }
        }
        return flag;
    }

    public bool ERPCodeIsDuplicate()
    {
        bool flag = false;
        DataTable dtSKUName = _mSkuController.GetERPCode(txtERPCode.Text.Trim());
        if (dtSKUName != null)
        {
            if (dtSKUName.Rows.Count > 0)
            {
                flag = true;
            }
        }
        return flag;
    }

    private void UploadImage()
    {
        try
        {
            if (hidSKUImageSource.Value.Length > 0)
            {
                string imageExtention = Path.GetExtension(hidSKUImageName.Value);
                hidSKUImageName.Value = "Sku_" + Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhmmssfff") + imageExtention;

                string file_name = hidSKUImageName.Value;
                if (file_name.Length == 0) return;

                string file_path = Server.MapPath("../UserImages/Sku");
                if (!Directory.Exists(file_path))
                {
                    Directory.CreateDirectory(file_path);
                }
                string temp = hidSKUImageSource.Value.Substring(0, hidSKUImageSource.Value.IndexOf("base64") + "base64".Length + 1);
                hidSKUImageSource.Value = hidSKUImageSource.Value.Replace(temp, "");
                byte[] renderedBytes = Convert.FromBase64String(hidSKUImageSource.Value);
                using (FileStream fileStream = File.Create(file_path + "\\" + file_name, renderedBytes.Length))
                {
                    fileStream.Write(renderedBytes, 0, renderedBytes.Length);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void ValidateMediaExtensions(ref string _error_info, string _extension, string allowed_extensions)
    {
        string[] ext_spliter = allowed_extensions.Split('|');
        bool _is_valid = false;
        for (int i = 0; i <= ext_spliter.Length - 1; i++)
        {
            if (ext_spliter[i] == _extension)
            {
                _is_valid = true;
            }
        }
        if (_is_valid == false)
        {
            _error_info = "Only (" + allowed_extensions + ") file extension(s) is allowed. ";
        }
    }

    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by NRefactory.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================

}
