import type { FormValues } from '@vben/common-ui';

import { beforeEach, describe, expect, it, vi } from 'vitest';

import { useAbpEditorDrawer } from '../use-abp-editor-drawer';

interface Dto {
  id: string;
}

interface Values extends FormValues {
  description: string;
  name: string;
}

const state = vi.hoisted(() => ({
  formCalls: [] as Array<{ api: any; options: any }>,
  drawerOptions: null as any,
  drawerApi: null as any,
  create: vi.fn(),
  toastSuccess: vi.fn(),
  toastError: vi.fn(),
}));

vi.mock('@vben/common-ui', () => ({
  confirm: vi.fn(async () => undefined),
  useVbenForm: vi.fn((options: any) => {
    const api: any = {
      getValues: vi.fn(async () => ({})),
      reset: vi.fn(async () => undefined),
      setState: vi.fn(),
      setValues: vi.fn(async () => undefined),
      updateSchema: vi.fn(),
      validate: vi.fn(async () => ({ errors: {}, valid: true })),
    };
    api.validateAndSubmit = vi.fn(async () => {
      const result = await api.validate();
      if (result.valid) {
        await options.handleSubmit?.(await api.getValues());
      }
    });
    state.formCalls.push({ api, options });
    return [{ name: 'FormMock', render: () => null }, api];
  }),
  useVbenDrawer: vi.fn((options: any) => {
    state.drawerOptions = options;
    const api = {
      close: vi.fn(async () => undefined),
      open: vi.fn(),
      setState: vi.fn(),
    };
    state.drawerApi = api;
    return [{ name: 'DrawerMock', render: () => null }, api];
  }),
}));

vi.mock('@vben-core/shadcn-ui', () => ({
  Tabs: { name: 'Tabs', render: () => null },
  TabsList: { name: 'TabsList', render: () => null },
  TabsTrigger: { name: 'TabsTrigger', render: () => null },
}));

vi.mock('@vben/locales', () => ({
  $t: (key: string) => key,
}));

vi.mock('@vben/utils', () => ({
  isEqual: (a: unknown, b: unknown) => JSON.stringify(a) === JSON.stringify(b),
}));

vi.mock('@abp/core', () => ({
  useNotifications: () => ({
    toastError: state.toastError,
    toastSuccess: state.toastSuccess,
  }),
}));

function useModal(
  options: Parameters<typeof useAbpEditorDrawer<Dto, Values>>[0],
) {
  return useAbpEditorDrawer<Dto, Values>(options);
}

function getFormCall(index: number) {
  const call = state.formCalls[index];
  if (!call) {
    throw new Error(`Missing form call at index ${index}`);
  }
  return call;
}

describe('useAbpEditorDrawer tabs', () => {
  beforeEach(() => {
    state.formCalls = [];
    state.drawerOptions = null;
    state.drawerApi = null;
    state.create.mockReset();
    state.toastSuccess.mockReset();
    state.toastError.mockReset();
  });

  it('adds tab classes to a single form schema', () => {
    useModal({
      create: state.create,
      formOptions: {
        schema: [
          { component: 'Input', fieldName: 'name', tabKey: 'basic' },
          {
            component: 'Input',
            fieldName: 'description',
            tabKey: 'advanced',
          },
        ],
        tabs: [
          { key: 'basic', title: 'Basic' },
          { key: 'advanced', title: 'Advanced' },
        ],
      },
      idKey: (dto) => dto.id,
    });

    expect(state.formCalls).toHaveLength(1);
    const schema = getFormCall(0).options.schema;
    expect(schema[0].formItemClass).toContain('abp-editor-tab-field-0');
    expect(schema[1].formItemClass).toContain('abp-editor-tab-field-1');
    expect(getFormCall(0).options.showDefaultActions).toBe(false);
  });

  it('puts schema without tabKey into the first tab', () => {
    useModal({
      formOptions: {
        schema: [
          { component: 'Input', fieldName: 'name' },
          { component: 'Input', fieldName: 'description', tabKey: 'advanced' },
        ],
        tabs: [
          { key: 'basic', title: 'Basic' },
          { key: 'advanced', title: 'Advanced' },
        ],
      },
      idKey: (dto) => dto.id,
    });

    const schema = getFormCall(0).options.schema;
    expect(schema[0].formItemClass).toContain('abp-editor-tab-field-0');
    expect(schema[1].formItemClass).toContain('abp-editor-tab-field-1');
  });

  it('submits the values of the single tabbed form', async () => {
    useModal({
      create: state.create,
      formOptions: {
        schema: [
          { component: 'Input', fieldName: 'name', tabKey: 'basic' },
          {
            component: 'Input',
            fieldName: 'description',
            tabKey: 'advanced',
          },
        ],
        tabs: [
          { key: 'basic', title: 'Basic' },
          { key: 'advanced', title: 'Advanced' },
        ],
      },
      idKey: (dto) => dto.id,
    });

    getFormCall(0).api.getValues.mockResolvedValue({
      description: 'Description',
      name: 'Book',
    });

    await state.drawerOptions.onConfirm();

    expect(state.create).toHaveBeenCalledWith({
      description: 'Description',
      name: 'Book',
    });
    expect(state.drawerApi.close).toHaveBeenCalled();
  });

  it('does not submit when the tabbed form is invalid', async () => {
    useModal({
      create: state.create,
      formOptions: {
        schema: [
          { component: 'Input', fieldName: 'name', tabKey: 'basic' },
          {
            component: 'Input',
            fieldName: 'description',
            tabKey: 'advanced',
          },
        ],
        tabs: [
          { key: 'basic', title: 'Basic' },
          { key: 'advanced', title: 'Advanced' },
        ],
      },
      idKey: (dto) => dto.id,
    });

    getFormCall(0).api.validate.mockResolvedValue({
      errors: { description: 'Required' },
      valid: false,
    });

    await state.drawerOptions.onConfirm();

    expect(state.create).not.toHaveBeenCalled();
    expect(state.drawerApi.close).not.toHaveBeenCalled();
  });

  it('keeps the plain schema when tabs are not configured', () => {
    const schema = [
      { component: 'Input', fieldName: 'name' },
      { component: 'Input', fieldName: 'description' },
    ];
    useModal({
      formOptions: { schema },
      idKey: (dto) => dto.id,
    });

    expect(state.formCalls).toHaveLength(1);
    expect(getFormCall(0).options.schema).toEqual(schema);
  });
});
