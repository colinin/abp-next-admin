<template>
  <CollapseContainer :title="L('Notifies')" :canExpan="false">
    <List>
      <template v-for="item in list" :key="item.key">
        <ListItem>
          <ListItemMeta>
            <template #title>
              {{ item.title }}
              <Switch
                v-if="item.switch"
                class="extra"
                default-checked
                v-model:checked="item.switch.checked"
                @change="handleChange(item.key, $event)"
              />
            </template>
            <template #description>
              <div>{{ item.description }}</div>
            </template>
          </ListItemMeta>
        </ListItem>
      </template>
    </List>
  </CollapseContainer>
</template>
<script lang="ts">
  import { List, Switch } from 'ant-design-vue';
  import { defineComponent, ref, onMounted } from 'vue';
  import { CollapseContainer } from '/@/components/Container/index';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ListItem, useProfile } from './useProfile';
  import { subscribe, unSubscribe } from '/@/api/messages/subscribes';
  export default defineComponent({
    components: {
      CollapseContainer,
      List,
      ListItem: List.Item,
      ListItemMeta: List.Item.Meta,
      Switch,
    },
    setup() {
      const { L } = useLocalization('AbpAccount');
      const list = ref<ListItem[]>([]);
      const { getMsgNotifyList } = useProfile();

      function _fetchNotifies() {
        getMsgNotifyList().then((res) => {
          list.value = res;
        });
      }

      onMounted(_fetchNotifies);

      function handleChange(name: string, checked: boolean) {
        checked && subscribe(name);
        !checked && unSubscribe(name);
      }

      return {
        L,
        list,
        handleChange,
      };
    },
  });
</script>
<style lang="less" scoped>
  .extra {
    float: right;
    margin-top: 10px;
    margin-right: 30px;
  }
</style>
