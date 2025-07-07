# LINGYUN.Abp.Exporter.Pdf

Pdf导出模块

## 配置使用


```csharp

    [DependsOn(
        typeof(AbpExporterPdfModule)
        )]
    public class YouProjectModule : AbpModule
    {

    }
```

> 导出数据
```csharp
    public class ExportDemoClass
    {
        private readonly IExcelToPdfProvider _exporterProvider;

        public ExportDemoClass(IExcelToPdfProvider exporterProvider)
        {
            _exporterProvider = exporterProvider;
        }

        public async virtual Task<IRemoteStreamContent> ExportAsync(Stream excelStream)
        {
            var stream = await _exporterProvider.ParseAsync(excelStream);

            return new RemoteStreamContent(stream, "demo.pdf");
        }
    }
```

