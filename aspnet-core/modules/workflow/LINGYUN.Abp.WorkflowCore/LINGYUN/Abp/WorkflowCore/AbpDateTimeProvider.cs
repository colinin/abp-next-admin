using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    [Dependency(ReplaceServices = true)]
    public class AbpDateTimeProvider : IDateTimeProvider
    {
        private readonly IClock _clock;

        public AbpDateTimeProvider(IClock clock)
        {
            _clock = clock;
        }

        public DateTime Now => _clock.Now;

        public DateTime UtcNow => _clock.Now.ToUtcDateTime();
    }
}
