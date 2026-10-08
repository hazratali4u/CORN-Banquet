using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;

public partial class Forms_frmItemDeals : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly SkuController mSKUController = new SkuController();
    readonly SkuHierarchyController sController = new SkuHierarchyController();

    DataTable _dealSkus;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("pragma", "no-cache");

            try
            {
                LoadHasModifier();
                LoadCategory();
                CreateTable();
                LoadGrid();
                UnAssignItem();
                LoadMaster();
                btnAssignItem.Attributes.Add("onclick", "return ValidateForm();");
                btnAssignAllItem.Attributes.Add("onclick", "return ValidateForm();");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    #region Load

    private void CreateTable()
    {
        _dealSkus = new DataTable();

        _dealSkus.Columns.Add("intStockMUnitCode", typeof(int));
        _dealSkus.Columns.Add("ModifierSKU_ID", typeof(int));
        _dealSkus.Columns.Add("ITEM_DEAL_NAME", typeof(string));
        _dealSkus.Columns.Add("CATEGORY_DEAL_ID", typeof(int));
        _dealSkus.Columns.Add("CATEGORY_ID", typeof(int));
        _dealSkus.Columns.Add("CATEGORY_NAME", typeof(string));
        _dealSkus.Columns.Add("IsModifierID", typeof(int));
        _dealSkus.Columns.Add("Is_Modifier", typeof(string));
        _dealSkus.Columns.Add("Default_Qty", typeof(int));
        _dealSkus.Columns.Add("SKU_QUANTITY", typeof(int));
        _dealSkus.Columns.Add("IS_CatChoiceBased", typeof(bool));
        _dealSkus.Columns.Add("IS_ChoiceBased", typeof(bool));
        _dealSkus.Columns.Add("IS_Optional", typeof(bool));
        _dealSkus.Columns.Add("IS_Active", typeof(bool));
        _dealSkus.Columns.Add("Status", typeof(string));
        Session.Add("dealSkus", _dealSkus);
    }

    private void LoadHasModifier()
    {
        try
        {
            DataTable Dtsku_HasModifier = mSKUController.SelectSkuHasModifier(0);
            clsWebFormUtil.FillDxComboBoxList(this.ddlHasModifier, Dtsku_HasModifier, 0, 2, true);

            if (Dtsku_HasModifier.Rows.Count > 0)
            {
                ddlHasModifier.SelectedIndex = 0;
            }
            Session.Add("dt", Dtsku_HasModifier);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void LoadCategory()
    {
        SkuHierarchyController _mHerController = new SkuHierarchyController();
        try
        {
            DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, 1, null, null, true, int.Parse(Session["CompanyId"].ToString()), Constants.IntNullValue);
            clsWebFormUtil.FillDxComboBoxList(drpCateogry, _mDt, 0, 3, false);

            if (_mDt.Rows.Count > 0)
            {
                drpCateogry.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    /// <summary>
    /// Load items on List Box category wise
    /// </summary>
    private void UnAssignItem()
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        lstUnAssignItem.Items.Clear();

        try
        {
            if (ddlHasModifier.Items.Count > 0 && drpCateogry.Items.Count > 0)
            {
                DataTable dt = sController.SelectDealAssignment(int.Parse(ddlHasModifier.SelectedItem.Value.ToString()), int.Parse(drpCateogry.SelectedItem.Value.ToString()), int.Parse(Session["DISTRIBUTOR_ID"].ToString()), 6);
                clsWebFormUtil.FillListBox(lstUnAssignItem, dt, 0, 1, true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Get Category base Information
    /// </summary>
    private void LoadMaster()
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        try
        {
            if (ddlHasModifier.Items.Count > 0 && drpCateogry.Items.Count > 0)
            {
                DataTable dt = sController.SelectDealAssignment(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 6);
                Session.Add("dtModifier", dt);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadGrid()
    {
        chkIsActive.Checked = true;
        grdPackage.DataSource = null;
        grdPackage.DataBind();
        try
        {
            if (ddlHasModifier.Items.Count > 0 && drpCateogry.Items.Count > 0)
            {
                DataTable _dealSkus = mSKUController.SelectModifier(Convert.ToInt32(ddlHasModifier.SelectedItem.Value));
                Session.Add("dealSkus", _dealSkus);
                grdPackage.DataSource = _dealSkus;
                grdPackage.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Sel/Index Change 

    protected void ddlHasModifier_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("IsModifierID  = '" + ddlHasModifier.SelectedItem.Value + "'");
        if (foundRows.Length > 0)
        {
            hfCategoryId.Value = foundRows[0]["CATEGORY_ID"].ToString();
        }
        LoadGrid();
    }
    protected void drpCateogry_SelectedIndexChanged(object sender, EventArgs e)
    {
        UnAssignItem();
    }
    
    #endregion

    protected void grdPackage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            _dealSkus = (DataTable)Session["dealSkus"];

            if (_dealSkus.Rows.Count > 0)
            {
                DataRow dr = _dealSkus.Rows[e.RowIndex];
                dr["Status"] = "Delete";
                Session.Add("dealSkus", _dealSkus);
                drpCateogry.Value = _dealSkus.Rows[e.RowIndex]["CATEGORY_ID"].ToString();
                grdPackage.DataSource = _dealSkus;
                grdPackage.DataBind();
                
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    private bool CheckDublicateSku(int sku_id)
    {
        try
        {
            _dealSkus = (DataTable)Session["dealSkus"];

            DataRow[] foundRows = _dealSkus.Select("ModifierSKU_ID  = '" + ddlHasModifier.SelectedItem.Value + "'and IsModifierID='" + sku_id + "'and Status='" + "New" + "'");

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


    #region Click Operations

    protected void btnAssignItem_Click(object sender, EventArgs e)
    {
        try
        {
            CreateTable();
            DataRow drNew;
            foreach (GridViewRow item in grdPackage.Rows)
            {
                if (ddlHasModifier.Items.Count > 0)
                {
                    if (item.Cells[5].Text.ToString() != "Delete")
                    {
                        drNew = _dealSkus.NewRow();
                        drNew["CATEGORY_ID"] = int.Parse(item.Cells[0].Text.ToString());
                        drNew["CATEGORY_NAME"] = item.Cells[1].Text.ToString();
                        drNew["Is_Modifier"] = item.Cells[3].Text.ToString();
                        drNew["ModifierSKU_ID"] = int.Parse(item.Cells[4].Text.ToString());
                        drNew["Default_Qty"] = 1;
                        drNew["Status"] = item.Cells[5].Text.ToString();
                        drNew["IsModifierID"] = Convert.ToInt32(item.Cells[6].Text);
                        drNew["intStockMUnitCode"] = Convert.ToInt32(item.Cells[6].Text);

                        _dealSkus.Rows.Add(drNew);
                    }
                }
            }

            DataRow dr = _dealSkus.NewRow();

            for (int i = 0; i < lstUnAssignItem.Items.Count; i++)
            {
                if (lstUnAssignItem.Items[i].Selected == true && drpCateogry.Items.Count > 0)
                {
                    if (!CheckDublicateSku(int.Parse(lstUnAssignItem.Items[i].Value)))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + lstUnAssignItem.Items[i].Text + " Already Exists ');", true);

                        return;
                    }
                    else
                    {
                        _dealSkus = (DataTable)Session["dealSkus"];
                        dr["ModifierSKU_ID"] = int.Parse(ddlHasModifier.SelectedItem.Value.ToString());
                        dr["CATEGORY_ID"] = int.Parse(drpCateogry.SelectedItem.Value.ToString());
                        dr["CATEGORY_NAME"] = drpCateogry.SelectedItem.Text;
                        dr["IsModifierID"] = int.Parse(lstUnAssignItem.Items[i].Value);
                        dr["Is_Modifier"] = lstUnAssignItem.Items[i].Text;
                        dr["Default_Qty"] = 1;
                        dr["Status"] = "New";
                        _dealSkus.Rows.Add(dr);
                        break;
                    }
                }
            }

            Session.Add("dealSkus", _dealSkus);

            grdPackage.DataSource = _dealSkus;
            grdPackage.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAssignAllItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (drpCateogry.Items.Count > 0)
            {
                _dealSkus = (DataTable)Session["dealSkus"];
                bool flag = false;

                for (int i = 0; i < lstUnAssignItem.Items.Count; i++)
                {
                    if (CheckDublicateSku(int.Parse(lstUnAssignItem.Items[i].Value)))
                    {

                        flag = true;
                        DataRow dr = _dealSkus.NewRow();
                        dr["ModifierSKU_ID"] = int.Parse(ddlHasModifier.SelectedItem.Value.ToString());
                        dr["CATEGORY_ID"] = int.Parse(drpCateogry.SelectedItem.Value.ToString());
                        dr["CATEGORY_NAME"] = drpCateogry.SelectedItem.Text;
                        dr["IsModifierID"] = int.Parse(lstUnAssignItem.Items[i].Value);
                        dr["Is_Modifier"] = lstUnAssignItem.Items[i].Text;
                        dr["Default_Qty"] = 1;
                        dr["Status"] = "New";
                        _dealSkus.Rows.Add(dr);
                    }
                }
                if (flag)
                {
                    Session.Add("dealSkus", _dealSkus);
                    grdPackage.DataSource = _dealSkus;
                    grdPackage.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            sController.UpdateAssignItem(int.Parse(ddlHasModifier.SelectedItem.Value.ToString()), int.Parse(Session["DISTRIBUTOR_ID"].ToString())
                               , DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(this.Session["UserId"].ToString()), null);

            LoadGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpSection.Hide();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable orderSkus = new DataTable();
            orderSkus.Columns.Add("IsModifierID", typeof(int));
            orderSkus.Columns.Add("StockUnitID", typeof(int));
            orderSkus.Columns.Add("Default_Qty", typeof(int));
            orderSkus.Columns.Add("Status", typeof(string));
            orderSkus.Columns.Add("IS_Manadatory",typeof(bool));
            foreach (GridViewRow item in grdPackage.Rows)
            {
                DataRow dr = orderSkus.NewRow();
                if (ddlHasModifier.Items.Count > 0)
                {
                    DataTable dtModifier = (DataTable)Session["dtModifier"];
                    DataRow[] foundRows = dtModifier.Select("SKU_ID='" + item.Cells[6].Text + "'");

                    dr["IsModifierID"] = int.Parse(item.Cells[6].Text.ToString());
                    dr["StockUnitID"] = Convert.ToInt32(foundRows[0]["PACKSIZE"]);
                    dr["Default_Qty"] = 1;
                    dr["Status"] = item.Cells[5].Text.ToString();
                    dr["IS_Manadatory"] = false;
                    orderSkus.Rows.Add(dr);
                }
            }

            if (orderSkus.Rows.Count > 0)
            {
                mSKUController.InsertModifierBulk(Convert.ToInt32(ddlHasModifier.SelectedItem.Value), orderSkus);
                LoadGrid();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record saved successfully');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Plz make a Deal');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
   
    #endregion
}