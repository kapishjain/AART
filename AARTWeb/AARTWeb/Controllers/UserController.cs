using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AARTWeb.Controllers
{
    public class UserController : AARTBaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddUser()
        {
            var model = new UserModel();
            return View("Adduser",model);
        }
        [HttpPost]
        public ActionResult AddUser(UserModel objumdl)
        {
            if (ModelState.IsValid)
            {
                objumdl.Adduser(objumdl);
                if(objumdl.error!=null)
                    return View("AddUser", objumdl);

            }
            ModelState.AddModelError(string.Empty, "Failed To add user. Please Try Again");

            return View("AddUser", objumdl);
        }
    }
}