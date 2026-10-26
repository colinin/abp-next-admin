# OpenIddict session validation diagnostics

The server and validation handlers log the endpoint type and the request URI's absolute path at Debug level. URI query parameters, fragments and authority information are excluded from both the rendered message and structured URI fields, because requests such as SignalR connections can carry access tokens in their query strings.

A missing request URI is logged as a null path. Session validation and rejection behavior remain the responsibility of the existing handlers.

Run the focused logging tests from the repository root:

```sh
dotnet test aspnet-core/tests/LINGYUN.Abp.Account.Web.OpenIddict.Tests/LINGYUN.Abp.Account.Web.OpenIddict.Tests.csproj
```
