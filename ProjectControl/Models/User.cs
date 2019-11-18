using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectControl.Models
{
    [Table("tblUsers")]
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string SecondName { get; set; }

        public string Avatar { get; set; }
        public string ActivateUrl { get; set; }

        public bool IsActivated { get; set; }

        public int RoleId { get; set; }

        public string FullName {
            get {
                return Name + " " + SecondName;
            }
        }

        public bool IsAdmin {
            get {
                return RoleId == 2;
            }
        }
    }
}