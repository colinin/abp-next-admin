<script setup lang="ts">
import type { VxeGridPropTypes } from 'vxe-table';

import type { PropertyInfo, PropertyProps } from './types';

import { computed, defineAsyncComponent, h, reactive } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Popconfirm } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

defineOptions({
  name: 'PropertyTable',
});

const props = defineProps<PropertyProps>();
const emits = defineEmits<{
  (event: 'change', data: PropertyInfo): void;
  (event: 'delete', data: PropertyInfo): void;
}>();

const getDataResource = computed((): PropertyInfo[] => {
  if (!props.data) return [];
  return Object.keys(props.data).map((item) => {
    return {
      key: item,
      value: props.data![item]!,
    };
  });
});

const columnsConfig = reactive<VxeGridPropTypes.Columns<PropertyInfo>>([
  {
    align: 'left',
    field: 'key',
    minWidth: 200,
    title: $t('AbpOpenIddict.Propertites:Key'),
  },
  {
    align: 'left',
    field: 'value',
    minWidth: 200,
    title: $t('AbpOpenIddict.Propertites:Value'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 180,
  },
]);

const [PropertyModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./PropertyModal.vue')),
});

function onCreate() {
  modalApi.open();
}

function onDelete(prop: PropertyInfo) {
  emits('delete', prop);
}

function onChange(prop: PropertyInfo) {
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
        {{ $t('AbpOpenIddict.Propertites:New') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Popconfirm
          :title="`${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.key])}`"
          @confirm="onDelete(row)"
        >
          <Button :icon="h(DeleteOutlined)" block danger type="link">
            {{ $t('AbpUi.Delete') }}
          </Button>
        </Popconfirm>
      </div>
    </template>
  </VxeGrid>
  <PropertyModal @change="onChange" />
</template>

<style scoped></style>
