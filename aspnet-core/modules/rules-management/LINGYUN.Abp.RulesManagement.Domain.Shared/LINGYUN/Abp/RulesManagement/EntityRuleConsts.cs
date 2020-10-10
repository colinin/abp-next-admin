namespace LINGYUN.Abp.RulesManagement
{
    public static class EntityRuleConsts
    {
        public static int MaxNameLength { get; set; } = 64;
        public static int MaxDisplayNameLength { get; set; } = 256;
        public static int MaxOperatorLength { get; set; } = 64;
        public static int MaxErrorMessageLength { get; set; } = 256;
        public static int MaxExpressionLength { get; set; } = 1024;
    }
}
