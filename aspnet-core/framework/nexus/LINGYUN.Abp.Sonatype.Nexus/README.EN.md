# LINGYUN.Abp.Sonatype.Nexus

[简体中文](./README.md) | English

ABP framework integration module for Sonatype Nexus REST API.

## Features

* Support for Nexus repository management
* Support for component management
* Support for asset management
* Support for search functionality
* Support for basic authentication

## Module Dependencies

```csharp
[DependsOn(typeof(AbpSonatypeNexusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

### 1. Configure Nexus Connection

Configure Nexus connection options in `appsettings.json`:

```json
{
  "SonatypeNexus": {
    "BaseUrl": "http://127.0.0.1:8081",
    "UserName": "sonatype",
    "Password": "sonatype"
  }
}
```

### 2. Use Repository Management

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

### 3. Use Component Management

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

### 4. Use Asset Management

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

## Configuration Options

* BaseUrl - Nexus server address
* UserName - Username
* Password - Password

## Best Practices

1. Keep authentication information secure
2. Plan repository structure and component classification properly
3. Regularly clean up unnecessary components and assets
4. Use appropriate error handling mechanisms

## Notes

1. Ensure Nexus server is properly configured and accessible
2. Consider network conditions and timeout settings when uploading large files
3. Delete operations are irreversible, proceed with caution
4. Some operations require administrator privileges
