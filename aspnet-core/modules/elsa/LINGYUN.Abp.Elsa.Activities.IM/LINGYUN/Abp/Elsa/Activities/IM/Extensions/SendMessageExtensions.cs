using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.IM;
public static class SendMessageExtensions
{
    public static ISetupActivity<SendMessage> WithContent(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, ValueTask<string>> value) => activity.Set(x => x.Content, value!);

    public static ISetupActivity<SendMessage> WithContent(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, string> value) => activity.Set(x => x.Content, value!);

    public static ISetupActivity<SendMessage> WithContent(
        this ISetupActivity<SendMessage> activity,
        Func<string> value) => activity.Set(x => x.Content, value!);

    public static ISetupActivity<SendMessage> WithContent(
        this ISetupActivity<SendMessage> activity,
        string value) => activity.Set(x => x.Content, value!);

    public static ISetupActivity<SendMessage> WithFormUser(
       this ISetupActivity<SendMessage> activity,
       Func<ActivityExecutionContext, ValueTask<Guid>> value) => activity.Set(x => x.FormUser, value!);

    public static ISetupActivity<SendMessage> WithFormUser(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, Guid> value) => activity.Set(x => x.FormUser, value!);

    public static ISetupActivity<SendMessage> WithFormUser(
        this ISetupActivity<SendMessage> activity,
        Func<Guid> value) => activity.Set(x => x.FormUser, value!);

    public static ISetupActivity<SendMessage> WithFormUser(
        this ISetupActivity<SendMessage> activity,
        Guid value) => activity.Set(x => x.FormUser, value!);

    public static ISetupActivity<SendMessage> WithFormUserName(
       this ISetupActivity<SendMessage> activity,
       Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.FormUserName, value);

    public static ISetupActivity<SendMessage> WithFormUserName(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.FormUserName, value);

    public static ISetupActivity<SendMessage> WithFormUserName(
        this ISetupActivity<SendMessage> activity,
        Func<string?> value) => activity.Set(x => x.FormUserName, value);

    public static ISetupActivity<SendMessage> WithFormUserName(
        this ISetupActivity<SendMessage> activity,
        string? value) => activity.Set(x => x.FormUserName, value);

    public static ISetupActivity<SendMessage> WithTo(
       this ISetupActivity<SendMessage> activity,
       Func<ActivityExecutionContext, ValueTask<Guid?>> value) => activity.Set(x => x.To, value);

    public static ISetupActivity<SendMessage> WithTo(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, Guid?> value) => activity.Set(x => x.To, value);

    public static ISetupActivity<SendMessage> WithTo(
        this ISetupActivity<SendMessage> activity,
        Func<Guid?> value) => activity.Set(x => x.To, value);

    public static ISetupActivity<SendMessage> WithTo(
        this ISetupActivity<SendMessage> activity,
        Guid? value) => activity.Set(x => x.To, value);

    public static ISetupActivity<SendMessage> WithGroupId(
       this ISetupActivity<SendMessage> activity,
       Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.GroupId, value);

    public static ISetupActivity<SendMessage> WithGroupId(
        this ISetupActivity<SendMessage> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.GroupId, value);

    public static ISetupActivity<SendMessage> WithGroupId(
        this ISetupActivity<SendMessage> activity,
        Func<string?> value) => activity.Set(x => x.GroupId, value);

    public static ISetupActivity<SendMessage> WithGroupId(
        this ISetupActivity<SendMessage> activity,
        string? value) => activity.Set(x => x.GroupId, value);
}
