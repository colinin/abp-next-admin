<template>
  <div style="width: 100%">
    <div :class="`${prefixCls}__toolbar`" v-if="!props.disabled">
      <Button type="primary" @click="handleAddNew">{{
        t('component.extra_property_dictionary.actions.create')
      }}</Button>
      <Button v-if="props.allowDelete" danger @click="handleClean">{{
        t('component.extra_property_dictionary.actions.clean')
      }}</Button>
    </div>
    <Card :title="t('component.extra_property_dictionary.title')">
      <Table
        sticky
        rowKey="key"
        :columns="getTableColumns"
        :data-source="state.table.dataSource"
        :scroll="{ x: 1500 }"
      >
        <template v-if="!props.disabled" #bodyCell="{ column, record }">
          <template v-if="column.key === 'action'">
            <div :class="`${prefixCls}__action`">
              <Button v-if="props.allowEdit" type="link" @click="() => handleEdit(record)">
                <template #icon>
                  <EditOutlined />
                </template>
                {{ t('component.extra_property_dictionary.actions.update') }}
              </Button>
              <Divider v-if="props.allowEdit && props.allowDelete" type="vertical" />
              <Button
                v-if="props.allowDelete"
                type="link"
                @click="() => handleDelete(record)"
                class="ant-btn-error"
              >
                <template #icon>
                  <DeleteOutlined />
                </template>
                {{ t('component.extra_property_dictionary.actions.delete') }}
              </Button>
            </div>
          </template>
        </template>
      </Table>
    </Card>
    <Modal v-bind="state.modal">
      <Form
        ref="formRef"
        :class="`${prefixCls}__form`"
        v-bind="state.form"
        :label-col="{ span: 6 }"
        :wrapper-col="{ span: 18 }"
      >
        <FormItem name="key" required :label="t('component.extra_property_dictionary.key')">
          <Input :disabled="state.editFlag" v-model:value="state.form.model.key" />
        </FormItem>
        <FormItem name="value" required :label="t('component.extra_property_dictionary.value')">
          <Input v-model:value="state.form.model.value" />
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>

<script lang="ts" setup>
  import type { RuleObject } from 'ant-design-vue/lib/form';
  import type { ColumnsType } from 'ant-design-vue/lib/table/interface';
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, ref, unref, watch } from 'vue';
  import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
  import { Button, Card, Divider, Form, Input, Table, Modal } from 'ant-design-vue';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { propTypes } from '/@/utils/propTypes';

  const FormItem = Form.Item;

  interface DataSource {
    key: string;
    value: string;
  }
  interface State {
    editFlag: boolean;
    modal: {
      title?: string;
      visible?: boolean;
      maskClosable?: boolean;
      width?: number;
      minHeight?: number;
      onOk?: (e: MouseEvent) => void;
      onCancel?: (e: MouseEvent) => void;
    };
    form: {
      model: any;
      rules?: Dictionary<string, RuleObject>;
    };
    table: {
      dataSource: DataSource[];
    };
  }

  const emits = defineEmits(['change', 'update:value']);
  const props = defineProps({
    value: {
      type: Object as PropType<Dictionary<string, string>>,
    },
    allowEdit: propTypes.bool.def(false),
    allowDelete: propTypes.bool.def(false),
    disabled: propTypes.bool.def(false),
  });

  const { prefixCls } = useDesign('extra-property-dictionary');
  const { t } = useI18n();
  const formRef = ref<any>();
  const getTableColumns = computed(() => {
    const columns: ColumnsType = [
      {
        title: t('component.extra_property_dictionary.key'),
        dataIndex: 'key',
        align: 'left',
        fixed: 'left',
        width: 180,
      },
      {
        title: t('component.extra_property_dictionary.value'),
        dataIndex: 'value',
        align: 'left',
        fixed: 'left',
        width: 'auto',
      },
    ];
    return columns.concat(
      props.disabled
        ? []
        : [
            {
              width: 220,
              title: t('component.extra_property_dictionary.actions.title'),
              align: 'center',
              dataIndex: 'action',
              key: 'action',
              fixed: 'right',
            },
          ],
    );
  });
  const state = reactive<State>({
    editFlag: false,
    modal: {
      width: 600,
      minHeight: 400,
      visible: false,
      maskClosable: false,
      onOk: handleSubmit,
      onCancel: handleCancel,
    },
    form: {
      model: {},
      rules: {
        key: {
          validator: (_rule, value) => {
            if (
              !state.editFlag &&
              state.table.dataSource &&
              state.table.dataSource.findIndex((x) => x.key === value) >= 0
            ) {
              return Promise.reject(
                t('component.extra_property_dictionary.validator.duplicateKey'),
              );
            }
            return Promise.resolve();
          },
        },
      },
    },
    table: {
      dataSource: [],
    },
  });
  watch(
    () => props.value,
    (dataSource) => {
      if (!dataSource) {
        state.table.dataSource = [];
        return;
      }
      state.table.dataSource = Object.keys(dataSource).map((key) => {
        return {
          key: key,
          value: dataSource[key],
        };
      });
    },
    {
      immediate: true,
    },
  );

  function handleAddNew() {
    state.form.model = {};
    state.editFlag = false;
    state.modal.title = t('component.extra_property_dictionary.actions.create');
    state.modal.visible = true;
  }

  function handleEdit(record) {
    state.editFlag = true;
    state.form.model = cloneDeep(record);
    state.modal.title = t('component.extra_property_dictionary.actions.update');
    state.modal.visible = true;
  }

  function handleClean() {
    emits('change', {});
    emits('update:value', {});
  }

  function handleDelete(record) {
    const dataSource = state.table.dataSource ?? [];
    const findIndex = dataSource.findIndex((x) => x.key === record.key);
    const changeData: Dictionary<string, string> = {};
    dataSource.splice(findIndex, 1);
    dataSource.forEach((item) => {
      changeData[item.key] = item.value;
    });
    emits('change', changeData);
    emits('update:value', changeData);
  }

  function handleCancel() {
    const form = unref(formRef);
    form?.resetFields();
    state.modal.visible = false;
  }

  function handleSubmit() {
    const form = unref(formRef);
    form?.validate().then(() => {
      const dataSource = state.table.dataSource ?? [];
      const changeData: Dictionary<string, string> = {};
      if (!state.editFlag) {
        dataSource.push(state.form.model);
      } else {
        const findIndex = dataSource.findIndex((x) => x.key === state.form.model.key);
        dataSource[findIndex] = state.form.model;
      }
      dataSource.forEach((item) => {
        changeData[item.key] = item.value;
      });
      emits('change', changeData);
      emits('update:value', changeData);
      form.resetFields();
      state.modal.visible = false;
    });
  }
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-extra-property-dictionary';

  .@{prefix-cls} {
    &__toolbar {
      flex: 1;
      display: flex;
      align-items: center;
      justify-content: flex-end;
      margin-bottom: 8px;

      > * {
        margin-right: 8px;
      }
    }

    &__action {
      display: flex;
      align-items: center;
    }

    &__form {
      margin: 10px;
    }
  }
</style>
