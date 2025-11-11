<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { EntityTypeInfoDto } from '../../types/entityTypeInfos';

import { h } from 'vue';

import { $t } from '@vben/locales';

import { useLocalization, useLocalizationSerializer } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { EditOutlined } from '@ant-design/icons-vue';
import { Button } from 'ant-design-vue';

import { useEntityTypeInfosApi } from '../../api/useEntityTypeInfosApi';

defineOptions({
  name: 'GdprTable',
});

const { getPagedListApi } = useEntityTypeInfosApi();

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();

const gridOptions: VxeGridProps<EntityTypeInfoDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      sortable: true,
      title: $t('DataProtection.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('DataProtection.DisplayName:DisplayName'),
    },
    {
      align: 'left',
      field: 'typeFullName',
      minWidth: 200,
      sortable: true,
      title: $t('DataProtection.DisplayName:TypeFullName'),
    },
    {
      align: 'left',
      field: 'isAuditEnabled',
      minWidth: 150,
      sortable: true,
      title: $t('DataProtection.DisplayName:IsAuditEnabled'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 180,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        const { totalCount, items } = await getPagedListApi({
          sorting,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
        return {
          totalCount,
          items: items.map((item) => {
            const displayName = deserialize(item.displayName);
            return {
              ...item,
              displayName: Lr(displayName.resourceName, displayName.name),
            };
          }),
        };
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    refresh: {
      code: 'query',
    },
  },
};

const gridEvents: VxeGridListeners<EntityTypeInfoDto> = {
  cellClick: () => {},
  sortChange: () => {
    gridApi.query();
  },
};
const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});
function onEdit(_record: EntityTypeInfoDto) {}
</script>

<template>
  <Grid :table-title="$t('DataProtection.EntityTypeInfos')">
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button :icon="h(EditOutlined)" block type="link" @click="onEdit(row)">
          {{ $t('AbpUi.Edit') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped></style>
