<script setup lang="ts">
import { ref } from 'vue';

import { $t } from '@vben/locales';

import { Button, Card, Modal } from 'ant-design-vue';

import { useGdprRequestsApi } from '../api/useGdprRequestsApi';
import GdprTable from './GdprTable.vue';

const emits = defineEmits<{
  (event: 'accountDelete'): void;
}>();
const { cancel, deletePersonalAccountApi } = useGdprRequestsApi();

const submiting = ref(false);

const onDelete = async () => {
  Modal.confirm({
    centered: true,
    content: $t('AbpGdpr.DeletePersonalAccountWarning'),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      submiting.value = true;
      try {
        await deletePersonalAccountApi();
        Modal.success({
          centered: true,
          content: $t('AbpGdpr.PersonalAccountDeleteRequestReceived'),
          onOk: () => {
            emits('accountDelete');
          },
          title: $t('AbpGdpr.RequestedSuccessfully'),
        });
      } finally {
        submiting.value = false;
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Card
    :bordered="false"
    :title="$t('abp.account.settings.personalDataSettings')"
  >
    <template #extra>
      <Button block danger type="dashed" @click="onDelete">
        {{ $t('AbpGdpr.DeletePersonalAccount') }}
      </Button>
    </template>
    <GdprTable />
  </Card>
</template>

<style scoped></style>
