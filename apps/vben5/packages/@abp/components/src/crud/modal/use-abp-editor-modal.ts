import type { FormValues } from '@vben/common-ui';

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

  const [Form, formApi] = useVbenForm<TFormValues>({
    ...options.formOptions,
    submitButtonOptions,
    handleSubmit: onSubmit,
    showDefaultActions: false,
  });

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
        return h(
          Modal as any,
          attrs as any,
          {
            ...modalSlots,
            default: () => h(Form as any, {}, formSlots),
          } as any,
        );
      };
    },
  }) as unknown as AbpEditorModalComponent<TFormValues>; // 保留 Modal 的 props/slots 类型推导

  return [AbpEditorModal, modalApi] as const;
}

export type UseAbpEditorModal = typeof useAbpEditorModal;
