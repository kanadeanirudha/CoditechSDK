using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;

namespace Coditech.Admin.Custom.APIController
{
    [ApiController]
    [Route("api/dbtmprofilereport")]
    public class DBTMProfileReportApiController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IDBTMReportsAgent _dBTMReportsAgent;
        private readonly IConfiguration _configuration;

        public DBTMProfileReportApiController(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IDBTMReportsAgent dBTMReportsAgent, IConfiguration configuration)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _dBTMReportsAgent = dBTMReportsAgent;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("getprofiledetails")]
        public async Task<IActionResult> GetProfileDetails(string dbtmTraineeDetailId)
        {
            (bool flowControl, IActionResult value) = IsAuthKeyValid();
            if (!flowControl)
            {
                return value;
            }

            DBTMReportTraineeProfileListModel list = _dBTMReportsAgent.GetBatchWiseTraineeProfileDetailsListV2(0, dbtmTraineeDetailId, "firstname", DateTime.Now, DateTime.Now, "batch", "Trainee");

            DBTMReportTraineeProfileModel profile = list.DBTMTraineeProfileList?.FirstOrDefault();
            if (profile == null)
            {
                return Ok(new
                {
                    success = true,
                    html = "No trainee data found."
                });
            }

            profile.IsGeneratePDFReport = true;
            string html = await RenderPartialViewToString("~/Views/DBTM/DBTMReports/_Profile.cshtml", profile);

            return Ok(new
            {
                success = true,
                html
            });
        }

        private (bool flowControl, IActionResult value) IsAuthKeyValid()
        {
            if (_configuration["appsettings:EnvironmentName"] != "dev")
            {
                string headerValue = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(headerValue) || !headerValue.StartsWith("Basic "))
                {
                    return (flowControl: false, value: Unauthorized(new { success = false, message = "Missing or invalid Authorization header." }));
                }
                else
                {
                    string authValue = headerValue?.Replace("Basic ", "");
                    string apiDomainName = CoditechAdminSettings.ApiDomainName;
                    string apiDomainKey = _configuration["appsettings:ApiDomainKey"];
                    if (authValue != HelperUtility.EncodeBase64($"{apiDomainName}|{apiDomainKey}"))
                    {
                        return (flowControl: false, value: Unauthorized(new { success = false, message = "Invalid Authorization header." }));
                    }
                }
            }
            return (flowControl: true, value: null);
        }

        private async Task<string> RenderPartialViewToString<TModel>(string viewName, TModel model)
        {
            ViewEngineResult viewResult = _viewEngine.GetView(null, viewName, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"The partial view '{viewName}' could not be found.");
            }

            await using StringWriter writer = new StringWriter();
            ViewDataDictionary<TModel> viewData = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model
            };

            TempDataDictionary tempData = new TempDataDictionary(HttpContext, _tempDataProvider);
            ViewContext viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                viewData,
                tempData,
                writer,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}
