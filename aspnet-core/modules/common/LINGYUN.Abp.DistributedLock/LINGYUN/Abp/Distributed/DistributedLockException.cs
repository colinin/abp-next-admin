using System;

namespace LINGYUN.Abp.Distributed
{
    public class DistributedLockException : Exception
    {
        public DistributedLockException(string message)
            : base(message)
        {

        }
    }
}
