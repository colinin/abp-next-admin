using LINGYUN.Abp.DataProtection.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessEntityTypeInfoProvider : IDataAccessEntityTypeInfoProvider, ISingletonDependency
{
    public async virtual Task<EntityTypeInfoModel> GetEntitTypeInfoAsync(DataAccessEntitTypeInfoContext context)
    {
        var allowProperties = new List<string>();

        var dataProtectionOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpDataProtectionOptions>>().Value;
        var javaScriptTypeConvert = context.ServiceProvider.GetRequiredService<IJavaScriptTypeConvert>();
        var localizerFactory = context.ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
        var stringLozalizer = localizerFactory.Create(context.ResourceType);

        var entityTypeRuleModel = new EntityTypeInfoModel
        {
            Name = context.EntityType.Name,
            DisplayName = stringLozalizer[$"DisplayName:{context.EntityType.Name}"].Value ?? context.EntityType.Name
        };

        var subjectContext = new DataAccessSubjectContributorContext(
            context.EntityType.FullName,
            context.Operation,
            context.ServiceProvider);

        foreach (var subjectContributor in dataProtectionOptions.SubjectContributors)
        {
            var subjectAllowProperties = await subjectContributor.GetAccessdProperties(subjectContext);

            allowProperties.AddIfNotContains(subjectAllowProperties);
        }

        IEnumerable<PropertyInfo> entityPropeties = context.EntityType.GetProperties();

        if (allowProperties.Count > 0)
        {
            if (dataProtectionOptions.EntityIgnoreProperties.TryGetValue(context.EntityType, out var entityIgnoreProps))
            {
                allowProperties.AddIfNotContains(entityIgnoreProps);
            }

            allowProperties.AddIfNotContains(dataProtectionOptions.GlobalIgnoreProperties);

            entityPropeties = entityPropeties.Where(x => allowProperties.Contains(x.Name));
        }

        foreach (var propertyInfo in entityPropeties)
        {
            // 字段本地化描述规则
            // 在本地化文件中定义 DisplayName:PropertyName
            var localizedProp = stringLozalizer[$"DisplayName:{propertyInfo.Name}"];
            var propertyInfoResult = javaScriptTypeConvert.Convert(propertyInfo.PropertyType);
            var entityPropertyInfo = new EntityPropertyInfoModel
            {
                Name = propertyInfo.Name,
                TypeFullName = propertyInfo.PropertyType.FullName,
                DisplayName = localizedProp.Value ?? propertyInfo.Name,
                JavaScriptType = propertyInfoResult.Type,
                JavaScriptName = propertyInfo.Name.ToCamelCase(),
                Operates = propertyInfoResult.AllowOperates
            };

            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsNullableType())
            {
                propertyType = propertyType.GetGenericArguments().FirstOrDefault();
            }

            if (typeof(Enum).IsAssignableFrom(propertyType))
            {
                var enumNames = Enum.GetNames(propertyType);
                var enumValues = Enum.GetValues(propertyType);
                var paramterOptions = new EntityEnumInfoModel[enumNames.Length];
                for (var index = 0; index < enumNames.Length; index++)
                {
                    var enumName = enumNames[index];
                    var localizerEnumKey = $"{propertyInfo.Name}:{enumName}";
                    var localizerEnumName = stringLozalizer[localizerEnumKey];
                    paramterOptions[index] = new EntityEnumInfoModel
                    {
                        Key = localizerEnumName.ResourceNotFound ? enumName : localizerEnumName.Value,
                        Value = enumValues.GetValue(index),
                    };
                }
                entityPropertyInfo.Enums = paramterOptions;
            }

            entityTypeRuleModel.Properties.Add(entityPropertyInfo);
        }

        return entityTypeRuleModel;
    }
}
