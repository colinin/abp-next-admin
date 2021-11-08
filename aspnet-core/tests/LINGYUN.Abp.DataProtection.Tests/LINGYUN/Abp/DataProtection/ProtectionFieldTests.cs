using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Volo.Abp.Security.Claims;
using System.Security.Claims;
using System;
using Shouldly;
using Volo.Abp.Uow;

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
        public virtual async Task FakeAsync()
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
                var resource = new ProtectedResource
                {
                    Resource = typeof(FakeProtectionObject).FullName,
                    Behavior = ProtectBehavior.All,
                    Owner = "user1",
                    Priority = 10,
                    Visitor = "user1,role1"
                };

                var fields = new List<ProtectedField>()
                {
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Owner = "user1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Value1),
                        Owner = "user2",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Value2),
                        Owner = "user1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Protect1),
                        Owner = "role1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                };

                var rules = new List<ProtectedFieldRule>()
                {
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Protect1),
                        Logic = PredicateOperator.And,
                        Operator = ExpressionType.Equal,
                        Resource = resource.Resource,
                        Value = "test"
                    },
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Logic = PredicateOperator.Or,
                        Operator = ExpressionType.LessThanOrEqual,
                        Resource = resource.Resource,
                        Value = 300
                    },
                };

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.AddItem<ResourceGrantedResult>(
                    "ResourceGranted",
                    new ResourceGrantedResult(
                        resource,
                        fields.ToArray(),
                        rules.ToArray()));

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
                var resource = new ProtectedResource
                {
                    Resource = typeof(FakeProtectionObject).FullName,
                    Behavior = ProtectBehavior.All,
                    Owner = "user1",
                    Priority = 10,
                    Visitor = "user1,role1"
                };

                var fields = new List<ProtectedField>()
                {
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Owner = "user1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Value1),
                        Owner = "user2",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Value2),
                        Owner = "user1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Protect1),
                        Owner = "role1",
                        Resource = resource.Resource,
                        Visitor = "",
                    },
                };

                var rules = new List<ProtectedFieldRule>()
                {
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Protect1),
                        Logic = PredicateOperator.And,
                        Operator = ExpressionType.Equal,
                        Resource = resource.Resource,
                        Value = "test"
                    },
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Logic = PredicateOperator.Or,
                        Operator = ExpressionType.LessThanOrEqual,
                        Resource = resource.Resource,
                        Value = 300
                    },
                };

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.AddItem<ResourceGrantedResult>(
                    "ResourceGranted",
                    new ResourceGrantedResult(
                        resource,
                        fields.ToArray(),
                        rules.ToArray()));

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
                var resource = new ProtectedResource
                {
                    Resource = typeof(FakeProtectionObject).FullName,
                    Behavior = ProtectBehavior.All,
                    Priority = 10,
                    Visitor = "user3"
                };

                var fields = new List<ProtectedField>()
                {
                    new ProtectedField
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Owner = "user1",
                        Resource = resource.Resource,
                        Visitor = "",
                    }
                };

                var rules = new List<ProtectedFieldRule>()
                {
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Protect1),
                        Logic = PredicateOperator.And,
                        Operator = ExpressionType.Equal,
                        Resource = resource.Resource,
                        Value = "test"
                    },
                    new ProtectedFieldRule
                    {
                        Field = nameof(FakeProtectionObject.Num3),
                        Logic = PredicateOperator.Or,
                        Operator = ExpressionType.LessThanOrEqual,
                        Resource = resource.Resource,
                        Value = 300
                    },
                };

                var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.AddItem<ResourceGrantedResult>(
                    "ResourceGranted",
                    new ResourceGrantedResult(
                        resource,
                        fields.ToArray(),
                        rules.ToArray()));

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
