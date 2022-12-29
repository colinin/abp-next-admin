using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Elsa.Scripting.JavaScript;

public class ConfigureJavaScriptEngine : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
{
    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpLocalizationOptions _localizationOptions;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ISettingProvider _settingProvider;
    private readonly IPermissionChecker _permissionChecker;

    public ConfigureJavaScriptEngine(
        IClock clock,
        ICurrentTenant currentTenant,
        IOptions<AbpLocalizationOptions> localizationOptions,
        IStringLocalizerFactory localizerFactory,
        IGuidGenerator guidGenerator,
        ISettingProvider settingProvider,
        IPermissionChecker permissionChecker)
    {
        _clock = clock;
        _currentTenant = currentTenant;
        _localizationOptions = localizationOptions.Value;
        _localizerFactory = localizerFactory;
        _guidGenerator = guidGenerator;
        _settingProvider = settingProvider;
        _permissionChecker = permissionChecker;
    }

    public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
    {
        var output = notification.Output;

        output.AppendLine("declare interface Clock {");
        output.AppendLine("  now(): DateTime;");
        output.AppendLine("  normalize(dateTime: DateTime): DateTime;");
        output.AppendLine("}");

        output.AppendLine("declare interface CurrentTenant {");
        output.AppendLine("  id(): string;");
        output.AppendLine("  name(): string;");
        output.AppendLine("}");

        output.AppendLine("declare interface Utils {");
        output.AppendLine("  guid(): string;");
        output.AppendLine("}");

        output.AppendLine("declare interface Localization {");
        output.AppendLine("  localize(key: string, resourceName?: string): string;");
        output.AppendLine("}");

        output.AppendLine("declare interface Setting {");
        output.AppendLine("  get(name: string): any;");
        output.AppendLine("  getNumber(name: string): number;");
        output.AppendLine("  getBoolean(name: string): boolean;");
        output.AppendLine("}");

        output.AppendLine("declare interface Auth {");
        output.AppendLine("  isGranted(name: string): boolean;");
        output.AppendLine("  isAnyGranted(names: string[]): boolean;");
        output.AppendLine("  areAllGranted(names: string[]): boolean;");
        output.AppendLine("}");

        output.AppendLine("declare interface Abp {");
        output.AppendLine(" clock: Clock;");
        output.AppendLine(" currentTenant: CurrentTenant;");
        output.AppendLine(" utils: Utils;");
        output.AppendLine(" localization: Localization;");
        output.AppendLine(" setting: Setting;");
        output.AppendLine(" auth: Auth;");
        output.AppendLine("}");

        output.AppendLine("declare const abp: Abp;");

        return Task.CompletedTask;
    }

    public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
    {
        var engine = notification.Engine;

        var abpFunctions = new Dictionary<string, object>();

        var clockModel = new Dictionary<string, object?>
        {
            ["now"] = (Func<object?>)(() => _clock.Now),
            ["normalize"] = (Func<DateTime, DateTime>)((date) => _clock.Normalize(date))
        };
        abpFunctions["clock"] = clockModel;

        var currentTenantModel = new Dictionary<string, object?>
        {
            ["id"] = (Func<object?>)(() => _currentTenant.Id?.ToString()),
            ["name"] = (Func<object?>)(() => _currentTenant.Name)
        };
        abpFunctions["currentTenant"] = currentTenantModel;

        var utilsModel = new Dictionary<string, object?>
        {
            ["guid"] = (Func<object?>)(() => _guidGenerator.Create().ToString())
        };
        abpFunctions["utils"] = utilsModel;

        var localizationModel = new Dictionary<string, object?>
        {
            ["localize"] = ((Func<string, string?, string>)((key, resourceName) =>
            {
                resourceName ??= LocalizationResourceNameAttribute.GetName(typeof(DefaultResource));
                IStringLocalizer? localizer = null;

                foreach (var resource in _localizationOptions.Resources.Values)
                {
                    if (string.Equals(resourceName, resource.ResourceName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        localizer = _localizerFactory.Create(resource.ResourceType);
                        break;
                    }
                }

                if (localizer == null)
                {
                    return key;
                }

                return localizer[key];
            }))
        };
        abpFunctions["localization"] = localizationModel;

        var settingModel = new Dictionary<string, object?>
        {
            ["get"] = (Func<string, object?>)((name) => _settingProvider.GetOrNullAsync(name).GetAwaiter().GetResult()),
            ["getNumber"] = (Func<string, int>)((name) => _settingProvider.GetAsync(name, 0).GetAwaiter().GetResult()),
            ["getBoolean"] = (Func<string, bool>)((name) => _settingProvider.GetAsync(name, false).GetAwaiter().GetResult())
        };
        abpFunctions["setting"] = settingModel;

        var authModel = new Dictionary<string, object?>
        {
            ["isGranted"] = (Func<string, bool>)((name) => _permissionChecker.IsGrantedAsync(name).GetAwaiter().GetResult()),
            ["isAnyGranted"] = (Func<string[], bool>)((names) =>
            {
                var anyGrantResult = _permissionChecker.IsGrantedAsync(names).GetAwaiter().GetResult();

                return !anyGrantResult.AllProhibited;
            }),
            ["areAllGranted"] = (Func<string[], bool>)((names) =>
            {
                var anyGrantResult = _permissionChecker.IsGrantedAsync(names).GetAwaiter().GetResult();

                return anyGrantResult.AllGranted;
            })
        };
        abpFunctions["auth"] = authModel;

        engine.SetValue("abp", abpFunctions);

        return Task.CompletedTask;
    }
}
