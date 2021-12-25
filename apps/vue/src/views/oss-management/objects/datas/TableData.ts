import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpOssManagement');
const kbUnit = 1 * 1024;
const mbUnit = kbUnit * 1024;
const gbUnit = mbUnit * 1024;

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 300,
      sorter: true,
      slots: {
        customRender: 'enable',
      },
    },
    {
      title: L('DisplayName:FileType'),
      dataIndex: 'isFolder',
      align: 'left',
      width: 120,
      sorter: true,
      format: (text) => {
        return Boolean(text) ? L('DisplayName:Folder') : L('DisplayName:Standard');
      },
    },
    {
      title: L('DisplayName:Size'),
      dataIndex: 'size',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        const size = Number(text);
        if (size > gbUnit) {
          let gb = Math.round(size / gbUnit);
          if (gb < 1) {
            gb = 1;
          }
          return gb + ' GB';
        }
        if (size > mbUnit) {
          let mb = Math.round(size / mbUnit);
          if (mb < 1) {
            mb = 1;
          }
          return mb + ' MB';
        }
        let kb = Math.round(size / kbUnit);
        if (kb < 1) {
          kb = 1;
        }
        return kb + ' KB';
      },
    },
    {
      title: L('DisplayName:CreationDate'),
      dataIndex: 'creationDate',
      align: 'left',
      width: 160,
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text, 'YYYY-MM-DD HH:mm:ss');
        }
        return text;
      },
    },
    {
      title: L('DisplayName:LastModifiedDate'),
      dataIndex: 'lastModifiedDate',
      align: 'left',
      width: 'auto',
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text, 'YYYY-MM-DD HH:mm:ss');
        }
        return text;
      },
    },
  ];
}
