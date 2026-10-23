import { $t } from '@vben/locales';

export function actionText(actionKey?: null | string): string {
  if (!actionKey) {
    return '-';
  }
  return $t(`ElsaNextApprovalFlow.ApprovalRecordAction:${actionKey}`);
}

export function formatDateTime(value?: null | string): string {
  return value ? new Date(value).toLocaleString() : '-';
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
      return '已通过';
    }
    case 'Cancelled': {
      return '已取消';
    }
    case 'Rejected': {
      return '已拒绝';
    }
    case 'Returned': {
      return '已退回';
    }
    case 'Running': {
      return '审批中';
    }
    case 'Withdrawn': {
      return '已撤回';
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
  const start = new Date(firstViewedAt).getTime();
  const end = new Date(completedAt).getTime();
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
    parts.push(`${days}天`);
  }
  if (hours > 0) {
    parts.push(`${hours}时`);
  }
  if (minutes > 0) {
    parts.push(`${minutes}分`);
  }
  if (seconds > 0 || parts.length === 0) {
    parts.push(`${seconds}秒`);
  }
  return parts.join('');
}
