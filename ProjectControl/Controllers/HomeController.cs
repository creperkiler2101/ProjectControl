using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectControl.Models;
using ProjectControl.Models.Contexts;

namespace ProjectControl.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //ViewBag.user = db.Users.Where(x => x.Login == "admin").FirstOrDefault();
            //Email.Send("creperkiler2101@mail.ru", "Test", "Test");
            return View();
        }

        public ActionResult ActivateAccount(string u)
        {
            ViewBag.url = u;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public new ActionResult Profile(int id = -1)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult EditUser(int id = -1)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
