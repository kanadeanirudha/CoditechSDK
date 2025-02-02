using Coditech.Common.Helper;

using Microsoft.Extensions.Configuration;

namespace Coditech.Admin.Utilities
{
    public static class CoditechCustomAdminSettings
    {
        /// <summary>
        /// Gets the appsettings configuration section.
        /// </summary>
        /// <returns>The appsettings configuration section.</returns>
        private static IConfigurationSection settings = CoditechDependencyResolver.GetService<IConfiguration>().GetSection("appsettings");

        
        public static string CoditechDBTMApiRootUri
        {
            get
            {
#if DEBUG
                return Convert.ToString(settings["CoditechDBTMApiRootUri"]);
#else
   return Convert.ToString($"{settings["Scheme"]}{settings["ClientName"]}-{settings["EnvironmentName"]}-api-dbtm.{settings["ApiDomainName"]}");
#endif
            }
        }
    }
}
