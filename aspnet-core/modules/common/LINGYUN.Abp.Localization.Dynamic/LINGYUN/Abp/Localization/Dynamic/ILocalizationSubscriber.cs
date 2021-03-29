using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Localization.Dynamic
{
    /// <summary>
    /// 本地化资源订阅
    /// </summary>
    internal interface ILocalizationSubscriber
    {
        /// <summary>
        /// 订阅变更事件
        /// </summary>
        /// <param name="func"></param>
        void Subscribe(Func<LocalizedStringCacheResetEventData, Task> func);
    }
}
