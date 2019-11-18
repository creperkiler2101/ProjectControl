using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class GetUserResponce
    {
        public int ResultCode { get; set; }
        public List<GetUser> User { get; set; } = new List<GetUser>();
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}