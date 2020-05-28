using System.Collections.Generic;

namespace LINGYUN.Abp.IM
{
    public interface IOnlineClientStore
    {
        void Add(IOnlineClient client);

        bool Remove(string connectionId);

        bool TryRemove(string connectionId, out IOnlineClient client);

        bool TryGet(string connectionId, out IOnlineClient client);

        bool Contains(string connectionId);

        IReadOnlyList<IOnlineClient> GetAll();
    }
}
