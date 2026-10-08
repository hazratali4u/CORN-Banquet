using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
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
    public class SectionPrinting : BaseWebService
    {
        [WebMethod(EnableSession = true)]
        public void PrintSectionOrder()
        {
            try
            {
                PCPrint printer = new PCPrint();
                printer.TextToPrint = Server.HtmlDecode(Request["html"]);

                SkuController _skuController = new SkuController();
                ProductSectionRequest request = new ProductSectionRequest();
                ProductSectionResponse response = _skuController.GetProductSectionsWithPrinters(request);
                if (response.IsException == false)
                {
                    List<ProductSection> ProductSectionList = response.ProductSectionList;
                    string SectionName = Convert.ToString(Request["sectionName"]).Trim().ToLower();

                    var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                    if (sectionPrinter != null)
                        printer.PrinterName = sectionPrinter.PrinterName;
                }
                printer.Print();
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