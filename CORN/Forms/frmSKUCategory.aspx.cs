using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI;
using System.IO;

/// <summary>
/// From To Add, Edit, Delete SKU Hierarchy
/// </summary>
public partial class frmSKUCategory : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuHierarchyController mController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();

    /// <summary>
    /// Page_Load Function Populates All Combos And Grids On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            //LoadParentCategories();
            LoadGrid("");
            btnSaveCategory.Attributes.Add("onclick", "return ValidateForm()");
        }
    }
    

    /// <summary>
    /// Loads Categories To Category Grid On Category Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpCategoryPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadGrid("");
    }

    private void LoadGrid(string pType)
    {
        DataTable dt = new DataTable();
        dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()), Constants.IntNullValue);
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR TYPE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            GrdCategory.DataSource = dt;
            GrdCategory.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR TYPE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                GrdCategory.PageIndex = 0;
            }
            GrdCategory.DataSource = dt;
            GrdCategory.DataBind();
        }
    }

    public void CheckCategoryUsed(int CategoryID)
    {
        DataTable dt = mController.SelectSkuHierarchy(Constants.IntNullValue, CategoryID, Constants.IntNullValue, null, null, true, 17, Constants.IntNullValue);
     
    }
    protected void GrdCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            mPopUpCategory.Show();
            btnSaveCategory.Text = "Update";
            hidSKUImageName.Value = (GrdCategory.Rows[e.NewEditIndex].FindControl("hidSKUImageName") as HiddenField).Value;
            if (skuImageUploadArea.Visible)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
            }
            hfcategoryId.Value = GrdCategory.Rows[e.NewEditIndex].Cells[1].Text;
            txtCategoryCode.Text = GrdCategory.Rows[e.NewEditIndex].Cells[2].Text;
            txtCategoryName.Text = GrdCategory.Rows[e.NewEditIndex].Cells[3].Text;
            hfStatus.Value = GrdCategory.Rows[e.NewEditIndex].Cells[5].Text;
            
            try
            {
                drpParentCategory.Value = GrdCategory.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "0");
            }
            catch (Exception)
            {
                drpParentCategory.SelectedIndex = 0;
            }
            txtCategoryName.Enabled = true;
            CheckCategoryUsed(Convert.ToInt32(hfcategoryId.Value));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    protected void GrdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdCategory.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    public bool CheckHierarchyLevel()
    {
        DataTable dt = new DataTable();
        dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue,
            Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()), Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            int level = Convert.ToInt32(dt.Rows[0][0].ToString());
            if (level < 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {
            //if (!CheckHierarchyLevel())
            //{
            //    mPopUpCategory.Show();
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "ErrorMessage()", true);
            //    return;
            //}

            UploadImage();
            if (btnSaveCategory.Text == "Save")
            {
                txtCategoryCode.Text = "";
                mController.InsertHierarchy(Constants.SKUCategory, 1, txtCategoryCode.Text,
                    txtCategoryName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()),
                    Constants.IntNullValue, hidSKUImageName.Value);
                ClearAll();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Record added successfully')", true);
                mPopUpCategory.Show();
            }
            else if (btnSaveCategory.Text == "Update")
            {
                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }

                mController.UpdateHierarchy(Constants.SKUCategory, Convert.ToInt32(hfcategoryId.Value),1,
                    txtCategoryCode.Text, txtCategoryName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                    Constants.IntNullValue, hidSKUImageName.Value);
                ClearAll();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Record updated successfully')", true);
                mPopUpCategory.Hide();
            }
            //LoadParentCategories();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpCategory.Show();
        }
    }
    private void ClearAll()
    {
        txtCategoryCode.Text = "";
        txtCategoryName.Text = "";
        hidSKUImageName.Value = "";
        hidSKUImageSource.Value = "";
        LoadGrid("");
        btnSaveCategory.Text = "Save";
    }
    private string GetAutoCode(string PreeFix, int CodeType)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        return AutoCode.GetAutoCode(PreeFix, CodeType, Constants.LongNullValue);
    }

    private void SetAutoCode(string PreeFix, long CValue)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        string result = AutoCode.GetAutoCode(PreeFix, 1, CValue);
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        //LoadGrid("");
        //LoadParentCategories();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        ClearAll();
        drpParentCategory.SelectedIndex = 0;
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
        //SearchCategroy();
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdCategory.Rows)
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

            foreach (GridViewRow dr in GrdCategory.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[5].Text) == "Active")
                    {


                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 5);

                        flag = true;
                    }
                    else
                    {


                        _UserCtrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 5);


                        flag = true;
                    }

                }

                if (flag)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
                }
                LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();
        mPopUpCategory.Hide();
        drpParentCategory.SelectedIndex = 0;
    }

    

    private void UploadImage()
    {
        try
        {
            if (hidSKUImageSource.Value.Length > 0)
            {
                string imageExtention = Path.GetExtension(hidSKUImageName.Value);
                hidSKUImageName.Value = "Promotion_" + Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhmmssfff") + imageExtention;
                string file_name = hidSKUImageName.Value;
                if (file_name.Length == 0) return;
                string file_path = Server.MapPath("../UserImages/Category");
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
}