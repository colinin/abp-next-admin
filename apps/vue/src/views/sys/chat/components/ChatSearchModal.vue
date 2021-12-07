<template>
  <BasicModal v-bind="$attrs" title="搜索" :width="660" :min-height="400">
    <Tabs>
      <TabPane key="user" tab="找好友">
        <List
          :grid="{ gutter: 10, column: 3 }"
          size="large"
          item-layout="horizontal"
          :data-source="users"
        >
          <template #header>
            <InputSearch @search="handleSearchFriend" />
          </template>
          <template #renderItem="{ item }">
            <ListItem>
              <ListItemMeta>
                <template #title>
                  <span class="title">{{ item.userName }}</span>
                </template>
                <template #avatar>
                  <Avatar v-if="item.avatarUrl" size="large" :src="item.avatarUrl" />
                  <Avatar v-else size="large" :src="userAvatar" />
                </template>
                <template #description>
                  <Button size="small" type="primary" @click="handleAddNewFriend(item)"
                    >加好友</Button
                  >
                </template>
              </ListItemMeta>
            </ListItem>
          </template>
        </List>
      </TabPane>
      <TabPane key="group" tab="找群">
        <List
          :grid="{ gutter: 10, column: 3 }"
          size="large"
          item-layout="horizontal"
          :data-source="groups"
        >
          <template #header>
            <InputSearch @search="handleSearchGroup" />
          </template>
          <template #renderItem="{ item }">
            <ListItem>
              <ListItemMeta>
                <template #title>
                  <span class="title">{{ item.name }}</span>
                </template>
                <template #avatar>
                  <Avatar v-if="item.avatarUrl" size="large" :src="item.avatarUrl" />
                  <Avatar v-else size="large" :src="userAvatar" />
                </template>
                <template #description>
                  <Button size="small" type="primary" @click="handleJoinGroup(item)">加入群</Button>
                </template>
              </ListItemMeta>
            </ListItem>
          </template>
        </List>
      </TabPane>
    </Tabs>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { Avatar, Button, Input, List, Tabs, message } from 'ant-design-vue';
  import { TeamOutlined } from '@ant-design/icons-vue';
  import { BasicModal } from '/@/components/Modal';
  import { search as searchGroup } from '/@/api/messages/groups';
  import { Group } from '/@/api/messages/model/groupModel';
  import { addFriend } from '/@/api/messages/friends';
  import { search as searchUser } from '/@/api/identity/userLookup';
  import { IUserData } from '/@/api/identity/model/userLookupModel';
  import userAvatar from '/@/assets/icons/64x64/color-user.png';

  export default defineComponent({
    name: 'ChatSearchModal',
    components: {
      Avatar,
      BasicModal,
      Button,
      InputSearch: Input.Search,
      List,
      ListItem: List.Item,
      ListItemMeta: List.Item.Meta,
      Tabs,
      TabPane: Tabs.TabPane,
      TeamOutlined,
    },
    setup() {
      const { t } = useI18n();
      const users = ref<IUserData[]>([]);
      const groups = ref<Group[]>([]);

      function handleSearchFriend(value: string) {
        searchUser({
          filter: value,
          sorting: '',
          skipCount: 0,
          maxResultCount: 25,
        }).then((res) => {
          users.value = res.items;
        });
      }

      function handleAddNewFriend(user: IUserData) {
        addFriend({
          remarkName: user.userName,
          friendId: user.id,
        }).then(() => {
          message.success(t('AbpUi.Successful'));
        });
      }

      function handleSearchGroup(value: string) {
        searchGroup({
          filter: value,
          sorting: '',
          skipCount: 0,
          maxResultCount: 25,
        }).then((res) => {
          groups.value = res.items;
        });
      }

      function handleJoinGroup(group: Group) {
        console.log(group);
      }

      return {
        users,
        groups,
        userAvatar,
        handleSearchFriend,
        handleSearchGroup,
        handleAddNewFriend,
        handleJoinGroup,
      };
    },
  });
</script>

<style lang="less" scoped>
  .title {
    width: 60px;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    display: inline-block;
  }
</style>
