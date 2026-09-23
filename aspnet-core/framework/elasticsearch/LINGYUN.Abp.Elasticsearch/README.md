# LINGYUN.Abp.Elasticsearch

简体中文 | [English](./README.EN.md)

Abp Elasticsearch集成,提供全局唯一IElasticClient访问接口

## 模块引用

```csharp
[DependsOn(typeof(AbpElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 接口定义

*   [IIndexMappingProvider](./LINGYUN/Abp/Elasticsearch/IIndexMappingProvider.cs)  获取索引映射属性  
*   [IExpressionQueryTranslator](./LINGYUN/Abp/Elasticsearch/IExpressionQueryTranslator.cs)  表达式树翻译为ES查询类  
*   [IExpressionQueryService](./LINGYUN/Abp/Elasticsearch/IExpressionQueryService.cs)  表达式树数据查询  
*   [IElasticsearchClientFactory](./LINGYUN/Abp/Elasticsearch/IElasticsearchClientFactory.cs)  ES客户端管理  

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