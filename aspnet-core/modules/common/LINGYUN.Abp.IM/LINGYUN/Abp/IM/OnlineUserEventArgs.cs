namespace LINGYUN.Abp.IM
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
