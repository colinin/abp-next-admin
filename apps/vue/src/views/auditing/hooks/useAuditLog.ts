import { computed } from 'vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { ChangeType } from '/@/api/auditing/model/auditLogModel';

export function useAuditLog() {
  const { L } = useLocalization('AbpAuditLogging');
  const changeTypeColorMap = {
    [ChangeType.Created]: { color: '#87d068', value: L('Created') },
    [ChangeType.Updated]: { color: '#108ee9', value: L('Updated') },
    [ChangeType.Deleted]: { color: 'red', value: L('Deleted') },
  };
  const methodColorMap: { [key: string]: string } = {
    ['GET']: 'blue',
    ['POST']: 'green',
    ['PUT']: 'orange',
    ['DELETE']: 'red',
    ['OPTIONS']: 'cyan',
    ['PATCH']: 'pink',
  };
  const entityChangeTypeColor = computed(() => {
    return (changeType?: ChangeType) => (changeType ? changeTypeColorMap[changeType].color : '');
  });
  const entityChangeType = computed(() => {
    return (changeType?: ChangeType) => (changeType ? changeTypeColorMap[changeType].value : '');
  });
  const httpMethodColor = computed(() => {
    return (method?: string) => {
      return method ? methodColorMap[method] : '';
    };
  });
  const httpStatusCodeColor = computed(() => {
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
    httpMethodColor,
    httpStatusCodeColor,
    entityChangeTypeColor,
    entityChangeType,
  };
}
