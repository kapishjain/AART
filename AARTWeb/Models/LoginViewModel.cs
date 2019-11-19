using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace AARTWeb.Models
{
    public class LoginViewModel : BaseModel
    {
        [Required]
        [Display(Name = "Email or UserName")]
        public string Email { get; set; }
        public string Status { get; set; }

        public string Message { get; set; }
        public bool success { get; set; } = false;
        //public bool warning { get; set; } = false;
        Auth Auth = new Auth();

               

        [Required]
        public string UserName { get; set; }
        [Required]
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be 8 to 16 characters.")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public Int32 user_id { get; set; }
        public string name { get; set; }
        public Int32 role_id { get; set; }
        public string role_name { get; set; }

        private static int Minimum_Length = 8;
        private static int Upper_Case_length = 1;
        private static int Lower_Case_length = 1;
        private static int NonAlpha_length = 1;
      //  private static int Numeric_length = 1;
        private static int Special_Character = 1;

        public LoginViewModel validateuser(string username, string password)
        {
            LoginViewModel lgnmdl = new LoginViewModel();
            try
            {
                Auth Auth = new Auth();
                string sessionID = HttpContext.Current.Session.SessionID;
                if (Auth.result == null)
                {
                    Auth.username = username;
                    Auth.password = password;

                    Auth.CallApi();
                }
                //var httpClientHandler = new HttpClientHandler()
                //{
                //    Credentials = new NetworkCredential("Sysadmin", "SYSADMIN"),
                //};
                if (HttpContext.Current.Session["token"] != null && Auth.error == null)
                {
                    using (var client = new HttpClient())
                    {
                        //https://localhost:44312/
                        //string URI = url + "auth / login";
                        client.BaseAddress = new Uri(WebURL + "user/");
                        //  client.BaseAddress = new Uri("https://localhost:44312/api/user/");
                        // string requestBody = string.Format("{{\"UserName\":\"{0}\",\"Password\":\"{1}\"}}", "Sysadmin", "SYSADMIN");

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("UserName", username);
                        client.DefaultRequestHeaders.Add("UserPassword", password);

                        // HttpContent content1 = new StringContent("{ UserName: 'Sysadmin',	UserPassword: 'SYSADMIN'}", Encoding.UTF8, "application/json");
                        // client.DefaultRequestHeaders.TryAddWithoutValidation("content", requestBody.ToString());
                        var responseTask = client.GetAsync("ValidateUser");

                        responseTask.Wait();

                        //To store result of web api response.   
                        var result = responseTask.Result;
                        // string val1 = null;
                        //If success received   
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
                                    InsertAudit("Login", error, "Failed", username);

                                    return lgnmdl;
                                }
                                else
                                {
                                    user_id = data[0].user_Id;
                                    username = data[0].userName;
                                    name = data[0].name;
                                    UserName = data[0].name;
                                    role_id = data[0].role_Id;
                                    Status = data[0].status;
                                    role_name= data[0].role_name;
                                    HttpContext.Current.Session["UserName"] = data[0].userName;
                                    HttpContext.Current.Session["UserID"] = data[0].user_Id;
                                    HttpContext.Current.Session["role"] = data[0].role_name;
                                    HttpContext.Current.Session["Name"] = data[0].name;

                                    InsertAudit("Login", "Logged In successfully", "Success", username,"");

                                    //HttpContext.Current.Session["role_id"] = data[0].role_Id;
                                }

                            }

                        }
                        else
                        {

                            error = result.RequestMessage.ToString();
                            InsertAudit("Login", error, "Failed", username);

                            error = "Username or Password does't match";
                        }
                        return lgnmdl;
                    }

                }
                else
                {
                   // InsertAudit("Login", Auth.error, "Failed", username);

                    lgnmdl.error = Auth.error;
                }
                return lgnmdl;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                //if (error.Equals(""))
                InsertAudit("Login", error, "Error", username);

                return lgnmdl;
            }
        }


        public LoginViewModel ForgotPassword(LoginViewModel objlgnmdl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = httpClient.GetAsync("user/ForgotPassword?UserNameorEmail=" + objlgnmdl.Email);
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
                                objlgnmdl.success = true;
                                objlgnmdl.Message = data.info;
                               // InsertAudit("ForgotPassword", objlgnmdl.Message, "Success", objlgnmdl.Email);

                            }
                            else if (value.Contains("error"))
                            {
                                objlgnmdl.Warning = true;
                                objlgnmdl.Message = data.error;
                                //InsertAudit("ForgotPassword", objlgnmdl.Message, "Failed" ,objlgnmdl.Email);

                            }
                        }
                    }

                }
                return objlgnmdl;
            }
            catch (Exception ex)
            {
                error = ex.Message;
              //  InsertAudit("Forgot Password", error, "Error", objlgnmdl.Email);

                return objlgnmdl;
            }
        }

        public LoginViewModel ChangePassword(FormCollection form)
        {

            LoginViewModel objlgnmdl = new LoginViewModel();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL);

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string OldPassword = form["old-password"];
                    string NewPassword = form["new-password"];

                    object odata = new
                    {
                        userid = HttpContext.Current.Session["UserID"].ToString(),
                        oldpassword = OldPassword,
                        newpassword = NewPassword
                    };

                    var myContent = JsonConvert.SerializeObject(odata);

                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var responseTask = httpClient.PutAsync("user/ChangePassword", byteContent);

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
                                Message = data.error;
                                InsertAudit("Change Password", Message, "Failed");

                                return objlgnmdl;
                            }
                            else
                            {
                                IsSuccess = true;
                                Message = data.message;
                                InsertAudit("Change Password", Message, "Success");

                                return objlgnmdl;
                            }
                        }
                    }
                }
                return objlgnmdl;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                InsertAudit("Change Password", error, "Error");

                return objlgnmdl;
            }

        }

        public void Logout()
        {
            InsertAudit("LogOut", "Successfully Logout", "Success");

        }
        public static bool IsValid(string Password)
        {
            if (Password.Length < Minimum_Length)
                return false;
            if (UpperCaseCount(Password) < Upper_Case_length)
                return false;
            if (LowerCaseCount(Password) < Lower_Case_length)
                return false;
            if (NumericCount(Password) < 1)
                return false;
            if (NonAlphaCount(Password) < NonAlpha_length)
                return false;
            if (SpecialCharacterCount(Password) < Special_Character)
                return false;
            return true;
        }
        private static int UpperCaseCount(string Password)
        {
            return Regex.Matches(Password, "[A-Z]").Count;
        }
        private static int LowerCaseCount(string Password)
        {
            return Regex.Matches(Password, "[a-z]").Count;
        }
        private static int NumericCount(string Password)
        {
            return Regex.Matches(Password, "[0-9]").Count;
        }
        private static int SpecialCharacterCount(string password)
        {
            return Regex.Matches(password, "[~!@#$%^&*()_+{}:\"<>?]").Count;
        }
        private static int NonAlphaCount(string Password)
        {
            return Regex.Matches(Password, @"[^0-9a-zA-Z\._]").Count;
        }
    }
}