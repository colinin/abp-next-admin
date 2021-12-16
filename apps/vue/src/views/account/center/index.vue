<template>
  <div :class="prefixCls">
    <a-row :class="`${prefixCls}-top`">
      <a-col :span="9" :class="`${prefixCls}-col`">
        <a-row>
          <a-col :span="8">
            <div :class="`${prefixCls}-top__avatar`">
              <img width="70" :src="userInfo.avatar ?? headerImg" />
              <span>{{ userInfo.realName ?? userInfo.username }}</span>
              <div>{{ userInfo.description }}</div>
            </div>
          </a-col>
        </a-row>
      </a-col>
    </a-row>
    <div :class="`${prefixCls}-bottom`">
      <Tabs tab-position="top" v-model:activeKey="activeTabKey">
        <template v-for="item in components" :key="item.key">
          <TabPane :tab="item.name">
            <component :is="item.component" />
          </TabPane>
        </template>
      </Tabs>
    </div>
  </div>
</template>

<script lang="ts">
  import { Tag, Tabs, Row, Col } from 'ant-design-vue';
  import { defineComponent, computed, ref } from 'vue';
  import { CollapseContainer } from '/@/components/Container/index';
  import Icon from '/@/components/Icon/index';
  import headerImg from '/@/assets/images/header.jpg';
  import { useUserStore } from '/@/store/modules/user';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import Cloud from './Cloud.vue';
  import Setting from './Setting.vue';
  export default defineComponent({
    components: {
      Cloud,
      CollapseContainer,
      Icon,
      Tag,
      Tabs,
      TabPane: Tabs.TabPane,
      Setting,
      [Row.name]: Row,
      [Col.name]: Col,
    },
    setup() {
      const userStore = useUserStore();
      const activeTabKey = ref('cloud');
      const { L } = useLocalization('AbpOssManagement', 'AbpSettingManagement');
      const components = [
        {
          key: 'cloud',
          name: L('MyCloud'),
          component: 'cloud',
        },
        {
          key: 'setting',
          name: L('DisplayName:UserSetting'),
          component: 'setting',
        },
      ];
      const userInfo = computed(() => userStore.getUserInfo);
      return {
        activeTabKey,
        prefixCls: 'account-center',
        userInfo,
        headerImg,
        components,
      };
    },
  });
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
