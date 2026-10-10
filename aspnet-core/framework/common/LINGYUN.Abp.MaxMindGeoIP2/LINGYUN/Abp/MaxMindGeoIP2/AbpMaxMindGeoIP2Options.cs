using MaxMind.Db;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// MaxMind GeoIP2 选项
/// </summary>
public class AbpMaxMindGeoIP2Options
{
    /// <summary>
    /// 数据库文件
    /// </summary>
    /// <remarks>
    /// 支持物理文件路径与虚拟文件系统路径(默认使用内置的 GeoLite2-City.mmdb 资源):
    /// * 传入物理文件路径时按 <see cref="FileAccessMode"/> 打开, 便于替换/更新离线库;
    /// * 否则从虚拟文件系统读取内置资源(始终加载到内存)。
    /// GeoLite2 离线库需要定期更新, 建议生产环境配置为物理文件路径。
    /// </remarks>
    public string DatabaseFile { get; set; } = AbpMaxMindGeoIP2Module.DefaultDatabaseFile;

    /// <summary>
    /// 物理数据库文件的访问模式
    /// </summary>
    /// <remarks>
    /// 默认 <see cref="FileAccessMode.MemoryMapped"/>(内存映射), 查询器为单例且线程安全
    /// </remarks>
    public FileAccessMode FileAccessMode { get; set; } = FileAccessMode.MemoryMapped;
}
