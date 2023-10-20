# Dynamic.Queryable

动态查询支持库, 为以后台服务提供动态查询支持  

* 应用程序接口定义层引用**LINGYUN.Abp.Dynamic.Queryable.Application.Contracts**模块, 实现**IDynamicQueryableAppService**接口可对外提供动态查询支持  

* 应用程序接口实现层引用**LINGYUN.Abp.Dynamic.Queryable.Application**模块，继承自**DynamicQueryableAppService**接口，实现**GetCountAsync**与**GetListAsync**方法即可(为仓储层接口提供传递的**Expression<Func<TEntity, bool>>**结构)  

* 控制器层引用**LINGYUN.Abp.Dynamic.Queryable.HttpApi**模块，继承**DynamicQueryableControllerBase**接口，自动公开动态查询接口。

## 接口说明

* **GetAvailableFieldsAsync**(<font color="green">[GET]</font> <font color="blue">/{controller}/available-fields</font>): 对外提供可选的属性明细, 默认返回所有字段(不包含已定义的排除字段).

* **GetListAsync**(<font color="orange">[POST]</font> <font color="blue">/{controller}/search</font>):    根据用户传递的动态条件返回查询结果列表.

## 配置使用

* AbpDynamicQueryableOptions.IgnoreFields:	定义不对用户公开的字段列表,默认排除租户标识、软删除过滤器接口字段.

