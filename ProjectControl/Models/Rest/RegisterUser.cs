using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ProjectControl.Models.Rest
{
    public class RegisterUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
    }
}