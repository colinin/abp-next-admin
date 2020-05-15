using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class Headers : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }
        public virtual string Key { get; private set; }
        public virtual string Value { get; private set; }
      
        protected Headers()
        {

        }
        public Headers(long rerouteId)
        {
            ReRouteId = rerouteId;
        }

        public void SetHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
