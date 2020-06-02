namespace LINGYUN.Abp.RealTime.Client
{
    public class OnlineUserEventArgs : OnlineClientEventArgs
    {
        public OnlineClientContext Context { get; }

        public OnlineUserEventArgs(OnlineClientContext context, IOnlineClient client)
            : base(client)
        {
            Context = context;
        }
    }
}
