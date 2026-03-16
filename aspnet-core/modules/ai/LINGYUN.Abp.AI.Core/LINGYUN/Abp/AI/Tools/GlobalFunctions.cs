using Microsoft.Extensions.AI;
using System;

namespace LINGYUN.Abp.AI.Tools;
public static class GlobalFunctions
{
    public static AITool Now => AIFunctionFactory.Create(
        () => DateTime.Now,
        nameof(Now),
        "Get now time");
}
