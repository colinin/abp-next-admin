# LINGYUN.Abp.Sonatype.Nexus

[English](./README.EN.md) | 简体中文

Sonatype Nexus REST API 的 ABP 框架集成模块。

## 功能

* 支持 Nexus 仓库管理
* 支持组件（Component）管理
* 支持资源（Asset）管理
* 支持搜索功能
* 支持基本认证

## 模块依赖

```csharp
[DependsOn(typeof(AbpSonatypeNexusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

### 1. 配置 Nexus 连接

在 `appsettings.json` 中配置 Nexus 连接选项：

```json
{
  "SonatypeNexus": {
    "BaseUrl": "http://127.0.0.1:8081",
    "UserName": "sonatype",
    "Password": "sonatype"
  }
}
```

### 2. 使用仓库管理

```csharp
public class MyService
{
    private readonly INexusRepositoryManager _repositoryManager;

    public MyService(INexusRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<List<NexusRepository>> GetRepositoriesAsync()
    {
        var result = await _repositoryManager.ListAsync();
        return result.Items;
    }
}
```

### 3. 使用组件管理

```csharp
public class MyService
{
    private readonly INexusComponentManager _componentManager;

    public MyService(INexusComponentManager componentManager)
    {
        _componentManager = componentManager;
    }

    public async Task UploadComponentAsync(string repository, string directory, byte[] fileBytes)
    {
        var asset = new Asset("filename.txt", fileBytes);
        var args = new NexusRawBlobUploadArgs(repository, directory, asset);
        await _componentManager.UploadAsync(args);
    }
}
```

### 4. 使用资源管理

```csharp
public class MyService
{
    private readonly INexusAssetManager _assetManager;

    public MyService(INexusAssetManager assetManager)
    {
        _assetManager = assetManager;
    }

    public async Task<List<NexusAsset>> GetAssetsAsync(string repository)
    {
        var result = await _assetManager.ListAsync(repository);
        return result.Items;
    }
}
```

## 配置选项

* BaseUrl - Nexus 服务器地址
* UserName - 用户名
* Password - 密码

## 最佳实践

1. 妥善保管认证信息，避免泄露
2. 合理规划仓库结构和组件分类
3. 定期清理不需要的组件和资源
4. 使用适当的错误处理机制

## 注意事项

1. 确保 Nexus 服务器已正确配置并可访问
2. 上传大文件时需要注意网络状况和超时设置
3. 删除操作不可恢复，请谨慎操作
4. 部分操作需要管理员权限
