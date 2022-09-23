<template>
  <PageWrapper>
    <template #headerContent> <WorkbenchHeader /> </template>
    <div class="lg:flex">
      <div class="lg:w-7/10 w-full !mr-4 enter-y">
        <MenuCard class="enter-y" title="常用" :menus="usedMenus" />
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
  import { reactive, ref } from 'vue';
  import { Card } from 'ant-design-vue';
  import { PageWrapper } from '/@/components/Page';
  import WorkbenchHeader from './components/WorkbenchHeader.vue';
  import MenuCard from './components/MenuCard.vue';

  const usedMenus = reactive([
     {
      title: '首页',
      icon: 'ion:home-outline',
      color: '#1fdaca',
     },
     {
      title: '仪表盘',
      icon: 'ion:grid-outline',
      color: '#bf0c2c',
    },
    {
      title: '组件',
      icon: 'ion:layers-outline',
      color: '#e18525',
    },
    {
      title: '系统管理',
      icon: 'ion:settings-outline',
      color: '#3fb27f',
    },
    {
      title: '权限管理',
      icon: 'ion:key-outline',
      color: '#4daf1bc9',
    },
    {
      title: '图表',
      icon: 'ion:bar-chart-outline',
      color: '#00d8ff',
    },
  ]);

  const loading = ref(true);

  setTimeout(() => {
    loading.value = false;
  }, 1500);
</script>
