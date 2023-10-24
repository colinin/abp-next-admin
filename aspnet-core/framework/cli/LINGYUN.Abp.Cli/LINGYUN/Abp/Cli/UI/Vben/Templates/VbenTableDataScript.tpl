import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization(['{{model.remote_service}}', 'AbpUi']);

export function getDataColumns(): BasicColumn[] {
  return [
    {{~ for ouputModel in model.ouput_models ~}}
    {
      title: L('{{ ouputModel.display_name }}'),
      dataIndex: '{{ ouputModel.name }}',
      align: '{{ ouputModel.align }}',
      width: {{ ouputModel.width }},
      sorter: {{ ouputModel.sorter }},
      resizable: {{ ouputModel.resizable }},
      {{~ if !ouputModel.show ~}}
      ifShow: false,
      {{~ end ~}}
      {{~ if ouputModel.has_date ~}}
      format: (text) => {
        return text ? formatToDateTime(text) : text;
      },
      {{~ end ~}}
    },
    {{~ end ~}}
  ];
}
