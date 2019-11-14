using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARTWeb.Controllers
{
    public class AuditController : Controller
    {
        // GET: Audit
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Audit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDetails() {
            var model = new AuditViewModel();
            var res = model.GetAuditDetails();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}