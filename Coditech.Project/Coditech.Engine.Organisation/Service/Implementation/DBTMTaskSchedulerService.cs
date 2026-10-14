using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
namespace Coditech.API.Service
{
    public class DBTMTaskSchedulerService : TaskSchedulerService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMTaskSchedulerService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
        }

        #region Protected Method

        //Set Scheduler Parameters
        protected override void SetSchedulerParameters(TaskSchedulerModel erpTaskSchedulerModel, char separator = ' ')
        {
            switch (erpTaskSchedulerModel.SchedulerCallFor)
            {
                case APIConstant.DeleteLogMessage:
                    // ExeParameters format: baseUrl,authValue,userId,schedulerCallFor,timeout
                    erpTaskSchedulerModel.ExeParameters = $"{BaseParameter(separator)}{separator}{erpTaskSchedulerModel.SchedulerCallFor}{separator}{ApiSettings.ApiRequestTimeout}";
                    break;
            }
            _coditechLogging.LogMessage("ExeParameters value: ", CoditechLoggingEnum.Components.ERP.ToString(), TraceLevel.Verbose, erpTaskSchedulerModel?.ExeParameters);
        }

        private string BaseParameter(char separator = ' ')
        {
            string apiDomainUrl = $"{HttpContextHelper.Current.Request.Scheme}://{HttpContextHelper.Current.Request.Headers[HeaderNames.Host]}";
            string authValue = HttpContextHelper.Current.Request.Headers[APIConstant.Authorization].ToString()?.Replace("Basic ", "");
            long loginUserId = HelperMethods.GetLoginUserId();
            // Return base parameters: apiDomainUrl, authValue, loginUserId
            return $"{apiDomainUrl}{separator}{authValue}{separator}{loginUserId}";
        }
        #endregion
    }
}
