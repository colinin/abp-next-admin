using Elsa.Builders;
using System;
using System.Runtime.CompilerServices;

namespace LINGYUN.Abp.Elsa.Activities.Notifications;
public static class SendNotificationBuilderExtensions
{
    public static IActivityBuilder SendNotification(
        this IBuilder builder,
        Action<ISetupActivity<SendNotification>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);
}
