using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class UpdateUserResponce
    {
        public List<string> ErrorMessage { get; set; } = new List<string>();
        public int ResultCode { get; set; }
    }
}