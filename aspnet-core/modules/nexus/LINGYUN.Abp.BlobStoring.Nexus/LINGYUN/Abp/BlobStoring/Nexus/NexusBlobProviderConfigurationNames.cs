namespace LINGYUN.Abp.BlobStoring.Nexus;
public static class NexusBlobProviderConfigurationNames
{
    /// <summary>
    /// 基础路径
    /// </summary>
    public const string BasePath = "Sonatype:Nexus:Raw:BasePath";
    /// <summary>
    /// 添加容器名称到基础路径
    /// </summary>
    public const string AppendContainerNameToBasePath = "Sonatype:Nexus:Raw:AppendContainerNameToBasePath";
    /// <summary>
    /// Nexus raw仓库名称
    /// </summary>
    public const string Repository = "Sonatype:Nexus:Raw:Repository";
}
