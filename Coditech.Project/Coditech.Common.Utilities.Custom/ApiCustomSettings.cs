using Coditech.Common.Helper;
using Microsoft.Extensions.Configuration;

namespace Coditech.Common.API
{
    public static class ApiCustomSettings
    {
        private static IConfigurationSection settings = CoditechDependencyResolver.GetService<IConfiguration>().GetSection("appsettings");

        public static void SetConfigurationSettingSource(IConfigurationSection settingSource)
        {
            settings = settingSource;
        }

        public static string DBTMDirectorMenuCode
        {
            get
            {
                return Convert.ToString(settings["DBTMDirectorMenuCode"]);

            }
        }
    }
}
