<template>
  <PageWrapper>
    <template #headerContent> <WorkbenchHeader /> </template>
    <div class="lg:flex">
      <div class="lg:w-7/10 w-full !mr-4 enter-y">
        <MenuCard
          class="enter-y"
          :title="t('routes.dashboard.workbench.menus.favoriteMenu')"
          :menus="myFavoriteMenus"
          @change="handleAddMyFavoriteMenu"
          @delete="handleDeleteMyFavoriteMenu"
        />
      </div>
      <div class="lg:w-3/10 w-full enter-y">
        <!-- <QuickNav :loading="loading" class="enter-y" /> -->

        <Card class="!my-4 enter-y" :loading="loading">
          <img class="xl:h-50 h-30 mx-auto" src="../../../assets/svg/illustration.svg" />
        </Card>

        <!-- <SaleRadar :loading="loading" class="enter-y" /> -->
      </div>
    </div>
  </PageWrapper>
</template>
<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { Card } from 'ant-design-vue';
  import { PageWrapper } from '/@/components/Page';
  import { Menu, useDefaultMenus } from './components/menuProps';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import {
    createMyFavoriteMenu,
    delMyFavoriteMenu,
    getMyFavoriteMenuList,
  } from '/@/api/platform/user-favorites-menu';
  import WorkbenchHeader from './components/WorkbenchHeader.vue';
  import MenuCard from './components/MenuCard.vue';

  const myFavoriteMenus = ref<Menu[]>(useDefaultMenus());

  const loading = ref(true);
  const { t } = useI18n();
  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpUi']);

  onMounted(fetchMyFavoriteMenus);

  function fetchMyFavoriteMenus() {
    loading.value = true;
    getMyFavoriteMenuList()
      .then((res) => {
        const defaultFavmenus = useDefaultMenus();
        const defineFavmenus = res.items.map((menu) => {
          return {
            id: menu.menuId,
            title: menu.aliasName ?? menu.displayName,
            icon: menu.icon,
            color: menu.color,
            path: menu.path,
            hasDefault: false,
          };
        });
        myFavoriteMenus.value = defaultFavmenus.concat(defineFavmenus);
      })
      .finally(() => {
        loading.value = false;
      });
  }

  function handleAddMyFavoriteMenu(menu) {
    createMyFavoriteMenu({
      framework: '',
      menuId: menu.menuId,
      icon: menu.icon,
      color: menu.color,
      aliasName: menu.aliasName,
    }).then(() => {
      createMessage.success(L('Successful'));
      fetchMyFavoriteMenus();
    });
  }

  function handleDeleteMyFavoriteMenu(menu: Menu) {
    delMyFavoriteMenu(menu.id).then(() => {
      createMessage.success(L('SuccessfullyDeleted'));
      fetchMyFavoriteMenus();
    });
  }
</script>
