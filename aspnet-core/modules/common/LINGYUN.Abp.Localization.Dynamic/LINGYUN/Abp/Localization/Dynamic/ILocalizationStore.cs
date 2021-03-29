using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public interface ILocalizationStore
    {
        /// <summary>
        /// 获取语言列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<LanguageInfo>> GetLanguageListAsync(
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 资源是否存在
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ResourceExistsAsync(
            string resourceName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取当前资源下的本地化字典
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Dictionary<string, ILocalizationDictionary>> GetLocalizationDictionaryAsync(
            string resourceName,
            CancellationToken cancellationToken = default);
    }
}
