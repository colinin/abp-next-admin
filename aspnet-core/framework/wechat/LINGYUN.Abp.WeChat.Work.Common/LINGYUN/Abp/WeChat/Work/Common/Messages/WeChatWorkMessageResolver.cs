using LINGYUN.Abp.WeChat.Common.Crypto;
using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
public class WeChatWorkMessageResolver : MessageResolverBase, IWeChatWorkMessageResolver
{
    private readonly AbpWeChatWorkMessageResolveOptions _options;
    public WeChatWorkMessageResolver(
        IWeChatCryptoService cryptoService, 
        IServiceProvider serviceProvider,
        IOptions<AbpWeChatWorkMessageResolveOptions> options) 
        : base(cryptoService, serviceProvider)
    {
        _options = options.Value;
    }

    protected async override Task<MessageResolveResult> ResolveMessageAsync(MessageResolveContext context)
    {
        var result = new MessageResolveResult(context.Origin);

        foreach (var messageResolver in _options.MessageResolvers)
        {
            await messageResolver.ResolveAsync(context);

            result.AppliedResolvers.Add(messageResolver.Name);

            if (context.HasResolvedMessage())
            {
                result.Message = context.Message;
                break;
            }
        }

        return result;
    }
}
