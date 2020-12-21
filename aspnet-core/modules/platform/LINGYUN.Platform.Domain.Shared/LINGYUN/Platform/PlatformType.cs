using System;

namespace LINGYUN.Platform
{
    /// <summary>
    /// 平台类型
    /// </summary>
    [Flags]
    public enum PlatformType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        None = 0,
        /// <summary>
        /// Windows CE
        /// </summary>
        WinCe = 2,
        /// <summary>
        /// Windows NT
        /// </summary>
        WinForm = 4,
        /// <summary>
        /// Windows桌面通用
        /// </summary>
        Desktop = WinCe | WinForm,
        /// <summary>
        /// WebForm
        /// </summary>
        WebForm = 8,
        /// <summary>
        /// MVC
        /// </summary>
        WebMvc = 16,
        /// <summary>
        /// 其他Mvvm架构
        /// </summary>
        WebMvvm = 32,
        /// <summary>
        /// Web通用
        /// </summary>
        Web = WebForm | WebMvc | WebMvvm,
        /// <summary>
        /// Android
        /// </summary>
        Android = 64,
        /// <summary>
        /// IOS
        /// </summary>
        iOS = 128,
        /// <summary>
        /// 移动端通用
        /// </summary>
        Mobile = Android | iOS,
        /// <summary>
        /// 小程序
        /// </summary>
        MiniProgram = 256,
        /// <summary>
        /// 所有平台通用
        /// </summary>
        All = Desktop | Web | Mobile | MiniProgram
    }
}
