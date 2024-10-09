# LINGYUN.Abp.Exporter.MagicodesIE.Excel

数据导出接口的 [Magicodes.IE.Excel](https://github.com/dotnetcore/Magicodes.IE) 实现

Import and export general library, support Dto import and export, template export, fancy export and dynamic export, support Excel, Csv, Word, Pdf and Html.

详情请参考 [Magicodes.IE.Excel 文档](https://github.com/dotnetcore/Magicodes.IE/blob/master/docs/2.%E5%9F%BA%E7%A1%80%E6%95%99%E7%A8%8B%E4%B9%8B%E5%AF%BC%E5%87%BAExcel.md)

## 配置使用


```csharp

    [DependsOn(
        typeof(AbpExporterMagicodesIEExcelModule)
        )]
    public class YouProjectModule : AbpModule
    {
        Configure<AbpExporterMagicodesIEExcelOptions>(options =>
        {
            // 配置类型数据导出
            options.MapExportSetting(typeof(DemoClass), config =>
            {
                // 表头位置从第二行开始
                config.HeaderRowIndex = 2;
                // 其他配置
            });
        });
    }

    public class DemoClass
    {
        public string Name { get; set; }
        public string Remarks { get; set; }

        public DemoClass()
        {
        }
    }
```

