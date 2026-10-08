using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_AddFinal_Budjet : System.Web.UI.Page
{
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();
    //public int /*Index*/;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSave.Text = "Save";
            LoadDataGrid("");
            CreateTable();
            LoadVoucher();
            LoadCategory();
            Internal_cost.Attributes.Add("readonly", "readonly");
            External_Code.Attributes.Add("readonly", "readonly");
            DrpCategory_SelectedIndexChanged(null, null);
            DrpItemType_SelectedIndexChanged(null, null);
            if (Session["FINAL_BUDGET_ID"] != null) 
            {
                btnEdit_Click(null, null);
            }
            if (Session["Voucher_ID"] != null)
            {
                btnEdit_Click(null, null);
                DrpVoucher.Enabled = false;
            }
            LoadDataGrid("");

            //btnSave.Attributes.Add("onclick", "return ValidateForm()");
            btnAdd.Attributes.Add("onclick", "return ValidateForm()");
        }
    }

    #region TableCreation
    public void CreateTable()
    {
        DataTable FBT = new DataTable();
        FBT.Columns.Add("Voucher_ID", typeof(int));
        FBT.Columns.Add("CATEGORY_ID", typeof(int));
        FBT.Columns.Add("Item_ID", typeof(int));
        FBT.Columns.Add("Size_Description", typeof(string));
        FBT.Columns.Add("Vendor_ID", typeof(int));
        FBT.Columns.Add("QUANTITY", typeof(string));
        FBT.Columns.Add("Rate", typeof(string));
        FBT.Columns.Add("INTERNAL_COST", typeof(string));
        FBT.Columns.Add("Margin", typeof(string));
        FBT.Columns.Add("External_Cost", typeof(string));
        FBT.Columns.Add("statuses", typeof(int));
        FBT.Columns.Add("IS_ACTIVE", typeof(bool));
        FBT.Columns.Add("TYPE_ID", typeof(int));
        FBT.Columns.Add("Voucher_Number", typeof(string));
        FBT.Columns.Add("CATEGORY_NAME", typeof(string));
        FBT.Columns.Add("VENDOR_NAME", typeof(string));
        FBT.Columns.Add("SKU_NAME", typeof(string));
        FBT.Columns.Add("STATUS", typeof(string));
        FBT.Columns.Add("User_ID", typeof(int));
        FBT.Columns.Add("FINAL_BUDGET_ID", typeof(int));
        Session.Add("FBT", FBT);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string voucherName = DrpVoucher.SelectedItem.Text;
            string CategoryName = DrpCategory.SelectedItem.Text;
            string ItemName = DrpItemType.SelectedItem.Text;
            string VendorName = DrpVendorName.SelectedItem.Text;

            int voucher = int.Parse(DrpVoucher.SelectedItem.Value.ToString());
            int mCategory_ID = int.Parse(DrpCategory.SelectedItem.Value.ToString());

            int mItem_ID = int.Parse(DrpItemType.SelectedItem.Value.ToString());
            int vender = int.Parse(DrpVendorName.SelectedItem.Value.ToString());

            if (mItem_ID == Constants.IntNullValue)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Please select Item');", true);
                return;
            }
            if (vender == Constants.IntNullValue)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Please select Supplier');", true);
                return;
            }

            DataTable FBT = (DataTable)(Session["FBT"]);
            if (btnAdd.Text == "Add")
            {
                DataRow DR = FBT.NewRow();
                DR["FINAL_BUDGET_ID"] = 0;
                DR["Voucher_ID"] = voucher;
                DR["CATEGORY_ID"] = mCategory_ID;
                DR["Item_ID"] = mItem_ID;
                DR["Size_Description"] = Size_Description.Text;
                DR["Vendor_ID"] = vender;
                DR["QUANTITY"] = Qty.Text;
                DR["Rate"] = Rate.Text;
                DR["INTERNAL_COST"] = Internal_cost.Text;
                DR["Margin"] = Margin.Text;
                DR["External_Cost"] = External_Code.Text;
                DR["IS_ACTIVE"] = true;
                DR["STATUS"] = "0";
                DR["Voucher_Number"] = voucherName;
                DR["CATEGORY_NAME"] = CategoryName;
                DR["VENDOR_NAME"] = VendorName;
                DR["SKU_NAME"] = ItemName;
                DR["User_ID"] = int.Parse(Session["UserID"].ToString());

                FBT.Rows.Add(DR);
                Session.Add("FBT", FBT);
            }
            else if(btnAdd.Text == "Update")
            {
                // For Update
                int idx = int.Parse(Session["Index"].ToString());
                var pageRecordNo = ((GridView1.PageIndex + 1) * 10) - (10 - idx);
                //int Id = int.Parse(Session["Index"].ToString());
                DataRow DR = FBT.Rows[pageRecordNo];
                DR["Voucher_ID"] = voucher;
                DR["CATEGORY_ID"] = mCategory_ID;
                DR["Item_ID"] = mItem_ID;
                DR["Size_Description"] = Size_Description.Text;
                DR["Vendor_ID"] = vender;
                DR["QUANTITY"] = Qty.Text;
                DR["Rate"] = Rate.Text;
                DR["INTERNAL_COST"] = Internal_cost.Text;
                DR["Margin"] = Margin.Text;
                DR["External_Cost"] = External_Code.Text;
                DR["IS_ACTIVE"] = true;
                DR["Voucher_Number"] = voucherName;
                DR["CATEGORY_NAME"] = CategoryName;
                DR["VENDOR_NAME"] = VendorName;
                DR["SKU_NAME"] = ItemName;
                DR["User_ID"] = int.Parse(Session["UserID"].ToString());
                Session.Add("FBT", FBT);
                btnAdd.Text = "Add";
                Session["Index"] = null;
            }
            LoadDataGrid("");
            ClearAll();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable FBT = (DataTable)(Session["FBT"]);
        int voucher = int.Parse(DrpVoucher.SelectedItem.Value.ToString());
        int mCategory_ID = int.Parse(DrpCategory.SelectedItem.Value.ToString());
        int mItem_ID = int.Parse(DrpItemType.SelectedItem.Value.ToString());
        int vender = int.Parse(DrpVendorName.SelectedItem.Value.ToString());
        try
        {
            if (Session["filter"] != null && !string.IsNullOrEmpty(Session["filter"].ToString()))
            {
                _mSkuController.InsertFinalBudget(Constants.IntNullValue, int.Parse(Session["UserID"].ToString()), mCategory_ID, vender, mItem_ID,
                   Size_Description.Text, Qty.Text, Rate.Text, Internal_cost.Text, Margin.Text,
                   External_Code.Text, true, voucher, FBT, 3);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Updated successfully.');", true);
                ClearAll();
            }
            else if (btnSave.Text == "Save")
            {
                if (FBT.Rows.Count > 0)
                {
                    _mSkuController.DeleteFinalBudget(Constants.IntNullValue,voucher, 7);

                    _mSkuController.InsertFinalBudget(Constants.IntNullValue, int.Parse(Session["UserID"].ToString()), mCategory_ID, vender, mItem_ID,
                        Size_Description.Text, Qty.Text, Rate.Text, Internal_cost.Text,
                        Margin.Text, External_Code.Text, true, voucher, FBT, 1);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                    ClearAll();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Add Data First');", true);
                    ClearAll();
                }
            }
            else if (btnSave.Text == "Update Latest")
            {
                _mSkuController.InsertFinalBudget(Constants.IntNullValue, int.Parse(Session["UserID"].ToString()), mCategory_ID, vender, mItem_ID,
                    Size_Description.Text, Qty.Text, Rate.Text, Internal_cost.Text, Margin.Text, 
                    External_Code.Text, true, voucher, FBT, 3);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Updated successfully.');", true);
                ClearAll();
            }
            if (FBT.Rows.Count > 0)
            {
                btnSave.Text = "Save";
                Session["Voucher_ID"] = null;
                Session["filter"] = null;
                FBT.Clear();
                //Showing report in the redirected page...
                Response.Redirect("~/Forms/LookUpFInalBudgetForm.aspx?LevelType=3&LevelID=314&VoucherID="+ voucher);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
    }

    protected void ClearAll()
    {
        DrpCategory.SelectedIndex = 0;
        DrpItemType.SelectedIndex = 0;
        DrpVendorName.SelectedIndex = 0;
        Size_Description.Text = "";
        Qty.Text = "";
        Rate.Text = "";
        Internal_cost.Text = "";
        Margin.Text = "";
        External_Code.Text = "";
        btnAdd.Text = "Add";
       // Session["Voucher_ID"] = null;
       // DataTable FBT = (DataTable)(Session["FBT"]);
       // FBT.Clear();
        //LoadDataGrid("");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int Voucher_ID = int.Parse(Session["Voucher_ID"].ToString());
        DataTable dt = _mSkuController.SelectFinalBudget
            (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true,Voucher_ID ,2);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        Session.Add("FBT", dt);
    }

    protected void del_Click(object sender, EventArgs e)
    {
        DataTable FBT = (DataTable)(Session["FBT"]);
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        FBT.Rows.RemoveAt(Row.RowIndex);
        LoadDataGrid("");
        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Deleted successfully.');", true);
    }

    protected void grdFinal_Budget_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    #region ClientData
    private void LoadDataGrid(string pType)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        DataTable FBT = (DataTable)(Session["FBT"]);

        GridView1.DataSource = FBT;
        GridView1.DataBind();

        if (FBT != null && FBT.Rows.Count > 0)
        {
            foreach (GridViewRow dr in GridView1.Rows)
            {
                ASPxComboBox DrpStastus = (dr.Cells[10]
                .FindControl("DrpStatusType") as ASPxComboBox);

                var status = dr.Cells[15].Text;
                DrpStastus.SelectedIndex = status == "0" ? 0 : 1;
            }

            //GridView1.PageIndex = 0;
        }
    }
    protected void grdFinalDataChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadDataGrid("");
    }
    #endregion
    private void LoadCategory()
    {
        try
        {
            DrpCategory.Items.Clear();

            //DataTable bt = _mSkuController.SelectFinalBudget
            //    (int.Parse(DrpVoucher.SelectedItem.Value.ToString()), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            //    Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true,Constants.IntNullValue ,5);

            DataTable dt = _skuHierarchyController.SelectDropdownCategory(Constants.IntNullValue, null, true);

            #region Foreachloop
            //foreach (DataRow row in bt.Rows)
            //{
            //    foreach (DataRow inner in dt.Rows)
            //    {
            //        if (int.Parse(row["CATEGORY_ID"].ToString()) == int.Parse(inner["CATEGORY_ID"].ToString()))
            //            inner.Delete();
            //    }
            //}
            //dt.AcceptChanges();
            #endregion
            DrpCategory.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            clsWebFormUtil.FillDxComboBoxList(DrpCategory, dt, "CATEGORY_ID", "SKU_HIE_NAME", false);
            if (dt.Rows.Count > 0)
            {
                DrpCategory.SelectedIndex = 0;
            }
            else
            {
                DrpCategory.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            }
        }
        catch (Exception ex)
        {
            DrpCategory.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
    }

    protected void DrpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            #region For Removing
        //    int Voucher_ID = int.Parse(Session["Voucher_ID"].ToString());
        //    DataTable bt = _mSkuController.SelectFinalBudget
        //(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
        //Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Voucher_ID, 2);
            #endregion

            DrpItemType.Items.Clear();
            DrpItemType.DataBind();
            int mCategory_ID = int.Parse(DrpCategory.SelectedItem.Value.ToString());

            DrpItemType.Items.Add("--Select--", Constants.IntNullValue.ToString());

            DataTable dt = _skuHierarchyController.SelectDropdownCategoryItems(Constants.IntNullValue, null, true, 1);
            clsWebFormUtil.FillDxComboBoxList(DrpItemType, dt, "SKU_ID", "SKU_NAME", false);

            DrpItemType.SelectedIndex = 0;
            DrpItemType_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }

    }

    protected void DrpItemType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int mItem_ID = int.Parse(DrpItemType.SelectedItem.Value.ToString());
            DrpVendorName.Items.Clear();
            DrpVendorName.DataBind();

            DrpVendorName.Items.Add("--Select--", Constants.IntNullValue.ToString());

            if (mItem_ID != int.Parse(Constants.IntNullValue.ToString()))
            {
                DataTable dI = _skuHierarchyController.SelectDropdownCategoryItems(mItem_ID, null, true, 2);
                clsWebFormUtil.FillDxComboBoxList(DrpVendorName, dI, "SKU_HIE_ID", "SKU_HIE_NAME", false);
            }

            DrpVendorName.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }

    }

    protected void LoadVoucher()
    {
        try
        {
            if (Session["Voucher_ID"] != null)
            {
                DataTable bt = _skuHierarchyController.DDLVoucherNumber(int.Parse(Session["Voucher_ID"].ToString()), null, 2);
                clsWebFormUtil.FillDxComboBoxList(DrpVoucher, bt, "id", "Voucher_no", true);
                if (bt.Rows.Count > 0)
                {
                    DrpVoucher.SelectedIndex = 0;
                }
                else
                {
                    DrpVoucher.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
                }
            }
            else
            {
                DataTable bt = new DataTable();
                bt = _mSkuController.SelectFinalBudget
                    (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                    Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Constants.IntNullValue, 6);
                DataTable dt = _skuHierarchyController.DDLVoucherNumber(Constants.IntNullValue, null, 1);

                #region Foreachloop
                List<DataRow> templist = new List<DataRow>();
                foreach (DataRow row in bt.Rows)
                {
                    foreach (DataRow inner in dt.Rows)
                    {
                        if (int.Parse(row["Id"].ToString()) == int.Parse(inner["id"].ToString()))
                            templist.Add(inner);
                    }
                }
                foreach (var row in templist)
                {
                    dt.Rows.Remove(row);
                }
                dt.AcceptChanges();
                #endregion
                clsWebFormUtil.FillDxComboBoxList(DrpVoucher, dt, "id", "Voucher_no", true);
                if (dt.Rows.Count > 0)
                {
                    DrpVoucher.SelectedIndex = 0;
                }
                else
                {
                    DrpVoucher.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            DrpVoucher.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }
    }


    protected void NewDel_Click(object sender, EventArgs e)
    {
        DataTable FBT = (DataTable)(Session["FBT"]);
        FBT.Clear();
        LoadDataGrid("");
    }

    protected void DrpVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCategory();
    }

    #region remove
    protected void btnNewEdit_Click(object sender, EventArgs e)
    {

    }
    #endregion
    protected void btnEditC_Click(object sender, EventArgs e)
    {
        Session["Index"] = null;
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        Session["Index"] = Row.RowIndex;
        //DrpVoucher.SelectedItem.Text = Row.Cells[0].Text;
        DrpVoucher.SelectedItem.Value = Row.Cells[11].Text;
        DrpCategory.SelectedItem.Text = Row.Cells[1].Text;
        DrpCategory.Value = Row.Cells[12].Text;

        DrpCategory_SelectedIndexChanged(null, null);


        DrpVendorName.SelectedItem.Text = Row.Cells[3].Text;
        DrpItemType.SelectedItem.Text = Row.Cells[4].Text;
        Size_Description.Text = Row.Cells[2].Text;
        DrpItemType.Value = Row.Cells[14].Text;

        DrpItemType_SelectedIndexChanged(null, null);

        DrpVendorName.Value = Row.Cells[13].Text;
        Qty.Text = Row.Cells[5].Text;
        Rate.Text = Row.Cells[6].Text;
        Internal_cost.Text = Row.Cells[7].Text;
        Margin.Text = Row.Cells[8].Text;
        External_Code.Text = Row.Cells[9].Text;
        btnAdd.Text = "Update";
    }

    protected void LookUp_Click(object sender, EventArgs e)
    {
        ClearAll();
        Session["Voucher_ID"] = null;
        Session["filter"] = null;
    }
    Int64 InternalTotal;
    Double ExternalTotal;
    protected void Total_Click(object sender, EventArgs e)
    {
        if (Session["Voucher_ID"] != null)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                InternalTotal += Convert.ToInt64(row.Cells[7].Text);
            }
            foreach (GridViewRow row in GridView1.Rows)
            {
                ExternalTotal += Convert.ToDouble(row.Cells[9].Text);
            }
            txtInternal.Text = InternalTotal.ToString();
            txtExternal.Text = ExternalTotal.ToString();
            // GridView1.FooterRow.Cells[2].Text = "Total";
            //GridView1.FooterRow.Cells[3].Text = GrandTotal.ToString();
        }
        else
        {
            
        }
    }
    protected void DrpStatusType_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            ASPxComboBox ddl = (ASPxComboBox)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            row.Cells[15].Text = ddl.Value.ToString();

            var pageRecordNo = ((GridView1.PageIndex + 1) * 10) - (10 - idx);

            DataTable FBT = (DataTable)(Session["FBT"]);
            FBT.Rows[pageRecordNo]["STATUS"] = row.Cells[15].Text;

            LoadDataGrid("");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
            int Voucher_ID = int.Parse(Session["Voucher_ID"].ToString());
            DataTable dt = _mSkuController.SelectFinalBudget
                (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Voucher_ID, 2);
            GridView1.DataSource = dt;
            GridView1.DataBind();

            DataTable originalDt = dt;
            DataRow[] dr = originalDt.Select("CATEGORY_NAME LIKE '%" + txtSearch.Text + "%'");

            if (dr == null || dr.Length == 0)
            {
                dr = originalDt.Select("VENDOR_NAME LIKE '%" + txtSearch.Text + "%'");
            }
            if (dr == null || dr.Length == 0)
            {
                dr = originalDt.Select("SKU_NAME LIKE '%" + txtSearch.Text + "%'");
            }
            if (dr == null || dr.Length == 0)
            {
                dr = originalDt.Select("Size_Description LIKE '%" + txtSearch.Text + "%'");
            }
            if (dr == null || dr.Length == 0)
            {
                if (txtSearch.Text.ToString().ToLower().Contains("pending"))
                    dr = originalDt.Select("Status = '0'");
                else if (dr == null || dr.Length == 0)
                {
                    dr = originalDt.Select("Status = '1'");
                }
            }

            dt = dr.CopyToDataTable();

            if (dt.Rows.Count > 0)
            {
                Session.Add("FBT", dt);
                Session.Add("filter", "searched");
            }
        }
        else
        {
            int Voucher_ID = int.Parse(Session["Voucher_ID"].ToString());
            DataTable dt = _mSkuController.SelectFinalBudget
                (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Voucher_ID, 2);
            GridView1.DataSource = dt;
            GridView1.DataBind();

            Session.Add("FBT", dt);
            Session["filter"] = null;
        }

        LoadDataGrid("filter");
    }
}