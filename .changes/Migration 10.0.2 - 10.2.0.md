# 10.0.2 - 10.2.0

## 模块变更

1. [权限管理模块]   abp官方新增 [ConfigurationFeatureValueProvider](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Features/Volo/Abp/Features/ConfigurationFeatureValueProvider.cs), 此提供者使用简称为 `C` 的功能提供者, 与本项目中客户端功能提供者 `ClientFeatureValueProvider` 重名, 因此 `ClientFeatureValueProvider`简称将变更为 `CT`

   > *需要手动执行数据库语句以适配此变更：*
   ```postgresql
   update "AbpFeatureValues" afv set "ProviderName" = 'CT' where afv."ProviderName" = 'C'
   ```

2. [权限管理模块]   增加 [资源权限](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.Authorization/Volo/Abp/Authorization/Permissions/Resources) 自定义接口. 

3. [缓存管理模块]   增加批量删除缓存功能.

4. [缓存管理模块]   修复模糊查询时数据为空问题.  

5. [缓存管理模块]   增加缓存管理页面.  

6. [多语言管理模块] 修复动态本地化

7. [权限管理模块]   增加组织机构资源权限提供者 `OrganizationUnitResourcePermissionValueProvider`

8. [对象存储模块]   重构领域层, 增加持久化层, 存储对象引用、私有文件访问权限, 实现层通过对象引用访问Oss

9. [单体应用服务]   重构数据种子, 解决部分与abp框架冲突种子数据, 造成新租户事件执行失败

10. [Elsa工作量模块] 由于Elsa2.x版本依赖Swashbuckle 6.x版本, 而abp依赖10.x版本, 暂时在Swagger配置中移除Elsa相关接口

11. [EFCoreMySql]    由于 `Pomelo.EntityFrameworkCore.MySql` 太长时间未适配.NET 10, 使用 `Microting.EntityFrameworkCore.MySql` 替换, 详情见: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/2007

12. [单体应用服务]   新增缺失的MySql数据库迁移脚本, 由于MySql InnoDB 引擎对索引有长度限制, abp 资源权限索引需要限制长度, 暂时移除资源名称加入索引中

13. [解决方案变更]   解决方案文件从 `sln` 格式变更为 `slnx` 格式  

14. [数据导出模块]   增加 `IExcelExporterProvider`、`IExcelImporterProvider`、`IWordExporterProvider` 接口, 原 `IExporterProvider`、`IImporterProvider` 标记为弃用  

15. [数据导出模块]   增加 `AbpExporterMiniSoftwareModule` 模块, 集成 `MiniExcel`、`MiniWord` 实现 `IExcelExporterProvider`、`IExcelImporterProvider`、`IWordExporterProvider` 接口  


## 依赖项变更  

| 库名称                                       | 原版本    | 现版本 |
| -------------------------------------------- | --------- | ------ |
| Volo.Abp.*                                   | 10.0.2    | 10.2.0 |
| Elsa2                                        | 2.16.0    | 2.16.1 |
| Elsa3                                        | 3.3.5     | 3.6.0  |
| Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite | 5.0.2     | 5.2.0  |
| Microsoft.Bcl.AsyncInterfaces                | 10.0.0    | 10.0.2 |
| MySql.EntityFrameworkCore                    | 10.0.0-rc | 10.0.1 |
| Microting.EntityFrameworkCore.MySql          |           | 10.0.3 |
| Serilog.Extensions.Logging                   | 9.0.1     | 9.0.2  |
| DocumentFormat.OpenXml                       |           | 3.5.1  |
| MiniExcel                                    | 1.42.0    | 1.43.0 |
| MiniWord                                     |           | 0.9.2  |
| Scriban                                      | 6.3.0     | 7.0.0  |
| Swashbuckle.AspNetCore                       | 9.0.4     | 10.0.1 |



## 数据库迁移

| Product                                    | Scripts     | ProductVersion |
| ------------------------------------------ | --------- | ------ |
| AdminService                                 | 20260403034826_Upgrade-Abp-Framework-To-10-2-0    | 10.2.0 |
| Elsa2                                      | 2.16.0    | 2.16.1 |
| Elsa3                                      | 3.3.5     | 3.6.0  |