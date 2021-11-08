using System;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    public static class ProtectedEntityHelper
    {
        public static void TrySetOwner(
            IDataProtection protectedEntity,
            Func<string> ownerFactory)
        {
            ObjectHelper.TrySetProperty(
                protectedEntity,
                x => x.Owner,
                ownerFactory,
                new Type[] { });
        }
    }
}
