import type { FormValues } from '@vben/common-ui';

import type {
  AbpEditorModalApi,
  AbpEditorModalComponent,
  AbpEditorModalOptions,
  AbpEditorMode,
  AbpIdOf,
} from './types';

import { defineComponent, h, ref } from 'vue';

import { confirm, useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { isEqual } from '@vben/utils';

import { useNotifications } from '@abp/core';

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
  const initialValues = ref<TFormValues>();
  const userOnOpenChange = options.onOpenChange;

  const { toastSuccess, toastError } = useNotifications();
  const [Form, formApi] = useVbenForm<TFormValues>({
    ...options.formOptions,
    handleSubmit: onSubmit,
    showDefaultActions: false,
  });

  const [Modal, baseModalApi] = useVbenModal<TDto>({
    ...options.modalOptions,
    onConfirm: async () => {
      await formApi.validateAndSubmit();
    },
    onOpenChange: async (isOpen) => {
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
    if (options.title) {
      return options.title(mode.value, currentDto.value);
    }
    const titleValue = currentDto.value
      ? options.titleKey(currentDto.value)
      : '';
    if (mode.value === 'create') {
      return $t('crud.create');
    }
    if (mode.value === 'view') {
      return titleValue ? `${$t('crud.view')} ${titleValue}` : $t('crud.view');
    }
    return titleValue ? `${$t('crud.edit')} ${titleValue}` : $t('crud.edit');
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
    initialValues.value = await formApi.getRawValues();
    formApi.setState((prev) => ({
      commonConfig: {
        ...prev.commonConfig,
        disabled,
      },
    }));
    baseModalApi.setState({
      showConfirmButton: !disabled,
      title: getTitle(),
    });
  }

  async function confirmBeforeClose(): Promise<boolean> {
    if (mode.value === 'view' || !initialValues.value) {
      return true;
    }
    const currentValues = await formApi.getRawValues();
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
        await options.create(values);
      } else if (dto) {
        await options.update(options.idKey(dto), values);
      } else {
        return;
      }
      initialValues.value = await formApi.getRawValues();
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
      return () =>
        h(
          Modal as any,
          attrs as any,
          {
            ...slots,
            default: () => h(Form as any),
          } as any,
        );
    },
  }) as unknown as AbpEditorModalComponent; // 保留 Modal 的 props/slots 类型推导

  return [AbpEditorModal, modalApi] as const;
}

export type UseAbpEditorModal = typeof useAbpEditorModal;
