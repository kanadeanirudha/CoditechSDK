using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMUserService
    {
        DBTMUserModel Login(UserLoginModel model);
        DBTMUserModel UpdateAdditionalInformation(DBTMUserModel dbtmUserModel);
        DBTMUserModel GetDBTMTraineeDetails(long entityId);
    }
}
