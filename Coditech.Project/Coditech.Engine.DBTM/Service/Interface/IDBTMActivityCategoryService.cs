using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMActivityCategoryService
    {
        DBTMActivityCategoryListModel GetDBTMActivityCategoryList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMActivityCategoryModel CreateDBTMActivityCategory(DBTMActivityCategoryModel model);
        DBTMActivityCategoryModel GetDBTMActivityCategory(short dBTMActivityCategoryId);
        bool UpdateDBTMActivityCategory(DBTMActivityCategoryModel model);
        bool DeleteDBTMActivityCategory(ParameterModel parameterModel);
    }
}
