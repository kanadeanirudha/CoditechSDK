using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMCampMasterService
    {
        DBTMCampMasterListModel GetDBTMCampList(string selectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMCampMasterModel CreateDBTMCamp(DBTMCampMasterModel model);
        DBTMCampMasterModel GetDBTMCamp(long DBTMCampMasterId);
        bool UpdateDBTMCamp(DBTMCampMasterModel model);
        bool DeleteDBTMCamp(ParameterModel parameterModel);
        DBTMCampUserListModel GetDBTMCampUserList(long dBTMCampMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        bool AssociateUnAssociateCampwiseUser(DBTMCampUserModel model);
        DBTMCampUserListModel GetCampUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long dBTMCampMasterId);
    }
}
