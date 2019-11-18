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
            return View();
        }

        public ActionResult Projects()
        {
            return View();
        }

        public ActionResult Tasks()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}