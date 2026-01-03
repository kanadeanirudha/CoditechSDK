using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMOrganisationCentreMasterController : BaseController
    {
        private readonly IDBTMOrganisationCentreMasterService _dBTMOrganisationCentreMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMOrganisationCentreMasterController(ICoditechLogging coditechLogging, IDBTMOrganisationCentreMasterService dBTMOrganisationCentreMasterService)
        {
            _dBTMOrganisationCentreMasterService = dBTMOrganisationCentreMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMOrganisationCentreMaster/GetActivityListViewSequenceList")]
        [Produces(typeof(DBTMActivityListViewSequenceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, string centreCode, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivityListViewSequenceListModel list = _dBTMOrganisationCentreMasterService.GetActivityListViewSequenceList(dBTMOrganisationCentreMasterId, centreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivityListViewSequenceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentre", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentre", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMOrganisationCentreMaster/GetDBTMCentrewiseTestParameterListView")]
        [HttpGet]
        [Produces(typeof(DBTMCentrewiseTestParameterListViewResponse))]
        public virtual IActionResult GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId)
        {
            try
            {
                DBTMCentrewiseTestParameterListViewModel dBTMOrganisationCentreModel = _dBTMOrganisationCentreMasterService.GetDBTMCentrewiseTestParameterListView(dBTMOrganisationCentreParameterListViewSequenceId);
                return IsNotNull(dBTMOrganisationCentreModel) ? CreateOKResponse(new DBTMCentrewiseTestParameterListViewResponse { DBTMCentrewiseTestParameterListViewModel = dBTMOrganisationCentreModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentre", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentrewiseTestParameterListViewResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentre", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentrewiseTestParameterListViewResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMOrganisationCentreMaster/UpdateDBTMCentrewiseTestParameterListView")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMCentrewiseTestParameterListViewResponse))]
        public virtual IActionResult UpdateDBTMCentrewiseTestParameterListView([FromBody] DBTMCentrewiseTestParameterListViewModel model)
        {
            try
            {
                DBTMCentrewiseTestParameterListViewModel dBTMActivityListViewSequence = _dBTMOrganisationCentreMasterService.UpdateDBTMCentrewiseTestParameterListView(model);
                return IsNotNull(dBTMActivityListViewSequence) ? CreateCreatedResponse(new DBTMCentrewiseTestParameterListViewResponse { DBTMCentrewiseTestParameterListViewModel = dBTMActivityListViewSequence }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentrewiseTestParameterListViewResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentrewiseTestParameterListViewResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}