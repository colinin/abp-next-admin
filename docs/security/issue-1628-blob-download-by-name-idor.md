# Issue #1628 分析报告：BlobManagement 按名称下载的跨租户越权（IDOR）

> 关联：<https://github.com/colinin/abp-next-admin/issues/1628>
> 版本：10.6.0 及当前 dev 分支（本地仓库 `E:\open-sources\labp`）
> 环境：`LINGYUN.Abp.MicroService.AllInOne`，`Single.PostgreSql.Dev`（http://localhost:30000）

---

## 1. 结论

漏洞真实存在，已在本地 AllInOne 单体内完整复现：

- 任意租户的管理员（持有 `AbpBlobManagement.Blob` 权限）可以把请求里的 `TenantId` 指定为**其它租户**，
  读取该租户 `users`（私有）容器中的任意文件，返回文件内容 200。
- 同一缺陷还导致**同租户内的资源级鉴权被绕过**：管理员可以读取/删除/生成其他用户私有文件的下载链接，
  而 private 端点对同一文件是正确拒绝的（403）。

CWE-639（Authorization Bypass Through User-Controlled Key）+ CWE-862（Missing Authorization）。

---

## 2. 调用链与缺陷点

```
GET /api/blob-management/blobs/download/by-name/t/{tenantId}/{name}
    ?ContainerName=users&BlobName=/tenant-b-secret.txt
        │
        ▼
BlobController.DownloadByNameAsync            HttpApi/.../BlobController.cs
        │
        ▼
BlobAppService.DownloadByNameAsync            Application/.../BlobAppService.cs
        │  CurrentTenant.Change(input.TenantId ?? CurrentTenant.Id)   ← 缺陷 1
        ▼
BlobContainerRepository.GetByNameAsync        （IMultiTenant 查询过滤器，此时已是受害租户）
        ▼
BlobAppServiceBase.DownloadByNameAsync        Application/.../BlobAppServiceBase.cs
        │  FindBlobByNameAsync → 命中受害租户的同名文件
        │  CheckGetPolicyAsync(containerName, blob)
        │      └─ BlobAppService 重写后只检查功能权限，丢弃容器策略   ← 缺陷 2
        ▼
BlobManager.DownloadBlobsync                  → 文件写入响应
```

### 缺陷 1：客户端可控租户（核心）

`BlobDownloadByNameInput.TenantId` 直接来自请求（查询串或 `t/{tenantId}` 路由），
`BlobAppService` 无条件用它切换 ABP 的租户上下文：

```csharp
// 修复前
using (CurrentTenant.Change(input.TenantId ?? CurrentTenant.Id))
{
    var blobContainer = await BlobContainerRepository.GetByNameAsync(
        BlobManager.GetBlobProvider(), input.ContainerName);
    return await base.DownloadByNameAsync(blobContainer, input.BlobName);
}
```

`Blob`、`BlobContainer` 都是 `IMultiTenant`，`BlobManagementDbContext` 未关闭租户过滤器，
所以切换后 `GetByNameAsync` / `FindByFullNameAsync` 会直接命中受害租户的数据行。

### 缺陷 2：资源级鉴权被覆盖丢弃

- `BlobAppServiceBase.CheckGetPolicyAsync(string containerName, Blob blob)` 会取
  `AbpBlobManagementOptions.BlobPolicyCheckProviders[containerName]`，
  对 `users` 容器执行资源级权限 `LINGYUN.Abp.BlobManagement.Blob.View`（基于 blob.Id 的授权）。
- `BlobAppService` 重写了该方法，却只调用 `base.CheckGetPolicyAsync(blob)`（即 `AbpBlobManagement.Blob` 功能权限），
  把容器级/资源级检查整个丢弃：

```csharp
// 修复前
protected async override Task CheckGetPolicyAsync(string containerName, Blob blob)
{
    await base.CheckGetPolicyAsync(blob);   // 只有功能权限
}
protected override Task CheckDeletePolicyAsync(string containerName, Blob blob)
{
    return base.CheckDeletePolicyAsync(blob);  // 同上
}
```

而 `GetAsync(id)` / `GenerateDownloadUrlAsync(id)` / `GeneratePreviewUrlAsync(id)` 走的是
`CheckGetPolicyAsync(blob)`（无容器名），同样只做功能权限检查，因此也绕过了资源级授权。

### 缺陷 3（放大器）：权限判定发生在租户切换之后

`CheckPolicyAsync("AbpBlobManagement.Blob")` 在 `CurrentTenant` 已变成受害租户之后执行：

- ABP 的权限授予（`AbpPermissionGrants`）按租户隔离；
- 用户令牌里的角色声明只有**角色名**（`role: "admin"`），不含租户；
- 每个租户创建时都会种一个 `admin` 角色，并被授予全部权限（`AdminServiceDataSeeder` → `IdentityDataSeeder`）。

于是攻击者自己租户的 `admin` 角色名，在受害租户的授权表里同样能命中 `AbpBlobManagement.Blob`
（以及 `users` 容器资源授权的绕过），构造出「任意租户管理员 → 任意租户文件」的越权。

实测数据库证据（修复前）：

```
AbpRoles:            (tenant-a, admin) / (tenant-b, admin)
AbpPermissionGrants: (tenant-b, AbpBlobManagement.Blob, R, admin)
AbpResourcePermissionGrants: (tenant-b, ...Blob.View, resourceKey=<blobId>, U, <tenant-b 用户Id>)
```

即：资源级授权只授予了上传者本人，而功能权限通过角色名 `admin` 被跨租户命中。

---

## 3. 复现步骤（修复前）

1. 宿主管理员（`admin` / `1q2w3E*`）通过 `/api/saas/tenants` 创建 `tenant-a`、`tenant-b`
   （各自带管理员 `admin@a.io`、`admin@b.io`）。
2. 以 tenant-b 管理员登录（`__tenant` 在 `/connect/authorize` 阶段解析），上传私有文件：

```bash
curl -X POST http://localhost:30000/api/blob-management/private/blobs/file \
  -H "Authorization: Bearer $TENANT_B_TOKEN" \
  -F "Name=tenant-b-secret.txt" -F "File=@tenant-b-secret.txt;type=text/plain"
# => fullName "/tenant-b-secret.txt", container "users" (tenant-b)
```

3. 以 **tenant-a** 管理员发起越权下载：

```bash
# 方式一：路由携带租户
curl "http://localhost:30000/api/blob-management/blobs/download/by-name/t/<tenant-b-id>/x?ContainerName=users&BlobName=/tenant-b-secret.txt" \
  -H "Authorization: Bearer $TENANT_A_TOKEN"

# 方式二：查询参数携带租户（TenantId 是输入模型的属性，与路由方式等价；
#         修复后已验证该参数仍会被绑定，只是不再参与租户切换）
curl "http://localhost:30000/api/blob-management/blobs/download/by-name/x?TenantId=<tenant-b-id>&ContainerName=users&BlobName=/tenant-b-secret.txt" \
  -H "Authorization: Bearer $TENANT_A_TOKEN"
```

修复前实测输出：

```
TOP-SECRET-OF-TENANT-B-8f3a1c
HTTP:200
```

基线对照（同一 token 走正常端点均被拒绝）：

```
GET /api/blob-management/blobs/{tenant-b-blob-id}
  => 404 不存在 id = ... 的实体 Blob！        （租户过滤器生效）

GET /api/blob-management/private/blobs/download/by-name/tenant-b-secret.txt
  => 403 BlobManagement:002004                  （查不到，正确）
```

同租户资源级鉴权绕过（也是修复前）：

```
bob（tenant-a 普通用户）上传 /bob-private.txt

bob 自己下载        private 端点  => 200 BOB-PRIVATE-FILE-2b7d9e
tenant-a 管理员     private 端点  => 403（资源级授权正确拒绝）
tenant-a 管理员     admin   端点  => 200 BOB-PRIVATE-FILE-2b7d9e   ← 绕过
```

---

## 4. 修复

| 文件 | 变更 |
| --- | --- |
| `...Application.Contracts/.../Dtos/BlobDownloadByNameInput.cs` | 移除 `TenantId` |
| `...HttpApi.Client/.../Dtos/BlobDownloadByNameInput.cs`、`framework/cli/.../Dtos/BlobDownloadByNameInput.cs` | 同步生成的 DTO |
| `...HttpApi/.../BlobController.cs` | 删除 `download/by-name/t/{tenantId}/{name}` 路由 |
| `...Application/.../BlobAppService.cs` | 去掉 `CurrentTenant.Change(input.TenantId)`；重写策略检查以同时执行功能权限 + 容器策略（资源级） |
| `...Application/.../BlobAppServiceBase.cs` | 新增 `CheckBlobTenant()`，在按名下载时校验 `blob.TenantId == CurrentTenant.Id`（纵深防御） |
| `...Domain/.../IBlobDownloadKeyGenerator.cs`、`BlobDownloadKey.cs`、`HmacBlobDownloadKeyGenerator.cs` | 新增下载密钥接口与 HMAC-SHA256 默认实现（替换原 MD5 缓存键方案） |
| `...Domain/.../BlobManager.cs` | 下载链接改用 `IBlobDownloadKeyGenerator` 生成/校验；删除 `BlobDownloadKeyCacheItem` 缓存依赖 |

### 关键代码（修复后）

```csharp
// BlobAppService
public async virtual Task<IRemoteStreamContent> DownloadByNameAsync(BlobDownloadByNameInput input)
{
    // 只能在当前租户上下文中访问, 禁止通过请求参数切换租户
    var blobContainer = await BlobContainerRepository.GetByNameAsync(
        BlobManager.GetBlobProvider(), input.ContainerName);

    return await base.DownloadByNameAsync(blobContainer, input.BlobName);
}

protected async override Task CheckGetPolicyAsync(string containerName, Blob blob)
{
    await base.CheckGetPolicyAsync(blob);                 // 功能权限 AbpBlobManagement.Blob
    await base.CheckGetPolicyAsync(containerName, blob);   // 容器策略(users -> 资源级 View)
}

protected async override Task CheckGetPolicyAsync(Blob blob)
{
    await base.CheckGetPolicyAsync(blob);
    var containerName = await BlobContainerRepository.GetNameAsync(blob.ContainerId);
    await base.CheckGetPolicyAsync(containerName, blob);
}

// BlobAppServiceBase
protected virtual void CheckBlobTenant(Blob blob)
{
    if (blob.TenantId != CurrentTenant.Id)
    {
        throw new AbpAuthorizationException(
            "The blob does not belong to the current tenant. Cross-tenant access is not allowed.");
    }
}
```

### 破坏性变更

- `BlobDownloadByNameInput.TenantId` 已删除；旧客户端传 `?TenantId=` 会被忽略（按当前租户查询）。
- `GET /api/blob-management/blobs/download/by-name/t/{tenantId}/{name}` 路由已删除（404）。
- 已有的下载链接（旧的 MD5 key）在升级后失效，需重新生成。
- 如确有平台侧跨租户运维需求，正确做法是使用 ABP 的租户解析（登录/授权阶段的 `__tenant`）
  并额外校验宿主身份与专用权限，而不是在应用服务里直接 `CurrentTenant.Change(用户输入)`。

---

## 5. 回归验证（修复后，同一环境）

| # | 场景 | 期望 | 实际 | 结果 |
| --- | --- | --- | --- | --- |
| A1 | 跨租户：`/t/{tenantId}/` 路由 | 404 | 404 | PASS |
| A2 | 跨租户：`?TenantId=` 参数 | 403 | 403 | PASS |
| B1 | 同租户：管理员按名读取他人私有文件 | 403 | 403 | PASS |
| B2 | 同租户：管理员删除他人私有文件 | 403 | 403 | PASS |
| B3 | 同租户：管理员生成他人文件下载链接 | 403 | 403 | PASS |
| R1 | 正常：用户下载自己的私有文件 | 200 | 200 | PASS |
| R2 | 正常：租户管理员下载本租户文件（admin 端点） | 200 | 200 | PASS |
| R3 | 正常：租户管理员获取自己文件元数据 | 200 | 200 | PASS |
| D1 | 正常：生成下载链接并用 key 匿名下载（租户文件） | 200 | 200 | PASS |
| D2 | 篡改 / 伪造下载 key | 拒绝 | 空内容 | PASS |

### 测试工程结构

| 工程 | 职责 | 用例数 |
| --- | --- | --- |
| `tests/LINGYUN.Abp.BlobManagement.Domain.Tests` | 领域对象、名称校验、下载密钥 | 86 |
| `tests/LINGYUN.Abp.BlobManagement.EntityFrameworkCore.Tests` | EF Core 测试宿主（InMemory）+ 仓储多租户过滤器 | 6 |
| `tests/LINGYUN.Abp.BlobManagement.Application.Tests` | 应用服务：租户隔离、资源级鉴权、下载密钥端到端 | 11 |
| `tests/LINGYUN.Abp.BlobManagement.FileSystem.Tests` | FileSystem provider 路径安全 | 现有 |

`Application.Tests` 依赖 `EntityFrameworkCore.Tests`（测试宿主复用），
EF 相关配置只在 `EntityFrameworkCore.Tests` 中维护：

```csharp
// AbpBlobManagementEntityFrameworkCoreTestsModule
context.Services.AddEntityFrameworkInMemoryDatabase();
Configure<AbpDbContextOptions>(o => o.Configure(ctx =>
    ctx.DbContextOptions.UseInMemoryDatabase(databaseName)));
Configure<AbpUnitOfWorkDefaultOptions>(o =>
    o.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled);
```

#### EF Core 仓储用例（6 个）

| 用例 | 断言 |
| --- | --- |
| `Should_Persist_And_Query_Blob_Of_Current_Tenant` | 同租户内按 FullName + ContainerId 可查到，且 TenantId 正确 |
| `Should_Filter_Blobs_By_Current_Tenant` | 两租户同 FullName 各自只查到自己的；宿主查不到 |
| `Should_Filter_Containers_By_Current_Tenant` | 同名容器按租户隔离 |
| `Should_Not_Find_Blob_Of_Another_Tenant_By_Id` | 跨租户按 Id 查询返回 null |
| `Should_Filter_Blobs_By_Container` | 同租户不同容器同名文件互不干扰 |
| `Should_Return_Null_For_Unknown_FullName` | 不存在的 FullName 返回 null |

#### 应用层用例（11 个）

- 依赖 `Application` / `EntityFrameworkCore.Tests`（EF 测试宿主）/ `FileSystem` provider / `TestBase`；
- 以 `TestAuthorizationService`（实现 `IAbpAuthorizationService`）替换真实授权服务，
  由用例分别控制**功能权限**（`AbpBlobManagement.Blob` 等）与**资源级权限**
  （`LINGYUN.Abp.BlobManagement.Blob.View/Delete` + BlobId），
  并记录被检查的策略/资源，用于断言容器级策略确实被调用；
- 11 个用例：

| 用例 | 断言 |
| --- | --- |
| `Should_Download_File_Of_Current_Tenant` | 本租户 + 功能权限 + 资源权限 → 返回文件内容 |
| `Should_Not_Download_File_Of_Another_Tenant` | 两租户存在同名容器时，授予全部权限仍无法读取他租户文件（`Blob.NameNotFound`） |
| `Should_Not_Expose_TenantId_In_Download_Input` | `BlobDownloadByNameInput` 不再包含 `TenantId`（防回归） |
| `Should_Require_Function_Permission_To_Download` | 缺少功能权限 → `AbpAuthorizationException` |
| `Should_Require_Resource_Permission_To_Download` | 有功能权限但无资源权限 → 拒绝，且容器策略被调用 |
| `Should_Require_Resource_Permission_To_Delete` | 删除他人文件 → 拒绝，且资源删除策略被调用 |
| `Should_Delete_File_With_Resource_Permission` | 有资源权限 → 删除成功 |
| `Should_Require_Resource_Permission_To_Generate_Download_Url` | 生成下载链接同样受资源级约束 |
| `Should_Download_By_Generated_Key` | 宿主上下文使用下载密钥仍可下载（租户由密钥携带） |
| `Should_Not_Download_By_Tampered_Key` | 篡改密钥 → 空内容 |
| `Should_Not_Get_Blob_Of_Another_Tenant_By_Id` | 按 id 读取他租户 Blob → `EntityNotFoundException` |

- `dotnet test tests/LINGYUN.Abp.BlobManagement.EntityFrameworkCore.Tests` → **6 passed / 0 failed**；
- `dotnet test tests/LINGYUN.Abp.BlobManagement.Application.Tests` → **11 passed / 0 failed**；
- 反向验证（mutation）：临时移除 `CheckGetPolicyAsync(string, Blob)` 中的容器策略检查后，
  `Should_Require_Resource_Permission_To_Download` 立即失败，证明用例能捕获该回归。

---

## 6. 下载密钥改造（HMAC）

### 设计

| 类型 | 位置 | 说明 |
| --- | --- | --- |
| `IBlobDownloadKeyGenerator` | `...Domain/.../IBlobDownloadKeyGenerator.cs` | 扩展点，可替换实现 |
| `BlobDownloadKey` | `...Domain/.../BlobDownloadKey.cs` | 解析结果：`BlobId` / `TenantId` / `ExpiresAt` |
| `HmacBlobDownloadKeyGenerator` | `...Domain/.../HmacBlobDownloadKeyGenerator.cs` | 默认实现，HMAC-SHA256 |

```csharp
public interface IBlobDownloadKeyGenerator
{
    string Generate(Blob blob);            // 有效期、签名密钥等全部由 Options 决定
    BlobDownloadKey? Parse(string key);
}
```

- key 作为 `download/{key}` 的路径段，使用 **Base64Url**（无 `+` `/` `=`）。
- 只接受**规范编码**：Base64Url 末尾存在未使用的填充位，非规范编码可能解码出相同字节，
  解析时会重新编码并与原文比较，不一致即拒绝（避免 key 同一性被绕过）。
- 签名校验用 `CryptographicOperations.FixedTimeEquals`，防时序侧信道。
- **时间分桶**：生成时把当前时间按 `DownloadKeyWindow` 对齐到窗口起点，
  过期时间 = 窗口结束 + `DownloadKeyLifetime`。
  - 同一窗口内重复生成得到**完全相同**的 key（URL 稳定，利于缓存/去重）；
  - 剩余有效期 ∈ `[DownloadKeyLifetime, DownloadKeyLifetime + DownloadKeyWindow)`；
  - `DownloadKeyWindow <= 0` 时退化为单次生成（不做分桶）。
- 默认值：`DownloadKeyLifetime = DownloadKeyWindow = 600s`，均可由
  `BlobManagement:DownloadKeyLifetime` / `BlobManagement:DownloadKeyWindow` 配置（`TimeSpan` 格式）。
- 签名密钥：`AbpBlobManagementOptions.DownloadKeySecret`（`BlobManagement:DownloadKeySecret`），
  未配置时回退 `StringEncryption:DefaultPassPhrase`；
  生产环境建议显式配置前者，并支持轮换（轮换后旧链接立即失效，符合短时效预期）。
- 被篡改、伪造、过期的 key 一律返回空内容，不泄露文件是否存在。

### 替换实现

```csharp
public class MyBlobDownloadKeyGenerator : IBlobDownloadKeyGenerator, ITransientDependency
{
    // 可由 Redis / KMS 等外部设施保存与校验 key
}
```

由于接口实现通过 ABP 约定注册（`ITransientDependency`），
覆盖实现时使用 `[Dependency(ReplaceServices = true)]` 或在模块中 `context.Services.Replace(...)` 即可。

### 单元测试

`tests/LINGYUN.Abp.BlobManagement.Domain.Tests/.../BlobDownloadKeyGenerator_Tests.cs`（14 个用例全部通过）：

- 租户 key / 宿主 key 的生成与解析往返；
- 有效期/窗口取自 `AbpBlobManagementOptions`（默认值与自定义值）；
- 同一窗口内多次生成得到相同的 key，跨窗口得到不同的 key；
- 篡改载荷、伪造格式、非规范 Base64Url 编码（末尾填充位被改）、
  过期 key、其它密钥签发的 key 均被拒绝；
- 生成的 key 不含需要 Url 转义的字符。

`dotnet test LINGYUN.Abp.BlobManagement.Domain.Tests`：**90 passed / 0 failed**。

---

## 7. 残留风险与后续建议

1. **列表接口的元数据**
   `GetListAsync` 只校验 `AbpBlobManagement.Blob`，会返回 `users` 容器内所有用户文件的名称/大小等元数据。
   如需严格隔离，应按资源级授权过滤。

2. **存在性探测**
   `DownloadByNameAsync` 先查文件再鉴权，「不存在」与「无权限」返回不同错误码（404/403），
   可用于探测同租户内文件是否存在。可将两种情况统一为 404。

3. **下载密钥的撤销**
   HMAC 方案是无状态的，无法单独撤销某一个 key（只能等过期或轮换密钥）。
   如需即时撤销，可在 `IBlobDownloadKeyGenerator` 的实现中加入黑名单/版本号。

4. **建议补充应用层集成测试**
   新增 `LINGYUN.Abp.BlobManagement.Application.Tests`（或 HttpApi 集成测试），
   覆盖：跨租户指定 TenantId、资源级 View/Delete、正常下载路径。
