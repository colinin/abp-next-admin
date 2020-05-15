using System;

namespace LINGYUN.ApiGateway.EventBus
{
    public class ApigatewayConfigChangeCommand
    {
        public const string EventName = nameof(ApigatewayConfigChangeCommand);
        public DateTime DateTime { get; set; }
        public string Method { get; set; }
        public string Object { get; set; }
        protected ApigatewayConfigChangeCommand()
        {

        }

        public ApigatewayConfigChangeCommand(string @object, string @method)
        {
            DateTime = DateTime.Now;
            Object = @object;
            Method = @method;
        }
    }
}
