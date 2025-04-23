using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dpk.DepositInterest.API.Configuration.Authorization;
using Dpk.DepositInterest.API.Configuration.ExecutionContext;
using Dpk.DepositInterest.API.Configuration.Extensions;
using Dpk.DepositInterest.API.Configuration.Validation;
using Dpk.DepositInterest.API.Modules.Administration;
using Dpk.DepositInterest.API.Modules.UserAccess;
using Dpk.DepositInterest.BuildingBlocks.Application;
using Dpk.DepositInterest.BuildingBlocks.Domain;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.Emails;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Identity;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace Dpk.DepositInterest.API
{
    public class Startup
    {
        private const string DepositInterestConnectionString = "DepositInterestConnectionString";
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables("Meetings_")
                .Build();

            _loggerForApi.Information("Connection string:" + _configuration.GetConnectionString(DepositInterestConnectionString));

            AuthorizationChecker.CheckAllEndpoints();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocumentation();

            services.ConfigureIdentityService();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes("Bearer");
                });
            });

            services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AdministrationAutofacModule());
            containerBuilder.RegisterModule(new UserAccessAutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var container = app.ApplicationServices.GetAutofacRoot();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            InitializeModules(container);

            app.UseMiddleware<CorrelationMiddleware>();

            app.UseSwaggerDocumentation();

            app.AddIdentityService();

            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "API");

            _loggerForApi.Information("Logger configured");
        }

        private void InitializeModules(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

            var emailsConfiguration = new EmailsConfiguration(_configuration["EmailsConfiguration:FromEmail"]);


            AdministrationStartup.Initialize(
                _configuration.GetConnectionString(DepositInterestConnectionString),
                executionContextAccessor,
                _logger,
                null);

            UserAccessStartup.Initialize(
                _configuration.GetConnectionString(DepositInterestConnectionString),
                executionContextAccessor,
                _logger,
                emailsConfiguration,
                _configuration["Security:TextEncryptionKey"],
                null,
                null);
        }
    }
}