<template>
  <div style="width: 100%" :class="`${prefixCls}__container`">
    <FormItemRest>
      <Card>
        <template #title>
          <div :class="`${prefixCls}__type`">
            <Row>
              <Col :span="11">
                <span>{{ t('component.value_type_nput.type.name') }}</span>
              </Col>
              <Col :span="2">
                <div style="width: 100%"></div>
              </Col>
              <Col :span="11">
                <span>{{ t('component.value_type_nput.validator.name') }}</span>
              </Col>
            </Row>
            <Row>
              <Col :span="11">
                <Select
                  :disabled="props.disabled"
                  :value="state.valueType.name"
                  @change="handleValueTypeChange"
                >
                  <Option value="FreeTextStringValueType">{{
                    t('component.value_type_nput.type.FREE_TEXT.name')
                  }}</Option>
                  <Option value="ToggleStringValueType">{{
                    t('component.value_type_nput.type.TOGGLE.name')
                  }}</Option>
                  <Option value="SelectionStringValueType">{{
                    t('component.value_type_nput.type.SELECTION.name')
                  }}</Option>
                </Select>
              </Col>
              <Col :span="2">
                <div style="width: 100%"></div>
              </Col>
              <Col :span="11">
                <Select
                  :disabled="props.disabled"
                  :value="state.valueType.validator.name"
                  @change="handleValidatorChange"
                >
                  <Option value="NULL">{{
                    t('component.value_type_nput.validator.NULL.name')
                  }}</Option>
                  <Option
                    value="BOOLEAN"
                    :disabled="state.valueType.name !== 'ToggleStringValueType'"
                  >
                    {{ t('component.value_type_nput.validator.BOOLEAN.name') }}
                  </Option>
                  <Option
                    value="NUMERIC"
                    :disabled="state.valueType.name !== 'FreeTextStringValueType'"
                  >
                    {{ t('component.value_type_nput.validator.NUMERIC.name') }}
                  </Option>
                  <Option
                    value="STRING"
                    :disabled="state.valueType.name !== 'FreeTextStringValueType'"
                  >
                    {{ t('component.value_type_nput.validator.STRING.name') }}
                  </Option>
                </Select>
              </Col>
            </Row>
          </div>
        </template>
        <div :class="`${prefixCls}__wrap`">
          <Row>
            <Col :span="24">
              <div v-if="state.valueType.name === 'FreeTextStringValueType'">
                <div v-if="state.valueType.validator.name === 'NUMERIC'" class="numeric">
                  <Row>
                    <Col :span="11">
                      <span>{{ t('component.value_type_nput.validator.NUMERIC.minValue') }}</span>
                    </Col>
                    <Col :span="2">
                      <div style="width: 100%"></div>
                    </Col>
                    <Col :span="11">
                      <span>{{ t('component.value_type_nput.validator.NUMERIC.maxValue') }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:value="(state.valueType.validator as NumericValueValidator).minValue"
                      />
                    </Col>
                    <Col :span="2">
                      <div style="width: 100%"></div>
                    </Col>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:value="(state.valueType.validator as NumericValueValidator).maxValue"
                      />
                    </Col>
                  </Row>
                </div>
                <div v-else-if="state.valueType.validator.name === 'STRING'" class="string">
                  <Row style="margin-top: 10px">
                    <Col :span="24">
                      <Checkbox
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:checked="(state.valueType.validator as StringValueValidator).allowNull"
                      >
                        {{ t('component.value_type_nput.validator.STRING.allowNull') }}
                      </Checkbox>
                    </Col>
                  </Row>
                  <Row style="margin-top: 10px">
                    <Col :span="24">
                      <span>{{
                        t('component.value_type_nput.validator.STRING.regularExpression')
                      }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="24">
                      <Input
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:value="(state.valueType.validator as StringValueValidator).regularExpression"
                      />
                    </Col>
                  </Row>
                  <Row style="margin-top: 10px">
                    <Col :span="11">
                      <span>{{ t('component.value_type_nput.validator.STRING.minLength') }}</span>
                    </Col>
                    <Col :span="2">
                      <div style="width: 100%"></div>
                    </Col>
                    <Col :span="11">
                      <span>{{ t('component.value_type_nput.validator.STRING.maxLength') }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:value="(state.valueType.validator as StringValueValidator).minLength"
                      />
                    </Col>
                    <Col :span="2">
                      <div style="width: 100%"></div>
                    </Col>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        style="width: 100%"
                        v-model:value="(state.valueType.validator as StringValueValidator).maxLength"
                      />
                    </Col>
                  </Row>
                </div>
              </div>
              <div v-else-if="state.valueType.name === 'SelectionStringValueType'">
                <Card class="selection">
                  <template #title>
                    <Row>
                      <Col :span="12">
                        <div
                          class="valid"
                          v-if="(state.valueType as SelectionStringValueType).itemSource.items.length <= 0"
                        >
                          <span>{{
                            t('component.value_type_nput.type.SELECTION.itemsNotBeEmpty')
                          }}</span>
                        </div>
                      </Col>
                      <Col :span="12">
                        <div class="toolbar" v-if="!props.disabled">
                          <Button type="primary" @click="handleAddNew">{{
                            t('component.value_type_nput.type.SELECTION.actions.create')
                          }}</Button>
                          <Button danger @click="handleClean">{{
                            t('component.value_type_nput.type.SELECTION.actions.clean')
                          }}</Button>
                        </div>
                      </Col>
                    </Row>
                  </template>
                  <Table
                    :columns="getTableColumns"
                    :data-source="(state.valueType as SelectionStringValueType).itemSource.items"
                  >
                    <template #bodyCell="{ column, record }">
                      <template v-if="column.key === 'displayText'">
                        <span>{{ getDisplayName(record.displayText) }}</span>
                      </template>
                      <template v-else-if="column.key === 'action'">
                        <div :class="`${prefixCls}__action`">
                          <Button
                            v-if="props.allowEdit"
                            type="link"
                            @click="() => handleEdit(record)"
                          >
                            <template #icon>
                              <EditOutlined />
                            </template>
                            {{ t('component.value_type_nput.type.SELECTION.actions.update') }}
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
                            {{ t('component.value_type_nput.type.SELECTION.actions.delete') }}
                          </Button>
                        </div>
                      </template>
                    </template>
                  </Table>
                </Card>
              </div>
            </Col>
          </Row>
        </div>
      </Card>
    </FormItemRest>
    <Modal :class="`${prefixCls}__modal`" v-bind="state.selection.modal">
      <Form
        ref="formRef"
        class="form"
        v-bind="state.selection.form"
        :label-col="{ span: 6 }"
        :wrapper-col="{ span: 18 }"
      >
        <FormItem
          name="displayText"
          required
          :label="t('component.value_type_nput.type.SELECTION.displayText')"
        >
          <LocalizableInput
            :disabled="props.disabled || state.selection.form.editFlag"
            v-model:value="state.selection.form.model.displayText"
          />
        </FormItem>
        <FormItem
          name="value"
          required
          :label="t('component.value_type_nput.type.SELECTION.value')"
        >
          <Input :disabled="props.disabled" v-model:value="state.selection.form.model.value" />
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>

<script setup lang="ts">
  import type { ColumnsType } from 'ant-design-vue/lib/table';
  import type { RuleObject } from 'ant-design-vue/lib/form';
  import { computed, reactive, ref, unref, watch } from 'vue';
  import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
  import {
    Button,
    Card,
    Checkbox,
    Col,
    Divider,
    Form,
    Input,
    InputNumber,
    Modal,
    Row,
    Select,
    Table,
  } from 'ant-design-vue';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import { propTypes } from '/@/utils/propTypes';
  import {
    AlwaysValidValueValidator,
    BooleanValueValidator,
    NumericValueValidator,
    StringValueValidator,
  } from './validator';
  import {
    StringValueType,
    FreeTextStringValueType,
    SelectionStringValueType,
    ToggleStringValueType,
    valueTypeSerializer,
  } from './valueType';
  import { LocalizableInput } from '../LocalizableInput';

  const FormItemRest = Form.ItemRest;
  const FormItem = Form.Item;
  const Option = Select.Option;
  const emits = defineEmits([
    'change',
    'update:value',
    'change:valueType',
    'change:validator',
    'change:selection',
  ]);
  const props = defineProps({
    value: propTypes.string.def('{}'),
    allowEdit: propTypes.bool.def(false),
    allowDelete: propTypes.bool.def(false),
    disabled: propTypes.bool.def(false),
  });

  const { t } = useI18n();
  const { Lr } = useLocalization();
  const { deserialize, serialize, validate: validateLocalizer } = useLocalizationSerializer();
  const { prefixCls } = useDesign('string-value-type-input');
  interface Selection {
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
      rules?: Dictionary<string, RuleObject>;
    };
  }
  interface State {
    value?: string;
    valueType: StringValueType;
    selection: Selection;
  }

  const formRef = ref<any>();
  const state = reactive<State>({
    valueType: new FreeTextStringValueType(),
    selection: {
      modal: {
        title: t('component.value_type_nput.type.SELECTION.modal.title'),
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
        rules: {
          displayText: {
            validator: (_rule, value) => {
              if (!validateLocalizer(value)) {
                return Promise.reject(
                  t('component.value_type_nput.type.SELECTION.displayTextNotBeEmpty'),
                );
              }
              const items = (state.valueType as SelectionStringValueType).itemSource.items;
              if (items.findIndex((x) => serialize(x.displayText) === value) >= 0) {
                return Promise.reject(
                  t('component.value_type_nput.type.SELECTION.duplicateKeyOrValue'),
                );
              }
              return Promise.resolve();
            },
          },
          value: {
            validator: (_rule, value) => {
              const items = (state.valueType as SelectionStringValueType).itemSource.items;
              if (items.findIndex((x) => x.value === value) >= 0) {
                return Promise.reject(
                  t('component.value_type_nput.type.SELECTION.duplicateKeyOrValue'),
                );
              }
              return Promise.resolve();
            },
          },
        },
      },
    },
  });
  const getTableColumns = computed(() => {
    const columns: ColumnsType = [
      {
        title: t('component.value_type_nput.type.SELECTION.displayText'),
        dataIndex: 'displayText',
        key: 'displayText',
        align: 'left',
        fixed: 'left',
        width: 180,
      },
      {
        title: t('component.value_type_nput.type.SELECTION.value'),
        dataIndex: 'value',
        key: 'value',
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
              width: 180,
              title: t('component.value_type_nput.type.SELECTION.actions.title'),
              align: 'center',
              dataIndex: 'action',
              key: 'action',
              fixed: 'right',
            },
          ],
    );
  });
  watch(
    () => props.value,
    (value) => {
      if (isNullOrWhiteSpace(value) || value === '{}') {
        state.valueType = new FreeTextStringValueType();
      } else {
        _formatValueType(value);
      }
    },
    {
      immediate: true,
    },
  );
  watch(
    () => state.valueType,
    (valueType) => {
      const isSelection = valueType.name === 'SelectionStringValueType';
      if (isSelection && (valueType as SelectionStringValueType).itemSource.items.length < 0) {
        return;
      }
      state.value = valueTypeSerializer.serialize(valueType);
      emits('change:valueType', state.valueType.name);
      emits('change:validator', state.valueType.validator.name);
      if (isSelection) {
        emits('change:selection', (valueType as SelectionStringValueType).itemSource.items);
      }
    },
    {
      deep: true,
      immediate: false,
    },
  );
  watch(
    () => state.value,
    (value) => {
      emits('change', value);
      emits('update:value', value);
    },
  );
  const getDisplayName = (displayName: LocalizableStringInfo) => {
    return Lr(displayName.resourceName, displayName.name);
  };

  function validate(value: any) {
    if (state.valueType.name === 'SelectionStringValueType') {
      const items = (state.valueType as SelectionStringValueType).itemSource.items;
      if (items.length === 0) {
        return Promise.reject(t('component.value_type_nput.type.SELECTION.itemsNotBeEmpty'));
      }
      if (value && items.findIndex((item) => item.value === value) < 0) {
        return Promise.reject(t('component.value_type_nput.type.SELECTION.itemsNotFound'));
      }
    }
    if (!state.valueType.validator.isValid(value)) {
      return Promise.reject(
        t('component.value_type_nput.validator.isInvalidValue', [
          t(`component.value_type_nput.validator.${state.valueType.validator.name}.name`),
        ]),
      );
    }
    return Promise.resolve(value);
  }

  function _formatValueType(valueTypeString: string) {
    try {
      state.valueType = valueTypeSerializer.deserialize(valueTypeString);
    } catch {
      console.warn('Unable to serialize validator example, check that "valueType" value is valid');
    }
  }

  function handleValueTypeChange(type: string) {
    switch (type) {
      case 'TOGGLE':
      case 'ToggleStringValueType':
        state.valueType = new ToggleStringValueType();
        break;
      case 'SELECTION':
      case 'SelectionStringValueType':
        state.valueType = new SelectionStringValueType();
        break;
      case 'FREE_TEXT':
      case 'FreeTextStringValueType':
      default:
        state.valueType = new FreeTextStringValueType();
        break;
    }
  }

  function handleValidatorChange(validator: string) {
    switch (validator) {
      case 'NULL':
        state.valueType.validator = new AlwaysValidValueValidator();
        break;
      case 'BOOLEAN':
        state.valueType.validator = new BooleanValueValidator();
        break;
      case 'NUMERIC':
        state.valueType.validator = new NumericValueValidator();
        break;
      case 'STRING':
      default:
        state.valueType.validator = new StringValueValidator();
        break;
    }
    emits('change:validator', state.valueType.validator.name);
  }

  function handleAddNew() {
    state.selection.form.model = {};
    state.selection.form.editFlag = false;
    state.selection.modal.visible = true;
  }

  function handleClean() {
    const items = (state.valueType as SelectionStringValueType).itemSource.items;
    items.length = 0;
    emits('change:selection', items);
  }

  function handleEdit(record) {
    state.selection.form.model = {
      value: record.value,
      displayText: serialize(record.displayText),
    };
    state.selection.form.editFlag = true;
    state.selection.modal.visible = true;
  }

  function handleCancel() {
    state.selection.form.model = {};
    state.selection.form.editFlag = false;
    state.selection.modal.visible = false;
  }

  function handleSubmit() {
    const form = unref(formRef);
    form?.validate().then(() => {
      const displayText = state.selection.form.model.displayText;
      const items = (state.valueType as SelectionStringValueType).itemSource.items;
      if (!state.selection.form.editFlag) {
        items.push({
          displayText: deserialize(displayText),
          value: state.selection.form.model.value,
        });
      } else {
        const findIndex = items.findIndex((x) => serialize(x.displayText) === displayText);
        items[findIndex].value = state.selection.form.model.value;
      }
      form.resetFields();
      state.selection.modal.visible = false;
      emits('change:selection', items);
    });
  }

  function handleDelete(record) {
    const displayText = serialize(record.displayText);
    const items = (state.valueType as SelectionStringValueType).itemSource.items;
    const findIndex = items.findIndex((x) => serialize(x.displayText) === displayText);
    items.splice(findIndex, 1);
    emits('change:selection', items);
  }

  defineExpose({
    validate,
  });
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-string-value-type-input';

  .@{prefix-cls} {
    &__type {
      width: 100% !important;
    }

    &__wrap {
      width: 100% !important;

      .selection {
        .valid {
          color: red;
          font-size: 14;
        }

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
  }

  &__modal {
    height: 500px;

    .form {
      margin: 10px;
    }
  }
</style>
