using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel mdl)
        {

            model.validateuser(mdl.UserName, mdl.Password);
            if (model.error == null)
            {
                if (model.Status == "I")
                {
                    return RedirectToAction("ChangePassword", "Account", new { area = "" });

                }
                if (model.role_id == 1)
                {
                    return RedirectToAction("Admin", "Admin", new { area = "" });

                }
                else if(model.role_id == 2)
                {
                    return RedirectToAction("Manager", "Home", new { area = "" });
                }
                else if (model.role_id == 3 || model.role_id == 4)
                {
                    return RedirectToAction("Author", "Home", new { area = "" });
                }
                else
                    return RedirectToAction("AuthorAssignment", "Home", new { area = "" });

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

            if (ModelState.IsValid)
            {
                model.ForgotPassword(objlgnmdl);
                return View("ForgotPassword", objlgnmdl);
            }
            //ModelState.AddModelError(string.Empty, "Failed To send Email. Please Try Again");
            return View("ForgotPassword", objlgnmdl);
        }
      

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

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

            if (!form["confirm-password"].Equals(form["new-password"]))
            {
                ViewBag.Cp = true;
                ViewBag.CPasswordError = "Confirm Password and New Password doesn't match.";
                return View("ChangePassword");
            }

            return View("ChangePassword");
        }

        public ActionResult Logout()
        {


            Session.Abandon();


            model.Logout();
            return View("Login");
        }
    }
}
