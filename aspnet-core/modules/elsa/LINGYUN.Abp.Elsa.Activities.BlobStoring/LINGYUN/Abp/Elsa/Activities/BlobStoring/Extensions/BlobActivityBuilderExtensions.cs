using Elsa.Builders;
using System;
using System.Runtime.CompilerServices;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;
public static class BlobActivityBuilderExtensions
{
    public static IActivityBuilder BlobExists(
        this IBuilder builder,
        Action<ISetupActivity<BlobExists>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);

    public static IActivityBuilder DeleteBlob(
        this IBuilder builder,
        Action<ISetupActivity<DeleteBlob>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);

    public static IActivityBuilder ReadBlob(
        this IBuilder builder,
        Action<ISetupActivity<ReadBlob>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);

    public static IActivityBuilder WriteBlob(
        this IBuilder builder,
        Action<ISetupActivity<WriteBlob>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);
}
