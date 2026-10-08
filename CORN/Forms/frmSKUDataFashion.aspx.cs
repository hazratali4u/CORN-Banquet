using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;
using System.Web.UI;
using System.IO;

/// <summary>
/// From To Add, Edit, Delete SKU Hierarchy
/// </summary
/// 
public partial class frmSKUDataFashion : System.Web.UI.Page
{
    //readonly SKUDataFashionController _mController = new SKUDataFashionController();
    readonly SkuHierarchyController _mDivisionController = new SkuHierarchyController();

    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        
        if (!IsPostBack)
        {
            btnSaveSKUDataFashion.Attributes.Add("onclick", "return ValidateForm()");
            // Division Drop Down
            DataTable dtDivision = new DataTable();
            dtDivision = _mDivisionController.SelectPrincipal(Constants.SKUDivision, int.Parse(this.Session["CompanyId"].ToString()));
            int i = 0;
            foreach (DataRow row in dtDivision.Rows)
            {
                ddlDivision.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlDivision.SelectedValue = "0";

            // Category Drop Down
            DataTable dtCategory = new DataTable();
            dtCategory = _mDivisionController.SelectPrincipal(Constants.SKUCategory, int.Parse(this.Session["CompanyId"].ToString()));
             i = 0;
            foreach (DataRow row in dtCategory.Rows)
            {
                ddlCategory.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlCategory.SelectedValue = "0";

            // SubCategory Drop Down
            // this.LoadSubCategory("");
            DataTable dtSubCategory = new DataTable();
            dtSubCategory = _mDivisionController.SelectPrincipal(Constants.SKUSubCategory, int.Parse(this.Session["CompanyId"].ToString()));
            i = 0;
            foreach (DataRow row in dtSubCategory.Rows)
            {
                ddlSubCategory.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlSubCategory.SelectedValue = "0";

            // Gender Drop Down
            DataTable dtGender = new DataTable();
            dtGender = _mDivisionController.SelectPrincipal(Constants.SKUGender, int.Parse(this.Session["CompanyId"].ToString()));
            i = 0;
            foreach (DataRow row in dtGender.Rows)
            {
                ddlGender.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlGender.SelectedValue = "0";

            // Brand Drop Down
            DataTable dtBrand = new DataTable();
            dtBrand = _mDivisionController.SelectPrincipal(Constants.SKUBrand, int.Parse(this.Session["CompanyId"].ToString()));
            i = 0;
            foreach (DataRow row in dtBrand.Rows)
            {
                ddlBrand.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlBrand.SelectedValue = "";


            LoadGrid("");
        }
    }
    private void LoadGrid(string pType)
    {
        GrdSKUSDataFashion.DataSource = null;
        GrdSKUSDataFashion.DataBind();

        DataTable dt = new DataTable();
       // dt = _mController.SelectSKUFashion(int.Parse(this.Session["CompanyId"].ToString()));

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Category LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            GrdSKUSDataFashion.DataSource = dt;
            GrdSKUSDataFashion.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_NAME LIKE '%" + txtSearch.Text + "%' OR Category LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                GrdSKUSDataFashion.PageIndex = 0;
            }
            GrdSKUSDataFashion.DataSource = dt;
            GrdSKUSDataFashion.DataBind();
        }
    }

    


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        ClearAll();
    }
   
    private void ClearAll()
    {
        ddlDivision.SelectedValue = "0";
        ddlCategory.SelectedValue = "0";
        ddlSubCategory.SelectedValue = "0";

        ddlGender.SelectedValue = "0";
        ddlBrand.SelectedValue = "0";
        ddlOrign.SelectedValue = "0";
        ddlGSTOn.SelectedValue = "0";
        ddlSeasion.SelectedValue = "0";
        txtStyleCode.Text = "";
        txtBarCode.Text = "";
        txtItemName.Text = "";
        txtSize.Text = "";
        txtYear.Text = "";
        txtColor.Text = "";
        txtSKU.Text = "";
        hfPic.Value = "";

        lblErrorMsg.Text = "";
        btnSaveSKUDataFashion.Text = "Save";
        myModalLabel.InnerText = "Add New Item Information";
        hfSKUlId.Value = "";

    }



    /// <summary>
    /// Gets Code For New Principal, Division, Category And Brand
    /// </summary>
    /// <param name="preeFix">Prefix</param>
    /// <param name="codeType">Type</param>
    /// <returns>Code As String</returns>
    private string GetAutoCode(string preeFix, int codeType)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        return AutoCode.GetAutoCode(preeFix, codeType, Constants.LongNullValue);
    }

    /// <summary>
    /// Sets Code For Principal, Division, Category And Brand
    /// </summary>
    /// <param name="preeFix">Prefix</param>
    /// <param name="cValue">Value</param>
    private void SetAutoCode(string preeFix, long cValue)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        string result = AutoCode.GetAutoCode(preeFix, 1, cValue);
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        UserController _UserCtrl = new UserController();
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in GrdSKUSDataFashion.Rows)
            {
                var chRelized2 = (CheckBox)dr2.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized2.Checked)
                {
                    check = true;
                    break;
                }

            }
            if (!check)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select record first');", true);
                return;
            }
            bool flag = false;

            foreach (GridViewRow dr in GrdSKUSDataFashion.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[18].Text) == "Active")
                    {
                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 1);
                        flag = true;
                    }
                    else
                    {
                        _UserCtrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 1);
                        flag = true;
                    }
                }
                if (flag)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
                }
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSaveSKUDataFashion.Text = "Save";
        myModalLabel.InnerText = "Add New Item Information";
        mPopUpLocation.Show();
    }
    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdSKUSDataFashion.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void GrdSKUSDataFashion_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Session.Add("Pic", "");
        hfPic.Value = "";
        hfSKUlId.Value = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[1].Text.Replace("&nbsp;", ""); ;
        ddlDivision.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[21].Text.Replace("&nbsp;", ""); ;

        hfPic.Value = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[26].Text.Replace("&nbsp;", "");
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "MyJSFunction", "MyJSFunction2();", true);
        if (Convert.ToString(hfPic.Value) == "")
        {
            Session["haspic"] = 0;
        }
        else
        {
            Session["haspic"] = 1;
        }



        
        ddlBrand.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[22].Text.Replace("&nbsp;", "");
        ddlCategory.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");

       // this.LoadSubCategory(ddlCategory.SelectedValue);

        ddlSubCategory.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[5].Text;

        this.LoadCategory(ddlSubCategory.SelectedValue);

        ddlGender.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[23].Text.ToString();
        txtBarCode.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
        txtStyleCode.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[13].Text.Replace("&nbsp;", "");
        txtItemName.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[14].Text.Replace("&nbsp;", "");
        txtSize.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[15].Text.Replace("&nbsp;", "");
        txtColor.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[16].Text.Replace("&nbsp;", "");
        txtYear.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[19].Text.Replace("&nbsp;", "");
        txtSKU.Text = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[15].Text.Replace("&nbsp;", "");
        ddlGSTOn.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[17].Text.Trim();
        ddlSeasion.SelectedValue = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[24].Text.Replace("&nbsp;", "");
        hfStatus.Value = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[18].Text;
        ddlOrign.SelectedValue  = GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[27].Text.Replace("&nbsp;", "");
        chkShowPOS.Checked= bool.Parse(GrdSKUSDataFashion.Rows[e.NewEditIndex].Cells[28].Text);

        btnSaveSKUDataFashion.Text = "Update";
        myModalLabel.InnerText = "Edit Product Information";
        mPopUpLocation.Show();



    }

    protected void btnSaveSKUDataFashion_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dr in GrdSKUSDataFashion.Rows)
        {
            var ItemName = dr.Cells[14].Text;

            if (ItemName==txtItemName.Text)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Duplicat Item Name');", true);
                mPopUpLocation.Show();
                return;
            }

        } 


        string fileName = null;
        if (fuPic.HasFile)
        {
            Session["haspic"] = 1;
            string path = Server.MapPath("~/Pics");
            string fExtension = "";
            FileInfo oFileInfo = new FileInfo(fuPic.PostedFile.FileName);
            fExtension = Path.GetExtension(fuPic.FileName);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "pic" + fExtension;
            string fullFileName = path + "\\" + fileName;
            Session.Add("Pic", fileName);
            hfPic.Value = fileName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fuPic.PostedFile.SaveAs(fullFileName);
        }

       

        try
        {
            if (Page.IsValid)
            {
                mPopUpLocation.Show();
                lblErrorMsg.Text = "";
                switch (btnSaveSKUDataFashion.Text)
                {

                    case "Save":


                        //_mController.InsertSKUDataFashion(0, txtStyleCode.Text, txtItemName.Text, txtColor.Text, (string)ddlGSTOn.SelectedValue, 
                        //    txtSize.Text, true, int.Parse(ddlDivision.SelectedValue), int.Parse(ddlBrand.SelectedValue), 
                        //    int.Parse(ddlCategory.SelectedValue), int.Parse(this.Session["CompanyId"].ToString()), txtBarCode.Text,
                        //    ddlOrign.SelectedValue, ddlSeasion.SelectedValue, int.Parse(ddlGender.SelectedValue), txtYear.Text, txtSKU.Text, int.Parse(ddlSubCategory.SelectedValue), fileName, chkShowPOS.Checked);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocation.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatus.Value != "Active")
                        {
                            status = false;
                        }
                        //_mController.UpdateSKUDataFashion(int.Parse(hfSKUlId.Value), txtStyleCode.Text, txtItemName.Text, txtColor.Text, (string)ddlGSTOn.SelectedValue,
                        //    txtSize.Text, true, int.Parse(ddlDivision.SelectedValue), int.Parse(ddlBrand.SelectedValue),
                        //    int.Parse(ddlCategory.SelectedValue), int.Parse(this.Session["CompanyId"].ToString()), txtBarCode.Text,
                        //    ddlOrign.SelectedValue, ddlSeasion.SelectedValue, int.Parse(ddlGender.SelectedValue), txtYear.Text, txtSKU.Text, int.Parse(ddlSubCategory.SelectedValue), fileName, chkShowPOS.Checked);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                       // mPopUpLocation.Hide();
                        break;
                }
                ClearAll();
                LoadGrid("");
            }
            else
            {
                mPopUpLocation.Show();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpLocation.Show();
        }

    }

    // Sub Category Drop Down
    //private void LoadSubCategory(string p_Category)
    //{

    //    DataTable dtSubCategory = new DataTable();
    //    dtSubCategory = _mDivisionController.SelectParentCategory(Constants.SKUCategory, int.Parse(this.Session["CompanyId"].ToString()), int.Parse(p_Category) );
    //    int i = 0;
    //    foreach (DataRow row in dtSubCategory.Rows)
    //    {
    //        ddlSubCategory.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
    //        i++;
    //    }
    //    ddlSubCategory.SelectedValue = "0";

    //}

    private void LoadCategory(string p_Category)
    {

        DataTable dtCategory = new DataTable();
        dtCategory = _mDivisionController.SelectParentCategory(Constants.SKUCategory, int.Parse(this.Session["CompanyId"].ToString()), int.Parse(p_Category));
        int i = 0;
        foreach (DataRow row in dtCategory.Rows)
        {
            
            ddlCategory.SelectedValue =  row["SKU_HIE_ID"].ToString();
                i++;
        }
        if (i == 0) {
            ddlCategory.SelectedValue = "0";
        }

    }



    //protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadSubCategory(ddlCategory.SelectedValue);
    //}

    //protected void ddlCategory_TextChanged(object sender, EventArgs e)
    //{
    //    LoadSubCategory(ddlCategory.SelectedValue);
    //    mPopUpLocation.Show();
    //}

    protected void ddlSubCategory_TextChanged(object sender, EventArgs e)
    {
        LoadCategory(ddlSubCategory.SelectedValue);
        mPopUpLocation.Show();
    }
}
