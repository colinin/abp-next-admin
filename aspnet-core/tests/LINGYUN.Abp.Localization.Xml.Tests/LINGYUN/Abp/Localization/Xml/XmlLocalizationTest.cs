using Microsoft.Extensions.Localization;
using Shouldly;
using System.IO;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.Localization.Xml
{
    public class XmlLocalizationTest : AbpLocalizationXmlTestBase
    {
        private readonly static string[] _localizerFiles = new string[]
        {
            Path.Combine("./TestResources", "zh-Hans.xml"),
            Path.Combine("./TestResources", "en.xml")
        };

        private readonly IStringLocalizer<LocalizationTestResource> _localizer;

        public XmlLocalizationTest()
        {
            _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
        }

        [Fact]
        public void Should_Get_Physical_Localized_Text_If_Defined_In_Current_Culture()
        {
            Init();

            using (CultureHelper.Use("zh-Hans"))
            {
                _localizer["Hello World"].Value.ShouldBe("世界你好!");
                _localizer["C# Test"].Value.ShouldBe("C#测试");
            }

            using (CultureHelper.Use("en"))
            {
                _localizer["Hello World"].Value.ShouldBe("Hello World!");
                _localizer["C# Test"].Value.ShouldBe("C# Test!");
            }
        }

        [Fact]
        public void Should_Get_Virtual_Localized_Text_If_Defined_In_Current_Culture()
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

        private static void Init()
        {
            var zhHansfile = new XmlLocalizationFile("zh-Hans")
            {
                Texts = new LocalizationText[]
                {
                    new LocalizationText("Hello World", "世界你好!"),
                    new LocalizationText("C# Test", "C#测试")
                }
            };

            zhHansfile.WriteToPath("./TestResources");

            var enFile = new XmlLocalizationFile("en")
            {
                Texts = new LocalizationText[]
                {
                    new LocalizationText("Hello World", "Hello World!"),
                    new LocalizationText("C# Test", "C# Test!")
                }
            };

            enFile.WriteToPath("./TestResources");
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var file in _localizerFiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
