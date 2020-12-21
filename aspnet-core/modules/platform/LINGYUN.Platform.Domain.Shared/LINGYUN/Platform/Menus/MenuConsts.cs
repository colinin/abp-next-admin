namespace LINGYUN.Platform.Menus
{
    public static class MenuConsts
    {
        public static int MaxComponentLength
        {
            get;
            set;
        } = 255;

        /// <summary>
        /// 最大深度
        /// </summary>
        /// <remarks>
        /// 默认为4,仅支持四级子菜单
        /// </remarks>
        public const int MaxDepth = 4;

        public const int MaxCodeLength = MaxDepth * (PlatformConsts.CodeUnitLength + 1) - 1;
    }
}
