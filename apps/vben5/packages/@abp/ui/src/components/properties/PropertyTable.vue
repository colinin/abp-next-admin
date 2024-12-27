<script setup lang="ts">
import type { TableColumnsType } from 'ant-design-vue';

import type { PropertyInfo, PropertyProps } from './types';

import { computed, defineAsyncComponent } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { Button, Popconfirm, Table } from 'ant-design-vue';

defineOptions({
  name: 'PropertyTable',
});

const props = withDefaults(defineProps<PropertyProps>(), {
  allowDelete: true,
  allowEdit: true,
  disabled: false,
});
const emits = defineEmits<{
  (event: 'change', data: PropertyInfo): void;
  (event: 'delete', data: PropertyInfo): void;
}>();
const DeleteOutlined = createIconifyIcon('ant-design:delete-outlined');
const PlusOutlined = createIconifyIcon('ant-design:plus-outlined');

const getDataResource = computed((): PropertyInfo[] => {
  if (!props.data) return [];
  return Object.keys(props.data).map((item) => {
    return {
      key: item,
      value: props.data![item]!,
    };
  });
});
const [PropertyModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./PropertyModal.vue')),
});
const getTableColumns = computed((): TableColumnsType<PropertyInfo> => {
  const columns: TableColumnsType<PropertyInfo> = [
    {
      align: 'left',
      dataIndex: 'key',
      fixed: 'left',
      minWidth: 100,
      title: $t('component.extra_property_dictionary.key'),
    },
    {
      align: 'left',
      dataIndex: 'value',
      fixed: 'left',
      title: $t('component.extra_property_dictionary.value'),
    },
  ];
  return [
    ...columns,
    ...(props.disabled
      ? []
      : ([
          {
            align: 'center',
            dataIndex: 'action',
            fixed: 'right',
            key: 'action',
            title: $t('component.extra_property_dictionary.actions.title'),
            width: 150,
          },
        ] as TableColumnsType<PropertyInfo>)),
  ];
});

function onCreate() {
  modalApi.open();
}

function onDelete(prop: Record<string, string>) {
  emits('delete', {
    key: prop.key!,
    value: prop.value!,
  });
}

function onChange(prop: Record<string, string>) {
  emits('change', {
    key: prop.key!,
    value: prop.value!,
  });
}
</script>

<template>
  <div class="flex flex-col gap-4">
    <div class="flex flex-row justify-end">
      <Button
        v-if="!props.disabled && props.allowEdit"
        class="flex items-center gap-2"
        type="primary"
        @click="onCreate"
      >
        <template #icon>
          <PlusOutlined class="inline" />
        </template>
        {{ $t('component.extra_property_dictionary.actions.create') }}
      </Button>
    </div>
    <div class="justify-center">
      <Table :columns="getTableColumns" :data-source="getDataResource">
        <template #bodyCell="{ record, column }">
          <template v-if="column.dataIndex === 'action'">
            <Popconfirm
              v-if="props.allowDelete"
              :title="`${$t('component.extra_property_dictionary.itemWillBeDeleted', [record.key])}`"
              @confirm="onDelete(record)"
            >
              <Button block class="flex items-center gap-2" danger type="link">
                <template #icon>
                  <DeleteOutlined class="inline" />
                </template>
                {{ $t('component.extra_property_dictionary.actions.delete') }}
              </Button>
            </Popconfirm>
          </template>
        </template>
      </Table>
    </div>
    <PropertyModal @change="onChange" />
  </div>
</template>

<style scoped></style>
