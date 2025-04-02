<!-- eslint-disable vue/custom-event-name-casing -->
<script setup lang="ts">
import type { LocalizableStringInfo } from '@abp/core';
import type { RuleObject } from 'ant-design-vue/lib/form';
import type { ColumnsType } from 'ant-design-vue/lib/table';

import type { StringValueTypeInstance } from './interface';
import type { SelectionStringValueItem, StringValueType } from './valueType';

import { computed, reactive, ref, unref, watch } from 'vue';

import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import {
  isNullOrWhiteSpace,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import {
  Button,
  Card,
  Checkbox,
  Col,
  Form,
  Input,
  InputNumber,
  Modal,
  Row,
  Select,
  Table,
} from 'ant-design-vue';

import LocalizableInput from '../localizable-input/LocalizableInput.vue';
import {
  AlwaysValidValueValidator,
  BooleanValueValidator,
  NumericValueValidator,
  StringValueValidator,
} from './validator';
import {
  FreeTextStringValueType,
  SelectionStringValueType,
  ToggleStringValueType,
  valueTypeSerializer,
} from './valueType';

interface Props {
  allowDelete: boolean;
  allowEdit: boolean;
  disabled: boolean;
  value: string;
}

const props = withDefaults(defineProps<Props>(), {
  allowDelete: false,
  allowEdit: false,
  disabled: false,
  value: '{}',
});
const emits = defineEmits<{
  (event: 'update:value', data: string | undefined): void;
  (event: 'change', data: string | undefined): void;
  (event: 'change:valueType', data: string): void;
  (event: 'change:validator', data: string): void;
  (event: 'change:selection', data: SelectionStringValueItem[]): void;
}>();
const FormItemRest = Form.ItemRest;
const FormItem = Form.Item;
const Option = Select.Option;
const DeleteOutlined = createIconifyIcon('ant-design:delete-outlined');
const EditOutlined = createIconifyIcon('ant-design:edit-outlined');

const { Lr } = useLocalization();
const {
  deserialize,
  serialize,
  validate: validateLocalizer,
} = useLocalizationSerializer();
interface Selection {
  form: {
    editFlag: boolean;
    model: any;
    rules?: Record<string, RuleObject>;
  };
  modal: {
    maskClosable?: boolean;
    minHeight?: number;
    onCancel?: (e: MouseEvent) => void;
    onOk?: (e: MouseEvent) => void;
    title?: string;
    visible?: boolean;
    width?: number;
  };
}
interface State {
  selection: Selection;
  value?: string;
  valueType: StringValueType;
}

const formRef = ref<any>();
const state = reactive<State>({
  selection: {
    form: {
      editFlag: false,
      model: {},
      rules: {
        displayText: {
          validator: (_rule, value) => {
            if (!validateLocalizer(value)) {
              return Promise.reject(
                $t(
                  'component.value_type_nput.type.SELECTION.displayTextNotBeEmpty',
                ),
              );
            }
            const items = (state.valueType as SelectionStringValueType)
              .itemSource.items;
            if (items.some((x) => serialize(x.displayText) === value)) {
              return Promise.reject(
                $t(
                  'component.value_type_nput.type.SELECTION.duplicateKeyOrValue',
                ),
              );
            }
            return Promise.resolve();
          },
        },
        value: {
          validator: (_rule, value) => {
            const items = (state.valueType as SelectionStringValueType)
              .itemSource.items;
            if (items.some((x) => x.value === value)) {
              return Promise.reject(
                $t(
                  'component.value_type_nput.type.SELECTION.duplicateKeyOrValue',
                ),
              );
            }
            return Promise.resolve();
          },
        },
      },
    },
    modal: {
      maskClosable: false,
      minHeight: 400,
      onCancel: handleCancel,
      onOk: handleSubmit,
      title: $t('component.value_type_nput.type.SELECTION.modal.title'),
      visible: false,
      width: 600,
    },
  },
  valueType: new FreeTextStringValueType(),
});
const getTableColumns = computed(() => {
  const columns: ColumnsType = [
    {
      align: 'left',
      dataIndex: 'displayText',
      fixed: 'left',
      key: 'displayText',
      title: $t('component.value_type_nput.type.SELECTION.displayText'),
      width: 180,
    },
    {
      align: 'left',
      dataIndex: 'value',
      fixed: 'left',
      key: 'value',
      title: $t('component.value_type_nput.type.SELECTION.value'),
      width: 200,
    },
  ];
  if (!props.disabled) {
    columns.push({
      align: 'center',
      dataIndex: 'action',
      fixed: 'right',
      key: 'action',
      title: $t('component.value_type_nput.type.SELECTION.actions.title'),
      width: 220,
    });
  }
  return columns;
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
    if (
      isSelection &&
      (valueType as SelectionStringValueType).itemSource.items.length < 0
    ) {
      return;
    }
    state.value = valueTypeSerializer.serialize(valueType);
    emits('change:valueType', state.valueType.name);
    emits('change:validator', state.valueType.validator.name);
    if (isSelection) {
      emits(
        'change:selection',
        (valueType as SelectionStringValueType).itemSource.items,
      );
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
    const items = (state.valueType as SelectionStringValueType).itemSource
      .items;
    if (items.length === 0) {
      return Promise.reject(
        $t('component.value_type_nput.type.SELECTION.itemsNotBeEmpty'),
      );
    }
    if (value && !items.some((item) => item.value === value)) {
      return Promise.reject(
        $t('component.value_type_nput.type.SELECTION.itemsNotFound'),
      );
    }
  }
  if (!state.valueType.validator.isValid(value)) {
    return Promise.reject(
      $t('component.value_type_nput.validator.isInvalidValue', [
        $t(
          `component.value_type_nput.validator.${state.valueType.validator.name}.name`,
        ),
      ]),
    );
  }
  return Promise.resolve(value);
}

function _formatValueType(valueTypeString: string) {
  try {
    state.valueType = valueTypeSerializer.deserialize(valueTypeString);
  } catch {
    console.warn(
      'Unable to serialize validator example, check that "valueType" value is valid',
    );
  }
}

function handleValueTypeChange(type?: string) {
  switch (type) {
    case 'SELECTION':
    case 'SelectionStringValueType': {
      state.valueType = new SelectionStringValueType();
      break;
    }
    case 'TOGGLE':
    case 'ToggleStringValueType': {
      state.valueType = new ToggleStringValueType();
      break;
    }
    default: {
      state.valueType = new FreeTextStringValueType();
      break;
    }
  }
}

function handleValidatorChange(validator?: string) {
  switch (validator) {
    case 'BOOLEAN': {
      state.valueType.validator = new BooleanValueValidator();
      break;
    }
    case 'NULL': {
      state.valueType.validator = new AlwaysValidValueValidator();
      break;
    }
    case 'NUMERIC': {
      state.valueType.validator = new NumericValueValidator();
      break;
    }
    default: {
      state.valueType.validator = new StringValueValidator();
      break;
    }
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

function handleEdit(record: Record<string, any>) {
  state.selection.form.model = {
    displayText: serialize(record.displayText),
    value: record.value,
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
    const items = (state.valueType as SelectionStringValueType).itemSource
      .items;
    if (state.selection.form.editFlag) {
      const findIndex = items.findIndex(
        (x) => serialize(x.displayText) === displayText,
      );
      items[findIndex] &&
        (items[findIndex].value = state.selection.form.model.value);
    } else {
      items.push({
        displayText: deserialize(displayText),
        value: state.selection.form.model.value,
      });
    }
    form.resetFields();
    state.selection.modal.visible = false;
    emits('change:selection', items);
  });
}

function handleDelete(record: Record<string, any>) {
  const displayText = serialize(record.displayText);
  const items = (state.valueType as SelectionStringValueType).itemSource.items;
  const findIndex = items.findIndex(
    (x) => serialize(x.displayText) === displayText,
  );
  items.splice(findIndex, 1);
  emits('change:selection', items);
}

defineExpose<StringValueTypeInstance>({
  validate,
});
</script>

<template>
  <div class="container">
    <FormItemRest>
      <Card>
        <template #title>
          <div class="type">
            <Row>
              <Col :span="11">
                <span>{{ $t('component.value_type_nput.type.name') }}</span>
              </Col>
              <Col :span="2">
                <div class="w-full"></div>
              </Col>
              <Col :span="11">
                <span>{{
                  $t('component.value_type_nput.validator.name')
                }}</span>
              </Col>
            </Row>
            <Row>
              <Col :span="11">
                <Select
                  :disabled="props.disabled"
                  :value="state.valueType.name"
                  @change="(value) => handleValueTypeChange(value?.toString())"
                >
                  <Option value="FreeTextStringValueType">
                    {{ $t('component.value_type_nput.type.FREE_TEXT.name') }}
                  </Option>
                  <Option value="ToggleStringValueType">
                    {{ $t('component.value_type_nput.type.TOGGLE.name') }}
                  </Option>
                  <Option value="SelectionStringValueType">
                    {{ $t('component.value_type_nput.type.SELECTION.name') }}
                  </Option>
                </Select>
              </Col>
              <Col :span="2">
                <div class="w-full"></div>
              </Col>
              <Col :span="11">
                <Select
                  :disabled="props.disabled"
                  :value="state.valueType.validator.name"
                  @change="(value) => handleValidatorChange(value?.toString())"
                >
                  <Option value="NULL">
                    {{ $t('component.value_type_nput.validator.NULL.name') }}
                  </Option>
                  <Option
                    value="BOOLEAN"
                    :disabled="state.valueType.name !== 'ToggleStringValueType'"
                  >
                    {{ $t('component.value_type_nput.validator.BOOLEAN.name') }}
                  </Option>
                  <Option
                    value="NUMERIC"
                    :disabled="
                      state.valueType.name !== 'FreeTextStringValueType'
                    "
                  >
                    {{ $t('component.value_type_nput.validator.NUMERIC.name') }}
                  </Option>
                  <Option
                    value="STRING"
                    :disabled="
                      state.valueType.name !== 'FreeTextStringValueType'
                    "
                  >
                    {{ $t('component.value_type_nput.validator.STRING.name') }}
                  </Option>
                </Select>
              </Col>
            </Row>
          </div>
        </template>
        <div class="wrap">
          <Row>
            <Col :span="24">
              <div v-if="state.valueType.name === 'FreeTextStringValueType'">
                <div
                  v-if="state.valueType.validator.name === 'NUMERIC'"
                  class="numeric"
                >
                  <Row>
                    <Col :span="11">
                      <span>{{
                        $t(
                          'component.value_type_nput.validator.NUMERIC.minValue',
                        )
                      }}</span>
                    </Col>
                    <Col :span="2">
                      <div class="w-full"></div>
                    </Col>
                    <Col :span="11">
                      <span>{{
                        $t(
                          'component.value_type_nput.validator.NUMERIC.maxValue',
                        )
                      }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:value="
                          (state.valueType.validator as NumericValueValidator)
                            .minValue
                        "
                      />
                    </Col>
                    <Col :span="2">
                      <div class="w-full"></div>
                    </Col>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:value="
                          (state.valueType.validator as NumericValueValidator)
                            .maxValue
                        "
                      />
                    </Col>
                  </Row>
                </div>
                <div
                  v-else-if="state.valueType.validator.name === 'STRING'"
                  class="string"
                >
                  <Row style="margin-top: 10px">
                    <Col :span="24">
                      <Checkbox
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:checked="
                          (state.valueType.validator as StringValueValidator)
                            .allowNull
                        "
                      >
                        {{
                          $t(
                            'component.value_type_nput.validator.STRING.allowNull',
                          )
                        }}
                      </Checkbox>
                    </Col>
                  </Row>
                  <Row style="margin-top: 10px">
                    <Col :span="24">
                      <span>{{
                        $t(
                          'component.value_type_nput.validator.STRING.regularExpression',
                        )
                      }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="24">
                      <Input
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:value="
                          (state.valueType.validator as StringValueValidator)
                            .regularExpression
                        "
                      />
                    </Col>
                  </Row>
                  <Row style="margin-top: 10px">
                    <Col :span="11">
                      <span>{{
                        $t(
                          'component.value_type_nput.validator.STRING.minLength',
                        )
                      }}</span>
                    </Col>
                    <Col :span="2">
                      <div class="w-full"></div>
                    </Col>
                    <Col :span="11">
                      <span>{{
                        $t(
                          'component.value_type_nput.validator.STRING.maxLength',
                        )
                      }}</span>
                    </Col>
                  </Row>
                  <Row>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:value="
                          (state.valueType.validator as StringValueValidator)
                            .minLength
                        "
                      />
                    </Col>
                    <Col :span="2">
                      <div class="w-full"></div>
                    </Col>
                    <Col :span="11">
                      <InputNumber
                        :disabled="props.disabled"
                        class="w-full"
                        v-model:value="
                          (state.valueType.validator as StringValueValidator)
                            .maxLength
                        "
                      />
                    </Col>
                  </Row>
                </div>
              </div>
              <div
                v-else-if="state.valueType.name === 'SelectionStringValueType'"
              >
                <Card class="selection">
                  <template #title>
                    <Row>
                      <Col :span="12">
                        <div
                          class="valid"
                          v-if="
                            (state.valueType as SelectionStringValueType)
                              .itemSource.items.length <= 0
                          "
                        >
                          <span>{{
                            $t(
                              'component.value_type_nput.type.SELECTION.itemsNotBeEmpty',
                            )
                          }}</span>
                        </div>
                      </Col>
                      <Col :span="12">
                        <div class="toolbar" v-if="!props.disabled">
                          <Button type="primary" @click="handleAddNew">
                            {{
                              $t(
                                'component.value_type_nput.type.SELECTION.actions.create',
                              )
                            }}
                          </Button>
                          <Button danger @click="handleClean">
                            {{
                              $t(
                                'component.value_type_nput.type.SELECTION.actions.clean',
                              )
                            }}
                          </Button>
                        </div>
                      </Col>
                    </Row>
                  </template>
                  <Table
                    :columns="getTableColumns"
                    :data-source="
                      (state.valueType as SelectionStringValueType).itemSource
                        .items
                    "
                  >
                    <template #bodyCell="{ column, record }">
                      <template v-if="column.key === 'displayText'">
                        <span>{{ getDisplayName(record.displayText) }}</span>
                      </template>
                      <template v-else-if="column.key === 'action'">
                        <div class="flex flex-row items-center gap-1">
                          <Button
                            v-if="props.allowEdit"
                            type="link"
                            @click="() => handleEdit(record)"
                          >
                            <template #icon>
                              <EditOutlined />
                            </template>
                            {{
                              $t(
                                'component.value_type_nput.type.SELECTION.actions.update',
                              )
                            }}
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
                            {{
                              $t(
                                'component.value_type_nput.type.SELECTION.actions.delete',
                              )
                            }}
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
    <Modal class="modal" v-bind="state.selection.modal">
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
          :label="$t('component.value_type_nput.type.SELECTION.displayText')"
        >
          <LocalizableInput
            :disabled="props.disabled || state.selection.form.editFlag"
            v-model:value="state.selection.form.model.displayText"
          />
        </FormItem>
        <FormItem
          name="value"
          required
          :label="$t('component.value_type_nput.type.SELECTION.value')"
        >
          <Input
            :disabled="props.disabled"
            v-model:value="state.selection.form.model.value"
          />
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>

<style lang="less" scoped>
.container {
  .type {
    width: 100% !important;
    min-height: 70px;
  }

  .wrap {
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

.modal {
  height: 500px;

  .form {
    margin: 10px;
  }
}
</style>
