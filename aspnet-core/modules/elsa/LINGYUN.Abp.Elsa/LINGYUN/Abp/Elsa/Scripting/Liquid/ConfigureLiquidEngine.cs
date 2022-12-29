using Elsa.Scripting.Liquid.Messages;
using Fluid;
using Fluid.Values;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Elsa.Scripting.Liquid;
public class ConfigureLiquidEngine : INotificationHandler<EvaluatingLiquidExpression>
{
    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpLocalizationOptions _localizationOptions;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ISettingProvider _settingProvider;
    private readonly IPermissionChecker _permissionChecker;

    public ConfigureLiquidEngine(
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

    public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
    {
        var context = notification.TemplateContext;
        var options = context.Options;

        options.Scope.SetValue("Abp", new ObjectValue(new LiquidAbpAccessor()));

        options.MemberAccessStrategy.Register<LiquidAbpAccessor, FluidValue>((_, name) =>
        {
            return name switch
            {
                nameof(IClock.Now) => new DateTimeValue(_clock.Now),
                _ => NilValue.Instance
            };
        });

        return Task.CompletedTask;
    }
}
