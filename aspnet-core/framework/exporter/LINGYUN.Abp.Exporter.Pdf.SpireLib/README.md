# LINGYUN.Abp.Exporter.Pdf.SpireLib

> Spire.XLS for .NET 是一款专业的 .NET Excel 组件， 它可以用在各种 .NET 框架中，包括 .NET Core、.NET 5.0、.NET 6.0、.NET 7.0、.NET Standard、 Xamarin、Mono Android、ASP.NET 和 Windows Forms 等相关的 .NET 应用程序。Spire.XLS for .NET 提供了一个对象模型 Excel API，使开发人员可以快速地在 .NET 平台上完成对 Excel 的各种编程操作，如根据模板创建新的 Excel 文档，编辑现有 Excel 文档以及对 Excel 文档进行转换。  

此模块使用 [Spire.XLS](https://www.e-iceblue.cn/spirexls/spire-xls-for-net-program-guide-content.html) 实现将Excel转换为Pdf,请在使用前配置许可.  

## 配置使用


```csharp

    [DependsOn(
        typeof(AbpExporterPdfModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // 配置许可
            Spire.Xls.License.LicenseProvider.SetLicense("xxx");
        }
    }
```


