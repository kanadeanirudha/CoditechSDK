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
        public static string DBTMTrainerMenuCode
        {
            get
            {
                return Convert.ToString(settings["DBTMTrainerMenuCode"]);

            }
        }
        public static short DirectorDepartmentId
        {
            get
            {
                return Convert.ToInt16(settings["DirectorDepartmentId"]);
            }
        }
        public static short TrainerDepartmentId
        {
            get
            {
                return Convert.ToInt16(settings["TrainerDepartmentId"]);
            }
        }
        public static short DirectorDesignationId
        {
            get
            {
                return Convert.ToInt16(settings["DirectorDesignationId"]);
            }
        }
        public static short TrainerDesignationId
        {
            get
            {
                return Convert.ToInt16(settings["TrainerDesignationId"]);
            }
        }
    }
}
