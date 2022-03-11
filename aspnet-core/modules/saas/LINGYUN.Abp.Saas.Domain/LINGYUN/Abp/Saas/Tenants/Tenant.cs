using JetBrains.Annotations;
using LINGYUN.Abp.Saas.Editions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.Saas.Tenants;

public class Tenant : FullAuditedAggregateRoot<Guid>
{
    protected const string DefaultConnectionStringName = Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName;

    public virtual string Name { get; protected set; }

    public virtual bool IsActive { get; set; }

    public virtual DateTime? EnableTime { get; protected set; }

    public virtual DateTime? DisableTime { get; protected set; }

    public virtual Guid? EditionId { get; set; }
    public virtual Edition Edition { get; set; }

    public virtual ICollection<TenantConnectionString> ConnectionStrings { get; protected set; }

    protected Tenant()
    {
        ConnectionStrings = new Collection<TenantConnectionString>();
    }

    protected internal Tenant(Guid id, [NotNull] string name)
        : base(id)
    {
        SetName(name);

        ConnectionStrings = new Collection<TenantConnectionString>();
    }

    public virtual void SetEnableTime(DateTime? enableTime = null)
    {
        EnableTime = enableTime;
    }

    public virtual void SetDisableTime(DateTime? disableTime = null)
    {
        DisableTime = disableTime;
    }

    [CanBeNull]
    public virtual string FindDefaultConnectionString()
    {
        return FindConnectionString(DefaultConnectionStringName);
    }

    [CanBeNull]
    public virtual string FindConnectionString(string name)
    {
        return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    public virtual void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(DefaultConnectionStringName, connectionString);
    }

    public virtual void SetConnectionString(string name, string connectionString)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            tenantConnectionString.SetValue(connectionString);
        }
        else
        {
            ConnectionStrings.Add(new TenantConnectionString(Id, name, connectionString));
        }
    }

    public virtual void RemoveDefaultConnectionString()
    {
        RemoveConnectionString(DefaultConnectionStringName);
    }

    public virtual void RemoveConnectionString(string name)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            ConnectionStrings.Remove(tenantConnectionString);
        }
    }

    protected internal virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
    }
}
