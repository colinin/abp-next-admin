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
<script lang="ts" setup>
  import { Card, List, Switch, Collapse } from 'ant-design-vue';
  import { ref, onMounted } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ListItem as ProfileItem, useProfile } from './useProfile';
  import { subscribe, unSubscribe } from '/@/api/messages/subscribes';
  import { MyProfile } from '/@/api/account/model/profilesModel';

  const CollapsePanel = Collapse.Panel;
  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;

  const props = defineProps({
    profile: {
      type: Object as PropType<MyProfile>,
    }
  });

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpAccount');
  const notifyGroup = ref<{[key: string]: ProfileItem[]}>({});
  const { getMsgNotifyList } = useProfile({ profile: props.profile });

  function _fetchNotifies() {
    getMsgNotifyList().then((res) => {
      notifyGroup.value = res;
    });
  }

  onMounted(_fetchNotifies);

  function handleChange(item: ProfileItem, checked) {
    item.loading = true;
    const api = checked ? subscribe(item.key) : unSubscribe(item.key);
    api.then(() => {
      createMessage.success(L('Successful'));
    }).finally(() => {
      item.loading = false;
    });
  }
</script>
<style lang="less" scoped>
  .extra {
    float: right;
    margin-top: 10px;
    margin-right: 30px;
  }
</style>
