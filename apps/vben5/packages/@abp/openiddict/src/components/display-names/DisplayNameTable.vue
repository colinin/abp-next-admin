<script setup lang="ts">
import type { VxeGridPropTypes } from 'vxe-table';

import type { DisplayNameInfo, DisplayNameProps } from './types';

import { computed, defineAsyncComponent, h, reactive } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Popconfirm } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

defineOptions({
  name: 'DisplayNameTable',
});

const props = defineProps<DisplayNameProps>();
const emits = defineEmits<{
  (event: 'change', data: DisplayNameInfo): void;
  (event: 'delete', data: DisplayNameInfo): void;
}>();

const getDataResource = computed((): DisplayNameInfo[] => {
  if (!props.data) return [];
  return Object.keys(props.data).map((item) => {
    return {
      culture: item,
      displayName: props.data![item]!,
    };
  });
});

const columnsConfig = reactive<VxeGridPropTypes.Columns<DisplayNameInfo>>([
  {
    align: 'left',
    field: 'culture',
    minWidth: 200,
    title: $t('AbpOpenIddict.DisplayName:CultureName'),
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 200,
    title: $t('AbpOpenIddict.DisplayName:DisplayName'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 180,
  },
]);

const [DisplayNameModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./DisplayNameModal.vue'),
  ),
});

function onCreate() {
  modalApi.open();
}

function onDelete(prop: DisplayNameInfo) {
  emits('delete', prop);
}

function onChange(prop: DisplayNameInfo) {
  emits('change', prop);
}
</script>

<template>
  <VxeGrid
    :columns="columnsConfig"
    :data="getDataResource"
    :toolbar-config="{ slots: { tools: 'toolbar_tools' } }"
  >
    <template #toolbar_tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('AbpOpenIddict.DisplayName:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Popconfirm
          :title="`${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.culture])}`"
          @confirm="onDelete(row)"
        >
          <Button :icon="h(DeleteOutlined)" block danger type="link">
            {{ $t('AbpUi.Delete') }}
          </Button>
        </Popconfirm>
      </div>
    </template>
  </VxeGrid>
  <DisplayNameModal @change="onChange" />
</template>

<style scoped></style>
