using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;
using System.Web.UI;

/// <summary>
/// From To Add, Edit, Delete SKU Hierarchy
/// </summary>
public partial class frmSKUHierarchyDataFashion : System.Web.UI.Page
{
    readonly SkuHierarchyController _mController = new SkuHierarchyController();


    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            btnSavePrincipal.Attributes.Add("onclick", "return ValidateForm()");
            btnSaveCategory.Attributes.Add("onclick", "return ValidateForm()");
            btnSaveSubCategory.Attributes.Add("onclick", "return ValidateForm()");
            btnSaveBrand.Attributes.Add("onclick", "return ValidateForm()");
            btnSaveGender.Attributes.Add("onclick", "return ValidateForm()");

            DataTable dtPCategory = new DataTable();
            dtPCategory = _mController.SelectPrincipal(Constants.SKUCategory, int.Parse(this.Session["CompanyId"].ToString()));
            int i = 0;
            foreach (DataRow row in dtPCategory.Rows)
            {
                ddlParentCategory.Items.Insert(i, new ListItem(row["SKU_HIE_NAME"].ToString(), row["SKU_HIE_ID"].ToString()));
                i++;
            }
            ddlParentCategory.SelectedValue = "0";


            LoadGrid("");
        }
    }
    private void LoadGrid(string pType)
    {
        GrdDivision.DataSource = null;
        GrdDivision.DataBind();

        GrdCategory.DataSource = null;
        GrdCategory.DataBind();

        GrdSubCategory.DataSource = null;
        GrdSubCategory.DataBind();

        GrdBrand.DataSource = null;
        GrdBrand.DataBind();

        GrdGender.DataSource = null;
        GrdGender.DataBind();

        DataTable dt = new DataTable();
        DataTable dtCategory = new DataTable();
        DataTable dtSubCategory = new DataTable();
        DataTable dtBrand = new DataTable();
        DataTable dtGender = new DataTable();

        dt = _mController.SelectPrincipal(Constants.SKUDivision, int.Parse(this.Session["CompanyId"].ToString()));
        dtCategory = _mController.SelectPrincipal(Constants.SKUCategory, int.Parse(this.Session["CompanyId"].ToString()));
        dtSubCategory = _mController.SelectPrincipal(Constants.SKUSubCategory, int.Parse(this.Session["CompanyId"].ToString()));
        
        dtBrand = _mController.SelectPrincipal(Constants.SKUBrand, int.Parse(this.Session["CompanyId"].ToString()));
        dtGender = _mController.SelectPrincipal(Constants.SKUGender, int.Parse(this.Session["CompanyId"].ToString()));

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            GrdDivision.DataSource = dt;
            GrdDivision.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                GrdDivision.PageIndex = 0;
            }
            GrdDivision.DataSource = dt;
            GrdDivision.DataBind();
        }

        ////Category
        if (pType == "")
        {
            if (txtSearchCategory.Text != "" || txtSearchCategory.Text != string.Empty)
            {
                dtCategory.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchCategory.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchCategory.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchCategory.Text + "%'";
            }
            GrdCategory.DataSource = dtCategory;
            GrdCategory.DataBind();
            


        }
        else
        {
            if (txtSearchCategory.Text != "" || txtSearchCategory.Text != string.Empty)
            {
                dtCategory.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchCategory.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchCategory.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchCategory.Text + "%'";
            }
            if (dtCategory.Rows.Count > 0)
            {
                GrdCategory.PageIndex = 0;
            }
            GrdCategory.DataSource = dtCategory;
            GrdCategory.DataBind();
        }

        ////Sub Category
        if (pType == "")
        {
            if (txtSearchSubCategory.Text != "" || txtSearchSubCategory.Text != string.Empty)
            {
                dtSubCategory.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchSubCategory.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchSubCategory.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchSubCategory.Text + "%'";
            }
            GrdSubCategory.DataSource = dtSubCategory;
            GrdSubCategory.DataBind();

            
        }
        else
        {
            if (txtSearchSubCategory.Text != "" || txtSearchSubCategory.Text != string.Empty)
            {
                dtSubCategory.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchSubCategory.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchSubCategory.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchSubCategory.Text + "%'";
            }
            if (dtSubCategory.Rows.Count > 0)
            {
                GrdSubCategory.PageIndex = 0;
            }
            GrdSubCategory.DataSource = dtSubCategory;
            GrdSubCategory.DataBind();
        }
        ////Brand
        if (pType == "")
        {
            if (txtSearchBrand.Text != "" || txtSearchBrand.Text != string.Empty)
            {
                dtBrand.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchBrand.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchBrand.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchBrand.Text + "%'";
            }
            GrdBrand.DataSource = dtBrand;
            GrdBrand.DataBind();
        }
        else
        {
            if (txtSearchBrand.Text != "" || txtSearchBrand.Text != string.Empty)
            {
                dtBrand.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchBrand.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchBrand.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchBrand.Text + "%'";
            }
            if (dtBrand.Rows.Count > 0)
            {
                GrdBrand.PageIndex = 0;
            }
            GrdBrand.DataSource = dtBrand;
            GrdBrand.DataBind();
        }

        ////Gender
        if (pType == "")
        {
            if (txtSearchGender.Text != "" || txtSearchGender.Text != string.Empty)
            {
                dtGender.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchGender.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchGender.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchGender.Text + "%'";
            }
            GrdGender.DataSource = dtGender;
            GrdGender.DataBind();
        }
        else
        {
            if (txtSearchGender.Text != "" || txtSearchGender.Text != string.Empty)
            {
                dtGender.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearchGender.Text + "%' OR CONTACT_PERSON LIKE '%" + txtSearchGender.Text + "%' OR IS_ACTIVE LIKE '" + txtSearchGender.Text + "%'";
            }
            if (dtGender.Rows.Count > 0)
            {
                GrdGender.PageIndex = 0;
            }
            GrdGender.DataSource = dtGender;
            GrdGender.DataBind();
        }
    }

    protected void GrdSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfSubCategoryID.Value = GrdSubCategory.Rows[e.NewEditIndex].Cells[1].Text;
        txtSubCategoryName.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[3].Text;
        txtAddressSubCategory.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");

        txtContactPersonSubCategory.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");

        txtEmailSubCategory.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtPhoneNumberSubCategory.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtFaxNumberSubCategory.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        hfStatusSubCategory.Value = GrdSubCategory.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");

        

        ddlParentCategory.SelectedValue = GrdSubCategory.Rows[e.NewEditIndex].Cells[12].Text;

        btnSaveSubCategory.Text = "Update";
        myModalLabelSubCategory.InnerText = "Edit Supplier Information";
        mPopUpLocationSubCategory.Show();
    }
    protected void GrdCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        
        hfCategoryID.Value = GrdCategory.Rows[e.NewEditIndex].Cells[1].Text;
        txtCategoryName.Text = GrdCategory.Rows[e.NewEditIndex].Cells[3].Text;
        txtAddress.Text = GrdCategory.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");

        txtContactPersonCategory.Text = GrdCategory.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");

        txtEmailCategory.Text = GrdCategory.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtPhoneNumberCategory.Text = GrdCategory.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtFaxNumberCategory.Text = GrdCategory.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        hfStatusCategory.Value = GrdCategory.Rows[e.NewEditIndex].Cells[10].Text;

        btnSaveCategory.Text = "Update";
        myModalLabelCategory.InnerText = "Edit Supplier Information";
        mPopUpLocationCategory.Show();

    }

    protected void GrdDivision_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfPrincipalId.Value = GrdDivision.Rows[e.NewEditIndex].Cells[1].Text;
        txtPrincipalName.Text = GrdDivision.Rows[e.NewEditIndex].Cells[3].Text;


        txtAddress.Text = GrdDivision.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        txtContactPerson.Text = GrdDivision.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
        txtEmail.Text = GrdDivision.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtPhoneNumber.Text = GrdDivision.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtFaxNumber.Text = GrdDivision.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        hfStatus.Value = GrdDivision.Rows[e.NewEditIndex].Cells[10].Text;

        btnSavePrincipal.Text = "Update";
        myModalLabel.InnerText = "Edit Supplier Information";
        mPopUpLocation.Show();
    }


    protected void GrdBrand_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfBrandId.Value = GrdBrand.Rows[e.NewEditIndex].Cells[1].Text;
        txtBrandName.Text = GrdBrand.Rows[e.NewEditIndex].Cells[3].Text;


        txtAddressBrand.Text = GrdBrand.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        txtContactPersonBrand.Text = GrdBrand.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
        txtEmailBrand.Text = GrdBrand.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtPhoneNumberBrand.Text = GrdBrand.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtFaxNumberBrand.Text = GrdBrand.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        hfStatusBrand.Value = GrdBrand.Rows[e.NewEditIndex].Cells[10].Text;

        btnSaveBrand.Text = "Update";
        myModalLabelBrand.InnerText = "Edit Supplier Information";
        mPopUpLocationBrand.Show();


    }

    protected void GrdGender_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfGenderId.Value = GrdGender.Rows[e.NewEditIndex].Cells[1].Text;
        txtGenderName.Text = GrdGender.Rows[e.NewEditIndex].Cells[3].Text;


        txtAddressGender.Text = GrdGender.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        txtContactPersonGender.Text = GrdGender.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
        txtEmailGender.Text = GrdGender.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtPhoneNumberGender.Text = GrdGender.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtFaxNumberGender.Text = GrdGender.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        hfStatusGender.Value = GrdGender.Rows[e.NewEditIndex].Cells[10].Text;

        btnSaveGender.Text = "Update";
        myModalLabelGender.InnerText = "Edit Supplier Information";
        mPopUpLocationGender.Show();

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        ClearAll();
    }

    protected void btnCancelCategory_Click(object sender, EventArgs e)
    {
        mPopUpLocationCategory.Show();
        ClearAllCategory();
    }

    protected void btnCancelSubCategory_Click(object sender, EventArgs e)
    {
        mPopUpLocationSubCategory.Show();
        ClearAllSubCategory();
    }

    protected void btnCancelBrand_Click(object sender, EventArgs e)
    {
        mPopUpLocationBrand.Show();
        ClearAllBrand();

    }

    protected void btnsearchGender_Click(object sender, EventArgs e)
    {
        mPopUpLocationGender.Show();
        ClearAllGender();

    }

    protected void btnsearchSubCategory_Click(object sender, EventArgs e)
    {
        mPopUpLocationSubCategory.Show();
        ClearAllSubCategory();

    }
    protected void btnsearchBrand_Click(object sender, EventArgs e)
    {
        mPopUpLocationBrand.Show();
        ClearAllBrand();

    }

   
    protected void btnSavePrincipal_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                mPopUpLocation.Show();
                lblErrorMsg.Text = "";
                switch (btnSavePrincipal.Text)
                {

                    case "Save":

                        _mController.InsertPrincipal(Constants.SKUDivision, Constants.IntNullValue, "", txtPrincipalName.Text,
                            null, true, int.Parse(this.Session["CompanyId"].ToString()), true,
                            txtAddress.Text, txtContactPerson.Text, txtEmail.Text, txtPhoneNumber.Text, txtFaxNumber.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocation.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatus.Value != "Active")
                        {
                            status = false;
                        }
                        _mController.UpdatePrincipal(Constants.SKUDivision, int.Parse(hfPrincipalId.Value), Constants.IntNullValue, "",
                            txtPrincipalName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                            true, txtAddress.Text, txtContactPerson.Text, txtEmail.Text, txtPhoneNumber.Text, txtFaxNumber.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        mPopUpLocation.Hide();
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

    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                mPopUpLocationCategory.Show();
                lblErrorMsg.Text = "";
                switch (btnSaveCategory.Text)
                {

                    case "Save":

                        _mController.InsertPrincipal(Constants.SKUCategory, Constants.IntNullValue, "", txtCategoryName.Text,
                            null, true, int.Parse(this.Session["CompanyId"].ToString()), true,
                            txtAddressCategory.Text, txtContactPerson.Text, txtEmailCategory.Text, txtPhoneNumberCategory.Text, txtFaxNumberCategory.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocationCategory.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatusCategory.Value != "Active")
                        {
                            status = false;
                        }
                        _mController.UpdatePrincipal(Constants.SKUCategory, int.Parse(hfCategoryID.Value), Constants.IntNullValue, "",
                            txtCategoryName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                            true, txtAddressCategory.Text, txtContactPersonCategory.Text, txtEmailCategory.Text, txtPhoneNumberCategory.Text, txtFaxNumberCategory.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        mPopUpLocationCategory.Hide();
                        break;
                }
                ClearAllCategory();
                LoadGrid("");
            }
            else
            {
                mPopUpLocationCategory.Show();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpLocation.Show();
        }

    }

    protected void btnSaveSubCategory_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                mPopUpLocationSubCategory.Show();
                lblErrorMsgSubCategory.Text = "";
                switch (btnSaveSubCategory.Text)
                {

                    case "Save":

                        _mController.InsertPrincipal(Constants.SKUSubCategory, Int32.Parse(ddlParentCategory.SelectedValue), "", txtSubCategoryName.Text,
                            null, true, int.Parse(this.Session["CompanyId"].ToString()), true,
                            txtAddressSubCategory.Text, txtContactPersonSubCategory.Text, txtEmailSubCategory.Text, txtPhoneNumberSubCategory.Text, txtFaxNumberSubCategory.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocationSubCategory.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatusSubCategory.Value != "Active")
                        {
                            status = false;
                        }
                        _mController.UpdatePrincipal(Constants.SKUSubCategory, int.Parse(hfSubCategoryID.Value), Int32.Parse(ddlParentCategory.SelectedValue), "",
                            txtSubCategoryName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                            true, txtAddressSubCategory.Text, txtContactPersonSubCategory.Text, txtEmailSubCategory.Text, txtPhoneNumberSubCategory.Text, txtFaxNumberSubCategory.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        mPopUpLocationSubCategory.Hide();
                        break;
                }
                ClearAllCategory();
                LoadGrid("");
            }
            else
            {
                mPopUpLocationSubCategory.Show();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpLocationSubCategory.Show();
        }

    }

    protected void btnSaveBrand_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                mPopUpLocationBrand.Show();
                lblErrorMsgBrand.Text = "";
                switch (btnSaveBrand.Text)
                {

                    case "Save":

                        _mController.InsertPrincipal(Constants.SKUBrand, Constants.IntNullValue, "", txtBrandName.Text,
                            null, true, int.Parse(this.Session["CompanyId"].ToString()), true,
                            txtAddressBrand.Text, txtContactPersonBrand.Text, txtEmailBrand.Text, txtPhoneNumberBrand.Text, txtFaxNumberBrand.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocationBrand.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatusBrand.Value != "Active")
                        {
                            status = false;
                        }
                        _mController.UpdatePrincipal(Constants.SKUBrand, int.Parse(hfBrandId.Value), Constants.IntNullValue, "",
                            txtBrandName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                            true, txtAddressBrand.Text, txtContactPersonBrand.Text, txtEmailBrand.Text, txtPhoneNumberBrand.Text, txtFaxNumberBrand.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        mPopUpLocationBrand.Hide();
                        break;
                }
                ClearAllBrand();
                LoadGrid("");
            }
            else
            {
                mPopUpLocationBrand.Show();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpLocationBrand.Show();
        }

    }
    protected void btnSaveGender_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                mPopUpLocationGender.Show();
                lblErrorMsgGender.Text = "";
                switch (btnSaveGender.Text)
                {

                    case "Save":

                        _mController.InsertPrincipal(Constants.SKUGender, Constants.IntNullValue, "", txtGenderName.Text,
                            null, true, int.Parse(this.Session["CompanyId"].ToString()), true,
                            txtAddressGender.Text, txtContactPersonGender.Text, txtEmailGender.Text, txtPhoneNumberGender.Text, txtFaxNumberGender.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);


                        mPopUpLocationGender.Show();
                        break;

                    case "Update":

                        bool status = true;
                        if (hfStatusGender.Value != "Active")
                        {
                            status = false;
                        }
                        _mController.UpdatePrincipal(Constants.SKUGender, int.Parse(hfBrandId.Value), Constants.IntNullValue, "",
                            txtGenderName.Text, null, status, int.Parse(this.Session["CompanyId"].ToString()),
                            true, txtAddressGender.Text, txtContactPersonGender.Text, txtEmailGender.Text, txtPhoneNumberGender.Text, txtFaxNumberGender.Text);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        mPopUpLocationGender.Hide();
                        break;
                }
                ClearAllGender();
                LoadGrid("");
            }
            else
            {
                mPopUpLocationGender.Show();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpLocationGender.Show();
        }

    }





    private void ClearAll()
    {
        txtPrincipalName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtEmail.Text = "";
        txtPhoneNumber.Text = "";
        txtFaxNumber.Text = "";
        lblErrorMsg.Text = "";
        btnSavePrincipal.Text = "Save";
        myModalLabel.InnerText = "Add New Division";
        hfPrincipalId.Value = "";

    }

    private void ClearAllCategory()
    {
        

        txtCategoryName.Text = "";
        txtAddressCategory.Text = "";
        txtContactPersonCategory.Text = "";
        txtEmailCategory.Text = "";
        txtPhoneNumberCategory.Text = "";
        txtFaxNumberCategory.Text = "";
        lblErrorMsg.Text = "";
        btnSaveCategory.Text = "Save";
        myModalLabelCategory.InnerText = "Add New Category";
        hfCategoryID.Value = "";

    }

    private void ClearAllSubCategory()
    {


        txtSubCategoryName.Text = "";
        txtAddressSubCategory.Text = "";
        txtContactPersonSubCategory.Text = "";
        txtEmailSubCategory.Text = "";
        txtPhoneNumberSubCategory.Text = "";
        txtFaxNumberSubCategory.Text = "";
        lblErrorMsgSubCategory.Text = "";
        btnSaveSubCategory.Text = "Save";
        myModalLabelSubCategory.InnerText = "Add New Supplier";
        hfSubCategoryID.Value = "";

    }

    private void ClearAllGender()
    {


        txtGenderName.Text = "";
        txtAddressGender.Text = "";
        txtContactPersonGender.Text = "";
        txtEmailGender.Text = "";
        txtPhoneNumberGender.Text = "";
        txtFaxNumberGender.Text = "";
        lblErrorMsgGender.Text = "";
        btnSaveGender.Text = "Save";
        myModalLabelGender.InnerText = "Add New Gender";
        hfGenderId.Value = "";

    }

    private void ClearAllBrand()
    {


        txtBrandName.Text = "";
        txtAddressBrand.Text = "";
        txtContactPersonBrand.Text = "";
        txtEmailBrand.Text = "";
        txtPhoneNumberBrand.Text = "";
        txtFaxNumberBrand.Text = "";
        lblErrorMsgBrand.Text = "";
        btnSaveBrand.Text = "Save";
        myModalLabelBrand.InnerText = "Add New Brand";
        hfBrandId.Value = "";

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

    protected void btnFilterSearch_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnCloseCategory_ServerClick(object sender, EventArgs e)
    {
        ClearAllCategory();
    }

    protected void btnCloseSubCategory_Click(object sender, EventArgs e)
    {
        ClearAllSubCategory();
    }
    protected void btnCloseGender_Click(object sender, EventArgs e)
    {
        ClearAllGender();

    }

    protected void btnCloseBrand_Click(object sender, EventArgs e)
    {
        ClearAllBrand();
    }

    protected void btnActiveCategory_Click(object sender, EventArgs e)
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

                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
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
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdDivision.Rows)
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

            foreach (GridViewRow dr in GrdDivision.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
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
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    protected void btnActiveSubCategory_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdSubCategory.Rows)
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

            foreach (GridViewRow dr in GrdSubCategory.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
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
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }
    protected void btnActiveBrand_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdBrand.Rows)
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

            foreach (GridViewRow dr in GrdBrand.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
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
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }
    protected void btnActiveGender_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdGender.Rows)
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

            foreach (GridViewRow dr in GrdGender.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
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
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }

    }

    protected void btnAddCategory_Click(object sender, EventArgs e)
    {
        btnSaveCategory.Text = "Save";
        myModalLabel.InnerText = "Add New Category";
        mPopUpLocationCategory.Show();

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSavePrincipal.Text = "Save";
        myModalLabel.InnerText = "Add New Division";
        mPopUpLocation.Show();
    }


    protected void btnAddSubCategory_Click(object sender, EventArgs e)
    {
        btnSaveSubCategory.Text = "Save";
        myModalLabelSubCategory.InnerText = "Add New Sub Category";
        mPopUpLocationSubCategory.Show();

    }
    protected void btnAddBrand_Click(object sender, EventArgs e)
    {
        btnSaveBrand.Text = "Save";
        myModalLabelBrand.InnerText = "Add New Brand";
        mPopUpLocationBrand.Show();

    }
    protected void btnAddGender_Click(object sender, EventArgs e)
    {
        btnSaveGender.Text = "Save";
        myModalLabelGender.InnerText = "Add New Supplier";
        mPopUpLocationGender.Show();

    }

    protected void GrdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        GrdCategory.PageIndex = e.NewPageIndex;
        LoadGrid("");

    }
    protected void grdDataDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdDivision.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void grdDataSubCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdSubCategory.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void GrdGender_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdGender.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
    protected void GrdBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdBrand.PageIndex = e.NewPageIndex;
        LoadGrid("");

    }


}
