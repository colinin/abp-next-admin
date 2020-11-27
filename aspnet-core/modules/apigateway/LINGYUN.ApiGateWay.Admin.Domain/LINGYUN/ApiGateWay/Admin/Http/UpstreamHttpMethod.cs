using System;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public class UpstreamHttpMethod : HttpMethod
    {
        public UpstreamHttpMethod(Guid id, string method) 
            : base(id, method)
        {

        }
    }
}
