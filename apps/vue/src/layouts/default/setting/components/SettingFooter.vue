<template>
  <div :class="prefixCls">
    <a-button type="primary" block @click="handleCopy" class="my-1">
      <CopyOutlined class="mr-2" />
      {{ t('layout.setting.copyBtn') }}
    </a-button>

    <a-button color="primary" block @click="handleSyncSetting" :loading="syncLoading"  class="my-1">
      <CloudSyncOutlined class="mr-2" />
      {{ t('layout.setting.syncBtn') }}
    </a-button>

    <a-button color="warning" block @click="handleResetSetting" class="my-1">
      <RedoOutlined class="mr-2" />
      {{ t('common.resetText') }}
    </a-button>

    <a-button color="error" block @click="handleClearAndRedo" class="my-1">
      <RedoOutlined class="mr-2" />
      {{ t('layout.setting.clearBtn') }}
    </a-button>
  </div>
</template>
<script lang="ts">
  import { defineComponent, ref, unref } from 'vue';

  import { CopyOutlined, RedoOutlined, CloudSyncOutlined } from '@ant-design/icons-vue';

  import { useAppStore } from '/@/store/modules/app';
  import { usePermissionStore } from '/@/store/modules/permission';
  import { useMultipleTabStore } from '/@/store/modules/multipleTab';
  import { useUserStore } from '/@/store/modules/user';

  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useCopyToClipboard } from '/@/hooks/web/useCopyToClipboard';

  import { updateColorWeak } from '/@/logics/theme/updateColorWeak';
  import { updateGrayMode } from '/@/logics/theme/updateGrayMode';
  import defaultSetting from '/@/settings/projectSetting';

  import { changeTheme } from '/@/api/sys/theme';
  import { ThemeSetting } from '/@/api/sys/model/themeModel';

  export default defineComponent({
    name: 'SettingFooter',
    components: { CopyOutlined, RedoOutlined, CloudSyncOutlined },
    setup() {
      const syncLoading = ref(false);
      const permissionStore = usePermissionStore();
      const { prefixCls } = useDesign('setting-footer');
      const { t } = useI18n();
      const { createSuccessModal, createMessage } = useMessage();
      const tabStore = useMultipleTabStore();
      const userStore = useUserStore();
      const appStore = useAppStore();

      function handleCopy() {
        const { isSuccessRef } = useCopyToClipboard(
          JSON.stringify(unref(appStore.getProjectConfig), null, 2),
        );
        unref(isSuccessRef) &&
          createSuccessModal({
            title: t('layout.setting.operatingTitle'),
            content: t('layout.setting.operatingContent'),
          });
      }
      function handleResetSetting() {
        try {
          appStore.setProjectConfig(defaultSetting);
          const { colorWeak, grayMode } = defaultSetting;
          // updateTheme(themeColor);
          updateColorWeak(colorWeak);
          updateGrayMode(grayMode);
          createMessage.success(t('layout.setting.resetSuccess'));
        } catch (error: any) {
          createMessage.error(error);
        }
      }

      function handleClearAndRedo() {
        localStorage.clear();
        appStore.resetAllState();
        permissionStore.resetState();
        tabStore.resetState();
        userStore.resetState();
        location.reload();
      }

      function handleSyncSetting() {
        const themeSetting: ThemeSetting = {
          darkMode: appStore.getDarkMode,
          projectConfig: appStore.getProjectConfig,
          beforeMiniInfo: appStore.getBeforeMiniInfo,
        };

        syncLoading.value = true;
        changeTheme(themeSetting).then(() => {
          createMessage.success(t('layout.setting.operatingTitle'));
        }).finally(() => {
          syncLoading.value = false;
        });
      }

      return {
        prefixCls,
        t,
        syncLoading,
        handleCopy,
        handleResetSetting,
        handleSyncSetting,
        handleClearAndRedo,
      };
    },
  });
</script>
<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-setting-footer';

  .@{prefix-cls} {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
</style>
