<script setup lang="ts">
import type { IdentityUserDto } from '@abp/identity';

import type { OpenIddictTokenDto } from '../../types/tokens';

import { useAccess } from '@vben/access';
import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { CodeEditor } from '@abp/components/codeeditor';
import { UserLookupPermissions, useUserLookupApi } from '@abp/identity';

import { useApplicationsApi } from '../../api/useApplicationsApi';
import { useTokensApi } from '../../api/useTokensApi';

defineOptions({
  name: 'TokenModal',
});

const { hasAccessByCodes } = useAccess();
const userLookupApi = useUserLookupApi();
const { getApi: getApplication } = useApplicationsApi();
const { cancel, getApi: getAuthorization } = useTokensApi();

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
      fieldName: 'authorizationId',
      label: $t('AbpOpenIddict.DisplayName:AuthorizationId'),
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
      fieldName: 'status',
      label: $t('AbpOpenIddict.DisplayName:Status'),
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
      fieldName: 'expirationDate',
      label: $t('AbpOpenIddict.DisplayName:ExpirationDate'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'redemptionDate',
      label: $t('AbpOpenIddict.DisplayName:RedemptionDate'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'referenceId',
      label: $t('AbpOpenIddict.DisplayName:ReferenceId'),
    },
    {
      component: 'Input',
      componentProps: {
        readonly: true,
      },
      fieldName: 'payload',
      label: $t('AbpOpenIddict.DisplayName:Payload'),
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
  onClosed() {
    cancel('TokenModal has closed!');
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      try {
        modalApi.setState({ loading: true });
        const { id } = modalApi.getData<OpenIddictTokenDto>();
        await onGet(id);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  showConfirmButton: false,
  title: $t('AbpOpenIddict.Tokens'),
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
      <template #payload="{ modelValue }">
        <CodeEditor :value="modelValue" readonly />
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
