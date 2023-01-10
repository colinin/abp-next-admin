using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

public static class BlobActivityExtensions
{
    public static ISetupActivity<BlobActivity> WithPath(
        this ISetupActivity<BlobActivity> activity,
        Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.Path, value);

    public static ISetupActivity<BlobActivity> WithPath(
        this ISetupActivity<BlobActivity> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.Path, value);

    public static ISetupActivity<BlobActivity> WithPath(
        this ISetupActivity<BlobActivity> activity,
        Func<string?> value) => activity.Set(x => x.Path, value);

    public static ISetupActivity<BlobActivity> WithPath(
        this ISetupActivity<BlobActivity> activity,
        string? value) => activity.Set(x => x.Path, value);
}
