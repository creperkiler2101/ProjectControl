using ProjectControl.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectControl.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            ViewBag.waiting = db.Tasks.Where(x => !x.IsAccepted).ToList().Count;
            return View();
        }

        public ActionResult Projects()
        {
            return View();
        }

        public ActionResult Tasks(bool showNew = false)
        {
            ViewBag.New = showNew;
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}