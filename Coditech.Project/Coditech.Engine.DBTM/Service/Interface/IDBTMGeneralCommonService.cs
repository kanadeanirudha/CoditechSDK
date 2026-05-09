using Coditech.Common.API.Model;
namespace Coditech.API.Service
{
    public interface IDBTMGeneralCommonService
    {
        DBTMDeviceDataDetailsModel GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds);
    }
}