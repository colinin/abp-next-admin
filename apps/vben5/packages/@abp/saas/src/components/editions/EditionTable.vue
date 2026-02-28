<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { EditionDto } from '../../types/editions';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { useFeatures } from '@abp/core';
import { FeatureModal } from '@abp/features';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useEditionsApi } from '../../api/useEditionsApi';
import { EditionsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'EditionTable',
});

const MenuItem = Menu.Item;
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');
const FeatureIcon = createIconifyIcon('pajamas:feature-flag');

const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { cancel, deleteApi, getPagedListApi } = useEditionsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
      },
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<EditionDto> = {
  columns: [
    {
      align: 'left',
      field: 'displayName',
      sortable: true,
      title: $t('AbpSaas.DisplayName:EditionName'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([
        EditionsPermissions.Update,
        EditionsPermissions.Delete,
      ]),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getPagedListApi({
          sorting,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<EditionDto> = {
  cellClick: () => {},
  sortChange: () => {
    gridApi.query();
  },
};
const [EditionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./EditionModal.vue')),
});
const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});
const [EditionChangeDrawer, entityChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
});
const [EditionFeatureModal, featureModalApi] = useVbenModal({
  connectedComponent: FeatureModal,
});

const onCreate = () => {
  modalApi.setData({});
  modalApi.open();
};

const onUpdate = (row: EditionDto) => {
  modalApi.setData(row);
  modalApi.open();
};

const onDelete = (row: EditionDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpSaas.EditionDeletionConfirmationMessage', [
      row.displayName,
    ]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onMenuClick = (row: EditionDto, info: MenuInfo) => {
  switch (info.key) {
    case 'entity-changes': {
      entityChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'LINGYUN.Abp.Saas.Edition',
        subject: row.displayName,
      });
      entityChangeDrawerApi.open();
      break;
    }
    case 'features': {
      featureModalApi.setData({
        displayName: row.displayName,
        providerKey: row.id,
        providerName: 'E',
      });
      featureModalApi.open();
    }
  }
};
</script>

<template>
  <Grid :table-title="$t('AbpSaas.Editions')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="[EditionsPermissions.Create]"
        @click="onCreate"
        :icon="h(PlusOutlined)"
      >
        {{ $t('AbpSaas.NewEdition') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[EditionsPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[EditionsPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="
                  isEnabled(['AbpAuditing.Logging.AuditLog']) &&
                  hasAccessByCodes([AuditLogPermissions.Default])
                "
                key="entity-changes"
                :icon="h(AuditLogIcon)"
              >
                {{ $t('AbpAuditLogging.EntitiesChanged') }}
              </MenuItem>
              <MenuItem
                v-if="hasAccessByCodes([EditionsPermissions.ManageFeatures])"
                key="features"
                :icon="h(FeatureIcon)"
              >
                {{ $t('AbpSaas.ManageFeatures') }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <EditionModal @change="() => gridApi.query()" />
  <EditionChangeDrawer />
  <EditionFeatureModal />
</template>

<style lang="scss" scoped></style>
