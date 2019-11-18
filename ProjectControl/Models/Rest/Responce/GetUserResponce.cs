using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class GetUserResponce
    {
        public int ResultCode { get; set; }
        public GetUser User { get; set; }
    }
}