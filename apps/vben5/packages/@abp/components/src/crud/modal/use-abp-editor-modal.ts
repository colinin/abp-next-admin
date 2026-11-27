import type { ExtendedFormApi, FormValues } from '@vben/common-ui';

import type { AbpIdOf } from '../types';
import type {
  AbpEditorModalApi,
  AbpEditorModalComponent,
  AbpEditorModalOptions,
  AbpEditorMode,
} from './types';

import { defineComponent, h, ref } from 'vue';

import { confirm, useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { isEqual } from '@vben/utils';

import { Tabs, TabsList, TabsTrigger } from '@vben-core/shadcn-ui';

import { useNotifications } from '@abp/core';

const FORM_ACTION_SLOT_NAMES = new Set([
  'default',
  'expand-after',
  'expand-before',
  'reset-before',
  'submit-before',
]);

function toCamelCaseSlotName(name: string) {
  return name.replaceAll(/-([a-z])/g, (_, char: string) => char.toUpperCase());
}

function isRecord(value: unknown): value is Record<string, any> {
  return typeof value === 'object' && value !== null;
}

/**
 * 基于 vben Modal + vben Form 的 ABP 编辑弹窗。
 *
 * - open() 新增
 * - open(dto) / open(id) 编辑
 * - open(dto, 'view') / view(dto) 浏览
 * - create/update 为提交 API，refresh 用于保存成功后刷新列表
 */
export function useAbpEditorModal<
  TDto extends Record<string, any> = any,
  TFormValues extends FormValues = FormValues,
>(options: AbpEditorModalOptions<TDto, TFormValues>) {
  const mode = ref<AbpEditorMode>('create');
  const currentDto = ref<TDto>();
  const checkValueChanged = ref(false);
  const initialValues = ref<TFormValues>();
  const userOnOpenChange = options.onOpenChange;
  const submitButtonOptions = {
    ...options.formOptions.submitButtonOptions,
    show: !options.create && !options.update,
  };

  const { toastSuccess, toastError } = useNotifications();

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

  function addTabClass(item: any, tabKey?: string) {
    const classes = new Set(
      String(item.formItemClass ?? '')
        .split(/\s+/)
        .filter(Boolean),
    );
    classes.add('abp-editor-tab-field');
    classes.add(`abp-editor-tab-field-${getTabIndex(tabKey)}`);
    return {
      ...item,
      formItemClass: [...classes].join(' '),
    };
  }

  function decorateSchema(
    schema: any[],
    inheritedTabKey: string = firstTabKey,
  ): any[] {
    return schema.map((item) => {
      const tabKey = item.tabKey ?? inheritedTabKey;
      if (item.type === 'group' && Array.isArray(item.children)) {
        return {
          ...addTabClass(item, tabKey),
          children: decorateSchema(item.children, tabKey),
        };
      }
      return addTabClass(item, tabKey);
    });
  }

  function getTabSchema(schema: any[]) {
    return hasTabs ? decorateSchema(schema) : schema;
  }

  const [Form, rawFormApi] = useVbenForm<TFormValues>({
    ...options.formOptions,
    schema: getTabSchema(formSchema),
    handleSubmit: onSubmit,
    submitButtonOptions,
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

  const [Modal, baseModalApi] = useVbenModal<TDto>({
    ...options.modalOptions,
    onConfirm: async () => {
      await formApi.validateAndSubmit();
    },
    onOpenChange: async (isOpen) => {
      checkValueChanged.value = isOpen;
      await userOnOpenChange?.(isOpen);
    },
    onBeforeClose: async () => {
      return await confirmBeforeClose();
    },
    onOpened: () => {
      void applyMode();
    },
  });

  const baseOpen = baseModalApi.open.bind(baseModalApi);
  const modalApi = baseModalApi as unknown as AbpEditorModalApi<TDto>;

  function getDefaultValues(): Partial<TFormValues> {
    if (typeof options.defaultValues === 'function') {
      return options.defaultValues();
    }
    return options.defaultValues ?? {};
  }

  function getTitle(): string {
    let titleValue = '';
    if (options.title) {
      titleValue = options.title(mode.value, currentDto.value) ?? '';
    }
    if (!titleValue && currentDto.value && options.titleKey) {
      titleValue = options.titleKey(currentDto.value);
    }
    if (mode.value === 'create') {
      return $t('crud.create');
    }
    if (mode.value === 'view') {
      return titleValue ? `${$t('crud.view')} ${titleValue}` : $t('crud.view');
    }
    return titleValue ? `${$t('crud.edit')} ${titleValue}` : $t('crud.edit');
  }

  function getDescription(): string | undefined {
    if (options.description) {
      return options.description(mode.value, currentDto.value);
    }
    return undefined;
  }

  async function applyMode() {
    const disabled = mode.value === 'view';
    await options.onOpen?.({
      dto: currentDto.value,
      formApi,
      mode: mode.value,
    });
    const values =
      mode.value === 'create' ? getDefaultValues() : (currentDto.value ?? {});
    await formApi.reset({ values });
    initialValues.value = await formApi.getValues();
    formApi.setState((prev) => ({
      commonConfig: {
        ...prev.commonConfig,
        disabled,
      },
    }));
    baseModalApi.setState({
      showConfirmButton: !disabled,
      title: getTitle(),
      description: getDescription(),
    });
  }

  async function confirmBeforeClose(): Promise<boolean> {
    if (
      !checkValueChanged.value ||
      mode.value === 'view' ||
      !initialValues.value
    ) {
      return true;
    }
    const currentValues = await formApi.getValues();
    if (isEqual(currentValues, initialValues.value)) {
      return true;
    }
    try {
      await confirm({
        centered: true,
        containerClass: '!w-[350px]',
        content: $t('crud.unsavedChanges'),
        escapeKeyClose: false,
        icon: 'warning',
        title: $t('crud.leaveConfirmTitle'),
      });
      return true;
    } catch {
      return false;
    }
  }

  async function onSubmit(values: TFormValues) {
    if (mode.value === 'view') {
      return;
    }
    const dto = currentDto.value;
    try {
      baseModalApi.setState({ submitting: true });
      if (mode.value === 'create') {
        options.create && (await options.create(values));
      } else if (dto) {
        options.update && (await options.update(options.idKey(dto), values));
      } else {
        return;
      }
      initialValues.value = await formApi.getValues();
      toastSuccess($t('crud.saveSuccess'));
      await options.refresh?.();
      options.onSuccess?.(mode.value, dto);
      await baseModalApi.close();
    } finally {
      baseModalApi.setState({ submitting: false });
    }
  }

  function openEditor(nextMode: AbpEditorMode, data?: TDto) {
    if (hasTabs) {
      activeTabKey.value = firstTabKey;
    }
    mode.value = nextMode;
    currentDto.value = data;
    baseModalApi.setState({
      showConfirmButton: nextMode !== 'view',
      title: getTitle(),
    });
    baseOpen();
  }

  async function resolveDto(data: AbpIdOf<TDto> | TDto): Promise<TDto> {
    if (isRecord(data)) {
      return data;
    }
    if (!options.get) {
      throw new Error(
        'useAbpEditorModal: get() is required when opening by id.',
      );
    }
    return await options.get(data);
  }

  modalApi.open = async (
    data?: AbpIdOf<TDto> | TDto,
    nextMode?: 'edit' | 'view',
  ) => {
    try {
      if (data === undefined || data === null) {
        openEditor('create');
        return;
      }
      const dto = await resolveDto(data);
      openEditor(nextMode ?? 'edit', dto);
    } catch (error) {
      toastError(error instanceof Error ? error.message : String(error));
    }
  };
  modalApi.create = async () => {
    await modalApi.open();
  };
  modalApi.update = async (data: AbpIdOf<TDto> | TDto) => {
    await modalApi.open(data, 'edit');
  };
  modalApi.view = async (data: AbpIdOf<TDto> | TDto) => {
    await modalApi.open(data, 'view');
  };
  modalApi.refresh = async () => {
    if (currentDto.value && options.get) {
      currentDto.value = await options.get(options.idKey(currentDto.value));
    }
    await applyMode();
  };

  const AbpEditorModal = defineComponent({
    name: 'AbpEditorModal',
    inheritAttrs: false,
    setup(_, { attrs, slots }) {
      return () => {
        const formSlots: Record<string, any> = {};
        const modalSlots: Record<string, any> = {};
        for (const [name, slot] of Object.entries(slots)) {
          if (name.startsWith('form-')) {
            const formSlotName = name.slice('form-'.length);
            if (formSlotName) {
              formSlots[formSlotName] = slot;
              if (!FORM_ACTION_SLOT_NAMES.has(formSlotName)) {
                const camelCaseName = toCamelCaseSlotName(formSlotName);
                if (camelCaseName !== formSlotName) {
                  formSlots[camelCaseName] = slot;
                }
              }
            }
          } else {
            modalSlots[name] = slot;
          }
        }
        const renderForm = () => h(Form as any, {}, formSlots);

        let formContent;
        if (hasTabs) {
          const tabStyles = tabs
            .map(
              (_, index) =>
                `.abp-editor-tabs-form[data-active-tab="${index}"] .abp-editor-tab-field:not(.abp-editor-tab-field-${index}) { display: none; }`,
            )
            .join('\n');

          formContent = h(
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
                  { class: 'w-full' },
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
                  { default: renderForm },
                ),
              ],
            },
          );
        } else {
          formContent = renderForm();
        }

        return h(
          Modal as any,
          attrs as any,
          {
            ...modalSlots,
            default: () => formContent,
          } as any,
        );
      };
    },
  }) as unknown as AbpEditorModalComponent<TFormValues>; // 保留 Modal 的 props/slots 类型推导

  return [AbpEditorModal, modalApi] as const;
}

export type UseAbpEditorModal = typeof useAbpEditorModal;
