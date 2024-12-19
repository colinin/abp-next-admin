# LINGYUN.Platform.Settings.VueVbenAdmin

VueVbenAdmin前端框架的平台设置模块，提供了主题、布局、菜单等设置功能。

## 功能特性

* 主题设置
  * 暗黑模式
  * 灰色模式
  * 色弱模式
  * 主题颜色

* 布局设置
  * 全屏模式
  * 内容模式
  * 页面加载状态
  * 页脚显示
  * 返回顶部

* 菜单设置
  * 菜单模式
  * 菜单主题
  * 菜单宽度
  * 菜单折叠
  * 菜单分割
  * 菜单拖拽

* 标题栏设置
  * 固定标题栏
  * 标题栏主题
  * 全屏按钮
  * 文档按钮
  * 通知按钮
  * 搜索按钮

* 多标签页设置
  * 标签页缓存
  * 标签页拖拽
  * 标签页刷新
  * 标签页折叠

## 模块引用

```csharp
[DependsOn(typeof(PlatformSettingsVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 本地化资源

* 支持中文简体
* 支持英文

## 配置项

```json
{
  "Settings": {
    "DarkMode": false,
    "PageLoading": true,
    "PermissionCacheType": 1,
    "ShowSettingButton": true,
    "ShowDarkModeToggle": true,
    "SettingButtonPosition": "auto",
    "PermissionMode": "ROUTE_MAPPING",
    "SessionTimeoutProcessing": 0,
    "GrayMode": false,
    "ColorWeak": false,
    "ThemeColor": "#0960bd",
    "FullContent": false,
    "ContentMode": "full",
    "ShowLogo": true,
    "ShowFooter": true,
    "OpenKeepAlive": true,
    "LockTime": 0,
    "ShowBreadCrumb": true,
    "ShowBreadCrumbIcon": false
  }
}
```

## 更多

更多信息请参考 [Platform](../README.md)
