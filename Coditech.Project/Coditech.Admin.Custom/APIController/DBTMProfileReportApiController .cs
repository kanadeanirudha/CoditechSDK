using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Text.RegularExpressions;

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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DBTMProfileReportApiController(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider,
                                              IDBTMReportsAgent dBTMReportsAgent, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _dBTMReportsAgent = dBTMReportsAgent;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
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

        [AllowAnonymous]
        [HttpGet("GenerateAthletePdfRemark")]
        public async Task<IActionResult> GenerateAthletePdfRemark(long dBTMTraineeDetailId, string remarks)
        {
            (bool flowControl, IActionResult value) = IsAuthKeyValid();
            if (!flowControl)
            {
                return value;
            }

            DBTMReportTraineeProfileListModel list = _dBTMReportsAgent.GetBatchWiseTraineeProfileDetailsListV2(0, dBTMTraineeDetailId.ToString(), "firstname", DateTime.Now, DateTime.Now, "batch", "Trainee");

            DBTMReportTraineeProfileModel profile = list.DBTMTraineeProfileList?.FirstOrDefault();
            if (profile == null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "No trainee data found.",
                    FileName = "",
                    FilePath = ""
                });
            }

            profile.IsGeneratePDFReport = true;
            profile.Remarks = remarks;

            string datetimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (remarks != null)
                profile.Remarks = remarks;
            profile.IsGeneratePDFReport = true;
            string html = await RenderPartialViewToString("~/Views/DBTM/DBTMReports/_Profile.cshtml", profile);
            BrowserFetcher browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            await using IBrowser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using IPage page = await browser.NewPageAsync();
            await page.SetContentAsync(html);
            byte[] pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "10mm",
                    Right = "10mm",
                    Bottom = "10mm",
                    Left = "10mm"
                }
            });

            string firstName = Regex.Replace(profile.FirstName ?? "Trainee", "[^A-Za-z0-9_-]", "_");
            string lastName = Regex.Replace(profile.LastName ?? string.Empty, "[^A-Za-z0-9_-]", "_");
            string fileName = $"PerformanceReport_{firstName}{(string.IsNullOrEmpty(lastName) ? string.Empty : "_" + lastName)}_{datetimeStamp}.pdf";
            string reportsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PerformanceReports");
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);
            string pdfPath = Path.Combine(reportsFolder, fileName);
            await System.IO.File.WriteAllBytesAsync(pdfPath, pdf);

            Response.Cookies.Append("FileDownload", "Completed", new CookieOptions { Path = "/", SameSite = SameSiteMode.Lax });

            return Ok(new
            {
                Success = true,
                Message = "",
                FileName = fileName,
                FilePath = reportsFolder
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
