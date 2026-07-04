using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Coditech.API.Common;

namespace Coditech.Engine.DBTM.Test.Tests
{
    public class RegisterStartupServicesTests
    {
        [Fact]
        public void RegisterCorsPolicy_Adds_CorsPolicy_To_Services()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ApplicationName = "TestApp" });
            // Ensure no exception when registering
            builder.RegisterCorsPolicy();

            var provider = builder.Services.BuildServiceProvider();
            var options = provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>>();
            Assert.NotNull(options);
            var policy = options.Value.GetPolicy(RegisterStatupServices.corsOrigin);
            Assert.NotNull(policy);
        }

        [Fact]
        public void ConfigureAutomapperAssemblies_Registers_CoditechTranslator()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ApplicationName = "TestApp" });

            builder.ConfigureAutomapperAssemblies();

            // We can't reference CoditechTranslator type directly (it's in a shared assembly), so inspect service descriptors
            var found = false;
            foreach (var sd in builder.Services)
            {
                if ((sd.ServiceType != null && sd.ServiceType.Name == "CoditechTranslator") ||
                    (sd.ImplementationType != null && sd.ImplementationType.Name == "CoditechTranslator") ||
                    (sd.ImplementationInstance != null && sd.ImplementationInstance.GetType().Name == "CoditechTranslator") )
                {
                    found = true;
                    break;
                }
            }

            Assert.True(found, "CoditechTranslator should be registered in the service collection");
        }
    }
}
