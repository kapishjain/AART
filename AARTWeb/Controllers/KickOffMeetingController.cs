using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;
using AARTServerVO;
using static AARTWeb.Models.KickOffMeetingModel;

namespace AARTWeb.Controllers
{
    public class KickOffMeetingController : AARTBaseController
    {
        // GET: KickOffMeeting
        public static  Int32 prodocid = 0;
        public ActionResult Index(int pro_doc_id)
        {
            ViewBag.pro_doc_id = pro_doc_id;
            prodocid = pro_doc_id;
            return View("KickoffMeeting");
        }

        public ActionResult ReassignAssignment(int pro_doc_id) {
            ViewBag.pro_doc_id = pro_doc_id;
            prodocid = pro_doc_id;
            return View("ReassignAssignment");
        }

        [HttpPost]
        public ActionResult InsertProDocKOMDtls(List<KickOffMeetingModel.ProDocAttendies> modelatten, List<ProDocActivityVo> docactivity, List<ProDocSectionAssignmentVO> docsection)
        {
            KickOffMeetingModel objmdl = new KickOffMeetingModel();
            objmdl.InsertProDocKOMDtls(modelatten, docactivity, docsection);
         //   objass.prodocid = prodocid;
         // objass.created_by = "4";
           // return View("KickoffMeeting");
            return Json(new { success = true, responseText = objmdl.Message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetProDocStructure()
        {
            var model = new KickOffMeetingModel();
            model.GetProDocDetails(prodocid);
            model.GetProDocTemplate(prodocid);
            model.GetProDocSecAssignment(prodocid);
            return Json(model);
        }
        
        [HttpGet]
        public void PrictDoc(String doctext)
        {
            string html = doctext;// "<strong>Hello</strong><p><strong> How are you ?</strong></p>     <ul>     <li> 12 </li>     <li> 12 </li>     <li> 12 </li>     </ul> ";
            //string yourHtmlContent = "<b>Hello!.. </b><i>This is Kaptain!</i>";
            var strBody = new StringBuilder();

            //-- add required formatting to html
            AddFormatting(strBody, html);

            //-- download file.. of you can write code to save word in any application folder
            // DownloadWord(strBody.ToString());
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/msword";
            string strFileName = "DocumentName" + ".doc";
            Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);

            Response.Write(strBody.ToString());
            Response.End();
            Response.Flush();
            //FileStream fs = null;
            //StreamWriter sw = null;
            //String outputpath = @"D:\Aart\Azure\AARTWeb\AARTWeb" + "\\" + strFileName + ".doc";
            //fs = new FileStream(outputpath, FileMode.Create, FileAccess.Write);
            //sw = new StreamWriter(fs, System.Text.Encoding.Unicode);
            //FileInfo excelFile = new FileInfo(outputpath);
            //excel.SaveAs(excelFile);

            //byte[] bytes = System.IO.File.ReadAllBytes(@"D:\Aart\Azure\AARTWeb\AARTWeb\test.docx");


            //MemoryStream stream = new MemoryStream();
            //DocX doc = DocX.Create(stream);

            //Paragraph par = doc.InsertParagraph();
            //par.Append("This is a dummy test");//.Font(new FontFamily("Times New Roman")).FontSize(32).Color(Color.Blue).Bold();

            //doc.Save();

            //return File(bytes, "application/msword", "FileName.docx");
            //  return File(strBody.ToString(), "application/octet-stream", "output.doc");
            //   return File(bytes, "application/msword", "DocumentName" + ".doc");
            // return View();

        }

        [HttpPost]
        public JsonResult GetAssignedActivitybyProDocId() {
          var model = new KickOffMeetingModel();
          var jRes = model.GetAssignedActivitybyProDocIdList(prodocid);
            
          return Json(jRes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssignedSectionAssinmentByProDocId()
        {
            var model = new KickOffMeetingModel();
            var jRes = model.GetAssignedSectionbyProDocIdList(prodocid);
            return Json(jRes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAssgnActPro(AssgnActProdList models) {
            var model = new KickOffMeetingModel();
            var resT = model.UpdateAssgnActPro(models); 
            return Json(resT, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAssgnActSec(AssgnSecProdList sModels) {
            var model = new KickOffMeetingModel();
            var resT = model.UpdateAssgnSecPro(sModels);
            return Json(resT, JsonRequestBehavior.AllowGet);
        }
        
        private void AddFormatting(StringBuilder strBody, string yourHtmlContent)
        {
            strBody.Append("<html " +
                "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                "xmlns='http://www.w3.org/TR/REC-html40'>" +
                "<head><title>Time</title>");

            //The setting specifies document's view after it is downloaded as Print
            //instead of the default Web Layout
            strBody.Append("<!--[if gte mso 9]>" +
                "<xml>" +
                "<w:WordDocument>" +
                "<w:View>Print</w:View>" +
                "<w:Zoom>90</w:Zoom>" +
                "<w:DoNotOptimizeForBrowser/>" +
                "</w:WordDocument>" +
                "</xml>" +
                "<![endif]-->");

            strBody.Append("<style>" +
                "<!-- /* Style Definitions */" +
                "@page Section1" +
                "   {size:8.5in 11.0in; " +
                "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                "   mso-header-margin:.5in; " +
                "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                " div.Section1" +
                "   {page:Section1;}" +
                "-->" +
                "</style></head>");

            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                "<div class=Section1>");
            strBody.Append(yourHtmlContent);
            strBody.Append("</div></body></html>");
        }
        private void DownloadWord(string strBody)
        {
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/msword";
            string strFileName = "DocumentName" + ".doc";
            Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);

            Response.Write(strBody);
            Response.End();
            Response.Flush();
        }
    }

   
}