using AARTServerVO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace AARTWeb.Models
{
    public class AuditViewModel
    {
     //   Auth Auth = new Auth();

        public List<AuditVO> audits { get; set; }
        public List<AuditVO> GetAuditDetails() {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];                

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Audit/GetAllAuditTrailByUserId?userid=" + Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<AuditVO>>();
                    try
                    {
                        rData.Wait();
                        audits = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return audits;
        }
    }
}