using System.Threading.Tasks;

namespace LINGYUN.Abp.Localization.Dynamic
{
    internal interface ILocalizationDispatcher
    {
        /// <summary>
        /// 发布变更事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task DispatchAsync(LocalizedStringCacheResetEventData data);
    }
}
