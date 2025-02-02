using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public interface IDBTMPrivacySettingService
    {
        DBTMPrivacySettingListModel GetDBTMPrivacySettingList(string selectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMPrivacySettingModel CreateDBTMPrivacySetting(DBTMPrivacySettingModel model);
        DBTMPrivacySettingModel GetDBTMPrivacySetting(int dBTMPrivacySettingId);
        bool UpdateDBTMPrivacySetting(DBTMPrivacySettingModel model);
        bool DeleteDBTMPrivacySetting(ParameterModel parameterModel);
    }
}
