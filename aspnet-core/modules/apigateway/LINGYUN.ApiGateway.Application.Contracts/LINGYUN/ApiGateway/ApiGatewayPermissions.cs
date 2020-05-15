namespace LINGYUN.ApiGateway
{
    public static class ApiGatewayPermissions
    {
        public const string GroupName = "ApiGateway";

        public static class RouteGroup
        {
            public const string Default = GroupName + ".RouteGroup";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
        }
        public static class Global
        {
            public const string Default = GroupName + ".Global";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
        }

        public static class Route
        {
            public const string Default = GroupName + ".Route";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
        }

        public static class DynamicRoute
        {
            public const string Default = GroupName + ".DynamicRoute";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
        }

        public static class AggregateRoute
        {
            public const string Default = GroupName + ".AggregateRoute";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
        }
    }
}
