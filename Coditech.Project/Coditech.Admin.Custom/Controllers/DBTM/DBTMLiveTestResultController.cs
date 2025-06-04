using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMLiveTestResultController : BaseController
    {
        private const string create = "~/Views/DBTM/DBTMLiveTestResult/LiveTestResult.cshtml";

        [HttpGet]
        public virtual ActionResult LiveTestResult()
        {
            return View(create);
        }
    }
}