<template>
  <Collapse>
    <template v-for="group in Object.keys(notifyGroup)" :key="group">
      <CollapsePanel :header="group">
        <Card>
          <List>
            <template v-for="item in notifyGroup[group]" :key="item.key">
              <ListItem>
                <ListItemMeta>
                  <template #title>
                    {{ item.title }}
                    <Switch
                      v-if="item.switch"
                      class="extra"
                      default-checked
                      v-model:checked="item.switch.checked"
                      :loading="item.loading"
                      @change="(checked) => handleChange(item, checked)"
                    />
                  </template>
                  <template #description>
                    <div>{{ item.description }}</div>
                  </template>
                </ListItemMeta>
              </ListItem>
            </template>
          </List>
        </Card>
      </CollapsePanel>
    </template>
  </Collapse>
</template>
<script lang="ts">
  import { Card, List, Switch, Collapse } from 'ant-design-vue';
  import { defineComponent, ref, onMounted } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ListItem, useProfile } from './useProfile';
  import { subscribe, unSubscribe } from '/@/api/messages/subscribes';
  import { MyProfile } from '/@/api/account/model/profilesModel';
  export default defineComponent({
    components: {
      Card,
      Collapse,
      CollapsePanel: Collapse.Panel,
      List,
      ListItem: List.Item,
      ListItemMeta: List.Item.Meta,
      Switch,
    },
    props: {
      profile: {
        type: Object as PropType<MyProfile>,
      }
    },
    setup(props) {
      const { L } = useLocalization('AbpAccount');
      const notifyGroup = ref<{[key: string]: ListItem[]}>({});
      const { getMsgNotifyList } = useProfile({ profile: props.profile });

      function _fetchNotifies() {
        getMsgNotifyList().then((res) => {
          notifyGroup.value = res;
        });
      }

      onMounted(_fetchNotifies);

      function handleChange(item: ListItem, checked) {
        item.loading = true;
        const api = checked ? subscribe(item.key) : unSubscribe(item.key);
        api.finally(() => {
          item.loading = false;
        });
      }

      return {
        L,
        notifyGroup,
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
