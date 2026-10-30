using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Authorization;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 测试用授权服务
/// </summary>
/// <remarks>
/// 替换真实的 IAuthorizationService, 由测试用例控制:
/// - 功能权限: policyName(如 AbpBlobManagement.Blob)
/// - 资源级权限: policyName + 资源标识(如 LINGYUN.Abp.BlobManagement.Blob.View + BlobId)
/// </remarks>
public class TestAuthorizationService : IAbpAuthorizationService
{
    private readonly HashSet<string> _grantedPolicies = new(StringComparer.Ordinal);
    private readonly HashSet<string> _grantedResources = new(StringComparer.Ordinal);

    public IServiceProvider ServiceProvider { get; }

    public ClaimsPrincipal CurrentPrincipal => new(new ClaimsIdentity());

    /// <summary>
    /// 已检查的授权策略(按调用顺序)
    /// </summary>
    public List<string> CheckedPolicies { get; } = new();

    /// <summary>
    /// 已检查的资源(按调用顺序)
    /// </summary>
    public List<object> CheckedResources { get; } = new();

    public TestAuthorizationService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// 授予功能权限
    /// </summary>
    public TestAuthorizationService Grant(params string[] policyNames)
    {
        foreach (var policyName in policyNames)
        {
            _grantedPolicies.Add(policyName);
        }

        return this;
    }

    /// <summary>
    /// 授予资源级权限
    /// </summary>
    public TestAuthorizationService GrantResource(string policyName, Guid blobId)
    {
        _grantedResources.Add(CalculateResourceKey(policyName, blobId));

        return this;
    }

    /// <summary>
    /// 撤销资源级权限
    /// </summary>
    public TestAuthorizationService RevokeResource(string policyName, Guid blobId)
    {
        _grantedResources.Remove(CalculateResourceKey(policyName, blobId));

        return this;
    }

    /// <summary>
    /// 清空所有授权
    /// </summary>
    public TestAuthorizationService Reset()
    {
        _grantedPolicies.Clear();
        _grantedResources.Clear();
        CheckedPolicies.Clear();
        CheckedResources.Clear();

        return this;
    }

    public Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user,
        object? resource,
        string policyName)
    {
        CheckedPolicies.Add(policyName);

        var granted = resource == null
            ? _grantedPolicies.Contains(policyName)
            : _grantedResources.Contains(CalculateResourceKey(policyName, resource));

        if (resource != null)
        {
            CheckedResources.Add(resource);
        }

        return Task.FromResult(granted
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failed());
    }

    public Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user,
        object? resource,
        IEnumerable<IAuthorizationRequirement> requirements)
    {
        // 本测试未使用基于 requirement 的授权
        return Task.FromResult(AuthorizationResult.Success());
    }

    protected static string CalculateResourceKey(string policyName, object resource)
    {
        return resource is Blob blob
            ? CalculateResourceKey(policyName, blob.Id)
            : $"{policyName}:{resource}";
    }

    protected static string CalculateResourceKey(string policyName, Guid blobId)
    {
        return $"{policyName}:{blobId:N}";
    }
}
