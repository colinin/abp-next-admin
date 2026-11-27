import { computed } from 'vue';

import { useLocalization } from '@abp/core';

import { ChangeType } from '../types/entity-changes';

export function useAuditlogs() {
  const { L } = useLocalization(['AbpAuditLogging', 'AbpUi']);

  const changeTypeColorMap = {
    [ChangeType.Created]: { color: '#87d068', value: L('Created') },
    [ChangeType.Deleted]: { color: '#ff4d4f', value: L('Deleted') },
    [ChangeType.Updated]: { color: '#108ee9', value: L('Updated') },
  };

  const methodColorMap: { [key: string]: string } = {
    DELETE: '#ff4d4f',
    GET: '#108ee9',
    OPTIONS: '#13c2c2',
    PATCH: '#eb2f96',
    POST: '#52c41a',
    PUT: '#fa8c16',
  };
  const getChangeTypeColor = computed(() => {
    return (changeType: ChangeType) => changeTypeColorMap[changeType].color;
  });
  const getChangeTypeValue = computed(() => {
    return (changeType: ChangeType) => changeTypeColorMap[changeType].value;
  });
  const getHttpMethodColor = computed(() => {
    return (method?: string) => {
      return method ? methodColorMap[method] : '';
    };
  });
  const getHttpStatusCodeColor = computed(() => {
    return (statusCode?: number) => {
      if (!statusCode) {
        return '';
      }
      if (statusCode >= 200 && statusCode < 300) {
        return '#87d068';
      }
      if (statusCode >= 300 && statusCode < 400) {
        return '#108ee9';
      }
      if (statusCode >= 400 && statusCode < 500) {
        return '#fa8c16';
      }
      if (statusCode >= 500) {
        return '#ff4d4f';
      }
      return '#13c2c2';
    };
  });

  return {
    getChangeTypeColor,
    getChangeTypeValue,
    getHttpMethodColor,
    getHttpStatusCodeColor,
  };
}
