using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AARTWeb.Models
{

    public class RoleModel: BaseModel
    {
       // Auth Auth = new Auth();

        public int RoleId { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Required]
        [Display(Name = "Role Description")]
        public string RoleDescription { get; set; }
        public bool IsStatus { get; set; } = true;
        public string Status { get; set; } = "A";
        public string CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }

        public RoleModel Addrole(RoleModel objrmdl)
        {
            try
            {
                objrmdl.CreateDate = Convert.ToString(DateTime.Now);
                objrmdl.CreateBy = -1;
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(WebURL + "role/AddRole");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["token"].ToString());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!objrmdl.IsStatus)
                    {
                        objrmdl.Status = "I";
                    }                  

                    var responseTask = httpClient.PostAsJsonAsync<RoleModel>("", objrmdl);
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
                                objrmdl.Message = data.warning;
                                objrmdl.Warning = true;
                                InsertAudit("Add role", Message, "Failed");

                                return objrmdl;
                            }
                            else
                            {
                                objrmdl.Message = data.info;
                                objrmdl.IsSuccess = true;
                                InsertAudit("Add role", Message, "Success");

                                return objrmdl;
                            }
                        }
                    }
                }
                return objrmdl;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                InsertAudit("Add role", error, "Error");

                return objrmdl;
            }
        }



    }
}