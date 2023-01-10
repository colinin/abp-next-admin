using Elsa.Builders;
using System;
using System.Runtime.CompilerServices;

namespace LINGYUN.Abp.Elsa.Activities.Sms;
public static class SendSmsBuilderExtensions
{
    public static IActivityBuilder SendSms(
        this IBuilder builder,
        Action<ISetupActivity<SendSms>>? setup,
        [CallerLineNumber] int lineNumber = default,
        [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);
}
