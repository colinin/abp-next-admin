using System.Collections.Generic;

namespace LINGYUN.Abp.Dapr.Client
{
    public class DaprRemoteServiceConfiguration : Dictionary<string, string>
    {
        /// <summary>
        /// Base AppId.
        /// </summary>
        public string AppId
        {
            get => this.GetOrDefault(nameof(AppId));
            set => this[nameof(AppId)] = value;
        }

        /// <summary>
        /// Version.
        /// </summary>
        public string Version
        {
            get => this.GetOrDefault(nameof(Version));
            set => this[nameof(Version)] = value;
        }

        public DaprRemoteServiceConfiguration()
        {
        }

        public DaprRemoteServiceConfiguration(
            string appId,
            string version)
        {
            this[nameof(AppId)] = appId;
            this[nameof(Version)] = version;
        }
    }
}
