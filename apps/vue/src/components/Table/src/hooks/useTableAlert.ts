import type { ComputedRef } from 'vue';
import type { BasicTableProps, TableRowSelection } from '../types/table';

import { computed, unref } from 'vue';
import {} from '/@/hooks/web/useI18n';
import { useI18n } from 'vue-i18n';

export function useTableAlert(
  propsRef: ComputedRef<BasicTableProps>,
  rowSelectionRef: ComputedRef<TableRowSelection | null>,
) {
  const { t } = useI18n();

  const getSelectRowKeysCount = computed(() => {
    const rowSelection = unref(rowSelectionRef);
    if (!rowSelection?.selectedRowKeys) {
      return 0;
    }
    return rowSelection.selectedRowKeys.length;
  });

  const getAlertEnabled = computed(() => {
    const props = unref(propsRef);
    return props.useSelectedAlert === true;
  });

  const getAlertMessage = computed(() => {
    return t('component.table.selectedRows', { count: unref(getSelectRowKeysCount) });
  });

  return {
    getAlertEnabled,
    getAlertMessage,
  };
}
