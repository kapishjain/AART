using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace AARTWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        LoginViewModel model = new LoginViewModel();



        //public ActionResult Index()
        //{
        //    return View();
        //}
      

        public ActionResult Login()
        {
            var model = new LoginViewModel();

            if (Session["UserID"] != null)
            {
                if (model.Status == "I")
                {
                    return RedirectToAction("ChangePassword", "Account", new { area = "" });

                }
                if (Session["role"] == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "" });

                }
                if (Session["role"].ToString() == "Administrator")
                {
                    return RedirectToAction("Admin", "Admin", new { area = "" });

                }
                else if (Session["role"].ToString() == "Manager")
                {
                    return RedirectToAction("Manager", "Home", new { area = "" });
                }
                else if (Session["role"].ToString() == "Author")
                {
                    return RedirectToAction("Author", "Home", new { area = "" });
                }
                else
                    return RedirectToAction("AuthorAssignment", "Home", new { area = "" });
            }
            else
                return View(model);
         //   return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel mdl)
        {
            TempData["username"] = mdl.UserName;
            TempData["Password"] = mdl.Password;

            string enteredUserName = mdl.UserName;
            string enteredPassword = mdl.Password;
           // string mode = "Login";

            if ((enteredUserName != null && enteredUserName.Length > 0) && (enteredPassword != null && enteredPassword.Length > 0))
            {
                LoginViewModel model1 =model.validateuser(mdl.UserName, mdl.Password, mdl.mode);
                if (model1.error == null && model.error == null)
                    {
                    if (model.Status == "I")
                    {
                        return RedirectToAction("ChangePassword", "Account", new { area = "" });

                    }
                    if (model.role_name == "Administrator")
                    {
                        return RedirectToAction("Admin", "Admin", new { area = "" });

                    }
                    else if (model.role_name == "Manager")
                    {
                        return RedirectToAction("Manager", "Home", new { area = "" });
                    }
                    else if (model.role_name == "Author")
                    {
                        return RedirectToAction("Author", "Home", new { area = "" });
                    }
                    else
                        return RedirectToAction("AuthorAssignment", "Home", new { area = "" });
                    //model.error = "Want to loggout from other system.";
                    ////ViewData["Error"] = "Want to loggout from other system.";
                    //ViewBag.Error = "Want to loggout from other system.";
                    //return View(model);
                }
                else
                {
                    if (model1.error == null)

                        return View(model);
                    else
                        return View(model1);

                }
            }
            
            return View(model);                                 
        }
        [HttpPost]

        public ActionResult ReLogin(string username,string password,string mode)
        {

            string enteredUserName = TempData["username"].ToString();
            string enteredPassword = TempData["Password"].ToString();
            // string mode = "Login";

            if ((enteredUserName != null && enteredUserName.Length > 0) && (enteredPassword != null && enteredPassword.Length > 0))
            {
                LoginViewModel model1 = model.validateuser(enteredUserName, enteredPassword, mode);
                if (model1.error == null && model.error == null)
                {
                    if (model.Status == "I")
                    {
                      //  return RedirectToAction("ChangePassword", "Account", new { area = "" });
                        return Json(new { result = "Redirect", url = Url.Action("ChangePassword", "Account") });

                    }
                    if (model.role_name == "Administrator")
                    {
                       // return RedirectToAction("Admin", "Admin", new { area = "" });
                        return Json(new { result = "Redirect", url = Url.Action("Admin", "Admin") });
                    }
                    else if (model.role_name == "Manager")
                    {
                        return Json(new { result = "Redirect", url = Url.Action("Manager", "Home") });

                       // return RedirectToAction("Manager", "Home", new { area = "" });
                    }
                    else if (model.role_name == "Author")
                    {
                        return Json(new { result = "Redirect", url = Url.Action("Author", "Home") });

                       // return RedirectToAction("Author", "Home", new { area = "" });
                    }
                    else
                        return Json(new { result = "Redirect", url = Url.Action("AuthorAssignment", "Home") });

                   // return RedirectToAction("AuthorAssignment", "Home", new { area = "" });
                    //model.error = "Want to loggout from other system.";
                    ////ViewData["Error"] = "Want to loggout from other system.";
                    //ViewBag.Error = "Want to loggout from other system.";
                    //return View(model);
                }
                else
                {
                    if (model1.error == null)

                        return View(model);
                    else
                        return View(model1);

                }
            }

            return View(model);
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(LoginViewModel objlgnmdl)
        {
            if (objlgnmdl.Email == null)
            {
                objlgnmdl.Message = "Email or UserName field is required.";
            }
            else
            {
                model.ForgotPassword(objlgnmdl);

            }
            return View("ForgotPassword", objlgnmdl);
        }
      

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ChangePassword(FormCollection form)
        //{

        //    string EnteredOldPassword = form["old-password"];
        //    string EnteredNewPassword = form["new-password"];
        //    string EnteredConfirmPassword = form["confirm-password"];


        //    if (!(EnteredOldPassword.Length > 0) || !(EnteredNewPassword.Length > 0) || !EnteredConfirmPassword.Equals(EnteredNewPassword))
        //    {
        //        CheckFields(form);
        //    }
        //    else
        //    {
        //        model.ChangePassword(form);
        //        return View("ChangePassword", model);
        //    }

        //    return View("ChangePassword");
        //}
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string EnteredOldPassword = form["old-password"];
            string EnteredNewPassword = form["new-password"];
            string EnteredConfirmPassword = form["confirm-password"];



            if (!(EnteredOldPassword.Length > 0) || !(EnteredNewPassword.Length > 0) || !EnteredConfirmPassword.Equals(EnteredNewPassword))
            {
                CheckFields(form);
            }
            else if (EnteredNewPassword.Length < 8 || EnteredNewPassword.Length > 16)
            {
                CheckFields(form);
            }
            else if (!LoginViewModel.IsValid(EnteredNewPassword))
            {
                ViewBag.Np = true;
                ViewBag.NPasswordError = "Password invalid. Password must contain one special character, upper case, lower case and it should be alphanumeric";
                return View("ChangePassword");
            }
            else
            {
                model.ChangePassword(form);
                return View("ChangePassword", model);
            }

            return View("ChangePassword");
        }



        private ActionResult CheckFields(FormCollection form)
        {
            if (!(form["old-password"].Length > 0))
            {
                ViewBag.Op = true;
                ViewBag.PasswordError = "Old Password Field is Required";
            }
            if (!(form["new-password"].Length > 0))
            {
                ViewBag.Np = true;
                ViewBag.NPasswordError = "New Password Field is Required";
            }

            if (form["new-password"].Length < 8 || form["new-password"].Length > 16)
            {
                ViewBag.Np = true;
                ViewBag.NPasswordError = "New Password should be of 8-16 characters";
            }
            if (!form["confirm-password"].Equals(form["new-password"]))
            {
                ViewBag.Cp = true;
                ViewBag.CPasswordError = "Confirm Password and New Password doesn't match.";
                return View("ChangePassword");
            }
            return View("ChangePassword");
        }
      
        //private ActionResult CheckFields(FormCollection form)
        //{

        //    if (!(form["old-password"].Length > 0))
        //    {
        //        ViewBag.Op = true;
        //        ViewBag.PasswordError = "Old Password Field is Required";
        //    }

        //    if (!(form["new-password"].Length > 0))
        //    {
        //        ViewBag.Np = true;
        //        ViewBag.NPasswordError = "New Password Field is Required";
        //    }

        //    if (!form["confirm-password"].Equals(form["new-password"]))
        //    {
        //        ViewBag.Cp = true;
        //        ViewBag.CPasswordError = "Confirm Password and New Password doesn't match.";
        //        return View("ChangePassword");
        //    }

        //    return View("ChangePassword");
        //}

        public ActionResult Logout()
        {
            //BaseModel audit = new BaseModel();

            //audit.InsertAudit("LogOut", "Successfully Logout", "Success");
            if (HttpContext.Application["EditUserSection"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Application["EditUserSection"];
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
                    HttpContext.Application["EditUserSection"] = dt;
                }
                else
                {

                }
            }

            // model.Logout();
            Session.Abandon();

            return View("Login");
        }
        public string sessionexpire()
        {
            //BaseModel audit = new BaseModel();

            //audit.InsertAudit("LogOut", "Successfully Logout", "Success");
            if (HttpContext.Application["EditUserSection"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Application["EditUserSection"];
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
                    HttpContext.Application["EditUserSection"] = dt;
                }
                else
                {

                }
            }

            // model.Logout();
            Session.Abandon();

            return "Logout";
            //return View("Login");
        }
        public string getsessiontime()
        {
            LoginViewModel objmdl = new LoginViewModel();
            objmdl.getremainingsession();
            //            HttpSessionState session = HttpContext.Current.Session;
            //            DateTime? sStart = session[SessionKeys.SessionStart] as DateTime?;

            //            Below code gives you remaining time

            //if (sessionStart.HasValue)
            //                TimeSpan remainingTime = sStart.Value - DateTime.Now;
            //HttpSessionState session = HttpContext.Current.Session;//Return current sesion
            //DateTime? sessionStart = session[SessionKeys.SessionStart] as DateTime?;//Convert into DateTime object
            //if (sessionStart.HasValue)//Check if session has not expired
            //    TimeSpan remaining = sessionStart.Value - DateTime.Now;//Get the remaining time
            return System.Web.HttpContext.Current.Session.Timeout.ToString();
        }
    }
}
