using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;


public partial class Forms_frmCustomerReceipt : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly LedgerController LController = new LedgerController();
    readonly BankController _bnkController = new BankController();
    readonly FinancialYearController _yearController = new FinancialYearController();
    readonly LedgerController _ledgerCtl = new LedgerController();
    readonly SkuHierarchyController _skuHierarchyController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetFinancialYear();
            LoadAccountHead();
            LoadDistributor();
            LoadAccountDetail();
            LoadData();
            //SelectCreditInvoice();
            LoadReceviedCheque();
            LoadVoucher();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            Session.Add("dtConfig", dtConfig);
            Session.Add("IsFinanceSetting", IsFinanceSetting);
        }
    }

    private void toggleControls(string pAccountType)
    {
        if (pAccountType == "21")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = false;
            txtStartDate.Visible = false;
            ibtnStartDate.Visible = false;
            GrdCO.Visible = true;
            GrdCheque.Visible = false;

        }
        else if (pAccountType == "33")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            lblChequeDate.Text = "Transfer Date";
            GrdCO.Visible = true;
            GrdCheque.Visible = false;
        }
        else
        {
            lblChequeNo.Visible = true;
            txtChequeNo.Visible = true;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            lblChequeDate.Text = "Cheque Date";
            GrdCO.Visible = false;
            GrdCheque.Visible = true;
        }
    }

    #region Load
    public void GetFinancialYear()
    {
        try
        {
            DataTable dt = _yearController.SelectFinancialYear(null, null, null, null, Constants.ShortNullValue, false, true, 1);
            DateTime StartDate = Convert.ToDateTime(dt.Rows[0]["dtStart"].ToString());
            DateTime EndDate = Convert.ToDateTime(dt.Rows[0]["dtEnd"].ToString());
            CalendarExtender1.StartDate = StartDate;
            CalendarExtender1.EndDate = EndDate;
            if (EndDate >= (DateTime)Session["CurrentWorkDate"])
            {
                txtTransactionDate.Text = ((DateTime)Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
            }
            else
            {
                txtTransactionDate.Text = (EndDate).ToString("dd-MMM-yyyy");
            }

            txtTransactionDate.Attributes.Add("readonly", "readonly");
        }
        catch (IndexOutOfRangeException)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please Start a financial year')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", string.Format("alert('{0}')", ex.Message), true);
        }
    }

    private void LoadPaymentRecieved()
    {
        DateTime CurrentWorkDate = (DateTime)Session["CurrentWorkDate"];
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            ChequeEntryController CController = new ChequeEntryController();
            LedgerController LController = new LedgerController();
            if (drpDistributor.Items.Count > 0)
            {
                DataSet dsReceived = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, 21, CurrentWorkDate);
                DataTable dtRealized = dsReceived.Tables[2];
                if (dtRealized.Rows.Count > 0)
                {
                    lblAmount.Text = string.Format("{0:0,0.00}", Convert.ToDecimal(dtRealized.Rows[0][0].ToString()));

                }
            }
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

        DateTime CurrentWorkDate = (DateTime)Session["CurrentWorkDate"];
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            txtTransactionDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            CalendarExtender1.EndDate = CurrentWorkDate;
        }
    }

    private void LoadData()
    {
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        DataTable dtCredit = _ledgerCtl.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()),0, Constants.LongNullValue, Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dtCredit, 0, 1, true);

        if (dtCredit.Rows.Count > 0)
        {
            ddlCustomer.SelectedIndex = 0;
        }
    }

    private void LoadVoucher()
    {
        DataTable dt = _skuHierarchyController.DDLVoucherNumber(Constants.IntNullValue, null, 1);
        clsWebFormUtil.FillDxComboBoxList(ddlVoucher, dt, "id", "Voucher_no", true);
        if (dt.Rows.Count > 0)
        {
            ddlVoucher.SelectedIndex = 0;
        }
        else
        {
            ddlVoucher.Attributes.Add("--Select--", Constants.IntNullValue.ToString());
        }
    }
    private void LoadReceviedCheque()
    {
        DateTime CurrentWorkDate = (DateTime)Session["CurrentWorkDate"];

        if (CurrentWorkDate != Constants.DateNullValue)
        {
            ChequeEntryController CController = new ChequeEntryController();
            GrdCheque.DataSource = null;
            GrdCheque.DataBind();
            GrdCO.DataSource = null;
            GrdCO.DataBind();
            decimal chqAmount = 0;
            if (DrpAccountType.SelectedItem.Value.ToString() == "21" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                if (drpDistributor.Items.Count > 0)
                {
                    DataTable dt = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedItem.Value.ToString()), Constants.IntNullValue, int.Parse(DrpAccountType.SelectedItem.Value.ToString()),CurrentWorkDate, CurrentWorkDate);
                    Session.Add("dt", dt);
                    GrdCO.DataSource = dt;
                    GrdCO.DataBind();
                    if (dt != null)
                    {
                        foreach (DataRow gvr in dt.Rows)
                        {
                            chqAmount += Convert.ToDecimal(gvr["Balance"]);
                        }
                        lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                    }
                }
            }
        }
    }

    private void checkDuplication()
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("VENDOR_ID = '" + ddlCustomer.SelectedItem.Value.ToString() + "' and CHEQUE_NO='" + txtChequeNo.Text + "'");

        if (foundRows.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Cheque No Already exist against this vendor!');", true);
        }
    }

    private void SelectCreditInvoice()
    {
        LedgerController CDC = new LedgerController();

        GrdCredit.DataSource = null;
        GrdCredit.DataBind();

        if (ddlCustomer.Items.Count > 0)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()),Constants.IntNullValue, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
            CheckBox cb = null;
            decimal TotalAmount = 0;
            foreach (GridViewRow gvr in GrdCredit.Rows)
            {
                cb = gvr.Cells[0].FindControl("ChbIsAssigned") as CheckBox;
                cb.Checked = true;
                TotalAmount += Convert.ToDecimal(gvr.Cells[4].Text);
            }
            txtTotalCreditAmount.Text = TotalAmount.ToString();
        }
    }

    private void LoadAccountHead()
    {
        try
        {
            Configuration.GetAccountHead();
        }
        catch (Exception)
        {
        }
        AccountHeadController mAccountController = new AccountHeadController();

        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, 2, Constants.AC_CashInHandAccountHead);
            clsWebFormUtil.FillDxComboBoxList(DrpBankAccount, dt, 0, 4, true);

            if (dt.Rows.Count > 0)
            {
                DrpBankAccount.SelectedIndex = 0;
            }
        }
        else
        {
            DataTable dt = mAccountController.SelectAccountHeadByMapping(Constants.AC_AccountHeadId, Constants.LongNullValue, 2, Constants.AC_BankAccountHead);
            clsWebFormUtil.FillDxComboBoxList(DrpBankAccount, dt, 0, 4, true);

            if (dt.Rows.Count > 0)
            {
                DrpBankAccount.SelectedIndex = 0;
            }
        }

    }

    private void LoadAccountDetail()
    {
        AccountHeadController mAccountController = new AccountHeadController();

        DataTable dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue, 1);
        clsWebFormUtil.FillDxComboBoxList(this.ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_NAME", true);

        if (dtHead.Rows.Count > 0)
        {
            ddlAccountHead.SelectedIndex = 0;
        }
    }

    #endregion

    private void CashAdvance(DateTime CurrentWorkDate)
    {
        string remarks = "Cash " + txtRemarks.Text;
        DateTime chqDate = Constants.DateNullValue;

        int VoucherType = Constants.CashPayment_Voucher;

        if (DrpAccountType.SelectedItem.Value.ToString() == "33")
        {
            chqDate = Convert.ToDateTime(txtStartDate.Text);
            VoucherType = Constants.Expanse_Voucher;
            remarks = "Online Transfer " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);

        Session.Add("VoucherNo", MaxDocumentID);

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, decimal.Parse(dc.chkNull_0(txtAmount.Text)),
                       CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                       null, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, "", chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "" );

        LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentID), long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), decimal.Parse(dc.chkNull_0(txtAmount.Text)), 0,
                         CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), Constants.Document_Purchase,
                         null, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, "", chqDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");
    }

    private void ChequeRealization(DateTime CurrentWorkDate)
    {
        DateTime ChequeDate = Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                ChequeDate = DateTime.Parse(txtStartDate.Text);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return;
        }

        LedgerController LController = new LedgerController();
        string MaxDocumentId = "";
        int VoucherType = Constants.Expanse_Voucher;

        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        string remarks = "";


        if (DrpAccountType.SelectedItem.Value.ToString() == "18")//Cheque
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Chq# " + txtChequeNo.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "33")//Online
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Online Transfer " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")//Cash
        {
            VoucherType = Constants.Cash_Voucher;
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0);
            remarks = "Cash " + txtRemarks.Text;
        }
        Session.Add("VoucherNo", MaxDocumentId);

        DataRow[] drConfig = null;
        DataTable dtConfig = (DataTable)Session["dtConfig"];
        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());
        string ChequeNo = null;
        if (DrpAccountType.SelectedIndex == 0)
        {
            ChequeNo = txtChequeNo.Text;
        }
        string manualNo = "";
        //foreach (GridViewRow dr in GrdCredit.Rows)
        //{
            LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), PayableAccount, int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, OfferAmount,
            CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
            ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(ddlVoucher.Value.ToString()), manualNo, Constants.IntNullValue, "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

            LController.PostingCash_Bank_Account(VoucherType, long.Parse(MaxDocumentId), Convert.ToInt32(DrpBankAccount.SelectedItem.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, 0,
            CurrentWorkDate, remarks, DateTime.Now, int.Parse(ddlCustomer.SelectedItem.Value.ToString()), 0,
            ChequeNo, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(ddlVoucher.Value.ToString()), manualNo, Constants.IntNullValue, "", ChequeDate, int.Parse(DrpAccountType.SelectedItem.Value.ToString()), "");

            LController.UpdateLedgerInvoice(Convert.ToInt64(ddlVoucher.Value.ToString()), int.Parse(drpDistributor.SelectedItem.Value.ToString()), OfferAmount, Constants.IntNullValue);

        //}
    }

    protected void GrdCheque_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
        CController.DeleteChequeEntry(long.Parse(GrdCheque.Rows[e.RowIndex].Cells[0].Text));
        LoadReceviedCheque();
    }

    protected void GrdCheque_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            hfChequeNo.Value = "";
            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdCheque.Rows[e.NewEditIndex].Cells[0].Text;
            ddlAccountHead.Value = GrdCheque.Rows[e.NewEditIndex].Cells[13].Text;
            ddlCustomer.Value = GrdCheque.Rows[e.NewEditIndex].Cells[1].Text;
            DrpBankAccount.Value = GrdCheque.Rows[e.NewEditIndex].Cells[9].Text;
            txtChequeNo.Text = GrdCheque.Rows[e.NewEditIndex].Cells[3].Text.ToString();
            txtStartDate.Text = Convert.ToDateTime(GrdCheque.Rows[e.NewEditIndex].Cells[4].Text).ToString("dd-MMM-yyyy");
            txtReceivedDate.Text = GrdCheque.Rows[e.NewEditIndex].Cells[5].Text;
            txtAmount.Text = GrdCheque.Rows[e.NewEditIndex].Cells[6].Text;
            txtRemarks.Text = GrdCheque.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
            ChkIsDiscount.Checked = Convert.ToBoolean(GrdCheque.Rows[e.NewEditIndex].Cells[11].Text);
            txtDiscount.Text = GrdCheque.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
            txtTax.Text = GrdCheque.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
            btnSave.Text = "Update";
            //SelectCreditInvoice();
            if (DrpAccountType.SelectedItem.Value.ToString() == "18")
            {
                DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);
                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    chRelized.Checked = false;
                    foreach (DataRow dbr in dt.Rows)
                    {
                        if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                        {
                            chRelized.Checked = true;
                        }
                    }
                }
            }
            hfChequeNo.Value = GrdCheque.Rows[e.NewEditIndex].Cells[3].Text.ToString();
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    #region Sel/Index Change

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Session["dtVendor"] != null)
        //{
        //    DataTable dt = (DataTable)Session["dtVendor"];
        //    DataRow[] foundRows = dt.Select("VENDOR_ID  = '" + ddlCustomer.SelectedValue + "'");
        //    if (foundRows.Length > 0)
        //    {
        //        hfVendorType.Value=Convert.ToString(foundRows[0]["vendorType"]);
        //    }
        //}
        //SelectCreditInvoice();
        LoadReceviedCheque();
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime CurrentWorkDate = (DateTime)Session["CurrentWorkDate"];

        if (CurrentWorkDate != Constants.DateNullValue)
        {
            txtTransactionDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
            CalendarExtender1.EndDate = CurrentWorkDate;
        }
        //SelectCreditInvoice();
        LoadReceviedCheque();
    }

    protected void DrpAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAccountHead();
        toggleControls(DrpAccountType.SelectedItem.Value.ToString());
        //SelectCreditInvoice();
        LoadReceviedCheque();
    }

    #endregion

    #region Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
        //    if (txtTransactionDate.Text.Length == 0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        //        return;
        //    }
            DateTime CurrentWorkDate = Convert.ToDateTime(txtTransactionDate.Text);
            ChequeEntryController CController = new ChequeEntryController();
            DateTime ChequeDate = Convert.ToDateTime(txtStartDate.Text);

            int InvoiceCount = Constants.IntNullValue;
            if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
            {
                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        InvoiceCount++;
                        break;
                    }
                }
                //if (InvoiceCount == Constants.IntNullValue)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select an invoice');", true);
                //    return;
                //}
            }
            if (btnSave.Text == "Save")
            {
                if (DrpAccountType.SelectedIndex != 0)//on Cash Realization check Invoice Selection
                {
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            InvoiceCount++;
                            break;
                        }
                    }
                    //if (InvoiceCount == Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                    //{
                    //    CashAdvance(CurrentWorkDate);
                    //    if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                    //    {
                    //        InsertGL2(null, CurrentWorkDate);
                    //    }
                    //    Session.Remove("VoucherNo");
                    //}
                    if (DrpAccountType.SelectedIndex != 0)
                    {
                        ChequeRealization(CurrentWorkDate);// used as All cash, online, cheque
                        if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                        {
                            string ChequeNo = null;
                            if (DrpAccountType.SelectedIndex == 0)
                            {
                                ChequeNo = txtChequeNo.Text;
                            }
                            InsertGL2(ChequeNo, CurrentWorkDate);
                        }
                        Session.Remove("VoucherNo");
                        //SelectCreditInvoice();
                    }
                }
                else if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
                {
                    //checkDuplication();
                    short chkDiscount = 0;
                    if (ChkIsDiscount.Checked)
                    {
                        chkDiscount = 1;
                    }
                    HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedItem.Value.ToString()), 0, Convert.ToInt32(ddlCustomer.SelectedItem.Value), txtChequeNo.Text, txtBankName.Text, ChequeDate,
                        CurrentWorkDate, Constants.DateNullValue, Constants.DateNullValue, Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)), Constants.Cheque_Clear, DateTime.Now, DrpAccountType.SelectedIndex, "", txtRemarks.Text, long.Parse(DrpBankAccount.SelectedItem.Value.ToString()), 2
                        , Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)), chkDiscount, Convert.ToDecimal(dc.chkNull_0(txtTax.Text)), Convert.ToInt32(ddlAccountHead.SelectedItem.Value.ToString()));
                    
                    CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(ddlVoucher.Value.ToString()));
                      

                    ChequeRealization(CurrentWorkDate);
                    if (Convert.ToBoolean(Session["IsFinanceSetting"]))
                    {
                        InsertGL(CurrentWorkDate);
                    }
                    txtChequeNo.Text = "";
                    Session.Remove("VoucherNo");
                    //SelectCreditInvoice();
                }
            }

            ClearAll();
            LoadReceviedCheque();
            LoadPaymentRecieved();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];
        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            switch (ddSearchType.SelectedIndex)
            {
                case 1:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 3:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 4:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                    break;
            }
            GrdCheque.DataSource = dt.DefaultView;
            GrdCheque.DataBind();
        }
        else
        {
            switch (ddSearchType.SelectedIndex)
            {
                case 1:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedItem.Value.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                    break;
            }
            GrdCO.DataSource = dt.DefaultView;
            GrdCO.DataBind();
        }
    }

    #endregion

    private void InsertGL(DateTime CurrentWorkDate)
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
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());


        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpBankAccount.SelectedItem.Text + " Paid to " + ddlCustomer.SelectedItem.Text;
        dr["DEBIT"] = 0;
        dr["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);

        //Debit Side Entry

        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = PayableAccount;
        dr1["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr1["CREDIT"] = 0;
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpBankAccount.SelectedItem.Text + " Paid to " + ddlCustomer.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);
        string MaxDocumentId = LController.SelectMaxVoucherId(Constants.Expanse_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate);
        LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), 0, MaxDocumentId, Constants.Expanse_Voucher, CurrentWorkDate, Constants.Bank_Deposit, Session["VoucherNo"].ToString(), "Suppler: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text, Constants.DateNullValue, txtChequeNo.
            Text, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.ChequePayment);
    }

    private void InsertGL2(string ChequeNo, DateTime CurrentWorkDate)
    {
        DateTime ChequeDate = Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedItem.Value.ToString() == "18" || DrpAccountType.SelectedItem.Value.ToString() == "33")
            {
                ChequeDate = DateTime.Parse(txtStartDate.Text);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return;
        }

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
        DataTable dtConfig = (DataTable)Session["dtConfig"];

        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
        int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());


        DataRow dr = dtVoucher.NewRow();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedItem.Value.ToString());
        dr["REMARKS"] = DrpAccountType.SelectedItem.Text + " received from " + ddlCustomer.SelectedItem.Text;
        dr["DEBIT"] = 0;
        dr["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);


        //Debit Side Entry

        DataRow dr1 = dtVoucher.NewRow();
        dr1["ACCOUNT_HEAD_ID"] = PayableAccount;
        dr1["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
        dr1["CREDIT"] = 0;
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpAccountType.SelectedItem.Text + " received from " + ddlCustomer.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);

        string MaxDocumentId = "";

        //using chequeDate as Transfer Date
        if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.CashPayment_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), int.Parse(ddlCustomer.SelectedItem.Value.ToString()), MaxDocumentId, Constants.CashPayment_Voucher, CurrentWorkDate, Constants.Cash_Relization, Session["VoucherNo"].ToString(), "Suppler: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text, Constants.DateNullValue, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.CashPayment);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId);
        }
        else
        {
            MaxDocumentId = LController.SelectMaxVoucherId(Constants.Expanse_Voucher, Convert.ToInt32(drpDistributor.SelectedItem.Value), CurrentWorkDate);
            LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedItem.Value.ToString()), int.Parse(ddlCustomer.SelectedItem.Value.ToString()), MaxDocumentId, Constants.Expanse_Voucher, CurrentWorkDate, Constants.Bank_Deposit, Session["VoucherNo"].ToString(), "Suppler: " + ddlCustomer.SelectedItem.Text + ", " + txtRemarks.Text, ChequeDate, ChequeNo, dtVoucher, Convert.ToInt32(Session["UserID"]), "-1", Constants.DateNullValue, true, Constants.ChequePayment);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Voucher No : " + MaxDocumentId + " saved successfully');", true);
            PrintVoucher(MaxDocumentId);
        }
    }

    private void PrintVoucher(string VoucherNo)
    {

        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpVoucherViewSupplier CrpReport = new CORNBusinessLayer.Reports.crpVoucherViewSupplier();
        int VoucherType = Constants.IntNullValue;

        string VoucherType2 = "";
        string pVoucherType = "";

        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }
        else if (DrpAccountType.SelectedItem.Value.ToString() == "21")
        {
            pVoucherType = "Cash Voucher";
            VoucherType = 24;
            VoucherType2 = "Cash Payment Voucher";
        }
        else
        {
            pVoucherType = "Bank Voucher";
            VoucherType = 17;
            VoucherType2 = "Bank Payment Voucher";
        }

        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(int.Parse(drpDistributor.SelectedItem.Value.ToString()), VoucherNo, VoucherType, Constants.IntNullValue);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
        CrpReport.SetParameterValue("VoucherType", pVoucherType);
        CrpReport.SetParameterValue("VoucherSubType", VoucherType2);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);


    }

    private void PrintChequeVoucher(string ChequeProcessId)
    {

        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        CORNBusinessLayer.Reports.crpChequeVoucher CrpReport = new CORNBusinessLayer.Reports.crpChequeVoucher();


        string VoucherType2 = "";
        string pVoucherType = "";

        if (DrpAccountType.SelectedItem.Value.ToString() == "18")
        {
            pVoucherType = "Bank Voucher";
            VoucherType2 = "Bank Payment Voucher";
        }


        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedItem.Value.ToString()));
        ds = RptAccountCtl.SelectUnpostVoucherForPrint(int.Parse(drpDistributor.SelectedItem.Value.ToString()), null, Convert.ToInt32(ChequeProcessId), 0);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("DISTRIBUTOR_NAME", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
        CrpReport.SetParameterValue("VoucherType", pVoucherType);
        CrpReport.SetParameterValue("VoucherSubType", VoucherType2);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    private void ClearAll()
    {
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtRemarks.Text = "";
        txtTax.Text = "";
        txtDiscount.Text = "";
        ChkIsDiscount.Checked = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", string.Format("alert('Error Occured: \n{0}');", ex), true);
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
    protected void GrdCO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int vendroID = Convert.ToInt32(GrdCO.Rows[e.RowIndex].Cells[0].Text);
    }

    protected void ddlVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}