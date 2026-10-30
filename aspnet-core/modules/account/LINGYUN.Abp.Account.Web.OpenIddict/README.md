# OpenIddict session validation diagnostics

The server and validation handlers log the endpoint type and the request URI's absolute path at Debug level. URI query parameters, fragments and authority information are excluded from both the rendered message and structured URI fields, because requests such as SignalR connections can carry access tokens in their query strings.

A missing request URI is logged as a null path. Session validation and rejection behavior remain the responsibility of the existing handlers.
