using MaxMind.Db;
using System;
using System.IO;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// MaxMind GeoIP2 选项
/// </summary>
public class AbpMaxMindGeoIP2Options
{
    /// <summary>
    /// GeoIP2 数据库文件(mmdb)的物理路径
    /// </summary>
    /// <remarks>
    /// 需要由使用方提供:
    /// * 本模块**不再内置** GeoLite2 离线库(GeoLite2 受 MaxMind 许可约束, 不得随包分发),
    ///   请自行从 MaxMind 下载并接受其许可协议(https://www.maxmind.com/en/geolite2/eula);
    /// * 通过配置节 <c>MaxMindGeoIP2:DatabaseFile</c>(或此处)指向解压后的 <c>GeoLite2-City.mmdb</c>;
    /// * 文件不存在时使用 <see cref="EmptyGeoIP2DatabaseReader"/>, 解析结果为空并记录警告日志。
    /// </remarks>
    public string DatabaseFile { get; set; } = string.Empty;

    /// <summary>
    /// 物理数据库文件的访问模式
    /// </summary>
    /// <remarks>
    /// 默认 <see cref="FileAccessMode.MemoryMapped"/>(内存映射), 查询器为单例且线程安全
    /// </remarks>
    public FileAccessMode FileAccessMode { get; set; } = FileAccessMode.MemoryMapped;

    /// <summary>
    /// 数据库文件是否存在
    /// </summary>
    public virtual bool DatabaseFileExists()
    {
        return !DatabaseFile.IsNullOrWhiteSpace() && File.Exists(DatabaseFile);
    }
}
