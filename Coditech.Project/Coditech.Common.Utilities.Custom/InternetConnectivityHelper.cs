using System.Net;

namespace Coditech.Common.Utilities.Custom
{
    /// <summary>
    /// Helper class to check internet connectivity
    /// </summary>
    public static class InternetConnectivityHelper
    {
        /// <summary>
        /// Check if internet is available
        /// </summary>
        /// <returns>True if internet is available, false otherwise</returns>
        public static bool IsInternetAvailable()
        {
            try
            {
                // Try to connect to a reliable external service
                using (var client = new WebClient())
                {
                    client.Proxy = null;
                    using (var response = client.OpenRead("https://www.google.com"))
                    {
                        if (response != null)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if internet is available using HttpClient (recommended for async scenarios)
        /// </summary>
        /// <returns>True if internet is available, false otherwise</returns>
        public static async Task<bool> IsInternetAvailableAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    using (var response = await client.GetAsync("https://www.google.com"))
                    {
                        return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.TemporaryRedirect;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if internet is available with a custom URL
        /// </summary>
        /// <param name="url">URL to check connectivity</param>
        /// <returns>True if internet is available, false otherwise</returns>
        public static async Task<bool> IsInternetAvailableAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return await IsInternetAvailableAsync();
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    using (var response = await client.GetAsync(url))
                    {
                        return response.IsSuccessStatusCode;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
