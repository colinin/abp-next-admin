using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    [Route("api/wrap-result-test")]
    public class WrapResultController: AbpController
    {
        public WrapResultController() 
        {
            LocalizationResource = typeof(MvcTestResource);
        }

        [HttpGet]
        [Route("get-text")]
        public async Task<IRemoteStreamContent> DontWrapRemoteStreamContext()
        {
            var textBytes = Encoding.UTF8.GetBytes("text");

            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(textBytes);

            return new RemoteStreamContent(memoryStream);
        }

        [HttpGet]
        [Route("exception")]
        public TestResultObject WrapBusinessException()
        {
            throw new BusinessException("Test:1001");
        }

        [HttpGet]
        [Route("wrap")]
        public TestResultObject Wrap()
        {
            return new TestResultObject
            {
                Id = Guid.NewGuid(),
                DateTime = Clock.Now,
                Double = 3.141592653d,
                Integer = 100,
                Name = "Wrap",
                Properties = new Dictionary<string, string>
                {
                    { "TestKey", "TestValue" }
                }
            };
        }

        [HttpGet]
        [Route("wrap-empty")]
        public TestResultObject WrapEmpty()
        {
            return null;
        }

        [HttpGet]
        [Route("not-wrap")]
        [IgnoreWrapResult]
        public TestResultObject NotWrap()
        {
            return new TestResultObject
            {
                Id = Guid.NewGuid(),
                DateTime = Clock.Now,
                Double = 3.141592653d,
                Integer = 100,
                Name = "Not Wrap",
                Properties = new Dictionary<string, string>
                {
                    { "TestKey", "TestValue" }
                }
            };
        }

        [HttpGet]
        [Route("not-wrap-exception")]
        public TestResultObject NotWrapHasDbException()
        {
            throw new HasDbException();
        }

        [HttpPut]
        [Route("not-wrap-204")]
        public Task NotWrapWith204()
        {
            return Task.CompletedTask;
        }
    }
}
