using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation
{
    public class ApplicationMenu : IHasMenuItems, IHasExtraProperties
    {
        public const int DefaultOrder = 1000;
        /// <summary>
        /// 名称
        /// </summary>
        [NotNull] 
        public string Name { get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [NotNull] 
        public string DisplayName { get; }
        /// <summary>
        /// 说明
        /// </summary>
        [CanBeNull]
        public string Description { get; }
        /// <summary>
        /// 路径
        /// </summary>
        [NotNull]
        public string Url { get; }
        /// <summary>
        /// 组件
        /// </summary>
        [CanBeNull]
        public string Component { get;  }
        /// <summary>
        /// 重定向
        /// </summary>
        [CanBeNull]
        public string Redirect { get; }
        /// <summary>
        /// 图标
        /// </summary>
        [CanBeNull] 
        public string Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 是否可视
        /// </summary>
        public bool IsVisible { get; set; }

        [NotNull]
        public ApplicationMenuList Items { get; }

        public bool IsLeaf => Items.IsNullOrEmpty();

        [NotNull]
        public ExtraPropertyDictionary ExtraProperties { get; }

        public MultiTenancySides MultiTenancySides { get; }

        public ApplicationMenu(
            [NotNull] string name,
            [NotNull] string displayName,
            [NotNull] string url,
            [CanBeNull] string component,
            string description = null,
            string icon = null,
            string redirect = null,
            int order = DefaultOrder,
            MultiTenancySides multiTenancySides = MultiTenancySides.Both)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            Check.NotNullOrWhiteSpace(url, nameof(url));

            Name = name;
            DisplayName = displayName;
            Url = url;
            Component = component;
            Description = description;
            Icon = icon;
            Redirect = redirect ?? "";
            Order = order;
            MultiTenancySides = multiTenancySides;

            Items = new ApplicationMenuList();
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public ApplicationMenu AddItem([NotNull] ApplicationMenu menuItem)
        {
            Items.Add(menuItem);
            return menuItem;
        }

        public override string ToString()
        {
            return $"[ApplicationMenu] Name = {Name}";
        }
    }
}
