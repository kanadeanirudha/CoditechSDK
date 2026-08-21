using AutoMapper;
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<FilterTuple, FilterDataTuple>().ReverseMap();
            CreateMap<DBTMDeviceMaster, DBTMDeviceModel>().ReverseMap();
            CreateMap<DBTMTraineeDetails, DBTMTraineeDetailsModel>().ReverseMap();
            CreateMap<GeneralPerson, GeneralPersonModel>().ReverseMap();
            CreateMap<UserMaster, GeneralPersonModel>().ReverseMap();
            CreateMap<DBTMActivityCategory, DBTMActivityCategoryModel>().ReverseMap();
            CreateMap<DBTMTestMaster, DBTMTestModel>().ReverseMap();
            CreateMap<DBTMDeviceRegistrationDetails, DBTMDeviceRegistrationDetailsModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignment, DBTMTraineeAssignmentModel>().ReverseMap();
            CreateMap<DBTMBatchActivity, DBTMBatchActivityModel>().ReverseMap();
            CreateMap<AdminSanctionPostModel, AdminSanctionPost>().ReverseMap();
            CreateMap<DBTMSubscriptionPlan, DBTMSubscriptionPlanModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanAssociatedToUser, DBTMSubscriptionPlanModel>().ReverseMap();
            CreateMap<DBTMSubscriptionPlanActivity, DBTMSubscriptionPlanActivityModel>().ReverseMap();
            CreateMap<DBTMPrivacySetting, DBTMPrivacySettingModel>().ReverseMap();
            CreateMap<DBTMDeviceData, DBTMDeviceDataModel>().ReverseMap();
            CreateMap<DBTMDeviceDataDetails, DBTMDeviceDataModel>().ReverseMap();
            CreateMap<DBTMDeviceData, DBTMActivitiesModel>().ReverseMap();
            CreateMap<GeneralBatchMaster, DBTMBatchModel>().ReverseMap();
            CreateMap<DBTMTraineeAssignmentToUser, DBTMTraineeAssignmentToUserModel>().ReverseMap();
            CreateMap<DBTMTestMaster, DBTMTestApiModel>().ReverseMap();
            CreateMap<DBTMTraineeProfileModel, DBTMTraineeDetails>().ReverseMap();
            CreateMap<DBTMGraphMasterModel, DBTMGraphMaster>().ReverseMap();
            CreateMap<DBTMActivityListViewSequenceModel, DBTMTestParameterListViewSequence>().ReverseMap();
            CreateMap<DBTMCampMasterModel, DBTMCampMaster>().ReverseMap();
            CreateMap<DBTMCampUserModel, DBTMCampUser>().ReverseMap();
            CreateMap<DBTMCentrewiseTestParameterListViewModel, DBTMCentrewiseTestParameterListView>().ReverseMap();
            CreateMap<DBTMCentreWiseSetting, DBTMCentreWiseSettingModel>().ReverseMap();
            CreateMap<DBTMActivityVerticalViewSequenceModel, DBTMTestParameterVerticalViewSequence>().ReverseMap();
            CreateMap<DBTMReportsModel, DBTMDeviceDataDetails>().ReverseMap();
            CreateMap<DBTMTestParameterListViewSequence, DBTMTestParameterVerticalViewSequence>().ReverseMap();
            CreateMap<DBTMCentreWiseTestModel, DBTMCentreWiseTest > ().ReverseMap();
            CreateMap<DBTMCampActivity, DBTMCampActivityModel>().ReverseMap();
            CreateMap<DBTMTestWisePerformanceStandard, DBTMTestWisePerformanceStandardModel>().ReverseMap();
            CreateMap<OrganisationCentrewiseJoiningCode, OrganisationCentrewiseJoiningCodeModel>().ReverseMap();
            CreateMap<DBTMTestwisePerformanceStandardCategory, DBTMTestwisePerformanceStandardCategoryModel>().ReverseMap();
            CreateMap<DBTMGraphVerticalViewSequenceModel, DBTMGraphVerticalViewSequence>().ReverseMap();
        }
    }
}
