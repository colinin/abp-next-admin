using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.OpenApi
{
    [Route("/api/invoke")]
    public class FakeInvokeController : AbpController
    {
        [HttpGet]
        public Task<string> Index()
        {
            return Task.FromResult("Hello");
        }
    }
}
