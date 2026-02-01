using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMServerController
    {
        public DBTMServerController()
        {
        }

        [AllowAnonymous]
        [Route("/dbtmserver/healthcheck")]
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return new OkResult();
        }

        [AllowAnonymous]
        [Route("/dbtmserver/servertime")]
        [HttpGet]
        public IActionResult ServerTime()
        {
            return new OkObjectResult(new
            {
                serverTime = DateTime.Now
            });
        }
    }
}