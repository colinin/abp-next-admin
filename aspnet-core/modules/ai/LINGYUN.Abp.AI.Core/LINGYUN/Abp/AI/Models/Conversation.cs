using System;

namespace LINGYUN.Abp.AI.Models;
public class Conversation
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiredAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public Conversation(
        Guid id, 
        string name, 
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdateAt = createdAt;
    }

    public Conversation WithName(string name)
    {
        Name = name;

        return this;
    }
}
