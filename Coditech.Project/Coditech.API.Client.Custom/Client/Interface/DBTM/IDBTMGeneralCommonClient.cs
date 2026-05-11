using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface IDBTMGeneralCommonClient : IBaseClient
    {
        DBTMDeviceDataDetailsResponse GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds);
    }
}
