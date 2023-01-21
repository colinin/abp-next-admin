using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Sms;
public static class SendSmsExtensions
{
    public static ISetupActivity<SendSms> WithMessage(
        this ISetupActivity<SendSms> activity,
        Func<ActivityExecutionContext, ValueTask<string>> value) => activity.Set(x => x.Message, value!);

    public static ISetupActivity<SendSms> WithMessage(
        this ISetupActivity<SendSms> activity,
        Func<ActivityExecutionContext, string> value) => activity.Set(x => x.Message, value!);

    public static ISetupActivity<SendSms> WithMessage(
        this ISetupActivity<SendSms> activity,
        Func<string> value) => activity.Set(x => x.Message, value!);

    public static ISetupActivity<SendSms> WithMessage(
        this ISetupActivity<SendSms> activity,
        string value) => activity.Set(x => x.Message, value!);

    public static ISetupActivity<SendSms> WithTo(
       this ISetupActivity<SendSms> activity,
       Func<ActivityExecutionContext, ValueTask<ICollection<string>>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendSms> WithTo(
        this ISetupActivity<SendSms> activity,
        Func<ActivityExecutionContext, ICollection<string>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendSms> WithTo(
        this ISetupActivity<SendSms> activity,
        Func<ICollection<string>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendSms> WithTo(
        this ISetupActivity<SendSms> activity,
        ICollection<string> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendSms> WithProperties(
       this ISetupActivity<SendSms> activity,
       Func<ActivityExecutionContext, ValueTask<Dictionary<string, string>>> value) => activity.Set(x => x.Properties, value!);

    public static ISetupActivity<SendSms> WithProperties(
        this ISetupActivity<SendSms> activity,
        Func<ActivityExecutionContext, Dictionary<string, string>> value) => activity.Set(x => x.Properties, value!);

    public static ISetupActivity<SendSms> WithProperties(
        this ISetupActivity<SendSms> activity,
        Func<Dictionary<string, string>> value) => activity.Set(x => x.Properties, value!);

    public static ISetupActivity<SendSms> WithProperties(
        this ISetupActivity<SendSms> activity,
        Dictionary<string, string> value) => activity.Set(x => x.Properties, value!);
}
