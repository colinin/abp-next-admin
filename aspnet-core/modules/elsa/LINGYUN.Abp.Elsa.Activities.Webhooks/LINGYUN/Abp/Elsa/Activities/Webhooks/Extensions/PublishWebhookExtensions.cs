using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks;
public static class PublishWebhookExtensions
{
    public static ISetupActivity<PublishWebhook> WithWebhooName(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, ValueTask<string>> value) => activity.Set(x => x.WebhooName, value!);

    public static ISetupActivity<PublishWebhook> WithWebhooName(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, string> value) => activity.Set(x => x.WebhooName, value!);

    public static ISetupActivity<PublishWebhook> WithWebhooName(
        this ISetupActivity<PublishWebhook> activity,
        Func<string> value) => activity.Set(x => x.WebhooName, value!);

    public static ISetupActivity<PublishWebhook> WithWebhooName(
        this ISetupActivity<PublishWebhook> activity,
        string value) => activity.Set(x => x.WebhooName, value!);

    public static ISetupActivity<PublishWebhook> WithWebhookData(
       this ISetupActivity<PublishWebhook> activity,
       Func<ActivityExecutionContext, ValueTask<object>> value) => activity.Set(x => x.WebhookData, value!);

    public static ISetupActivity<PublishWebhook> WithWebhookData(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, object> value) => activity.Set(x => x.WebhookData, value!);

    public static ISetupActivity<PublishWebhook> WithWebhookData(
        this ISetupActivity<PublishWebhook> activity,
        Func<object> value) => activity.Set(x => x.WebhookData, value!);

    public static ISetupActivity<PublishWebhook> WithWebhookData(
        this ISetupActivity<PublishWebhook> activity,
        object value) => activity.Set(x => x.WebhookData, value!);

    public static ISetupActivity<PublishWebhook> WithSendExactSameData(
       this ISetupActivity<PublishWebhook> activity,
       Func<ActivityExecutionContext, ValueTask<bool>> value) => activity.Set(x => x.SendExactSameData, value!);

    public static ISetupActivity<PublishWebhook> WithSendExactSameData(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, bool> value) => activity.Set(x => x.SendExactSameData, value!);

    public static ISetupActivity<PublishWebhook> WithSendExactSameData(
        this ISetupActivity<PublishWebhook> activity,
        Func<bool> value) => activity.Set(x => x.SendExactSameData, value!);

    public static ISetupActivity<PublishWebhook> WithSendExactSameData(
        this ISetupActivity<PublishWebhook> activity,
        bool value) => activity.Set(x => x.SendExactSameData, value!);

    public static ISetupActivity<PublishWebhook> WithUseOnlyGivenHeaders(
      this ISetupActivity<PublishWebhook> activity,
      Func<ActivityExecutionContext, ValueTask<bool>> value) => activity.Set(x => x.UseOnlyGivenHeaders, value!);

    public static ISetupActivity<PublishWebhook> WithUseOnlyGivenHeaders(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, bool> value) => activity.Set(x => x.UseOnlyGivenHeaders, value!);

    public static ISetupActivity<PublishWebhook> WithUseOnlyGivenHeaders(
        this ISetupActivity<PublishWebhook> activity,
        Func<bool> value) => activity.Set(x => x.UseOnlyGivenHeaders, value!);

    public static ISetupActivity<PublishWebhook> WithUseOnlyGivenHeaders(
        this ISetupActivity<PublishWebhook> activity,
        bool value) => activity.Set(x => x.UseOnlyGivenHeaders, value!);

    public static ISetupActivity<PublishWebhook> WithHeaders(
       this ISetupActivity<PublishWebhook> activity,
       Func<ActivityExecutionContext, ValueTask<IDictionary<string, string>>> value) => activity.Set(x => x.Headers, value!);

    public static ISetupActivity<PublishWebhook> WithHeaders(
        this ISetupActivity<PublishWebhook> activity,
        Func<ActivityExecutionContext, IDictionary<string, string>> value) => activity.Set(x => x.Headers, value!);

    public static ISetupActivity<PublishWebhook> WithHeaders(
        this ISetupActivity<PublishWebhook> activity,
        Func<IDictionary<string, string>> value) => activity.Set(x => x.Headers, value!);

    public static ISetupActivity<PublishWebhook> WithHeaders(
        this ISetupActivity<PublishWebhook> activity,
        IDictionary<string, string> value) => activity.Set(x => x.Headers, value!);
}
