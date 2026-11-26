# CRUD

English | [简体中文](./README.zh-CN.md)

`@abp/components/crud` provides reusable CRUD primitives for ABP-based applications:

- `useAbpDataTable` / `AbpDataTable`: ABP paging, sorting, search, delete confirmation, action column and bulk selection.
- `useAbpEditorModal` / `AbpEditorModal`: create/edit/view modal built on Vben Modal and Vben Form.
- Typed helpers: `AbpIdOf`, `AbpDataTableOptions`, `AbpEditorModalOptions`, etc.

## Import

```ts
import { useAbpDataTable, useAbpEditorModal } from '@abp/components/crud';
```

The package is exported through the `./crud` subpath of `@abp/components`.

## Quick start

```ts
import type { BookDto, CreateUpdateBookDto } from './types';

import { useAbpDataTable, useAbpEditorModal } from '@abp/components/crud';
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
        label: 'Search',
      },
    ],
  },
  gridOptions: {
    columns: [
      { field: 'name', title: 'Name', sortable: true },
      { field: 'price', title: 'Price', sortable: true },
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
      { component: 'Input', fieldName: 'name', label: 'Name' },
      { component: 'InputNumber', fieldName: 'price', label: 'Price' },
    ],
  },
});
```

```vue
<template>
  <Table>
    <template #toolbar-tools>
      <Button type="primary" @click="modalApi.create()">New</Button>
    </template>
  </Table>

  <Modal />
</template>
```

## `useAbpDataTable`

### Load contract

`load` is called with ABP-compatible parameters:

```ts
interface AbpDataTableLoadParams {
  pager: {
    maxResultCount: number;
    skipCount: number;
  };
  searchValues: Record<string, any>;
  sorting?: string; // e.g. "name asc"
}
```

The default response convention is:

```ts
{
  items: BookDto[];
  totalCount: number;
}
```

`load` may also return `{ totalCount, items }` from an ABP `PagedResultDto`.

### Response mapping and frontend paging

Use `response` when the API returns different field names:

```ts
useAbpDataTable<BookDto>({
  load,
  response: {
    list: 'result.items',
    total: 'result.totalCount',
  },
});
```

If `total` is missing, empty or invalid, the table falls back to **frontend pagination**:

- `totalCount` becomes `items.length`;
- the current page is sliced from the returned array;
- if `pagerConfig.enabled` is `false`, all items are returned.

### Options

| Option | Description |
| --- | --- |
| `load` | Required. Loads rows using `pager`, `searchValues`, `sorting`. |
| `delete` | Delete one row. Receives the DTO. |
| `deleteMany` | Delete multiple rows by id. |
| `idKey` | Returns the primary key from a DTO. Defaults to `dto.id`. |
| `deleteWarnMessage` | Custom delete confirmation text. |
| `response` | Custom list/total paths. |
| `edit` | Called by the default edit action. Usually `modalApi.open(row)`. |
| `editAuth` / `deleteAuth` | Permission codes passed to `VbenTableAction`. |
| `editText` / `deleteText` | Override action text. |
| `actions` | Extra `VbenTableAction` items for each row. |
| `dropdownActions` | Extra items in the "more" dropdown. |
| `actionColumn` | Action column title/width, or `false` to disable the default action column. |
| `formOptions` | Search form options. |
| `gridOptions` | vxe-grid options. `columns` are merged with the generated action column. |
| `gridEvents` | vxe-grid events. |
| `tableTitle` / `tableTitleHelp` | Table title and help text. |
| `hasPermission` | Permission checker used by the action column. |

### Action column

When `actionColumn` is not `false`, an action column is appended:

- field: `action`
- fixed to the right
- `width` / `minWidth`: `150` by default
- rendered with `VbenTableAction`

The default actions are:

- **Edit** when `edit` is provided;
- **Delete** when `delete` is provided.

Custom actions are appended through `actions(row)`.

### Table API

```ts
const [Table, tableApi] = useAbpDataTable<BookDto>({ ... });
```

`tableApi` extends the Vben vxe-grid API and adds:

| Method            | Description                              |
| ----------------- | ---------------------------------------- |
| `refresh()`       | Reload the current query.                |
| `delete(dto)`     | Confirm and delete one row.              |
| `deleteMany(ids)` | Confirm and delete multiple rows.        |
| `edit(dto)`       | Invoke the configured `edit` callback.   |
| `query()`         | Run the underlying vxe-grid proxy query. |

### Slots

`AbpDataTable` keeps the Grid slot types, including:

- `#toolbar-tools`
- `#toolbar-actions`
- `#action`
- all vxe-grid column slots

```vue
<Table>
  <template #action="{ row }">
    <Button @click="modalApi.open(row)">Edit</Button>
  </template>
</Table>
```

## `useAbpEditorModal`

### Modes

```ts
modalApi.open(); // create
modalApi.open(dto); // edit
modalApi.open(dto, 'view'); // view
modalApi.open(id); // edit by id, requires get()
modalApi.create(); // create
modalApi.update(dto); // edit
modalApi.view(dto); // view
```

Opening by id requires a `get(id)` function.

### Options

| Option | Description |
| --- | --- |
| `formOptions` | Required Vben Form options. |
| `create` | Create API. Optional. |
| `update` | Update API, receives `idKey(dto)` and form values. Optional. |
| `get` | Get DTO by id. Required for opening by id. |
| `refresh` | Called after a successful save. Usually `tableApi.refresh()`. |
| `titleKey` | Business value used in edit/view titles. |
| `idKey` | Required. Returns the primary key from a DTO. |
| `modalOptions` | Extra Vben Modal options. |
| `onOpen` | Called after the modal opens, before form values are applied. Useful for dynamic schemas. |
| `onOpenChange` | Modal open state callback. |
| `defaultValues` | Default values for create mode. |
| `title` | Fully custom title. |
| `description` | Modal description. |
| `onSuccess` | Called after a successful save. |

### Form slots

Slots starting with `form-` are forwarded to the internal Form:

```vue
<Modal>
  <template #form-name="{ modelValue, componentField }">
    <Input v-bind="componentField" />
  </template>

  <template #form-property-changes="{ values }">
    {{ values.propertyChanges?.length }}
  </template>

  <template #form-submit-before="{ formApi, values }">
    <!-- rendered before submit actions -->
  </template>

  <template #footer>
    <!-- normal Modal footer slot -->
  </template>
</Modal>
```

Rules:

- `#form-{field}` → Form slot `{field}`;
- kebab-case is supported: `#form-property-changes` → `propertyChanges`;
- `#form-default`, `#form-submit-before`, `#form-reset-before`, etc. map to the matching Form action slot;
- slots without `form-` are passed to Modal (`#footer`, `#title`, ...).

Form slot props are typed from `VbenFormSlots<TFormValues>`.

### Unsaved changes

When the form value differs from the value captured on open, closing the modal asks for confirmation:

- only create/edit modes are checked;
- view mode does not prompt;
- saving successfully updates the baseline and closes without prompting;
- switching keep-alive tabs does not prompt.

The default texts come from `crud.leaveConfirmTitle` and `crud.unsavedChanges`.

### Form values and codec

`onSubmit(values)` receives the formatted submit values from Vben Form, so `options.create` / `options.update` receive codec-encoded data. Do not call `formApi.getValues()` again in `onSubmit`.

The baseline and current values used for unsaved-change detection are also read through `formApi.getValues()` so both sides use the same codec result.

## Localization

CRUD defaults use the `crud` namespace from `@abp/components` locales:

| Key                          | Purpose                 |
| ---------------------------- | ----------------------- |
| `crud.actions`               | Action column title     |
| `crud.create`                | Create title            |
| `crud.edit`                  | Edit title/action       |
| `crud.view`                  | View title              |
| `crud.delete`                | Delete action           |
| `crud.saveSuccess`           | Save success message    |
| `crud.deleteSuccess`         | Delete success message  |
| `crud.confirmTitle`          | Confirm title           |
| `crud.deleteWarnMessage`     | Single delete message   |
| `crud.deleteManyWarnMessage` | Bulk delete message     |
| `crud.leaveConfirmTitle`     | Unsaved changes title   |
| `crud.unsavedChanges`        | Unsaved changes message |

The application must load `@abp/components/locales` through `loadComponentMessages`.

## Types

```ts
export type AbpId = number | string;

export type AbpIdOf<TDto extends AbpRecord> = TDto extends { id: infer TId }
  ? TId
  : AbpId;
```

`AbpIdOf` infers the id type from `TDto.id`. If the DTO has no `id`, it falls back to `number | string`.

`AbpDataTableOptions`, `AbpDataTableApi`, `AbpEditorModalOptions` and `AbpEditorModalApi` are exported for reuse in wrappers.
