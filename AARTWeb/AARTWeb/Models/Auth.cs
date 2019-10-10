using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace AARTWeb.Models
{
    public class Auth
    {
        
        public static string result = null;
        public static string username = null;
        public static string password = null;

        public void CallApi()
        {
            ////https://localhost:44312/ 
            //System.Uri myUri = new System.Uri("http://192.168.1.64:9810/api/auth/login");
            ////System.Uri myUri = new System.Uri("https://localhost:44312/api/auth/login");

            //HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            //webRequest.Method = "POST";
            //webRequest.ContentType = "application/json";


            //webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
            var url = ConfigurationManager.AppSettings["WEBAPIURL"];
            string URI = url + "auth/login";
            string requestBody = string.Format("{{\"UserName\":\"{0}\",\"Password\":\"{1}\"}}", username, password);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string HtmlResult = wc.UploadString(URI, requestBody);
                var jo = JObject.Parse(HtmlResult);
                var value = jo["tokenString"].ToString();
                result = value;
            }
        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);

            string requestBody = string.Format("{{\"UserName\":\"{0}\",\"Password\":\"{1}\"}}", username, password);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            webRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), webRequest);
        }

        void GetResponseStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
           
            {
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = httpWebStreamReader.ReadToEnd();
                    var jo = JObject.Parse(result);
                    var value = jo["tokenString"].ToString();
                    result = value;
                    Debug.WriteLine(result);
                }
            }
        }
    }
}
