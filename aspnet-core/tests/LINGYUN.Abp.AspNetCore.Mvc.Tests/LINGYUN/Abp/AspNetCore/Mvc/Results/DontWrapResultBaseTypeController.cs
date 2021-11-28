using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    [Route("api/dont-base-type/wrap-result-test")]
    public class DontWrapResultBaseTypeController : AbpController, IWrapDisabled
    {
        [HttpGet]
        public TestResultObject Wrap()
        {
            return new TestResultObject
            {
                Id = Guid.NewGuid(),
                DateTime = Clock.Now,
                Double = 3.141592653d,
                Integer = 100,
                Name = "Not Wrap For Base Type",
                Properties = new Dictionary<string, string>
                {
                    { "TestKey", "TestValue" }
                }
            };
        }
    }
}
