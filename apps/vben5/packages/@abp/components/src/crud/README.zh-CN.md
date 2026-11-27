# CRUD

[English](./README.md) | 简体中文

`@abp/components/crud` 提供基于 ABP 的通用 CRUD 能力：

- `useAbpDataTable` / `AbpDataTable`：ABP 分页、排序、查询、删除确认、操作列和批量选择；
- `useAbpEditorModal` / `AbpEditorModal`：基于 Vben Modal + Vben Form 的新增/编辑/查看弹窗；
- 类型工具：`AbpIdOf`、`AbpDataTableOptions`、`AbpEditorModalOptions` 等。

## 导入

```ts
import { useAbpEditorModal } from '@abp/components/crud/modal';
import { useAbpDataTable } from '@abp/components/crud/table';
```

组件按模块子路径导出：

- `@abp/components/crud/table`：`useAbpDataTable` 及表格类型；
- `@abp/components/crud/modal`：`useAbpEditorModal` 及弹窗类型；
- `@abp/components/crud/drawer`：`useAbpEditorDrawer` 及抽屉类型；
- `@abp/components/crud`：兼容入口，统一再导出两个模块以及共享的 `AbpRecord` / `AbpId` / `AbpIdOf` 类型。

## 快速开始

```ts
import type { BookDto, CreateUpdateBookDto } from './types';

import { useAbpEditorModal } from '@abp/components/crud/modal';
import { useAbpDataTable } from '@abp/components/crud/table';
import { useBooksApi } from './useBooksApi';

const {
  createApi,
  deleteApi,
  deleteManyApi,
  getApi,
  getPagedListApi,
  updateApi,
} = useBooksApi();

const [Table, tableApi] = useAbpDataTable<BookDto>({
  edit: (row) => {
    void modalApi.open(row);
  },
  formOptions: {
    schema: [
      {
        component: 'Input',
        fieldName: 'filter',
        label: '搜索',
      },
    ],
  },
  gridOptions: {
    columns: [
      { field: 'name', title: '名称', sortable: true },
      { field: 'price', title: '单价', sortable: true },
    ],
  },
  load: ({ pager, searchValues, sorting }) =>
    getPagedListApi({
      maxResultCount: pager.maxResultCount,
      skipCount: pager.skipCount,
      sorting,
      ...searchValues,
    }),
  delete: (dto) => deleteApi(dto.id),
  deleteMany: deleteManyApi,
  idKey: (dto) => dto.id,
});

const [Modal, modalApi] = useAbpEditorModal<BookDto, CreateUpdateBookDto>({
  create: createApi,
  update: updateApi,
  get: getApi,
  idKey: (dto) => dto.id,
  titleKey: (dto) => dto.name,
  refresh: () => tableApi.refresh(),
  formOptions: {
    schema: [
      { component: 'Input', fieldName: 'name', label: '名称' },
      { component: 'InputNumber', fieldName: 'price', label: '单价' },
    ],
  },
});
```

```vue
<template>
  <Table>
    <template #toolbar-tools>
      <Button type="primary" @click="modalApi.create()">新增</Button>
    </template>
  </Table>

  <Modal />
</template>
```

## `useAbpDataTable`

### load 参数

```ts
interface AbpDataTableLoadParams {
  pager: {
    maxResultCount: number;
    skipCount: number;
  };
  searchValues: Record<string, any>;
  sorting?: string; // 例如 "name asc"
}
```

默认响应格式：

```ts
{
  items: BookDto[];
  totalCount: number;
}
```

也兼容 ABP 的 `PagedResultDto`。

### 响应映射与前端分页

字段名不一致时可以通过 `response` 配置：

```ts
useAbpDataTable<BookDto>({
  load,
  response: {
    list: 'result.items',
    total: 'result.totalCount',
  },
});
```

如果 `total` 缺失、为空或非法，会使用前端分页：

- `totalCount` 使用 `items.length`；
- 按当前页从返回数组中切片；
- 如果 `pagerConfig.enabled` 为 `false`，则直接返回全部数据。

### 配置项

| 配置 | 说明 |
| --- | --- |
| `load` | 必填，加载数据 |
| `delete` | 单条删除，接收 DTO |
| `deleteMany` | 批量删除，接收 id 数组 |
| `rowKey` | vxe-table 行主键字段，默认 `id`；自定义主键字段时使用 |
| `deleteWarnMessage` | 自定义删除确认文案 |
| `response` | 自定义列表/总数字段路径 |
| `edit` | 默认编辑操作回调，通常调用 `modalApi.open(row)` |
| `editAuth` / `deleteAuth` | 操作按钮权限码 |
| `editText` / `deleteText` | 覆盖编辑/删除按钮文案 |
| `actions` | 追加 `VbenTableAction` 操作项 |
| `dropdownActions` | 追加“更多”下拉操作 |
| `actionColumn` | 操作列标题/宽度；传 `false` 关闭默认操作列 |
| `formOptions` | 搜索表单配置 |
| `gridOptions` | vxe-grid 配置，`columns` 会与自动操作列合并 |
| `gridEvents` | 带类型的 vxe-grid 事件；使用 checkbox 列时支持 `onCheckboxChange(rows)` |
| `tableTitle` / `tableTitleHelp` | 表格标题和帮助 |
| `hasPermission` | 操作列权限判断 |

### 操作列

未设置 `actionColumn: false` 时，会自动追加操作列：

- 字段：`action`
- `fixed: 'right'`
- 默认 `width` / `minWidth`：`150`
- 使用 `VbenTableAction` 渲染

默认操作：

- 提供 `edit` 时显示编辑；
- 提供 `delete` 时显示删除。

自定义操作通过 `actions(row)` 追加。

### tableApi

```ts
const [Table, tableApi] = useAbpDataTable<BookDto>({ ... });
```

除了 vben vxe-grid 原有 API，还新增：

| 方法              | 说明                   |
| ----------------- | ---------------------- |
| `refresh()`       | 刷新当前查询           |
| `delete(dto)`     | 确认并删除单条         |
| `deleteMany(ids)` | 确认并批量删除         |
| `edit(dto)`       | 调用配置的编辑回调     |
| `query()`         | 执行底层 vxe-grid 查询 |

表格数据变化时会清空选择；`delete` / `deleteMany` 执行后也会清空选择。

### 插槽

`AbpDataTable` 保留 Grid 的插槽类型，包括：

- `#toolbar-tools`
- `#toolbar-actions`
- `#action`
- vxe-grid 的列插槽

```vue
<Table>
  <template #action="{ row }">
    <Button @click="modalApi.open(row)">编辑</Button>
  </template>
</Table>
```

## `useAbpEditorModal`

### 模式

```ts
modalApi.open(); // 新增
modalApi.open(dto); // 编辑
modalApi.open(dto, 'view'); // 查看
modalApi.open(id); // 按 id 编辑，需要 get()
modalApi.create(); // 新增
modalApi.update(dto); // 编辑
modalApi.view(dto); // 查看
```

按 id 打开时，必须提供 `get(id)`。

### 配置项

| 配置            | 说明                                            |
| --------------- | ----------------------------------------------- |
| `formOptions`   | 必填，Vben Form 配置                            |
| `create`        | 新增 API，可选                                  |
| `update`        | 修改 API，接收 `idKey(dto)` 和表单值            |
| `get`           | 按 id 获取 DTO，按 id 打开时必填                |
| `refresh`       | 保存成功后刷新回调，通常为 `tableApi.refresh()` |
| `titleKey`      | 编辑/查看标题中的业务值                         |
| `idKey`         | 必填，返回 DTO 主键                             |
| `modalOptions`  | 透传 Vben Modal 配置                            |
| `onOpen`        | 弹窗打开、应用表单值前调用，可用于动态 schema   |
| `onOpenChange`  | 弹窗打开状态回调                                |
| `defaultValues` | 新增默认值                                      |
| `title`         | 完全自定义标题                                  |
| `description`   | 弹窗描述                                        |
| `onSuccess`     | 保存成功回调                                    |

### Form 插槽

以 `form-` 开头的插槽会透传给内部 Form：

```vue
<Modal>
  <template #form-name="{ modelValue, componentField }">
    <Input v-bind="componentField" />
  </template>

  <template #form-property-changes="{ values }">
    {{ values.propertyChanges?.length }}
  </template>

  <template #form-submit-before="{ formApi, values }">
    <!-- 提交按钮前的内容 -->
  </template>

  <template #footer>
    <!-- 普通 Modal footer 插槽 -->
  </template>
</Modal>
```

规则：

- `#form-{field}` 映射到 Form 的 `{field}` 插槽；
- 支持 kebab-case：`#form-property-changes` 映射到 `propertyChanges`；
- `#form-default`、`#form-submit-before`、`#form-reset-before` 等映射到 Form 对应动作插槽；
- 不带 `form-` 的插槽传给 Modal（`#footer`、`#title` 等）。

Form 插槽参数类型由 `VbenFormSlots<TFormValues>` 推导。

### Tabs 分标签卡表单

在 `formOptions` 中提供 `tabs`，并通过 schema 的 `tabKey` 指定字段所属 Tab：

```ts
const [Modal] = useAbpEditorModal<BookDto, BookForm>({
  idKey: (dto) => dto.id,
  formOptions: {
    tabs: [
      { key: 'basic', title: '基本信息' },
      { key: 'advanced', title: '高级设置' },
    ],
    schema: [
      { component: 'Input', fieldName: 'name', label: '名称', tabKey: 'basic' },
      {
        component: 'Input',
        fieldName: 'description',
        label: '描述',
        tabKey: 'advanced',
      },
    ],
  },
});
```

- Tab 使用 `@vben-core/shadcn-ui` 渲染。
- `key` 必须唯一，并与 `schema.tabKey` 对应。
- 未设置 `tabKey` 的字段会放入第一个 Tab。
- 所有 Tab 共用一个 Vben Form，字段始终保持挂载，提交时统一校验。
- 切换 Tab 只隐藏不属于当前 Tab 的字段。

### 抽屉编辑器

`useAbpEditorDrawer` 与 `useAbpEditorModal` 能力一致，只是容器换成 Vben Drawer：

```ts
import { useAbpEditorDrawer } from '@abp/components/crud/drawer';

const [Drawer, drawerApi] = useAbpEditorDrawer<BookDto, BookForm>({
  idKey: (dto) => dto.id,
  drawerOptions: { placement: 'right' },
  formOptions: { schema: [] },
  get: getApi,
  create: createApi,
  update: updateApi,
});
```

同样支持 `formOptions.tabs`、`schema.tabKey`、`#form-*` 插槽、新增/编辑/查看以及未保存变更确认；配置项使用 `drawerOptions`。

### 未保存变更提示

表单值与打开时记录的快照不一致时，关闭弹窗会弹出确认：

- 仅新增/编辑模式检查；
- 查看模式不提示；
- 保存成功后更新快照并关闭，不会重复提示；
- 切换 keep-alive 页签不会提示。

默认文案来自 `crud.leaveConfirmTitle` 和 `crud.unsavedChanges`。

### 表单值与 codec

`onSubmit(values)` 中的 `values` 已经是 Vben Form 经过 codec 处理的提交值，因此 `create` / `update` 直接使用即可，不需要在 `onSubmit` 中再次调用 `formApi.getValues()`。

未保存变更比较的初始快照和当前值都通过 `formApi.getValues()` 获取，保证比较双方使用相同的 codec 结果。

## 本地化

CRUD 默认文案使用 `@abp/components` 的 `crud` 命名空间：

| Key                          | 用途           |
| ---------------------------- | -------------- |
| `crud.actions`               | 操作列标题     |
| `crud.create`                | 新增标题       |
| `crud.edit`                  | 编辑标题/操作  |
| `crud.view`                  | 查看标题       |
| `crud.delete`                | 删除操作       |
| `crud.saveSuccess`           | 保存成功       |
| `crud.deleteSuccess`         | 删除成功       |
| `crud.confirmTitle`          | 确认标题       |
| `crud.deleteWarnMessage`     | 单条删除提示   |
| `crud.deleteManyWarnMessage` | 批量删除提示   |
| `crud.leaveConfirmTitle`     | 未保存提示标题 |
| `crud.unsavedChanges`        | 未保存提示内容 |

应用需要通过 `loadComponentMessages` 加载 `@abp/components/locales`。

## 类型

```ts
export type AbpId = number | string;

export type AbpIdOf<TDto extends AbpRecord> = TDto extends { id: infer TId }
  ? TId
  : AbpId;
```

`AbpIdOf` 会从 `TDto.id` 推导主键类型；DTO 没有 `id` 时回退为 `number | string`。

`AbpDataTableOptions`、`AbpDataTableApi`、`AbpEditorModalOptions`、`AbpEditorModalApi` 均已导出，可用于二次封装。
