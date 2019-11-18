using ProjectControl.Models;
using ProjectControl.Models.Contexts;
using ProjectControl.Models.Rest;
using ProjectControl.Models.Rest.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectControl.Controllers
{
    public class UserSearchController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private bool IsNotAccessable()
        {
            return UserControll.LoggedAs == null || !UserControll.LoggedAs.IsAdmin;
        }

        public GetUserResponce Get(string login, string email, string name, string sname, bool? activated, int roleId = -1)
        {
            GetUserResponce responce = new GetUserResponce();
            responce.ResultCode = 200;

            if (IsNotAccessable())
            {
                responce.ResultCode = 403;
                responce.ErrorMessage.Add("Not enough access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(login))
                    login = "";
                if (string.IsNullOrWhiteSpace(email))
                    email = "";
                if (string.IsNullOrWhiteSpace(name))
                    name = "";
                if (string.IsNullOrWhiteSpace(sname))
                    sname = "";

                List<User> users = new List<User>();
                if (roleId == -1 && activated == null)
                {
                    users = db.Users.Where(x => x.Login.Contains(login) && x.Email.Contains(email)
                                            && x.Name.Contains(name) && x.SecondName.Contains(sname)).ToList();
                }
                else if (roleId == -1 && activated != null)
                {
                    users = db.Users.Where(x => x.Login.Contains(login) && x.Email.Contains(email)
                                            && x.Name.Contains(name) && x.SecondName.Contains(sname)
                                            && x.IsActivated == activated).ToList();
                }
                else if (roleId != -1 && activated == null)
                {
                    users = db.Users.Where(x => x.Login.Contains(login) && x.Email.Contains(email)
                                            && x.Name.Contains(name) && x.SecondName.Contains(sname)
                                            && x.RoleId == roleId).ToList();
                }
                else if (roleId != -1 && activated != null)
                {
                    users = db.Users.Where(x => x.Login.Contains(login) && x.Email.Contains(email)
                                            && x.Name.Contains(name) && x.SecondName.Contains(sname)
                                            && x.IsActivated == activated && x.RoleId == roleId).ToList();
                }

                foreach (User user in users)
                {
                    GetUser _user = new GetUser();
                    _user.Email = user.Email;
                    _user.Id = user.Id;
                    _user.IsActivated = user.IsActivated;
                    _user.Login = user.Login;
                    _user.Name = user.Name;
                    _user.SecondName = user.SecondName;
                    _user.RoleId = user.RoleId;
                    if (user.IsAdmin)
                        _user.Role = "Admin";
                    else
                        _user.Role = "User";

                    responce.User.Add(_user);
                }
            }

            return responce;
        }
    }
}