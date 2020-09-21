using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.ApiGateway.Utils
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
