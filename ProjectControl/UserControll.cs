using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ProjectControl.Models;
using ProjectControl.Models.Contexts;

namespace ProjectControl
{
    public class UserControll
    {
        public static User LoggedAs {
            get {
                DatabaseContext db = new DatabaseContext();
                if (HttpContext.Current.Session["LoggedAs"] != null)
                {
                    string login = (string)HttpContext.Current.Session["LoggedAs"];

                    return db.Users.Where(x => x.Login == login).FirstOrDefault();
                }
                else if (HttpContext.Current.Request.Cookies["LoggedAs"] != null)
                {
                    string login = HttpContext.Current.Request.Cookies["LoggedAs"].Value;

                    return db.Users.Where(x => x.Login == login).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public static int AccessLevel {
            get {
                DatabaseContext db = new DatabaseContext();
                User user = LoggedAs;
                if (user != null)
                {
                    Role role = db.Roles.Where(x => x.Id == user.RoleId).FirstOrDefault();
                    return role.AccessLevel;
                }

                return 0;
            }
        }
    }
}