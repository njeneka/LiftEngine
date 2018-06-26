using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiftEngine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var lift = System.Web.HttpContext.Current.Application["Lift"];
            return View(lift);
        }
    }
}