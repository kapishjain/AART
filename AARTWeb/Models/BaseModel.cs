﻿using AARTServerVO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
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
        public string InsertAudit(string module, string message,string status, string username = "",string changes="")
        {
            AuditVO auditvo = new AuditVO();
            auditvo.Description = message;
            auditvo.Module = module;
            auditvo.Ip_Address = GetIp();
            auditvo.Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss").ToString();
            auditvo.Status = status;
            auditvo.Changes = changes;
            //auditvo.User_id = Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"].ToString());
            if (System.Web.HttpContext.Current.Session["UserID"] == null)
                auditvo.User = username;
            else
                auditvo.User = System.Web.HttpContext.Current.Session["UserName"].ToString();

            string value = "";
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                Auth Auth = new Auth();

               // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
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
                         //   Message = data.error;
                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                           // Message = data.warning;
                        }
                        else
                        {
                            IsSuccess = true;
                          //  Message = data.info;
                        }
                    }
                }
                return value;
            }
        }
        public string GetIp()
        {

            //string strHostName = "";
            //strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //IPAddress[] addr = ipEntry.AddressList;
            //return addr[addr.Length - 1].ToString();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            System.Web.HttpContext.Current.Session["ip"] = context.Request.ServerVariables["REMOTE_ADDR"];
            return context.Request.ServerVariables["REMOTE_ADDR"];
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            //throw new Exception("No network adapters with an IPv4 address in the system!");
            //string strHostName = "";
            //strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //IPAddress[] addr = ipEntry.AddressList;
            //return addr[addr.Length - 1].ToString();
        }

        public string InsertLogoutAudit(string module, string message, string status,string username,string Ip)
        {
            AuditVO auditvo = new AuditVO();
            auditvo.Description = message;
            auditvo.Module = module;
            auditvo.Ip_Address = Ip ;
            auditvo.Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss").ToString();
            auditvo.Status = status;
            auditvo.User = username;
            //auditvo.Changes = changes;
            //auditvo.User_id = Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"].ToString());
            //if (System.Web.HttpContext.Current.Session["UserID"] == null)
            //    auditvo.User = username;
            //else
            //    auditvo.User = System.Web.HttpContext.Current.Session["UserName"].ToString();

            string value = "";
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                Auth Auth = new Auth();

                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
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
                            //   Message = data.error;
                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            // Message = data.warning;
                        }
                        else
                        {
                            IsSuccess = true;
                            //  Message = data.info;
                        }
                    }
                }
                return value;
            }
        }
    }
}