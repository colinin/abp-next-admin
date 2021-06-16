using JetBrains.Annotations;
using System;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    internal static class Check
    {
        public static int Range(
            int value,
            [InvokerParameterName][NotNull] string parameterName,
            int minimum = int.MinValue,
            int maximum = int.MaxValue)
        {
            if (value < minimum)
            {
                throw new ArgumentException($"{parameterName} must be equal to or lower than {minimum}!", parameterName);
            }
            if (value > maximum)
            {
                throw new ArgumentException($"{parameterName} must be equal to or greater than {maximum}!", parameterName);
            }
            return value;
        }
    }
}
