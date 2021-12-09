import type { Router } from 'vue-router';
import { useAbpStore } from '/@/store/modules/abp';
import { useAppStore } from '/@/store/modules/app';
import { useMultipleTabStore } from '/@/store/modules/multipleTab';
import { useUserStore } from '/@/store/modules/user';
import { usePermissionStore } from '/@/store/modules/permission';
import { PageEnum } from '/@/enums/pageEnum';
import { removeTabChangeListener } from '/@/logics/mitt/routeChange';

export function createStateGuard(router: Router) {
  router.afterEach((to) => {
    // Just enter the login page and clear the authentication information
    if (to.path === PageEnum.BASE_LOGIN) {
      const tabStore = useMultipleTabStore();
      const userStore = useUserStore();
      const abpStore = useAbpStore();
      const appStore = useAppStore();
      const permissionStore = usePermissionStore();
      abpStore.resetSession();
      appStore.resetAllState();
      permissionStore.resetState();
      tabStore.resetState();
      userStore.resetState();
      removeTabChangeListener();
    }
  });
}
