import type { ExtendedFormApi, FormValues } from '@vben/common-ui';

import type { AbpEditorFormOptions } from './types';

import { h, ref } from 'vue';

import { useVbenForm } from '@vben/common-ui';

import { Tabs, TabsList, TabsTrigger } from '@vben-core/shadcn-ui';

export interface UseAbpEditorFormOptions<TFormValues extends FormValues> {
  formOptions: AbpEditorFormOptions<TFormValues>;
  onSubmit: (values: TFormValues) => Promise<void> | void;
  submitButtonOptions?: Record<string, any>;
}

function addTabClass(item: any, tabIndex: number) {
  const classes = new Set(
    String(item.formItemClass ?? '')
      .split(/\s+/)
      .filter(Boolean),
  );
  classes.add('abp-editor-tab-field');
  classes.add(`abp-editor-tab-field-${tabIndex}`);
  return {
    ...item,
    formItemClass: [...classes].join(' '),
  };
}

/**
 * 编辑器共用的单 Form + Tabs 逻辑。
 * Modal 与 Drawer 只负责容器和提交生命周期，表单能力在这里统一维护。
 */
export function useAbpEditorForm<TFormValues extends FormValues>(
  options: UseAbpEditorFormOptions<TFormValues>,
) {
  const tabs = options.formOptions.tabs ?? [];
  const hasTabs = tabs.length > 0;
  const firstTabKey = tabs[0]?.key ?? '';
  const activeTabKey = ref(firstTabKey);
  const formSchema = options.formOptions.schema ?? [];

  function getTabIndex(tabKey?: string) {
    if (!hasTabs) {
      return 0;
    }
    const key = tabKey ?? firstTabKey;
    const index = tabs.findIndex((tab) => tab.key === key);
    return Math.max(index, 0);
  }

  function decorateSchema(
    schema: any[],
    inheritedTabKey: string = firstTabKey,
  ): any[] {
    return schema.map((item) => {
      const tabKey = item.tabKey ?? inheritedTabKey;
      if (item.type === 'group' && Array.isArray(item.children)) {
        return {
          ...addTabClass(item, getTabIndex(tabKey)),
          children: decorateSchema(item.children, tabKey),
        };
      }
      return addTabClass(item, getTabIndex(tabKey));
    });
  }

  function getTabSchema(schema: any[]) {
    return hasTabs ? decorateSchema(schema) : schema;
  }

  const [Form, rawFormApi] = useVbenForm<TFormValues>({
    ...options.formOptions,
    schema: getTabSchema(formSchema),
    handleSubmit: options.onSubmit,
    submitButtonOptions: options.submitButtonOptions,
    showDefaultActions: false,
  });

  const formApi = new Proxy(rawFormApi, {
    get(target, property, receiver) {
      if (property === 'setState') {
        return (updater: any) => {
          if (typeof updater === 'function') {
            target.setState((prev: any) => {
              const next = updater(prev);
              return next?.schema
                ? { ...next, schema: getTabSchema(next.schema) }
                : next;
            });
          } else {
            target.setState(
              updater?.schema
                ? { ...updater, schema: getTabSchema(updater.schema) }
                : updater,
            );
          }
        };
      }
      if (property === 'updateSchema') {
        return (schema: any[]) => target.updateSchema(getTabSchema(schema));
      }
      return Reflect.get(target, property, receiver);
    },
  }) as ExtendedFormApi<TFormValues>;

  function resetActiveTab() {
    if (hasTabs) {
      activeTabKey.value = firstTabKey;
    }
  }

  function renderFormContent(formSlots: Record<string, any>) {
    const renderForm = () => h(Form as any, {}, formSlots);

    if (!hasTabs) {
      return renderForm();
    }

    const tabStyles = tabs
      .map(
        (_, index) =>
          `.abp-editor-tabs-form[data-active-tab="${index}"] .abp-editor-tab-field:not(.abp-editor-tab-field-${index}) { display: none; }`,
      )
      .join('\n');

    return h(
      Tabs as any,
      {
        class: 'w-full',
        modelValue: activeTabKey.value,
        'onUpdate:modelValue': (value: string) => {
          activeTabKey.value = value;
        },
      },
      {
        default: () => [
          h('style', { textContent: tabStyles }),
          h(
            TabsList as any,
            { class: 'w-full', variant: 'line' },
            {
              default: () =>
                tabs.map((tab) =>
                  h(
                    TabsTrigger as any,
                    {
                      key: tab.key,
                      disabled: tab.disabled,
                      value: tab.key,
                    },
                    { default: () => tab.title },
                  ),
                ),
            },
          ),
          h(
            'div',
            {
              class: 'abp-editor-tabs-form',
              'data-active-tab': String(getTabIndex(activeTabKey.value)),
            },
            [renderForm()],
          ),
        ],
      },
    );
  }

  return {
    Form,
    formApi,
    renderFormContent,
    resetActiveTab,
  };
}
