using LINGYUN.Abp.AuditLogging.OpenObserve.Localization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

public class AuditLoggingOpenObserveLocalization_Tests : AbpAuditLoggingOpenObserveTestBase
{
    private readonly IStringLocalizer<AuditLoggingOpenObserveResource> _localizer;

    public AuditLoggingOpenObserveLocalization_Tests()
    {
        _localizer = GetRequiredService<IStringLocalizer<AuditLoggingOpenObserveResource>>();
    }

    [Fact]
    public void Should_Localize_Delete_Not_Supported_Message()
    {
        using (CultureHelper.Use("zh-Hans"))
        {
            _localizer[AuditLoggingOpenObserveErrorCodes.DeleteNotSupported]
                .Value
                .ShouldBe("当前日志设施不支持删除记录!");
        }
    }
}
