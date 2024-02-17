namespace LINGYUN.Abp.DataProtection;
public interface IHasDataAccess
{
    public DataAccessOwner Owner { get; }
}
