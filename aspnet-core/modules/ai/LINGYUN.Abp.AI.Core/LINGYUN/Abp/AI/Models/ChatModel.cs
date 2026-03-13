namespace LINGYUN.Abp.AI.Models;

public class ChatModel
{
    public string Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public ChatModel(string id, string name, string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
