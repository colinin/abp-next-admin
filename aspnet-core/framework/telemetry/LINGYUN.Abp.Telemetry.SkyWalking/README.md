# LINGYUN.Abp.Telemetry.SkyWalking

分布式追踪系统 `SkyWalking` 集成  

## 功能特性


## 模块引用

```csharp
[DependsOn(typeof(AbpTelemetrySkyWalkingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```shell

# 切换到主程序目录
cd my-host-project-path

# 安装 skyapm 命令行工具
dotnet tool install -g SkyAPM.DotNet.CLI

# 生成 SkyWalking 配置文件, localhost:11800 为你运行的SkyWalking实例暴露的Grpc端口
dotnet skyapm config auth_server localhost:11800
```

```csharp

public class YouProjectModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<SkyApmExtensions>(skyapm =>
        {
            skyapm.AddCap();

            // other...
        });
    }
}


```

## 更多文档

* [SkyWalking](https://skywalking.apache.org/)
* [SkyWalking .NET](https://github.com/SkyAPM/SkyAPM-dotnet)
