using Hangfire.Dashboard;
using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace Hangfire
{
    public static class DashboardOptionsExtensions
    {
        public static DashboardOptions AddAuthorization(
            [NotNull] this DashboardOptions options,
            [NotNull] IDashboardAuthorizationFilter authorizationFilter)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(authorizationFilter, nameof(authorizationFilter));

            List<IDashboardAuthorizationFilter> filters = new List<IDashboardAuthorizationFilter>();
            filters.AddRange(options.Authorization);
            filters.AddIfNotContains(authorizationFilter);

            options.Authorization = filters;

            return options;
        }

        public static DashboardOptions AddAuthorizations(
            [NotNull] this DashboardOptions options,
            [NotNull] IEnumerable<IDashboardAuthorizationFilter> authorizationFilters)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(authorizationFilters, nameof(authorizationFilters));

            List<IDashboardAuthorizationFilter> filters = new List<IDashboardAuthorizationFilter>();
            filters.AddRange(options.Authorization);
            filters.AddIfNotContains(authorizationFilters);

            options.Authorization = filters;

            return options;
        }

        public static DashboardOptions UseAuthorization(
            [NotNull] this DashboardOptions options,
            [NotNull] IDashboardAuthorizationFilter authorizationFilter)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(authorizationFilter, nameof(authorizationFilter));

            List<IDashboardAuthorizationFilter> filters = new List<IDashboardAuthorizationFilter>
            {
                authorizationFilter
            };

            options.Authorization = filters;

            return options;
        }

        public static DashboardOptions UseAuthorizations(
            [NotNull] this DashboardOptions options,
            [NotNull] IEnumerable<IDashboardAuthorizationFilter> authorizationFilters)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(authorizationFilters, nameof(authorizationFilters));

            List<IDashboardAuthorizationFilter> filters = new List<IDashboardAuthorizationFilter>();
            filters.AddRange(authorizationFilters);

            options.Authorization = filters;

            return options;
        }
    }
}
