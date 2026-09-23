# LINGYUN.Abp.Elasticsearch.EsqlQuery

[IExpressionQueryService](../LINGYUN.Abp.Elasticsearch/LINGYUN/Abp/Elasticsearch/IExpressionQueryService.cs) 的ES|QL查询集成.  

**注意**: 
* 使用 ES|QL 查询方案不能进行深度分页,数据条目超出10000时依旧存在ES分页限制.  
如需支持深度分页,请使用默认  [ExpressionQueryService](../LINGYUN.Abp.Elasticsearch/LINGYUN/Abp/Elasticsearch/ExpressionQueryService.cs) `search_after` 方案

* 不支持使用 由于ES|QL不支持 `Skip`, 暂不支持分页查询  

* 对于嵌套类型的集合方法测试未通过,请勿使用集合的过滤条件  


## 模块引用

```csharp
[DependsOn(typeof(AbpElasticsearchEsqlQueryModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
