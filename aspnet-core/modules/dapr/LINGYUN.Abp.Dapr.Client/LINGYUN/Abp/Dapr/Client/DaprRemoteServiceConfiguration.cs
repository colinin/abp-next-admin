using System.Collections.Generic;
using Volo.Abp.Http.Client;

namespace LINGYUN.Abp.Dapr.Client
{
    public class DaprRemoteServiceConfiguration : RemoteServiceConfiguration
    {
        /// <summary>
        /// Base AppId.
        /// </summary>
        public string AppId
        {
            get => this.GetOrDefault(nameof(AppId));
            set => this[nameof(AppId)] = value;
        }

        public DaprRemoteServiceConfiguration()
        {
        }

        public DaprRemoteServiceConfiguration(
            string appId,
            string baseUil,
            string version)
            : base(baseUil, version)
        {
            this[nameof(AppId)] = appId;
        }
    }
}
