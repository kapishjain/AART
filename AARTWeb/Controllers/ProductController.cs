using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AARTWeb.Models.ProductModel;

namespace AARTWeb.Controllers
{
    public class ProductController : AARTBaseController
    {
        public static List<UsersByRole> rUserList = new List<UsersByRole>();
        // GET: Product
        public ActionResult Index()
        {
            return View("AddProduct");
        }
        ProductModel prdctmdl = new ProductModel();
        [HttpPost]
        public ActionResult AddPrdctAndDoc(String productname, String productdesc, String docdtls)
        {
            Int32 prdctid = 0;
            if (productname != null && productdesc != null)
            {
                prdctid = prdctmdl.AddProduct(productname, productdesc);
                if (prdctmdl.error == null)
                {
                    if (prdctid != 0)
                    {
                        if (Convert.ToString(docdtls) != null)
                        {
                            String msg = prdctmdl.AddDocument(docdtls);
                            if (prdctmdl.error == null)
                            {
                                return Json("Records Inserted successfully. Mail sent to Main Authors.");

                            }
                        }
                    }
                    return Json(prdctmdl.Message);
                }
                {
                    return Json(prdctmdl.error);
                }
            }
            return Json("Fileds Empty.");
        }

        public JsonResult GetUsersList()
        {
            var model = new ProductModel();
            String usersRoles = model.getUsersList();
            return Json(usersRoles, JsonRequestBehavior.AllowGet);
        }
               
        public JsonResult GetRolesList()
        {
            var model = new ProductModel();
            String  roles = model.getRolesList();
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredUserList(string id) {
            //var rUserList = model.GetFilteredUsersList(Convert.ToInt32(id));
            if (rUserList.Count == 0)
            {
                var model = new ProductModel();
                rUserList = model.GetUsersLi();
            }

            //if (id != null) {
            var user = rUserList.FindAll(a => a.role_id == id);
                return Json(user, JsonRequestBehavior.AllowGet);
            //}
            
        }
        [HttpPost]
        public JsonResult GetAllProductList()
        {
            var pList = prdctmdl.GetAllProductList();
            return Json(pList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllDocType()
        {
            var dList = prdctmdl.GetAllDocTypeList();
            return Json(dList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetAllComplexity()
        {
            var cList = prdctmdl.GetAllComplexityList();
            return Json(cList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertAdhocReport(List<InsertProducts> models, string protext)
        {
            String res = prdctmdl.InsertNewProducts(models, protext);

            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }
    }
}