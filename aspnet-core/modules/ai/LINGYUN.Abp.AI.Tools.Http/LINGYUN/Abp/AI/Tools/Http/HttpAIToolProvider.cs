using LINGYUN.Abp.AI.Localization;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AI.Tools.Http;
public class HttpAIToolProvider : IAIToolProvider, ITransientDependency
{
    public const string ProviderName = "Http";
    public string Name => ProviderName;
    protected IServiceProvider ServiceProvider { get; }
    public HttpAIToolProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual AIToolPropertyDescriptor[] GetPropertites()
    {
        return [
            AIToolPropertyDescriptor.CreateStringProperty(
                HttpAIToolDefinitionExtenssions.Endpoint,
                LocalizableString.Create<AbpAIResource>("HttpAITool:Endpoint"),
                required: true),
            AIToolPropertyDescriptor.CreateSelectProperty(
                HttpAIToolDefinitionExtenssions.Method,
                LocalizableString.Create<AbpAIResource>("HttpAITool:Method"),
                [
                    new(HttpMethod.Get.Method, HttpMethod.Get.Method),
                    new(HttpMethod.Post.Method, HttpMethod.Post.Method),
                ]),
            AIToolPropertyDescriptor.CreateDictionaryProperty(
                HttpAIToolDefinitionExtenssions.Headers,
                LocalizableString.Create<AbpAIResource>("HttpAITool:Headers")),
            AIToolPropertyDescriptor.CreateBoolProperty(
                HttpAIToolDefinitionExtenssions.CurrentAccessToken,
                LocalizableString.Create<AbpAIResource>("HttpAITool:CurrentAccessToken"))];
    }

    public virtual Task<AITool[]> CreateToolsAsync(AIToolDefinition definition)
    {
        string? description = null;
        if (definition.Description != null)
        {
            var localizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
            description = definition.Description.Localize(localizerFactory)?.Value;
        }

        var httpAITool = AIFunctionFactory.Create(
            method: typeof(HttpAITool).GetMethod(nameof(HttpAITool.InvokeAsync))!,
            createInstanceFunc: (AIFunctionArguments args) =>
            {
                var context = new HttpAIToolInvokeContext(
                    args.Services ?? ServiceProvider,
                    definition);

                return new HttpAITool(context);
            },
            options: new AIFunctionFactoryOptions
            {
                Name = definition.Name,
                Description = description,
                AdditionalProperties = definition.Properties,
            });

        return Task.FromResult<AITool[]>([httpAITool]);
    }
}
