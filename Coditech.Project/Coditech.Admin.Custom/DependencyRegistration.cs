﻿using Coditech.Admin.Agents;
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
            #endregion 
            #endregion Client
        }
    }
}
