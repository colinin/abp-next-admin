using System;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public class DownstreamHttpMethod : HttpMethod
    {
        public DownstreamHttpMethod(Guid id, string method) 
            : base(id, method)
        {

        }
    }
}
