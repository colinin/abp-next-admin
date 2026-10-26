using System.Security.Claims;
using LINGYUN.Abp.Account.Web.OpenIddict.Handlers;
using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OpenIddict.Server;
using OpenIddict.Validation;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Xunit;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Tests;

public class TokenValidationLoggingTests
{
    public static TheoryData<string?, string?, bool> Requests => new()
    {
        { "https://example.invalid/signalr-hubs/notifications?id=fixture-id&access_token=fixture-first&access_token=fixture-second#fixture-fragment", "/signalr-hubs/notifications", true },
        { "https://example.invalid/connect/token?%61ccess_token=fixture-encoded%26secret&refresh_token=fixture-refresh", "/connect/token", false },
        { "https://example.invalid/api/profile", "/api/profile", true },
        { null, null, false }
    };

    [Theory]
    [MemberData(nameof(Requests))]
    public async Task Validation_logs_only_path_and_preserves_session_decision(string? requestUri, string? path, bool validSession)
    {
        var principal = CreatePrincipal();
        var checker = Substitute.For<IIdentitySessionChecker>();
        checker.ValidateSessionAsync(principal).Returns(validSession);
        var logger = new RecordingLogger<ValidationTokenCheckIdentitySession>();
        var handler = new ValidationTokenCheckIdentitySession(Substitute.For<ICurrentTenant>(), checker) { Logger = logger };
        var context = new OpenIddictValidationEvents.ValidateTokenContext(new OpenIddictValidationTransaction
        {
            RequestUri = requestUri is null ? null : new Uri(requestUri)
        }) { Principal = principal };

        await handler.HandleAsync(context);

        AssertSafeLog(logger, LogLevel.Debug, path);
        Assert.Equal(!validSession, context.IsRejected);
        await checker.Received(1).ValidateSessionAsync(principal);
    }

    [Theory]
    [MemberData(nameof(Requests))]
    public async Task Server_validation_logs_only_path_and_preserves_session_decision(string? requestUri, string? path, bool validSession)
    {
        var principal = CreatePrincipal();
        var checker = Substitute.For<IIdentitySessionChecker>();
        checker.ValidateSessionAsync(principal).Returns(validSession);
        var logger = new RecordingLogger<ServerValidationTokenCheckIdentitySession>();
        var handler = new ServerValidationTokenCheckIdentitySession(Substitute.For<ICurrentTenant>(), checker) { Logger = logger };
        var context = new OpenIddictServerEvents.ValidateTokenContext(new OpenIddictServerTransaction
        {
            RequestUri = requestUri is null ? null : new Uri(requestUri)
        }) { Principal = principal };

        await handler.HandleAsync(context);

        AssertSafeLog(logger, LogLevel.Debug, path);
        Assert.Equal(!validSession, context.IsRejected);
        await checker.Received(1).ValidateSessionAsync(principal);
    }

    private static ClaimsPrincipal CreatePrincipal() => new(new ClaimsIdentity(
        [new Claim(AbpClaimTypes.SessionId, Guid.NewGuid().ToString())], "Bearer"));

    private static void AssertSafeLog<T>(RecordingLogger<T> logger, LogLevel expectedLevel, string? path)
    {
        var entry = logger.Entries[0];
        Assert.Equal(expectedLevel, entry.Level);
        Assert.Equal(path, entry.Properties.Single(property => property.Key == "requestPath").Value);
        Assert.Contains(entry.Properties, property => property.Key == "endpointType");
        Assert.DoesNotContain("fixture-", entry.Message);
        Assert.DoesNotContain("access_token", entry.Message);
        Assert.DoesNotContain(entry.Properties, property => property.Value is Uri);
        Assert.DoesNotContain(entry.Properties, property => property.Value?.ToString()?.Contains("fixture-") == true);
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<(LogLevel Level, string Message, KeyValuePair<string, object?>[] Properties)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) => Entries.Add((logLevel, formatter(state, exception),
                ((IEnumerable<KeyValuePair<string, object?>>)state!).ToArray()));
    }
}
