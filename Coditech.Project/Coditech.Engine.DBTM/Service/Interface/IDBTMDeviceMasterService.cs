using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMDeviceMasterService
    {
        DBTMDeviceListModel GetDBTMDeviceList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMDeviceModel CreateDBTMDevice(DBTMDeviceModel model);
        DBTMDeviceModel GetDBTMDevice(long dBTMDeviceId);
        bool UpdateDBTMDevice(DBTMDeviceModel model);
        bool DeleteDBTMDevice(ParameterModel parameterModel);
        bool IsValidDeviceSerialCode(string deviceSerialCode);
    }
}
