namespace Coditech.Admin
{
    /// <summary>
    /// Middleware to check and handle internet connectivity
    /// </summary>
    public class InternetConnectivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InternetConnectivityMiddleware> _logger;

        public InternetConnectivityMiddleware(RequestDelegate next, ILogger<InternetConnectivityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check internet connectivity for non-static resources
            if (!IsStaticResource(context.Request.Path))
            {
                var isInternetAvailable = await CheckInternetConnectivity();

                if (!isInternetAvailable)
                {
                    _logger.LogWarning("No internet connectivity detected");
                    context.Items["InternetAvailable"] = false;
                }
                else
                {
                    context.Items["InternetAvailable"] = true;
                }
            }

            await _next(context);
        }

        private bool IsStaticResource(PathString path)
        {
            var pathLower = path.Value?.ToLowerInvariant() ?? string.Empty;
            var staticExtensions = new[] { ".css", ".js", ".jpg", ".jpeg", ".png", ".gif", ".svg", ".woff", ".woff2", ".ttf", ".eot" };

            return staticExtensions.Any(ext => pathLower.EndsWith(ext));
        }

        private async Task<bool> CheckInternetConnectivity()
        {
            try
            {
                return await Coditech.Common.Utilities.Custom.InternetConnectivityHelper.IsInternetAvailableAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking internet connectivity: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// Extension method to add internet connectivity middleware
    /// </summary>
    public static class InternetConnectivityMiddlewareExtensions
    {
        public static IApplicationBuilder UseInternetConnectivityCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InternetConnectivityMiddleware>();
        }
    }
}
