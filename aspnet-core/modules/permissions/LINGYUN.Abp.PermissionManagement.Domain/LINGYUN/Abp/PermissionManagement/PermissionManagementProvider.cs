using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement
{
    public abstract class PermissionManagementProvider : IPermissionManagementProvider
    {
        public abstract string Name { get; }

        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected ICurrentTenant CurrentTenant { get; }
    }
}
