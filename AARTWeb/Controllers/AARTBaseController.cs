using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARTWeb.Controllers
{
    public class AARTBaseController : Controller
    {
        // GET: AARTBase

        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //Log the error!!
            //  _Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            //  HttpContext ctx = HttpContext.Current;
            // if (Session["UserID"].ToString() == null)
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