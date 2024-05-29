<template>
  <BasicModal
    @register="registerModal"
    :title="L('TokenInfo')"
    :can-fullscreen="false"
    :show-ok-btn="false"
    :width="800"
    :height="500"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getModalFormSchemas } from '../datas/ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { OpenIddictTokenDto } from '/@/api/openiddict/open-iddict-token/model';
  import { get as getToken } from '/@/api/openiddict/open-iddict-token';
  import { get as getApplication } from '/@/api/openiddict/open-iddict-application';
  import { get as getAuthorization } from '/@/api/openiddict/open-iddict-authorization';

  const { L } = useLocalization(['AbpOpenIddict']);
  const [registerForm, { setFieldsValue, resetFields }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
    transformDateFunc: (date) => {
      return date ? formatToDateTime(date) : '';
    },
  });
  const [registerModal] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      fetchToken(data.id);
    });
  });

  function fetchToken(id: string) {
    getToken(id).then((dto) => {
      setFieldsValue(dto);
      fetchApplication(dto);
      fetchAuthorization(dto);
    });
  }

  function fetchApplication(token: OpenIddictTokenDto) {
    token.applicationId &&
      getApplication(token.applicationId).then((dto) => {
        setFieldsValue({
          applicationId: `${dto.clientId}(${token.applicationId})`,
        });
      });
  }

  function fetchAuthorization(token: OpenIddictTokenDto) {
    token.authorizationId &&
      getAuthorization(token.authorizationId).then((dto) => {
        setFieldsValue({
          authorizationId: `${dto.subject}(${token.authorizationId})`,
        });
      });
  }
</script>
