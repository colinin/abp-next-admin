# LINGYUN.Abp.BlobStoring.Nexus

[English](./README.EN.md) | 简体中文

基于 Sonatype Nexus 的 Blob 存储提供程序。

## 功能

* 支持将文件存储到 Nexus 仓库
* 支持文件的上传、下载、删除等基本操作
* 支持文件路径规范化
* 支持文件重复检查和覆盖选项
* 支持多租户

## 模块依赖

```csharp
[DependsOn(typeof(AbpBlobStoringNexusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

### 1. 配置 Nexus 存储

在 `appsettings.json` 中配置 Nexus 存储选项：

```json
{
  "BlobStoring": {
    "Nexus": {
      "Repository": "your-repository-name"
    }
  }
}
```

### 2. 配置容器

```csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseNexus(nexus =>
        {
            nexus.Repository = "your-repository-name";
        });
    });
});
```

### 3. 使用 BLOB 存储

```csharp
public class MyService
{
    private readonly IBlobContainer _blobContainer;

    public MyService(IBlobContainer blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveBlobAsync(byte[] bytes)
    {
        await _blobContainer.SaveAsync("my-blob-name", bytes);
    }

    public async Task<byte[]> GetBlobAsync()
    {
        return await _blobContainer.GetAllBytesAsync("my-blob-name");
    }
}
```

## 配置选项

* Repository - Nexus 仓库名称

## 最佳实践

1. 合理规划文件路径和命名，便于管理和维护
2. 根据实际需求配置文件覆盖选项
3. 合理使用多租户功能，确保数据隔离

## 注意事项

1. 确保 Nexus 仓库已正确配置并可访问
2. 文件路径会被自动规范化，将反斜杠替换为正斜杠
3. 默认情况下不允许覆盖已存在的文件，需要显式设置 OverrideExisting 选项
4. 文件操作可能会受到网络状况的影响，建议添加适当的错误处理机制
