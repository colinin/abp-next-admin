using Elsa.Studio.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LINGYUN.Abp.ElsaNext.Studio.Translations;

/// <summary>
/// Registers the Elsa Studio translations infrastructure into an ABP server-side host.
/// </summary>
public static class ElsaStudioTranslationsServiceCollectionExtensions
{
    /// <summary>
    /// Registers the circuit-culture holder and <see cref="ElsaStudioLocalizationProvider"/> - an
    /// Elsa <see cref="ILocalizationProvider"/> that translates via the ABP-registered
    /// <c>IStringLocalizer&lt;ElsaStudioTranslationsResource&gt;</c> - as the
    /// <see cref="ILocalizationProvider"/> (scoped, so the provider can see the per-circuit
    /// culture). The registration is additive (TryAdd), so a host that configures its own provider
    /// later keeps its own; call <see cref="AddElsaStudioTranslations"/> instead to force-replace.
    /// </summary>
    public static IServiceCollection AddElsaStudioTranslationsIfNotRegistered(this IServiceCollection services)
    {
        services.TryAddScoped<ILocalizationProvider, ElsaStudioLocalizationProvider>();

        return services;
    }

    /// <summary>
    /// Force-registers <see cref="ElsaStudioLocalizationProvider"/> as the single
    /// <see cref="ILocalizationProvider"/>, removing every other registration (including Elsa's
    /// default <c>DefaultLocalizationProvider</c>).
    /// </summary>
    public static IServiceCollection AddElsaStudioTranslations(this IServiceCollection services)
    {
        services.RemoveAll<ILocalizationProvider>();
        services.AddScoped<ILocalizationProvider, ElsaStudioLocalizationProvider>();

        return services;
    }
}
