using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

            string enteredUserName = mdl.UserName;
            string enteredPassword = mdl.Password;

            if ((enteredUserName != null && enteredUserName.Length > 0) && (enteredPassword != null && enteredPassword.Length > 0))
            {
                LoginViewModel model1 =model.validateuser(mdl.UserName, mdl.Password);
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


            // model.Logout();
            Session.Abandon();

            return View("Login");
        }
    }
}
