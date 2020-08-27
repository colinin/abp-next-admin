using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using System;
using Volo.Abp;

namespace Hangfire
{
    public static class HangfireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireJwtToken(
            [NotNull]  this IApplicationBuilder app)
        {
            return app.UseMiddleware<HangfireJwtTokenMiddleware>();
        }

        public static IApplicationBuilder UseHangfireDashboard(
            [NotNull] this IApplicationBuilder app,
            [CanBeNull] Action<DashboardOptions> setup = null)
        {
            Check.NotNull(app, nameof(app));
            return app.UseHangfireDashboard("/hangfire", setup, null);
        }

        public static IApplicationBuilder UseHangfireDashboard(
            [NotNull] this IApplicationBuilder app,
            [CanBeNull] string pathMatch = "/hangfire",
            [CanBeNull] Action<DashboardOptions> setup = null)
        {
            Check.NotNull(app, nameof(app));
            return app.UseHangfireDashboard(pathMatch, setup, null);
        }

        public static IApplicationBuilder UseHangfireDashboard(
            [NotNull] this IApplicationBuilder app,
            [CanBeNull] string pathMatch = "/hangfire",
            [CanBeNull] Action<DashboardOptions> setup = null,
            [CanBeNull] JobStorage storage = null)
        {
            Check.NotNull(app, nameof(app));

            var options = new DashboardOptions();
            setup?.Invoke(options);

            return app.UseHangfireDashboard(pathMatch: pathMatch, options: options, storage: storage);
        }
    }
}
