using System.Web.Mvc;
using LiftEngine.Domain.Models;

namespace LiftEngine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // reset the lift
            System.Web.HttpContext.Current.Application["Lift"] = new Lift(11);

            return View();
        }
    }
}