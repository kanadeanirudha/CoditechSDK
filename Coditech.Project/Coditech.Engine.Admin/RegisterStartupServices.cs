using Coditech.Admin.Helpers;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Admin.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Coditech.Admin
{
    public static class RegisterStartupServices
    {
        /// <summary>
        /// Service level configuratin registered.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterCommonServices(this WebApplicationBuilder builder)
        {
            // Registred all media setting.
            builder.RegisterMedia();

            //This method configures the MVC services for the commonly used features for pages.This
            // combines the effects of <see cref="MvcCoreServiceCollectionExtensions.AddMvcCore(IServiceCollection)"/>
            builder.Services.AddRazorPages();

            //Register Dependecncy.
            builder.RegisterDI();
            builder.RegisterCustomDI();

            // Adds a default implementation for the Microsoft.AspNetCore.Http.IHttpContextAccessor
            // service. 
            builder.Services.AddHttpContextAccessor();

            //builder.Services.AddHostedService<ScheduledTaskMiddleware>();

            //Register custom filters.
            builder.RegisterFilters();
            // Adds distributed sqlsessioncache and memorycache.
            builder.AddSession();

            // Adds caching in the application.
            builder.RegisterCaching();

            // Extensions to scan for AutoMapper classes and register the configuration, mapping, and extensions with the service collection:
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Adds MVC services to the specified <see cref="IServiceCollection" />.
            builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

            // Configures Newtonsoft.Json specific features such as input and output formatters.
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
            // register services used for pages.
            builder.RegisterControllerAndViews();

            // Registers services required by authentication services. <paramref name="defaultScheme"/> specifies the name of the
            // scheme to use by default when a specific scheme isn't requested.
            builder.AddCookieBaseAuthentication();

            // Configure logging functinality using Log4Net.
            //builder.Logging.AddLog4Net("log4net.config");
        }

        /// <summary>
        /// Application level services registered.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="builder"></param>
        public static void RegisterApplicationServices(this WebApplication app, WebApplicationBuilder builder)
        {
            // Build Service Provider static instance.
            CoditechDependencyResolver._staticServiceProvider = builder.Services.BuildServiceProvider();

            //Assign value to automapper translator
            ConfigureAutomapperServices();

            // Adds a middleware type to the application's request pipeline.
            app.UseMiddleware<RequestMiddleware>();

            // Adds the static file configurations with custom path.
            app.UseStaticFiles(builder);

            // Adds the <see cref="SessionMiddleware"/> to automatically enable session state for the application.
            app.UseSession();

            // Adds a <see cref="EndpointRoutingMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>.
            app.UseRouting();

            // Adds the <see cref="AuthorizationMiddleware"/> to the specified <see cref="IApplicationBuilder"/>,
            // which enables authorization capabilities.
            app.UseAuthentication();

            // Adds the<see cref= "AuthorizationMiddleware" /> to the specified < see cref = "IApplicationBuilder" />, which enables authorization capabilities.
            app.UseAuthorization();
            // Adds a route to the Microsoft.AspNetCore.Routing.IRouteBuilder with the specified
            // name and template.
            app.RegisterRoute();

            // Adds endpoints for Razor Pages to the Microsoft.AspNetCore.Routing.IEndpointRouteBuilder.
            app.MapRazorPages();
        }

        #region Common extenssion methods

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for views or pages.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterFilters(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for pages.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterControllerAndViews(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
        }

        /// <summary>
        ///  Enables static file serving for the given request path
        /// </summary>
        /// <param name="app"></param>
        /// <param name="builder"></param>
        public static void UseStaticFiles(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseStaticFiles();
        }

        /// <summary>
        ///   Adds a route to the Microsoft.AspNetCore.Routing.IRouteBuilder with the specified
        ///   name and template.
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterRoute(this WebApplication app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}");
            });
        }

        /// <summary>
        /// Assigning the values to the TanslatorExtension (Automapper)
        /// </summary>
        public static void ConfigureAutomapperServices()
        {
            // Assigned Translator to TranslatorExtension.
            TranslatorExtension.TranslatorInstance = CoditechDependencyResolver._staticServiceProvider?.GetService<CoditechTranslator>();
        }

        /// <summary>
        /// Registring all media setting
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterMedia(this WebApplicationBuilder builder)
        {
            // Coditech entity registration
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 104857600;
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = 104857600;
                options.MultipartBodyLengthLimit = 104857600;
                options.MultipartHeadersLengthLimit = 104857600;
            });

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 104857600; ;
            });
        }
        #endregion
    }
}
