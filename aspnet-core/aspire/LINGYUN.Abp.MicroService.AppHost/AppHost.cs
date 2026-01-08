using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Redis
var redis = builder.AddRedis("redis")
    .WithContainerName("redis")
    .WithDataVolume("redis-dev");

// Elasticsearch
var elasticsearch = builder.AddElasticsearch("elasticsearch")
    .WithContainerName("elasticsearch")
    .WithDataVolume("elasticsearch-dev")
    .WithEnvironment("ES_JAVA_OPTS", "-Xms2g -Xmx2g");

// Postgres
var postgres = builder.AddPostgres("postgres")
    .WithPassword(builder.AddParameter("postgres-pwd", "123456", secret: true))
    .WithImage("postgres", "17-alpine")
    .WithContainerName("postgres")
    .WithDataVolume("postgres-dev");

var abpDb = postgres.AddDatabase("abp");

// RabbitMQ
var rabbitmq = builder.AddRabbitMQ("rabbitmq",
    userName: builder.AddParameter("rabbitmq-username", "admin", secret: true),
    password: builder.AddParameter("rabbitmq-password", "123456", secret: true))
    .WithContainerName("rabbitmq")
    .WithDataVolume("rabbitmq-dev")
    .WithManagementPlugin();

IResourceBuilder<ProjectResource> AddDotNetProject<TDbMigrator, TProject>(
    IDistributedApplicationBuilder builder, 
    string servicePrefix,
    int port,
    string portName,
    string serviceSuffix = "Service",
    string migratorSuffix = "Migrator",
    IResourceBuilder<ProjectResource>? waitProject = null) 
    where TDbMigrator : IProjectMetadata, new()
    where TProject : IProjectMetadata, new()
{
    IResourceBuilder<ProjectResource> service;
    if (builder.Environment.IsDevelopment())
    {
        var dbMigrator = builder
            .AddProject<TDbMigrator>($"{servicePrefix}{migratorSuffix}")
            .WithReference(abpDb, "Default")
            .WithReference(redis, "Redis")
            .WithReference(elasticsearch, "Elasticsearch")
            .WaitFor(elasticsearch)
            .WaitFor(redis)
            .WaitFor(abpDb)
            .WithReplicas(1);

        service = builder.AddProject<TProject>($"{servicePrefix}{serviceSuffix}")
            .WithHttpEndpoint(port: port, name: portName)
            .WithExternalHttpEndpoints()
            .WithReference(abpDb, "Default")
            .WithReference(redis, "Redis")
            .WithReference(rabbitmq, "Rabbitmq")
            .WithReference(elasticsearch, "Elasticsearch")
            .WaitFor(elasticsearch)
            .WaitFor(redis)
            .WaitFor(rabbitmq)
            .WaitFor(abpDb)
            .WaitForCompletion(dbMigrator);
    }
    else
    {
        service = builder.AddProject<TProject>($"{servicePrefix}{serviceSuffix}")
            .WithHttpEndpoint(port: port, name: portName)
            .WithExternalHttpEndpoints()
            .WithReference(abpDb, "Default")
            .WithReference(redis, "Redis")
            .WithReference(rabbitmq, "Rabbitmq")
            .WithReference(elasticsearch, "Elasticsearch")
            .WaitFor(elasticsearch)
            .WaitFor(redis)
            .WaitFor(rabbitmq)
            .WaitFor(abpDb);
    }

    if (waitProject != null)
    {
        service.WaitFor(waitProject);
    }

    return service;
}

// LocalizationService
var localizationService = AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_LocalizationService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_LocalizationService>(
    builder:            builder, 
    servicePrefix:      "Localization",
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30030,
    portName:           "localization")
    .WithHttpHealthCheck("/service-health");

// AuthServer
var authServer = AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_AuthServer_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_AuthServer>(
    builder:            builder,
    servicePrefix:      "Auth",
    serviceSuffix:      "Server",
    migratorSuffix:     "Migrator",
    port:               44385,
    portName:           "auth", 
    waitProject:        localizationService);

// AdminService
var adminService = AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_AdminService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_AdminService>(
    builder:            builder,
    servicePrefix:      "Admin",
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30010,
    portName:           "admin",
    waitProject:        authServer);

// IdentityService
AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_AuthServer_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_IdentityService>(
    builder:            builder,
    servicePrefix:      "Identity",
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30015,
    portName:           "identity",
    waitProject:        authServer);

// TaskService
var taskService = AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_TaskService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_TaskService>(
    builder:            builder, 
    servicePrefix:      "Task", 
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30040, 
    portName:           "task", 
    waitProject:        adminService)
    .WithHttpHealthCheck("/service-health");

// MessageService
AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_MessageService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_MessageService>(
    builder:            builder, 
    servicePrefix:      "Message",
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator", 
    port:               30020, 
    portName:           "message", 
    waitProject:        taskService);

// WebhookService
AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_WebhookService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_WebhookService>(
    builder:            builder, 
    servicePrefix:      "Webhook",
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30045,
    portName:           "webhook", 
    waitProject:        taskService);

// PlatformService
AddDotNetProject<
    Projects.LINGYUN_Abp_MicroService_PlatformService_DbMigrator,
    Projects.LINGYUN_Abp_MicroService_PlatformService>(
    builder:            builder, 
    servicePrefix:      "Platform", 
    serviceSuffix:      "Service",
    migratorSuffix:     "Migrator",
    port:               30025, 
    portName:           "platform",
    waitProject:        adminService);

// WeChatService
builder.AddProject<Projects.LINGYUN_Abp_MicroService_WeChatService>("WeChatService")
    .WithHttpEndpoint(port: 30060, name: "wechat")
    .WithExternalHttpEndpoints()
    .WithReference(abpDb, "Default")
    .WithReference(redis, "Redis")
    .WithReference(rabbitmq, "Rabbitmq")
    .WithReference(elasticsearch, "Elasticsearch")
    .WaitFor(elasticsearch)
    .WaitFor(redis)
    .WaitFor(abpDb)
    .WaitFor(rabbitmq)
    .WaitFor(localizationService);

// WorkflowService
builder.AddProject<Projects.LINGYUN_Abp_MicroService_WorkflowService>("WorkflowService")
    .WithHttpEndpoint(port: 30050, name: "workflow")
    .WithExternalHttpEndpoints()
    .WithReference(abpDb, "Default")
    .WithReference(redis, "Redis")
    .WithReference(rabbitmq, "Rabbitmq")
    .WithReference(elasticsearch, "Elasticsearch")
    .WaitFor(elasticsearch)
    .WaitFor(redis)
    .WaitFor(abpDb)
    .WaitFor(rabbitmq)
    .WaitFor(taskService);

// ApiGateway
var apigateway = builder.AddProject<Projects.LINGYUN_Abp_MicroService_ApiGateway>("ApiGateway")
    .WithHttpEndpoint(port: 30000, name: "gateway")
    .WithExternalHttpEndpoints()
    .WithReference(redis, "Redis")
    .WithReference(elasticsearch, "Elasticsearch")
    .WaitFor(elasticsearch)
    .WaitFor(redis);

// vben5
builder.AddViteApp("Frontend", "../../../apps/vben5", "dev:app")
    .WithHttpEndpoint(port: 5666, targetPort: 5666, name: "frontend", env: "VITE_PORT", isProxied: false)
    .WithExternalHttpEndpoints()
    .WithEnvironment("BROWSER", "none")
    .WithArgs("--port", "5666")
    .WithPnpm()
    .WithReference(apigateway)
    .WaitFor(apigateway);

builder.Build().Run();
