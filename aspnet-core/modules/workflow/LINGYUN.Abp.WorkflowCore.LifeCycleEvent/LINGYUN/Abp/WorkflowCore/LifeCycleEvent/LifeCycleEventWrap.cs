namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
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
