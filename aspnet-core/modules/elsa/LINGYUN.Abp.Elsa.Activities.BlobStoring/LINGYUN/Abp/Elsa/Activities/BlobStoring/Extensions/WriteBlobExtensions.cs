using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;
public static class WriteBlobExtensions
{
    public static ISetupActivity<WriteBlob> WithOverwrite(
        this ISetupActivity<WriteBlob> activity,
        Func<ActivityExecutionContext, ValueTask<bool>> value) => activity.Set(x => x.Overwrite, value);

    public static ISetupActivity<WriteBlob> WithOverwrite(
        this ISetupActivity<WriteBlob> activity,
        Func<ActivityExecutionContext, bool> value) => activity.Set(x => x.Overwrite, value);

    public static ISetupActivity<WriteBlob> WithOverwrite(
        this ISetupActivity<WriteBlob> activity,
        Func<bool> value) => activity.Set(x => x.Overwrite, value);

    public static ISetupActivity<WriteBlob> WithOverwrite(
        this ISetupActivity<WriteBlob> activity,
        bool value) => activity.Set(x => x.Overwrite, value);

    public static ISetupActivity<WriteBlob> WithBytes(
        this ISetupActivity<WriteBlob> activity,
        Func<ActivityExecutionContext, ValueTask<byte[]?>> value) => activity.Set(x => x.Bytes, value);

    public static ISetupActivity<WriteBlob> WithBytes(
        this ISetupActivity<WriteBlob> activity,
        Func<ActivityExecutionContext, byte[]?> value) => activity.Set(x => x.Bytes, value);

    public static ISetupActivity<WriteBlob> WithBytes(
        this ISetupActivity<WriteBlob> activity,
        Func<byte[]?> value) => activity.Set(x => x.Bytes, value);

    public static ISetupActivity<WriteBlob> WithBytes(
        this ISetupActivity<WriteBlob> activity,
        byte[]? value) => activity.Set(x => x.Bytes, value);
}
