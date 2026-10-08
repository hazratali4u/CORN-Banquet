using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_VerificationFinalBudget_Form : System.Web.UI.Page
{
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();
    readonly LedgerController LController = new LedgerController();
    COAMappingController _cController = new COAMappingController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadVoucher();
            DrpVoucher_SelectedIndexChanged(null, null);
            CreateTable();
            grdVerificationFinal_budget_form.ControlStyle.Font.Size = 12;
            UserController userController = new UserController();
            DataTable dt = userController.SelectSlashUser2(Convert.ToInt32(Session["UserID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                bool canPost = Convert.ToBoolean(dt.Rows[0]["CanPost"].ToString());
                if (canPost)
                    btnPost.Enabled = true;
                else
                    btnPost.Enabled = false;
            }
        }
    }

    protected void btnSave_click(object sender, ImageClickEventArgs e)
    {
        //Do Work


    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {

    }

    protected void btnOpenPopUp_Click(object sender, EventArgs e)
    {

    }

    protected void btnedit_click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        var me = int.Parse(Row.Cells[0].Text);
    }

    protected void grdVerificationChanging(object sender, GridViewPageEventArgs e)
    {
        grdVerificationFinal_budget_form.PageIndex = e.NewPageIndex;
        LoadGrid(int.Parse(DrpVoucher.SelectedItem.Value.ToString()));
    }

    private void LoadGrid(int Voucher_ID)
    {
        if (Voucher_ID != int.Parse(Constants.IntNullValue.ToString()))
        {
            DataTable dt = _mSkuController.SelectFinalBudget
                (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, Voucher_ID, 2);
            grdVerificationFinal_budget_form.DataSource = dt;
            grdVerificationFinal_budget_form.DataBind();
        }
        else
        {
            grdVerificationFinal_budget_form.DataSource = null;
            grdVerificationFinal_budget_form.DataBind();
        }
    }
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

    #region LoadCategory
    //private void LoadCategory()
    //{
    //    DataTable dt = _skuHierarchyController.DDLCategory(Constants.IntNullValue, null);
    //    clsWebFormUtil.FillDxComboBoxList(DrpCategory, dt, "SKU_HIE_ID", "SKU_HIE_NAME");
    //    if (dt.Rows.Count > 0)
    //    {
    //        DrpCategory.SelectedIndex = 0;
    //    }
    //}
    #endregion


    protected void DrpVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid(int.Parse(DrpVoucher.SelectedItem.Value.ToString()));
    }

    protected void ClearAll()
    {
        foreach (GridViewRow row in grdVerificationFinal_budget_form.Rows)
        {
            foreach (TableCell cell in row.Cells)
            {
                foreach (var control in cell.Controls)
                {
                    var box = control as TextBox;
                    if (box != null)
                    {
                        box.Text = string.Empty;
                    }
                }
            }
        }
    }

    public void CreateTable()
    {
        DataTable IVF = new DataTable();
        IVF.Columns.Add("Voucher_ID", typeof(int));
        IVF.Columns.Add("FINAL_BUDGET_ID", typeof(int));
        IVF.Columns.Add("User_ID", typeof(int));
        IVF.Columns.Add("SKU_ID", typeof(int));
        IVF.Columns.Add("CATEGORY_ID", typeof(int));
        IVF.Columns.Add("VENDOR_ID", typeof(int));
        IVF.Columns.Add("Size_Description", typeof(string));
        IVF.Columns.Add("QUANTITY", typeof(int));
        IVF.Columns.Add("Actual_Quantity", typeof(int));
        IVF.Columns.Add("RATE", typeof(decimal));
        IVF.Columns.Add("Diff_Id", typeof(int));
        IVF.Columns.Add("IsPosted", typeof(bool));
        IVF.Columns.Add("VENDOR_NAME", typeof(string));
        Session.Add("IVF", IVF);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int Code = int.Parse(DrpVoucher.SelectedItem.Value.ToString());
        if (btnSave.Text == "Save Draft")
        {
            int ProjectCode = int.Parse(DrpVoucher.SelectedItem.Value.ToString());

            foreach (GridViewRow Row in grdVerificationFinal_budget_form.Rows)
            {
                //GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
                string QUANTITY = ((System.Web.UI.WebControls.TextBox)Row.Cells[7].FindControl("QUANTITY")).Text;
                string Actual_Quantity = ((System.Web.UI.WebControls.TextBox)Row.Cells[8].FindControl("Actual_Quantity")).Text;
                string Diff_Id = ((System.Web.UI.WebControls.TextBox)Row.Cells[9].FindControl("Diff_Id")).Text;
                string rate = Row.Cells[10].Text;
                DataTable IVF = (DataTable)(Session["IVF"]);
                DataRow DR = IVF.NewRow();
                DR["Voucher_ID"] = ProjectCode;
                DR["FINAL_BUDGET_ID"] = int.Parse(Row.Cells[0].Text);
                DR["User_ID"] = int.Parse(Session["UserID"].ToString());
                DR["SKU_ID"] = int.Parse(Row.Cells[1].Text);
                DR["CATEGORY_ID"] = int.Parse(Row.Cells[2].Text);
                DR["VENDOR_ID"] = int.Parse(Row.Cells[3].Text);
                DR["VENDOR_NAME"] = Row.Cells[6].Text;
                DR["QUANTITY"] = QUANTITY;
                DR["Actual_Quantity"] = Actual_Quantity;
                DR["RATE"] = rate;
                DR["Diff_Id"] = Diff_Id;
                DR["IsPosted"] = false;
                IVF.Rows.Add(DR);
                Session.Add("IVF", IVF);
            }
            DataTable IV = (DataTable)(Session["IVF"]);

            _mSkuController.DeleteItemVerification(Constants.IntNullValue, Code, 8);
            _mSkuController.InsertItemVerification(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue
                 , true, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, IV, 1);

           DataTable dt = IV.AsEnumerable()
        .GroupBy(r => new { Col1 = r["VENDOR_ID"], Col2 = r["Voucher_ID"] })
        .Select(g => g.First())
        .CopyToDataTable();

            foreach (DataRow item in dt.Rows)
            {
               DataRow[] dr = IV.Select("VENDOR_ID = '" + item["VENDOR_ID"] + "' AND Voucher_ID = '" + item["Voucher_ID"] + "'");

                if (dr.Length > 0)
                {
                   var amount = dr.AsEnumerable().Where(x=> x.Field<int>("VENDOR_ID") == int.Parse(dr[0]["VENDOR_ID"].ToString()))
    .Sum(x => x.Field<int>("Actual_Quantity") * x.Field<decimal>("RATE"))
    .ToString();

                    InsertGL2(dr[0]["VENDOR_ID"].ToString(), amount, DateTime.Now, dr[0]["VENDOR_NAME"].ToString());
                }

            }

            IV.Clear();
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record added successfully.');", true);
            ShowReportPopUp(ProjectCode);
        }
    }
    private void InsertGL2(string vendorID, string amount, DateTime CurrentWorkDate, string vendorName)
    {
        try
        {
            Configuration.GetAccountHead();
        }
        catch (Exception)
        {
        }
        DataTable dtVoucher = new DataTable();

        dtVoucher.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));
        dtVoucher.Columns.Add("DEBIT", typeof(decimal));
        dtVoucher.Columns.Add("CREDIT", typeof(decimal));
        dtVoucher.Columns.Add("REMARKS", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));

        DataRow[] drConfig = null;
        // DataTable dtConfig = (DataTable)Session["dtConfig"];

        DataTable dtConfig = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");
        if (dtConfig.Rows.Count > 0)
        {
            int PayableAccount = 0;
            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
            if (drConfig.Length > 0)
            {
                PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
            }


            DataRow dr = dtVoucher.NewRow();
            dr["ACCOUNT_HEAD_ID"] = "20822";
            dr["REMARKS"] = " Credit to " + vendorName;
            dr["DEBIT"] = amount;
            dr["CREDIT"] = 0;
            dr["Principal_Id"] = 0;
            dtVoucher.Rows.Add(dr);


            //Debit Side Entry

            DataRow dr1 = dtVoucher.NewRow();
            dr1["ACCOUNT_HEAD_ID"] = PayableAccount;
            dr1["DEBIT"] = 0;
            dr1["CREDIT"] = amount;
            dr1["Principal_Id"] = 0;
            dr1["REMARKS"] = "Credit to " + vendorName;
            dtVoucher.Rows.Add(dr1);

            string MaxDocumentId = "";
            int VoucherType = Constants.Cash_Voucher;

            //using chequeDate as Transfer Date

            //MaxDocumentId = LController.SelectMaxVoucherId(Constants.CashPayment_Voucher, Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()), CurrentWorkDate);
            //LController.Add_Voucher(Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(vendorID), MaxDocumentId, Constants.CashPayment_Voucher, CurrentWorkDate, Constants.Cash_Relization, MaxDocumentId.ToString(), "Suppler: " + vendorName, Constants.DateNullValue, "", dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.Credit);

            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), 1);

            long LedgerID = LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), 0, decimal.Parse(amount),
                    CurrentWorkDate, "Credit to " + vendorName, DateTime.Now, int.Parse(vendorID), Constants.Document_Purchase,
                     "", int.Parse(Session["UserId"].ToString()), 0, "", 21, "", "", CurrentWorkDate);
            if (LedgerID > 0)
            {
                LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), 20822, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), decimal.Parse(amount), 0,
              CurrentWorkDate, "", DateTime.Now, int.Parse(vendorID), Constants.Document_Purchase,
              "", int.Parse(Session["UserId"].ToString()), 0, "", 21, "", "", CurrentWorkDate);
            }
        }
        }
    public void ShowReportPopUp(int savedID)
    {
        try
        {
            DocumentPrintController DPrint = new DocumentPrintController();

            DsReport2 ds = new DsReport2();
            DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));

            DataControl dc = new DataControl();
            DataTable rowsDt = _mSkuController.SelectFinalBudget
            (Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, null, null, null, null, null, null, Constants.IntNullValue, true, savedID, 2);

            foreach (DataRow dr in rowsDt.Rows)
            {
                ds.Tables["RptFinalBudget"].ImportRow(dr);
            }

            ReportDocument CrpReport = new ReportDocument();
            CrpReport = new CrpEventItemVerification();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            //const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            //Type cstype = this.GetType();
            //ClientScriptManager cs = Page.ClientScript;
            //cs.RegisterStartupScript(cstype, "OpenWindow", script);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}