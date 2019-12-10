using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;


namespace AARTWeb.Models
{
    public class AdminModel : BaseModel
    {
        Auth Auth = new Auth();

        public string allusers { get; set; }
        public string allroles { get; set; }
        public List<UsersModel> users { get; set; }
        public List<RolesModel> roles { get; set; }

        public List<UsersModel> GetAllUsers()
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("user/GetAllUsers");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rData = result.Content.ReadAsAsync<List<UsersModel>>();
                    rData.Wait();

                    users = rData.Result;
                }
                return users;
            }
        }
        public List<RolesModel> GetAllRoles()
        {
            using (var httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri("http://192.168.1.81:9810/api/user/");
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = httpClient.GetAsync("Role/GetAllRoles");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var roData = result.Content.ReadAsAsync<List<RolesModel>>();
                    roData.Wait();

                    roles = roData.Result;
                }
                else
                {

                }


                return roles;
            }
        }
        public string AddUser(UsersModel rUsers)
        {

            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var obj = rUsers;
                string rS = "";
                string rL = "";
                if (obj.status == "true")
                {
                    rS = "A";
                }
                else
                {
                    rS = "S";
                }

                if (obj.is_locked == "true")
                {
                    rL = "Y";
                }
                else
                {
                    rL = "N";
                }

                object cObj = new
                {
                    CreadtedBy = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()),
                    UserName = obj.user_name,
                    Name = obj.name,
                    Status = rS,
                    UserType = "User",
                    EmailID = obj.email_id,
                    Phone = Convert.ToInt64(obj.phone_number),
                    Is_Locked = rL,
                    Role_Id = Convert.ToInt32(obj.role_id.role_id)
                };

                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(cObj);
                var responseTask = httpClient.PostAsJsonAsync<object>("user/InsertUser", cObj);
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
                            Warning = true;
                            IsSuccess = false;
                            Message = data.error;
                            users = GetAllUsers();
                            //InsertAudit("Add user", Message, "Failed");

                            return Message;

                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                            users = GetAllUsers();
                            //InsertAudit("Add user", Message, "Failed");

                            return Message;

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            users = GetAllUsers();
                            //InsertAudit("Add user", Message, "Success");

                            return Message;

                            // users = GetAllUsers();
                        }
                    }

                }
                return "";

            }
        }
        public string UpdateUser(UsersModel uUsers)
        {
            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var obj = uUsers;
                string rS = "";
                string rL = "";
                if (obj.status == "true")
                {
                    rS = "A";
                }
                else
                {
                    rS = "S";
                }

                if (obj.is_locked == null || obj.is_locked == "false")
                {
                    rL = "N";
                }
                else
                {
                    rL = "Y";
                }

                object cObj = new
                {
                    User_Id = obj.user_id,
                    LastModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()),
                    Name = obj.name,
                    Status = rS,
                    UserType = "User",
                    EmailID = obj.email_id,
                    Phone = Convert.ToInt64(obj.phone_number),
                    Is_Locked = rL,
                    Role_Id = Convert.ToInt32(obj.role_id.role_id)
                };


                var responseTask = httpClient.PostAsJsonAsync<object>("User/UpdateUserInfo", cObj);
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
                            Warning = true;
                            IsSuccess = false;
                            Message = data.error;
                            //InsertAudit("Update user details", Message, "Failed");

                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                            //InsertAudit("Update user details", Message, "Failed");
                        }
                        else
                        {
                            IsSuccess = true;
                            Message = value;
                            System.Threading.Thread.Sleep(1000);
                            users = GetAllUsers();
                            //InsertAudit("Update user details", Message, "Failed");
                            return Message;
                        }
                    }

                }

            }
            return Message;
        }
        public string AddRole(RolesModel aRoles)
        {
            using (var httpClient = new HttpClient())
            {

                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string rS = "";
                var obj = aRoles;
                if (obj.status == "false")
                {
                    rS = "I";
                }
                else
                {
                    rS = "A";
                }


                object cObj = new
                {
                    RoleName = obj.role_name,
                    RoleDescription = obj.role_description,
                    Status = rS,
                    CreateBy = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString())
                };


                var responseTask = httpClient.PostAsJsonAsync<object>("role/AddRole", cObj);
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
                            IsSuccess = false;
                            Warning = true;
                            Message = data.error;
                            //InsertAudit("Add new role", Message, "Failed");

                            return Message;
                        }
                        else if (value.Contains("warning"))
                        {
                            IsSuccess = false;
                            Warning = true;
                            Message = data.warning;
                            //InsertAudit("Add new role", Message, "Failed");

                            return Message;
                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            //InsertAudit("Add new role", Message, "Success");

                            return Message;
                        }
                    }

                }
                return "";
            }
        }

        public string UpdateRole(RolesModel aRoles)
        {
            using (var httpClient = new HttpClient())
            {

                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string rS = "";
                var obj = aRoles;
                if (obj.status == "false")

                    rS = "I";

                else
                    rS = "A";

                object cObj = new
                {
                    RoleId = obj.role_id,
                    RoleName = obj.role_name,
                    RoleDescription = obj.role_description,
                    Status = rS,
                    LastModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()),
                    LastModifiedDate = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")
                };


                var responseTask = httpClient.PutAsJsonAsync<object>("role/UpdateRole", cObj);
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
                            IsSuccess = false;
                            Warning = true;
                            Message = value;
                            //InsertAudit("Update role details", Message, "Failed");

                        }
                        else if (value.Contains("warning"))
                        {
                            IsSuccess = false;
                            Warning = true;
                            Message = value;
                            //InsertAudit("Update role details", Message, "Failed");

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = value;
                            roles = GetAllRoles();
                            //InsertAudit("Update role details", Message, "Success");
                            return Message;
                        }
                    }

                }
                return Message;
            }
        }
        public class UsersModel
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public string email_id { get; set; }
            public string phone_number { get; set; }
            public string is_locked { get; set; }
            public RolesModel role_id { get; set; }
        }
        public class RolesModel
        {
            public string role_id { get; set; }
            public string role_name { get; set; }
            public string role_description { get; set; }
            public string status { get; set; }
        }

        public string ReturnErrorMessage
        {
            get
            {
                return Message;
            }
        }
        public bool ReturnStatus
        {
            get
            {
                return IsSuccess;
            }
        }
    }
}