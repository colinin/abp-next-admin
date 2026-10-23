using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Email.Contracts;
using LINGYUN.Abp.ElsaNext.Email.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace LINGYUN.Abp.ElsaNext.Email.Features;

public class EmailFeature : FeatureBase
{
    public Action<IServiceProvider, HttpClient> ConfigureDownloaderHttpClient { get; set; } = (_, _) => { };

    public EmailFeature(IModule module) : base(module)
    {
    }

    public override void Configure()
    {
        Module.AddActivitiesFrom<EmailFeature>();
    }

    public override void Apply()
    {
        Services
            .AddHttpClient<IDownloader, DefaultDownloader>(ConfigureDownloaderHttpClient);
    }
}
