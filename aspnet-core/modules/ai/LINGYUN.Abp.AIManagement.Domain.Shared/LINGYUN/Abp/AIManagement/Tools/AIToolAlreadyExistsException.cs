using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolAlreadyExistsException : BusinessException
{
    public string AITool { get; }
    public AIToolAlreadyExistsException(string aiTool)
        : base(
            AIManagementErrorCodes.AITool.NameAlreadyExists,
            $"A AITool named {aiTool} already exists!")
    {
        AITool = aiTool;

        WithData(nameof(AITool), aiTool);
    }
}
