<script setup lang="ts">
import type { ColumnsType } from 'ant-design-vue/lib/table';

import type { PropType } from 'vue';

import type { SimplaCheckStateBase } from './interface';

import { computed, reactive, unref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { isNullOrUnDef, useSimpleStateCheck } from '@abp/core';
import { Button, Card, Col, Row, Table, Tag } from 'ant-design-vue';

import SimpleStateCheckingModal from './SimpleStateCheckingModal.vue';

const props = defineProps({
  allowDelete: {
    default: false,
    type: Boolean,
  },
  allowEdit: {
    default: false,
    type: Boolean,
  },
  disabled: {
    default: false,
    type: Boolean,
  },
  state: {
    required: true,
    type: Object as PropType<SimplaCheckStateBase>,
  },
  value: {
    default: '',
    type: String,
  },
});
const emits = defineEmits(['change', 'update:value']);
const DeleteOutlined = createIconifyIcon('ant-design:delete-outlined');
const EditOutlined = createIconifyIcon('ant-design:edit-outlined');

const simpleCheckerMap = reactive<{ [key: string]: string }>({
  A: $t('component.simple_state_checking.requireAuthenticated.title'),
  F: $t('component.simple_state_checking.requireFeatures.title'),
  G: $t('component.simple_state_checking.requireGlobalFeatures.title'),
  P: $t('component.simple_state_checking.requirePermissions.title'),
});

const { deserialize, deserializeArray, serializeArray } = useSimpleStateCheck();

const [Modal, modalApi] = useVbenModal({
  connectedComponent: SimpleStateCheckingModal,
});

const getTableColumns = computed(() => {
  const columns: ColumnsType = [
    {
      align: 'left',
      dataIndex: 'name',
      fixed: 'left',
      key: 'name',
      maxWidth: 150,
      title: $t('component.simple_state_checking.table.name'),
    },
    {
      align: 'left',
      dataIndex: 'properties',
      fixed: 'left',
      key: 'properties',
      title: $t('component.simple_state_checking.table.properties'),
    },
  ];
  return [
    ...columns,
    ...(props.disabled
      ? []
      : [
          {
            align: 'center',
            dataIndex: 'action',
            fixed: 'right',
            key: 'action',
            title: $t('component.simple_state_checking.table.actions'),
            width: 180,
          },
        ]),
  ];
});
const getSimpleCheckers = computed(() => {
  if (isNullOrUnDef(props.value) || props.value.length === 0) {
    return [];
  }
  const simpleCheckers = deserializeArray(props.value, props.state);
  return simpleCheckers;
});
const getStateCheckerOptions = computed(() => {
  const stateCheckers = unref(getSimpleCheckers);
  return Object.keys(simpleCheckerMap).map((key) => {
    return {
      disabled: stateCheckers.some((x) => (x as any).name === key),
      label: simpleCheckerMap[key],
      value: key,
    };
  });
});

function handleAddNew() {
  modalApi.setData({
    options: getStateCheckerOptions.value,
  });
  modalApi.open();
}

function handleEdit(record: any) {
  modalApi.setData({
    options: getStateCheckerOptions.value,
    record,
  });
  modalApi.open();
}

function onChange(record: any) {
  const stateChecker = deserialize(record, props.state)!;
  const stateCheckers = unref(getSimpleCheckers);
  const inputIndex = stateCheckers.findIndex(
    (x) => (x as any).name === record.name,
  );
  if (inputIndex === -1) {
    stateCheckers.push(stateChecker);
  } else {
    stateCheckers[inputIndex] = stateChecker;
  }
  const updateValue = serializeArray(stateCheckers);

  emits('change', updateValue);
  emits('update:value', updateValue);
}

function handleDelete(record: any) {
  const stateCheckers = unref(getSimpleCheckers);
  const filtedStateCheckers = stateCheckers.filter(
    (x) => (x as any).name !== record.name,
  );
  if (filtedStateCheckers.length === 0) {
    handleClean();
    return;
  }
  const serializedCheckers = serializeArray(filtedStateCheckers);
  emits('change', serializedCheckers);
  emits('update:value', serializedCheckers);
}

function handleClean() {
  emits('change', undefined);
  emits('update:value', undefined);
}
</script>

<template>
  <div class="w-full">
    <div class="card">
      <Card>
        <template #title>
          <Row>
            <Col :span="12">
              <span>{{ $t('component.simple_state_checking.title') }}</span>
            </Col>
            <Col :span="12">
              <div class="toolbar" v-if="!props.disabled">
                <Button type="primary" @click="handleAddNew">
                  {{ $t('component.simple_state_checking.actions.create') }}
                </Button>
                <Button danger @click="handleClean">
                  {{ $t('component.simple_state_checking.actions.clean') }}
                </Button>
              </div>
            </Col>
          </Row>
        </template>
        <Table :columns="getTableColumns" :data-source="getSimpleCheckers">
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'name'">
              <span>{{ simpleCheckerMap[record.name] }}</span>
            </template>
            <template v-else-if="column.key === 'properties'">
              <div v-if="record.name === 'F'">
                <Tag v-for="feature in record.featureNames" :key="feature">
                  {{ feature }}
                </Tag>
              </div>
              <div v-else-if="record.name === 'G'">
                <Tag
                  v-for="feature in record.globalFeatureNames"
                  :key="feature"
                >
                  {{ feature }}
                </Tag>
              </div>
              <div v-else-if="record.name === 'P'">
                <Tag
                  v-for="permission in record.model.permissions"
                  :key="permission"
                >
                  {{ permission }}
                </Tag>
              </div>
              <div v-else-if="record.name === 'A'">
                <span>{{
                  $t(
                    'component.simple_state_checking.requireAuthenticated.title',
                  )
                }}</span>
              </div>
            </template>
            <template v-else-if="column.key === 'action'">
              <div class="flex flex-row">
                <Button
                  v-if="props.allowEdit"
                  type="link"
                  @click="() => handleEdit(record)"
                >
                  <template #icon>
                    <EditOutlined />
                  </template>
                  {{ $t('component.simple_state_checking.actions.update') }}
                </Button>
                <Button
                  v-if="props.allowDelete"
                  type="link"
                  @click="() => handleDelete(record)"
                  danger
                >
                  <template #icon>
                    <DeleteOutlined />
                  </template>
                  {{ $t('component.simple_state_checking.actions.delete') }}
                </Button>
              </div>
            </template>
          </template>
        </Table>
      </Card>
    </div>
  </div>
  <Modal @change="onChange" />
</template>

<style lang="less" scoped>
.card {
  .toolbar {
    flex: 1;
    display: flex;
    align-items: center;
    justify-content: flex-end;
    margin-bottom: 8px;

    > * {
      margin-right: 8px;
    }
  }
}
</style>
