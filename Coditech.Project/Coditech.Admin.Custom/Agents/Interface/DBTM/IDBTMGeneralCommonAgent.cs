using Coditech.Common.API.Model;
namespace Coditech.Admin.Agents
{
    public interface IDBTMGeneralCommonAgent
    {
        DBTMDeviceDataDetailsModel GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds);
    }
}
