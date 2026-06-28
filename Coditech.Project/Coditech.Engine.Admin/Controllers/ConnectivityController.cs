using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Coditech.Engine.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectivityController : ControllerBase
    {
        private readonly ILogger<ConnectivityController> _logger;

        public ConnectivityController(ILogger<ConnectivityController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check if internet is available
        /// </summary>
        /// <returns>Boolean indicating internet availability</returns>
        [HttpGet("check")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckConnectivity()
        {
            try
            {
                var isInternetAvailable = await Coditech.Common.Utilities.Custom.InternetConnectivityHelper.IsInternetAvailableAsync();

                return Ok(new
                {
                    success = true,
                    isInternetAvailable = isInternetAvailable,
                    timestamp = DateTime.UtcNow,
                    message = isInternetAvailable ? "Internet connection is available" : "No internet connection detected"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking internet connectivity: {ex.Message}");

                return Ok(new
                {
                    success = false,
                    isInternetAvailable = false,
                    timestamp = DateTime.UtcNow,
                    message = "Unable to determine internet connectivity",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Check connectivity with a specific URL
        /// </summary>
        /// <param name="url">URL to check connectivity</param>
        /// <returns>Boolean indicating connectivity to specified URL</returns>
        [HttpGet("check-url")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckConnectivityToUrl([FromQuery] string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "URL parameter is required"
                    });
                }

                var isConnected = await Coditech.Common.Utilities.Custom.InternetConnectivityHelper.IsInternetAvailableAsync(url);

                return Ok(new
                {
                    success = true,
                    isConnected = isConnected,
                    url = url,
                    timestamp = DateTime.UtcNow,
                    message = isConnected ? $"Connection to {url} is available" : $"Cannot connect to {url}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking connectivity to {url}: {ex.Message}");

                return Ok(new
                {
                    success = false,
                    isConnected = false,
                    url = url,
                    timestamp = DateTime.UtcNow,
                    message = "Unable to determine connectivity",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Simple ping endpoint to test basic connectivity
        /// </summary>
        /// <returns>Pong response</returns>
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new
            {
                success = true,
                message = "pong",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
