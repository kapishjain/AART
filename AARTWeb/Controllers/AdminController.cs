using AARTWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AARTWeb.Controllers
{
    public class AdminController : AARTBaseController
    {
        // GET: Admin
        public ActionResult Admin()
        {
            AdminModel objadm = new AdminModel();
            return View(objadm);
        }
        //[HttpPost]
        //public JsonResult Admin(AdminModel adml)
        //{
        //    AdminModel objadm = new AdminModel();
        //    objadm.GetAllUsers();
        //    //objadm.GetAllRoles();
        //    return Json(objadm, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetUsers() {
            var model = new AdminModel();
            var res = model.GetAllUsers();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRoles() {
            var model = new AdminModel();
            var res = model.GetAllRoles();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(AdminModel.UsersModel models)
        {
            var model = new AdminModel();
            var uData = model.UpdateUser(models);
            return uData;
        }

        [HttpPost]
        public string Create(AdminModel.UsersModel models)
        {
            var model = new AdminModel();
            var cData = model.AddUser(models);
            return cData;
        }

        [HttpPost]
        public string CreateRole(AdminModel.RolesModel rModels)
        {
          var model = new AdminModel();
            var roleData = model.AddRole(rModels);
            return roleData;
        }
        public string UpdateRole(AdminModel.RolesModel rModels)
        {
            var model = new AdminModel();
            var roleData = model.UpdateRole(rModels);
            var error = model.ReturnErrorMessage;
            var status = model.ReturnStatus;
            //TempData["RoleMessage"] = error;
            //TempData["RoleStatus"] = status;
            return roleData;
            //return Json(roleData, JsonRequestBehavior.AllowGet);
        }
    }
}