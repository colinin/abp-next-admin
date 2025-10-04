namespace LINGYUN.Abp.PermissionManagement;
public class PermissionChangeState
{
    public string Name { get; }
    public bool IsGranted { get; }
    public PermissionChangeState(string name, bool isGranted)
    {
        Name = name;
        IsGranted = isGranted;
    }
}
