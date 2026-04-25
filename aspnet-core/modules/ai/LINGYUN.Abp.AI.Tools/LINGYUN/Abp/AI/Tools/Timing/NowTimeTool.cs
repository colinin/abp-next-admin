using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AI.Tools.Timing;
public class NowTimeTool : ITransientDependency
{
    public const string Name = "GetNowTime";

    private readonly IClock _clock;
    public NowTimeTool(IClock clock)
    {
        _clock = clock;
    }

    public object? Invoke()
    {
        return _clock.Now;
    }
}
