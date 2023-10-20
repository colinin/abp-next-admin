using System;

namespace LINGYUN.Abp.Idempotent;

[AttributeUsage(AttributeTargets.Method)]
public class IgnoreIdempotentAttribute : Attribute
{
}
