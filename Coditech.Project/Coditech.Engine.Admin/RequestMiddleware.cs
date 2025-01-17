using Coditech.Admin.Utilities;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using System.Diagnostics;

namespace Coditech.Admin
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICoditechLogging _CoditechLogging;
        /// <summary>
        /// Custom middleware to handle the operation before request and after the response.
        /// </summary>
        /// <param name="next"></param>
        public RequestMiddleware(RequestDelegate next)
        {
            _CoditechLogging = CoditechDependencyResolver._staticServiceProvider?.GetService<ICoditechLogging>();
            _next = next;
        }
        /// <summary>
        /// Configured middleware to handle or perform the operation before request execute or response return.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            // Do tasks before other middleware here, aka 'BeginRequest'
            try
            {
                // Let the middleware pipeline run
                //CookieHelper.SetCookie("culture", CultureInfo.CurrentCulture.TwoLetterISOLanguageName, isCookieHttpOnly: false);
                bool isMaintenance = CoditechAdminSettings.MaintenanceMode;
                if (isMaintenance)
                {
                    context.Response.Redirect("maintenance.html");
                    await _next(context);
                }

                HelperUtility.ReplaceProxyToClientIp();
                SetGlobalLoggingSetting();
                SetDefaultGlobalTimeZone();
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

            //Disposing the context

        }
        /// <summary>
        /// Set logging settings in the cache.
        /// </summary>
        private void SetGlobalLoggingSetting()
        {
            //ICacheManager _cacheManager = CoditechDependencyResolver.GetService<ICacheManager>();

            ////Store Global Default Logging Setting in cache
            //CoditechDependencyResolver.GetService<ILogMessageAgent>()?.SetGlobalLoggingSetting();
        }

        //Set Global time zone data to Global TimeZone Helper.
        private void SetDefaultGlobalTimeZone()
        {
            //CoditechDependencyResolver.GetService<IServiceProvider>()?.GetTimeZoneService<IDefaultGlobalTimeZoneHelper>();
        }
        /// <summary>
        /// Logging error if request is break.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            try
            {
                var exception = ex.GetBaseException();

                if (exception.GetType() == typeof(CoditechException))
                {
                    context.Response.Clear();
                    switch ((exception as CoditechException).ErrorCode)
                    {
                        case ErrorCodes.InvalidSqlConfiguration:
                        case ErrorCodes.InvalidCoditechLicense:
                        case ErrorCodes.InvalidDomainConfiguration:
                            context.Response.ContentType = "text/html;charset=UTF-8";
                            await context.Response.WriteAsync($"<p style=\"text-align:center;font-size:30;\">{exception.Message}</p>");
                            break;
                        case ErrorCodes.NotPermitted:
                            context.Response.ContentType = "text/html;charset=UTF-8";
                            await context.Response.WriteAsync($"<p style=\"text-align:center;font-size:30;\">{exception.Message}</p>");
                            break;
                        default:
                            context.Response.ContentType = "text/html;charset=UTF-8";
                            await context.Response.WriteAsync($"<p style=\"text-align:center;font-size:30;\">A generic error occurred.</p>");
                            break;
                    }
                }
                else
                {
                    RedirectToErrorPage(context, exception);
                }
                _CoditechLogging.LogMessage(exception, "AdminApplicationError", TraceLevel.Error);
                //await context.Response.WriteAsync(exception.ToString());
            }
            catch (Exception exeption)
            {
                _CoditechLogging.LogMessage(exeption, "AdminApplicationError", TraceLevel.Error);
                RedirectToErrorPage(context, exeption);
            }

        }

        /// <summary>
        /// Displaying error page.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        private void RedirectToErrorPage(HttpContext context, Exception exception)
        {
            var routeData = new RouteData();
            context.Response.Clear();
            SetErrorPage(exception, routeData);
        }
        /// <summary>
        /// Assigning the error pages to display the error Msg.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="routeData"></param>
        private void SetErrorPage(Exception exception, RouteData routeData)
        {
            CoditechException CoditechException = exception as CoditechException;
            routeData.Values.Add("controller", "dashboard");

            switch (CoditechException?.ErrorCode)
            {
                case ErrorCodes.WebAPIKeyNotFound:
                    routeData.Values.Add("action", "TokenError");
                    break;
                case ErrorCodes.InvalidDomainConfiguration:
                case ErrorCodes.InvalidSqlConfiguration:
                case ErrorCodes.InvalidCoditechLicense:
                    routeData.Values.Add("action", "ConfigurationError");
                    break;
                default:
                    routeData.Values.Add("action", "ConfigurationError");
                    break;
            }

            routeData.Values.Add("exception", exception);
        }
    }
}
