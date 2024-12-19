# LINGYUN.Platform.Theme.VueVbenAdmin

VueVbenAdmin前端框架的主题管理模块，提供主题、布局、菜单等相关配置的管理功能。

## 功能特性

* 主题设置
  * 暗黑模式
  * 灰色模式
  * 色弱模式
  * 主题色配置

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
  * 菜单拆分
  * 菜单拖拽

* 标题栏设置
  * 固定头部
  * 头部主题
  * 全屏按钮
  * 文档按钮
  * 通知按钮
  * 搜索按钮

* 多标签页设置
  * 标签页缓存
  * 标签页拖拽
  * 标签页刷新
  * 标签页折叠

## 快速开始

1. 引用模块
```csharp
[DependsOn(typeof(PlatformThemeVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 配置选项
```json
{
  "Theme": {
    "DarkMode": false,
    "GrayMode": false,
    "ColorWeak": false,
    "ThemeColor": "#0960bd"
  },
  "Layout": {
    "FullContent": false,
    "ContentMode": "full",
    "ShowLogo": true,
    "ShowFooter": true,
    "ShowBreadCrumb": true,
    "ShowBreadCrumbIcon": false
  },
  "Menu": {
    "Mode": "inline",
    "Theme": "dark",
    "Width": 210,
    "Collapsed": false,
    "Split": false,
    "Draggable": true
  },
  "Header": {
    "Fixed": true,
    "Theme": "light",
    "ShowFullScreen": true,
    "ShowDoc": true,
    "ShowNotice": true,
    "ShowSearch": true
  },
  "MultiTab": {
    "Cache": true,
    "Draggable": true,
    "Refresh": true,
    "Fold": true
  }
}
```

## 使用说明

1. 主题配置
   * 支持多种主题模式切换
   * 可自定义主题色
   * 提供主题持久化存储

2. 布局配置
   * 灵活的布局模式
   * 可配置的页面元素显示
   * 支持响应式布局

3. 菜单配置
   * 多样的菜单显示模式
   * 支持菜单拖拽排序
   * 菜单主题自定义

4. 扩展开发
   * 支持自定义主题配置提供者
   * 可扩展的主题设置项
   * 主题数据本地化支持

## 注意事项

1. 主题配置
   * 主题配置会影响所有用户的界面显示
   * 建议根据实际需求选择合适的主题模式

2. 性能考虑
   * 合理配置标签页缓存数量
   * 适当使用页面加载动画

## 更多信息

* [VueVbenAdmin主题配置文档](https://doc.vvbin.cn/guide/design.html)
* [ABP官方文档](https://docs.abp.io/)
