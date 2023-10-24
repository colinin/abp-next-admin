using LINGYUN.Abp.WxPusher.Features;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WxPusher.Messages;

[RequiresFeature(WxPusherFeatureNames.Enable)]
public class WxPusherMessageProvider : WxPusherRequestProvider, IWxPusherMessageProvider
{
    public async virtual Task<WxPusherResult<int>> QueryMessageAsync(
        int messageId,
        CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.GetWxPusherClient();

        var content = await client.QueryMessageAsync(
            messageId,
            cancellationToken);

        return JsonSerializer.Deserialize<WxPusherResult<int>>(content);
    }
}
