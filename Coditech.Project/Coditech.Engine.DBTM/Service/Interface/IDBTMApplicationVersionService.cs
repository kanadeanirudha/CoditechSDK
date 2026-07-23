using Coditech.API.Model.Custom.DBTM.DBTMApplicationVersion;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMApplicationVersionService
    {
        DBTMApplicationVersionListModel GetDBTMApplicationVersionList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMApplicationVersionModel CreateDBTMApplicationVersion(DBTMApplicationVersionModel model);
        DBTMApplicationVersionModel GetDBTMApplicationVersion(long dBTMApplicationVersionId);
        bool UpdateDBTMApplicationVersion(DBTMApplicationVersionModel model);
        bool DeleteDBTMApplicationVersion(ParameterModel parameterModel);
    }
}
