namespace LINGYUN.Abp.DataProtection;
public interface IDataAccessKeywordContributor
{
    string Keyword { get; }
    object Contribute(DataAccessKeywordContributorContext context);
}
