using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProjectControl.Models;
using ProjectControl.Models.Contexts;
using ProjectControl.Models.Rest;
using ProjectControl.Models.Rest.Responce;

namespace ProjectControl.Controllers
{
    public class LogSessionController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        //Log in
        public LoginResponce Post(LoginUser logUser)
        {
            LoginResponce responce = new LoginResponce();
            User user = db.Users.Where(x => x.Login == logUser.Login).FirstOrDefault();

            if (user == null)
            {
                responce.ResultCode = 404;
                responce.ErrorMessage = $"User with login '{logUser.Login}' not exists";
            }
            else if (!user.IsActivated)
            {
                responce.ResultCode = 400;
                responce.ErrorMessage = $"User with login '{logUser.Login}' not activated";
            }
            else if (logUser.Login == user.Login && Hash.ComputeSha256Hash(logUser.Password) == user.Password)
            {
                responce.ResultCode = 200;
                responce.User = logUser.Login;

                HttpContext.Current.Session.Add("LoggedAs", user.Login);
                Debug.WriteLine(logUser.RememberMe);
                if (logUser.RememberMe)
                {
                    HttpContext.Current.Response.SetCookie(new HttpCookie("LoggedAs") {
                        Value = user.Login,
                        Expires = DateTime.Now.AddYears(1)
                    });
                }
            }
            else
            {
                responce.ResultCode = 403;
                responce.ErrorMessage = "Wrong password";
            }

            return responce;
        }

        //Log out
        public void Delete()
        {
            HttpContext.Current.Session.Remove("LoggedAs");
            HttpContext.Current.Response.SetCookie(new HttpCookie("LoggedAs") {
                Value = "",
                Expires = DateTime.Now - new TimeSpan(1, 0, 0, 0)
            });
        }
    }
}