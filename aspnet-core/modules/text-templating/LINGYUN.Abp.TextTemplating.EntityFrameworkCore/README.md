# LINGYUN.Abp.TextTemplating.EntityFrameworkCore

## 模块说明

文本模板 EntityFrameworkCore 模块，提供文本模板的数据访问实现。

### 基础模块

* LINGYUN.Abp.TextTemplating.Domain
* Volo.Abp.EntityFrameworkCore

### 功能定义

* 实现文本模板的仓储接口
  * EfCoreTextTemplateDefinitionRepository - 模板定义仓储实现
  * EfCoreTextTemplateContentRepository - 模板内容仓储实现
* 提供数据库上下文和配置
  * ITextTemplatingDbContext - 文本模板数据库上下文接口
  * TextTemplatingDbContext - 文本模板数据库上下文
  * TextTemplatingDbContextModelCreatingExtensions - 数据库模型配置扩展

### 数据库表

* AbpTextTemplateDefinitions - 模板定义表
  * Id - 主键
  * Name - 模板名称
  * DisplayName - 显示名称
  * IsLayout - 是否为布局模板
  * Layout - 布局名称
  * IsInlineLocalized - 是否内联本地化
  * DefaultCultureName - 默认文化名称
  * LocalizationResourceName - 本地化资源名称
  * RenderEngine - 渲染引擎
  * IsStatic - 是否为静态模板
* AbpTextTemplateContents - 模板内容表
  * Id - 主键
  * Name - 模板名称
  * CultureName - 文化名称
  * Content - 模板内容

### 如何使用

1. 添加 `AbpTextTemplatingEntityFrameworkCoreModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 配置数据库上下文

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, ITextTemplatingDbContext
{
    public DbSet<TextTemplateDefinition> TextTemplateDefinitions { get; set; }
    public DbSet<TextTemplateContent> TextTemplateContents { get; set; }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTextTemplating();
    }
}
```

[查看英文](README.EN.md)
