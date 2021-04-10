using System.Collections.Generic;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class DaprActorConfiguration : Dictionary<string, string>
    {
        /// <summary>
        /// Base ActorId.
        /// </summary>
        public string ActorId
        {
            get => this.GetOrDefault(nameof(ActorId));
            set => this[nameof(ActorId)] = value;
        }

        /// <summary>
        /// Base Url.
        /// </summary>
        public string BaseUrl
        {
            get => this.GetOrDefault(nameof(BaseUrl));
            set => this[nameof(BaseUrl)] = value;
        }

        public DaprActorConfiguration()
        {
        }

        public DaprActorConfiguration(
            string actorId,
            string baseUrl)
        {
            this[nameof(ActorId)] = actorId;
            this[nameof(BaseUrl)] = baseUrl;
        }
    }
}
