using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowManagement
{
    public class WorkflowData : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid WorkflowId { get; protected set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; protected set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual DataType DataType { get; protected set; }
        /// <summary>
        /// 是否必输
        /// 默认: false
        /// </summary>
        public virtual bool IsRequired { get; protected set; }
        /// <summary>
        /// 是否区分大小写
        /// 默认: false
        /// 暂无用
        /// </summary>
        public virtual bool IsCaseSensitive { get; protected set; }
        protected WorkflowData() { }
        public WorkflowData(
            Guid id,
            Guid workflowId,
            string name,
            string displayName,
            DataType dataType = DataType.String,
            bool isRequired = false,
            bool isCaseSensitive = false,
            Guid? tenantId = null) : base(id)
        {
            WorkflowId = workflowId;
            Name = name;
            DisplayName = displayName;
            DataType = dataType;
            IsRequired = isRequired;
            IsCaseSensitive = isCaseSensitive;
            TenantId = tenantId;
        }

        public bool TryParse(object input, out object value)
        {
            if (input == null)
            {
                if (IsRequired)
                {
                    throw new BusinessException(WorkflowManagementErrorCodes.InvalidInputNullable)
                        .WithData("Property", DisplayName);
                }
                // 字典类型不能为空
                value = new { };
                return true;
            }

            switch (DataType)
            {
                case DataType.String:
                    value = input.ToString();
                    return true;
                case DataType.Booleaen:
                    if (input is bool boValue)
                    {
                        value = boValue;
                        return true;
                    }
                    value = input.ToString().ToLower() == "true";
                    return true;
                case DataType.Date:
                case DataType.DateTime:
                    if (input is DateTime dateValue)
                    {
                        value = DataType == DataType.Date
                            ? dateValue.Date
                            : dateValue;
                        return true;
                    }
                    value = DateTime.Parse(input.ToString());
                    return true;
                case DataType.Number:
                    if (int.TryParse(input.ToString(), out int intValue))
                    {
                        value = intValue;
                        return true;
                    }
                    value = 0;
                    return false;
                case DataType.Object:
                default:
                    value = input;
                    return true;
            }
        }
    }
}
