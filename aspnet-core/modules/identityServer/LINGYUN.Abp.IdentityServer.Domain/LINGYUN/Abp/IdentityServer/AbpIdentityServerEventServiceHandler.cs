using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Internal;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.IdentityServer;

/// <summary>
/// The default event service
/// </summary>
/// <seealso cref="DefaultEventService" />
public class AbpIdentityServerEventServiceHandler : IAbpIdentityServerEventServiceHandler, ITransientDependency
{
    /// <summary>
    /// The options
    /// </summary>
    protected readonly IdentityServerOptions Options;

    /// <summary>
    /// The context
    /// </summary>
    protected readonly IHttpContextAccessor Context;

    /// <summary>
    /// The sink
    /// </summary>
    protected readonly IEventSink Sink;

    /// <summary>
    /// The clock
    /// </summary>
    protected readonly IClock Clock;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultEventService"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="context">The context.</param>
    /// <param name="sink">The sink.</param>
    /// <param name="clock">The clock.</param>
    public AbpIdentityServerEventServiceHandler(IdentityServerOptions options, IHttpContextAccessor context, IEventSink sink, IClock clock)
    {
        Options = options;
        Context = context;
        Sink = sink;
        Clock = clock;
    }

    /// <summary>
    /// Raises the specified event.
    /// </summary>
    /// <param name="evt">The event.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">evt</exception>
    public async virtual Task RaiseAsync(Event evt)
    {
        if (evt == null) throw new ArgumentNullException(nameof(evt));

        if (CanRaiseEvent(evt))
        {
            await PrepareEventAsync(evt);
            await Sink.PersistAsync(evt);
        }
    }

    /// <summary>
    /// Indicates if the type of event will be persisted.
    /// </summary>
    /// <param name="evtType"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public virtual bool CanRaiseEventType(EventTypes evtType)
    {
        return evtType switch
        {
            EventTypes.Failure => Options.Events.RaiseFailureEvents,
            EventTypes.Information => Options.Events.RaiseInformationEvents,
            EventTypes.Success => Options.Events.RaiseSuccessEvents,
            EventTypes.Error => Options.Events.RaiseErrorEvents,
            _ => throw new ArgumentOutOfRangeException(nameof(evtType)),
        };
    }

    /// <summary>
    /// Determines whether this event would be persisted.
    /// </summary>
    /// <param name="evt">The evt.</param>
    /// <returns>
    ///   <c>true</c> if this event would be persisted; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool CanRaiseEvent(Event evt)
    {
        return CanRaiseEventType(evt.EventType);
    }

    /// <summary>
    /// Prepares the event.
    /// </summary>
    /// <param name="evt">The evt.</param>
    /// <returns></returns>
    protected virtual Task PrepareEventAsync(Event evt)
    {
        evt.ActivityId = Context.HttpContext.TraceIdentifier;
        evt.TimeStamp = Clock.Now;
        evt.ProcessId = Process.GetCurrentProcess().Id;

        if (Context.HttpContext.Connection.LocalIpAddress != null)
        {
            evt.LocalIpAddress = Context.HttpContext.Connection.LocalIpAddress.ToString() + ":" + Context.HttpContext.Connection.LocalPort;
        }
        else
        {
            evt.LocalIpAddress = "unknown";
        }

        if (Context.HttpContext.Connection.RemoteIpAddress != null)
        {
            evt.RemoteIpAddress = Context.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        else
        {
            evt.RemoteIpAddress = "unknown";
        }
        // TODO: Event.PrepareAsync();
        // await evt.PrepareAsync();

        return Task.CompletedTask;
    }
}
