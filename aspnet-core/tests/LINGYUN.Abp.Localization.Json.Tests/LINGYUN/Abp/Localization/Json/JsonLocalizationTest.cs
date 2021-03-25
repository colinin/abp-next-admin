using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.Localization.Json
{
    public class JsonLocalizationTest : AbpLocalizationJsonTestBase
    {
        private readonly IStringLocalizer<LocalizationTestResource> _localizer;

        public JsonLocalizationTest()
        {
            _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
        }


        [Fact]
        public void Should_Get_Physical_Localized_Text_If_Defined_In_Current_Culture()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                _localizer["Hello China"].Value.ShouldBe("China 你好!");
            }

            using (CultureHelper.Use("en"))
            {
                _localizer["Hello China"].Value.ShouldBe("Hello China!");
            }
        }
    }
}
