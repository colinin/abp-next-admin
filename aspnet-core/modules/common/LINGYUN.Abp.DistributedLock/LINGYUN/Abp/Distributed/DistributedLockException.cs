using LINGYUN.Abp.ExceptionHandling;
using System;

namespace LINGYUN.Abp.Distributed
{
    public class DistributedLockException : Exception, IHasNotifierErrorMessage
    {
        public DistributedLockException(string message)
            : base(message)
        {

        }
    }
}
