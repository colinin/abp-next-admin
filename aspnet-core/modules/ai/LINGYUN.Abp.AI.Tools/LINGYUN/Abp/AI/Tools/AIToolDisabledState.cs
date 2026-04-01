namespace LINGYUN.Abp.AI.Tools;
public class AIToolDisabledState
{
    public bool IsDisabled { get; private set; }

    public AIToolDisabledState(bool isDisabled)
    {
        IsDisabled = isDisabled;
    }
}
