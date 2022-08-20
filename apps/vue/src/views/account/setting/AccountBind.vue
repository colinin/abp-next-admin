<template>
  <CollapseContainer :title="L('Binding')" :canExpan="false">
    <List>
      <template v-for="item in getAccountBindList()" :key="item.key">
        <ListItem>
          <ListItemMeta>
            <template #avatar>
              <Icon v-if="item.avatar" class="avatar" :icon="item.avatar" :color="item.color" />
            </template>
            <template #title>
              {{ item.title }}
              <a-button type="link" size="small" v-if="item.extra" class="extra">
                {{ item.extra }}
              </a-button>
            </template>
            <template #description>
              <div>
                <span>{{ item.description }}</span>
                <Tag v-if="item.tag" :color="item.tag.color">{{ item.tag.title }}</Tag>
              </div>
            </template>
          </ListItemMeta>
        </ListItem>
      </template>
    </List>
  </CollapseContainer>
</template>
<script lang="ts" setup>
  import { List, Tag } from 'ant-design-vue';
  import { CollapseContainer } from '/@/components/Container/index';
  import { useProfile } from './useProfile';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { MyProfile } from '/@/api/account/model/profilesModel';
  import Icon from '/@/components/Icon/index';

  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;

  const props = defineProps({
    profile: {
      type: Object as PropType<MyProfile>,
    }
  });

  const { L } = useLocalization('AbpAccount');
  const { getAccountBindList } = useProfile({ profile: props.profile });
</script>
<style lang="less" scoped>
  .avatar {
    font-size: 40px !important;
  }

  .extra {
    float: right;
    margin-top: 10px;
    margin-right: 30px;
    cursor: pointer;
  }
</style>
