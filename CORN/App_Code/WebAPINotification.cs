using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

/// <summary>
/// Summary description for WebAPINotification
/// </summary>
public class WebAPINotification
{
    public WebAPINotification()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void SendNotification(string p_Title,string p_Body)
    {
        CallWebAPIAsync(p_Title, p_Body).Wait();
    }
    static async Task CallWebAPIAsync(string p_Title,string p_Body)
    {
        string Pin = "";
        try
        {
            Pin = System.Configuration.ConfigurationManager.AppSettings["ConnectionPin"].ToString();
        }
        catch (Exception ex1)
        {
            CORNCommon.Classes.ExceptionPublisher.PublishException(ex1);
            Pin = "";
        }

        try
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://fcm.googleapis.com/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "key = AAAA2CqrMwQ:APA91bGngJmBafGcDGJb7NjlT977NnHWrwotdsW0G6kacyElV2dMdAgTsqnxPU4ymGJeuDVsQSI4vDWEu6GUgAO5j3W6KhcdUDc-Z2wGVUpi8rcr01zKte_q_CbcubXuYbauF6V4bVId");
                //var notfication = new clsNotification() { title = p_Title, body = p_Body };
                //var body = new MessageBody() { to = "/topics/" + Pin, notification = notfication, data = { } };
                //HttpResponseMessage responsePost = await client.PostAsJsonAsync("fcm/send", body);
                //if (responsePost.IsSuccessStatusCode)
                //{
                //}
            }
        }
        catch (Exception ex)
        {
            CORNCommon.Classes.ExceptionPublisher.PublishException(ex);
        }
    }
}
class MessageBody
{
    public string to { get; set; }
    public clsNotification notification { get; set; }
    public clsData data { get; set; }
}
class clsNotification
{
    public string title { get; set; }
    public string body { get; set; }
}
class clsData
{
    public string title { get; set; }
    public string body { get; set; }
    public string extradata { get; set; }
    public string screen { get; set; }
    public string sound { get; set; }
    public string status { get; set; }
}