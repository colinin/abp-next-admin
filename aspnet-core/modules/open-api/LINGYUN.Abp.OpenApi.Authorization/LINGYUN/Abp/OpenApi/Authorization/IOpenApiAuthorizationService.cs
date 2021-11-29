using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenApi.Authorization
{
    public interface IOpenApiAuthorizationService
    {
        Task<bool> AuthorizeAsync(HttpContext httpContext);
    }
}
