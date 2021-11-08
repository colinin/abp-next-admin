using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.DataProtection
{
    public class FakeProtectionObject : Entity<int>, IDataProtection
    {
        public virtual string Owner { get; set; }
        public virtual string Protect1 { get; set; }
        public virtual string Protect2 { get; set; }
        public virtual string Value1 { get; set; }
        public virtual string Value2 { get; set; }
        public virtual int ProtectNum1 { get; set; }
        public virtual int ProtectNum2 { get; set; }
        public virtual int Num3 { get; set; }
    }
}
