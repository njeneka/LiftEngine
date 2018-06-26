using System.Web;
using System.Web.Http;
using LiftEngine.Domain.Entities;
using LiftEngine.Domain.Models;
using LiftEngine.Domain.Services;

namespace LiftEngine.Api
{
    [RoutePrefix("api/lift")]
    public class LiftController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Json(HttpContext.Current.Application["Lift"]);
        }

        [HttpPost]
        [Route("stops")]
        public IHttpActionResult AddStop(StopModel model)
        {
            var lift = HttpContext.Current.Application["Lift"];
            var liftService = new LiftService((Lift)lift);
            liftService.AddStop(model);

            HttpContext.Current.Application["Lift"] = liftService.Lift;
            return Ok();
        }

    }
}