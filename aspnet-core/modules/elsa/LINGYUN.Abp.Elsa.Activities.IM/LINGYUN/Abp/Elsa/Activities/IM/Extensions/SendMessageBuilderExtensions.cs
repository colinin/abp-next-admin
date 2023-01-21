using Elsa.Builders;
using System;
using System.Runtime.CompilerServices;

namespace LINGYUN.Abp.Elsa.Activities.IM;
public static class SendEmailingBuilderExtensions
{
    public static IActivityBuilder SendMessage(
        this IBuilder builder,
        Action<ISetupActivity<SendMessage>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);
}
