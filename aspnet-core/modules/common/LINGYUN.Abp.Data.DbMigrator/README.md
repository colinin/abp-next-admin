# LINGYUN.Abp.Data.DbMigrator

数据迁移模块,用于配合需要提供数据初始化的场景（例如新租户创建时）  

## 配置使用


```csharp
[DependsOn(typeof(AbpDataDbMigratorModule))]
public class YouProjectModule : AbpModule
{
  // other
}

public class FakeDbContext : AbpDbContext<FakeDbContext>
{
	public FakeDbContext(DbContextOptions<FakeDbContext> options)
		: base(options)
    {
        
     }

	 protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
		base.OnModelCreating(modelBuilder);
     }
}

public class FakeMigretor 
{
	private readonly IDbSchemaMigrator _migrator;

	public FakeMigretor(
		IDbSchemaMigrator migrator)
	{
		_migrator = migrator;
	}

	public async Task MigrateAsync()
	{
		// 将执行 EF 迁移脚本, 需要提供用作数据迁移的 DbContext
		await _migrator.MigrateAsync<FakeDbContext>(
			// connectionString:	当前租户连接字符串,用于构建连接
			// builder:				用于初始化上下文的构造器
			(connectionString, builder) =>
			{

				// 在委托中由用户决定数据库参数
				builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

				// 返回实例化的DbContext
				return new FakeDbContext(builder.Options);
            });
	}
}
```
