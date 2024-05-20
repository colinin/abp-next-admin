using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.DataProtection
{
    public class FakeProtectionObject : AuditedAggregateRoot<int>, IDataProtected
    {
        public virtual string Protect1 { get; set; }
        public virtual string Protect2 { get; set; }
        public virtual string Value1 { get; set; }
        public virtual string Value2 { get; set; }
        public virtual int ProtectNum1 { get; set; }
        public virtual int ProtectNum2 { get; set; }
        public virtual int Num3 { get; set; }

        public FakeProtectionObject()
        {
        }

        public FakeProtectionObject(int id) : base(id)
        {
        }
    }
}
