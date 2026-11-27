import type { FormValues } from '@vben/common-ui';

import type { AbpIdOf } from '../types';
import type {
  AbpEditorDrawerApi,
  AbpEditorDrawerComponent,
  AbpEditorDrawerMode,
  AbpEditorDrawerOptions,
} from './types';

import { defineComponent, h, ref } from 'vue';

import { confirm, useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { isEqual } from '@vben/utils';

import { useNotifications } from '@abp/core';

import { splitEditorSlots } from '../form/slots';
import { useAbpEditorForm } from '../form/use-abp-editor-form';

function isRecord(value: unknown): value is Record<string, any> {
  return typeof value === 'object' && value !== null;
}

/**
 * 基于 vben Drawer + vben Form 的 ABP 编辑抽屉。
 *
 * - open() 新增
 * - open(dto) / open(id) 编辑
 * - open(dto, 'view') / view(dto) 浏览
 * - create/update 为提交 API，refresh 用于保存成功后刷新列表
 */
export function useAbpEditorDrawer<
  TDto extends Record<string, any> = any,
  TFormValues extends FormValues = FormValues,
>(options: AbpEditorDrawerOptions<TDto, TFormValues>) {
  const mode = ref<AbpEditorDrawerMode>('create');
  const currentDto = ref<TDto>();
  const checkValueChanged = ref(false);
  const initialValues = ref<TFormValues>();
  const userOnOpenChange = options.onOpenChange;
  const submitButtonOptions = {
    ...options.formOptions.submitButtonOptions,
    show: !options.create && !options.update,
  };

  const { toastSuccess, toastError } = useNotifications();

  const { formApi, renderFormContent, resetActiveTab } = useAbpEditorForm({
    formOptions: options.formOptions,
    onSubmit,
    submitButtonOptions,
  });

  const [Drawer, baseDrawerApi] = useVbenDrawer<TDto>({
    ...options.drawerOptions,
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

  const baseOpen = baseDrawerApi.open.bind(baseDrawerApi);
  const drawerApi = baseDrawerApi as unknown as AbpEditorDrawerApi<TDto>;

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
    baseDrawerApi.setState({
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
      baseDrawerApi.setState({ submitting: true });
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
      await baseDrawerApi.close();
    } finally {
      baseDrawerApi.setState({ submitting: false });
    }
  }

  function openEditor(nextMode: AbpEditorDrawerMode, data?: TDto) {
    resetActiveTab();
    mode.value = nextMode;
    currentDto.value = data;
    baseDrawerApi.setState({
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
        'useAbpEditorDrawer: get() is required when opening by id.',
      );
    }
    return await options.get(data);
  }

  drawerApi.open = async (
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
  drawerApi.create = async () => {
    await drawerApi.open();
  };
  drawerApi.update = async (data: AbpIdOf<TDto> | TDto) => {
    await drawerApi.open(data, 'edit');
  };
  drawerApi.view = async (data: AbpIdOf<TDto> | TDto) => {
    await drawerApi.open(data, 'view');
  };
  drawerApi.refresh = async () => {
    if (currentDto.value && options.get) {
      currentDto.value = await options.get(options.idKey(currentDto.value));
    }
    await applyMode();
  };

  const AbpEditorDrawer = defineComponent({
    name: 'AbpEditorDrawer',
    inheritAttrs: false,
    setup(_, { attrs, slots }) {
      return () => {
        const { containerSlots, formSlots } = splitEditorSlots(slots);
        return h(
          Drawer as any,
          attrs as any,
          {
            ...containerSlots,
            default: () => renderFormContent(formSlots),
          } as any,
        );
      };
    },
  }) as unknown as AbpEditorDrawerComponent<TFormValues>;

  return [AbpEditorDrawer, drawerApi] as const;
}

export type UseAbpEditorDrawer = typeof useAbpEditorDrawer;
