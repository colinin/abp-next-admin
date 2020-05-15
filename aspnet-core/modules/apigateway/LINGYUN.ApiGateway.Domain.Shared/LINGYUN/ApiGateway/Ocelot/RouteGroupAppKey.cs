namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupAppKey
    {
        public string AppId { get; }
        public string AppName { get; }
        public RouteGroupAppKey(string appId, string appName)
        {
            AppId = appId;
            AppName = appName;
        }
    }
}
