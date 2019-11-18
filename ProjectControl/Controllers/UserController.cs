using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using ProjectControl.Models;
using ProjectControl.Models.Contexts;
using ProjectControl.Models.Rest;
using ProjectControl.Models.Rest.Responce;

namespace ProjectControl.Controllers
{
    public class UserController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        //Get all users
        public string Get()
        {
            return "a";
        }

        //Get user with id
        public GetUserResponce Get(int id)
        {
            GetUserResponce responce = new GetUserResponce();
            GetUser user = new GetUser();

            User dbUser = db.Users.Where(x => x.Id == id).FirstOrDefault();
            if (dbUser != null)
            {
                user.Id = dbUser.Id;
                user.Login = dbUser.Login;
                user.Email = dbUser.Email;
                user.Name = dbUser.Name;
                user.SecondName = dbUser.SecondName;
                user.IsActivated = dbUser.IsActivated;

                responce.ResultCode = 200;
                responce.User = user;
            }
            else
                responce.ResultCode = 404;

            return responce;
        }

        //Update user with id
        public ActivateResponce Put(ActivateUser user)
        {
            ActivateResponce responce = new ActivateResponce();
            responce.ResultCode = 200;

            User u = db.Users.Where(x => x.ActivateUrl == user.Url && !x.IsActivated).FirstOrDefault();

            if (u == null)
            {
                responce.ResultCode = 404;
                responce.ErrorMessage = "No user with that activation url";
            }
            else
            {
                responce.ErrorMessage = "Account activated";
                u.IsActivated = true;
                db.SaveChanges();
            }

            return responce;
        }

        public UpdateUserResponce Put(int id, UpdateUser updUser)
        {
            UpdateUserResponce respone = new UpdateUserResponce();
            User user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            respone.ResultCode = 200;
            if (user != null)
            {
                if (updUser.Name.Trim().Length == 0)
                {
                    respone.ResultCode = 400;
                    respone.ErrorMessage.Add($"Name is required");
                }
                else if (updUser.Name.Length < 1 || user.Name.Length > 30)
                {
                    respone.ResultCode = 400;
                    respone.ErrorMessage.Add($"Name length need to be between 1 and 30");
                }

                if (updUser.SecondName.Trim().Length == 0)
                {
                    respone.ResultCode = 400;
                    respone.ErrorMessage.Add($"Second name is required");
                }
                else if (updUser.SecondName.Length < 1 || updUser.SecondName.Length > 30)
                {
                    respone.ResultCode = 400;
                    respone.ErrorMessage.Add($"Second name length need to be between 1 and 30");
                }

                if (respone.ResultCode == 200)
                {
                    user.Name = updUser.Name;
                    user.SecondName = updUser.SecondName;
                    db.SaveChanges();
                }
            }
            else
                respone.ResultCode = 404;

            return respone;
        }

        //Register new user
        public RegisterResponce Post(RegisterUser user)
        {
            RegisterResponce result = new RegisterResponce();
            result.ErrorMessage = new List<string>();
            result.ResultCode = 201;

            if (user.Login.Trim().Length == 0)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Login is required");
            }
            else if (user.Login.Length < 5 || user.Login.Length > 15)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Login length need to be between 5 and 15");
            }
            else if (db.Users.Where(x => x.Login == user.Login).FirstOrDefault() != null)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"User with login '{user.Login}' already exists");
            }

            if (user.Email.Trim().Length == 0)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Email is required");
            }
            else if (!Email.Validate(user.Email))
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Email format is wrong");
            }
            else if (db.Users.Where(x => x.Email == user.Email).FirstOrDefault() != null)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"User with email '{user.Email}' already exists");
            }

            if (user.Password.Trim().Length == 0)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Password is required");
            }
            else if (user.Password != user.RepeatPassword)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Passwords do not match");
            }

            if (user.Name.Trim().Length == 0)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Name is required");
            }
            else if (user.Name.Length < 1 || user.Name.Length > 30)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Name length need to be between 1 and 30");
            }

            if (user.SecondName.Trim().Length == 0)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Second name is required");
            }
            else if (user.SecondName.Length < 1 || user.SecondName.Length > 30)
            {
                result.ResultCode = 400;
                result.ErrorMessage.Add($"Second name length need to be between 1 and 30");
            }

            if (result.ResultCode == 201)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
                string url = new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());

                User u = new User()
                {
                    Login = user.Login,
                    Password = Hash.ComputeSha256Hash(user.Password),
                    Name = user.Name,
                    SecondName = user.SecondName,
                    Email = user.Email,
                    IsActivated = false,
                    ActivateUrl = url,
                    RoleId = 1,
                };
                db.Users.Add(u);
                db.SaveChanges();

                string msg = "<h1>Activate your account</h1>";
                string domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
                msg += $"<p>Click <a href='{domain}/Home/ActivateAccount?u={url}'>here</a> to activate your account</p>";
                Email.Send(user.Email, "Activate account", msg);

                result.ErrorMessage.Add(user.Email);
            }

            return result;
        }

        //Delete existing user with id
        public string Delete(int id)
        {
            return "abc";
        }
    }
}