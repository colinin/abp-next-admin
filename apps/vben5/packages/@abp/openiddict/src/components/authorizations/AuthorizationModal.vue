<script setup lang="ts">
import type { IdentityUserDto } from '@abp/identity';

import type { OpenIddictAuthorizationDto } from '../../types';

import { useAccess } from '@vben/access';
import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { UserLookupPermissions, useUserLookupApi } from '@abp/identity';
import { CodeEditor } from '@abp/ui';
import { Select } from 'ant-design-vue';

import { getApi as getApplication } from '../../api/applications';
import { getApi as getAuthorization } from '../../api/authorizations';

defineOptions({
  name: 'AuthorizationModal',
});

const Option = Select.Option;

const { hasAccessByCodes } = useAccess();
const userLookupApi = useUserLookupApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  schema: [
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'applicationId',
      label: $t('AbpOpenIddict.DisplayName:ApplicationId'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'subject',
      label: $t('AbpOpenIddict.DisplayName:Subject'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'type',
      label: $t('AbpOpenIddict.DisplayName:Type'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'creationDate',
      label: $t('AbpOpenIddict.DisplayName:CreationDate'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'status',
      label: $t('AbpOpenIddict.DisplayName:Status'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'scopes',
      label: $t('AbpOpenIddict.DisplayName:Scopes'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'properties',
      label: $t('AbpOpenIddict.DisplayName:Properties'),
    },
  ],

  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      try {
        modalApi.setState({ loading: true });
        const { id } = modalApi.getData<OpenIddictAuthorizationDto>();
        await onGet(id);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  showConfirmButton: false,
  title: $t('AbpOpenIddict.Authorizations'),
});

async function onGet(id: string) {
  const authorization = await getAuthorization(id);
  const application = await getApplication(authorization.applicationId!);
  let subjectInfo: IdentityUserDto | undefined;
  if (hasAccessByCodes([UserLookupPermissions.Default])) {
    subjectInfo = await userLookupApi.findByIdApi(authorization.subject!);
  }
  formApi.setValues({
    ...authorization,
    applicationId: `${application.clientId}(${authorization.applicationId})`,
    subject: subjectInfo?.userName
      ? `${subjectInfo.userName}(${authorization.subject})`
      : authorization.subject,
  });
}
</script>

<template>
  <Modal>
    <Form>
      <template #scopes="{ modelValue }">
        <Select :disabled="true" :value="modelValue" mode="tags">
          <Option
            v-for="scope in modelValue"
            :key="scope"
            :title="scope"
            :value="scope"
          />
        </Select>
      </template>
      <template #properties="{ modelValue }">
        <CodeEditor :value="modelValue" readonly />
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
