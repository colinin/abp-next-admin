using LINGYUN.Abp.WxPusher.Features;
using LINGYUN.Abp.WxPusher.Token;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WxPusher.User;

[RequiresFeature(WxPusherFeatureNames.Enable)]
public class WxPusherUserProvider : WxPusherRequestProvider, IWxPusherUserProvider
{
    protected IWxPusherTokenProvider WxPusherTokenProvider { get; }

    public WxPusherUserProvider(IWxPusherTokenProvider wxPusherTokenProvider)
    {
        WxPusherTokenProvider = wxPusherTokenProvider;
    }

    public async virtual Task<bool> DeleteUserAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var token = await WxPusherTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.GetWxPusherClient();

        var content = await client.DeleteUserAsync(
            token,
            id,
            cancellationToken);

        var response = JsonSerializer.Deserialize<WxPusherResult<string>>(content);

        return response.Success;
    }

    public async virtual Task<WxPusherPagedResult<UserProfile>> GetUserListAsync(
        int page = 1,
        int pageSize = 10,
        string uid = null,
        bool? isBlock = null, 
        FlowType? type = null, 
        CancellationToken cancellationToken = default)
    {
        if (pageSize > 100)
        {
            throw new ArgumentException("pageSize must be equal to or lower than 100!", nameof(pageSize));
        }

        var token = await WxPusherTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.GetWxPusherClient();

        var content = await client.GetUserListAsync(
            token,
            page,
            pageSize,
            uid,
            isBlock,
            type,
            cancellationToken);

        var response = JsonSerializer
            .Deserialize<WxPusherResult<WxPusherPagedResult<UserProfile>>>(content);

        return response.GetData();
    }

    public async virtual Task<bool> RejectUserAsync(
        int id, 
        bool reject, 
        CancellationToken cancellationToken = default)
    {
        var token = await WxPusherTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.GetWxPusherClient();

        var content = await client.RejectUserAsync(
            token,
            id,
            reject,
            cancellationToken);

        var response = JsonSerializer.Deserialize<WxPusherResult<string>>(content);

        return response.Success;
    }
}
