using System.Runtime.Serialization;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class AbpDaprActorCallException : AbpException, IHasErrorCode, IHasErrorDetails
    {
        public string Code => Error?.Code;

        public string Details => Error?.Details;

        public RemoteServiceErrorInfo Error { get; set; }

        public AbpDaprActorCallException()
        {

        }

        public AbpDaprActorCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public AbpDaprActorCallException(RemoteServiceErrorInfo error)
            : base(error.Message)
        {
            Error = error;

            if (error.Data != null)
            {
                foreach (var dataKey in error.Data.Keys)
                {
                    Data[dataKey] = error.Data[dataKey];
                }
            }
        }
    }
}
