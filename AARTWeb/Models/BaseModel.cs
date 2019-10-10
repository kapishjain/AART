using AARTServerVO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace AARTWeb.Models
{
    public class BaseModel
    {
        private static String _apiurl;
        public static String WebURL
        {
            get
            {
                return _apiurl;
            }
            set
            {
                _apiurl = ConfigurationManager.AppSettings["WEBAPIURL"];
            }

        }
        public string error { get; set; }
        public Boolean Warning { get; set; }

        public Boolean IsSuccess { get; set; }

        public string Message { get; set; }

      public  BaseModel()
        {
            WebURL= ConfigurationManager.AppSettings["WEBAPIURL"];
        }
        public string InsertAudit(string module, string message)
        {
            AuditVO auditvo = new AuditVO();
            auditvo.Description = message;
            auditvo.Module = module;
            auditvo.IpAddress = GetIp();
            auditvo.Modified_Date = DateTime.Now.ToString();
            //auditvo.User_id = Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"].ToString());
            auditvo.User_id = 1;

            string value = "";
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = httpClient.PostAsync("Audit/InsertAuditTrail", new StringContent(new JavaScriptSerializer().Serialize(auditvo), Encoding.UTF8, "application/json")).Result;
                if (result.IsSuccessStatusCode)
                {
                    using (HttpContent content = result.Content)
                    {
                        Task<string> result1 = content.ReadAsStringAsync();
                        var rs = result1.Result;
                        dynamic data = JsonConvert.DeserializeObject(rs);
                        value = Convert.ToString(data);



                        if (value.Contains("error"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.error;



                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                        }
                    }
                }
                return value;
            }
        }
        public string GetIp()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();



            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);



            IPAddress[] addr = ipEntry.AddressList;



            return addr[addr.Length - 1].ToString();
        }
    }
}