using Coditech.Admin.Agents;
using Coditech.API.Client;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Coditech.Admin.Custom
{
    public static class DependencyRegistration
    {
        public static void RegisterCustomDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CoditechTranslator>();
            #region Agent
            #region DBTM         
            builder.Services.AddScoped<IDBTMActivityCategoryAgent, DBTMActivityCategoryAgent>();
            builder.Services.AddScoped<IDBTMTestAgent, DBTMTestAgent>();
            builder.Services.AddScoped<IDBTMDeviceRegistrationDetailsAgent, DBTMDeviceRegistrationDetailsAgent>();
            builder.Services.AddScoped<IDBTMTraineeAssignmentAgent, DBTMTraineeAssignmentAgent>();
            builder.Services.AddScoped<IDBTMNewRegistrationAgent, DBTMNewRegistrationAgent>();
            builder.Services.AddScoped<IDBTMDeviceAgent, DBTMDeviceAgent>();
            builder.Services.AddScoped<IDBTMTraineeDetailsAgent, DBTMTraineeDetailsAgent>();
            builder.Services.AddScoped<IDBTMBatchActivityAgent, DBTMBatchActivityAgent>();
            builder.Services.AddScoped<IDBTMSubscriptionPlanAgent, DBTMSubscriptionPlanAgent>();
            builder.Services.AddScoped<IDBTMMySubscriptionPlanAgent, DBTMMySubscriptionPlanAgent>();
            builder.Services.AddScoped<IDBTMPrivacySettingAgent, DBTMPrivacySettingAgent>();
            builder.Services.AddScoped<IDBTMDashboardAgent, DBTMDashboardAgent>();
            builder.Services.AddScoped<IDBTMReportsAgent, DBTMReportsAgent>();
            builder.Services.AddScoped<ILiveTestResultDashboardAgent, LiveTestResultDashboardAgent>();
            builder.Services.AddScoped<IGeneralBatchAgent, DBTMBatchAgent>();
            builder.Services.AddScoped<IDBTMBatchAgent, DBTMBatchAgent>();
            builder.Services.AddScoped<IDBTMGraphAgent, DBTMGraphAgent>();
            builder.Services.AddScoped<IOrganisationCentrewiseJoiningCodeAgent, DBTMOrganisationCentrewiseJoiningCodeAgent>();
            builder.Services.AddScoped<IDBTMCampAgent, DBTMCampAgent>();
            builder.Services.AddScoped<IDBTMOrganisationCentreAgent, DBTMOrganisationCentreAgent>();
            builder.Services.AddScoped<IDBTMCentreWiseSettingAgent, DBTMCentreWiseSettingAgent>();
            builder.Services.AddScoped<IDBTMOrganisationCentrewiseJoiningCodeAgent, DBTMOrganisationCentrewiseJoiningCodeAgent>();
            builder.Services.AddScoped<IDBTMGeneralCommonAgent, DBTMGeneralCommonAgent>();
            builder.Services.AddScoped<IDBTMPrintQRAgent, DBTMPrintQRAgent>();
            builder.Services.AddScoped<IGeneralTrainerAgent, DBTMTrainerAgent>();
            #endregion
            #endregion Agent

            #region Client
            #region DBTM         
            builder.Services.AddScoped<IDBTMDeviceClient, DBTMDeviceClient>();
            builder.Services.AddScoped<IDBTMTraineeDetailsClient, DBTMTraineeDetailsClient>();
            builder.Services.AddScoped<IDBTMActivityCategoryClient, DBTMActivityCategoryClient>();
            builder.Services.AddScoped<IDBTMTestClient, DBTMTestClient>();
            builder.Services.AddScoped<IDBTMDeviceRegistrationDetailsClient, DBTMDeviceRegistrationDetailsClient>();
            builder.Services.AddScoped<IDBTMTraineeAssignmentClient, DBTMTraineeAssignmentClient>();
            builder.Services.AddScoped<IDBTMNewRegistrationClient, DBTMNewRegistrationClient>();
            builder.Services.AddScoped<IDBTMBatchActivityClient, DBTMBatchActivityClient>();
            builder.Services.AddScoped<IDBTMSubscriptionPlanClient, DBTMSubscriptionPlanClient>();
            builder.Services.AddScoped<IDBTMMySubscriptionPlanClient, DBTMMySubscriptionPlanClient>();
            builder.Services.AddScoped<IDBTMPrivacySettingClient, DBTMPrivacySettingClient>();
            builder.Services.AddScoped<IDBTMDashboardClient, DBTMDashboardClient>();
            builder.Services.AddScoped<IDBTMUserClient, DBTMUserClient>();
            builder.Services.AddScoped<IDBTMReportsClient, DBTMReportsClient>();
            builder.Services.AddScoped<ILiveTestResultDashboardClient, LiveTestResultDashboardClient>();
            builder.Services.AddScoped<IDBTMBatchClient, DBTMBatchClient>();
            builder.Services.AddScoped<IDBTMGraphClient, DBTMGraphClient>();
            builder.Services.AddScoped<IDBTMCampClient, DBTMCampClient>();
            builder.Services.AddScoped<IDBTMOrganisationCentreClient, DBTMOrganisationCentreClient>();
            builder.Services.AddScoped<IDBTMCentreWiseSettingClient, DBTMCentreWiseSettingClient>();
            builder.Services.AddScoped<IDBTMOrganisationCentrewiseJoiningCodeClient, DBTMOrganisationCentrewiseJoiningCodeClient>();
            builder.Services.AddScoped<IDBTMGeneralCommonClient, DBTMGeneralCommonClient>();
            builder.Services.AddScoped<IDBTMPrintQRClient, DBTMPrintQRClient>();
            #endregion 
            #endregion Client
        }
    }
}
