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

public partial class Forms_EventBooking_Form : System.Web.UI.Page
{
    readonly SkuController _mSkuController = new SkuController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtBookingDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            Event_type_date.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtBookingDate.Attributes.Add("readonly", "readonly");
            Event_type_date.Attributes.Add("readonly", "readonly");
            voucher_no.Attributes.Add("readonly", "readonly");
            LoadEventType();
            LoadGrid("");
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            btnSave.Text = "Save";
        }

    }


    private void ClearAll()
    {
        try
        {
            //LoadGrid("");
            txtBookingDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            voucher_no.Text = "--0000--";
            Eventcost_entry.Text = "";
            Payment_plan.Text = "";
            CNIC_NTN_no.Text = "";
            Contact_no.Text = "";
            Address.Text = "";
            Name.Text = "";
            DrpEventType.SelectedIndex = 0;
            Location.Text = "";
            Event_type_date.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            btnSave.Text = "Save";

        }
        catch (Exception)
        {

        }
    }

    private void LoadEventType() 
    {
        DataTable dt = _mHerController.SelectDropdown(Constants.IntNullValue,null,true);
        clsWebFormUtil.FillDxComboBoxList(DrpEventType, dt, "EventTypeID", "EventName", true);
        if (dt.Rows.Count > 0)
        {
            DrpEventType.SelectedIndex = 0;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool success = false;
            string savedID = "0";
            if (btnSave.Text == "Save")
            {

                savedID = _mSkuController.InsertSKUSCustomerForm(Constants.IntNullValue, Convert.ToDateTime(txtBookingDate.Text), voucher_no.Text, Eventcost_entry.Text, Payment_plan.Text, CNIC_NTN_no.Text,
                    Contact_no.Text, Address.Text, Convert.ToDateTime(Event_type_date.Text),Name.Text, Convert.ToInt32(DrpEventType.SelectedItem.Value), Location.Text,1);

                success = true;

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                ClearAll();
                mpePopUp.Hide();
            }   
            else if(btnSave.Text == "Update")
            {
                savedID = _mSkuController.InsertSKUSCustomerForm(int.Parse(hfID.Value.ToString()), Convert.ToDateTime(txtBookingDate.Text), voucher_no.Text, Eventcost_entry.Text, Payment_plan.Text, CNIC_NTN_no.Text,
                       Contact_no.Text, Address.Text, Convert.ToDateTime(Event_type_date.Text), Name.Text,Convert.ToInt32( DrpEventType.SelectedItem.Value), Location.Text, 3);

                success = true;

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Updated successfully.');", true);
                ClearAll();
            }
            btnSave.Text = "Save";
            LoadGrid("");

            if (success == true)
            {
                ShowReportPopUp(savedID);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('" + ex.Message + "');", true);
        }

    }
    public void ShowReportPopUp(string savedID)
    {
        try
        {
            DocumentPrintController DPrint = new DocumentPrintController();

            DsReport2 ds = new DsReport2();
            DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));

            DataControl dc = new DataControl();
            DataTable rowsDt = _mSkuController.SelectSKUSCustomerFormByID(Convert.ToInt32(savedID), 5);

            foreach (DataRow dr in rowsDt.Rows)
            {
                ds.Tables["RptEventBooking"].ImportRow(dr);
            }

            ReportDocument CrpReport = new ReportDocument();
            CrpReport = new CrpEventBookingPopup();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("Address", dt.Rows[0]["CompanyAddress"].ToString());
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("Contact", dt.Rows[0]["CONTACT_NUMBER"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../Terms.pdf' ,'_blank');", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void LoadGrid(string pType)
    {
        

        grdEventBooking_form.DataSource = null;
        grdEventBooking_form.DataBind();

        DataTable dt = new DataTable();
        dt = _mSkuController.SelectSKUSCustomerForm(Constants.DateNullValue, null, null, null, null,null, null, Constants.DateNullValue,null, Constants.IntNullValue, null, 2);

        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)//In case after  Filter
            {
                dt.DefaultView.RowFilter = "Location LIKE '%" + txtSearch.Text + "%' OR CNIC_NTN_no_client LIKE '%" + txtSearch.Text + "%' OR EventName LIKE '%" + txtSearch.Text + "%' OR Voucher_no LIKE '%" + txtSearch.Text + "%' OR Name LIKE '%" + txtSearch.Text + "%' OR Contact_no_client LIKE '" + txtSearch.Text + "%'";
            }
            grdEventBooking_form.DataSource = dt;
            grdEventBooking_form.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "Location LIKE '%" + txtSearch.Text + "%' OR CNIC_NTN_no_client LIKE '%" + txtSearch.Text + "%' OR EventName LIKE '%" + txtSearch.Text + "%' OR Voucher_no LIKE '%" + txtSearch.Text + "%' OR Name LIKE '%" + txtSearch.Text + "%' OR Contact_no_client LIKE '" + txtSearch.Text + "%'";
            }
            if (dt.Rows.Count > 0)
            {
                grdEventBooking_form.PageIndex = 1;
            }
            grdEventBooking_form.DataSource = dt;
            grdEventBooking_form.DataBind();
        }
    }


    protected void grdEventBookingChanging(object sender, GridViewPageEventArgs e)
    {
        grdEventBooking_form.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        mpePopUp.Show();
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        DateTime BookingDate = DateTime.Parse(Row.Cells[0].Text);
        txtBookingDate.Text = BookingDate.ToString("dd-MMM-yyyy");
        DateTime EventDate = DateTime.Parse(Row.Cells[1].Text);
        Event_type_date.Text = EventDate.ToString("dd-MMM-yyyy");
        Name.Text = Row.Cells[2].Text;
        Contact_no.Text = Row.Cells[3].Text;
        CNIC_NTN_no.Text = Row.Cells[4].Text;
        Location.Text = Row.Cells[5].Text;
        DrpEventType.Text = Row.Cells[6].Text;
        voucher_no.Text = Row.Cells[7].Text;
        Payment_plan.Text = Row.Cells[8].Text;
        Eventcost_entry.Text = Row.Cells[9].Text;
        Address.Text = Row.Cells[10].Text;
        hfID.Value = Row.Cells[11].Text;
        
        btnSave.Text = "Update";

    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        mpePopUp.Hide();
        ClearAll();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
        btnSave.Text = "Save";
        mpePopUp.Show();
    }

    protected void btnOpenPopUp_Click(object sender, EventArgs e)
    {
        mpePopUp.Show();
    }

    protected void del_Click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        _mSkuController.InsertSKUSCustomerForm(int.Parse(Row.Cells[11].Text), Convert.ToDateTime(txtBookingDate.Text), voucher_no.Text, Eventcost_entry.Text, Payment_plan.Text, CNIC_NTN_no.Text,
        Contact_no.Text, Address.Text, Convert.ToDateTime(Event_type_date.Text), Name.Text, Convert.ToInt32(DrpEventType.SelectedItem.Value), Location.Text, 4);
            LoadGrid("");
        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record Deleted successfully.');", true);
    }
}