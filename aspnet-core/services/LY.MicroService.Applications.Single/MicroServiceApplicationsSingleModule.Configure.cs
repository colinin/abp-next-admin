using VoloAbpExceptionHandlingOptions = Volo.Abp.AspNetCore.ExceptionHandling.AbpExceptionHandlingOptions;

namespace LY.MicroService.Applications.Single;

public partial class MicroServiceApplicationsSingleModule
{
    public static string ApplicationName { get; set; } = "MicroService-Applications-Single";
    private readonly static OneTimeRunner OneTimeRunner = new();

    private void PreConfigureFeature()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.Editions().EnableAll();
        });
    }

    private void PreConfigureApp(IConfiguration configuration)
    {
        AbpSerilogEnrichersConsts.ApplicationName = ApplicationName;

        PreConfigure<AbpSerilogEnrichersUniqueIdOptions>(options =>
        {
            // 以开放端口区别，应在0-31之间
            options.SnowflakeIdOptions.WorkerId = 1;
            options.SnowflakeIdOptions.WorkerIdBits = 5;
            options.SnowflakeIdOptions.DatacenterId = 1;
        });

        if (configuration.GetValue<bool>("App:ShowPii"))
        {
            IdentityModelEventSource.ShowPII = true;
        }
    }

    private void PreConfigureCAP(IConfiguration configuration)
    {
        PreConfigure<CapOptions>(options =>
        {
            options.UseDashboard();
            if (!configuration.GetValue<bool>("CAP:IsEnabled"))
            {
                options
                    .UseInMemoryStorage()
                    .UseRedis(configuration["CAP:Redis:Configuration"]);
                return;
            }
            options
                .UseMySql(mySqlOptions =>
                {
                    configuration.GetSection("CAP:MySql").Bind(mySqlOptions);
                })
                .UseRabbitMQ(rabbitMQOptions =>
                {
                    configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
                });
        });
    }

    private void PreConfigureAuthServer(IConfiguration configuration)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                //options.AddAudiences("lingyun-abp-application");

                options.UseLocalServer();

                options.UseAspNetCore();

                options.UseDataProtection();
            });
        });
    }

    private void PreConfigureIdentity()
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder.AddDefaultTokenProviders();
        });
    }

    private void PreConfigureCertificate(IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                builder.UseDataProtection();

                // 禁用https
                builder.UseAspNetCore()
                    .DisableTransportSecurityRequirement();

                builder.AddProductionEncryptionAndSigningCertificate(configuration["App:SslFile"], configuration["App:SslPassword"]);
            });
        }
        else
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                using (var algorithm = RSA.Create(keySizeInBits: 2048))
                {
                    var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
                    var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
                    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                    builder.AddSigningCertificate(certificate);
                }

                using (var algorithm = RSA.Create(keySizeInBits: 2048))
                {
                    var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
                    var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
                    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                    builder.AddEncryptionCertificate(certificate);
                }

                builder.UseDataProtection();

                // 禁用https
                builder.UseAspNetCore()
                    .DisableTransportSecurityRequirement();
            });
        }
    }

    private void PreConfigureQuartz(IConfiguration configuration)
    {
        PreConfigure<AbpQuartzOptions>(options =>
        {
            // 如果使用持久化存储, 则配置quartz持久层
            if (configuration.GetValue("Quartz:UsePersistentStore", false))
            {
                var settings = configuration.GetSection("Quartz:Properties").Get<Dictionary<string, string>>();
                if (settings != null)
                {
                    foreach (var setting in settings)
                    {
                        options.Properties[setting.Key] = setting.Value;
                    }
                }

                options.Configurator += (config) =>
                {
                    config.UsePersistentStore(store =>
                    {
                        store.UseProperties = false;
                        store.UseNewtonsoftJsonSerializer();
                    });
                };
            }
        });
    }

    private void PreConfigureElsa(IServiceCollection services, IConfiguration configuration)
    {
        var elsaSection = configuration.GetSection("Elsa");
        var startups = new[]
            {
                typeof(Elsa.Activities.Console.Startup),
                typeof(Elsa.Activities.Http.Startup),
                typeof(Elsa.Activities.UserTask.Startup),
                typeof(Elsa.Activities.Temporal.Quartz.Startup),
                typeof(Elsa.Activities.Email.Startup),
                typeof(Elsa.Scripting.JavaScript.Startup),
                typeof(Elsa.Activities.Webhooks.Startup),
            };

        PreConfigure<ElsaOptionsBuilder>(elsa =>
        {
            elsa
                .AddActivitiesFrom<MicroServiceApplicationsSingleModule>()
                .AddWorkflowsFrom<MicroServiceApplicationsSingleModule>()
                .AddFeatures(startups, configuration)
                .ConfigureWorkflowChannels(options => elsaSection.GetSection("WorkflowChannels").Bind(options));

            elsa.DistributedLockingOptionsBuilder
                .UseProviderFactory(sp => name =>
                {
                    var provider = sp.GetRequiredService<IDistributedLockProvider>();

                    return provider.CreateLock(name);
                });
        });

        services.AddNotificationHandlersFrom<MicroServiceApplicationsSingleModule>();

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(Elsa.Webhooks.Api.Endpoints.List).Assembly);
        });
    }

    private void ConfigureAuthServer(IConfiguration configuration)
    {
        Configure<OpenIddictServerAspNetCoreBuilder>(builder =>
        {
            builder.DisableTransportSecurityRequirement();
        });

        Configure<OpenIddictServerAspNetCoreOptions>(options =>
        {
            options.DisableTransportSecurityRequirement = true;
        });

        Configure<AbpOpenIddictAspNetCoreSessionOptions>(options =>
        {
            options.PersistentSessionGrantTypes.Add(SmsTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(PortalTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(LinkUserTokenExtensionGrantConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(WeChatTokenExtensionGrantConsts.OfficialGrantType);
            options.PersistentSessionGrantTypes.Add(WeChatTokenExtensionGrantConsts.MiniProgramGrantType);
            options.PersistentSessionGrantTypes.Add(AbpWeChatWorkGlobalConsts.GrantType);
            options.PersistentSessionGrantTypes.Add(QrCodeLoginProviderConsts.GrantType);
        });

        Configure<OpenIddictServerOptions>(options =>
        {
            var lifetime = configuration.GetSection("OpenIddict:Lifetime");
            options.AuthorizationCodeLifetime = lifetime.GetValue("AuthorizationCode", options.AuthorizationCodeLifetime);
            options.AccessTokenLifetime = lifetime.GetValue("AccessToken", options.AccessTokenLifetime);
            options.DeviceCodeLifetime = lifetime.GetValue("DeviceCode", options.DeviceCodeLifetime);
            options.IdentityTokenLifetime = lifetime.GetValue("IdentityToken", options.IdentityTokenLifetime);
            options.RefreshTokenLifetime = lifetime.GetValue("RefreshToken", options.RefreshTokenLifetime);
            options.RefreshTokenReuseLeeway = lifetime.GetValue("RefreshTokenReuseLeeway", options.RefreshTokenReuseLeeway);
            options.UserCodeLifetime = lifetime.GetValue("UserCode", options.UserCodeLifetime);
        });
    }

    private void ConfigureEndpoints(IServiceCollection services)
    {
        // 不需要
        //Configure<AbpEndpointRouterOptions>(options =>
        //{
        //    options.EndpointConfigureActions.Add(
        //        (context) =>
        //        {
        //            context.Endpoints.MapFallbackToPage("/_Host");
        //        });
        //});
        var preActions = services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>();

        services.AddAbpApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;

            //options.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Supports header too
            //options.ApiVersionReader = new MediaTypeApiVersionReader(); //Supports accept header too
        }, mvcOptions =>
        {
            mvcOptions.ConfigureAbp(preActions.Configure());
        });

        //services.AddApiVersioning(config =>
        //{
        //    // Specify the default API Version as 1.0
        //    config.DefaultApiVersion = new ApiVersion(1, 0);
        //    // Advertise the API versions supported for the particular endpoint (through 'api-supported-versions' response header which lists all available API versions for that endpoint)
        //    config.ReportApiVersions = true;
        //});

        //services.AddVersionedApiExplorer(options =>
        //{
        //    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        //    // note: the specified format code will format the version as "'v'major[.minor][-status]"
        //    options.GroupNameFormat = "'v'VVV";

        //    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        //    // can also be used to control the format of the API version in route templates
        //    options.SubstituteApiVersionInUrl = true;
        //});
        services.AddRazorPages();
        services.AddControllersWithViews().AddNewtonsoftJson();
    }

    private void ConfigureKestrelServer(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = null;
            options.Limits.MaxRequestBufferSize = null;
        });

        if (!environment.IsDevelopment())
        {
            Configure<KestrelServerOptions>(options =>
            {
                options.ConfigureHttpsDefaults(https =>
                {
                    https.ServerCertificate = X509CertificateLoader.LoadPkcs12FromFile(
                        configuration["App:SslFile"], 
                        configuration["App:SslPassword"]);
                });
            });
        }
    }

    private void ConfigureOssManagement(IServiceCollection services, IConfiguration configuration)
    {
        var useMinio = configuration.GetValue<bool>("OssManagement:UseMinio");
        if (useMinio)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseMinio(minio =>
                    {
                        configuration.GetSection("Minio").Bind(minio);
                    });
                });
            });
            services.AddMinioContainer();
        }
        else
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = Path.Combine(Directory.GetCurrentDirectory(), "blobs");
                    });
                });
            });
            services.AddFileSystemContainer();
        }
    }

    private void ConfigureBackgroundTasks()
    {
        Configure<AbpBackgroundTasksOptions>(options =>
        {
            options.NodeName = ApplicationName;
            options.JobCleanEnabled = true;
            options.JobFetchEnabled = true;
            options.JobCheckEnabled = true;
        });
    }

    private void ConfigureTextTemplating(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("TextTemplating:IsDynamicStoreEnabled"))
        {
            Configure<AbpTextTemplatingCachingOptions>(options =>
            {
                options.IsDynamicTemplateDefinitionStoreEnabled = true;
            });
        }
    }

    private void ConfigureFeatureManagement(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("FeatureManagement:IsDynamicStoreEnabled"))
        {
            Configure<FeatureManagementOptions>(options =>
            {
                options.IsDynamicFeatureStoreEnabled = true;
            });
        }
        Configure<FeatureManagementOptions>(options =>
        {
            options.ProviderPolicies[EditionFeatureValueProvider.ProviderName] = AbpSaasPermissions.Editions.ManageFeatures;
            options.ProviderPolicies[TenantFeatureValueProvider.ProviderName] = AbpSaasPermissions.Tenants.ManageFeatures;
        });
    }

    private void ConfigureSettingManagement(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("SettingManagement:IsDynamicStoreEnabled"))
        {
            Configure<SettingManagementOptions>(options =>
            {
                options.IsDynamicSettingStoreEnabled = true;
            });
        }
    }

    private void ConfigureWebhooksManagement(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("WebhooksManagement:IsDynamicStoreEnabled"))
        {
            Configure<WebhooksManagementOptions>(options =>
            {
                options.IsDynamicWebhookStoreEnabled = true;
            });
        }
    }
    /// <summary>
    /// 配置数据导出
    /// </summary>
    private void ConfigureExporter()
    {
        Configure<AbpExporterMiniExcelOptions>(options =>
        {
            options.MapExportSetting(typeof(BookDto), config =>
            {
                config.DynamicColumns = new[]
                {
                    // 忽略某些字段
                    new DynamicExcelColumn(nameof(BookDto.AuthorId)){ Ignore = true },
                    new DynamicExcelColumn(nameof(BookDto.LastModificationTime)){ Ignore = true },
                    new DynamicExcelColumn(nameof(BookDto.LastModifierId)){ Ignore = true },
                    new DynamicExcelColumn(nameof(BookDto.CreationTime)){ Ignore = true },
                    new DynamicExcelColumn(nameof(BookDto.CreatorId)){ Ignore = true },
                    new DynamicExcelColumn(nameof(BookDto.Id)){ Ignore = true },
                };
            });
        });
    }
    /// <summary>
    /// 配置数据权限
    /// </summary>
    private void ConfigureEntityDataProtected()
    {
        Configure<DataProtectionManagementOptions>(options =>
        {
            options.AddEntities(typeof(DemoResource),
                new[]
                {
                    typeof(Book),
                });
        });
    }

    private void ConfigurePermissionManagement(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("PermissionManagement:IsDynamicStoreEnabled"))
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.IsDynamicPermissionStoreEnabled = true;
            });
        }
        Configure<PermissionManagementOptions>(options =>
        {
            // Rename IdentityServer.Client.ManagePermissions
            // See https://github.com/abpframework/abp/blob/dev/modules/identityserver/src/Volo.Abp.PermissionManagement.Domain.IdentityServer/Volo/Abp/PermissionManagement/IdentityServer/AbpPermissionManagementDomainIdentityServerModule.cs
            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = AbpOpenIddictPermissions.Applications.ManagePermissions;

            //if (configuration.GetValue<bool>("AuthServer:UseOpenIddict"))
            //{
            //    options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = AbpOpenIddictPermissions.Applications.ManagePermissions;
            //}
            //else
            //{
            //    options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = AbpIdentityServerPermissions.Clients.ManagePermissions;
            //}
        });
    }

    private void ConfigureNotificationManagement(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("NotificationsManagement:IsDynamicStoreEnabled"))
        {
            Configure<AbpNotificationsManagementOptions>(options =>
            {
                options.IsDynamicNotificationsStoreEnabled = true;
            });
        }
    }

    private void ConfigureDistributedLock(IServiceCollection services, IConfiguration configuration)
    {
        var distributedLockEnabled = configuration["DistributedLock:IsEnabled"];
        if (distributedLockEnabled.IsNullOrEmpty() || bool.Parse(distributedLockEnabled))
        {
            var redis = ConnectionMultiplexer.Connect(configuration["DistributedLock:Redis:Configuration"]);
            services.AddSingleton<IDistributedLockProvider>(_ => new RedisDistributedSynchronizationProvider(redis.GetDatabase()));
        }
    }

    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MicroServiceApplicationsSingleModule>("LY.MicroService.Applications.Single");
        });
    }

    private void ConfigureIdempotent()
    {
        Configure<AbpIdempotentOptions>(options =>
        {
            options.IsEnabled = true;
            options.DefaultTimeout = 0;
        });
    }

    private void ConfigureDbContext(IConfiguration configuration)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            // 
        });
    }

    private void ConfigureExceptionHandling()
    {
        // 自定义需要处理的异常
        Configure<AbpExceptionHandlingOptions>(options =>
        {
            //  加入需要处理的异常类型
            options.Handlers.Add<Volo.Abp.Data.AbpDbConcurrencyException>();
            options.Handlers.Add<AbpInitializationException>();
            options.Handlers.Add<OutOfMemoryException>();
            options.Handlers.Add<System.Data.Common.DbException>();
            options.Handlers.Add<Microsoft.EntityFrameworkCore.DbUpdateException>();
            options.Handlers.Add<System.Data.DBConcurrencyException>();
        });
        // 自定义需要发送邮件通知的异常类型
        Configure<AbpEmailExceptionHandlingOptions>(options =>
        {
            // 是否发送堆栈信息
            options.SendStackTrace = true;
            // 未指定异常接收者的默认接收邮件
            // 指定自己的邮件地址
        });

        Configure<VoloAbpExceptionHandlingOptions>(options =>
        {
            options.SendStackTraceToClients = false;
            options.SendExceptionsDetailsToClients = false;
        });
    }

    private void ConfigureJsonSerializer(IConfiguration configuration)
    {
        // 统一时间日期格式
        Configure<AbpJsonOptions>(options =>
        {
            var jsonConfiguration = configuration.GetSection("Json");
            if (jsonConfiguration.Exists())
            {
                jsonConfiguration.Bind(options);
            }
        });
        // 中文序列化的编码问题
        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        });
    }

    private void ConfigureCaching(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            configuration.GetSection("DistributedCache").Bind(options);
        });

        Configure<RedisCacheOptions>(options =>
        {
            var redisConfig = ConfigurationOptions.Parse(options.Configuration);
            options.ConfigurationOptions = redisConfig;
            options.InstanceName = configuration["Redis:InstanceName"];
        });
    }

    private void ConfigureMultiTenancy(IConfiguration configuration)
    {
        // 多租户
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        var tenantResolveCfg = configuration.GetSection("App:Domains");
        if (tenantResolveCfg.Exists())
        {
            Configure<AbpTenantResolveOptions>(options =>
            {
                var domains = tenantResolveCfg.Get<string[]>();
                foreach (var domain in domains)
                {
                    options.AddDomainTenantResolver(domain);
                }
            });
        }
    }

    private void ConfigureAuditing(IConfiguration configuration)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.ApplicationName = ApplicationName;
            // 是否启用实体变更记录
            var allEntitiesSelectorIsEnabled = configuration["Auditing:AllEntitiesSelector"];
            if (allEntitiesSelectorIsEnabled.IsNullOrWhiteSpace() ||
                (bool.TryParse(allEntitiesSelectorIsEnabled, out var enabled) && enabled))
            {
                options.EntityHistorySelectors.AddAllEntities();
            }
        });
    }

    private void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        // Swagger
        services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                { configuration["AuthServer:Audience"], "Single APP"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Single APP API", Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "colin",
                        Email = "colin.in@foxmail.com",
                        Url = new Uri("https://github.com/colinin")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/colinin/abp-next-admin/blob/master/LICENSE")
                    }
                });
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
    }

    private void ConfigureIdentity(IConfiguration configuration)
    {
        // 增加配置文件定义,在新建租户时需要
        Configure<IdentityOptions>(options =>
        {
            var identityConfiguration = configuration.GetSection("Identity");
            if (identityConfiguration.Exists())
            {
                identityConfiguration.Bind(options);
            }
        });
        Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;

            options.DynamicClaims.AddIfNotContains(AbpClaimTypes.Picture);
        });
        Configure<IdentitySessionCleanupOptions>(options =>
        {
            options.IsCleanupEnabled = true;
        });
    }

    private void ConfigureMvcUiTheme()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            //options.StyleBundles.Configure(
            //    LeptonXLiteThemeBundles.Styles.Global,
            //    bundle =>
            //    {
            //        bundle.AddFiles("/global-styles.css");
            //    }
            //);
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

            options
                .AddLanguagesMapOrUpdate(
                    "vue-admin-element-ui",
                    new NameValue("zh-Hans", "zh"),
                    new NameValue("en", "en"));

            // vben admin 语言映射
            options
                .AddLanguagesMapOrUpdate(
                    "vben-admin-ui",
                    new NameValue("zh_CN", "zh-Hans"));

            options.Resources.Get<AbpSettingManagementResource>()
                .AddBaseTypes(
                typeof(IdentityResource),
                typeof(AliyunResource),
                typeof(TencentCloudResource),
                typeof(WeChatResource),
                typeof(PlatformResource),
                typeof(AbpOpenIddictResource));
        });

        Configure<AbpLocalizationCultureMapOptions>(options =>
        {
            var zhHansCultureMapInfo = new CultureMapInfo
            {
                TargetCulture = "zh-Hans",
                SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
            };

            options.CulturesMaps.Add(zhHansCultureMapInfo);
            options.UiCulturesMaps.Add(zhHansCultureMapInfo);
        });

        Configure<AbpLocalizationManagementOptions>(options =>
        {
            options.SaveStaticLocalizationsToDatabase = true;
        });
    }

    private void ConfigureWrapper()
    {
        Configure<AbpWrapperOptions>(options =>
        {
            options.IsEnabled = true;
            // options.IsWrapUnauthorizedEnabled = true;
            options.IgnoreNamespaces.Add("Elsa");
            // 微信消息不能包装
            options.IgnoreNamespaces.Add("LINGYUN.Abp.WeChat");
        });
    }

    private void PreConfigureWrapper()
    {
        //PreConfigure<AbpDaprClientProxyOptions>(options =>
        //{
        //    options.ProxyRequestActions.Add(
        //        (appid, httprequestmessage) =>
        //        {
        //            httprequestmessage.Headers.TryAddWithoutValidation(AbpHttpWrapConsts.AbpDontWrapResult, "true");
        //        });
        //});

        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientActions.Add(
                (_, _, client) =>
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(AbpHttpWrapConsts.AbpDontWrapResult, "true");
                });
        });
    }

    private void ConfigureAuditing()
    {
        Configure<AbpAuditingOptions>(options =>
        {
            // options.IsEnabledForGetRequests = true;
            options.ApplicationName = ApplicationName;
        });
    }

    private void ConfigureSingleModule(IServiceCollection services, bool isDevelopment)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(StandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(SingleGlobalScriptContributor));
                });
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // 允许第三方调用集成服务
            options.ExposeIntegrationServices = true;
        });

        Configure<AbpIdentitySessionAspNetCoreOptions>(options =>
        {
            // abp 9.0版本可存储登录IP地域, 开启IP解析
            options.IsParseIpLocation = true;
        });

        // 用于消息中心邮件集中发送
        services.Replace<Volo.Abp.Emailing.IEmailSender, PlatformEmailSender>(ServiceLifetime.Transient);
        services.AddKeyedTransient<Volo.Abp.Emailing.IEmailSender, MailKitSmtpEmailSender>("DefaultEmailSender");

        // 用于消息中心短信集中发送
        services.Replace<ISmsSender, PlatformSmsSender>(ServiceLifetime.Transient);
        services.AddKeyedSingleton<ISmsSender, AliyunSmsSender>("DefaultSmsSender");
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            var applicationConfiguration = configuration.GetSection("App:Urls:Applications");
            foreach (var appConfig in applicationConfiguration.GetChildren())
            {
                options.Applications[appConfig.Key].RootUrl = appConfig["RootUrl"];
                foreach (var urlsConfig in appConfig.GetSection("Urls").GetChildren())
                {
                    options.Applications[appConfig.Key].Urls[urlsConfig.Key] = urlsConfig.Value;
                }
            }
        });
    }

    private void ConfigureSecurity(IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });

        services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddAuthentication()
            .AddAbpJwtBearer(options =>
            {
                configuration.GetSection("AuthServer").Bind(options);

                var validIssuers = configuration.GetSection("AuthServer:ValidIssuers").Get<List<string>>();
                if (validIssuers?.Count > 0)
                {
                    options.TokenValidationParameters.ValidIssuers = validIssuers;
                    options.TokenValidationParameters.IssuerValidator = TokenWildcardIssuerValidator.IssuerValidator;
                }

                options.Events ??= new JwtBearerEvents();
                options.Events.OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/api/files")))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                };
            });

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = (ctx) =>
            {
                if (ctx.Request.Path.Value.StartsWith("/api") ||
                    ctx.Request.Path.Value.StartsWith("/connect/token"))
                {
                    ctx.Response.Clear();
                    ctx.Response.StatusCode = 401;
                    return Task.FromResult(0);
                }

                string authorization = ctx.Request.Headers.Authorization;
                if (!authorization.IsNullOrWhiteSpace() &&
                    authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                {
                    ctx.Response.Clear();
                    ctx.Response.ContentType = "application/json";
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;
            };
        });

        if (!isDevelopment)
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            services
                .AddDataProtection()
                .SetApplicationName("LINGYUN.Abp.Application")
                .PersistKeysToStackExchangeRedis(redis, "LINGYUN.Abp.Application:DataProtection:Protection-Keys");
        }

        services.AddSameSiteCookiePolicy();
    }

    private void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                var corsOrigins = configuration.GetSection("App:CorsOrigins").Get<List<string>>();
                if (corsOrigins == null || corsOrigins.Count == 0)
                {
                    corsOrigins = configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToList() ?? new List<string>();
                }
                builder
                    .WithOrigins(corsOrigins
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .WithAbpWrapExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureWeChat()
    {
        Configure<AbpWeChatMessageHandleOptions>(options =>
        {
            // 回复文本消息
            options.MapMessage<
                LINGYUN.Abp.WeChat.Official.Messages.Models.TextMessage,
                LY.MicroService.Applications.Single.WeChat.Official.Messages.TextMessageReplyContributor>();
            // 处理关注事件
            options.MapEvent<
                LINGYUN.Abp.WeChat.Official.Messages.Models.UserSubscribeEvent,
                LY.MicroService.Applications.Single.WeChat.Official.Messages.UserSubscribeEventContributor>();

            options.MapMessage<
                LINGYUN.Abp.WeChat.Work.Common.Messages.Models.TextMessage,
                LY.MicroService.Applications.Single.WeChat.Work.Messages.TextMessageReplyContributor>();
        });
    }
}

