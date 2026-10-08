using CORNBusinessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CORN.WebServices
{
    /// <summary>
    /// Summary description for MenuCardWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MenuCardWS : BaseWebService
    {
        [WebMethod(EnableSession = true)]
        public void GetMenuCardDetails()
        {
            try
            {
                SKURequest request = new SKURequest();
                request.CategoryTypeId = 1;
                request.CompanyId = int.Parse(Session["CompanyId"].ToString());

                SKUResponse response = new SkuController().GetMenuCardDetails(request);
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