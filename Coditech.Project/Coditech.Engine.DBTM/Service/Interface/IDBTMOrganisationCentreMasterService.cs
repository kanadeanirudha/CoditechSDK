using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public interface IDBTMOrganisationCentreMasterService
    {     
        DBTMActivityListViewSequenceListModel GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, string centreCode,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMCentrewiseTestParameterListViewModel GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId, string centreCode);
        DBTMCentrewiseTestParameterListViewModel UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewModel model);
    }
}
