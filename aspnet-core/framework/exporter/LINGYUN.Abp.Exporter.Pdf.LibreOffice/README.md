# LINGYUN.Abp.Exporter.Pdf.LibreOffice

> LibreOffice is Free and Open Source Software. Development is open to new talent and new ideas, and our software is tested and used daily by a large and devoted user community.  

此模块使用本地 `LibreOffice` 命令行实现将Excel转换为Pdf, 请引用此模块前确保已安装有 `LibreOffice`, 如未安装在默认目录, 请在使用前手动指定安装目录.  

## 配置使用


```csharp

    [DependsOn(
        typeof(AbpExporterPdfLibreOfficeModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // 手动指定安装目录
            LibreOfficeCommands.WindowsCliDir = "path\\to\\libreoffice";
            LibreOfficeCommands.UnixCliDir = "path/to/libreoffice";
        }
    }
```


