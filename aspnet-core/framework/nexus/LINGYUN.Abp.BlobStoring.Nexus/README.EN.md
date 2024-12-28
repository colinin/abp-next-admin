# LINGYUN.Abp.BlobStoring.Nexus

[简体中文](./README.md) | English

A BLOB storage provider based on Sonatype Nexus.

## Features

* Support for storing files in Nexus repository
* Support for basic file operations including upload, download, and delete
* Support for file path normalization
* Support for file duplication check and override options
* Multi-tenancy support

## Module Dependencies

```csharp
[DependsOn(typeof(AbpBlobStoringNexusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

### 1. Configure Nexus Storage

Configure Nexus storage options in `appsettings.json`:

```json
{
  "BlobStoring": {
    "Nexus": {
      "Repository": "your-repository-name"
    }
  }
}
```

### 2. Configure Container

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

### 3. Use BLOB Storage

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

## Configuration Options

* Repository - Nexus repository name

## Best Practices

1. Plan file paths and naming properly for better management and maintenance
2. Configure file override options based on actual requirements
3. Use multi-tenancy features appropriately to ensure data isolation

## Notes

1. Ensure Nexus repository is properly configured and accessible
2. File paths are automatically normalized, replacing backslashes with forward slashes
3. By default, overwriting existing files is not allowed, the OverrideExisting option needs to be explicitly set
4. File operations may be affected by network conditions, consider adding appropriate error handling mechanisms
