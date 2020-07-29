using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes
{
    // TODO: 因为abp的菜单设计模式是按照权限分配的,是否可以去掉路由的后台配置? 
    // 按钮、菜单权限可以在子权限中定义
    // 数据权限需要DbContext拦截器或者创建一个数据过滤仓储的抽象类,需要过滤数据权限的继承自此仓储
    public class Route : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 全名
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; protected set; }
        /// <summary>
        /// 平台标识
        /// </summary>
        public virtual PlatformType PlatformType { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; protected set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public virtual string LinkUrl { get; protected set; }
        /// <summary>
        /// 是否菜单
        /// </summary>
        public virtual bool IsMenu { get; set; }
        /// <summary>
        /// 是否工具栏
        /// </summary>
        public virtual bool IsToolBar { get; set; }
        /// <summary>
        /// 是否侧边栏
        /// </summary>
        public virtual bool IsSideBar { get; set; }
        /// <summary>
        /// 是否公共路由
        /// </summary>
        public virtual bool IsPublic { get; set; }
        /// <summary>
        /// 是否内置
        /// </summary>
        public virtual bool IsStatic { get; set; }
        /// <summary>
        /// 总是显示根菜单
        /// </summary>
        public virtual bool AlwaysShow { get; set; }
        /// <summary>
        /// 父级标识
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        protected Route()
        {

        }

        public Route(Guid id, string name, string displayName, string url)
        {
            Id = id;
            LinkToUrl(url);
            ChangeName(name, displayName);
        }

        public void LinkToUrl([NotNull] string linkUrl)
        {
            LinkUrl = Check.NotNullOrWhiteSpace(linkUrl, nameof(linkUrl));
        }

        public void ChangeIcon(string icon)
        {
            Icon = icon;
        }

        public void ChangeName([NotNull] string name, [CanBeNull] string displayName)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName;
        }

        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', RouteConsts.CodeUnitLength))).JoinAsString(".");
        }

        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }
    }
}
