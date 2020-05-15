using System.Threading.Tasks;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IRouteGroupChecker
    {
        Task CheckActiveAsync(string appId);
    }
}
