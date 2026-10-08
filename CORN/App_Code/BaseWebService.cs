using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Reflection;
using System.Text;

namespace CORN.WebServices
{
    /// <summary>
    /// Summary description for BaseWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class BaseWebService : System.Web.Services.WebService
    {
        public static string CurrentExecutionFilePath { get; set; }
        public HttpServerUtility Server { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public JavaScriptSerializer JSONSerializer { get; set; }

        protected HttpSessionState Session { get; set; }
        public ResponseView ServerResponse { get; set; }
        protected HttpApplication HttpApplicationInstance { get; private set; }

        public BaseWebService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 

            try
            {
                HttpApplicationInstance = Context.ApplicationInstance;
                Server = Context.Server;
                Request = Context.Request;
                Response = Context.Response;
                CurrentExecutionFilePath = Request.CurrentExecutionFilePath.ToLower();
                Response.ContentType = "application/json";
                JSONSerializer = new JavaScriptSerializer();
                JSONSerializer.MaxJsonLength = 2147483647;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session = Context.Session;
                ServerResponse = new ResponseView();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();
                Response.AppendHeader("pragma", "no-cache");

                //MethodInfo method = this.GetType().GetMethod(Request["method"]);

                //if (method != null)
                //{
                //    method.Invoke(this, new object[] { });
                //}
                //else
                //{
                //    StringBuilder resp = new StringBuilder();
                //    resp.Append(JSONSerializer.Serialize(new ResponseView() { Result = "Error", Status = "Error", Message = "Invalid Operation." }));
                //    Response.Write(resp);
                //    Context.ApplicationInstance.CompleteRequest();
                //}

            }
            catch (Exception ex)
            {
                //ex.GetMessage("I crashed here", "", 0);
                Response.Write(GetResponse("Error", ex.Message));
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        public string GetResponse(string status, string message)
        {
            ServerResponse.Result = status;
            ServerResponse.Status = status;
            ServerResponse.Message = message;
            return JSONSerializer.Serialize(ServerResponse);
        }
        public string GetResponse(string status, string message, object data)
        {
            ServerResponse.Result = status;
            ServerResponse.Status = status;
            ServerResponse.Message = message;
            ServerResponse.Records = data;
            return JSONSerializer.Serialize(ServerResponse);
        }
        public string GetResponse(string status, string message, object data, long recordCount)
        {
            ServerResponse.Result = status;
            ServerResponse.Status = status;
            ServerResponse.Message = message;
            ServerResponse.Records = data;
            ServerResponse.TotalRecordCount = recordCount;
            return JSONSerializer.Serialize(ServerResponse);
        }
        public string GetResponse(string status, string message, object data, long recordCount, object extData)
        {
            ServerResponse.Result = status;
            ServerResponse.Status = status;
            ServerResponse.Message = message;
            ServerResponse.Records = data;
            ServerResponse.TotalRecordCount = recordCount;
            ServerResponse.ExtraData = extData;
            return JSONSerializer.Serialize(ServerResponse);
        }


    }

    public class ResponseView
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public object Records { get; set; }
        public string Status { get; set; }
        public long TotalRecordCount { get; set; }
        public object ExtraData { get; set; }
        public ResponseView()
        { }
        public ResponseView(string status, string message)
        {
            this.Result = status;
            this.Message = message;
            this.Status = status;
        }
    }
}