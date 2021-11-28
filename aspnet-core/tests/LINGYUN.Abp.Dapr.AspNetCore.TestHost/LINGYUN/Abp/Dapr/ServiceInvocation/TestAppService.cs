using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Dapr.ServiceInvocation
{
    [RemoteService(Name = "TestDapr")]
    [Route("api/dapr/test")]
    public class TestAppService : AbpController, ITestAppService
    {
        private static int _inctement;
        private readonly List<NameValue> _cache = new List<NameValue>
                    {
                        new NameValue("name1", "value1"),
                        new NameValue("name2", "value2"),
                        new NameValue("name3", "value3"),
                        new NameValue("name4", "value4"),
                        new NameValue("name5", "value5")
                    };

        [HttpGet]
        public Task<ListResultDto<NameValue>> GetAsync()
        {
            return Task.FromResult(new ListResultDto<NameValue>(_cache));
        }

        [HttpPut]
        public Task<NameValue> UpdateAsync()
        {
            Interlocked.Increment(ref _inctement);

            _cache[0].Value = $"value:updated:{_inctement}";

            return Task.FromResult(_cache[0]);
        }

        [HttpGet]
        [Route("{name}")]
        public Task<TestNeedWrapObject> GetWrapedAsync(string name)
        {
            var obj = new TestNeedWrapObject
            {
                Name = name
            };

            return Task.FromResult(obj);
        }
    }
}
