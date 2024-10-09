# LINGYUN.Abp.Exporter.MiniExcel

数据导出接口的 [MiniExcel](https://github.com/mini-software/MiniExcel) 实现

MiniExcel简单、高效避免OOM的.NET处理Excel查、写、填充数据工具。

目前主流框架大多需要将数据全载入到内存方便操作，但这会导致内存消耗问题，MiniExcel 尝试以 Stream 角度写底层算法逻辑，能让原本1000多MB占用降低到几MB，避免内存不够情况。

详情请参考 [MiniExcel 文档](https://github.com/mini-software/MiniExcel/blob/master/README.md)

## 配置使用


```csharp

    [DependsOn(
        typeof(AbpExporterMiniExcelModule)
        )]
    public class YouProjectModule : AbpModule
    {
        Configure<AbpExporterMiniExcelOptions>(options =>
        {
            // 配置类型数据导出
            options.MapExportSetting(typeof(DemoClass), config =>
            {
                config.DynamicColumns = new[]
                {
                    // 忽略Name字段
                    new DynamicExcelColumn(nameof(DemoClass.Name)){ Ignore = true },
                    // 其他配置
                };
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

