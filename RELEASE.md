## 总览

# [2021-12-11]

* 升级 abp framework 5.0.0 RC-2

1、修改SettingManagement模块, 只返回指定提供者所有的配置;

2、工作流增加 Elasticsearch 持久层扩展;
  
3、修复SignalR序列化协议不一致引起的IM错误;

4、修复构建模块依赖路径引用错误;

5、可以在文件url中添加token解决身份认证问题;

6、因聚合网关导致的租户IsAvailable可能为空;

7、聚合网关使用本地配置文件。

# [2021-12-07]

* 升级 abp framework 5.0.0 RC-1

1、移除 **vueJs** 模块, 将 **vue-vben-admin** 导入 [vue-vben-admin](./apps/vue);

2、移除Identity模块api：ChangePassword;
  
3、移除分布式锁模块, 如有项目引用此模块, 请使用 **Volo.Abp.DistributedLocking**;

4、移除网关管理模块, 使用本地文件作为路由配置;

5、移除动态网关数据库脚本文件;

6、将 Profile 相关api从Identity模块移动到Account模块;

7、加入Fody统一配置ConfigureAwait;

8、使用Directory.Build.props统一管理导入版本;

# [2021-03-29]
1、增加动态本地化组件支持,用于在运行时替换本地化文本,需要实现 ILocalizationStore;
  
2、增加本地化文档管理项目,实现了 ILocalizationStore持久化本地资源到数据库;
  使用须知: 需要先定位到 [LocalizationManagement](./aspnet-core/services/localization/LINGYUN.Abp.LocalizationManagement.HttpApi.Host) 目录下  
  执行Ef迁移命令: **dotnet ef database update**

3、增加一个dotnet-compose配置文件[docker-compose.configuration.yml](./docker-compose.configuration.yml);

4、vueJs 增加本地化管理组件视图: views/localization-management;

5、vueJs 用户登录后的异步等待调用修改为通过store来调用abp模块初始化事件;

6、vueJs 修复了HttpProxy模块传递的params与data使用情况混淆的问题;

7、同步本地化管理组件的网关路由初始化配置。