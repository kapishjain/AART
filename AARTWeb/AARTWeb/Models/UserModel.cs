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
using System.Web.Script.Serialization;

namespace AARTWeb.Models
{
    public class UserModel:BaseModel
    {
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Password must be between 8 to 16 characters", MinimumLength = 6)]
        public string UserPassword { get; set; }
        [Required]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        //[StringLength(10, ErrorMessage = "Can't be more than 10 characters")]
        public long Phone { get; set; }
        public bool IsStatus { get; set; } = true;
        public bool Locked { get; set; }
        public string Status { get; set; } = "A";
        public string Is_Locked { get; set; } = "N";


        public UserModel Adduser(UserModel objumdl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL + "user/InsertUser");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.result);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!objumdl.IsStatus)
                    {
                        objumdl.Status = "I";
                    }

                    if (objumdl.Locked)
                    {
                        objumdl.Is_Locked = "Y";
                    }
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(objumdl);
                    var responseTask = httpClient.PostAsJsonAsync<UserModel>("", objumdl);
                    responseTask.Wait();

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
                                objumdl.Message = data.warning;
                                objumdl.Warning = true;
                                return objumdl;
                            }
                            else
                            {
                                objumdl.Message = data.info;
                                objumdl.IsSuccess = true;
                                return objumdl;
                            }
                        }
                    }
                }
                return objumdl;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return objumdl;
            }
        }
    }
}