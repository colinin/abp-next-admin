using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class LanguageInfoComparer : IEqualityComparer<LanguageInfo>
    {
        public bool Equals(LanguageInfo x, LanguageInfo y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.CultureName.Equals(y.CultureName);
        }

        public int GetHashCode(LanguageInfo obj)
        {
            return obj?.CultureName.GetHashCode() ?? GetHashCode();
        }
    }
}
