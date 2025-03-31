using LINGYUN.Abp.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore
{
    public static class AbpDataProtectionManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureDataProtectionManagement(
            this ModelBuilder builder,
            Action<AbpDataProtectionManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AbpDataProtectionManagementModelBuilderConfigurationOptions(
                DataProtectionManagementDbProterties.DbTablePrefix,
                DataProtectionManagementDbProterties.DbSchema
            );
            optionsAction?.Invoke(options);

            builder.Entity<EntityTypeInfo>(b =>
            {
                b.ToTable(options.TablePrefix + "Entitites", options.Schema);

                b.Property(p => p.Name)
                 .HasColumnName(nameof(EntityPropertyInfo.Name))
                 .HasMaxLength(EntityTypeInfoConsts.MaxNameLength)
                 .IsRequired();
                b.Property(p => p.DisplayName)
                 .HasColumnName(nameof(EntityPropertyInfo.DisplayName))
                 .HasMaxLength(EntityTypeInfoConsts.MaxDisplayNameLength)
                 .IsRequired();
                b.Property(p => p.TypeFullName)
                 .HasColumnName(nameof(EntityPropertyInfo.TypeFullName))
                 .HasMaxLength(EntityTypeInfoConsts.MaxTypeFullNameLength)
                 .IsRequired();

                b.HasMany(p => p.Properties)
                 .WithOne(p => p.TypeInfo)
                 .HasForeignKey(f => f.TypeInfoId)
                 .IsRequired();

                b.ConfigureByConvention();

                b.HasIndex(p => p.TypeFullName);
            });

            builder.Entity<EntityPropertyInfo>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityProperties", options.Schema);

                b.Property(p => p.Name)
                 .HasColumnName(nameof(EntityPropertyInfo.Name))
                 .HasMaxLength(EntityPropertyInfoConsts.MaxNameLength)
                 .IsRequired();
                b.Property(p => p.DisplayName)
                 .HasColumnName(nameof(EntityPropertyInfo.DisplayName))
                 .HasMaxLength(EntityPropertyInfoConsts.MaxDisplayNameLength)
                 .IsRequired();
                b.Property(p => p.TypeFullName)
                 .HasColumnName(nameof(EntityPropertyInfo.TypeFullName))
                 .HasMaxLength(EntityPropertyInfoConsts.MaxTypeFullNameLength)
                 .IsRequired();
                b.Property(p => p.JavaScriptType)
                 .HasColumnName(nameof(EntityPropertyInfo.JavaScriptType))
                 .HasMaxLength(EntityPropertyInfoConsts.MaxTypeFullNameLength)
                 .IsRequired();

                b.HasMany(p => p.Enums)
                 .WithOne(p => p.PropertyInfo)
                 .HasForeignKey(f => f.PropertyInfoId)
                 .IsRequired();

                b.ConfigureByConvention();

                b.HasIndex(p => new { p.TypeInfoId, p.TypeFullName });
            });

            builder.Entity<EntityEnumInfo>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityEnums", options.Schema);

                b.Property(p => p.Name)
                 .HasColumnName(nameof(EntityEnumInfo.Name))
                 .HasMaxLength(EntityEnumInfoConsts.MaxNameLength)
                 .IsRequired();
                b.Property(p => p.DisplayName)
                 .HasColumnName(nameof(EntityEnumInfo.DisplayName))
                 .HasMaxLength(EntityEnumInfoConsts.MaxDisplayNameLength)
                 .IsRequired();
                b.Property(p => p.Value)
                 .HasColumnName(nameof(EntityEnumInfo.Value))
                 .HasMaxLength(EntityEnumInfoConsts.MaxValueLength)
                 .IsRequired();

                b.ConfigureByConvention();

                b.HasIndex(p => new { p.PropertyInfoId, p.Name });
            });

            builder.Entity<RoleEntityRule>(b =>
            {
                b.ToTable(options.TablePrefix + "RoleEntityRules", options.Schema);

                b.Property(p => p.RoleName)
                 .HasColumnName(nameof(RoleEntityRule.RoleName))
                 .HasMaxLength(RoleEntityRuleConsts.MaxRuletNameLength)
                 .IsRequired();
                b.Property(p => p.AccessedProperties)
                 .HasColumnName(nameof(RoleEntityRule.AccessedProperties))
                 .HasMaxLength(EntityRuleConsts.MaxAccessedPropertiesLength);

                b.Property(p => p.FilterGroup)
                 .HasColumnName(nameof(RoleEntityRule.FilterGroup))
                 .HasConversion(new AbpJsonValueConverter<DataAccessFilterGroup>());

                b.HasOne(p => p.EntityTypeInfo)
                 .WithMany()
                 .HasForeignKey(f => f.EntityTypeId)
                 .IsRequired();

                b.ConfigureByConvention();
            });

            builder.Entity<OrganizationUnitEntityRule>(b =>
            {
                b.ToTable(options.TablePrefix + "OrganizationUnitEntityRules", options.Schema);

                b.Property(p => p.OrgCode)
                 .HasColumnName(nameof(OrganizationUnitEntityRule.OrgCode))
                 .HasMaxLength(OrganizationUnitEntityRuleConsts.MaxCodeLength)
                 .IsRequired();
                b.Property(p => p.AccessedProperties)
                 .HasColumnName(nameof(RoleEntityRule.AccessedProperties))
                 .HasMaxLength(EntityRuleConsts.MaxAccessedPropertiesLength);

                b.Property(p => p.FilterGroup)
                 .HasColumnName(nameof(RoleEntityRule.FilterGroup))
                 .HasConversion(new AbpJsonValueConverter<DataAccessFilterGroup>());

                b.HasOne(p => p.EntityTypeInfo)
                 .WithMany()
                 .HasForeignKey(f => f.EntityTypeId)
                 .IsRequired();

                b.ConfigureByConvention();
            });
        }
    }
}
