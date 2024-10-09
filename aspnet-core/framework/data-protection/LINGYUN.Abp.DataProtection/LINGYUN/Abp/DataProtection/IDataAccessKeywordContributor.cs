using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection;
public interface IDataAccessKeywordContributor
{
    string Keyword { get; }
    Expression Contribute(DataAccessKeywordContributorContext context);
}
