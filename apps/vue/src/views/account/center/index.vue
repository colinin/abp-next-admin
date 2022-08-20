<template>
  <div :class="prefixCls">
    <Row :class="`${prefixCls}-top`">
      <Col :span="9" :class="`${prefixCls}-col`">
        <Row>
          <Col :span="8">
            <div :class="`${prefixCls}-top__avatar`">
              <img width="70" :src="userInfo.avatar ?? headerImg" />
              <span>{{ userInfo.realName ?? userInfo.username }}</span>
              <div>{{ userInfo.description }}</div>
            </div>
          </Col>
        </Row>
      </Col>
    </Row>
    <div :class="`${prefixCls}-bottom`">
      <Tabs
        v-model:activeKey="activeTabKey"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
      >
        <template v-for="item in components" :key="item.key">
          <TabPane :tab="item.name">
            <component :is="componentsRef[item.component]" />
          </TabPane>
        </template>
      </Tabs>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { Tabs, Row, Col } from 'ant-design-vue';
  import { computed, ref, shallowRef } from 'vue';
  import headerImg from '/@/assets/images/header.jpg';
  import { useUserStore } from '/@/store/modules/user';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import Cloud from './Cloud.vue';
  import Setting from './Setting.vue';

  const TabPane = Tabs.TabPane;
  
  const componentsRef = shallowRef({
    'Cloud': Cloud,
    'Setting': Setting,
  });

  const prefixCls = 'account-center';
  const userStore = useUserStore();
  const activeTabKey = ref('Cloud');
  const { L } = useLocalization(['AbpOssManagement', 'AbpSettingManagement']);
  const components = [
    {
      key: 'Cloud',
      name: L('MyCloud'),
      component: 'Cloud',
    },
    {
      key: 'Setting',
      name: L('DisplayName:UserSetting'),
      component: 'Setting',
    },
  ];
  const tabsStyle = useTabsStyle(
    'top',
    {},
    {
      top: '80px',
    }
  );
  const userInfo = computed(() => userStore.getUserInfo);
</script>
<style lang="less" scoped>
  .account-center {
    &-col:not(:last-child) {
      padding: 0 10px;

      &:not(:last-child) {
        border-right: 1px dashed rgb(206 206 206 / 50%);
      }
    }

    &-top {
      padding: 10px;
      margin: 16px 16px 12px;
      background-color: @component-background;
      border-radius: 3px;

      &__avatar {
        text-align: center;

        img {
          margin: auto;
          border-radius: 50%;
        }

        span {
          display: block;
          font-size: 20px;
          font-weight: 500;
        }

        div {
          margin-top: 3px;
          font-size: 12px;
        }
      }

      &__detail {
        padding-left: 20px;
        margin-top: 15px;
      }

      &__team {
        &-item {
          display: inline-block;
          padding: 4px 24px;
        }

        span {
          margin-left: 3px;
        }
      }
    }

    &-bottom {
      padding: 10px;
      margin: 0 16px 16px;
      background-color: @component-background;
      border-radius: 3px;
    }
  }
</style>
