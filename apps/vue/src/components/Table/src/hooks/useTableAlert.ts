import type { ComputedRef } from 'vue';
import type { BasicTableProps } from '../types/table';

import { computed, unref } from 'vue';
import {} from '/@/hooks/web/useI18n';
import { useI18n } from 'vue-i18n';

export function useTableAlert(
  propsRef: ComputedRef<BasicTableProps>,
  getSelectRowKeys: () => string[]
) {
  const { t } = useI18n();

  const getAlertEnabled = computed(() => {
    const props = unref(propsRef);
    if (!props.useSelectedAlert) {
      return false;
    }
    const selectedKeys = getSelectRowKeys();
    return selectedKeys.length > 0;
  });

  const getAlertMessage = computed(() => {
    const selectedKeys = getSelectRowKeys();
    console.log(selectedKeys);
    return t('component.table.selectedRows', { count: selectedKeys.length });
  });

  return {
    getAlertEnabled,
    getAlertMessage,
  };
}
