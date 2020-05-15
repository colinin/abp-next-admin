namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupAppIdsDto
    {
        public string AppId { get; }
        public string AppName { get; }
        public RouteGroupAppIdsDto(string appId, string appName)
        {
            AppId = appId;
            AppName = appName;
        }
    }
}
