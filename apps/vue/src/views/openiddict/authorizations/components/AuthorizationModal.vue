<template>
  <BasicModal
    @register="registerModal"
    :title="L('TokenInfo')"
    :can-fullscreen="false"
    :show-ok-btn="false"
    :width="800"
    :height="500"
  >
    <BasicForm @register="registerForm">
      <template #scopes="{ model, field }">
        <Select mode="tags" :value="model[field]" :disabled="true">
          <Option v-for="scope in model[field]" :key="scope" :title="scope" :value="scope" />
        </Select>
      </template>
    </BasicForm>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { Select } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getModalFormSchemas } from '../datas/ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { OpenIddictAuthorizationDto } from '/@/api/openiddict/open-iddict-authorization/model';
  import { get as getAuthorization } from '/@/api/openiddict/open-iddict-authorization';
  import { get as getApplication } from '/@/api/openiddict/open-iddict-application';

  const Option = Select.Option;

  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
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
      fetchAuth(data.id);
    });
  });

  function fetchAuth(id: string) {
    getAuthorization(id).then((dto) => {
      setFieldsValue(dto);
      fetchApplication(dto);
    });
  }

  function fetchApplication(auth: OpenIddictAuthorizationDto) {
    auth.applicationId &&
      getApplication(auth.applicationId).then((dto) => {
        setFieldsValue({
          applicationId: `${dto.clientId}(${auth.applicationId})`,
        });
      });
  }
</script>
