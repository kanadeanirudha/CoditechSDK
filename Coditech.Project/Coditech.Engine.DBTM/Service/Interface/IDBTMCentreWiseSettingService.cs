using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMCentreWiseSettingService
    {
        DBTMCentreWiseSettingModel GetDBTMCentreWiseSetting(int organisationCentreId);
        DBTMCentreWiseSettingModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel model);
        bool AssociateUnAssociateCentreTest(DBTMCentreWiseTestModel dBTMCentreWiseTestModel);
        DBTMCentreWiseTestModel AssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds);
        DBTMCentreWiseTestModel UnAssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds);
    }
}
