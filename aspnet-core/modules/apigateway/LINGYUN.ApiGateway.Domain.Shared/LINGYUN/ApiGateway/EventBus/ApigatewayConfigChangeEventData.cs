using System;

namespace LINGYUN.ApiGateway.EventBus
{
    public class ApigatewayConfigChangeEventData
    {
        public DateTime DateTime { get; set; }
        public string AppId { get; set; }
        public string Method { get; set; }
        public string Object { get; set; }
        protected ApigatewayConfigChangeEventData()
        {

        }

        public ApigatewayConfigChangeEventData(string appId, string @object, string @method)
        {
            AppId = appId;
            DateTime = DateTime.Now;
            Object = @object;
            Method = @method;
        }
    }
}
