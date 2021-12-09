<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { useUserStoreWithOut } from '../../../store/modules/user';

  const userStore = useUserStoreWithOut();

  onMounted(() => {
    userStore.logout(true);
  });
</script>
