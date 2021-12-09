import { FormSchema } from '/@/components/Form/index';
import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { useSettings } from '/@/hooks/abp/useSettings';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { getTwoFactorEnabled } from '/@/api/account/profiles';
import { getAssignableNotifiers } from '/@/api/messages/notifications';
import { getAll as getMySubscribes } from '/@/api/messages/subscribes';

export interface ListItem {
  key: string;
  title: string;
  description: string;
  extra?: string;
  tag?: {
    title: string;
    color: string;
  };
  switch?: {
    checked: boolean;
  };
  avatar?: string;
  color?: string;
}

export function useProfile() {
  const { settingProvider } = useSettings();
  const { L } = useLocalization('AbpAccount');
  const { localizer: IdentityLocalizer } = useLocalization('AbpIdentity');
  // tab的list
  function getSettingList() {
    return [
      {
        key: '1',
        name: L('BasicSettings'),
        component: 'BaseSetting',
      },
      {
        key: '2',
        name: L('SecuritySettings'),
        component: 'SecureSetting',
      },
      {
        key: '3',
        name: L('Binding'),
        component: 'AccountBind',
      },
      {
        key: '4',
        name: L('Notifies'),
        component: 'MsgNotify',
      },
    ];
  }

  // 基础设置 form
  function getBaseSetschemas(): FormSchema[] {
    return [
      {
        field: 'userName',
        component: 'Input',
        label: L('DisplayName:UserName'),
        dynamicDisabled: () => {
          return !settingProvider.isTrue('Abp.Identity.User.IsUserNameUpdateEnabled');
        },
        colProps: { span: 18 },
        required: true,
      },
      {
        field: 'email',
        component: 'Input',
        label: L('DisplayName:Email'),
        dynamicDisabled: () => {
          return !settingProvider.isTrue('Abp.Identity.User.IsEmailUpdateEnabled');
        },
        colProps: { span: 18 },
        required: true,
      },
      {
        field: 'surname',
        component: 'Input',
        label: L('DisplayName:Surname'),
        colProps: { span: 18 },
      },
      {
        field: 'name',
        component: 'Input',
        label: L('DisplayName:Name'),
        colProps: { span: 18 },
      },
      {
        field: 'phoneNumber',
        component: 'Input',
        label: L('DisplayName:PhoneNumber'),
        colProps: { span: 18 },
        dynamicDisabled: true,
      },
      {
        field: 'extraProperties.AvatarUrl',
        component: 'Input',
        label: 'AvatarUrl',
        ifShow: false,
      },
      {
        field: 'concurrencyStamp',
        component: 'Input',
        label: 'ConcurrencyStamp',
        ifShow: false,
      },
    ];
  }

  // 安全设置 list
  async function getSecureSettingList() {
    const abpStore = useAbpStoreWithOut();
    const phoneNumber = abpStore.getApplication.currentUser.phoneNumber ?? '';
    const phoneNumberConfirmed = abpStore.getApplication.currentUser.phoneNumberVerified;
    const twoFactorEnabled = await getTwoFactorEnabled();
    return [
      {
        key: 'password',
        title: L('DisplayName:Password'),
        description: L('ResetMyPassword'),
        extra: L('Edit'),
      },
      {
        key: 'phoneNumber',
        title: L('DisplayName:PhoneNumber'),
        description: phoneNumber,
        tag: {
          title: phoneNumberConfirmed ? L('Confirmed') : L('NotConfirmed'),
          color: phoneNumberConfirmed ? 'green' : 'warning',
        },
        extra: L('Edit'),
      },
      {
        key: 'twofactor',
        title: IdentityLocalizer.L('DisplayName:TwoFactorEnabled'),
        description: IdentityLocalizer.L('DisplayName:TwoFactorEnabled'),
        switch: {
          checked: twoFactorEnabled.enabled,
        },
      },
    ];
  }

  // 账号绑定 list
  function getAccountBindList(): ListItem[] {
    return [];
  }

  // 订阅消息通知 list
  async function getMsgNotifyList() {
    const groupResult = await getAssignableNotifiers();
    const mySubResult = await getMySubscribes();
    const mySubNotifies = mySubResult.items.map((x) => x.name);
    const notifies: ListItem[] = [];
    groupResult.items.forEach((group) => {
      group.notifications.forEach((notifier) => {
        notifies.push({
          key: notifier.name,
          title: notifier.displayName,
          description: notifier.description,
          switch: {
            checked: mySubNotifies.includes(notifier.name),
          },
        });
      });
    });
    return notifies;
  }

  return {
    getBaseSetschemas,
    getSecureSettingList,
    getSettingList,
    getAccountBindList,
    getMsgNotifyList,
  };
}
