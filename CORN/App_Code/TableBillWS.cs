using CORNBusinessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CORN.WebServices
{
    /// <summary>
    /// Summary description for TableBillWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TableBillWS : BaseWebService
    {
        [WebMethod(EnableSession = true)]
        public void GetOccupiedTablesForBilling()
        {
            try
            {
                TableInvoiceRequest request = new TableInvoiceRequest();
                request.DocumentDate = DateTime.Parse(Request["DocumentDate"]);
                request.DistributorId = int.Parse(Request["DistributorId"]);

                TableInvoiceResponse response = new TableBillController().GetOccupiedTablesForBilling(request);
                Response.Write(GetResponse("Success", "Success", response));
            }
            catch (Exception ex)
            {
                Response.Write(GetResponse("Error", ex.Message));
            }
            finally
            {
                HttpApplicationInstance.CompleteRequest();
            }
        }

        [WebMethod(EnableSession = true)]
        public void GetOccupiedTableBill()
        {
            try
            {
                TableInvoiceRequest request = new TableInvoiceRequest();
                request.InvoiceId = long.Parse(Request["InvoiceId"]);

                TableInvoiceResponse response = new TableBillController().GetOccupiedTableBill(request);
                response.OrderBookerInfo.OrderDateTime = response.OrderBookerInfo.OrderDate.ToString("dd-MMM-yyyy") + " " + response.OrderBookerInfo.OrderTime.ToString("hh:mm tt");
                Response.Write(GetResponse("Success", "Success", response));
            }
            catch (Exception ex)
            {
                Response.Write(GetResponse("Error", ex.Message));
            }
            finally
            {
                HttpApplicationInstance.CompleteRequest();
            }
        }

        [WebMethod(EnableSession = true)]
        public void UnholdTableByBillPaid()
        {
            try
            {
                TableInvoiceRequest request = new TableInvoiceRequest();
                request.InvoiceId = long.Parse(Request["InvoiceId"]);

                TableInvoiceResponse response = new TableBillController().UnholdTableByBillPaid(request);
                Response.Write(GetResponse("Success", "Success", response));
            }
            catch (Exception ex)
            {
                Response.Write(GetResponse("Error", ex.Message));
            }
            finally
            {
                HttpApplicationInstance.CompleteRequest();
            }
        }

    }
}