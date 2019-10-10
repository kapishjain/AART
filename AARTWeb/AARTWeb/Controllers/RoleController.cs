using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARTWeb.Controllers
{
    public class RoleController : AARTBaseController
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddRole()
        {
            var model = new RoleModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult PostRole(RoleModel objrmdl) {
            if (ModelState.IsValid)
            {
                objrmdl.Addrole(objrmdl);
                if (objrmdl.error != null)
                    return View("AddRole", objrmdl);

            }
            ModelState.AddModelError(string.Empty, "Failed To add user. Please Try Again");

            return View("AddRole", objrmdl);
        }
    }
}