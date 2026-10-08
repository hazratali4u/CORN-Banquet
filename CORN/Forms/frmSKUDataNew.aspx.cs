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

public partial class Forms_frmSKUDataNew : System.Web.UI.Page
{
    readonly SkuController _mSkuController = new SkuController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();


    #region Load

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            CreateTable();
            LoadGrid("");
            LoadCheckBoxCategory();
            LoadCheckBoxVendors();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
        }

    }
    private void LoadCheckBoxCategory()
    {
        try
        {
            DataTable _mDt = _mHerController.CheckBoxCategory(Constants.IntNullValue, null);
            clsWebFormUtil.FillDxListBox(ChCategoryList, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    private void LoadCheckBoxVendors()
    {
        try
        {
            DataTable _mDt = _mHerController.CheckBoxVendor(Constants.IntNullValue, null);
            clsWebFormUtil.FillDxListBox(ChVendorList, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }

    private void LoadGrid(string pType)
    {

        grdSKUData.DataSource = null;
        grdSKUData.DataBind();

        DataTable dt = new DataTable();
        dt = _mSkuController.SelectSKUSNew(null, null, false, 2);

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)//In case after  Filter
            {
                dt.DefaultView.RowFilter = "SKU_NAME LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Description LIKE '" + txtSearch.Text + "%'";
            }
            grdSKUData.DataSource = dt;
            grdSKUData.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_NAME LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%' OR SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Description LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                grdSKUData.PageIndex = 0;
            }
            grdSKUData.DataSource = dt;
            grdSKUData.DataBind();
        }
    }

    #endregion

    #region Grid
    protected void grdSKUData_RowEditing(object sender, GridViewEditEventArgs e)
    {

        TabContainer1.ActiveTabIndex = 0;
        btnSave.Text = "Update";

        try
        {

            txtskucode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");

            StringWriter myWriter = new StringWriter();
            HttpUtility.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", ""), myWriter);
            txtskuname.Text = myWriter.ToString();
            Session.Add("OldSKUName", txtskuname.Text);

            hfStatus.Value = grdSKUData.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
            txtDescription2.Text = grdSKUData.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
            hidSKUImageName.Value = (grdSKUData.Rows[e.NewEditIndex].FindControl("hidSKUImageName") as HiddenField).Value;

            mPopUpLocation.Show();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    #endregion

    #region Click 

    public void CreateTable()
    {
        DataTable CT = new DataTable();
        DataTable VT = new DataTable();
        CT.Columns.Add("CATEGORY_ID", typeof(int));
        Session.Add("CT", CT);
        VT.Columns.Add("VENDOR_ID", typeof(int));
        Session.Add("VT", VT);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();

        int sectionId = 0;

        try
        {
            DataTable CT = (DataTable)(Session["CT"]);
            DataTable VT = (DataTable)(Session["VT"]);

            for (int i = 0; i < ChCategoryList.Items.Count; i++)
            {
                if (ChCategoryList.Items[i].Selected)
                {
                    DataRow DR = CT.NewRow();
                    DR["CATEGORY_ID"] = Convert.ToInt32(ChCategoryList.Items[i].Value);
                    CT.Rows.Add(DR);
                    Session.Add("CT", CT);
                }

            }

            for (int i = 0; i < ChVendorList.Items.Count; i++)
            {
                if (ChVendorList.Items[i].Selected)
                {
                    DataRow DR = VT.NewRow();
                    DR["VENDOR_ID"] = Convert.ToInt32(ChVendorList.Items[i].Value);
                    VT.Rows.Add(DR);
                    Session.Add("VT", VT);
                }

            }

            if (btnSave.Text == "Save")
            {
                if (NameIsDuplicate())
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('This name already exist for this Category Type and Category.');", true);
                    return;
                }

                _mSkuController.InsertSKUSNew(Constants.IntNullValue, txtskuname.Text, txtDescription2.Text, true, int.Parse(Session["UserID"].ToString()), 1, CT, VT);


                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                mPopUpLocation.Hide();
            }
            else if (btnSave.Text == "Update")
            {
                DataTable dt = new DataTable();
                _mSkuController.InsertSKUSNew(int.Parse(hfSKU_ID.Value.ToString()), txtskuname.Text, txtDescription2.Text, true, int.Parse(Session["UserID"].ToString()), 3, CT, VT);
                mPopUpLocation.Hide();


                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                mPopUpLocation.Hide();
            }
            CT.Clear();
            VT.Clear();
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


    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        try
        {
            txtskuname.Text = "";
            txtDescription2.Text = "";
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
                    if (Convert.ToString(dr.Cells[4].Text) == "Active")
                    {
                        _mUController.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), int.Parse(Session["UserId"].ToString()), 1);

                        flag = true;
                    }
                    else
                    {
                        _mUController.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), int.Parse(Session["UserId"].ToString()), 1);

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
    }
    #endregion

    #region Index Change

    protected void grdSKUData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSKUData.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
    #endregion

    private void CLearAll()
    {
        try
        {
            LoadGrid("");
            txtskucode.Text = "";
            txtskuname.Text = "";
            btnSave.Text = "Save";
            txtDescription2.Text = "";
            for (int i = 0; i < ChCategoryList.Items.Count; i++)
            {
                ChCategoryList.Items[i].Selected = false;
            }
            for (int i = 0; i < ChVendorList.Items.Count; i++)
            {
                ChVendorList.Items[i].Selected = false;
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
        }
        catch (Exception)
        {
        }
    }

    public bool NameIsDuplicate()
    {
        bool flag = false;
        DataTable dtSKUName = _mSkuController.GetSKUByName(txtskuname.Text.Trim(), Constants.IntNullValue, Constants.IntNullValue);
        if (dtSKUName != null)
        {
            if (dtSKUName.Rows.Count > 0)
            {
                flag = true;
            }
        }
        return flag;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();

        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        hfSKU_ID.Value = Row.Cells[1].Text;

        #region Load Categories
        DataTable dtCategory = new DataTable();
        dtCategory = _mSkuController.SelectSKUCategory(int.Parse(Row.Cells[1].Text), 1);

        ChCategoryList.Items.Clear();
        DataTable _mDt = _mHerController.CheckBoxCategory(Constants.IntNullValue, null);
        clsWebFormUtil.FillDxListBox(ChCategoryList, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");

        for (int i = 0; i < ChCategoryList.Items.Count; i++)
        {
            foreach (DataRow dr in dtCategory.Rows)
            {
                if (Convert.ToInt32(ChCategoryList.Items[i].Value) == int.Parse(dr["CATEGORY_ID"].ToString()))
                {
                    ChCategoryList.Items[i].Selected = true;
                }
            }

        }
        #endregion

        #region Load Vendor
        DataTable dtVendor = new DataTable();
        dtVendor = _mSkuController.SelectSKUCategory(int.Parse(Row.Cells[1].Text), 2);

        ChVendorList.Items.Clear();
        DataTable _mDtv = _mHerController.CheckBoxVendor(Constants.IntNullValue, null);
        clsWebFormUtil.FillDxListBox(ChVendorList, _mDtv, "SKU_HIE_ID", "SKU_HIE_NAME");

        for (int i = 0; i < ChVendorList.Items.Count; i++)
        {
            foreach (DataRow dr in dtVendor.Rows)
            {
                if (Convert.ToInt32(ChVendorList.Items[i].Value) == int.Parse(dr["VENDOR_ID"].ToString()))
                {
                    ChVendorList.Items[i].Selected = true;
                }
            }

        }
        #endregion
        txtskuname.Text = Row.Cells[2].Text;
        txtDescription2.Text = Row.Cells[3].Text;
        btnSave.Text = "Update";

    }



    #region remove
    //protected void del_Click(object sender, EventArgs e)
    //{
    //    DataTable dt = new DataTable();
    //    GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
    //    _mSkuController.InsertSKUSNew(int.Parse(Row.Cells[1].Text), txtskuname.Text, txtDescription2.Text, true, int.Parse(Session["UserID"].ToString()), 4, dt, dt);
    //    LoadGrid("");
    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Deleted successfully.');", true);
    //}
    #endregion
}





