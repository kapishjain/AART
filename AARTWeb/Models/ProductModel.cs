using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AARTServerVO;
using System.Web.Script.Serialization;

namespace AARTWeb.Models
{
    public class ProductModel : BaseModel
    {
        Auth Auth = new Auth();

        public Int32 productcount { get; set; }
        public Int32 documentcount { get; set; }
        public String getallproduct { get; set; }
        public String getreportsbyuser { get; set; }
        public String getprodocbyuser { get; set; }
        public List<ProDocTemplateByUserModel> getProductDocList { get; set; }
        public List<ProDocSectionByUserModel> getSecAssUserList { get; set; }
        public List<GetProDocTemplateForChartModel> getroDocTemplateForChart { get; set; }
        public List<GetProDocSectionAssignmentForChartModel> getProDocSectionAssignmentForChart { get; set; }
        public List<ProductDetails> productListDetails { get; set; }
        public List<AuthorProductDetails> authorproductListDetails { get; set; }

        public List<UsersByRole> leadAuthorList { get; set; }
        public List<UsersByRole> coAuthorList { get; set; }
        public List<TemplateSectionVo> getProDocSecAssList { get; set; }
        public List<ProductList> getAllProductList { get; set; }
        public List<DocTypeList> getAllDocTypeList { get; set; }
        public List<ComplexityList> getAllComplexityList { get; set; }
        public string newtemplate { get; set; }
        public string oldtemplate { get; set; }
        public List<DashBoardDetails> getDashBoardDetails { get; set; }
        public List<ManagerDashBoardDetails> getMangrDashBoardDetails { get; set; }

        public Int32 AddProduct(String prdctname, String prdctdesc)
        {
            if (IsProductExists(prdctname))
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(WebURL + "product/AddProduct");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                        object odata = new
                        {
                            userid = HttpContext.Current.Session["UserID"],
                            pro_name = prdctname,
                            pro_desp = prdctdesc
                        };

                        var myContent = JsonConvert.SerializeObject(odata);

                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                        var responseTask = httpClient.PostAsJsonAsync<Object>("", odata);
                        // var responseTask = httpClient.PostAsJsonAsync<("", myContent);

                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            using (HttpContent content = result.Content)
                            {
                                Task<string> result1 = content.ReadAsStringAsync();
                                var rs = result1.Result;
                                dynamic data = JsonConvert.DeserializeObject(rs);
                                string value = Convert.ToString(data);

                                if (value.Contains("error"))
                                {
                                    error = data.error;
                                    Message = data.error;
                                   // InsertAudit("Add new Product", Message, "Failed");

                                    return 0;
                                }
                                else if (value.Contains("warning"))
                                {
                                    Warning = true;
                                    error = data.warning;
                                    Message = data.warning;
                                   // InsertAudit("Add new Product", Message, "Failed");

                                    return 0;
                                }
                                else
                                {
                                    IsSuccess = true;
                                    Message = data.prodctid;
                                  //  InsertAudit("Add new Product", Message, "Success");

                                    return Convert.ToInt32(data.prodctid);
                                }
                            }
                        }
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                   // InsertAudit("Add new Product", error, "Failed");

                    return 0;
                }
            }
            else
            {
                Message = "Product name already Exists.";
                return 0;
            }
        }
        public bool IsProductExists(String prdctname)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = httpClient.GetAsync("product/IsProductExists?productname=" + prdctname);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);

                            if (value.Contains("info"))
                            {
                                Warning = true;
                                Message = data.info;
                                return true;
                            }
                            else if (value.Contains("error"))
                            {
                                error = data.error;
                                Message = data.error;
                                return false;
                            }


                        }
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        public String AddDocument(String docdetails)
        {
            String docvalues = Convert.ToString(docdetails);
            String[] docararr = docvalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            {
                for (int i = 0, j = 0; i < docararr.Length; i += 4, j++)
                {

                }

            }


            return null;
        }
        public void GetProductCount()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("Product/GetProductCount");
                rt.Wait();
                var r = rt.Result;
                if (r.IsSuccessStatusCode == true)
                {
                    string res = "";

                    var readTask = r.Content.ReadAsStringAsync();
                    res = readTask.Result;
                    dynamic data = JsonConvert.DeserializeObject(res);
                    string value = Convert.ToString(data);
                    if (value.Contains("error"))
                    {
                        error = data.error;
                    }
                    else
                    {
                        productcount = data.count;
                    }
                }
            }
        }
        public void GetDocumentCount()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("DocumentType/GetDocumentCount");
                rt.Wait();
                var r = rt.Result;
                if (r.IsSuccessStatusCode == true)
                {
                    string res = "";

                    var readTask = r.Content.ReadAsStringAsync();
                    res = readTask.Result;
                    dynamic data = JsonConvert.DeserializeObject(res);
                    string value = Convert.ToString(data);
                    if (value.Contains("error"))
                    {
                        error = data.error;
                    }
                    else
                    {
                        documentcount = data.count;
                    }
                }
            }


        }
        public List<ProductDetails> GetProductDetails()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(WebURL);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductDetails");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string res = "";
                    using (HttpContent content = result.Content)
                    {

                        // ... Read the string.
                        Task<string> result1 = content.ReadAsStringAsync();
                        res = result1.Result;
                        //JObject json = JObject.Parse(res);
                        //  json.de
                        dynamic data = JsonConvert.DeserializeObject(res);
                        string value = Convert.ToString(data);
                        if (value.Contains("error"))
                        {
                            error = data.error;
                        }
                        else
                        {
                            //resultList = data.result;
                            //result = res;
                            //productListDetails;                           
                            getallproduct = Convert.ToString(data.userarr);
                            productListDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(getallproduct);
                        }

                    }
                }
            }
            return productListDetails;
        }
        public List<UsersByRole> GetUsersForLeadAuthor()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Role/GetUserByRoleID?Roleid=" + 3);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UsersByRole>>();
                    rData.Wait();

                    leadAuthorList = rData.Result;
                }
            }
            return leadAuthorList;
        }
        public List<UsersByRole> GetUsersForCoAuthor()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Role/GetUserByRoleID?Roleid=" + 3);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UsersByRole>>();
                    try
                    {
                        rData.Wait();
                        coAuthorList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return coAuthorList;
        }
        public List<AuthorProductDetails> GetReportsByUser()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(WebURL);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetReportsByUser?userid=" + HttpContext.Current.Session["UserID"]);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string res = "";
                    using (HttpContent content = result.Content)
                    {
                        // ... Read the string.
                        Task<string> result1 = content.ReadAsStringAsync();
                        res = result1.Result;
                        //JObject json = JObject.Parse(res);
                        //  json.de
                        dynamic data = JsonConvert.DeserializeObject(res);
                        string value = Convert.ToString(data);
                        if (value.Contains("error"))
                        {
                            error = data.error;
                        }
                        else
                        {
                            //getreportsbyuser = Convert.ToString(data.result);
                            getreportsbyuser = Convert.ToString(data.result);
                            authorproductListDetails = JsonConvert.DeserializeObject<List<AuthorProductDetails>>(getreportsbyuser);

                        }

                    }


                }
            }
            return authorproductListDetails;

        }
        public void GetProDocByUser()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(WebURL);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProDocByUser?user_id=" + HttpContext.Current.Session["UserID"]);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string res = "";
                    using (HttpContent content = result.Content)
                    {
                        // ... Read the string.
                        Task<string> result1 = content.ReadAsStringAsync();
                        res = result1.Result;
                        //JObject json = JObject.Parse(res);
                        //  json.de
                        dynamic data = JsonConvert.DeserializeObject(res);
                        string value = Convert.ToString(data);
                        if (value.Contains("error"))
                        {
                            error = data.error;
                        }
                        else
                        {
                            getprodocbyuser = Convert.ToString(data);
                        }
                    }
                }
            }
        }
        public List<UsersByRole> GetUsersLi()
        {
            List<UsersByRole> getUserLi = new List<UsersByRole>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("role/GetUserByRole");
                rt.Wait();
                var result = rt.Result;
                if (result.IsSuccessStatusCode == true)
                {
                    //string res = "";
                    using (HttpContent content = result.Content)
                    {
                        var rData = result.Content.ReadAsAsync<List<UsersByRole>>();
                        rData.Wait();

                        getUserLi = rData.Result;

                    }
                }
                return getUserLi;
            }
        }
        public String getUsersList()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("role/GetUserByRole");
                rt.Wait();
                var result = rt.Result;
                if (result.IsSuccessStatusCode == true)
                {
                    string res = "";
                    using (HttpContent content = result.Content)
                    {
                        // ... Read the string.
                        Task<string> result1 = content.ReadAsStringAsync();
                        res = result1.Result;
                        //JObject json = JObject.Parse(res);
                        //  json.de
                        dynamic data = JsonConvert.DeserializeObject(res);
                        string value = Convert.ToString(data);
                        if (value.Contains("error"))
                        {
                            error = data.error;
                        }
                        else
                        {
                            return Convert.ToString(data);
                        }

                    }
                }
                return null;
            }
        }
        public String getRolesList()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("Role/GetAllRoles");
                rt.Wait();
                var result = rt.Result;
                if (result.IsSuccessStatusCode == true)
                {
                    string res = "";
                    using (HttpContent content = result.Content)
                    {
                        // ... Read the string.
                        Task<string> result1 = content.ReadAsStringAsync();
                        res = result1.Result;
                        //JObject json = JObject.Parse(res);
                        //  json.de
                        dynamic data = JsonConvert.DeserializeObject(res);
                        string value = Convert.ToString(data);
                        if (value.Contains("error"))
                        {
                            error = data.error;
                        }
                        else
                        {
                            return Convert.ToString(data);
                        }
                    }
                }
            }

            return null;
        }
        public List<ProDocTemplateByUserModel> GetProDocTemplateByUser()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetActivityByUser?user_id=" + HttpContext.Current.Session["UserID"]);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ProDocTemplateByUserModel>>();
                    rData.Wait();

                    getProductDocList = rData.Result;
                }
            }
            return getProductDocList;
        }

        /*
         * This method is used to get users sections list (logged in as user)
         */
        public List<ProDocSectionByUserModel> GetProDocSectionAssignmentByUser()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetSectionAssignmentByUser?user_id=" + HttpContext.Current.Session["UserID"]);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ProDocSectionByUserModel>>();
                    try
                    {
                        rData.Wait();
                        getSecAssUserList = rData.Result;
                        if (getSecAssUserList.Count < 0 || getSecAssUserList == null) {
                            Message = "No Sections Assigned to this user";
                        }
                    }
                    catch (Exception e) {
                        _ = e.InnerException;
                    }

                }
            }
            return getSecAssUserList;
        }
        //public string UpdateActityUser(TemplateSectionVo sectionVo)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var url = ConfigurationManager.AppSettings["WEBAPIURL"];

        //        httpClient.BaseAddress = new Uri(url);
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //        sectionVo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //        sectionVo.Last_Modified_By = Convert.ToString(11);
        //        sectionVo.Last_Modified_Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //        sectionVo.Status = "S";
        //        var responseTask = httpClient.PutAsJsonAsync("Product/UpdateActivityRecordByUser", sectionVo);
        //        responseTask.Wait();
        //        var result = responseTask.Result;

        //        if (result.IsSuccessStatusCode)
        //        {
        //            using (HttpContent content = result.Content)
        //            {
        //                Task<string> result1 = content.ReadAsStringAsync();
        //                var rs = result1.Result;
        //                dynamic data = JsonConvert.DeserializeObject(rs);
        //                string value = Convert.ToString(data);

        //                return value;
        //            }
        //        }
        //    }
        //    return "";
        //}
        public string UpdateActivityByUser(ProDocActivityVo objprovo)
        {
            TemplateSectionVo sectionVo = new TemplateSectionVo();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    sectionVo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    sectionVo.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();
                    sectionVo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    sectionVo.Template_Content = objprovo.Template_Content;
                    sectionVo.ProDoc_Template_id = Convert.ToInt32(objprovo.Pro_doc_template);
                    sectionVo.Status = "O";
                    //var str=
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(sectionVo);
                    var responseTask = httpClient.PutAsJsonAsync("Product/UpdateActivityRecordByUser", sectionVo);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Update actvity ", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Update actvity ", ex.Message, "Error");
                return null;

            }
        }
        public string UpdateSecAssUser(TemplateSectionVo sectionVo)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    sectionVo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    sectionVo.Last_Modified_By = Convert.ToString(11);
                    sectionVo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    sectionVo.Status = "O";
                    var responseTask = httpClient.PutAsJsonAsync("Product/UpdateSecAsignRecordByUser", sectionVo);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Update section ", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Update section ", ex.Message, "Success");
                return null;

            }
        }
        public List<GetProDocTemplateForChartModel> GetProDocTemplateForChart()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductActivityDtls");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<GetProDocTemplateForChartModel>>();
                    rData.Wait();

                    getroDocTemplateForChart = rData.Result;
                }
            }
            return getroDocTemplateForChart;
        }
        public List<GetProDocSectionAssignmentForChartModel> GetProDocSectionAssignmentForChart()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductSectionAssignmentDtls");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<GetProDocSectionAssignmentForChartModel>>();
                    rData.Wait();

                    getProDocSectionAssignmentForChart = rData.Result;
                }
            }
            return getProDocSectionAssignmentForChart;
        }
        public string UpdateProDocRole(ProductDetails product)
        {
            try
            {
                if (product.co_author.user_name == product.leadAuthor.user_name)
                {
                    return "Lead author and Co-Author can not be same";
                }
                if (product.leadAuthor.user_name==null)

                {
                    return "Please select lead Author";
                }
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var isNumeric = int.TryParse(product.co_author.user_name, out int n);
                    int leadauthoruserid;
                    int coleadauthoruserid;

                    if (int.TryParse(product.co_author.user_name, out int coleadauthoruserid1))
                    {
                        coleadauthoruserid = coleadauthoruserid1;

                    }
                    else
                    {
                        coleadauthoruserid = Convert.ToInt32(product.co_author.user_id);
                    }
                    if (int.TryParse(product.leadAuthor.user_name, out int leadauthoruserid1))
                        {
                        leadauthoruserid = leadauthoruserid1;

                    } else
                    {
                        leadauthoruserid = Convert.ToInt32(product.leadAuthor.user_id);
                    }
                    var responseTask = httpClient.PutAsJsonAsync("Product/UpdateAuthorInProduct?pdmid=" + product.pro_doc_id + "&lauserid=" + leadauthoruserid + "&coauserid=" + coleadauthoruserid + "&lmodifiedby=" + Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()) + "&lmodifieddate=" + DateTime.Now.ToString("dd/MMM/yyyy"), "");
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data.info);
                           // InsertAudit("Update Author or co-author for document", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Update Author or co-author for document", ex.Message, "Error");
                return null;

            }
        }

   
        public List<TemplateSectionVo> GetProDocActvity(int prodocmapid)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductDocumentActivityAuthoring?prodocmapid=" + prodocmapid + "&userid=" + Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<TemplateSectionVo>>();

                    try
                    {
                        rData.Wait();
                        getProDocSecAssList = rData.Result;
                        if (getProDocSecAssList.Count < 0) {
                            Message = "No Activities exists";
                        }
                    }
                    catch (Exception e) {
                        _ = e.InnerException;
                    }

                }
            }
            return getProDocSecAssList;
        }
        public List<TemplateSectionVo> GetProDocSecAss(int prodocmapid)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductDocumentAuthoring?prodocmapid=" + prodocmapid + "&userid=" + Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<TemplateSectionVo>>();

                    try
                    {
                        rData.Wait();
                        getProDocSecAssList = rData.Result;
                        if (getProDocSecAssList.Count < 0)
                        {
                            Message = "No Sections exists";
                        }
                    }
                    catch (Exception e) {
                        _ = e.InnerException;
                    }

                }
            }
            return getProDocSecAssList;
        }
        public string UpdateProDocSecAss(TemplateSectionVo templateSection)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    templateSection.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    templateSection.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();
                    templateSection.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    if (templateSection.Assigned_Username == HttpContext.Current.Session["UserName"].ToString())
                        templateSection.Status = "O";
                    else
                        templateSection.Status = "I";
                    //List<TemplateSectionVo> tList = new List<TemplateSectionVo>();
                    //tList.Add(templateSection);
                    var responseTask = httpClient.PutAsJsonAsync("Product/UpdateSecAsignRecordByUser", templateSection);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Update section ", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Update section ", ex.Message, "Error");
                return null;

            }
        }
        public string UpdateProDocActAss(TemplateSectionVo templateSection)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    templateSection.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    templateSection.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();
                    templateSection.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    templateSection.Status = "O";
                    //  List<TemplateSectionVo> tList = new List<TemplateSectionVo>();
                    // tList.Add(templateSection);
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(templateSection);
                    var responseTask = httpClient.PutAsJsonAsync("Product/UpdateActivityRecordByUser", templateSection);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Update actvity ", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
              //  InsertAudit("Update actvity ", ex.Message, "Error");
                return null;

            }
        }
        public List<UsersByRole> GetFilteredUsersList(int id) {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Role/GetUserByRoleID?Roleid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UsersByRole>>();

                    try
                    {
                        rData.Wait();

                        leadAuthorList = rData.Result;
                    }
                    catch (Exception e) {
                        _ = e.InnerException;
                    }

                }
            }
            return leadAuthorList;
        }

        public List<ProductList> GetAllProductList()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetAllProduct");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ProductList>>();
                    try
                    {
                        rData.Wait();
                        getAllProductList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return getAllProductList;
        }
        public List<DocTypeList> GetAllDocTypeList()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("DocumentType/GetAllDoctype");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<DocTypeList>>();
                    try
                    {
                        rData.Wait();
                        getAllDocTypeList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return getAllDocTypeList;
        }
        public List<ComplexityList> GetAllComplexityList()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("DocumentType/GetDocumentComplexity");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ComplexityList>>();
                    try
                    {
                        rData.Wait();
                        getAllComplexityList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return getAllComplexityList;
        }
        public string InsertNewProducts(List<InsertProducts> models, string protext)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string value = null;
                    List<ProductDocumentMappingVo> lstpdm = new List<ProductDocumentMappingVo>();

                    ProductDocumentMappingVo vo;
                    var tObj = new Object();

                    foreach (var item in models)
                    {
                        vo = new ProductDocumentMappingVo();
                        vo.ProductName = protext;
                        vo.DocumentName = item.DocumentType.document_name;
                        vo.LeadAuthorUserId = Convert.ToInt32(item.MainAuthor.user_id);
                        vo.CoLeadAuthorUserId = Convert.ToInt32(item.CoAuthor.user_id);
                        vo.Complexity = Convert.ToInt32(item.Complexity.document_complexity_id);
                        vo.ReviewPreiodFrom = item.ReviewPeriodFrom;
                        vo.ReviewPeriodTo = item.ReviewPeriodTo;
                        vo.Status = "I";
                        vo.Dlp = item.ReviewPeriodTo;
                        vo.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]); ;
                        vo.CreatedDate = DateTime.Now.ToString("dd/MMM/yyyy");
                        vo.LastModifiedBy = HttpContext.Current.Session["UserID"].ToString();
                        vo.LastModifiedDate = DateTime.Now.ToString("dd/MMM/yyyy");
                        lstpdm.Add(vo);
                    }

                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = httpClient.PostAsJsonAsync("Product/InsertAdhocReport", lstpdm);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);

                            value = Convert.ToString(data);

                            if (value.Contains("info"))
                            {
                                value = Convert.ToString(data.info);
                               // InsertAudit("Insert new product", value, "Success");

                            }
                            return value;
                        }

                    }
                }
                catch (Exception ex)
                {
                   // InsertAudit("Insert new product", ex.Message, "Error");

                    return ex.Message;

                }
                return null;
            }
        }
        public string UpdateAcceptReject(int id, string status, string comment, int prodocId, string template, string templateVar)
        {

            //var restClient = new RestClient(WebURL);
            //RestRequest request = new RestRequest("", Method.POST);
            //request.AddParameter("Authorization", string.Format("Bearer " + HttpContext.Current.Session["token"].ToString()), ParameterType.HttpHeader);

            ReviewCommentVO commentVO = new ReviewCommentVO();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (templateVar == "Section")
                        commentVO.Pro_Doc_Section_Assignment_Id = Convert.ToString(id);
                    else
                        commentVO.Pro_Doc_Template = Convert.ToString(id);
                    commentVO.Pro_Doc_Mapping_Id = Convert.ToString(prodocId);
                    commentVO.Status = status;
                    commentVO.Reviewer_Datetime = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    commentVO.Review_Comment = comment;
                    commentVO.Newtemplate = template;
                    commentVO.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();
                    commentVO.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    commentVO.User_id = HttpContext.Current.Session["UserID"].ToString();
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(commentVO);
                    if (templateVar == "Section")
                    {
                        var responseTask = httpClient.PostAsJsonAsync("Product/InsUpdtProDocSectionReview", commentVO);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            using (HttpContent content = result.Content)
                            {
                                Task<string> result1 = content.ReadAsStringAsync();
                                var rs = result1.Result;
                                dynamic data = JsonConvert.DeserializeObject(rs);
                                string value = Convert.ToString(data);
                               // InsertAudit("Accept or Revert section ", value, "Success");

                                return value;
                            }

                        }
                    }
                    else
                    {
                        var responseTask = httpClient.PostAsJsonAsync("Product/InsUpdtActivityReview", commentVO);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            using (HttpContent content = result.Content)
                            {
                                Task<string> result1 = content.ReadAsStringAsync();
                                var rs = result1.Result;
                                dynamic data = JsonConvert.DeserializeObject(rs);
                                string value = Convert.ToString(data);
                               // InsertAudit("Accept or Revert actvity ", value, "Success");

                                return value;
                            }

                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Accept or Revert Section ", ex.Message, "Error");
                return "";
            }
        }
        public List<ReviewCommentVO> GetViewData(int id, string from)
        {
            List<ReviewCommentVO> getViewDat = new List<ReviewCommentVO>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (from == "Section")
                {
                    var responseTask = httpClient.GetAsync("Product/GetReviewSectionCommentsById?prodocsecid=" + id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rData = result.Content.ReadAsAsync<List<ReviewCommentVO>>();
                        try
                        {
                            rData.Wait();
                            getViewDat = rData.Result;
                        }
                        catch (Exception e)
                        {
                            _ = e.InnerException;
                        }

                    }
                }
                else {
                    var responseTask = httpClient.GetAsync("Product/GetReviewActivityCommentsById?prodoctempid=" + id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rData = result.Content.ReadAsAsync<List<ReviewCommentVO>>();
                        try
                        {
                            rData.Wait();
                            getViewDat = rData.Result;
                        }
                        catch (Exception e)
                        {
                            _ = e.InnerException;
                        }

                    }
                }


            }
            return getViewDat;
        }
        public ReviewCommentVO GetDiffData(int id, string from)
        {
            List<ReviewCommentVO> model = new List<ReviewCommentVO>();
            ReviewCommentVO reviewComment = new ReviewCommentVO();

            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (from == "Section")
                {
                    var responseTask = httpClient.GetAsync("Product/GetDifferenceSection?revwsecid=" + id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rData = result.Content.ReadAsAsync<List<ReviewCommentVO>>();
                        try
                        {
                            rData.Wait();
                            model = rData.Result;
                            reviewComment = model[0];
                        }
                        catch (Exception e)
                        {
                            _ = e.InnerException;
                        }

                    }
                }
                else {
                    var responseTask = httpClient.GetAsync("Product/GetDifferenceActivity?revwactid=" + id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rData = result.Content.ReadAsAsync<List<ReviewCommentVO>>();
                        try
                        {
                            rData.Wait();
                            model = rData.Result;
                            reviewComment = model[0];
                        }
                        catch (Exception e)
                        {
                            _ = e.InnerException;
                        }

                    }
                }

            }
            return reviewComment;
        }
        public List<TemplateSectionVo> GetProDocActAss(int prodocmapid)
        {
            List<TemplateSectionVo> sectionVos = new List<TemplateSectionVo>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductDocumentActivityAuthoring?prodocmapid=" + prodocmapid + "&userid=" + Convert.ToInt32(HttpContext.Current.Session["UserID"]));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<TemplateSectionVo>>();

                    try
                    {
                        rData.Wait();
                        sectionVos = rData.Result;
                    }
                    catch (Exception e) {
                        _ = e.InnerException;
                    }

                }
            }
            return sectionVos;
        }
        public string SubmitReport(int prodocid)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //   templateSection.Status = "O";
                    TemplateSectionVo tList = new TemplateSectionVo();
                    //  tList.Add(templateSection);
                    var responseTask = httpClient.GetAsync("Product/submitreport?prodocid=" + prodocid + "&user_id=" + HttpContext.Current.Session["UserID"] + "&comments= Work Done");

                    //   var responseTask = httpClient.GetAsync("Product/SubmitReport?prodoc_map_id=" + prodocid + ")";
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Submit complete report", value, "Success");

                            return value;
                        }
                    }
                }
                return "";

            }
            catch (Exception ex)
            {
               // InsertAudit("Submit complete report", ex.Message, "Error");
                return "";

            }
        }
        public string SubmitRecordByUser(TemplateSectionVo vo) {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    vo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    vo.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();

                    vo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    vo.Status = "I";
                    var responseTask = httpClient.PutAsJsonAsync("Product/SubmitSecAsignRecordByUser", vo);


                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                           // InsertAudit("Submit section", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Submit section", ex.Message, "Error");
                return null;
            }
        }

        public string SubmitActRecordByUser(TemplateSectionVo vo)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    vo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    vo.Status = "I";
                    vo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    vo.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();

                    var responseTask = httpClient.PutAsJsonAsync("Product/SubmitActivityRecordByUser", vo);


                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                          //  InsertAudit("Submit activity", value, "Success");

                            return value;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               // InsertAudit("Submit activity", ex.Message, "Error");
                return null;

            }
        }

        public string SubmitAuthRevSecRecordByUser(TemplateSectionVo vo)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                vo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                if(vo.Access_as== "Lead Author" || vo.Access_as == "Maanger")
                    vo.Status = "S";
                else
                    vo.Status = "I";

                vo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                vo.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();

                var responseTask = httpClient.PutAsJsonAsync("Product/SubmitSecAsignRecordByUser", vo);
                

                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    using (HttpContent content = result.Content)
                    {
                        Task<string> result1 = content.ReadAsStringAsync();
                        var rs = result1.Result;
                        dynamic data = JsonConvert.DeserializeObject(rs);
                        string value = Convert.ToString(data);
                      //  InsertAudit("Submit section", value, "Success");

                        return value;
                    }
                }
            }
            return "";
        }

        public string SubmitAuthRevActRecordByUser(TemplateSectionVo vo)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                vo.User_id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                vo.Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                vo.Last_Modified_By = HttpContext.Current.Session["UserID"].ToString();
                if (vo.Access_as == "Lead Author" || vo.Access_as == "Maanger")
                    vo.Status = "S";
                else
                    vo.Status = "I";
                var responseTask = httpClient.PutAsJsonAsync("Product/SubmitActivityRecordByUser", vo);


                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    using (HttpContent content = result.Content)
                    {
                        Task<string> result1 = content.ReadAsStringAsync();
                        var rs = result1.Result;
                        dynamic data = JsonConvert.DeserializeObject(rs);
                        string value = Convert.ToString(data);
                      //  InsertAudit("Submit activity", value, "Success");

                        return value;
                    }
                }
            }
            return "";
        }

        /*
         * Representation of Dashboard values in AuthorReview page
         */
        public List<DashBoardDetails> GetDashBoardDetails() {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetCountActivitySectionByUser?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<DashBoardDetails>>();
                    try
                    {
                        rData.Wait();
                        getDashBoardDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return getDashBoardDetails;
        }
        /*
         * Representation of Manager DashBoard values in Manager Page
         */
        public List<ManagerDashBoardDetails> GetManagerDashBoardDetails()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetProductCountByUser?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ManagerDashBoardDetails>>();
                    try
                    {
                        rData.Wait();
                        getMangrDashBoardDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return getMangrDashBoardDetails;
        }

        /*
         * Getting chart data for manager
         */
        public List<UserChartDetails> GetUserChartDetails() {
            List<UserChartDetails> chartDetails = new List<UserChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/ProductWiseCount?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UserChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return chartDetails;
        }
        public List<UserChartDetails> GetUserReportChartDetails()
        {
            List<UserChartDetails> chartDetails = new List<UserChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetTotalCount?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UserChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return chartDetails;
        }

        public List<UserChartDetails> GetUserDocumentChartDetails()
        {
            List<UserChartDetails> chartDetails = new List<UserChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetDetailsDocWiseCount?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UserChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return chartDetails;
        }
        public List<ChartDetails> GetUserCountdetailsActivity()
        {
            List<ChartDetails> chartDetails = new List<ChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetUserCountdetailsActivity?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return chartDetails;
        }
        public List<ChartDetails> GetUserCountdetailsSection()
        {
            List<ChartDetails> chartDetails = new List<ChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];
                var id = HttpContext.Current.Session["UserId"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetUserCountdetailsSection?userid=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }

                }
            }
            return chartDetails;
        }
        public string CheckTemplateavailability(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var responseTask = httpClient.GetAsync("Product/CheckTemplateByProDocMapId?prodocmapid=" + id);




                responseTask.Wait();
                var result = responseTask.Result;



                if (result.IsSuccessStatusCode)
                {
                    using (HttpContent content = result.Content)
                    {
                        Task<string> result1 = content.ReadAsStringAsync();
                        var rs = result1.Result;
                        dynamic data = JsonConvert.DeserializeObject(rs);
                        string value = Convert.ToString(data);

                        return value;
                    }
                }
            }
            return "";
        }
        public string CheckRoleAvailability(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                var responseTask = httpClient.GetAsync("Product/CheckUsersForAllRole?prodocmapid=" + id);




                responseTask.Wait();
                var result = responseTask.Result;



                if (result.IsSuccessStatusCode)
                {
                    using (HttpContent content = result.Content)
                    {
                        Task<string> result1 = content.ReadAsStringAsync();
                        var rs = result1.Result;
                        dynamic data = JsonConvert.DeserializeObject(rs);
                        string value = Convert.ToString(data);



                        return value;
                    }
                }
            }
            return "";
        }
        public List<DashBoardDetails> GetUsersData(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync(""); ;
                if (id == "0")
                {
                    responseTask = httpClient.GetAsync("Product/GetCountActivitySectionAllUser");
                }
                else
                {
                    responseTask = httpClient.GetAsync("Product/GetCountActivitySectionByUser?userid=" + id);
                }

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<DashBoardDetails>>();
                    try
                    {
                        rData.Wait();
                        getDashBoardDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }



                }
            }
            return getDashBoardDetails;
        }
        public List<ChartDetails> GetUserActivityChartDetails(string id)
        {
            List<ChartDetails> chartDetails = new List<ChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("");
                if (id == "0")
                {
                    responseTask = httpClient.GetAsync("Product/GetAllUserCountdetailsActivity");
                }
                else
                {
                    responseTask = httpClient.GetAsync("Product/GetUserCountdetailsActivity?userid=" + id);
                }
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }



                }
            }
            return chartDetails;
        }
        public List<ChartDetails> GetUserSectionChartDetails(string id)
        {
            List<ChartDetails> chartDetails = new List<ChartDetails>();
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];



                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("");
                if (id == "0")
                {
                    responseTask = httpClient.GetAsync("Product/GetAllUserCountdetailsSection");
                }
                else
                {
                    responseTask = httpClient.GetAsync("Product/GetUserCountdetailsSection?userid=" + id);
                }
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<ChartDetails>>();
                    try
                    {
                        rData.Wait();
                        chartDetails = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }



                }
            }
            return chartDetails;
        }
        public class ManagerDashBoardDetails {
            public int documentCount { get; set; }
            public int toBeInitatedCount { get; set; }
            public int onGoingCount { get; set; }
            public int CompletedCount { get; set; }
        }
        public class ProDocTemplateByUserModel
        {
            public int document_template_id { get; set; }
            public int product_document_mapping_id { get; set; }
            public int pro_doc_template { get; set; }

            public string product_name { get; set; }
            public string document_name { get; set; }
            public string action { get; set; }
            public string aggred_timeline { get; set; }
            public string target_timeline { get; set; }
            public string comment { get; set; }
            public string status { get; set; }
            public string template_content { get; set; }
            public string prodocid { get; set; }

        }
        public class ProDocSectionByUserModel
        {
            public int prodoc_section_assignment_id { get; set; }
            public string product_name { get; set; }
            public string document_name { get; set; }
            public string section { get; set; }
            public string template_content { get; set; }
            public string target_timeline { get; set; }
            public string status { get; set; }
            public string prodocid { get; set; }

        }
        public class GetProDocTemplateForChartModel
        {
            public int id { get; set; }
            public int pro_doc_id { get; set; }
            public string tittle { get; set; }
            public int productID { get; set; }
            public int orderID { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public float percentComplete { get; set; }
            public string summary { get; set; }
            public string expanded { get; set; }
        }
        public class GetProDocSectionAssignmentForChartModel
        {
            public int id { get; set; }
            public int? parent_id { get; set; }
            public int pro_doc_id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string expanded { get; set; }
            public string summary { get; set; }
            public string percentComplete { get; set; }
        }
        public class ProductDetails
        {
            public int pro_doc_id { get; set; }
            public string product_description { get; set; }
            public string document_name { get; set; }
            public string fromDate { get; set; }
            public string todate { get; set; }
        //   public string leadAuthor { get; set; }
            public int leadAuthorid { get; set; }
          //  public string co_author { get; set; }
            public int co_authorid { get; set; }
            public string status { get; set; }
            public string progress { get; set; }
            public string reporting_interval { get; set; }
            public string completion_date { get; set; }
            public string cutoff_date { get; set; }
            public string reviewreport { get; set; }
            public string comments { get; set; }
            public string prodocid { get; set; }

            public leadAuthor1 leadAuthor { get; set; }

            public co_author1 co_author { get; set; }

        }
        public class AuthorProductDetails
        {
            public int pro_doc_id { get; set; }
            public string product_description { get; set; }
            public string document_name { get; set; }
            public string fromDate { get; set; }
            public string todate { get; set; }
            public string leadAuthor { get; set; }
            public int leadAuthorid { get; set; }
            public string co_author { get; set; }
            public int co_authorid { get; set; }
            public string status { get; set; }
            public string progress { get; set; }
            public string reporting_interval { get; set; }
            public string completion_date { get; set; }
            public string cutoff_date { get; set; }
            public string comments { get; set; }
            public string reviewreport { get; set; }
            public string prodocid { get; set; }
            public string lead_author_user_id { get; set; }
            public string co_lead_author_user_id { get; set; }
            public string Worked_as { get; set; }


            //public leadAuthor1 leadAuthor { get; set; }

            //public co_author1 co_author { get; set; }

        }
        public class UsersByRole
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
            public string role_id { get; set; }
        }
        public class leadAuthor1
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
        }
        public class co_author1
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
        }
        public class InsertProducts
        {
            public DocTypeList DocumentType { get; set; }
            public UsersByRole MainAuthor { get; set; }
            public UsersByRole CoAuthor { get; set; }
            public ComplexityList Complexity { get; set; }
            public string ReviewPeriodFrom { get; set; }
            public string ReviewPeriodTo { get; set; }
        }
        public class ProductList
        {
            public string product_id { get; set; }
            public string product_description { get; set; }
        }
        public class DocTypeList
        {
            public string document_code { get; set; }
            public string document_name { get; set; }
        }
        public class ComplexityList
        {
            public string document_complexity_id { get; set; }
            public string complexity_description { get; set; }
            public string kom_days { get; set; }
        }
        public class DashBoardDetails {
            public string name { get; set; }
            public int total { get; set; }
            public int ToBeInitiated { get; set; }
            public int onGoing { get; set; }
            public int submited { get; set; }
            public int reverted { get; set; }
            public int toBeRevied { get; set; }
        }

        public class UserChartDetails
        {
            public string product_name { get; set; }
            public int total_Count { get; set; }
            public string document_code { get; set; }            
            public int tobeinitated { get; set; }
            public int onGoing { get; set; }
            public int Completed { get; set; }
            public string status { get; set; }
            public int count { get; set; }

           
        }
        public class ChartDetails
        {
            public string product_name { get; set; }
            public string document_name { get; set; }
            public int Tobeinitiated { get; set; }
            public int OnGoing { get; set; }
            public int SubmittedForReview { get; set; }
            public string Reverted { get; set; }
            public int Accepted { get; set; }
        }
        public string ResMessage {
            get {
                return Message;
            }
        }
    }

}
