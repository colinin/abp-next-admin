using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Localization.BlazorServer.Extensions;
using Elsa.Studio.Localization.BlazorServer.Services;
using Elsa.Studio.Localization.Models;
using Elsa.Studio.Localization.Services;
using Elsa.Studio.Login.BlazorServer.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Models;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Translations;
using Elsa.Studio.Workflows.Designer.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Workflows;
using LINGYUN.Abp.ElsaNext.Server;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace LY.MicroService.WorkflowManagement.Next;

[DependsOn(typeof(AbpElsaNextServerModule))]
[DependsOn(typeof(AbpAspNetCoreSerilogModule))]
[DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
[DependsOn(typeof(AbpAspNetCoreAuthenticationJwtBearerModule))]
[DependsOn(typeof(AbpJsonModule))]
[DependsOn(typeof(AbpSwashbuckleModule))]
[DependsOn(typeof(AbpMailKitModule))]
[DependsOn(typeof(AbpAutofacModule))]
public class WorkflowManagementNextHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            // Configure Management layer to use EF Core.
            elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef => ef.UseSqlite()));

            // Configure Runtime layer to use EF Core.
            elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef => ef.UseSqlite()));

            // Default Identity features for authentication/authorization.
            elsa.UseIdentity(identity =>
            {
                identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
                identity.UseAdminUserProvider();
            });

            // Configure ASP.NET authentication/authorization.
            elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        // Register Razor services.
        context.Services.AddRazorPages();

        context.Services.AddCoreInternal();
        context.Services.AddSharedServices();
        context.Services.AddTranslations();

        context.Services.AddServerSideBlazor(options =>
        {
            // Register the root components.
            options.RootComponents.RegisterCustomElsaStudioElements();
        });
        // Register shell services and modules.
        var backendApiConfig = new BackendApiConfig
        {
            ConfigureBackendOptions = options => configuration.GetSection("Backend").Bind(options),
            ConfigureHttpClientBuilder = options => options.AuthenticationHandler = typeof(AuthenticatingApiHttpMessageHandler),
        };
        context.Services.AddShell(options => configuration.GetSection("Shell").Bind(options));
        context.Services.AddRemoteBackend(backendApiConfig);
        context.Services.AddLoginModule();
        context.Services.AddDashboardModule();
        context.Services.AddWorkflowsModule();
        context.Services.AddLocalizationModule(new LocalizationConfig
        {
            ConfigureLocalizationOptions = options => configuration.GetSection("Localization").Bind(options),
        });
        // Configure SignalR.
        context.Services.AddSignalR(options =>
        {
            // Set MaximumReceiveMessageSize to handle large workflows.
            options.MaximumReceiveMessageSize = 5 * 1024 * 1000; // 5MB
        });

        // Add Health Checks.
        context.Services.AddHealthChecks();


        // Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
        context.Services.AddCors(cors => cors
            .AddDefaultPolicy(policy => policy
                .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("x-elsa-workflow-instance-id")));

        // Swagger
        context.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Workflow API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new string[] { }
                        }
                });
                options.OperationFilter<TenantHeaderParamter>();
            });

        context.Services.AddTransient<AuthenticatingApiHttpMessageHandler>();
        context.Services.AddSingleton<IActivityFactory, AbpActivityFactory>();
        context.Services.AddScoped<ICultureService, AbpBlazorServerCultureService>();
    }
}
