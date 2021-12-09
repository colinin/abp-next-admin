using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
    [EventName("abp.workflowcore.life_cycle_event")]
    public class LifeCycleEventWrap
    {
        public string Data { get; set; }
        public LifeCycleEventWrap() { }
        public LifeCycleEventWrap(string data)
        {
            Data = data;
        }
    }
}
