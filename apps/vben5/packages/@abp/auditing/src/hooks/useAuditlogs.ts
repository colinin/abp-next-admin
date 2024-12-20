import { computed } from 'vue';

import { useLocalization } from '@abp/core';

import { ChangeType } from '../types/entity-changes';

export function useAuditlogs() {
  const { L } = useLocalization(['AbpAuditLogging', 'AbpUi']);

  const changeTypeColorMap = {
    [ChangeType.Created]: { color: '#87d068', value: L('Created') },
    [ChangeType.Deleted]: { color: 'red', value: L('Deleted') },
    [ChangeType.Updated]: { color: '#108ee9', value: L('Updated') },
  };

  const methodColorMap: { [key: string]: string } = {
    DELETE: 'red',
    GET: 'blue',
    OPTIONS: 'cyan',
    PATCH: 'pink',
    POST: 'green',
    PUT: 'orange',
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
        return 'orange';
      }
      if (statusCode >= 500) {
        return 'red';
      }
      return 'cyan';
    };
  });

  return {
    getChangeTypeColor,
    getChangeTypeValue,
    getHttpMethodColor,
    getHttpStatusCodeColor,
  };
}
