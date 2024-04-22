import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization(['AbpPermissionManagement', 'AbpUi']);

export function getSearchFormSchemas():FormSchema[] {
  return [
    {
      field: 'groupName',
      component: 'ApiSelect',
      label: L('DisplayName:GroupName'),
      colProps: { span: 6 },
      slot: 'groupName',
    },
    {
      field: 'filter',
      component: 'Input',
      label: L('Search'),
      colProps: { span: 18 },
    },
  ];
}
