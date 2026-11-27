<script setup lang="ts">
import type { EntityTypeInfoDto } from '../../types';

import { onMounted, ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';

import { Select } from 'ant-design-vue';

import { useEntityTypeInfosApi } from '../../api/useEntityTypeInfosApi';
import { DataAccessOperation } from '../../types/entityRules';

const emits = defineEmits<{
  (event: 'entityChange', id: string): void;
}>();

const entityTypes = ref<EntityTypeInfoDto[]>([]);
const [Form, formApi] = useVbenForm({
  schema: [
    {
      component: 'Select',
      fieldName: 'entityTypeId',
    },
    {
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '查询',
            value: DataAccessOperation.Read,
          },
          {
            label: '编辑',
            value: DataAccessOperation.Write,
          },
          {
            label: '删除',
            value: DataAccessOperation.Delete,
          },
        ],
      },
      fieldName: 'operation',
    },
  ],
  showDefaultActions: false,
});
const [Modal] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
});

const { getPagedListApi } = useEntityTypeInfosApi();

async function onInit(filter?: string) {
  const { items } = await getPagedListApi({ filter });
  entityTypes.value = items;
}

onMounted(onInit);
</script>

<template>
  <Modal>
    <Form>
      <template #entityTypeId="slotProps">
        <Select
          v-bind="slotProps.componentProps"
          :options="entityTypes"
          :field-names="{ label: 'displayName', value: 'id' }"
          @change="(value) => emits('entityChange', value!.toString())"
        />
      </template>
      <slot name="subject"></slot>
    </Form>
  </Modal>
</template>

<style scoped></style>
