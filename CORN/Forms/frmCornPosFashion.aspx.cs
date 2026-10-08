using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

public partial class Forms_frmCornPosFashion : System.Web.UI.Page
{


    readonly CustomerDataController cController = new CustomerDataController();

    readonly DistributorController _mDist = new DistributorController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();

    readonly OrderEntryController _or = new OrderEntryController();
    readonly SKUGroupController _groupCtl = new SKUGroupController();

    readonly RptCustomerController rcc = new RptCustomerController();

    DataTable PurchaseSKU;
    
    long saleInvoiceID = 0;

    public int printType;
    public static string CompanyName;
    public static string CompanyPhonNmbr;
    private static string _hirarchyNameQuiz;

    DataControl _dc = new DataControl();

    DataTable dt;
    DataTable dt_Notes;
    DataTable dTable;
    int _noteID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadProduct();
            LoadCustomerData();
            LoadSaleForce();

            txtQuantity.Text = "1";
            txtDiscount.Text = "0";
            txtskuCode.Focus();

            lblInvoiceDate.Text = DateTime.Now.ToShortDateString();
            LoadloginDetail();
            CreditLimit();
            //GetLicense();
            txtGrossAmount.Attributes.Add("readonly", "readonly");
            numtxtTotalExtraDiscnt.Attributes.Add("readonly", "readonly");
            numTxtTotalGST.Attributes.Add("readonly", "readonly");
            numTxtTotlAmnt.Attributes.Add("readonly", "readonly");
            txtBalance.Attributes.Add("readonly", "readonly");

            LoadSlipNotes();
            //GetDocumentNo();
            //LoadDistributor();
            //LoadSkuDetail();
        }
        hfMaxId.Value = OrderEntryController.GetMaxInvoiceId();

    }

    private void LoadCustomerData()
    {
        DataTable dtCustomer = new DataTable();
        if (Cache["CustomersData"] == null)
        {
            dtCustomer = cController.SelectAllCustomer(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), Constants.IntNullValue, Constants.IntNullValue);
            clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CUSTOMER_DETAIL", true);
            ddlCustomer.Items.Insert(0, new ListItem("WalkIn Customer-FS-00001", "0"));

            if (ddlCustomer.Items.Count > 0)
            {
                ddlCustomer.SelectedIndex = 0;
                Cache["CustomersData"] = dtCustomer;
                Session.Add("dtCustomer", dtCustomer);
            }      
        }
        else
        {          
            clsWebFormUtil.FillDropDownList(ddlCustomer, (DataTable)Cache["CustomersData"], "CUSTOMER_ID", "CUSTOMER_DETAIL", true);
            ddlCustomer.Items.Insert(0, new ListItem("WalkIn Customer-FS-00001", "0"));

            if (ddlCustomer.Items.Count > 0)
            {
                ddlCustomer.SelectedIndex = 0;
                Session.Add("dtCustomer", (DataTable)Cache["CustomersData"]);
            }
        }
    }


    private void LoadSaleForce()
    {

        int customerTypeId = Constants.SALES_FORCE_SALESPERSON;

        SaleForceController mDController = new SaleForceController();
        DataTable dt = mDController.SelectSaleForceAssignedArea(customerTypeId, int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString())
            , Constants.IntNullValue, int.Parse(HttpContext.Current.Session["companyId"].ToString()), Constants.IntNullValue);

        clsWebFormUtil.FillDropDownList(ddsalesForce, dt, "USER_ID", "USER_NAME");
        ddsalesForce.SelectedValue = Convert.ToString(Session["UserId"]);
    }

 
    //private void LoadGird()
    //{
    //    if (Session["PurchaseSKU"] != null)
    //    {
    //        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
    //        GrdPurchase.DataSource = PurchaseSKU;
    //        GrdPurchase.DataBind();
    //    }
    //    else
    //    {
    //        GrdPurchase.DataSource = null;
    //        GrdPurchase.DataBind();
    //    }
    //}

    private void LoadProduct()
    {
        try
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();

            if (Cache["ProductsData"] == null)
            {
                DataTable dtProduct = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    hfProduct.Value = GetJson(dtProduct);
                    Session.Add("Dtsku_Price", dtProduct);
                    Cache["ProductsData"] = dtProduct;
                }
            }
            else
            {              
                hfProduct.Value = GetJson((DataTable)Cache["ProductsData"]);
            }                       
        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }

    public string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);

            //foreach (DataColumn col in dt.Columns)
            //{
            //    row.Add(col.ColumnName, dr[col]);
            //}
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    private void CreditLimit()
    {
        CustomerDataController cdCtrl = new CustomerDataController();

        lblCreditLimit.Text = "0";
        lblLedgerBalance.Text = "0";
        lblAllowLimit.Text = "0";

        if (ddlCustomer.Items.Count > 0)
        {
            // --working on both options credit, credit and cash
            //var customerId = ddlCustomer.SelectedItem.Value != null ? ddlCustomer.SelectedItem.Value.ToString() : string.Empty;

            DataTable dt = cdCtrl.SelectCustomerCreditBalance(Convert.ToInt64(ddlCustomer.SelectedValue), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()), Constants.Credit);

            if (dt == null) return;

            if (dt.Rows.Count < 0) return;
            {
                //This Limit is AllowLimit + Ledger Balance
                lblAllowLimit.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][0].ToString())));

                //This Limit is entered by user while adding customer
                lblCreditLimit.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][1].ToString())));

                lblLedgerBalance.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][2].ToString())));
            }
        }
    }

    private void LoadloginDetail()
    {
        try
        {
            UserController userControl = new UserController();
            DataTable dt = userControl.SelectSlashUser2(int.Parse(Session["UserId"].ToString()));
            lbllogintimedate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");//DateTime.Now.ToString("MM/dd/yyyy");
            lbluserlogin.Text = dt.Rows[0]["USER_NAME"].ToString();
            hfuserlogin.Value = lbluserlogin.Text;
            lblLoacation.Text = dt.Rows[0]["DISTRIBUTOR_NAME"].ToString();
            hfLocationName.Value = lblLoacation.Text;
            CompanyName = dt.Rows[0]["COMPANY_NAME"].ToString();
            hfCompanyName.Value = CompanyName;
            CompanyPhonNmbr = dt.Rows[0]["CONTACT_NUMBER"].ToString();//Location Contact Number
            hfContactNo.Value = "PH: " + CompanyPhonNmbr;


            AutoComplete.ContextKey = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
            Session.Add("DISTRIBUTOR_ID", dt.Rows[0]["DISTRIBUTOR_ID"].ToString());

            DataTable dt2 = userControl.SelectUserPrincipal(int.Parse(Session["UserId"].ToString()));
            if (dt2 != null)
            {
                _hirarchyNameQuiz = dt2.Rows[0]["SKU_HIE_NAME"].ToString();

                //  Session.Add("PRINCIPAL_ID", dt2.Rows[0]["PRINCIPAL_ID"].ToString());
            }

        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static void InsertInvoice(string orderedProducts, string amountDue, string author, string discount, string netAmount, 
        string paidIn,string payType, string Gst, string manualId, string customerId, string saleForce)
    {
        try
        {
            // FindCustomer(customerCode);
            DataControl _dc = new DataControl();

            var manualId2 = manualId == "SALE MODE" ? "2" : "1";

            DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            int userId = int.Parse(HttpContext.Current.Session["UserId"].ToString());

            //  int principalId = int.Parse(HttpContext.Current.Session["PRINCIPAL_ID"].ToString());
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());


            var SaleSKU = (DataTable)JsonConvert.DeserializeObject(orderedProducts, (typeof(DataTable)));


            if (SaleSKU != null && SaleSKU.Rows.Count > 0)
            {
                OrderEntryController.Add_Invoice2(distributerId, manualId2, 0, long.Parse(customerId), long.Parse(customerId), 0, Convert.ToInt32(saleForce), 0,
                        decimal.Parse(_dc.chkNull_0(amountDue)), decimal.Parse(_dc.chkNull_0(discount)), decimal.Parse(_dc.chkNull_0(paidIn)), decimal.Parse(_dc.chkNull_0(Gst)), Decimal.Parse(_dc.chkNull_0(netAmount)), 0, int.Parse(payType),
                        SaleSKU, userId, 0, currentWorkDate, 0, 0, _dc.chkNull_0(author));
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {

        //decimal NetAmount = 0;
        try
        {
            // if (FindCustomer())
            // {
            string manualId = null;

            if (hfToggleMode.Value == "SALE MODE")
            {
                manualId = "2";//for order invoice
            }
            else
            {
                manualId = "1";//for sale return
            }

            DataTable PurchaseSKU = (DataTable)JsonConvert.DeserializeObject(tab.Value, (typeof(DataTable)));

            //PurchaseSKU = (DataTable)Session["PurchaseSKU"];
            //dtFreeSKU = null;//(DataTable)Session["dtFreeSKU"];
            OrderEntryController mOrderController = new OrderEntryController();

            if (PurchaseSKU.Rows.Count > 0)
            {

                //decimal totalAmount = decimal.Parse(_dc.chkNull_0(txtGrossAmount.Text));
                //decimal dscAmount = decimal.Parse(_dc.chkNull_0(numtxtTotalExtraDiscnt.Text));
                //decimal gst = decimal.Parse(_dc.chkNull_0(numTxtTotalGST.Text));


                //NetAmount = (totalAmount - dscAmount) + gst;


                if (btnSaveOrder.ToolTip == "Save")
                {
                    saleInvoiceID = 0;// mOrderController.Add_Invoice2(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), manualId, mTownId, 0, int.Parse(Session["PRINCIPAL_ID"].ToString()), long.Parse(Session["CUSTOMER_ID"].ToString()), long.Parse(Session["CUSTOMER_ID"].ToString()), 0, Convert.ToInt32(ddsalesForce.SelectedValue), 0,
                                      // decimal.Parse(_dc.chkNull_0(txtGrossAmount.Text)), decimal.Parse(_dc.chkNull_0(numtxtTotalExtraDiscnt.Text)), decimal.Parse(_dc.chkNull_0(txtCashRecieved2.Text)), decimal.Parse(_dc.chkNull_0(numTxtTotalGST.Text)), Decimal.Parse(_dc.chkNull_0(numTxtTotlAmnt.Text)), 0, int.Parse(DrpPayMode.SelectedValue),
                                      // PurchaseSKU, int.Parse(Session["UserId"].ToString()), 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()), 0, 0, _dc.chkNull_0(txtAuthorisedBy.Text));

                    if (saleInvoiceID == -2 || saleInvoiceID == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Insertion Failed!!!.');", true);
                    }
                    else
                    {


                        //if (System.Configuration.ConfigurationManager.AppSettings["IsPrint"].ToString() == "1")
                        //{
                        //     int i =int.Parse(txt_chkprint.Text);

                        //     for (int g = 0; g < i; g++)
                        //     {

                        //         PrintReport(1);
                        //     }
                        //}
                        ClearMasterAll();

                        btnToggleMode.Disabled = false;

                        //ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);

                        ////   ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record insert successfully.');", true);

                        //CORNBusinessLayer.Reports.CrpPrintInvoice crpReport = new CORNBusinessLayer.Reports.CrpPrintInvoice();
                       

                        //DataSet ds = null;

                        //ds = rcc.PrintInvoice(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["PRINCIPAL_ID"].ToString()), 2, int.Parse(saleInvoiceID.ToString()), Constants.DateNullValue, Constants.DateNullValue);


                        //crpReport.SetDataSource(ds);
                        //crpReport.Refresh();
                        //if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                        //{
                        //    _hirarchyNameQuiz = "POS Fashion";
                        //}
                        //crpReport.SetParameterValue("COMPANY_NAME", _hirarchyNameQuiz);
                        //crpReport.SetParameterValue("INVOICENO", Convert.ToString(saleInvoiceID));
                        ////crpReport.SetParameterValue("LOCATION", lblLoacation.Text);// sidd
                        //crpReport.SetParameterValue("PHONE_NUMBER", CompanyPhonNmbr);
                        ////crpReport.SetParameterValue("CASHIER", lbluserlogin.Text);// sidd
                        //Session.Add("CrpReport", crpReport);
                        //Session.Add("ReportType", 0);


                        const string url = "'Default.aspx'";
                        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=500,height=550,left=20,top=20\");</script>";
                        Type cstype = GetType();
                        var cs = Page.ClientScript;
                        cs.RegisterStartupScript(cstype, "OpenWindow", script);
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No product added');", true);
            }
            // }
            //  else
            // {
            //      ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('please select a customer or uncheck customer checkbox');", true);
            //  }
            ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
        }
        catch (Exception eee)
        {
            //eee.Message.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }



    protected void btnViewSalesReport_Click(object sender, EventArgs e)
    {

        ClearAll();
        ClearMasterAll();

        //ScriptManager.RegisterStartupScript(this, GetType(), "Close Modal Popup", "Closepopup();", true);

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#myModalReportCriteria').modal('hide');", true);

        if (ddlReportType.SelectedValue == "2")
        {
            try
            {

                CORNBusinessLayer.Reports.crpSalesReportPos CrpReport = new CORNBusinessLayer.Reports.crpSalesReportPos();

                DataSet ds = null;

                ds = _rptSaleCtl.SelectSaleReport(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(ddsalesForce.SelectedValue),Convert.ToDateTime(txtstartDate.Text) , Convert.ToDateTime(txtEndDate.Text) , Constants.LongNullValue);

                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                {
                    _hirarchyNameQuiz = "CORN Fashion";
                }
                CrpReport.SetParameterValue("COMPANY_NAME", _hirarchyNameQuiz);
                CrpReport.SetParameterValue("FROM_DATE", Convert.ToDateTime(txtstartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", Convert.ToDateTime(txtEndDate.Text));
                CrpReport.SetParameterValue("USER_NAME", Convert.ToString(ddsalesForce.SelectedItem.Text));
                CrpReport.SetParameterValue("LOCATION", lblLoacation.Text);
                CrpReport.SetParameterValue("PHONE_NUMBER", CompanyPhonNmbr);

                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=450,height=450,left=40,top=40\");</script>";
                Type cstype = GetType();
                var cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        else
        {
            try
            {
                

                CORNBusinessLayer.Reports.CRPSalesReportSummary CrpReport = new CORNBusinessLayer.Reports.CRPSalesReportSummary();

                DataSet ds = null;

                ds = _rptSaleCtl.SelectSaleReport(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(ddsalesForce.SelectedValue), Convert.ToDateTime(txtstartDate.Text), Convert.ToDateTime(txtEndDate.Text), -1);

                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                {
                    _hirarchyNameQuiz = "CORN Fashion";
                }
                CrpReport.SetParameterValue("COMPANY_NAME", _hirarchyNameQuiz);
                CrpReport.SetParameterValue("FROM_DATE", Convert.ToDateTime(txtstartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", Convert.ToDateTime(txtEndDate.Text));
                CrpReport.SetParameterValue("USER_NAME", Convert.ToString(ddsalesForce.SelectedItem.Text));
                CrpReport.SetParameterValue("LOCATION", lblLoacation.Text);
                CrpReport.SetParameterValue("PHONE_NUMBER", CompanyPhonNmbr);

                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=450,height=450,left=40,top=40\");</script>";
                Type cstype = GetType();
                var cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }

    public void LoadSlipNotes()
    {
        
       
    }
    protected void btnVoid_Click(object sender, EventArgs e)//cancel button click 
    {

        //if (Session["SALE_ORDER_ID"] != null)
        //{
        //    var orderId = Convert.ToInt64(Session["SALE_ORDER_ID"]);

        //    _or.Update_Order(orderId);
        //}


        ClearAll();
        ClearMasterAll();
        btnToggleMode.Disabled = false;
        txtskuCode.Focus();
    }


    #region Clear

    /// Clears Some Of Controls
    private void ClearDetail()
    {
        try
        {
            txtskuCode.Text = "";
            txtskuName.Text = "";
            txtUnitRate.Text = "";
            hfToggleMode.Value = "SALE MODE";
            if (hfToggleMode.Value != "SALE MODE")
            {
                txtQuantity.Text = "-1";
            }
            else
            {
                txtQuantity.Text = "1";
            }
            txtDiscount.Text = "0";
            //txtcolor.Text = "";
            txtsize.Text = "";

            //btnSave.ToolTip = "Add Sku";
            btnSaveOrder.Enabled = true;

            txtskuCode.Enabled = true;
            txtskuCode.Focus();

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }
    private void ClearAll()
    {
        try
        {
            txtskuCode.Text = "";
            txtskuName.Text = "";
            txtUnitRate.Text = "";
            hfToggleMode.Value = "SALE MODE";
            if (hfToggleMode.Value != "SALE MODE")
            {
                txtQuantity.Text = "-1";
            }
            else
            {
                txtQuantity.Text = "1";
            }
            txtDiscount.Text = "0";
            //txtcolor.Text = "";
            txtsize.Text = "";
            txtCashRecieved2.Text = "";
            // btnSave.ToolTip = "Add Sku";
            btnSaveOrder.Enabled = true;

            txtskuCode.Enabled = true;

            txtskuCode.Focus();
            txtAuthorisedBy.Text = "";
            txtBalance.Text = "";
        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }

    }
    /// Clears All Controls
    private void ClearMasterAll()
    {
        try
        {
            //EnableDisableController(true);
            //Session.Remove("PurchaseSKU");
            //Session.Remove("PurchaseSKUS");

            Session.Remove("CustName");
            Session.Remove("CustCode");

            //   LoadGird();
            txtGrossAmount.Text = "";
            numtxtTotalExtraDiscnt.Text = "";
            txtBalance.Text = "";
            numTxtTotalGST.Text = "";
            numTxtTotlAmnt.Text = "";
            txtCashRecieved2.Text = "";

            txtAuthorisedBy.Text = "";
            hfToggleMode.Value = "SALE MODE";
            if (hfToggleMode.Value != "SALE MODE")
            {
                txtQuantity.Text = "-1";
            }
            else
            {
                txtQuantity.Text = "1";
            }

            if (DrpPayMode.SelectedValue == "215" || DrpPayMode.SelectedValue == "218")
            {
                txtCashRecieved2.Text = "";

                txtBalance.Text = numTxtTotlAmnt.Text;
                txtCashRecieved2.Attributes.Add("readonly", "readonly");
            }
            else
            {
                txtCashRecieved2.ReadOnly = false;
            }

        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }

    #endregion



    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
        CreditLimit();
        txtskuCode.Text = "";
        txtskuName.Text = "";
        txtUnitRate.Text = "";
        txtDiscount.Text = "0";
        txtsize.Text = "";
        txtGrossAmount.Text = "";
        numTxtTotlAmnt.Text = "";
        numTxtTotalGST.Text = "";
        numtxtTotalExtraDiscnt.Text = "";
        txtBalance.Text = "";
        txtCashRecieved2.Text = "";
        txtskuCode.Focus();
    }
}