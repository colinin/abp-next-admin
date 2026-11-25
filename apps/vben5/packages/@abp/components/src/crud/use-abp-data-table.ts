import type {
  ActionItem,
  BaseFormComponentType,
  FormValues,
} from '@vben/common-ui';

import type {
  AbpDataTableApi,
  AbpDataTableLoadParams,
  AbpDataTableOptions,
  AbpIdOf,
} from './types';

import { defineComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { VbenTableAction } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { message, Modal } from 'ant-design-vue';

function getByPath(source: any, path?: string) {
  if (!path) {
    return undefined;
  }
  let target = source;
  for (const key of path.split('.')) {
    target = target?.[key];
  }
  return target;
}

/**
 * 基于 vben vxe-table 的 ABP 数据表格。
 */
export function useAbpDataTable<
  TDto extends Record<string, any> = any,
  TFormValues extends FormValues = FormValues,
>(options: AbpDataTableOptions<TDto, TFormValues>) {
  const { hasAccessByCodes } = useAccess();
  const {
    actionColumn,
    actions,
    delete: deleteApi,
    deleteMany: deleteManyApi,
    deleteAuth,
    deleteText,
    deleteWarnMessage,
    dropdownActions,
    edit,
    editAuth,
    editText,
    idKey,
    load,
    response,
    ...gridProps
  } = options;

  const getId = (idKey ?? ((dto: TDto) => dto.id)) as (
    dto: TDto,
  ) => AbpIdOf<TDto>;

  const listPath = response?.list ?? 'items';
  const totalPath = response?.total ?? 'totalCount';
  const gridOptions: any = gridProps.gridOptions ?? {};
  const pagerEnabled = gridOptions.pagerConfig?.enabled !== false;

  async function query(params: any, formValues: TFormValues) {
    const page = params?.page ?? { currentPage: 1, pageSize: 10 };
    const pager: AbpDataTableLoadParams['pager'] = {
      maxResultCount: page.pageSize,
      skipCount: (page.currentPage - 1) * page.pageSize,
    };
    const sorting = params?.sort?.order
      ? `${params.sort.field} ${params.sort.order}`
      : undefined;

    const result = await load({
      pager,
      searchValues: (formValues ?? {}) as Record<string, any>,
      sorting,
    });

    const rawList = getByPath(result, listPath);
    const items = Array.isArray(rawList) ? rawList : [];
    const rawTotal = getByPath(result, totalPath);
    const total = Number(rawTotal);
    const totalValid =
      rawTotal !== undefined &&
      rawTotal !== null &&
      rawTotal !== '' &&
      Number.isFinite(total) &&
      total >= 0;

    if (totalValid) {
      return { items, totalCount: total };
    }

    // 总数为空/非法：前端分页
    if (!pagerEnabled) {
      return { items, totalCount: items.length };
    }
    return {
      items: items.slice(
        pager.skipCount,
        pager.skipCount + pager.maxResultCount,
      ),
      totalCount: items.length,
    };
  }

  const columns = [...(gridOptions.columns ?? [])];
  if (
    actionColumn !== false &&
    !columns.some((column: any) => column.field === 'action')
  ) {
    columns.push({
      align: 'center',
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: actionColumn?.title ?? $t('crud.actions'),
      width: actionColumn?.width ?? 150,
    });
  }

  const [Grid, gridApi] = useVbenVxeGrid<
    TDto,
    BaseFormComponentType,
    Record<string, never>,
    TFormValues
  >({
    ...gridProps,
    gridOptions: {
      ...gridOptions,
      columns,
      pagerConfig: {
        pageSize: 10,
        pageSizes: [10, 25, 50, 100],
        ...gridOptions.pagerConfig,
      },
      proxyConfig: {
        ...gridOptions.proxyConfig,
        autoLoad: gridOptions.proxyConfig?.autoLoad ?? true,
        ajax: {
          ...gridOptions.proxyConfig?.ajax,
          query,
        },
        response: {
          ...gridOptions.proxyConfig?.response,
          list: 'items',
          total: 'totalCount',
        },
      },
    },
  });

  async function refresh() {
    await gridApi.query();
  }

  async function remove(dto: TDto) {
    if (!deleteApi) {
      return;
    }
    const content = deleteWarnMessage
      ? deleteWarnMessage(dto)
      : $t('crud.deleteWarnMessage', [getId(dto)]);
    Modal.confirm({
      centered: true,
      content,
      onOk: async () => {
        await deleteApi(dto);
        message.success($t('crud.deleteSuccess'));
        await refresh();
      },
      title: $t('crud.confirmTitle'),
    });
  }

  async function removeMany(ids: AbpIdOf<TDto>[]) {
    if (!deleteManyApi) {
      return;
    }
    Modal.confirm({
      centered: true,
      content: $t('crud.deleteManyWarnMessage'),
      onOk: async () => {
        await deleteManyApi(ids);
        message.success($t('crud.deleteSuccess'));
        (gridApi.grid as any)?.clearCheckboxRow?.();
        await refresh();
      },
      title: $t('crud.confirmTitle'),
    });
  }

  const tableApi = gridApi as unknown as AbpDataTableApi<TDto>;
  tableApi.delete = remove;
  tableApi.deleteMany = removeMany;
  tableApi.refresh = refresh;
  tableApi.edit = (dto: TDto) => edit?.(dto);

  function getActions(row: TDto): ActionItem[] {
    const items = actions?.(row) ?? [];
    if (edit) {
      items.push({
        key: 'edit',
        auth: editAuth,
        onClick: () => edit(row),
        icon: createIconifyIcon('ant-design:edit-outlined'),
        text: editText ?? $t('crud.edit'),
      });
    }
    if (deleteApi) {
      items.push({
        key: 'delete',
        auth: deleteAuth,
        danger: true,
        onClick: () => {
          void tableApi.delete(row);
        },
        icon: createIconifyIcon('ant-design:delete-outlined'),
        text: deleteText ?? $t('crud.delete'),
      });
    }
    return items;
  }

  function getDropdownActions(row: TDto): ActionItem[] {
    return dropdownActions?.(row) ?? [];
  }

  const Table = defineComponent({
    name: 'AbpDataTable',
    inheritAttrs: false,
    setup(_, { attrs, slots }) {
      return () =>
        h(
          Grid as any,
          attrs as any,
          {
            ...slots,
            action:
              slots.action ??
              ((params: any) =>
                h(VbenTableAction, {
                  actions: getActions(params.row as TDto),
                  dropdownActions: getDropdownActions(params.row as TDto),
                  align: 'center',
                  hasPermission(auth) {
                    if (!auth) {
                      return true;
                    }
                    return hasAccessByCodes(Array.isArray(auth) ? auth : [auth]);
                  },
                })),
          } as any,
        );
    },
  });

  return [Table, tableApi] as const;
}

export type UseAbpDataTable = typeof useAbpDataTable;
