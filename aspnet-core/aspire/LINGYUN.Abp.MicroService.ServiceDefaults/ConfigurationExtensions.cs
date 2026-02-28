using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class ConfigurationExtensions
{
    public static TBuilder MapDefaultConfiguration<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddRedisClient("redis");
        builder.AddRabbitMQClient("rabbitmq");
        builder.AddElasticsearchClient("elasticsearch");

        // CAP PostgreSql
        builder.Configuration["CAP:PostgreSql:ConnectionString"] = builder.Configuration.GetConnectionString("Default");

        // CAP RabbitMQ
        builder.Configuration["CAP:RabbitMQ:HostName"] = builder.Configuration["RABBITMQ_HOST"];
        builder.Configuration["CAP:RabbitMQ:UserName"] = builder.Configuration["RABBITMQ_USERNAME"];
        builder.Configuration["CAP:RabbitMQ:Password"] = builder.Configuration["RABBITMQ_PASSWORD"];
        builder.Configuration["CAP:RabbitMQ:Port"] = builder.Configuration["RABBITMQ_PORT"];

        // Abp RabbitMQ
        builder.Configuration["RabbitMQ:Default:HostName"] = builder.Configuration["RABBITMQ_HOST"];
        builder.Configuration["RabbitMQ:Default:UserName"] = builder.Configuration["RABBITMQ_USERNAME"];
        builder.Configuration["RabbitMQ:Default:Password"] = builder.Configuration["RABBITMQ_PASSWORD"];
        builder.Configuration["RabbitMQ:Default:Port"] = builder.Configuration["RABBITMQ_PORT"];

        // Elsa RabbitMQ
        builder.Configuration["Elsa:Rebus:RabbitMQ:Connection"] = builder.Configuration.GetConnectionString("RabbitMQ");

        // Redis
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        builder.Configuration["Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=10";

        // DistributedLock
        builder.Configuration["DistributedLock:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=11";

        // Features
        builder.Configuration["Features:Validation:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=12";

        // SignalR
        builder.Configuration["SignalR:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=13,channelPrefix=abp-realtime-channel";

        // Elasticsearch
        var elasticSearchUrl = builder.Configuration.GetConnectionString("Elasticsearch");
        var serialogEsConfig = builder.Configuration.GetSection("Serilog:WriteTo");

        void ReplaceElasticsearchLogging(IConfiguration configuration)
        {
            foreach (var config in configuration.GetChildren())
            {
                if (string.Equals(config["Name"], "Async", StringComparison.InvariantCultureIgnoreCase))
                {
                    var configureArgs = config.GetSection("Args:configure");
                    ReplaceElasticsearchLogging(configureArgs);
                }
                if (string.Equals(config["Name"], "Elasticsearch", StringComparison.InvariantCultureIgnoreCase))
                {
                    config["Args:nodeUris"] = elasticSearchUrl;
                }
            }
        }

        if (serialogEsConfig.Exists())
        {
            ReplaceElasticsearchLogging(serialogEsConfig);
        }
        builder.Configuration["Elasticsearch:NodeUris"] = elasticSearchUrl;

        return builder;
    }
}
