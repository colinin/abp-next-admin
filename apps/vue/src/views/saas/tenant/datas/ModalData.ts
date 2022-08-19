import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';
import { usePasswordValidator } from '/@/hooks/security/usePasswordValidator';
import { FormProps, FormSchema } from '/@/components/Form';
import { getList as getEditions } from '/@/api/saas/editions';

const { L } = useLocalization('AbpSaas');
const { ruleCreator } = useValidation();
const { validate } = usePasswordValidator();

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 120,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 24 },
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      show: false,
      dynamicDisabled: true,
    },
    {
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      show: false,
      dynamicDisabled: true,
    },
    {
      field: 'isActive',
      component: 'Checkbox',
      label: '',
      labelWidth: 50,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('DisplayName:IsActive'),
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:TenantName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'editionId',
      component: 'ApiSelect',
      label: L('DisplayName:EditionName'),
      colProps: { span: 24 },
      componentProps: {
        api: getEditions,
        params: {
          skipCount: 0,
          maxResultCount: 100,
        },
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'id',
      },
    },
    {
      field: 'enableTime',
      component: 'DatePicker',
      label: L('DisplayName:EnableTime'),
      colProps: { span: 24 },
      defaultValue: new Date(),
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
    {
      field: 'disableTime',
      component: 'DatePicker',
      label: L('DisplayName:DisableTime'),
      colProps: { span: 24 },
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
    {
      field: 'adminEmailAddress',
      component: 'Input',
      label: L('DisplayName:AdminEmailAddress'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
    {
      field: 'adminPassword',
      component: 'StrengthMeter',
      label: L('DisplayName:AdminPassword'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
      rules: [
        ...ruleCreator.defineValidator({
          trigger: 'blur',
          required: true,
          validator: (_, value: any) => {
            return validate(value);
          },
        }),
      ],
    },
    {
      field: 'useSharedDatabase',
      component: 'Checkbox',
      label: '',
      colProps: { span: 24 },
      defaultValue: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
      renderComponentContent: L('DisplayName:UseSharedDatabase'),
    },
    {
      field: 'defaultConnectionString',
      component: 'InputTextArea',
      label: L('DisplayName:DefaultConnectionString'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return !values.useSharedDatabase;
      },
      componentProps: {
        rows: 4,
      },
    },
  ];
}

export function getConnectionFormSchemas(): FormSchema[] {
  return [
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: L('DisplayName:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
