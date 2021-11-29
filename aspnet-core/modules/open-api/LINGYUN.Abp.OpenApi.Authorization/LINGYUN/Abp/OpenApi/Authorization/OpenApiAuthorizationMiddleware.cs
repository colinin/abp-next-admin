using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OpenApi.Authorization
{
    public class OpenApiAuthorizationMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IOpenApiAuthorizationService _authorizationService;
        public OpenApiAuthorizationMiddleware(
            IOpenApiAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _authorizationService.AuthorizeAsync(context))
            {
                await next(context);
            }
        }
    }
}
