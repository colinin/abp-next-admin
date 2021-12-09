using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.MicroService.Internal.ApiGateway.Utils
{
    public interface ILoadBalancerFinder
    {
        Task<List<LoadBalancerDescriptor>> GetLoadBalancersAsync();
    }

    public class LoadBalancerDescriptor
    {
        public string Type { get; }
        public string DisplayName { get; }
        public LoadBalancerDescriptor(string type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }
    }
}
