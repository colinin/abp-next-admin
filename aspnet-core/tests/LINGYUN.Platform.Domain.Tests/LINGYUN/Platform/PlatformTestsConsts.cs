using System;

namespace LINGYUN.Platform
{
    public static class PlatformTestsConsts
    {
        public static Guid User1Id { get; } = Guid.NewGuid();

        public static Guid User2Id { get; } = Guid.NewGuid();

        public static Guid TenantId { get; } = Guid.NewGuid();

        public static string Role1Name { get; } = "TestRole1";

        public static string Role2Name { get; } = "TestRole2";
    }
}
