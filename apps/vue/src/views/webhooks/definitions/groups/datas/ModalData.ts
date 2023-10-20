import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization(['AbpUi']);

export function getSearchFormSchemas():FormSchema[] {
  return [
    {
      field: 'filter',
      component: 'Input',
      label: L('Search'),
      colProps: { span: 24 },
    },
  ];
}
