<template>
  <div>
    <CollapseContainer :title="L('SecuritySettings')" :canExpan="false">
      <List>
        <template v-for="item in secureSettingList" :key="item.key">
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
                  @change="handleChange(item, $event)"
                />
                <Button v-else-if="item.extra" class="extra" type="link" :loading="item.loading" :disbled="!item.enabled" @click="toggleCommand(item)">
                  {{ item.extra }}
                </Button>
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
    <SettingFormModal @register="registerModal" />
  </div>
</template>
<script lang="ts" setup>
  import type { ListItem as ProfileItem } from './useProfile';
  import { Button, List, Switch, Tag } from 'ant-design-vue';
  import { ref, onMounted } from 'vue';
  import { CollapseContainer } from '/@/components/Container';
  import { useModal } from '/@/components/Modal';
  import { useProfile } from './useProfile';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { changeTwoFactorEnabled, sendEmailConfirmLink } from '/@/api/account/profiles';
  import { MyProfile } from '/@/api/account/model/profilesModel';
  import SettingFormModal from './SettingFormModal.vue';

  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;

  const props = defineProps({
    profile: {
      type: Object as PropType<MyProfile>,
    }
  });

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpAccount');
  const { getSecureSettingList } = useProfile({ profile: props.profile });
  const secureSettingList = ref<ProfileItem[]>([]);
  const [registerModal, { openModal }] = useModal();

  onMounted(() => {
    getSecureSettingList().then((res) => {
      secureSettingList.value = res;
    });
  });

  function toggleCommand(item: ProfileItem) {
    item.loading = true;
    switch (item.key) {
      case 'password':
      case 'phoneNumber':
        openModal(true, item);
        item.loading = false;
        break;
      case 'email':
        sendEmailConfirmLink({
          email: item.description,
          appName: 'STS',
          returnUrl: window.location.href,
        }).finally(() => {
          item.loading = false;
        });
        break;
    }
  }

  function handleChange(item: ProfileItem, checked) {
    item.loading = true;
    switch (item.key) {
      case 'twofactor':
        changeTwoFactorEnabled(checked).then(() => {
          createMessage.success(L('Successful'));
        }).finally(() => {
          item.loading = false;
        });
        break;
    }
  }
</script>
<style lang="less" scoped>
  .extra {
    float: right;
    margin-top: 10px;
    margin-right: 30px;
    font-weight: normal;
    color: #1890ff;
    cursor: pointer;
  }
</style>
