import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';

const { L } = useLocalization(['AbpTextTemplating']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 100,
      sorter: true,
      ellipsis: true,
      defaultHidden: true,
      resizable: true,
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 200,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('DisplayName:IsStatic'),
      dataIndex: 'isStatic',
      align: 'center',
      width: 150,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('DisplayName:IsInlineLocalized'),
      dataIndex: 'isInlineLocalized',
      align: 'center',
      width: 150,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('DisplayName:IsLayout'),
      dataIndex: 'isLayout',
      align: 'center',
      width: 150,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('Layout'),
      dataIndex: 'layout',
      align: 'left',
      width: 200,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('DisplayName:DefaultCultureName'),
      dataIndex: 'defaultCultureName',
      align: 'left',
      width: 200,
      sorter: true,
      ellipsis: true,
      resizable: true,
    },
    {
      title: L('DisplayName:LocalizationResourceName'),
      dataIndex: 'localizationResourceName',
      align: 'left',
      width: 150,
      sorter: true,
      ellipsis: true,
      resizable: true,
      defaultHidden: true,
    },
    {
      title: L('DisplayName:RenderEngine'),
      dataIndex: 'renderEngine',
      align: 'left',
      width: 150,
      sorter: true,
      ellipsis: true,
      resizable: true,
      defaultHidden: true,
    },
  ];
}
