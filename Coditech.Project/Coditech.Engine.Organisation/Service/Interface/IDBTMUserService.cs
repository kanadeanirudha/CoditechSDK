using Coditech.Common.API.Model;
using System.Data;

namespace Coditech.API.Service
{
    public interface IDBTMUserService
    {
        GeneralPersonModel DBTMRegisterTrainee(GeneralPersonModel generalPersonModel);
        DBTMTraineeUploadResultModel UploadTrainee(DBTMTraineeUploadRowListModel table);
    }
}
