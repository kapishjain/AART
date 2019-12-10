using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AARTServerVO;
using System.Configuration;
using System.Web.Script.Serialization;

namespace AARTWeb.Models
{
    public class KickOffMeetingModel : BaseModel
    {
       // Auth Auth = new Auth();

        public String getprodocdtls { get; set; }
        public String getprodoctemplate { get; set; }
        public String getprodocsecassignment { get; set; }
        public Int32 pro_doc_id;
        public List<AssgnActProdList> getAssActByProIdList { get; set; }
        public List<AssgnSecProdList> getAssSecByProIdList { get; set; }

        public void GetProDocDetails(int prodocid)
        {
            pro_doc_id = prodocid;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("Product/GetPDMDetails?pdmid=" + prodocid);
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
                            getprodocdtls = Convert.ToString(data);
                        }

                    }
                }
            }
        }
        public void GetProDocTemplate(int prodocid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("Product/GetProDocTemplet?pdmid=" + prodocid);
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
                            getprodoctemplate = Convert.ToString(data);
                        }

                    }
                }
            }
        }
        public void GetProDocSecAssignment(int prodocid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rt = client.GetAsync("Product/GetProDocSecAssignment?pdmid=" + prodocid);
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
                            getprodocsecassignment = Convert.ToString(data);
                        }

                    }
                }
            }
        }
        public List<AssgnActProdList> GetAssignedActivitybyProDocIdList(int pro_doc_id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetAssignedActivitybyProDocId?prodocmapid=" + pro_doc_id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<AssgnActProdList>>();
                    try
                    {
                        rData.Wait();
                        getAssActByProIdList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }
                }
            }
            return getAssActByProIdList;
        }
        public List<AssgnSecProdList> GetAssignedSectionbyProDocIdList(int pro_doc_id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Product/GetAssignedSectionAssinmentByProDocId?prodocmapid=" + pro_doc_id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<AssgnSecProdList>>();
                    try
                    {
                        rData.Wait();
                        getAssSecByProIdList = rData.Result;
                    }
                    catch (Exception e)
                    {
                        _ = e.InnerException;
                    }
                }
            }
            return getAssSecByProIdList;
        }
        public string UpdateAssgnActPro(AssgnActProdList pObj)
        {
            string value = null;

            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<Object> pList = new List<Object>();

                var nObj = new
                {
                    ProDoc_Template_id = pObj.pro_doc_template,
                    User_id = pObj.user.user_id,
                    Last_Modified_By = HttpContext.Current.Session["UserID"].ToString(),
                    Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")
            };

                pList.Add(nObj);

                var responseTask = httpClient.PutAsJsonAsync("Product/ReassignActivityAsign", pList);
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

                        if (value.Contains("error"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.error;
                            //InsertAudit("Reassign user for actvity", Message, "Failed");

                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                          //  InsertAudit("Reassign user for actvity", Message, "Failed");

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            getAssActByProIdList = GetAssignedActivitybyProDocIdList(pro_doc_id);
                           // InsertAudit("Reassign user for actvity", Message, "Success");

                            return value;
                        }
                    }
                }
            }
            return value;
        }
        public string UpdateAssgnSecPro(AssgnSecProdList sObj)
        {
            string value = null;

            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                List<Object> sList = new List<Object>();
                var nObj = new
                {
                    ProDoc_Section_Assignment_id = sObj.pro_doc_section_assignment_id,
                    User_id = sObj.user_id.user_id,
                    Last_Modified_By = HttpContext.Current.Session["UserID"].ToString(),
                    Last_Modified_Date = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")
                };

                sList.Add(nObj);

                var responseTask = httpClient.PutAsJsonAsync("Product/ReassignSecAssign", sList);
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

                        if (value.Contains("error"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.error;
                          //  InsertAudit("Reassign user for section", Message, "Failed");

                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                         //   InsertAudit("Reassign user for section", Message, "Failed");

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            getAssSecByProIdList = GetAssignedSectionbyProDocIdList(pro_doc_id);
                          //  InsertAudit("Reassign user for section", Message, "Success");

                            return value;
                        }
                    }
                }
            }
            return value;
        }
        public string InsertProDocKOMDtls(List<KickOffMeetingModel.ProDocAttendies> models, List<ProDocActivityVoforKOM> docactivity, List<ProDocSectionAssignmentVoforKOM> docsection)
        {
            List<ProDocAttendiesVO> lstattendies = new List<ProDocAttendiesVO>();
            List<ProDocActivityVoforKOM> lstactivity = new List<ProDocActivityVoforKOM>();
            List<ProDocSectionAssignmentVoforKOM> lstdoc = new List<ProDocSectionAssignmentVoforKOM>();
            try
            {

                foreach (var i in models)
                {
                    Attendiesrole role = i.attendiesrole;
                    Attendiesuser user = i.attendiesuser;
                    Attendiesbehalfof behalfof = i.attendiesbehalfof;
                    ProDocAttendiesVO objattendies = new ProDocAttendiesVO();

                    if (role != null)
                    {
                        objattendies.Role_ID = role.role_id;
                        objattendies.Role_Name = role.role_name;

                    }
                    if (user != null)
                    {
                        objattendies.User_Id = user.user_id;
                        objattendies.User_Name = user.user_name;
                    }
                    if (behalfof != null)
                    {
                        objattendies.Behalfuser_Id = behalfof.user_id;
                        objattendies.Behalfuser_name = behalfof.user_name;

                    }
                    objattendies.ProDocMap_Id = i.prodocid.ToString();
                    objattendies.created_by = HttpContext.Current.Session["UserID"].ToString();
                    objattendies.Created_Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    lstattendies.Add(objattendies);
                }
                foreach (ProDocActivityVoforKOM objactivity in docactivity)
                {
                    objactivity.Created_By = HttpContext.Current.Session["UserID"].ToString();
                    objactivity.Created_Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    objactivity.Status = "A";
                    lstactivity.Add(objactivity);
                }
                foreach (ProDocSectionAssignmentVoforKOM obsection in docsection)
                {
                    obsection.Created_By = HttpContext.Current.Session["UserID"].ToString();
                    obsection.Created_Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    obsection.Status = "A";
                    lstdoc.Add(obsection);
                }
                ArrayList myList = new ArrayList(10);
                myList.Add(lstattendies);
                myList.Add(lstactivity);
                myList.Add(lstdoc);
                Console.WriteLine(myList);
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(myList);
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL + "product/InsertProductKickOffMeetingDetails");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                    var responseTask = httpClient.PostAsJsonAsync<ArrayList>("", myList);
                    responseTask.Wait();
                    Console.Write(myList);

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        using (HttpContent content = result.Content)
                        {
                            Task<string> result1 = content.ReadAsStringAsync();
                            var rs = result1.Result;
                            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(rs);
                            string value = Convert.ToString(data);
                            if (value.Contains("warning"))
                            {
                                Message = data.warning;
                                Warning = true;
                              //  InsertAudit("Insert Kick-Off Metting", Message, "Failed");

                                return value;
                            }
                            if (value.Contains("error"))
                            {
                                Message = data.error;
                                Warning = true;
                              //  InsertAudit("Insert Kick-Off Metting", Message, "Failed");

                                return value;
                            }
                            else
                            {
                                Message = data.info;
                                IsSuccess = true;
                               // InsertAudit("Insert Kick-Off Metting", Message, "Success");

                                return value;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
              //  InsertAudit("Insert Kick-Off Metting", ex.Message, "Erroe");

                return null;
            }
        }
        public class ProDocAttendies
        {
            public Int32 prodocid
            { get; set; }
            public String created_by { get; set; }

            public Attendiesrole attendiesrole { get; set; }
            public Attendiesuser attendiesuser { get; set; }
            public Attendiesbehalfof attendiesbehalfof { get; set; }
        }


        public class Attendiesrole
        {
            public string role_id { get; set; }
            public string role_name { get; set; }
            public string role_description { get; set; }
        }
        public class Attendiesuser
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
        }
        public class Attendiesbehalfof
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
        }
        class Attendies
        {
            public string role_id { get; set; }
            public string user_id { get; set; }
            public string behalfuser_id { get; set; }

            public string created_by { get; set; }
            public string created_date { get; set; }

            public string ProDocMap_Id { get; set; }
        }
        public class AssgnActProdList
        {
            public int pro_doc_template { get; set; }
            public string document_template_id { get; set; }
            public string responsible_role_id { get; set; }
            public string role_name { get; set; }
            public User user { get; set; }
            public string action { get; set; }
            public string aggred_timeline { get; set; }
            public string target_timeline { get; set; }
            public string status { get; set; }
            public string comment { get; set; }
        }
        public class AssgnSecProdList
        {
            public string pro_doc_section_assignment_id { get; set; }
            public string document_section_assignment_id { get; set; }
            public string role_id { get; set; }
            public string role_name { get; set; }
            public string sectionid { get; set; }
            public string sectionname { get; set; }
            public User user_id { get; set; }
            public string target_date { get; set; }
            public string status { get; set; }
            public string comment { get; set; }
        }
        public class User
        {
            public int user_id { get; set; }
            public string user_name { get; set; }
        }
    }
        
    //public class ProDocActivity
    //{
    //    public int prodocid { get; set; }

    //    public string role_id { get; set; }
    //    public string user { get; set; }
    //    public string role_name { get; set; }

    //    public string action { get; set; }

    //    public string agree_Timeline { get; set; }
    //    public string target_Timeline { get; set; }
    //    public string remarks { get; set; }
    //}
    //public class ProDocSection
    //{
    //    public int prodocid { get; set; }

    //    public string role_id { get; set; }
    //    public string user { get; set; }
    //    public string role_name { get; set; }

    //    public string section_heading { get; set; }
    //    public string section_sub_id { get; set; }
    //    public string section_sub_heading { get; set; }
    //    public string target_timeline { get; set; }
    //    public string remarks { get; set; }
    //}
    //public class completesubmit
    //{
    //    public List<KickOffMeetingModel.ProDocAttendies> attendiesrole { get; set; }
    //    public List<ProDocActivity> ProDocActivity { get; set; }
    //    public List<ProDocSection> ProDocSection { get; set; }


    //}
}