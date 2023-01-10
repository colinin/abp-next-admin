using Elsa.Builders;
using System;
using System.Runtime.CompilerServices;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks;
public static class PublishWebhookBuilderExtensions
{
    public static IActivityBuilder PublishWebhook(
        this IBuilder builder,
        Action<ISetupActivity<PublishWebhook>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);
}
