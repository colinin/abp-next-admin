import { beforeEach, describe, expect, it, vi } from 'vitest';

import { splitEditorSlots } from '../slots';
import { useAbpEditorForm } from '../use-abp-editor-form';

const state = vi.hoisted(() => ({
  FormMock: { name: 'Form', render: () => null },
  TabsListMock: { name: 'TabsList', render: () => null },
  TabsMock: { name: 'Tabs', render: () => null },
  TabsTriggerMock: { name: 'TabsTrigger', render: () => null },
  formOptions: null as any,
  formApi: null as any,
  onSubmit: vi.fn(),
  rawSetState: vi.fn((_state?: any) => undefined),
  rawUpdateSchema: vi.fn((_schema?: any[]) => undefined),
}));

const FormMock = state.FormMock;
const TabsListMock = state.TabsListMock;

vi.mock('@vben/common-ui', () => ({
  useVbenForm: vi.fn((options: any) => {
    state.formOptions = options;
    const api = {
      getValues: vi.fn(async () => ({})),
      reset: vi.fn(async () => undefined),
      setState: state.rawSetState,
      setValues: vi.fn(async () => undefined),
      updateSchema: state.rawUpdateSchema,
      validate: vi.fn(async () => ({ errors: {}, valid: true })),
    };
    state.formApi = api;
    return [state.FormMock, api];
  }),
}));

vi.mock('@vben-core/shadcn-ui', () => ({
  Tabs: state.TabsMock,
  TabsList: state.TabsListMock,
  TabsTrigger: state.TabsTriggerMock,
}));

function createForm(options: any = {}) {
  return useAbpEditorForm({
    formOptions: options,
    onSubmit: state.onSubmit,
  });
}

function renderSlotChildren(vnode: any) {
  const children = vnode.children;
  return typeof children === 'object' && children?.default
    ? children.default()
    : children;
}

function findVNode(nodes: any[], type: any): any {
  return nodes.find((node) => node?.type === type);
}

function requireVNode(node: any) {
  if (!node) {
    throw new Error('Expected a VNode');
  }
  return node;
}

describe('useAbpEditorForm', () => {
  beforeEach(() => {
    state.formOptions = null;
    state.formApi = null;
    state.onSubmit.mockReset();
    state.rawSetState.mockReset();
    state.rawUpdateSchema.mockReset();
  });

  it('passes the schema and submit handler to useVbenForm', () => {
    const schema = [{ component: 'Input', fieldName: 'name' }];
    createForm({ schema });

    expect(state.formOptions.schema).toEqual(schema);
    expect(state.formOptions.handleSubmit).toBe(state.onSubmit);
    expect(state.formOptions.showDefaultActions).toBe(false);
  });

  it('adds tab classes to fields and inherits a group tab key', () => {
    createForm({
      schema: [
        { component: 'Input', fieldName: 'name', tabKey: 'basic' },
        {
          children: [{ component: 'Input', fieldName: 'description' }],
          tabKey: 'advanced',
          type: 'group',
        },
      ],
      tabs: [
        { key: 'basic', title: 'Basic' },
        { key: 'advanced', title: 'Advanced' },
      ],
    });

    const [nameField, group] = state.formOptions.schema;
    expect(nameField.formItemClass).toContain('abp-editor-tab-field-0');
    expect(group.children[0].formItemClass).toContain('abp-editor-tab-field-1');
  });

  it('decorates schemas passed through setState and updateSchema', () => {
    const { formApi } = createForm({
      schema: [{ component: 'Input', fieldName: 'name', tabKey: 'basic' }],
      tabs: [
        { key: 'basic', title: 'Basic' },
        { key: 'advanced', title: 'Advanced' },
      ],
    });

    const schema = [
      { component: 'Input', fieldName: 'description', tabKey: 'advanced' },
    ];
    formApi.setState({ schema });
    const setStateArg = state.rawSetState.mock.calls[0]?.[0];
    if (!setStateArg) {
      throw new Error('setState was not called');
    }
    expect(setStateArg.schema[0].formItemClass).toContain(
      'abp-editor-tab-field-1',
    );

    formApi.updateSchema(schema);
    const updateArg = state.rawUpdateSchema.mock.calls[0]?.[0];
    if (!updateArg?.[0]) {
      throw new Error('updateSchema was not called');
    }
    expect(updateArg[0].formItemClass).toContain('abp-editor-tab-field-1');
  });

  it('renders a single form without tabs', () => {
    const { renderFormContent } = createForm({
      schema: [{ component: 'Input', fieldName: 'name' }],
    });

    const content = renderFormContent({});
    expect(content.type).toBe(FormMock);
  });

  it('renders line Tabs and resets the active tab', () => {
    const { renderFormContent, resetActiveTab } = createForm({
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
    });

    const content = renderFormContent({});
    const children = renderSlotChildren(content);
    const tabsList = requireVNode(findVNode(children, TabsListMock));
    expect(tabsList.props.variant).toBe('line');

    const triggerNodes = renderSlotChildren(tabsList);
    expect(triggerNodes.map((node: any) => node.props.value)).toEqual([
      'basic',
      'advanced',
    ]);

    if (!content.props) {
      throw new Error('Tabs props are missing');
    }
    content.props['onUpdate:modelValue']('advanced');
    let formWrapper = requireVNode(
      findVNode(renderSlotChildren(renderFormContent({})), 'div'),
    );
    expect(formWrapper.props?.['data-active-tab']).toBe('1');

    resetActiveTab();
    formWrapper = requireVNode(
      findVNode(renderSlotChildren(renderFormContent({})), 'div'),
    );
    expect(formWrapper.props?.['data-active-tab']).toBe('0');
  });
});

describe('splitEditorSlots', () => {
  it('splits form slots from container slots and supports kebab-case aliases', () => {
    const footer = () => 'footer';
    const title = () => 'title';
    const userName = () => 'user-name';
    const propertyChanges = () => 'property-changes';

    const { containerSlots, formSlots } = splitEditorSlots({
      'form-property-changes': propertyChanges,
      'form-userName': userName,
      footer,
      title,
    });

    expect(containerSlots).toEqual({ footer, title });
    expect(formSlots).toEqual({
      propertyChanges,
      'property-changes': propertyChanges,
      userName,
    });
  });
});
