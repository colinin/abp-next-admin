<script setup lang="ts">
import type {
  DataDto,
  DataItemCreateDto,
  DataItemDto,
  DataItemUpdateDto,
} from '../../types';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDate, formatToDateTime } from '@abp/core';
import { message } from 'ant-design-vue';
import cloneDeep from 'lodash.clonedeep';

import { useDataDictionariesApi } from '../../api';
import { ValueType } from '../../types';

interface ModalState {
  data: DataDto;
  item: DataItemDto;
}

const emits = defineEmits<{
  (event: 'change'): void;
}>();

const { createItemApi, updateItemApi } = useDataDictionariesApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      dependencies: {
        show: false,
        triggerFields: ['name'],
      },
      fieldName: 'id',
    },
    {
      component: 'Input',
      dependencies: {
        disabled: (values) => {
          return !!values?.id;
        },
        triggerFields: ['id'],
      },
      fieldName: 'name',
      label: $t('AppPlatform.DisplayName:Name'),
      rules: 'required',
    },
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'displayName',
      label: $t('AppPlatform.DisplayName:DisplayName'),
      rules: 'required',
    },
    {
      component: 'Select',
      componentProps: {
        onChange(valueType: ValueType) {
          onValueTypeChange(valueType);
        },
        options: [
          { label: 'String', value: ValueType.String },
          { label: 'Number', value: ValueType.Numeic },
          { label: 'Date', value: ValueType.Date },
          { label: 'DateTime', value: ValueType.DateTime },
          { label: 'Boolean', value: ValueType.Boolean },
          { label: 'Array', value: ValueType.Array },
          // TODO: Object 暂不支持
          // { label: 'Object', value: ValueType.Object },
        ],
      },
      fieldName: 'valueType',
      label: $t('AppPlatform.DisplayName:ValueType'),
      rules: 'selectRequired',
    },
    {
      component: 'Input',
      fieldName: 'defaultValue',
      label: $t('AppPlatform.DisplayName:DefaultValue'),
    },
    {
      component: 'Checkbox',
      fieldName: 'allowBeNull',
      label: $t('AppPlatform.DisplayName:AllowBeNull'),
    },
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 3,
        },
      },
      fieldName: 'description',
      label: $t('AppPlatform.DisplayName:Description'),
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange(isOpen) {
    if (isOpen) {
      onInit();
    }
  },
});

function onInit() {
  formApi.resetForm();
  const state = modalApi.getData<ModalState>();
  let title = $t('AppPlatform.Data:AppendItem');
  if (state.item?.id) {
    onValueTypeChange(state.item.valueType);
    formApi.setValues({
      ...state.item,
      defaultValue: mapFromDefaultValue(state.item),
    });
    title = `${$t('AppPlatform.Data:EditItem')} - ${state.item.name}`;
  } else {
    formApi.updateSchema([
      {
        component: 'Input',
        componentProps: {
          autocomplete: 'off',
        },
        dependencies: {
          rules(values) {
            return values.allowBeNull ? null : 'required';
          },
          triggerFields: ['valueType', 'allowBeNull'],
        },
        fieldName: 'defaultValue',
      },
    ]);
    formApi.setValues({
      allowBeNull: true,
    });
  }
  modalApi.setState({ title });
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const state = modalApi.getData<ModalState>();
    const input = cloneDeep(values);
    input.defaultValue = mapToDefaultValue(input.valueType, input.defaultValue);
    const api = input.id
      ? updateItemApi(state.data.id, input.name, input as DataItemUpdateDto)
      : createItemApi(state.data.id, input as DataItemCreateDto);
    await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change');
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}

function onValueTypeChange(valueType: ValueType) {
  let component = 'Input';
  let componentProps: Record<string, any> | undefined;
  switch (valueType) {
    case ValueType.Array: {
      component = 'Select';
      componentProps = {
        mode: 'tags',
      };
      break;
    }
    case ValueType.Boolean: {
      component = 'Checkbox';
      break;
    }
    case ValueType.Date:
    case ValueType.DateTime: {
      component = 'DatePicker';
      componentProps = {
        showTime: valueType === ValueType.DateTime,
        valueFormat:
          valueType === ValueType.DateTime
            ? 'YYYY-MM-DD HH:mm:ss'
            : 'YYYY-MM-DD',
      };
      break;
    }
    case ValueType.Numeic: {
      component = 'InputNumber';
      break;
    }
    case ValueType.String: {
      componentProps = {
        autocomplete: 'off',
      };
      break;
    }
  }
  formApi.updateSchema([
    {
      component,
      componentProps,
      dependencies: {
        rules(values) {
          return values.allowBeNull ? null : 'required';
        },
        triggerFields: ['valueType', 'allowBeNull'],
      },
      fieldName: 'defaultValue',
    },
  ]);
  formApi.setFieldValue('defaultValue', undefined);
}

function mapToDefaultValue(valueType: ValueType, defaultValue?: any) {
  if (!defaultValue) {
    return undefined;
  }
  switch (valueType) {
    case ValueType.Array: {
      return (defaultValue as string[]).join(',');
    }
    case ValueType.Boolean: {
      return (defaultValue as Boolean).toString();
    }
    case ValueType.Date: {
      return formatToDate(defaultValue);
    }
    case ValueType.DateTime: {
      return formatToDateTime(defaultValue);
    }
    case ValueType.Numeic: {
      return (defaultValue as Number).toString();
    }
  }
  return defaultValue;
}

function mapFromDefaultValue(dataItem: DataItemDto) {
  if (!dataItem.defaultValue) {
    return undefined;
  }
  let defaultValue: any | undefined;
  switch (dataItem.valueType) {
    case ValueType.Array: {
      defaultValue = dataItem.defaultValue.split(',');
      break;
    }
    case ValueType.Boolean: {
      defaultValue = Boolean(dataItem.defaultValue);
      break;
    }
    case ValueType.Date: {
      defaultValue = formatToDate(dataItem.defaultValue);
      break;
    }
    case ValueType.DateTime: {
      defaultValue = formatToDateTime(dataItem.defaultValue);
      break;
    }
    case ValueType.Numeic: {
      defaultValue = Number(dataItem.defaultValue);
      break;
    }
    case ValueType.String: {
      defaultValue = dataItem.defaultValue;
      break;
    }
  }
  return defaultValue;
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
