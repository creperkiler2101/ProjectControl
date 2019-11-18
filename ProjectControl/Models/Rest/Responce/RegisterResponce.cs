using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class RegisterResponce
    {
        public string User { get; set; }
        public List<string> ErrorMessage { get; set; }
        public int ResultCode { get; set; }
    }
}