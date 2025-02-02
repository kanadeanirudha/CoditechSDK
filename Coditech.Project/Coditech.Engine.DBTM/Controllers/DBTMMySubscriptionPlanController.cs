using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Coditech.API.Controllers
{
    public class DBTMMySubscriptionPlanController : BaseController
    {
        private readonly IDBTMMySubscriptionPlanService _dBTMMySubscriptionPlanService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMMySubscriptionPlanController(ICoditechLogging coditechLogging, IDBTMMySubscriptionPlanService dBTMMySubscriptionPlanService)
        {
            _dBTMMySubscriptionPlanService = dBTMMySubscriptionPlanService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMMySubscriptionPlan/GetDBTMMySubscriptionPlanList")]
        [Produces(typeof(DBTMMySubscriptionPlanListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMMySubscriptionPlanList(long entityId, ExpandCollection expand, FilterCollection filter, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMMySubscriptionPlanListModel list = _dBTMMySubscriptionPlanService.GetDBTMMySubscriptionPlanList(entityId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMMySubscriptionPlanListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMySubscriptionPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMMySubscriptionPlanListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMySubscriptionPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMMySubscriptionPlanListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}