namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public class AIToolProviderDto
{
    public string Name { get; }
    public AIToolPropertyDescriptorDto[] Properties { get; }
    public AIToolProviderDto(string name, AIToolPropertyDescriptorDto[] properties)
    {
        Name = name;
        Properties = properties;
    }
}
