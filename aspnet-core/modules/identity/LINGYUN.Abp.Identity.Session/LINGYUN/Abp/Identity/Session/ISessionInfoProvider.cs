using System;

namespace LINGYUN.Abp.Identity.Session;

[Obsolete("The ISessionInfoProvider has been deprecated. Please use ICurrentUser.FindSessionId() instead.")]
public interface ISessionInfoProvider
{
    string? SessionId { get; }

    IDisposable Change(string? sessionId);
}
