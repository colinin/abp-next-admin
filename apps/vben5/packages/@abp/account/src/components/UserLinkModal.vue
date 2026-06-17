<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { LinkUserDto } from '../types';

import { h, ref } from 'vue';

import { confirm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { sortby } from '@abp/core';
import { useMessage, useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  LoginOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button } from 'ant-design-vue';

import { useLinkUsersApi } from '../api/useLinkUsersApi';

const emit = defineEmits<{
  (event: 'link', token: string): void;
  (event: 'login', userId: string, tenantId?: string): void;
}>();

const message = useMessage();
const { getListApi, generateLinkTokenApi, unLinkApi } = useLinkUsersApi();

const linkUsers = ref<LinkUserDto[]>([]);

const [Modal, modalApi] = useVbenModal({
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onInit();
    }
  },
  title: $t('AbpAccount.LinkUser'),
});
const gridOptions: VxeGridProps<LinkUserDto> = {
  columns: [
    {
      align: 'left',
      field: 'linkUserName',
      sortable: true,
      title: $t('AbpAccount.DisplayName:LinkUserName'),
    },
    {
      align: 'left',
      field: 'directlyLinked',
      slots: { default: 'directlyLinked' },
      sortable: true,
      title: $t('AbpAccount.DisplayName:DirectlyLinked'),
      width: 150,
    },
    {
      field: 'actions',
      fixed: 'right',
      slots: { default: 'actions' },
      title: $t('AbpUi.Actions'),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(linkUsers.value, sort.field);
        if (sort.order === 'desc') {
          items = items.toReversed();
        }
        const result = {
          totalCount: linkUsers.value.length,
          items: items.slice(
            (page.currentPage - 1) * page.pageSize,
            page.currentPage * page.pageSize,
          ),
        };
        return new Promise((resolve) => {
          resolve(result);
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  sortConfig: {
    remote: true,
    allowBtn: true,
  },
  toolbarConfig: {
    custom: true,
    export: true,
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridOptions,
});

async function onInit() {
  const { items } = await getListApi();
  linkUsers.value = items;
  gridApi.query();
}

async function onCreatLink() {
  confirm({
    async beforeClose(scope) {
      if (scope.isConfirm) {
        const token = await generateLinkTokenApi();
        emit('link', token);
      }
    },
    centered: true,
    content: $t('AbpAccount.LinkAccountWillBeCreateWarning'),
    title: $t('AbpUi.AreYouSure'),
  });
}

function onLoginByLink(record: LinkUserDto) {
  emit('login', record.linkUserId, record.linkTenantId);
  modalApi.close();
}

async function onDeleteLink(record: LinkUserDto) {
  confirm({
    async beforeClose(scope) {
      if (scope.isConfirm) {
        await unLinkApi({
          userId: record.linkUserId,
          tenantId: record.linkTenantId,
        });
        message.success($t('AbpUi.DeletedSuccessfully'));
        await onInit();
      }
    },
    centered: true,
    content: $t('AbpAccount.LinkAccountWillBeDeleteWarning', [
      record.linkUserName,
    ]),
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Modal>
    <Grid>
      <template #toolbar-tools>
        <Button :icon="h(PlusOutlined)" type="primary" @click="onCreatLink">
          {{ $t('AbpAccount.LinkUser:New') }}
        </Button>
      </template>
      <template #directlyLinked="{ row }">
        <div class="flex flex-row justify-center">
          <CheckOutlined v-if="row.directlyLinked" class="text-green-500" />
          <CloseOutlined v-else class="text-red-500" />
        </div>
      </template>
      <template #actions="{ row }">
        <div class="flex flex-row">
          <Button
            :icon="h(LoginOutlined)"
            block
            type="link"
            @click="onLoginByLink(row)"
          >
            {{ $t('AbpAccount.LinkUser:Login') }}
          </Button>
          <Button
            :icon="h(DeleteOutlined)"
            block
            danger
            type="link"
            @click="onDeleteLink(row)"
          >
            {{ $t('AbpUi.Delete') }}
          </Button>
        </div>
      </template>
    </Grid>
  </Modal>
</template>

<style scoped></style>
