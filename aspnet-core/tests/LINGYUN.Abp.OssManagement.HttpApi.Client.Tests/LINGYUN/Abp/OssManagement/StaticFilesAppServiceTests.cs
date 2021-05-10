using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Client.DynamicProxying;
using Xunit;

namespace LINGYUN.Abp.OssManagement
{
    public class OssObjectAppServiceTests : AbpOssManagementHttpApiClientTestBase
    {
        private readonly IOssObjectAppService _service;

        public OssObjectAppServiceTests()
        {
            _service = GetRequiredService<IOssObjectAppService>();
        }

        [Fact]
        public async Task Get_By_Bucket_And_Object_Shouldly_Not_Null()
        {
            var ossObject = await _service
                .GetAsync(
                    new GetOssObjectInput
                    {
                        Bucket = "abp-file-management",
                        Object = "123.png"
                    });

            ossObject.ShouldNotBeNull();
        }
    }
}
