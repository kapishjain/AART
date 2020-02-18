using AARTWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            if (Application["EditUserSection"] == null)
            {
                DataTable table = new DataTable();
                table.Columns.Add("UserID", typeof(int));
                table.Columns.Add("SectionNo", typeof(string));
                table.Columns.Add("Name", typeof(string));

                Application["EditUserSection"] = table;
            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context != null && context.Session != null && !context.Session.IsReadOnly)
            {
                context.Session["_LastAccessTime"] = DateTime.Now;
            }
        }
        void Session_End(object sender, EventArgs e)
        {
            // perform your logic
            BaseModel audit = new BaseModel();
            string username = Session["UserName"].ToString();
            try
            {
                if (Application["EditUserSection"] != null)
                {
                    DataTable dt = (DataTable)Application["EditUserSection"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int r = 0; r < dt.Rows.Count; ++r)
                        {
                            DataRow dr = dt.Rows[r];
                            if (dr["UserID"].ToString() == Session["UserID"].ToString())
                            {
                                // do your deed
                                dr.Delete();
                            }
                            //...
                        }
                        dt.AcceptChanges();
                        Application["EditUserSection"] = dt;
                    }
                    else
                    {

                    }
                }
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
