using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

public static class AbpDataProtectionDbContextModelBuilderExtensions
{
    public static void ConfigureEntityAuth<TEntity, TKey, TEntityAuth>(this ModelBuilder builder)
        where TEntity : class
        where TEntityAuth : DataAuthBase<TEntity, TKey>
    {
        Check.NotNull(builder, nameof(builder));

        var entityType = builder.Model.FindEntityType(typeof(TEntity)) ??
            throw new ArgumentException("In the builder.ConfigureEntityAuth<TEntity> configuration entity mapping relationship before ().");

        var entityTableName = entityType.GetTableName() ?? typeof(TEntity).Name;
        var entitySchema = entityType.GetSchema();
        var keyValueType = typeof(TKey);

        builder.Entity<TEntityAuth>(b =>
        {
            b.ToTable(entityTableName + "Auths", entitySchema);

            b.Property(p => p.EntityType)
             .HasColumnName(nameof(DataAuthBase<TEntity, TKey>.EntityType))
             .HasMaxLength(128)
             .IsRequired();

            if (keyValueType == typeof(string) || keyValueType == typeof(Guid))
            {
                b.Property(p => p.EntityId)
                 .HasColumnName(nameof(DataAuthBase<TEntity, TKey>.EntityId))
                 .HasMaxLength(64)
                 .IsRequired();
            }
            else
            {
                b.Property(p => p.EntityId)
                 .HasColumnName(nameof(DataAuthBase<TEntity, TKey>.EntityId))
                 .IsRequired();
            }

            b.Property(p => p.Role)
             .HasColumnName(nameof(DataAuthBase<TEntity, TKey>.Role))
             .HasMaxLength(32);
            b.Property(p => p.OrganizationUnit)
             .HasColumnName(nameof(DataAuthBase<TEntity, TKey>.OrganizationUnit))
             .HasMaxLength(20);

            b.HasIndex(p => p.Role);
            b.HasIndex(p => p.OrganizationUnit);
        });
    }
}
