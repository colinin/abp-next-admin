<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { mgr } from '/@/utils/auth/oidc';
  import { del as delCookie } from '/@/utils/cookie';
  import { Persistent } from '/@/utils/cache/persistent';
  import { useUserStoreWithOut } from '/@/store/modules/user';

  const userStore = useUserStoreWithOut();

  onMounted(() => {
    mgr
      .signinRedirectCallback()
      .then((user) => {
        Persistent.setTenant({
          id: user.profile?.tenantid ?? '',
          name: '',
        });
        // TODO: 暂时先删除XSRF TOKEN?
        delCookie('XSRF-TOKEN');
        userStore.oidcLogin(user);
        // go(PageEnum.BASE_HOME);
      })
      .catch((error) => {
        if (error.error === 'access_denied') {
          userStore.logout(true);
        }
      });
  });
</script>
