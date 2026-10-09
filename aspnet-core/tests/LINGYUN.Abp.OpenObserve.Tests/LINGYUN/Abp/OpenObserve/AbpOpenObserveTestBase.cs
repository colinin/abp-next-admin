using LINGYUN.Abp.OpenObserve.Serialization;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace LINGYUN.Abp.OpenObserve;

public abstract class AbpOpenObserveTestBase : AbpTestsBase<AbpOpenObserveTestModule>
{
    protected FakeHttpMessageHandler HttpMessageHandler { get; } = new FakeHttpMessageHandler();

    protected virtual OpenObserveClient CreateClient(Action<AbpOpenObserveOptions>? configure = null)
    {
        var options = new AbpOpenObserveOptions
        {
            Endpoint = "http://localhost:5080",
            Organization = "default",
            UserName = "admin@abp.io",
            Password = "test-password",
        };

        configure?.Invoke(options);

        var httpClient = new HttpClient(HttpMessageHandler)
        {
            BaseAddress = new Uri(options.Endpoint, UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds),
        };

        var authorization = options.CreateAuthorizationHeader();
        if (authorization != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = authorization;
        }

        return new OpenObserveClient(
            httpClient,
            ServiceProvider.GetRequiredService<IOpenObserveSerializer>(),
            Options.Create(options));
    }
}
