using Shouldly;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Xunit;

namespace LINGYUN.Abp.DataProtection
{
    public class ProtectionFieldTests : AbpDataProtectionTestBase
    {
        private readonly IFakeProtectionObjectRepository _repository;
        private readonly ICurrentPrincipalAccessor _accessor;

        public ProtectionFieldTests()
        {
            _repository = GetRequiredService<IFakeProtectionObjectRepository>();
            _accessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        [Fact]
        public async virtual Task FakeAsync()
        {

            var values = new List<FakeProtectionObject>()
            {
                new FakeProtectionObject
                {
                    Protect1 = "Protect1",
                    Protect2 = "Protect1",
                    Value1 = "Value1",
                    Value2 = "Value1",
                    ProtectNum1 = 100,
                    ProtectNum2 = 200,
                    Num3 = 400
                },
                new FakeProtectionObject
                {
                    Protect1 = "test",
                    Protect2 = "Protect2",
                    Value1 = "Value2",
                    Value2 = "Value2",
                    ProtectNum1 = 1000,
                    ProtectNum2 = 2000,
                    Num3 = 3000
                },
                new FakeProtectionObject
                {
                    Protect1 = "test1",
                    Protect2 = "Protect3",
                    Value1 = "Value3",
                    Value2 = "Value3",
                    ProtectNum1 = 10000,
                    ProtectNum2 = 20000,
                    Num3 = 300
                },
                new FakeProtectionObject
                {
                    Protect1 = "test3",
                    Protect2 = "Protect4",
                    Value1 = "Value4",
                    Value2 = "Value4",
                    ProtectNum1 = 10000,
                    ProtectNum2 = 20000,
                    Num3 = 300
                }
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var user1Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Write,
                    DataAccessRole.User,
                    "user1",
                    [
                        new(nameof(FakeProtectionObject.Num3)),
                        new(nameof(FakeProtectionObject.Value2)),
                    ]);
                var user2Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Write,
                    DataAccessRole.User,
                    "user2",
                    [
                        new(nameof(FakeProtectionObject.Value1)),
                    ]);
                var role1Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Write,
                    DataAccessRole.Role,
                    "role1",
                    [
                        new(nameof(FakeProtectionObject.Protect1)),
                    ]);

                var dataAccessRule = new DataAccessRuleInfo(
                    [
                        user1Rule,
                        user2Rule,
                        role1Rule,
                    ]);

                //var rules = new List<ProtectedFieldRule>()
                //{
                //    new ProtectedFieldRule
                //    {
                //        Field = nameof(FakeProtectionObject.Protect1),
                //        Logic = PredicateOperator.And,
                //        Operator = ExpressionType.Equal,
                //        Resource = resource.Resource,
                //        Value = "test"
                //    },
                //    new ProtectedFieldRule
                //    {
                //        Field = nameof(FakeProtectionObject.Num3),
                //        Logic = PredicateOperator.Or,
                //        Operator = ExpressionType.LessThanOrEqual,
                //        Resource = resource.Resource,
                //        Value = 300
                //    },
                //};

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

                unitOfWorkManager.Current.SetAccessRuleInfo(dataAccessRule);

                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()));
                identity.AddClaim(new Claim(AbpClaimTypes.UserName, "user1"));
                identity.AddClaim(new Claim(AbpClaimTypes.Role, "role1"));
                identity.AddClaim(new Claim(AbpClaimTypes.Role, "role2"));
                using (_accessor.Change(new ClaimsPrincipal(identity)))
                {
                    await _repository.InsertManyAsync(values, true);
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var user1Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Read,
                    DataAccessRole.User,
                    "user1",
                    [
                        new(nameof(FakeProtectionObject.Num3)),
                        new(nameof(FakeProtectionObject.Value2)),
                    ]);
                var user2Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Write,
                    DataAccessRole.User,
                    "user2",
                    [
                        new(nameof(FakeProtectionObject.Value1)),
                    ]);
                var role1Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Write,
                    DataAccessRole.Role,
                    "role1",
                    [
                        new(nameof(FakeProtectionObject.Protect1)),
                    ]);

                var dataAccessRule = new DataAccessRuleInfo(
                    [
                        user1Rule,
                        user2Rule,
                        role1Rule,
                    ]);

                //var rules = new List<ProtectedFieldRule>()
                //{
                //    new ProtectedFieldRule
                //    {
                //        Field = nameof(FakeProtectionObject.Protect1),
                //        Logic = PredicateOperator.And,
                //        Operator = ExpressionType.Equal,
                //        Resource = resource.Resource,
                //        Value = "test"
                //    },
                //    new ProtectedFieldRule
                //    {
                //        Field = nameof(FakeProtectionObject.Num3),
                //        Logic = PredicateOperator.Or,
                //        Operator = ExpressionType.LessThanOrEqual,
                //        Resource = resource.Resource,
                //        Value = 300
                //    },
                //};

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.SetAccessRuleInfo(dataAccessRule);

                var identity2 = new ClaimsIdentity();
                identity2.AddClaim(new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()));
                identity2.AddClaim(new Claim(AbpClaimTypes.UserName, "user2"));
                identity2.AddClaim(new Claim(AbpClaimTypes.Role, "role2"));
                using (_accessor.Change(identity2))
                {
                    var result = await _repository.GetListAsync();
                    result.Count.ShouldBe(3);
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var user1Rule = new DataAccessRule(
                    typeof(FakeProtectionObject).FullName,
                    DataAccessOperation.Read,
                    DataAccessRole.User,
                    "user3",
                    [
                        new(nameof(FakeProtectionObject.Num3)),
                        new(nameof(FakeProtectionObject.Value2)),
                    ]);

                var dataAccessRule = new DataAccessRuleInfo(
                    [
                        user1Rule,
                    ]);

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.SetAccessRuleInfo(dataAccessRule);

                var identity3 = new ClaimsIdentity();
                identity3.AddClaim(new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()));
                identity3.AddClaim(new Claim(AbpClaimTypes.UserName, "user3"));
                identity3.AddClaim(new Claim(AbpClaimTypes.Role, "role3"));
                using (_accessor.Change(identity3))
                {
                    var result = await _repository.GetListAsync();
                    result.Count.ShouldBe(0);
                }
            });
        }
    }
}
