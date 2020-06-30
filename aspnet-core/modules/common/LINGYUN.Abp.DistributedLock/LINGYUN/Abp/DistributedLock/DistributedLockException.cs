using LINGYUN.Abp.ExceptionHandling;
using System;

namespace LINGYUN.Abp.DistributedLock
{
    public class DistributedLockException : Exception, IHasNotifierErrorMessage
    {
        public DistributedLockException(string message)
            : base(message)
        {

        }
    }
}
