import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form/index';

import { valueTypeOptions } from './BasicType';

const { L } = useLocalization('AppPlatform');

/**
 * 构建数据字典表单
 * @param data 数据字典
 * @returns 返回表单定义
 */
export function getDateFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'parentId',
      component: 'Input',
      label: 'parentId',
      colProps: { span: 24 },
      show: false,
      defaultValue: undefined,
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('DisplayName:DisplayName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'description',
      component: 'InputTextArea',
      label: L('DisplayName:Description'),
      colProps: { span: 24 },
    },
  ];
}

/**
 * 构建数据字典项目表单
 * @param dataItem 数据字典项目
 * @returns 返回表单定义
 */
export function getDataItemFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      defaultValue: undefined,
      show: false,
    },
    {
      field: 'dataId',
      component: 'Input',
      label: 'dataId',
      colProps: { span: 24 },
      defaultValue: undefined,
      show: false,
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
      dynamicDisabled: ({ model }) => {
        return model.id ? true : false;
      },
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('DisplayName:DisplayName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'valueType',
      component: 'Select',
      label: L('DisplayName:ValueType'),
      colProps: { span: 24 },
      required: true,
      componentProps: {
        options: valueTypeOptions,
      },
    },
    {
      field: 'defaultValue',
      component: 'Input',
      label: L('DisplayName:DefaultValue'),
      colProps: { span: 24 },
      // dynamicRules: ({ values }) => {
      //   return [
      //     {
      //       required: values.allowBeNull !== undefined || !values.allowBeNull,
      //     },
      //   ];
      // },
    },
    {
      field: 'allowBeNull',
      component: 'Checkbox',
      label: L('DisplayName:AllowBeNull'),
      defaultValue: true,
      colProps: { span: 24 },
    },
    {
      field: 'description',
      component: 'InputTextArea',
      label: L('DisplayName:Description'),
      colProps: { span: 24 },
    },
  ];
}
