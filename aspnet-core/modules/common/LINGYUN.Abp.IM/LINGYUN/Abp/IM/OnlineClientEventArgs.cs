using System;

namespace LINGYUN.Abp.IM
{
    public class OnlineClientEventArgs : EventArgs
    {
        public IOnlineClient Client { get; }

        public OnlineClientEventArgs(IOnlineClient client)
        {
            Client = client;
        }
    }
}
