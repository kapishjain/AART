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
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
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
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
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
        public String AddUser(UsersModel rUsers)
        {

            using (var httpClient = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
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
                            return Message;

                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                            return Message;

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            return Message;

                            // users = GetAllUsers();
                        }
                    }

                }
                return Message;

            }
        }
        public String UpdateUser(UsersModel uUsers)
        {
            using (var httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri("http://192.168.1.81:9810/api/user/");
                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
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
                    lLastModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()),
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
                        }
                        else if (value.Contains("warning"))
                        {
                            Warning = true;
                            IsSuccess = false;
                            Message = data.warning;
                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            System.Threading.Thread.Sleep(1000);
                            users = GetAllUsers();
                            return Message;
                        }
                    }

                }

            }
            return Message;
        }
        public List<RolesModel> AddRole(RolesModel aRoles)
        {
            using (var httpClient = new HttpClient())
            {

                var url = ConfigurationManager.AppSettings["WEBAPIURL"];

                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string rS = "";
                var obj = aRoles;
                if (obj.status == "true")
                {
                    rS = "A";
                }
                else
                {
                    rS = "I";
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
                        }
                        else if (value.Contains("warning"))
                        {
                            IsSuccess = false;
                            Warning = true;
                            Message = data.warning;

                        }
                        else
                        {
                            IsSuccess = true;
                            Message = data.info;
                            roles = GetAllRoles();
                        }
                    }

                }
                return roles;
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