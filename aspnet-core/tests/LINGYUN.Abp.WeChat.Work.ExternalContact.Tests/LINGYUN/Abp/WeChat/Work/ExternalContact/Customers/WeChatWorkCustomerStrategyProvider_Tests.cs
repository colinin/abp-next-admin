using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers;
public class WeChatWorkCustomerStrategyProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkCustomerStrategyProvider _provider;
    public readonly IConfiguration _configuration;
    public WeChatWorkCustomerStrategyProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkCustomerStrategyProvider>();
        _configuration = GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async virtual Task Should_Get_Customer_Strategy_List()
    {
        var res = await _provider.GetCustomerStrategyListAsync(
            new WeChatWorkGetCustomerStrategyListRequest());

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Get_Customer_Strategy()
    {
        var exception = await Assert.ThrowsAsync<AbpWeChatWorkException>(async () =>
            await _provider.GetCustomerStrategyAsync(
                new WeChatWorkGetCustomerStrategyRequest(0)));

        exception.Code.ShouldBe($"WeChatWork:40058");
        exception.Message.ShouldContain($"invalid strategy_id:0");
    }

    [Fact]
    public async virtual Task Should_Get_Customer_Strategy_Range()
    {
        var exception = await Assert.ThrowsAsync<AbpWeChatWorkException>(async () =>
            await _provider.GetCustomerStrategyRangeAsync(
                new WeChatWorkGetCustomerStrategyRangeRequest(0)));

        exception.Code.ShouldBe($"WeChatWork:40058");
        exception.Message.ShouldContain($"invalid strategy_id:0");
    }

    // TODO: 其他敏感接口自行实现测试
}
