using Volo.Abp.Content;

namespace LINGYUN.Abp.AI.Models;
public class MediaMessage
{
    public string Id { get; }
    public IRemoteStreamContent Content { get; }
    public MediaMessage(string id, IRemoteStreamContent content)
    {
        Id = id;
        Content = content;
    }
}
