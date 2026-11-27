import { beforeEach, describe, expect, it, vi } from 'vitest';

import { useAbpDataTable } from '../use-abp-data-table';

interface Dto {
  id: string;
  name: string;
}

const state = vi.hoisted(() => ({
  confirm: vi.fn(async (_options?: any) => undefined),
  gridApi: null as any,
  gridOptions: null as any,
  load: vi.fn(),
  toastSuccess: vi.fn(),
  deleteApi: vi.fn(),
}));

vi.mock('@vben/common-ui', () => ({
  confirm: state.confirm,
  VbenTableAction: { name: 'VbenTableAction', render: () => null },
}));

vi.mock('@vben/access', () => ({
  useAccess: () => ({ hasAccessByCodes: () => true }),
}));

vi.mock('@vben/icons', () => ({
  createIconifyIcon: vi.fn(() => 'icon'),
}));

vi.mock('@vben/locales', () => ({
  $t: (key: string) => key,
}));

vi.mock('@abp/core', () => ({
  useNotifications: () => ({ toastSuccess: state.toastSuccess }),
}));

vi.mock('@abp/ui', () => ({
  useVbenVxeGrid: vi.fn((options: any) => {
    state.gridOptions = options.gridOptions;
    const api = {
      grid: { clearCheckboxRow: vi.fn() },
      query: vi.fn(async () => undefined),
    };
    state.gridApi = api;
    return [{ name: 'GridMock', render: () => null }, api];
  }),
}));

describe('useAbpDataTable', () => {
  beforeEach(() => {
    state.confirm.mockClear();
    state.load.mockReset();
    state.toastSuccess.mockClear();
    state.deleteApi.mockReset();
    state.gridApi = null;
    state.gridOptions = null;
  });

  it('appends a fixed action column with the default width', () => {
    useAbpDataTable<Dto>({
      gridOptions: {
        columns: [{ field: 'name', title: 'Name' }],
      },
      load: state.load,
    });

    expect(state.gridOptions.columns).toHaveLength(2);
    expect(state.gridOptions.columns[1]).toMatchObject({
      align: 'center',
      field: 'action',
      fixed: 'right',
      minWidth: 150,
      slots: { default: 'action' },
      title: 'crud.actions',
      width: 150,
    });
  });

  it('keeps an existing action column and supports actionColumn: false', () => {
    useAbpDataTable<Dto>({
      gridOptions: {
        columns: [
          { field: 'name', title: 'Name' },
          { field: 'action', fixed: 'right', title: 'Custom' },
        ],
      },
      load: state.load,
    });

    expect(state.gridOptions.columns).toHaveLength(2);
    expect(state.gridOptions.columns[1].title).toBe('Custom');

    useAbpDataTable<Dto>({
      actionColumn: false,
      gridOptions: { columns: [{ field: 'name', title: 'Name' }] },
      load: state.load,
    });

    expect(state.gridOptions.columns).toHaveLength(1);
  });

  it('translates ABP paging and sorting into load parameters', async () => {
    state.load.mockResolvedValue({
      items: [{ id: '1', name: 'Book' }],
      totalCount: 21,
    });

    useAbpDataTable<Dto>({
      gridOptions: { columns: [] },
      load: state.load,
    });

    const result = await state.gridOptions.proxyConfig.ajax.query(
      {
        page: { currentPage: 2, pageSize: 10 },
        sort: { field: 'name', order: 'asc' },
      },
      { filter: 'book' },
    );

    expect(state.load).toHaveBeenCalledWith({
      pager: { maxResultCount: 10, skipCount: 10 },
      searchValues: { filter: 'book' },
      sorting: 'name asc',
    });
    expect(result).toEqual({
      items: [{ id: '1', name: 'Book' }],
      totalCount: 21,
    });
  });

  it('falls back to frontend pagination when totalCount is missing', async () => {
    state.load.mockResolvedValue({
      items: [{ id: '1' }, { id: '2' }, { id: '3' }],
    });

    useAbpDataTable<Dto>({
      gridOptions: { columns: [] },
      load: state.load,
    });

    const result = await state.gridOptions.proxyConfig.ajax.query(
      { page: { currentPage: 2, pageSize: 2 } },
      {},
    );

    expect(result).toEqual({
      items: [{ id: '3' }],
      totalCount: 3,
    });
  });

  it('confirms before deleting a row and refreshes the grid', async () => {
    const [, tableApi] = useAbpDataTable<Dto>({
      delete: state.deleteApi,
      gridOptions: { columns: [] },
      load: state.load,
    });
    await tableApi.delete({ id: '1', name: 'Book' });

    const confirmOptions = state.confirm.mock.calls[0]?.[0];
    expect(confirmOptions.content).toBe('crud.deleteWarnMessage');
    await confirmOptions.beforeClose({ isConfirm: true });

    expect(state.deleteApi).toHaveBeenCalledWith({ id: '1', name: 'Book' });
    expect(state.toastSuccess).toHaveBeenCalledWith('crud.deleteSuccess');
    expect(state.gridApi.query).toHaveBeenCalled();
  });
});
