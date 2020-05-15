using DotNetCore.CAP.Internal;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.ApiGateway.Snowflake
{
    public class CapSnowflakeIdGenerator : ISnowflakeIdGenerator, ISingletonDependency
    {
        private readonly SnowflakeId _snowflakeId;
        public CapSnowflakeIdGenerator()
        {
            _snowflakeId = SnowflakeId.Default();
        }

        public long NextId()
        {
            return _snowflakeId.NextId();
        }
    }
}
