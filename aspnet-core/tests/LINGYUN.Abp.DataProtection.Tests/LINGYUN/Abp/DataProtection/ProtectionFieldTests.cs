using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.DataProtection.Keywords;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Xunit;

namespace LINGYUN.Abp.DataProtection
{
    public class ProtectionFieldTests : AbpDataProtectionTestBase
    {
        private readonly IFakeProtectionObjectRepository _repository;
        private readonly ICurrentPrincipalAccessor _accessor;
        private readonly IDataProtectedResourceStore _store;
        private readonly IDataFilter _dataFilter;

        public ProtectionFieldTests()
        {
            _repository = GetRequiredService<IFakeProtectionObjectRepository>();
            _accessor = GetRequiredService<ICurrentPrincipalAccessor>();
            _store = GetRequiredService<IDataProtectedResourceStore>();
            _dataFilter = GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async virtual Task Should_Protected_Data_By_Repo()
        {
            var validUser = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(AbpClaimTypes.UserId, validUser.ToString()));
                identity.AddClaim(new Claim(AbpClaimTypes.UserName, "user1"));
                identity.AddClaim(new Claim(AbpClaimTypes.Role, "role1"));
                identity.AddClaim(new Claim(AbpClaimTypes.Role, "role2"));
                identity.AddClaim(new Claim(AbpOrganizationUnitClaimTypes.OrganizationUnit, "00001"));
                identity.AddClaim(new Claim(AbpOrganizationUnitClaimTypes.OrganizationUnit, "00001.00002"));
                using (_accessor.Change(new ClaimsPrincipal(identity)))
                {
                    var values = new List<FakeProtectionObject>()
                    {
                        new FakeProtectionObject(1)
                        {
                            Protect1 = "Protect1",
                            Protect2 = "Protect1",
                            Value1 = "Value1",
                            Value2 = "Value1",
                            ProtectNum1 = 100,
                            ProtectNum2 = 200,
                            Num3 = 400,
                        },
                        new FakeProtectionObject(2)
                        {
                            Protect1 = "test",
                            Protect2 = "Protect2",
                            Value1 = "Value2",
                            Value2 = "Value2",
                            ProtectNum1 = 1000,
                            ProtectNum2 = 2000,
                            Num3 = 3000
                        },
                        new FakeProtectionObject(3)
                        {
                            Protect1 = "test1",
                            Protect2 = "Protect3",
                            Value1 = "Value3",
                            Value2 = "Value3",
                            ProtectNum1 = 10000,
                            ProtectNum2 = 20000,
                            Num3 = 300
                        },
                        new FakeProtectionObject(4)
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
                    await _repository.InsertManyAsync(values, true);
                }

                // role2规则
                var rule2FilterGroup = new DataAccessFilterGroup();
                // 只允许查询Num3小于等于400
                rule2FilterGroup.AddRule(new DataAccessFilterRule(nameof(FakeProtectionObject.Num3), 400, DataAccessFilterOperate.LessOrEqual));
                _store.Set(new DataAccessResource(RolePermissionValueProvider.ProviderName, "role2", typeof(FakeProtectionObject).FullName, DataAccessOperation.Read, rule2FilterGroup));

                // role3编辑规则
                var rule3WriteAccess = new DataAccessFilterGroup();
                // 只允许编辑自己提交的数据
                rule3WriteAccess.AddRule(new DataAccessFilterRule(nameof(FakeProtectionObject.CreatorId), DataAccessCurrentUserContributor.Name, DataAccessFilterOperate.Equal));
                var rule3WriteAccessCacheItem = new DataAccessResource(RolePermissionValueProvider.ProviderName, "role3", typeof(FakeProtectionObject).FullName, DataAccessOperation.Write, rule3WriteAccess);
                // 只允许编辑Num3字段
                rule3WriteAccessCacheItem.AllowProperties.AddRange(new string[] { nameof(FakeProtectionObject.Num3) });
                _store.Set(rule3WriteAccessCacheItem);

                // role1读取规则
                var rule1ReadAccess = new DataAccessFilterGroup();
                // 只允许读取自己提交的数据
                rule1ReadAccess.AddRule(new DataAccessFilterRule(nameof(FakeProtectionObject.CreatorId), DataAccessCurrentUserContributor.Name, DataAccessFilterOperate.Equal));
                var rule1ReadAccessCacheItem = new DataAccessResource(RolePermissionValueProvider.ProviderName, "role1", typeof(FakeProtectionObject).FullName, DataAccessOperation.Read, rule1ReadAccess);
                // 只允许读取Num3字段
                rule1ReadAccessCacheItem.AllowProperties.AddRange(new string[] { nameof(FakeProtectionObject.Id), nameof(FakeProtectionObject.Num3) });
                _store.Set(rule1ReadAccessCacheItem);


                // ou1读取规则
                var ou1ReadAccess = new DataAccessFilterGroup();
                // 允许读本部门及下级部门数据
                // 获取部门树结构列表, 便利增加多个部门条件集
                ou1ReadAccess.AddRule(new DataAccessFilterRule($"{nameof(FakeProtectionObject.CreatorId)}", new List<Guid?>() { validUser, Guid.NewGuid() }, DataAccessFilterOperate.Contains, true));
                //ou1ReadAccess.AddRule(new DataAccessFilterRule($"{nameof(FakeProtectionObject.ExtraProperties)}.{DataAccessKeywords.AUTH_ORGS}", "[00001]", DataAccessFilterOperate.Contains));
                //ou1ReadAccess.AddRule(new DataAccessFilterRule($"{nameof(FakeProtectionObject.ExtraProperties)}.{DataAccessKeywords.AUTH_ORGS}", "[00001.00002]", DataAccessFilterOperate.Contains));

                var ou1ReadAccessCacheItem = new DataAccessResource(OrganizationUnitPermissionValueProvider.ProviderName, "00001", typeof(FakeProtectionObject).FullName, DataAccessOperation.Read, ou1ReadAccess);

                // 只允许读取Num3字段
                ou1ReadAccessCacheItem.AllowProperties.AddRange(new string[] { nameof(FakeProtectionObject.Id), nameof(FakeProtectionObject.Num3) });
                _store.Set(ou1ReadAccessCacheItem);
            });

            await WithUnitOfWorkAsync(async () =>
            {
                // 标记不启用数据过滤器应该能获取全部数据
                var result = await _repository.GetAllListAsync();
                result.Count.ShouldBe(4);
            });

            var readUser = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                var identity2 = new ClaimsIdentity();
                identity2.AddClaim(new Claim(AbpClaimTypes.UserId, readUser.ToString()));
                identity2.AddClaim(new Claim(AbpClaimTypes.UserName, "user2"));
                identity2.AddClaim(new Claim(AbpClaimTypes.Role, "role2"));
                using (_accessor.Change(identity2))
                {
                    // 应该只能读取到 Num3小于等于400的数据，只有3条
                    var result = await _repository.GetListAsync();
                    result.Count.ShouldBe(3);
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var identity3 = new ClaimsIdentity();
                identity3.AddClaim(new Claim(AbpClaimTypes.UserId, readUser.ToString()));
                identity3.AddClaim(new Claim(AbpClaimTypes.UserName, "user2"));
                identity3.AddClaim(new Claim(AbpOrganizationUnitClaimTypes.OrganizationUnit, "00001"));
                using (_accessor.Change(identity3))
                {
                    // 应该能获取4条数据
                    var result = await _repository.GetListAsync();
                    result.Count.ShouldBe(4);
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var identity3 = new ClaimsIdentity();
                identity3.AddClaim(new Claim(AbpClaimTypes.UserId, validUser.ToString()));
                identity3.AddClaim(new Claim(AbpClaimTypes.UserName, "user3"));
                identity3.AddClaim(new Claim(AbpClaimTypes.Role, "role3"));
                using (_accessor.Change(identity3))
                {
                    var entity = await _repository.FindAsync(1);
                    entity.Protect1 = "New Protect1";
                    entity.Num3 = 100;
                    // 切换为授权用户应该可以保存数据
                    var resultEntity = await _repository.UpdateAsync(entity, true);
                    // 授权字段应该会保存成功
                    resultEntity.Num3.ShouldBe(100);
                    // 未授权字段应该不会保存成功
                    resultEntity.Protect1.ShouldBe("Protect1");
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var identity3 = new ClaimsIdentity();
                identity3.AddClaim(new Claim(AbpClaimTypes.UserId, validUser.ToString()));
                identity3.AddClaim(new Claim(AbpClaimTypes.UserName, "user3"));
                identity3.AddClaim(new Claim(AbpClaimTypes.Role, "role1"));
                // identity3.AddClaim(new Claim(AbpClaimTypes.Role, "role3"));
                using (_accessor.Change(identity3))
                {

                    //var filterBuilder = GetRequiredService<IEntityTypeFilterBuilder>();
                    //var group = new DataAccessFilterGroup();
                    //group.AddRule(new DataAccessFilterRule(nameof(FakeProtectionObject.Num3), 300, DataAccessFilterOperate.LessOrEqual));

                    //var exp1 = await filterBuilder.BuildAsync(typeof(FakeProtectionObject), DataAccessOperation.Read, group);
                    //var queryable = await _repository.GetQueryableAsync();
                    //var entities = queryable.Where(exp1.Compile().As<Func<FakeProtectionObject, bool>>()).ToList();

                    var entity = await _repository.FindAsync(1);

                    var resultBuilder = GetRequiredService<IEntityPropertyResultBuilder>();
                    var exp = resultBuilder.Build(typeof(FakeProtectionObject), DataAccessOperation.Read);
                    var comp = exp.Compile();
                    // 使用非泛型接口编译表达式树，对返回结果进行替换
                    var resultEntity = comp.DynamicInvoke(entity).As<FakeProtectionObject>();
                    // 未授权字段应返回默认值
                    resultEntity.Protect1.ShouldBeNullOrWhiteSpace();
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var identity3 = new ClaimsIdentity();
                identity3.AddClaim(new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()));
                identity3.AddClaim(new Claim(AbpClaimTypes.UserName, "user3"));
                identity3.AddClaim(new Claim(AbpClaimTypes.Role, "role3"));
                using (_accessor.Change(identity3))
                {
                    var entity = await _repository.FindAsync(1);

                    entity.Num3 = 300;
                    // 对未授权实体的操作应该抛出未授权异常
                    await Assert.ThrowsAsync<AbpAuthorizationException>(async () => await _repository.UpdateAsync(entity, true));
                }
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var entity = await _repository.FindAsync(1);
                entity.Num3 = 200;
                using (_dataFilter.Disable<IDataProtected>())
                {
                    // 禁用数据保护过滤器之后应可保存数据
                    var resultEntity = await _repository.UpdateAsync(entity);
                    resultEntity.Num3.ShouldBe(200);
                }
            });
        }
    }
}
