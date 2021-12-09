import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';
import { Layout } from '/@/api/platform/model/layoutModel';
import { getByName, getAll } from '/@/api/platform/dataDic';

const { L } = useLocalization('AppPlatform');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('DisplayName:Filter'),
        colProps: { span: 8 },
      },
      {
        field: 'framework',
        component: 'ApiSelect',
        label: L('DisplayName:UIFramework'),
        colProps: { span: 8 },
        componentProps: {
          api: () => getByName('UI Framework'),
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'name',
        },
      },
    ],
  };
}
/**
 * 构建布局表单
 * @param data 布局定义
 * @param uiItems ui框架,来源于数据字典
 * @param consts 布局约束,来源于数据字典
 * @returns 返回表单定义
 */
export function getModalFormSchemas(layout: Layout): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      defaultValue: layout.id,
      show: false,
    },
    {
      field: 'framework',
      component: 'ApiSelect',
      label: L('DisplayName:UIFramework'),
      colProps: { span: 24 },
      required: true,
      show: ({ values }) => {
        return values.id === undefined;
      },
      defaultValue: layout.framework,
      componentProps: {
        api: () => getByName('UI Framework'),
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'name',
      },
    },
    {
      field: 'dataId',
      component: 'ApiSelect',
      label: L('DisplayName:Constraint'),
      colProps: { span: 24 },
      required: true,
      show: ({ values }) => {
        return values.id === undefined;
      },
      defaultValue: layout.dataId,
      componentProps: {
        api: () => getAll(),
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'id',
      },
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
      defaultValue: layout.name,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('DisplayName:DisplayName'),
      colProps: { span: 24 },
      required: true,
      defaultValue: layout.displayName,
    },
    {
      field: 'path',
      component: 'Input',
      label: L('DisplayName:Path'),
      colProps: { span: 24 },
      required: true,
      defaultValue: layout.path,
    },
    {
      field: 'redirect',
      component: 'Input',
      label: L('DisplayName:Redirect'),
      colProps: { span: 24 },
      defaultValue: layout.redirect,
    },
    {
      field: 'description',
      component: 'InputTextArea',
      label: L('DisplayName:Description'),
      colProps: { span: 24 },
      defaultValue: layout.description,
    },
  ];
}
