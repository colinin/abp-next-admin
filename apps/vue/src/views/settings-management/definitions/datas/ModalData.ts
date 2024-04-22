import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization(['AbpSettingManagement', 'AbpUi']);

export function getSearchFormSchemas():FormSchema[] {
  return [
    {
      field: 'providerName',
      component: 'Select',
      label: L('DisplayName:Providers'),
      colProps: { span: 6 },
      componentProps: {
        options: [
          { label: L('Providers:Default'), value: 'D' },
        { label: L('Providers:Configuration'), value: 'C' },
        { label: L('Providers:Global'), value: 'G' },
        { label: L('Providers:Tenant'), value: 'T' },
        { label: L('Providers:User'), value: 'U' },
        ],
      }
    },
    {
      field: 'filter',
      component: 'Input',
      label: L('Search'),
      colProps: { span: 18 },
    },
  ];
}
