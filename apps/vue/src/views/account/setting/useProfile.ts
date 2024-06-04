import { computed } from 'vue';
import { FormSchema } from '/@/components/Form/index';
import { useSettings } from '/@/hooks/abp/useSettings';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { getTwoFactorEnabled } from '/@/api/account/profiles';
import { getAssignableNotifiers } from '/@/api/messages/notifications';
import { getAll as getMySubscribes } from '/@/api/messages/subscribes';
import { MyProfile } from '/@/api/account/profiles/model';
import { getUserInfo } from '/@/api/sys/user';

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
  enabled?: boolean;
  loading?: boolean;
}

interface UseProfile {
  profile: Nullable<MyProfile> | undefined,
}

export function useProfile({ profile }: UseProfile) {
  const { settingProvider } = useSettings();
  const { L } = useLocalization('AbpAccount');
  const { localizer: IdentityLocalizer } = useLocalization('AbpIdentity');
  const isExternalUser = computed(() => {
    if (!profile) {
      return false;
    }
    return profile.isExternal;
  });
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
    const currentUserInfo = await getUserInfo();
    const phoneNumber = currentUserInfo['phone_number'] ?? '';
    const phoneNumberConfirmed = currentUserInfo['phone_number_verified'] === 'True';
    const email = currentUserInfo['email'] ?? '';
    const emailVerified = currentUserInfo['email_verified'] === 'True';
    const twoFactorEnabled = await getTwoFactorEnabled();
    return [
      {
        key: 'password',
        title: L('DisplayName:Password'),
        description: L('ResetMyPassword'),
        extra: L('Edit'),
        enable: !isExternalUser.value,
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
        key: 'email',
        title: L('DisplayName:Email'),
        description: email,
        tag: {
          title: emailVerified ? L('Confirmed') : L('NotConfirmed'),
          color: emailVerified ? 'green' : 'warning',
        },
        extra: L('ClickToValidation'),
        enabled: !emailVerified,
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
    const notifies: {[key: string]: ListItem[] } = {};
    groupResult.items.forEach((group) => {
      notifies[group.displayName] = [];
      group.notifications.forEach((notifier) => {
        notifies[group.displayName].push({
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
