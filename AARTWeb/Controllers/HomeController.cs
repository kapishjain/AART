﻿using AARTWeb.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.XPath;
using AARTServerVO;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;

namespace AARTWeb.Controllers
{
    public class HomeController : AARTBaseController
    {
        ProductModel objprmdl = new ProductModel();
        public static String docval = null;
        public static Int32 prodocid = 0;
        public static String htmlString = null;
        public static string destinationFile = null;

        public ActionResult Index()
        {
            //DocX document = null;

            //document = DocX.Create(Server.MapPath("~/mydoc.docx"), DocumentTypes.Document);

            ////Image img = document.AddImage(Server.MapPath("~/Images/mvc.png"));

            ////Picture pic = img.CreatePicture(100, 100);


            //Paragraph picturepara = document.InsertParagraph();
            //picturepara.Alignment = Alignment.center;
            // picturepara.Append("                                ");
            // picturepara.AppendPicture(pic).Alignment = Alignment.center;

            //var headLineFormat = new Formatting();
            //headLineFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
            //headLineFormat.Size = 18D;
            //headLineFormat.Position = 12;

            //string headlineText = "What is Lorem Ipsum?";
            //document.InsertParagraph(headlineText, false, headLineFormat);

            //var paraFormat = new Formatting();
            //paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            //paraFormat.Size = 11.0f;
            //paraFormat.CapsStyle = CapsStyle.none;



            //string p1TExt = @"Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country.";
            //document.InsertParagraph(p1TExt, false, paraFormat).Alignment = Alignment.both;
            //document.InsertParagraph(" ");

            //string p2Text = @"Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic life One day however a small line of blind text by the name of Lorem Ipsum decided to leave for the far World of Grammar. The Big Oxmox advised her not to do so, because there were thousands of bad Commas, wild Question Marks and devious Semikoli, but the Little Blind Text didn’t listen. She packed her seven versalia, put her initial into the belt and made herself on the way. When she reached the first hills of the Italic Mountains, she had a last view back on the skyline of her hometown Bookmarksgrove, the headline of Alphabet Village and the subline of her own road, the Line Lane. Pityful a rethoric question ran over her cheek, then";
            //document.InsertParagraph(p2Text, false, paraFormat).Alignment = Alignment.both;
            //document.InsertParagraph(" ");

            //Table tbl = document.AddTable(5, 4);
            //tbl.Alignment = Alignment.center;
            //tbl.Design = TableDesign.LightGridAccent2;

            //tbl.Rows[0].Cells[0].Paragraphs.First().Append("Name");
            //tbl.Rows[0].Cells[1].Paragraphs.First().Append("Father Name");
            //tbl.Rows[0].Cells[2].Paragraphs.First().Append("Address");
            //tbl.Rows[0].Cells[3].Paragraphs.First().Append("Phone");

            //for (int i = 1; i <= 4; i++)
            //{
            //    for (int j = 0; j <= 3; j++)
            //    {
            //        tbl.Rows[i].Cells[j].Paragraphs.First().Append("(" + i + "," + j + ")");
            //    }
            //}
            //document.app("sdafsd fs fs fsafasd fsdaf");
            //document.InsertTable(tbl);

            //// For  Farsi, Arabic and Urdu.
            //// document.SetDirection(Direction.RightToLeft); 

            //document.Save();


            //MemoryStream ms = new MemoryStream();
            //document.SaveAs(ms);
            //// document.Save(ms, SaveFormat.Docx);
            //byte[] byteArray = ms.ToArray();
            //ms.Flush();
            //ms.Close();
            //ms.Dispose();
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment; filename=report.docx");
            //Response.AddHeader("Content-Length", byteArray.Length.ToString());
            //Response.ContentType = "application/msword";
            //Response.BinaryWrite(byteArray);
            //Response.End();

            //StringBuilder strBody = new StringBuilder();
            //strBody.Append("<h1>Hello</h2>");
            //Response.AppendHeader("Content-Type", "application/msword");
            //Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");
            //Response.Write(strBody);
            //Response.End();





            //using (WordprocessingDocument doc = WordprocessingDocument.Open(@"D:\Aart\Azure\AARTWeb\AARTWeb\test.docx", true))
            //            {
            //                string altChunkId = "myId111";
            //                MainDocumentPart mainDocPart = doc.MainDocumentPart;

            //                var run = new DocumentFormat.OpenXml.Drawing.Run(new Text("test"));
            //                var p = new DocumentFormat.OpenXml.Drawing.Paragraph(new ParagraphProperties(
            //                     new Justification() { Val = JustificationValues.Center }),
            //                                   run);

            //                var body = mainDocPart.Document.Body;
            //                body.Append(p);

            //                //  MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes());
            //                MemoryStream ms = new MemoryStream(new UTF8Encoding(true).GetPreamble().Concat(Encoding.UTF8.GetBytes("<html><head></head><body><h1>HELLO</h1></body></html>")).ToArray());
            //                // Uncomment the following line to create an invalid word document.
            //                // MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("<h1>HELLO</h1>"));

            //                // Create alternative format import part.
            //                AlternativeFormatImportPart formatImportPart =
            //                   mainDocPart.AddAlternativeFormatImportPart(
            //                      AlternativeFormatImportPartType.Html, altChunkId);
            //                //ms.Seek(0, SeekOrigin.Begin);

            //                // Feed HTML data into format import part (chunk).
            //                formatImportPart.FeedData(ms);
            //                DocumentFormat.OpenXml.Wordprocessing.AltChunk altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
            //                altChunk.Id = altChunkId;

            //                //mainDocPart.Document.Body.Append(altChunk);
            //                //using (System.IO.StreamWriter stringStream = new System.IO.StreamWriter(altChunk))

            //                   // stringStream.Write(altChunk);
            //                Response.AppendHeader("Content-Type", "application/msword");
            //                Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");
            //                Response.Write(altChunk);
            //                Response.End();
            //            }
            //string html = "<strong>Hello</strong><p><strong> How are you ?</strong></p>     <ul>     <li> 12 </li>     <li> 12 </li>     <li> 12 </li>     </ul> ";
            ////string yourHtmlContent = "<b>Hello!.. </b><i>This is Kaptain!</i>";
            //var strBody = new StringBuilder();

            ////-- add required formatting to html
            //AddFormatting(strBody, html);

            ////-- download file.. of you can write code to save word in any application folder
            //// DownloadWord(strBody.ToString());
            //Response.Clear();
            //Response.Charset = "";
            //Response.ContentType = "application/msword";
            //string strFileName = "DocumentName" + ".doc";
            //Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);

            //Response.Write(strBody.ToString());
            //Response.End();
            //Response.Flush();

            return View("AuthorAssignment");
        }
        public string GetUserName()
        {
            return Session["UserName"].ToString();
        }
        public ActionResult Manager()
        {
            //Auth cl = new Auth();
            //cl.CallApi();
            objprmdl.GetProductCount();
            objprmdl.GetDocumentCount();
            objprmdl.GetProductDetails();
            objprmdl.GetManagerDashBoardDetails();

            return View(objprmdl);
        }

        [HttpPost]
        public JsonResult GetAllProducts(string id)
        {
            var productDetails = objprmdl.GetProductDetails();
            if (id != "All")
            {
                var product = productDetails.Where(item => item.status == id);
                return Json(product, JsonRequestBehavior.AllowGet);

            }
            return Json(productDetails, JsonRequestBehavior.AllowGet);

            //return Json(objprmdl.getallproduct);
        }
        public ActionResult Author()
        {
            //Auth cl = new Auth();
            //cl.CallApi();
            //objprmdl.GetProductCount();
            // objprmdl.GetDocumentCount();
            //objprmdl.GetProductDetails();
            objprmdl.GetManagerDashBoardDetails();
            return View(objprmdl);
        }

        public ActionResult AuthorAssignment()
        {
            //Auth cl = new Auth();
            //cl.CallApi();
            //objprmdl.GetProductCount();
            // objprmdl.GetDocumentCount();
            //objprmdl.GetProductDetails();
            objprmdl.GetDashBoardDetails();
            return View("AuthorAssignment", objprmdl);
        }
        public ActionResult GetReportsByUser(string id)
        {
            //   objprmdl.GetReportsByUser();
            var productDetails = objprmdl.GetReportsByUser();
            if (id != "All")
            {
                var product = productDetails.Where(item => item.status == id);
                return Json(product, JsonRequestBehavior.AllowGet);

            }
            return Json(productDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetProDocByUser()
        {
            objprmdl.GetProDocByUser();

            return Json(objprmdl.getprodocbyuser);
        }
        public ActionResult Authoring(int pro_doc_id)
        {
            prodocid = pro_doc_id;
            return View("AuthorAssignment");
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
        [HttpPost]
        public JsonResult GetProDocTemplate(string id)
        {
            var productDetails = objprmdl.GetProDocTemplateByUser();
            if (id != "All")
            {
                var product = productDetails.Where(item => item.status == id);
                return Json(product, JsonRequestBehavior.AllowGet);

            }
            return Json(productDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProDocSectionAssignmentByUser(string id)
        {
            var productDetails = objprmdl.GetProDocSectionAssignmentByUser();
            if (id != "All")
            {
                var product = productDetails.Where(item => item.status == id);
                return Json(product, JsonRequestBehavior.AllowGet);

            }
            return Json(productDetails, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public string UpdateActivitySection(ProDocActivityVo models)
        {
            string result = objprmdl.UpdateActivityByUser(models);
            return result;
        }
        [ValidateInput(false)]
        [HttpPost]
        public string UpdateSection(TemplateSectionVo models)
        {
            string result = objprmdl.UpdateSecAssUser(models);
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
                    return result;

                }
                else
                {
                    return result;

                }
            }
            return result;
        }
        [HttpPost]
        public JsonResult GetProDocTemplateForChart()
        {
            var model = objprmdl.GetProDocTemplateForChart();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProDocSectionAssignmentForChart()
        {
            var model = objprmdl.GetProDocSectionAssignmentForChart();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UpdateProDocRole(ProductModel.ProductDetails models)
        {
            string model = objprmdl.UpdateProDocRole(models);
            return model;
        }
        [HttpPost]
        public JsonResult GetUsersForLeadAuthor()
        {
            var model = objprmdl.GetUsersForLeadAuthor();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUsersForCoAuthor()
        {
            var model = objprmdl.GetUsersForCoAuthor();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredCoAuthors(string selectedItem)
        {
            var model = objprmdl.GetUsersForCoAuthor();

            //  var itemToRemove1 = model.FirstOrDefault(r => r.user_name == selectedItem);
            if (selectedItem != "Select LeadAuthor")
            {
                var itemToRemove = model.Single(r => r.user_name == selectedItem);
                model.Remove(itemToRemove);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuthorReview(Int32 pro_doc_id, string status)
        {
            prodocid = pro_doc_id;
            ViewData["status"] = status;
            return View("AuthorReview");
        }
        [HttpPost]
        public JsonResult GetProDocActvity()
        {
            var proDocSecActList = objprmdl.GetProDocActvity(prodocid);
            return Json(proDocSecActList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ProDocSecAssList()
        {
            var proDocSecAssList = objprmdl.GetProDocSecAss(prodocid);
            return Json(proDocSecAssList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProDocActList()// get prodoclist
        {
            var proDocSecActList = objprmdl.GetProDocActAss(prodocid);
            return Json(proDocSecActList, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public string UpdateProDocSecAss(TemplateSectionVo models)
        {
            string res = objprmdl.UpdateProDocSecAss(models);
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
                    return res;

                }
                else
                {
                    return res;

                }
            }
            return res;
        }

        [ValidateInput(false)]
        [HttpPost]
        public string UpdateProDocActAss(TemplateSectionVo models)
        {
            string res = objprmdl.UpdateProDocActAss(models);
            if (HttpContext.Application["EditUserSection"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Application["EditUserSection"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["UserID"].ToString() == Session["UserID"].ToString())
                        {
                            // do your deed
                            dr.Delete();
                            dt.AcceptChanges();
                        }
                    }
                    HttpContext.Application["EditUserSection"] = dt;
                    return res;

                }
                else
                {
                    return res;

                }
            }

            return res;
        }

        /*Method used in authorreivew page on accept and revert 
         * click of section and activity
        */
        [ValidateInput(false)]
        [HttpPost]
        public string UpdateAcceptReject(int id, string status, string comment, string recTemplate, string templateVar)
        {
            var model = objprmdl.UpdateAcceptReject(id, status, comment, prodocid, recTemplate, templateVar);
            return model;
        }


        /*Method used in authorreivew page to get the data on click of view 
         * button
         */
        [HttpPost]
        public JsonResult GetViewData(int id, string templateVar)
        {
            var model = objprmdl.GetViewData(id, templateVar);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /*
         * Method to get difference of template changes and comments
         * from Lead Author and author
        */
        [HttpPost]
        public string GetDifferenceData(int id, string templateVar)
        {
            var model = objprmdl.GetDiffData(id, templateVar);
            if (model.Oldtemplate == null)
                model.Oldtemplate = "";
            var diffHelper = new HtmlDiff.HtmlDiff(model.Oldtemplate, model.Newtemplate);
            var diff = diffHelper.Build();
            return diff;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PrictDoc(string doctext)
        {
            List<TemplateSectionVo> Grid = JsonConvert.DeserializeObject<List<TemplateSectionVo>>(doctext);
            //htmlString = "<h1 style='text-align: center;'><strong>Report</strong></h1>" + "<br>";
            string sourceFile = Server.MapPath(Path.Combine("/", "Themes/Template/PSURTemplate.dotx"));
            var filename = Path.GetFileNameWithoutExtension(sourceFile);
            destinationFile = Server.MapPath(Path.Combine("/", "Themes/DownloadDocs/" + "PBRER" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".docx"));
            try
            {
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);
                WordprocessingDocument wordDoc =
                WordprocessingDocument.Open(destinationFile, true);
                wordDoc.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                //marker for main document part
                var mainDocPart = wordDoc.MainDocumentPart;

                //Iterate through the paragraphs to find the bookmarks inside
                //load bookmarks
                var bookmarks = mainDocPart.Document.Body.Descendants<BookmarkStart>();

                for (int i = 0; i < Grid.Count; i++)
                {
                    //find our bookmark
                    string bookmarkname1 = Regex.Match(Grid[i].Section, @"(\d+.\d+.\d+.|\d+.\d+.|\d+.)").Value;
                    string bookmarkname = bookmarkname1.Replace(".", "_");
                    //get to first element
                    if (bookmarkname1 == null || bookmarkname1 == "")
                    {
                        bookmarkname = Regex.Replace(Grid[i].Section, @"[\d+|.|\s]", "");
                    }
                    var bookmark = from bookmarkIndex in bookmarks where bookmarkIndex.Name == "BK_" + bookmarkname select bookmarkIndex;
                    if (bookmark.Count() > 0)
                    {
                        htmlString = "";
                        if (Grid[i].Template_Content != null)
                        {
                            htmlString = "<html>" + Grid[i].Template_Content.Replace("<table>", "<table border='1' style=' border-collapse: collapse; border: 1px solid black;width:100%;'>");
                        }
                        string UrlLink = new Uri(Request.Url, Url.Content("~")).AbsoluteUri + "ImageBrowser/Image";
                        string source = "/ImageBrowser/Image";
                        string CON = htmlString;
                        htmlString = CON.Replace(source, UrlLink);
                        OpenXmlElement elem = bookmark.First().Parent;
                        String cid = "chunkid_" + i;
                        MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlString));
                        AlternativeFormatImportPart formatImportPart = wordDoc.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, cid);
                        formatImportPart.FeedData(ms);
                        AltChunk altChunk = new AltChunk();
                        altChunk.Id = cid;
                        elem.InsertAfterSelf<AltChunk>(altChunk);
                        elem.Remove();
                    }
                }
                DocumentSettingsPart settingsPart = mainDocPart.Document.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();
                // Create object to update fields on open
                UpdateFieldsOnOpen updateFields = new UpdateFieldsOnOpen();
                updateFields.Val = new DocumentFormat.OpenXml.OnOffValue(true);
                // Insert object into settings part.
                settingsPart.Settings.PrependChild<UpdateFieldsOnOpen>(updateFields);
                settingsPart.Settings.Save();
                wordDoc.Close();
                return Json("Sucess");

            }
            catch (Exception ex)
            {
                //string ErrorData = "Please Close the Word and try again";
                return Json(ex.Message);
            }
            return Json("Sucess");

        }
        public void GenerateDoc()
        {
            //var strBody = new StringBuilder();
            //string UrlLink = new Uri(Request.Url, Url.Content("~")).AbsoluteUri + "ImageBrowser/Image";
            //string source = "/ImageBrowser/Image";
            //string CON = htmlString;
            //htmlString = CON.Replace(source, UrlLink);

            //AddFormatting(strBody, htmlString);
            //Response.Clear();
            //Response.Charset = "";
            //Response.ContentType = "application/msword";
            //string strFileName = "Amitiza(Periodic Benefit Risk Evaluation Report(PBRER))" + ".doc";
            //Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
            //Response.Write(strBody.ToString());
            //Response.End();
            //Response.Flush();
            System.IO.FileInfo file = new System.IO.FileInfo(destinationFile);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }

            //  return View();
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


        public ActionResult SubmitReport()
        {
            var proDocSecAssList = objprmdl.SubmitReport(prodocid);
            return Json(proDocSecAssList, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public string SubmitSecAsignRecordByUser(int id, string comment, string section, string status, string templateContent) {

            TemplateSectionVo template = new TemplateSectionVo();
            template.ProDoc_Section_Assignment_id = id;
            template.Comment = comment;
            template.Section = section;
            template.Status = status;
            template.Template_Content = templateContent;

            var model = new ProductModel();
            var res = model.SubmitRecordByUser(template);

            return res;
        }

        [ValidateInput(false)]
        [HttpPost]
        public string SubmitActAsignRecordByUser(int id, string comment, string status, string templateContent)
        {

            TemplateSectionVo template = new TemplateSectionVo();
            template.ProDoc_Template_id = id;
            template.Comment = comment;
            template.Status = status;
            template.Template_Content = templateContent;

            var model = new ProductModel();
            var res = model.SubmitActRecordByUser(template);

            return res;
        }

        [ValidateInput(false)]
        [HttpPost]
        public string SubmitAuthRevSecAsignRecordByUser(int id, string comment, string status, string templateContent,string accessas)
        {

            TemplateSectionVo template = new TemplateSectionVo();
            template.ProDoc_Section_Assignment_id = id;
            template.Comment = comment;
            template.Status = status;
            template.Template_Content = templateContent;
            template.Access_as = accessas;
            var model = new ProductModel();
            var res = model.SubmitAuthRevSecRecordByUser(template);

            return res;
        }

        [ValidateInput(false)]
        [HttpPost]
        public string SubmitAuthRevActAsignRecordByUser(int id, string comment, string status, string templateContent, string accessas)
        {

            TemplateSectionVo template = new TemplateSectionVo();
            template.ProDoc_Template_id = id;
            template.Comment = comment;
            template.Status = status;
            template.Template_Content = templateContent;
            template.Access_as = accessas;

            var model = new ProductModel();
            var res = model.SubmitAuthRevActRecordByUser(template);

            return res;
        }

        /*
         * chart count for product wise in manager dashboard
         */
        [HttpPost]
        public JsonResult GetProductWiseCount() {
            var model = objprmdl.GetUserChartDetails();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReportWiseCount()
        {
            var model = objprmdl.GetUserReportChartDetails();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDocumentWiseCount() {
            var model = objprmdl.GetUserDocumentChartDetails();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CheckTemplateAvailability(string receivedId)
        {
            var response = objprmdl.CheckTemplateavailability(receivedId);
            return response;
        }
        [HttpPost]
        public string CheckRoleAvailability(string receivedId)
        {
            var response = objprmdl.CheckRoleAvailability(receivedId);
            return response;
        }

        public ActionResult UsersStatus()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetUserData(string selectedItem = "0")
        {
            var res = objprmdl.GetUsersData(selectedItem);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserActivityChart(string selectedItem = "0")
        {
            var res = objprmdl.GetUserActivityChartDetails(selectedItem);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserSectionChart(string selectedItem = "0")
        {
            var res = objprmdl.GetUserSectionChartDetails(selectedItem);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string Editsection(String prodocsecID)
        {
            if (HttpContext.Application["EditUserSection"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Application["EditUserSection"];
                if (dt.Rows.Count < 1)
                {
                    DataRow workRow;
                    workRow = dt.NewRow();
                    workRow[0] = Session["UserID"];
                    workRow[1] = prodocsecID;
                    workRow[2] = Session["Name"];

                    dt.Rows.Add(workRow);
                    return "falsenooneisusing";
                }
                else
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["SectionNo"].ToString() == prodocsecID && dr["UserID"].ToString() != Session["UserID"].ToString())
                        {
                            // do your deed
                            return dr["Name"].ToString();
                        }
                    }
                    //else
                    //{
                    DataRow workRow;
                    workRow = dt.NewRow();
                    workRow[0] = Session["UserID"];
                    workRow[1] = prodocsecID;
                    workRow[2] = Session["Name"];

                    dt.Rows.Add(workRow);
                    return "falsenooneisusing";
                    //  }
                }
            }
            return "falsenooneisusing";

        }
        [HttpGet]
        public void Deletesectionfromsession()
        {
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
                    //foreach (DataRow dr in dt.Rows)
                    //{
                        
                    //}
                    dt.AcceptChanges();

                    HttpContext.Application["EditUserSection"] = dt;

                }
                else
                {

                }
            }
        }
        public void ExtendTimeout()
        {
            string res = null;
          //  if (timeout > 0) //20 is the default timeout setting
           // {
                //HttpContext.Session.Clear();
                //HttpContext.Session.RemoveAll();
              //  res = System.Web.HttpContext.Current.Session.Timeout.ToString();
                HttpContext.Session.Timeout = 2;
                //filterContext.Result = new RedirectResult("/");
          //  }
          //  return res.ToString();
            // return Json(Session.Timeout);
        }
    }


}
  