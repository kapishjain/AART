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

namespace AARTWeb.Controllers
{
    public class AARTBaseController : Controller
    {
        public string WebURL { get; private set; }

        // GET: AARTBase

        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null || Session["token"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                // filterContext.Result = RedirectToRoute("Login", "Account");
                // OR 
                //filterContext.Result = new ViewResult
                //{
                //    ViewName = "~/Views/Account/Login.cshtml"
                //};
            }
            try
            {

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WEBAPIURL"]);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = httpClient.GetAsync("user/GetUserSessionInfo?userid=" + Session["UserID"].ToString() + "&sessionid="+ Session.SessionID);
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

                            if (value.Contains("info"))
                            {
                               
                            }
                            else if (value.Contains("error"))
                            {
                                Session.Abandon();
                                filterContext.Result = new RedirectResult("~/Account/Login");
                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");

            }
            //Log the error!!
            //  _Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            //  HttpContext ctx = HttpContext.Current;
            // if (Session["UserID"].ToString() == null)

        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            //Log the error!!
            //  _Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            //  HttpContext ctx = HttpContext.Current;
            // if (Session["UserID"].ToString() == null)
            if (Session["UserID"] == null || Session["token"]==null)
            {
                Session.Abandon();

                filterContext.Result = new RedirectResult("~/Account/Login");
                // filterContext.Result = RedirectToRoute("Login", "Account");
                // OR 
                //filterContext.Result = new ViewResult
                //{
                //    ViewName = "~/Views/Account/Login.cshtml"
                //};
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            //  _Logger.Error(filterContext.Exception);
            Session.Abandon();

            //Redirect or return a view, but not both.
            // filterContext.Result = RedirectToRoute("Account", "Login");
            filterContext.Result = new RedirectResult("~/Account/Login");

            // OR 
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Account/Login.cshtml"
            //};
        }
    }
}