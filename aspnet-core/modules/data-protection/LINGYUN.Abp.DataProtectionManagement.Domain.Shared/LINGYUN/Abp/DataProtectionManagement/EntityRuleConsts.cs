﻿namespace LINGYUN.Abp.DataProtectionManagement;
public static class EntityRuleConsts
{
    public static int MaxEntityTypeFullNameLength { get; set; } = EntityPropertyInfoConsts.MaxTypeFullNameLength;
    public static int MaxAccessedPropertiesLength { get; set; } = 512;
}
