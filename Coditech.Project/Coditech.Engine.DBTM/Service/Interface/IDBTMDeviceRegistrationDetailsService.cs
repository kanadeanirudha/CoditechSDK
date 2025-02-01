using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMDeviceRegistrationDetailsService
    {
        DBTMDeviceRegistrationDetailsListModel GetDBTMDeviceRegistrationDetailsList(long UserMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMDeviceRegistrationDetailsModel CreateRegistrationDetails(DBTMDeviceRegistrationDetailsModel model);
        DBTMDeviceRegistrationDetailsModel GetRegistrationDetails(long dBTMDeviceRegistrationDetailId);
        bool UpdateRegistrationDetails(DBTMDeviceRegistrationDetailsModel model);
        bool DeleteRegistrationDetails(ParameterModel parameterModel);
    }
}
