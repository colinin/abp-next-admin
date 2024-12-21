<script setup lang="ts">
import type { VxeGridPropTypes } from 'vxe-table';

import { computed, defineAsyncComponent, h, reactive } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Popconfirm } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

defineOptions({
  name: 'UriTable',
});

const props = defineProps<{
  title: string;
  uris?: string[];
}>();

const emits = defineEmits<{
  (event: 'change', uri: string): void;
  (event: 'delete', uri: string): void;
}>();

interface Uri {
  uri: string;
}

const getDataResource = computed((): Uri[] => {
  if (!props.uris) return [];
  return props.uris.map((uri) => {
    return { uri };
  });
});

const columnsConfig = reactive<VxeGridPropTypes.Columns<Uri>>([
  {
    align: 'left',
    field: 'uri',
    title: 'Uri',
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 180,
  },
]);
const toolbarConfig = reactive<VxeGridPropTypes.ToolbarConfig>({
  slots: {
    buttons: 'toolbar_buttons',
    tools: 'toolbar_tools',
  },
});

const [UriModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./UriModal.vue')),
});

function onCreate() {
  modalApi.open();
}

function onDelete(uri: Uri) {
  emits('delete', uri.uri);
}

function onChange(uri: string) {
  emits('change', uri);
}
</script>

<template>
  <VxeGrid
    :columns="columnsConfig"
    :data="getDataResource"
    :toolbar-config="toolbarConfig"
  >
    <template #toolbar_buttons>
      <h3>{{ title }}</h3>
    </template>
    <template #toolbar_tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('AbpOpenIddict.Uri:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Popconfirm
          :title="`${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.uri])}`"
          @confirm="onDelete(row)"
        >
          <Button :icon="h(DeleteOutlined)" block danger type="link">
            {{ $t('AbpUi.Delete') }}
          </Button>
        </Popconfirm>
      </div>
    </template>
  </VxeGrid>
  <UriModal @change="onChange" />
</template>

<style scoped></style>
