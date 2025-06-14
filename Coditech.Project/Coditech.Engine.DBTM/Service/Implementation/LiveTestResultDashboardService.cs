using Coditech.Common.API.Model;
using Coditech.Common.Logger;
using Coditech.Common.Service;
namespace Coditech.API.Service
{
    public class LiveTestResultDashboardService : BaseService, ILiveTestResultDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        //private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;

        public LiveTestResultDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            //_adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Dashboard Details by selected Admin Role Master id.
        public virtual LiveTestResultDashboardModel GetLiveTestResultDashboard(string selectedCentreCode, long entityId)
        {
            //if (selectedAdminRoleMasterId <= 0)
            //    throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "SelectedAdminRoleMasterId"));

            //if (userMasterId <= 0)
            //    throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            //int? dashboardFormEnumId = _adminRoleMasterRepository.Table.Where(x => x.AdminRoleMasterId == selectedAdminRoleMasterId)?.Select(y => y.DashboardFormEnumId)?.FirstOrDefault();
            //DashboardModel dashboardModel = new DashboardModel();
            //if (dashboardFormEnumId > 0)
            //{
            //    string dashboardFormEnumCode = GetEnumCodeByEnumId((int)dashboardFormEnumId);
            //    dashboardModel.DashboardFormEnumCode = dashboardFormEnumCode;
            //}
            return null;
        }
    }
}

