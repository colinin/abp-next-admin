namespace LINGYUN.Platform
{
    public static class PlatformConsts
    {
        /// <summary>
        /// 编号不足位补足字符，如编号为 3，长度5位，补足字符为 0，则编号为00003
        /// </summary>
        public static char CodePrefix { get; set; } = '0';
        /// <summary>
        /// 编号长度
        /// </summary>
        public const int CodeUnitLength = 5;
    }
}
