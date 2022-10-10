import type { ComputedRef, Slots } from 'vue';
import type { BasicTableProps, FetchParams } from '../types/table';
import type { FormProps } from '/@/components/Form';
import type { DynamicQueryable } from '../types/advancedSearch';

import { unref, computed } from 'vue';
import { isFunction } from '/@/utils/is';

export function useTableForm(
  propsRef: ComputedRef<BasicTableProps>,
  slots: Slots,
  fetch: (opt?: FetchParams | undefined) => Promise<void>,
  getLoading: ComputedRef<boolean | undefined>,
  setFieldsValue: <T>(values: T) => Promise<void>,
) {
  const getFormProps = computed((): Partial<FormProps> => {
    const { formConfig, advancedSearchConfig } = unref(propsRef);
    if (advancedSearchConfig?.useAdvancedSearch && formConfig?.schemas) {
      const advIndex = formConfig.schemas.findIndex(s => s.field === 'queryable');
      if (advIndex < 0) {
        // 加入高级条件的隐藏字段
        formConfig.schemas.push({
          label: 'queryable',
          field: 'queryable',
          component: 'CodeEditorX',
          show: false,
          colProps: { span: 24 },
        });
      }
    }
    const { submitButtonOptions } = formConfig || {};
    return {
      showAdvancedButton: true,
      ...formConfig,
      submitButtonOptions: { loading: unref(getLoading), ...submitButtonOptions },
      compact: true,
    };
  });

  const getFormSlotKeys: ComputedRef<string[]> = computed(() => {
    const keys = Object.keys(slots);
    return keys
      .map((item) => (item.startsWith('form-') ? item : null))
      .filter((item) => !!item) as string[];
  });

  const getAdvancedSearchProps = computed(() => {
    const { advancedSearchConfig } = unref(propsRef);

    return advancedSearchConfig;
  });

  function replaceFormSlotKey(key: string) {
    if (!key) return '';
    return key?.replace?.(/form\-/, '') ?? '';
  }

  function handleSearchInfoChange(info: Recordable) {
    const { handleSearchInfoFn } = unref(propsRef);
    if (handleSearchInfoFn && isFunction(handleSearchInfoFn)) {
      info = handleSearchInfoFn(info) || info;
    }
    fetch({ searchInfo: info, page: 1 });
  }

  function handleAdvanceSearchChange(queryable: DynamicQueryable) {
    setFieldsValue({ queryable: queryable });
  }

  function handleAdvanceSearchInfoChange(queryable: DynamicQueryable) {
    setFieldsValue({ queryable: queryable });
    setTimeout(() => {
      fetch({ page: 1 });
    }, 300);
  }

  return {
    getFormProps,
    replaceFormSlotKey,
    getFormSlotKeys,
    getAdvancedSearchProps,
    handleSearchInfoChange,
    handleAdvanceSearchChange,
    handleAdvanceSearchInfoChange,
  };
}
