<script lang="ts" setup>
  import { onMounted, inject } from 'vue';
  import { mgr } from '/@/utils/auth/oidc';
  import { useUserStoreWithOut } from '/@/store/modules/user';

  const cookies = inject<any>('$cookies');

  const userStore = useUserStoreWithOut();

  onMounted(() => {
    mgr
      .signinRedirectCallback()
      .then((user) => {
        // TODO: 暂时先删除XSRF TOKEN?
        cookies?.remove('XSRF-TOKEN');
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
