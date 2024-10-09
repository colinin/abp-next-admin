# LINGYUN.Abp.Exporter.Application.Contracts

数据导出/导入接口契约层,定义通用的数据导入/导入服务接口

## 配置使用


> 配置模块依赖
```csharp

[DependsOn(
    typeof(AbpExporterApplicationContractsModule)
    )]
public class YouProjectModule : AbpModule
{

}
```

> 定义导出类型
```csharp
public class DemoClassDto
{
    public string Name { get; set; }
    public string Remarks { get; set; }

    public DemoClassDto()
    {
    }
}
```

> 定义导入类型
```csharp
public class DemoClassImportInput
{
    [Required]
    public IRemoteStreamContent Content { get; set; }   

    public DemoClassImportInput()
    {
    }
}
```

> 定义数据过滤类型
```csharp
public class DemoClassExportListInput
{
    public string Filter { get; set; }

    [Required]
    public string FileName { get; set; }

    public DemoClassExportListInput()
    {
    }
}
```

> 导出数据
```csharp
    public class ExportDemoClassExportAppService : IExporterAppService<DemoClassDto, DemoClassExportListInput>
    {
        private readonly IExporterProvider _exporterProvider;

        public ExportDemoClassExportAppService(IExporterProvider exporterProvider)
        {
            _exporterProvider = exporterProvider;
        }

        public async virtual Task<IRemoteStreamContent> ExportAsync(DemoClassExportListInput input)
        {
            var dtos = 通过仓储接口查询数据列表;

            var stream = await _exporterProvider.ExportAsync(dtos);

            return new RemoteStreamContent(stream, input.FileName);
        }
    }
```

> 导入数据
```csharp
    public class ExportDemoClassImportAppService : IImporterAppService<DemoClassImportInput>
    {
        private readonly IImporterProvider _importerProvider;

        public ExportDemoClassImportAppService(IImporterProvider importerProvider)
        {
            _importerProvider = importerProvider;
        }

        public async virtual Task ImportAsync(DemoClassImportInput input)
        {
            var stream = input.Content.GetStream();

            var demos = await _importerProvider.ImportAsync<DemoClass>(stream);

            // 其他操作
        }
    }
```
