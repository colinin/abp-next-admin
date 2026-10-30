# 审批流（基于 Elsa.UserTasks）

本目录实现一套构建在 `Elsa.UserTasks` 之上的审批流：审批任务本身由 Elsa 的用户任务持久化与鉴权体系管理，
ABP 侧只负责审批模板、审批实例、节点任务、审批记录、转办/加签、抄送与委派规则。

## 模块组成

| 项目 | 职责 |
| --- | --- |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.Domain.Shared` | 枚举、错误码、本地化资源 |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.Activities` | Elsa 活动 `ApprovalTask`、`ApprovalCc`，审批人规则解析、会签/或签/顺签、完成策略 |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.Domain` | 模板、实例、节点任务、审批记录、委派、抄送、决策处理、访问策略 |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore` | `ElsaNextApprovalFlowDbContext`、仓储、初始迁移 |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.Application` | 按实体拆分的应用服务 `ITemplateAppService` / `IInstanceAppService` / `ITaskAppService` / `IRecordAppService` / `IFormDefinitionAppService`、权限定义 |
| `LINGYUN.Abp.ElsaNext.ApprovalFlow.HttpApi` | `TemplateController` / `InstanceController` / `TaskController` / `RecordController` / `FormDefinitionController` |

## 工作原理

1. 发起：`IApprovalInstanceStarter.StartAsync` 写入 `ApprovalInstance`，随后按模板绑定的 Elsa 定义派发工作流，输入包含
   `ApprovalInstanceId`、`ApprovalInitiator`、`ApprovalBusinessKey`、`ApprovalFormData`。
2. 节点：工作流中的 `ApprovalTask` 活动解析审批人，内部会为每位审批人创建 Elsa `UserTask`（`TaskType = "approval"`），
   并按 `SignMode`（`Any` / `All` / `Sequential`）等待决策。
3. 映射：`UserTaskCreated` 通知由 `UserTaskCreatedHandler` 处理，按用户任务标签中的节点实例标识登记 `ApprovalNodeTask`。
4. 决策：`IApprovalDecisionHandler.DecideAsync` 通过 `IUserTaskManager.CompleteAsync` 完成 Elsa 用户任务，
   Elsa 恢复工作流；同时写入 `ApprovalRecord`。
5. 终态：`WorkflowFinishedHandler` 监听 Elsa 工作流结束事件，按工作流输出 `ApprovalOutcome`（缺省视为通过）把实例置为通过/拒绝/退回/撤回/取消；撤回与退回也可通过 `ApprovalInstanceTerminationHandler` 直接结束实例。
6. 鉴权：`ApprovalFlowUserTaskAccessPolicy` 替换 Elsa 默认策略，补充“被委派人可代办”的语义。

## 工作流约定（重要）

`ApprovalTask` 依赖以下约定才能把 Elsa 用户任务与审批实例关联起来：

- `Reference` 必须绑定为 `inputs.ApprovalInstanceId`（发起审批时写入的工作流输入）。
  未绑定时 `UserTaskCreatedHandler` 无法登记节点任务，审批决策会返回“没有待办”。
- `Title` 一般绑定 `inputs.ApprovalTitle` 或实例标题。
- `Actions` 缺省为 `Approve`（同意）/ `Reject`（拒绝），可通过 `Actions` 输入覆盖。
- `ApproverRules` 支持 `Participant`、`Initiator`、`InitiatorSelection`、`FormField` 四类规则；
  角色、部门等规则可通过实现 `IApproverRuleResolver` 扩展。

## HTTP API

| 方法 | 路径 | 说明 |
| --- | --- | --- |
| GET | `api/approval-flow/templates` | 模板列表 |
| POST | `api/approval-flow/templates` | 创建模板 |
| PUT | `api/approval-flow/templates/{id}` | 修改模板 |
| POST | `api/approval-flow/templates/{id}/publish` | 发布模板（后端按设计自动生成 Elsa 定义） |
| DELETE | `api/approval-flow/templates/{id}` | 删除模板 |
| POST | `api/approval-flow/instances` | 发起审批 |
| GET | `api/approval-flow/instances` | 我的审批（可切换全部） |
| GET | `api/approval-flow/instances/{id}` | 审批详情 |
| GET | `api/approval-flow/instances/{id}/records` | 审批记录 |
| GET | `api/approval-flow/tasks` | 我的待办（数据来自 Elsa 用户任务） |
| POST | `api/approval-flow/instances/{id}/decide` | 同意/拒绝 |
| POST | `api/approval-flow/instances/{id}/withdraw` | 撤回 |

审批人的待办也可以直接使用 Elsa 自带的 `/elsa/api/user-tasks` 接口与 Studio 的 User Tasks 界面。

## 配置

```json
"WorkflowService": {
  "EfCore": {
    "AutoMigrate": true
  }
},
"Elsa": {
  "ApprovalFlow": {
    "MaxApproversPerNode": 200,
    "RequireCommentOnReject": true,
    "DiscloseApprovers": true
  }
}
```

- `AutoMigrate`（宿主配置 `WorkflowService:EfCore:AutoMigrate`）：服务启动时由宿主迁移模块
  `WorkflowServiceMigrationsEntityFrameworkCoreModule` 自动执行待应用迁移。
- 数据库表前缀为 `EX_ApprovalFlow`，可通过 `ElsaNextApprovalFlowDbProperties.DbTablePrefix` 调整。

## 数据库

审批流、表单、部门责任人的迁移统一放在宿主
`LINGYUN.Abp.MicroService.WorkflowService.EntityFrameworkCore`（`WorkflowServiceMigrationsDbContext`），
业务模块的 DbContext 只负责读写，不携带迁移。也可以手动执行：

```bash
dotnet ef database update \
  --project aspire/LINGYUN.Abp.MicroService.WorkflowService.EntityFrameworkCore \
  --startup-project aspire/LINGYUN.Abp.MicroService.WorkflowService.EntityFrameworkCore
```

## 权限

- `ApprovalFlow.Templates`（+ `.Create` / `.Update` / `.Delete` / `.Publish`）
- `ApprovalFlow.Instances`（+ `.Create` / `.Decide` / `.Withdraw` / `.Manage`）

## 表单与人员部门支持

审批流本身不直接依赖 ABP Identity 或具体表单实现，二者都以接口/独立模块接入：

- 表单：表单定义（多版本、字段、发布）与实例表单数据已合并进 `LINGYUN.Abp.ElsaNext.ApprovalFlow.*`；
  `FormDefinitionUserTaskFormProvider` 实现 Elsa 的 `IUserTaskFormProvider`（ProviderName = `form-definitions`）。
  发起审批时，若模板绑定了表单，`ApprovalInstanceStarter` 会向工作流输入 `ApprovalFormReference`；
  工作流中的 `ApprovalTask.Form` 绑定 `inputs.ApprovalFormReference` 即可启用表单渲染与提交校验。
  表单 API：`/api/form-definitions`（含字段维护、发布、实例数据读写）。
- 人员/部门：`LINGYUN.Abp.ElsaNext.ApprovalFlow.Domain` 定义 `IOrganizationHierarchy`、`OrganizationUnitSupervisor`
  等契约，Identity 相关实现位于 `LINGYUN.Abp.ElsaNext.ApprovalFlow.Domain.Identity`；
  该模块用 `ElsaOrganizationParticipantDirectory` 替换 Elsa 默认的参与者目录，并按用户/部门检索、展开成员；
  `OrganizationApproverRuleResolver` 支持“部门成员”“部门主管”规则。

## vben5 设计器

`apps/vben5/packages/@abp/approval-flow`（ant-design-vue）提供全屏四步向导（AntFlow-Designer 风格）：

- `ApprovalFlowWizard`：基础设置 → 表单设计 → 流程设计 → 发布；全屏展示，仅通过顶部「取消」或最后一步「确认发布/完成」关闭；
- `FormDesigner`：控件面板（11 种控件）拖动到画布，字段属性（键、必填、权限 R/E/H、选项）编辑；
- `ApprovalFlowDesigner`：浅灰画布 + 纵向节点卡片 + 节点配置抽屉 + 缩放工具，节点间通过蓝色「＋」添加；
  支持审批人/抄送人/条件节点、或签/会签/顺签、审批人规则（指定人员/部门/角色、发起人、发起人自选、部门主管、部门成员、表单字段）；
- `ApprovalTemplateTable`：模板列表，新建/设计/发布均打开向导；已发布模板可直接「发起」；
- `StartApprovalModal`：按模板表单动态渲染，提交后调用 `POST /instances` 发起审批；
- `MyApprovalTable`：待办（同意/拒绝/查看）、我发起的、审批记录；
- `ApprovalInstanceDetailModal` + `ApprovalFlowViewer`：只读流程渲染与执行记录，节点颜色：通过=绿、拒绝=红、进行中=蓝、未执行=灰。

对应页面位于 `apps/app-antd/src/views/approval-flow/*`，表单管理页面为
`apps/app-antd/src/views/form-definitions/index.vue`（`@abp/form-definitions`）。
> 完整设计说明与流转图：`docs/approval-flow/README.md`（lifecycle.svg / sequence.svg）。
