using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using Volo.Abp;
using Volo.Abp.Logging;

namespace LINGYUN.Abp.Identity
{
    public class IdentityException : BusinessException, IExceptionWithSelfLogging
    {
        public IdentityException(
            SerializationInfo serializationInfo, 
            StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public IdentityException(
            string code = null, 
            string message = null, 
            string details = null, 
            Exception innerException = null, 
            LogLevel logLevel = LogLevel.Warning) 
            : base(code, message, details, innerException, logLevel)
        {
        }

        public virtual void Log(ILogger logger)
        {
            logger.Log(
                LogLevel,
                "An id error occurred,code: {0}, Message: {1}, Details: {2}",
                Code,
                Message,
                Details);
        }
    }
}
