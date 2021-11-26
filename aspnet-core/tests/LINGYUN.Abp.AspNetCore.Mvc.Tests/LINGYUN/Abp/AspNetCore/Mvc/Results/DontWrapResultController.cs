using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    [Route("api/dont/wrap-result-test")]
    public class DontWrapResultController: AbpController
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
                Name = "Not Wrap",
                Properties = new Dictionary<string, string>
                {
                    { "TestKey", "TestValue" }
                }
            };
        }
    }
}
