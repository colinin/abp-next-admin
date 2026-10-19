using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/users
    /// 参考: https://openobserve.ai/docs/reference/api/user/create/
    /// </remarks>
    /// <param name="email">邮箱</param>
    /// <param name="password">密码</param>
    /// <param name="role">角色: admin / user</param>
    public async virtual Task<OpenObserveCodeMessageResponse> CreateUserAsync(
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        string? role = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/users";

        var request = new CreateUserRequest
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            Role = role,
        };

        return (await PostJsonAsync<CreateUserRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <remarks>
    /// PUT /api/{organization}/users/{user_email}
    /// 不修改密码时不要传 oldPassword / newPassword
    /// 参考: https://openobserve.ai/docs/reference/api/user/create/
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> UpdateUserAsync(
        string userEmail,
        string? firstName = null,
        string? lastName = null,
        string? oldPassword = null,
        string? newPassword = null,
        string? role = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/users/{Uri.EscapeDataString(userEmail)}";

        var request = new UpdateUserRequest
        {
            FirstName = firstName,
            LastName = lastName,
            OldPassword = oldPassword,
            NewPassword = newPassword,
            Role = role,
        };

        return (await PutJsonAsync<UpdateUserRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 添加已有用户到组织
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/users/{user_email}
    /// 参考: https://openobserve.ai/docs/reference/api/user/create/
    /// </remarks>
    /// <param name="role">角色: admin / user</param>
    public async virtual Task<OpenObserveCodeMessageResponse> AddUserToOrganizationAsync(
        string userEmail,
        string? role = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/users/{Uri.EscapeDataString(userEmail)}";

        var request = new AddUserToOrganizationRequest
        {
            Role = role,
        };

        return (await PostJsonAsync<AddUserToOrganizationRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 从组织移除用户
    /// </summary>
    /// <remarks>
    /// DELETE /api/{organization}/users/{user_email}
    /// 参考: https://openobserve.ai/docs/reference/api/user/delete/
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> RemoveUserFromOrganizationAsync(
        string userEmail,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/users/{Uri.EscapeDataString(userEmail)}";

        return (await DeleteAsync<OpenObserveCodeMessageResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 列出组织内的用户
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/users
    /// 参考: https://openobserve.ai/docs/reference/api/user/list/
    /// </remarks>
    public async virtual Task<UserListResponse> GetUsersAsync(
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/users";

        return (await GetAsync<UserListResponse>(requestUri, cancellationToken: cancellationToken))!;
    }
}
