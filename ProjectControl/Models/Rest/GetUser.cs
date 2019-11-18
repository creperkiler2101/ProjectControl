using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest
{
    public class GetUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }
    }
}