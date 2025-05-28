using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMDeviceDataService
    {
        bool InsertDeviceData(List<DBTMDeviceDataModel> model);
    }
}
