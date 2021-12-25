import { FormSchema } from '/@/components/Form';
import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';
import { getAllList as getRoles } from '/@/api/identity/role';
import { search as getUsers } from '/@/api/identity/userLookup';

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

export function getShareDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:Path'),
      dataIndex: 'path',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('DisplayName:Url'),
      dataIndex: 'url',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('DisplayName:ExpirationTime'),
      dataIndex: 'expirationTime',
      align: 'left',
      width: 200,
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text, 'YYYY-MM-DD HH:mm:ss');
        }
        return text;
      },
    },
    {
      title: L('DisplayName:AccessCount'),
      dataIndex: 'accessCount',
      align: 'left',
      width: 100,
      sorter: true,
    },
    {
      title: L('DisplayName:MaxAccessCount'),
      dataIndex: 'maxAccessCount',
      align: 'left',
      width: 100,
      sorter: true,
    },
  ];
}

export function getShareModalSchemas(): FormSchema[] {
  return [
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
      dynamicDisabled: true,
    },
    {
      field: 'path',
      component: 'Input',
      label: L('DisplayName:Path'),
      colProps: { span: 24 },
      required: true,
      dynamicDisabled: true,
    },
    {
      field: 'expirationTime',
      component: 'DatePicker',
      label: L('DisplayName:ExpirationTime'),
      colProps: { span: 24 },
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
    {
      field: 'maxAccessCount',
      component: 'InputNumber',
      label: L('DisplayName:MaxAccessCount'),
      colProps: { span: 24 },
      defaultValue: 0,
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
    {
      field: 'roles',
      component: 'ApiSelect',
      label: L('DisplayName:AccessRoles'),
      colProps: { span: 24 },
      componentProps: {
        mode: 'multiple',
        api: () => getRoles(),
        resultField: 'items',
        labelField: 'name',
        valueField: 'name',
      },
    },
    {
      field: 'users',
      component: 'ApiSelect',
      label: L('DisplayName:AccessUsers'),
      colProps: { span: 24 },
      componentProps: {
        mode: 'multiple',
        api: () => getUsers({ sorting: '', skipCount: 0, maxResultCount: 100 }),
        resultField: 'items',
        labelField: 'userName',
        valueField: 'userName',
      },
    },
  ];
}
