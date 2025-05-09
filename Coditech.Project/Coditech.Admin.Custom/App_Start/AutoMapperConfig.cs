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
            CreateMap<DBTMTestParameterModel, DBTMTestParameterViewModel>().ReverseMap();
            CreateMap<DBTMTestParameterListModel, DBTMTestParameterListViewModel>().ReverseMap();
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
            CreateMap<DBTMActivitiesDetailsModel, DBTMActivitiesDetailsViewModel>().ReverseMap();
            CreateMap<DBTMActivitiesDetailsListModel, DBTMActivitiesDetailsListViewModel>().ReverseMap();
            CreateMap<DBTMTestCalculationModel, DBTMTestCalculationViewModel>().ReverseMap();
            CreateMap<DBTMTestCalculationListModel, DBTMTestCalculationListViewModel>().ReverseMap();
            CreateMap<DBTMNewRegistrationViewModel, GeneralPersonModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentToUserListModel, DBTMTraineeAssignmentToUserListViewModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentToUserModel, DBTMTraineeAssignmentToUserViewModel>().ReverseMap();
            #endregion
        }
    }
}
