using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackendAdmin.Tests
{
    public class ProxyConnectTestMiddleware
    {
        private readonly RequestDelegate _next;
        public ProxyConnectTestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/connect"))
            {
                var contentBuilder = new StringBuilder();

                contentBuilder.AppendLine("Connection:");
                contentBuilder.AppendLine($"Id: {context.Connection.Id}");
                contentBuilder.AppendLine($"LocalIpAddress: {context.Connection.LocalIpAddress}:{context.Connection.LocalPort}");
                contentBuilder.AppendLine($"RemoteIpAddress: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}");
                contentBuilder.AppendLine();
                contentBuilder.AppendLine("Headers:");
                contentBuilder.Append(context.Request.Headers.Select(h => $"Key:{h.Key}, Value:{h.Value}").JoinAsString(Environment.NewLine));
                contentBuilder.AppendLine();
                contentBuilder.AppendLine();
                contentBuilder.AppendLine("Host:");
                contentBuilder.AppendLine(context.Request.Host.ToUriComponent());
                contentBuilder.AppendLine();
                contentBuilder.AppendLine("Scheme:");
                contentBuilder.AppendLine(context.Request.Scheme);
                await context.Response.WriteAsync(contentBuilder.ToString());
                return;
            }
            await _next(context);
        }
    }
}
