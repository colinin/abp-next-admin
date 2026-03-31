using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Tokens;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Tokens;

[Dependency(ReplaceServices = true)]
public class TokenUsageStore : ITokenUsageStore, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ITokenUsageRecordRepository _tokenUsageRecordRepository;

    public TokenUsageStore(
        ICurrentTenant currentTenant, 
        IGuidGenerator guidGenerator, 
        ITokenUsageRecordRepository tokenUsageRecordRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _tokenUsageRecordRepository = tokenUsageRecordRepository;
    }

    public async virtual Task SaveTokenUsageAsync(TokenUsageInfo tokenUsageInfo)
    {
        var tokenUsageRecord = await _tokenUsageRecordRepository.FindByMessageIdAsync(
            tokenUsageInfo.ConversationId,
            tokenUsageInfo.MessageId);

        if (tokenUsageRecord == null)
        {
            tokenUsageRecord = new TokenUsageRecord(
                _guidGenerator.Create(),
                tokenUsageInfo.MessageId,
                tokenUsageInfo.ConversationId,
                tokenUsageInfo.InputTokenCount,
                tokenUsageInfo.OutputTokenCount,
                tokenUsageInfo.TotalTokenCount,
                tokenUsageInfo.CachedInputTokenCount,
                tokenUsageInfo.ReasoningTokenCount,
                _currentTenant.Id);

            await _tokenUsageRecordRepository.InsertAsync(tokenUsageRecord);
        }
        else
        {
            tokenUsageRecord.InputTokenCount ??= 0;
            tokenUsageRecord.InputTokenCount += tokenUsageInfo.InputTokenCount;

            tokenUsageRecord.OutputTokenCount ??= 0;
            tokenUsageRecord.OutputTokenCount += tokenUsageInfo.OutputTokenCount;

            tokenUsageRecord.CachedInputTokenCount ??= 0;
            tokenUsageRecord.CachedInputTokenCount += tokenUsageInfo.CachedInputTokenCount;

            tokenUsageRecord.ReasoningTokenCount ??= 0;
            tokenUsageRecord.ReasoningTokenCount += tokenUsageInfo.ReasoningTokenCount;

            tokenUsageRecord.TotalTokenCount ??= 0;
            tokenUsageRecord.TotalTokenCount += tokenUsageInfo.TotalTokenCount;

            await _tokenUsageRecordRepository.UpdateAsync(tokenUsageRecord);
        }
    }
}
