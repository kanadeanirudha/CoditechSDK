using Coditech.Common.API.Model;
namespace Coditech.API.Service
{
    public interface IDBTMCentreWiseSettingService
    {
        DBTMCentreWiseSettingModel GetDBTMCentreWiseSetting(int organisationCentreId);
        DBTMCentreWiseSettingModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel model);
    }
}
