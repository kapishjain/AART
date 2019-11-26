using AARTWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AARTWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_End(object sender, EventArgs e)
        {
            // perform your logic
            BaseModel audit = new BaseModel();
            string username = Session["UserName"].ToString();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                    var responseTask = httpClient.PutAsJsonAsync("user/UserLogout?userid=" + Session["UserID"].ToString(), "");
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }



           // audit.InsertLogoutAudit("LogOut", "Successfully Logout", "Success", username, Ip);
            Session.Abandon();

        }
    }
}
