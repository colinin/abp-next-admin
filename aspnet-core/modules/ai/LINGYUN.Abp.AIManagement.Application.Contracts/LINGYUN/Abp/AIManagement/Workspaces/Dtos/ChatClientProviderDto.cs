namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;

public class ChatClientProviderDto
{
    public string Name { get; }
    public string[] Models { get; }
    public ChatClientProviderDto(string name, string[] models)
    {
        Name = name;
        Models = models;
    }
}
