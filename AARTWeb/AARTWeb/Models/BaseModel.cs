using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AARTWeb.Models
{
    public class BaseModel
    {
        private static String _apiurl;
        public static String WebURL
        {
            get
            {
                return _apiurl;
            }
            set
            {
                _apiurl = ConfigurationManager.AppSettings["WEBAPIURL"];
            }

        }
        public string error { get; set; }
        public Boolean Warning { get; set; }

        public Boolean IsSuccess { get; set; }

        public string Message { get; set; }

      public  BaseModel()
        {
            WebURL= ConfigurationManager.AppSettings["WEBAPIURL"];
        }
    }
}