import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { getList as getLanguages } from '/@/api/localization/languages';
import { getList as getResources } from '/@/api/localization/resources';

const { L } = useLocalization(['LocalizationManagement', 'AbpUi']);

export function getSearchFormSchemas(submitFunc?: () => Promise<void>): Partial<FormProps> {
  return {
    labelWidth: 100,
    submitFunc: submitFunc,
    schemas: [
      {
        field: 'cultureName',
        component: 'ApiSelect',
        label: L('DisplayName:CultureName'),
        colProps: { span: 6 },
        required: true,
        componentProps: {
          api: getLanguages,
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'cultureName',
        },
      },
      {
        field: 'targetCultureName',
        component: 'ApiSelect',
        label: L('DisplayName:TargetCultureName'),
        colProps: { span: 6 },
        required: true,
        componentProps: {
          api: getLanguages,
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'cultureName',
        },
      },
      {
        field: 'resourceName',
        component: 'ApiSelect',
        label: L('DisplayName:ResourceName'),
        colProps: { span: 6 },
        componentProps: {
          api: getResources,
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'name',
        },
      },
      {
        field: 'onlyNull',
        component: 'Select',
        label: L('DisplayName:TargetValue'),
        colProps: { span: 6 },
        componentProps: {
          options: [
            {
              key: L('DisplayName:Any'),
              label: L('DisplayName:Any'),
              value: false,
            },
            {
              key: L('DisplayName:OnlyNull'),
              label: L('DisplayName:OnlyNull'),
              value: true,
            },
          ],
        },
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 24 },
      },
    ],
  };
}
