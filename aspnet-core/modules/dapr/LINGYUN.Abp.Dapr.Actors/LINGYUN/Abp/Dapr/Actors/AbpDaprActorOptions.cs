namespace LINGYUN.Abp.Dapr.Actors
{
    public class AbpDaprActorOptions
    {
        public DaprActorConfigurationDictionary RemoteActors { get; set; }

        public AbpDaprActorOptions()
        {
            RemoteActors = new DaprActorConfigurationDictionary();
        }
    }
}
