using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Emailing;

public static class SendEmailingExtensions
{
    public static ISetupActivity<SendEmailing> WithTo(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, ValueTask<ICollection<string>>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendEmailing> WithTo(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, ICollection<string>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendEmailing> WithTo(
        this ISetupActivity<SendEmailing> activity,
        Func<ICollection<string>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendEmailing> WithTo(
        this ISetupActivity<SendEmailing> activity,
        ICollection<string> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendEmailing> WithSubject(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.Subject, value);

    public static ISetupActivity<SendEmailing> WithSubject(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.Subject, value);

    public static ISetupActivity<SendEmailing> WithSubject(
        this ISetupActivity<SendEmailing> activity,
        Func<string?> value) => activity.Set(x => x.Subject, value);

    public static ISetupActivity<SendEmailing> WithSubject(
        this ISetupActivity<SendEmailing> activity,
        string? value) => activity.Set(x => x.Subject, value);

    public static ISetupActivity<SendEmailing> WithBody(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.Body, value);

    public static ISetupActivity<SendEmailing> WithBody(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.Body, value);

    public static ISetupActivity<SendEmailing> WithBody(
        this ISetupActivity<SendEmailing> activity,
        Func<string?> value) => activity.Set(x => x.Body, value);

    public static ISetupActivity<SendEmailing> WithBody(
        this ISetupActivity<SendEmailing> activity,
        string? value) => activity.Set(x => x.Body, value);

    public static ISetupActivity<SendEmailing> WithCulture(
       this ISetupActivity<SendEmailing> activity,
       Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.Culture, value);

    public static ISetupActivity<SendEmailing> WithCulture(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.Culture, value);

    public static ISetupActivity<SendEmailing> WithCulture(
        this ISetupActivity<SendEmailing> activity,
        Func<string?> value) => activity.Set(x => x.Culture, value);

    public static ISetupActivity<SendEmailing> WithCulture(
        this ISetupActivity<SendEmailing> activity,
        string? value) => activity.Set(x => x.Culture, value);

    public static ISetupActivity<SendEmailing> WithTemplate(
      this ISetupActivity<SendEmailing> activity,
      Func<ActivityExecutionContext, ValueTask<string?>> value) => activity.Set(x => x.Template, value);

    public static ISetupActivity<SendEmailing> WithTemplate(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, string?> value) => activity.Set(x => x.Template, value);

    public static ISetupActivity<SendEmailing> WithTemplate(
        this ISetupActivity<SendEmailing> activity,
        Func<string?> value) => activity.Set(x => x.Template, value);

    public static ISetupActivity<SendEmailing> WithTemplate(
        this ISetupActivity<SendEmailing> activity,
        string? value) => activity.Set(x => x.Template, value);

    public static ISetupActivity<SendEmailing> WithModel(
      this ISetupActivity<SendEmailing> activity,
      Func<ActivityExecutionContext, ValueTask<object?>> value) => activity.Set(x => x.Model, value);

    public static ISetupActivity<SendEmailing> WithModel(
        this ISetupActivity<SendEmailing> activity,
        Func<ActivityExecutionContext, object?> value) => activity.Set(x => x.Model, value);

    public static ISetupActivity<SendEmailing> WithModel(
        this ISetupActivity<SendEmailing> activity,
        Func<object?> value) => activity.Set(x => x.Model, value);

    public static ISetupActivity<SendEmailing> WithModel(
        this ISetupActivity<SendEmailing> activity,
        object? value) => activity.Set(x => x.Model, value);
}
