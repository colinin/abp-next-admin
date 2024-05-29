<template>
  <div :class="`${prefixCls}__container`">
    <div class="card">
      <Card>
        <template #title>
          <Row>
            <Col :span="12">
              <span>{{ t('component.simple_state_checking.title') }}</span>
            </Col>
            <Col :span="12">
              <div class="toolbar" v-if="!props.disabled">
                <Button type="primary" @click="handleAddNew">{{
                  t('component.simple_state_checking.actions.create')
                }}</Button>
                <Button danger @click="handleClean">{{
                  t('component.simple_state_checking.actions.clean')
                }}</Button>
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
                <Tag v-for="feature in record.featureNames">{{ feature }}</Tag>
              </div>
              <div v-else-if="record.name === 'G'">
                <Tag v-for="feature in record.globalFeatureNames">{{ feature }}</Tag>
              </div>
              <div v-else-if="record.name === 'P'">
                <Tag v-for="permission in record.model.permissions">{{ permission }}</Tag>
              </div>
              <div v-else-if="record.name === 'A'">
                <span>{{ t('component.simple_state_checking.requireAuthenticated.title') }}</span>
              </div>
            </template>
            <template v-else-if="column.key === 'action'">
              <div :class="`${prefixCls}__action`">
                <Button v-if="props.allowEdit" type="link" @click="() => handleEdit(record)">
                  <template #icon>
                    <EditOutlined />
                  </template>
                  {{ t('component.simple_state_checking.actions.update') }}
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
                  {{ t('component.simple_state_checking.actions.delete') }}
                </Button>
              </div>
            </template>
          </template>
        </Table>
      </Card>
    </div>
    <Modal :class="`${prefixCls}__modal`" v-bind="state.modal">
      <Form
        ref="formRef"
        class="form"
        v-bind="state.form"
        :label-col="{ span: 6 }"
        :wrapper-col="{ span: 16 }"
      >
        <FormItem name="name" required :label="t('component.simple_state_checking.form.name')">
          <Select
            :disabled="state.form.editFlag"
            :options="getStateCheckerOptions"
            v-model:value="state.form.model.name"
            @change="handleStateCheckerChange"
          />
        </FormItem>
        <component
          :is="componentsRef[state.form.editComponent]"
          v-model:value="state.form.model.stateChecker"
        />
      </Form>
    </Modal>
  </div>
</template>

<script setup lang="ts">
  import type { RuleObject } from 'ant-design-vue/lib/form';
  import type { ColumnsType } from 'ant-design-vue/lib/table';
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, ref, unref, shallowRef, shallowReactive } from 'vue';
  import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
  import {
    Button,
    Card,
    Col,
    Divider,
    Empty,
    Form,
    Modal,
    Row,
    Select,
    Table,
    Tag,
  } from 'ant-design-vue';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { isNullOrUnDef } from '/@/utils/is';
  import { SimplaCheckStateBase } from './typing';
  import { useSimpleStateCheck } from '/@/hooks/abp/useSimpleStateCheck';
  import { propTypes } from '/@/utils/propTypes';
  import RequireGlobalFeaturesSimpleStateChecker from './src/globalFeatures/RequireGlobalFeaturesSimpleStateChecker.vue';
  import RequirePermissionsSimpleStateChecker from './src/permissions/RequirePermissionsSimpleStateChecker.vue';
  import RequireFeaturesSimpleStateChecker from './src/features/RequireFeaturesSimpleStateChecker.vue';

  const FormItem = Form.Item;
  interface State {
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
      editFlag: boolean;
      editComponent: string;
      rules?: Dictionary<string, RuleObject>;
    };
  }
  const emits = defineEmits(['change', 'update:value']);
  const props = defineProps({
    value: propTypes.string,
    state: {
      type: Object as PropType<SimplaCheckStateBase>,
      required: true,
    },
    allowEdit: propTypes.bool.def(false),
    allowDelete: propTypes.bool.def(false),
    disabled: propTypes.bool.def(false),
  });

  const { t } = useI18n();
  const { serializer } = useSimpleStateCheck();
  const { prefixCls } = useDesign('simple-state-checking');
  const formRef = ref<any>();
  const componentsRef = shallowRef({
    Empty: Empty,
    F: RequireFeaturesSimpleStateChecker,
    P: RequirePermissionsSimpleStateChecker,
    G: RequireGlobalFeaturesSimpleStateChecker,
  });
  const simpleCheckerMap = shallowReactive({
    F: t('component.simple_state_checking.requireFeatures.title'),
    G: t('component.simple_state_checking.requireGlobalFeatures.title'),
    P: t('component.simple_state_checking.requirePermissions.title'),
    A: t('component.simple_state_checking.requireAuthenticated.title'),
  });
  const state = reactive<State>({
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
      editFlag: false,
      editComponent: 'Empty',
    },
  });
  const getTableColumns = computed(() => {
    const columns: ColumnsType = [
      {
        title: t('component.simple_state_checking.table.name'),
        dataIndex: 'name',
        key: 'name',
        align: 'left',
        fixed: 'left',
        width: 130,
      },
      {
        title: t('component.simple_state_checking.table.properties'),
        dataIndex: 'properties',
        key: 'properties',
        align: 'left',
        fixed: 'left',
        width: 200,
      },
    ];
    return columns.concat(
      props.disabled
        ? []
        : [
            {
              width: 230,
              title: t('component.simple_state_checking.table.actions'),
              align: 'center',
              dataIndex: 'action',
              key: 'action',
              fixed: 'right',
            },
          ],
    );
  });
  const getSimpleCheckers = computed(() => {
    if (isNullOrUnDef(props.value) || props.value.length === 0) {
      return [];
    }
    const simpleCheckers = serializer.deserializeArray(props.value, props.state);
    return simpleCheckers;
  });
  const getStateCheckerOptions = computed(() => {
    const stateCheckers = unref(getSimpleCheckers);
    return Object.keys(simpleCheckerMap).map((key) => {
      return {
        label: simpleCheckerMap[key],
        disabled: stateCheckers.some((x) => (x as any).name === key),
        value: key,
      };
    });
  });

  function handleAddNew() {
    state.form.editFlag = false;
    state.form.editComponent = 'Empty';
    state.form.model = {};
    state.modal.title = t('component.simple_state_checking.title');
    state.modal.visible = true;
  }

  function handleStateCheckerChange(value: string) {
    const stateChecker = serializer.deserialize(
      {
        T: value,
        A: true,
        N: [],
      },
      props.state,
    );
    state.form.model.stateChecker = stateChecker;
    state.form.editComponent = value;
  }

  function handleEdit(record) {
    state.form.editFlag = true;
    state.form.editComponent = record.name;
    state.form.model = {
      name: record.name,
      stateChecker: cloneDeep(record),
    };
    state.modal.title = simpleCheckerMap[record.name];
    state.modal.visible = true;
  }

  function handleDelete(record) {
    const stateCheckers = unref(getSimpleCheckers);
    const filtedStateCheckers = stateCheckers.filter((x) => (x as any).name !== record.name);
    if (filtedStateCheckers.length === 0) {
      handleClean();
      return;
    }
    const serializedCheckers = serializer.serializeArray(filtedStateCheckers);
    emits('change', serializedCheckers);
    emits('update:value', serializedCheckers);
  }

  function handleClean() {
    emits('change', undefined);
    emits('update:value', undefined);
  }

  function handleCancel() {
    state.form.model = {};
    state.form.editFlag = false;
    state.form.editComponent = 'Empty';
    state.modal.visible = false;
  }

  function handleSubmit() {
    const form = unref(formRef);
    form?.validate().then(() => {
      const input = cloneDeep(state.form.model.stateChecker);
      const stateCheckers = cloneDeep(unref(getSimpleCheckers));
      const inputIndex = stateCheckers.findIndex((x) => (x as any).name === input.name);
      if (inputIndex >= 0) {
        stateCheckers[inputIndex] = input;
      } else {
        stateCheckers.push(input);
      }
      const updateValue = serializer.serializeArray(stateCheckers);
      emits('change', updateValue);
      emits('update:value', updateValue);
      form?.resetFields();
      state.modal.visible = false;
    });
  }
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-simple-state-checking';

  .@{prefix-cls} {
    &__container {
      width: 100%;

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
    }

    &__modal {
      .form {
        margin: 20px;
      }
    }
  }
</style>
