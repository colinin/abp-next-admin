using IP2Region.Net.XDB;

namespace LINGYUN.Abp.IP2Region;

/// <summary>
/// IP2Region 选项
/// </summary>
public class AbpIP2RegionOptions
{
    /// <summary>
    /// IPv4 数据库文件
    /// </summary>
    /// <remarks>
    /// 支持物理文件路径与虚拟文件系统路径(默认使用内置的 ip2region_v4.xdb 资源):
    /// * 传入物理文件路径时优先按物理文件加载, 便于替换/更新离线库;
    /// * 否则从虚拟文件系统读取内置资源。
    /// </remarks>
    public string IPv4DatabaseFile { get; set; } = AbpIP2RegionModule.DefaultIPv4DatabaseFile;

    /// <summary>
    /// IPv6 数据库文件
    /// </summary>
    /// <remarks>
    /// 与 <see cref="IPv4DatabaseFile"/> 一致(物理文件优先);
    /// IPv6 离线库可选, 文件不存在时 IPv6 查询返回 null。
    /// </remarks>
    public string IPv6DatabaseFile { get; set; } = AbpIP2RegionModule.DefaultIPv6DatabaseFile;

    /// <summary>
    /// 缓存策略
    /// </summary>
    /// <remarks>
    /// 默认 <see cref="CachePolicy.Content"/>: 将离线库全部加载到内存后再查询, 是**并发安全**的策略
    /// (IPv4 库约 10MB、IPv6 库约 35MB)。
    /// 
    /// <see cref="CachePolicy.File"/> 与 <see cref="CachePolicy.VectorIndex"/> 基于"共享数据流 + Seek"实现,
    /// 而 <c>ISearcher</c> 注册为单例, 多线程并发查询会互相打断数据流定位,
    /// 仅在能够保证串行查询(或自行保证线程安全)时使用。
    /// </remarks>
    public CachePolicy CachePolicy { get; set; } = CachePolicy.Content;
}
