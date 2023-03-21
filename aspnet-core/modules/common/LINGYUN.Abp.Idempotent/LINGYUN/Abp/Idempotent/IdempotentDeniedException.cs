using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.Idempotent;
public class IdempotentDeniedException : BusinessException, IHasHttpStatusCode
{
    public string IdempotentKey { get; }

    public int HttpStatusCode { get; set; }

    public IdempotentDeniedException(
        string idempotentKey,
        string? code = null, 
        string? message = null, 
        string? details = null, 
        Exception? innerException = null, 
        LogLevel logLevel = LogLevel.Warning)
            : base(code, message, details, innerException, logLevel)
    {
        IdempotentKey = idempotentKey;
    }

    public IdempotentDeniedException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
}
