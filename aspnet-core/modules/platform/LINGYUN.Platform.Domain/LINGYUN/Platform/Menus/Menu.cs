using JetBrains.Annotations;
using LINGYUN.Platform.Routes;
using System;
using Volo.Abp;

namespace LINGYUN.Platform.Menus;

/// <summary>
/// 菜单
/// </summary>
public class Menu : Route
{
    /// <summary>
    /// 框架
    /// </summary>
    public virtual string Framework { get; set; }
    /// <summary>
    /// 菜单编号
    /// </summary>
    public virtual string Code { get; set; }
    /// <summary>
    /// 菜单布局页,Layout的路径
    /// </summary>
    public virtual string Component { get; set; }
    /// <summary>
    /// 所属的父菜单
    /// </summary>
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 所属布局标识
    /// </summary>
    public virtual Guid LayoutId { get; set; }
    /// <summary>
    /// 公共菜单
    /// </summary>
    public virtual bool IsPublic { get; set; }
    protected Menu() 
    {
    }

    public Menu(
        [NotNull] Guid id,
        [NotNull] Guid layoutId,
        [NotNull] string path, 
        [NotNull] string name,
        [NotNull] string code,
        [NotNull] string component,
        [NotNull] string displayName,
        [NotNull] string framework,
        string redirect = "",
        string description = "",
        Guid? parentId = null,
        Guid? tenantId = null) 
        : base(id, path, name, displayName, redirect, description, tenantId)
    {
        Check.NotNullOrWhiteSpace(code, nameof(code));

        LayoutId = layoutId;
        Code = code;
        Component = component;// 下属的一级菜单的Component应该是布局页, 例如vue-admin中的 component: Layout, 其他前端框架雷同, 此处应传递布局页的路径让前端import
        Framework = framework;
        ParentId = parentId;

        IsPublic = false;
    }
}
