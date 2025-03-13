using Coditech.Admin.Agents;
using Coditech.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Coditech.Admin.Custom
{
    public static class DependencyRegistration
    {
        public static void RegisterCustomDI(this WebApplicationBuilder builder)
        {
            #region Agent
            #region BioradMedisy
            builder.Services.AddScoped<IBioradMedisyMediaManagerAgent, BioradMedisyMediaManagerAgent>();
            builder.Services.AddScoped<IMediaManagerFolderAgent, BioradMedisyMediaManagerAgent>();
            #endregion BioradMedisy
            #endregion Agent

            #region Client
            #region BioradMedisy
            builder.Services.AddScoped<IBioradMedisyMediaManagerClient, BioradMedisyMediaManagerClient>();
            #endregion BioradMedisy
            #endregion Client
        }
    }
}
