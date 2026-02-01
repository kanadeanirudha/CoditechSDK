using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Engine.DBTM.Controllers
{
    [ApiController]
    [Route("dbtmserver")]
    public class DBTMServerController : ControllerBase
    {
        public DBTMServerController()
        {
        }

        [AllowAnonymous]
        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "Healthy"
            });
        }

        [AllowAnonymous]
        [HttpGet("servertime")]
        public IActionResult ServerTime()
        {
            return Ok(new
            {
                serverTime = DateTime.Now
            });
        }
    }
}
