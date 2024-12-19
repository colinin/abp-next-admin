# LINGYUN.Abp.OssManagement.SettingManagement

对象存储管理设置管理模块

## 功能

* 提供对象存储管理的设置管理功能
* 实现设置的读取和修改
* 支持多租户配置
* 支持不同级别的设置管理

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementSettingManagementModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 设置项

### 基础设置
* DownloadPackageSize：下载包大小
* FileLimitLength：文件大小限制
* AllowFileExtensions：允许的文件扩展名

### API接口

* GET /api/oss-management/settings：获取设置
* PUT /api/oss-management/settings：更新设置

### 权限

* AbpOssManagement.Setting：设置管理权限

## 注意事项

* 需要具有相应的权限才能访问设置
* 某些设置可能需要重启应用才能生效
* 建议在修改设置前备份当前配置

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
