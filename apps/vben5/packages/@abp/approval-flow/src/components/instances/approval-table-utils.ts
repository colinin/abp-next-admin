import { $t } from '@vben/locales';

import { formatToDateTime, toDate } from '@abp/core';

export function actionText(actionKey?: null | string): string {
  if (!actionKey) {
    return '-';
  }
  return $t(`ElsaNextApprovalFlow.ApprovalRecordAction:${actionKey}`);
}

export function formatDateTime(value?: null | string): string {
  return value ? formatToDateTime(value) : '-';
}

export function statusColor(status: string): string {
  switch (status) {
    case 'Approved': {
      return 'green';
    }
    case 'Rejected': {
      return 'red';
    }
    case 'Running': {
      return 'blue';
    }
    default: {
      return 'default';
    }
  }
}

export function statusText(status: string): string {
  switch (status) {
    case 'Approved': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Approved');
    }
    case 'Cancelled': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Cancelled');
    }
    case 'Rejected': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Rejected');
    }
    case 'Returned': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Returned');
    }
    case 'Running': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Running');
    }
    case 'Withdrawn': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Withdrawn');
    }
    default: {
      return status;
    }
  }
}

export function formatDuration(
  firstViewedAt?: null | string,
  completedAt?: null | string,
): string {
  if (!firstViewedAt || !completedAt) {
    return '-';
  }
  const start = toDate(firstViewedAt).valueOf();
  const end = toDate(completedAt).valueOf();
  if (Number.isNaN(start) || Number.isNaN(end) || end < start) {
    return '-';
  }

  let seconds = Math.floor((end - start) / 1000);
  const days = Math.floor(seconds / 86_400);
  seconds %= 86_400;
  const hours = Math.floor(seconds / 3600);
  seconds %= 3600;
  const minutes = Math.floor(seconds / 60);
  seconds %= 60;

  const parts: string[] = [];
  if (days > 0) {
    parts.push($t('antflow.duration.days', [String(days)]));
  }
  if (hours > 0) {
    parts.push($t('antflow.duration.hours', [String(hours)]));
  }
  if (minutes > 0) {
    parts.push($t('antflow.duration.minutes', [String(minutes)]));
  }
  if (seconds > 0 || parts.length === 0) {
    parts.push($t('antflow.duration.seconds', [String(seconds)]));
  }
  return parts.join('');
}
