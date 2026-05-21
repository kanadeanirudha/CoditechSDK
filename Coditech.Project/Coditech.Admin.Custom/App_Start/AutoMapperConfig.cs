using AutoMapper;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Custom
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region DBTM
            CreateMap<DBTMDashboardModel, DBTMDashboardViewModel>().ReverseMap();
            CreateMap<DBTMDeviceModel, DBTMDeviceViewModel>().ReverseMap();
            CreateMap<DBTMDeviceListModel, DBTMDeviceListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeDetailsModel, DBTMTraineeDetailsViewModel>().ReverseMap();
            CreateMap<DBTMTraineeDetailsListModel, DBTMTraineeDetailsListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeDetailsCreateEditViewModel, GeneralPersonModel>().ReverseMap();
            CreateMap<DBTMActivityCategoryModel, DBTMActivityCategoryViewModel>().ReverseMap();
            CreateMap<DBTMActivityCategoryListModel, DBTMActivityCategoryListViewModel>().ReverseMap();
            CreateMap<DBTMTestModel, DBTMTestViewModel>().ReverseMap();
            CreateMap<DBTMTestListModel, DBTMTestListViewModel>().ReverseMap();
            CreateMap<DBTMDeviceRegistrationDetailsModel, DBTMDeviceRegistrationDetailsViewModel>().ReverseMap();
            CreateMap<DBTMDeviceRegistrationDetailsListModel, DBTMDeviceRegistrationDetailsListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentModel, DBTMTraineeAssignmentViewModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentListModel, DBTMTraineeAssignmentListViewModel>().ReverseMap();
            CreateMap<DBTMNewRegistrationModel, DBTMNewRegistrationViewModel>().ReverseMap();
            CreateMap<DBTMBatchActivityListModel, DBTMBatchActivityListViewModel>().ReverseMap();
            CreateMap<DBTMBatchActivityModel, DBTMBatchActivityViewModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanListModel, DBTMSubscriptionPlanListViewModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanModel, DBTMSubscriptionPlanViewModel>().ReverseMap();
            CreateMap<DBTMMySubscriptionPlanListModel, DBTMMySubscriptionPlanListViewModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanActivityListModel, DBTMSubscriptionPlanActivityListViewModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanActivityModel, DBTMSubscriptionPlanActivityViewModel>().ReverseMap();
            CreateMap<DBTMPrivacySettingModel, DBTMPrivacySettingViewModel>().ReverseMap();
            CreateMap<DBTMPrivacySettingListModel, DBTMPrivacySettingListViewModel>().ReverseMap();
            CreateMap<DBTMActivitiesModel, DBTMActivitiesViewModel>().ReverseMap();
            CreateMap<DBTMActivitiesListModel, DBTMActivitiesListViewModel>().ReverseMap();
            CreateMap<DBTMActivitiesDetailsListModel, DBTMActivitiesDetailsListViewModel>().ReverseMap();
            CreateMap<DBTMNewRegistrationViewModel, GeneralPersonModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentToUserListModel, DBTMTraineeAssignmentToUserListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentToUserModel, DBTMTraineeAssignmentToUserViewModel>().ReverseMap();
            CreateMap<DBTMReportsListModel, DBTMReportsListViewModel>().ReverseMap();
            CreateMap<LiveTestResultDashboardModel, LiveTestResultDashboardViewModel>().ReverseMap();
            CreateMap<LiveTestResultLoginModel, LiveTestResultLoginViewModel>().ReverseMap();
            CreateMap<DBTMGraphMasterModel, DBTMGraphMasterViewModel>().ReverseMap();
            CreateMap<DBTMGraphMasterListModel, DBTMGraphMasterListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeProfileListModel, DBTMTraineeProfileListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeProfileModel, DBTMTraineeProfileViewModel>().ReverseMap();
            CreateMap<DBTMActivityListViewSequenceListModel, DBTMActivityListViewSequenceListViewModel>().ReverseMap();
            CreateMap<DBTMActivityListViewSequenceModel, DBTMActivityListViewSequenceViewModel>().ReverseMap();
            CreateMap<DBTMNewRegistrationListModel, DBTMNewRegistrationListViewModel>().ReverseMap();
            CreateMap<DBTMCampMasterModel, DBTMCampMasterViewModel>().ReverseMap();
            CreateMap<DBTMCampMasterListModel, DBTMCampListViewModel>().ReverseMap();
            CreateMap<DBTMCampUserModel, DBTMCampUserViewModel>();
            CreateMap<DBTMCampUserViewModel, DBTMCampUserModel>();
            CreateMap<DBTMCentrewiseTestParameterListViewModel, DBTMCentrewiseTestParameterListViewViewModel>().ReverseMap();
            CreateMap<DBTMCentrewiseTestParameterListViewListModel, DBTMCentrewiseTestParameterListViewListViewModel>().ReverseMap();
            CreateMap<DBTMCentreWiseSettingModel, DBTMCentreWiseSettingViewModel>();
            CreateMap<DBTMCentreWiseSettingViewModel, DBTMCentreWiseSettingModel>();
            CreateMap<DBTMActivityVerticalViewSequenceListModel, DBTMActivityVerticalViewSequenceListViewModel>().ReverseMap();
            CreateMap<DBTMActivityVerticalViewSequenceModel, DBTMActivityVerticalViewSequenceViewModel>().ReverseMap();
            CreateMap<DBTMReportVerticalDataModel, DBTMReportVerticalDataViewModel>().ReverseMap();
            CreateMap<DBTMOrganisationCentrewiseJoiningCodeListModel, DBTMOrganisationCentrewiseJoiningCodeListViewModel>().ReverseMap();
            CreateMap<DBTMOrganisationCentrewiseJoiningCodeModel, DBTMOrganisationCentrewiseJoiningCodeViewModel>().ReverseMap();
            CreateMap<DBTMTraineeUploadModel, DBTMTraineeUploadResultViewModel>().ReverseMap();
            CreateMap<DBTMCentreWiseTestModel, DBTMCentreWiseTestViewModel>().ReverseMap();
            CreateMap<DBTMCentreWiseTestListModel, DBTMCentreWiseTestListViewModel>().ReverseMap();
            CreateMap<DBTMCampActivityListModel, DBTMCampActivityListViewModel>().ReverseMap();
            CreateMap<DBTMCampActivityModel, DBTMCampActivityViewModel>().ReverseMap();
            CreateMap<DBTMTestWisePerformanceStandardModel, DBTMTestWisePerformanceStandardViewModel>().ReverseMap();
            #endregion
        }
    }
}
