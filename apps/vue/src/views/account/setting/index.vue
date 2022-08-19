<template>
  <ScrollContainer>
    <div ref="wrapperRef" :class="prefixCls">
      <Tabs tab-position="left" :tabBarStyle="tabBarStyle">
        <template v-for="item in getSettingList()" :key="item.key">
          <TabPane :tab="item.name">
            <component :is="item.component" :profile="profileRef" @profile-change="initUserInfo" />
          </TabPane>
        </template>
      </Tabs>
    </div>
  </ScrollContainer>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { Tabs } from 'ant-design-vue';
  import { ScrollContainer } from '/@/components/Container/index';
  import { getSettingList } from './data';
  import { get as getProfile } from '/@/api/account/profiles';
  import { MyProfile } from '/@/api/account/model/profilesModel';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import BaseSetting from './BaseSetting.vue';
  import SecureSetting from './SecureSetting.vue';
  import AccountBind from './AccountBind.vue';
  import MsgNotify from './MsgNotify.vue';
  export default defineComponent({
    components: {
      ScrollContainer,
      Tabs,
      TabPane: Tabs.TabPane,
      BaseSetting,
      SecureSetting,
      AccountBind,
      MsgNotify,
    },
    setup() {
      const profileRef = ref<MyProfile>();
      onMounted(fetchProfile);

      async function fetchProfile() {
        const profile = await getProfile();
        profileRef.value = profile;
      }

      async function initUserInfo() {
        const abpStore = useAbpStoreWithOut();
        await abpStore.initlizeAbpApplication();
        await fetchProfile();
      }
      
      return {
        prefixCls: 'account-setting',
        profileRef,
        initUserInfo,
        getSettingList,
        tabBarStyle: {
          width: '220px',
        },
      };
    },
  });
</script>
<style lang="less">
  .account-setting {
    margin: 12px;
    background-color: @component-background;

    .base-title {
      padding-left: 0;
    }

    .ant-tabs-tab-active {
      background-color: @item-active-bg;
    }
  }
</style>
