using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class ActivateResponce
    {
        public string User { get; set; }
        public string ErrorMessage { get; set; }
        public int ResultCode { get; set; }
    }
}