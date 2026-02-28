using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers;
public class WeChatWorkCustomerProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkCustomerProvider _provider;
    public readonly IConfiguration _configuration;
    public WeChatWorkCustomerProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkCustomerProvider>();
        _configuration = GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async virtual Task Should_Get_Customer_List()
    {
        var userId = _configuration["WeChat:Work:ExternalContact:Customers:GetCustomerList:UserId"];
        var res = await _provider.GetCustomerListAsync(userId);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Bulk_Get_Customer()
    {
        var userId = _configuration["WeChat:Work:ExternalContact:Customers:BulkGetCustomer:UserId"];
        var res = await _provider.BulkGetCustomerAsync(
            new WeChatWorkBulkGetCustomerRequest(
                new List<string> { userId }));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Get_Customer()
    {
        var externalUserid = _configuration["WeChat:Work:ExternalContact:Customers:GetCustomer:ExternalUserid"];
        var res = await _provider.GetCustomerAsync(externalUserid);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Update_Customer_Remark()
    {
        var userId = _configuration["WeChat:Work:ExternalContact:Customers:UpdateCustomerRemark:UserId"];
        var externalUserid = _configuration["WeChat:Work:ExternalContact:Customers:UpdateCustomerRemark:ExternalUserid"];
        var res = await _provider.UpdateCustomerRemarkAsync(
            new WeChatWorkUpdateCustomerRemarkRequest(
                userId,
                externalUserid));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}
