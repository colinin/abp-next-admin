# LINGYUN.Abp.Elasticsearch

Abp Elasticsearch集成,提供全局唯一IElasticClient访问接口

## 模块引用


```csharp
[DependsOn(typeof(AbpElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*	AbpElasticsearchOptions.FieldCamelCase      字段是否采用 camelCase 格式, 默认false
*	AbpElasticsearchOptions.NodeUris            ES端点,多个端点以,或;分隔
*	AbpElasticsearchOptions.TypeName            文档名称,默认_doc
*	AbpElasticsearchOptions.ConnectionLimit     最大连接数,详情见 NEST 文档
*	AbpElasticsearchOptions.UserName            连接用户,详情见 NEST 文档
*	AbpElasticsearchOptions.Password            用户密码,详情见 NEST 文档
*	AbpElasticsearchOptions.ConnectionTimeout   连接超时时间,详情见 NEST 文档

## appsettings.json

```json
{
  "Elasticsearch": {
    "NodeUris": "http://localhost:9200"
  }
}

```