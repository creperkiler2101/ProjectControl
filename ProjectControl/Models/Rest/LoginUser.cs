using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest
{
    public class LoginUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}