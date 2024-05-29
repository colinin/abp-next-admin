<template>
  <CollapseContainer :title="L('Authenticator')" :canExpan="false">
    <Card v-if="authenticator?.isAuthenticated === false" :title="L('Authenticator')">
      <Steps :current="step">
        <Step>
          <template #title>{{ L('Authenticator') }}</template>
        </Step>
        <Step>
          <template #title>{{ L('ValidAuthenticator') }}</template>
        </Step>
        <Step>
          <template #title>{{ L('RecoveryCode') }}</template>
        </Step>
      </Steps>
      <div v-if="step === 0" class="steps">
        <Card :title="L('AuthenticatorDesc')">
          <Row justify="center">
            <Col :span="12">
              <Card class="card" :title="L('Authenticator:UseQrCode')">
                <div class="content">
                  <QrCode :value="authenticator?.authenticatorUri" />
                </div>
              </Card>
            </Col>
            <Col :span="12">
              <Card class="card" :title="L('Authenticator:InputCode')">
                <template #extra>
                  <Button type="primary" @click="handleCopy(authenticator?.sharedKey)">{{
                    L('Authenticator:CopyToClipboard')
                  }}</Button>
                </template>
                <div class="content">
                  <div class="key-box">
                    <span class="key">{{ authenticator?.sharedKey }}</span>
                  </div>
                </div>
              </Card>
            </Col>
          </Row>
        </Card>
      </div>
      <div v-if="step === 1" class="steps">
        <Card>
          <template #title>
            <Row>
              <div class="title">
                <span>{{ L('ValidAuthenticator') }}</span>
              </div>
            </Row>
            <Row>
              <div class="desc">
                <span>{{ L('ValidAuthenticatorDesc') }}</span>
              </div>
            </Row>
          </template>
          <Form ref="form" :model="formModel">
            <FormItem :label="L('DisplayName:AuthenticatorCode')" name="code" required>
              <Row :gutter="16" align="middle">
                <Col :span="9">
                  <Input v-model:value="formModel.code" />
                </Col>
                <Col :span="4">
                  <Button style="width: 100px" type="primary" @click="handleValidateCode">{{
                    L('ValidAuthenticator:Valid')
                  }}</Button>
                </Col>
              </Row>
            </FormItem>
          </Form>
        </Card>
      </div>
      <div v-if="step === 2" class="steps">
        <Card class="card">
          <template #extra>
            <Button type="primary" @click="handleCopy(recoveryCodes.join('\r'))">{{
              L('Authenticator:CopyToClipboard')
            }}</Button>
          </template>
          <template #title>
            <Row>
              <div class="title">
                <span>{{ L('RecoveryCode') }}</span>
              </div>
            </Row>
            <Row>
              <div class="desc">
                <span>{{ L('RecoveryCodeDesc') }}</span>
              </div>
            </Row>
          </template>
          <div class="content">
            <div class="key-box">
              <span class="key">{{ recoveryCodes.slice(0, 5).join('\r\n') }}</span>
              <span class="key">{{ recoveryCodes.slice(5).join('\r\n') }}</span>
            </div>
          </div>
        </Card>
      </div>
      <template #actions>
        <div class="actions">
          <Button v-if="step > 0 && !hasSubmit" @click="handlePreStep">{{
            L('Steps:PreStep')
          }}</Button>
          <Button
            v-if="step < 2"
            :loading="submiting"
            :disabled="step === 1 && !hasSubmit"
            type="primary"
            @click="handleNextStep"
            >{{ L('Steps:NextStep') }}</Button
          >
          <Button
            v-if="step === 2"
            :loading="submiting"
            type="primary"
            @click="fetchAuthenticator"
            >{{ L('Steps:Done') }}</Button
          >
        </div>
      </template>
    </Card>
    <Card v-else-if="authenticator?.isAuthenticated === true" :title="L('Authenticator')">
      <List>
        <ListItem>
          <template #extra>
            <Button
              class="extra"
              type="primary"
              :loading="submiting"
              @click="handleRestAuthenticator"
            >
              {{ L('ResetAuthenticator') }}
            </Button>
          </template>
          <ListItemMeta>
            <template #title>
              {{ L('ResetAuthenticator') }}
            </template>
            <template #description>
              <div>
                <span>{{ L('ResetAuthenticatorDesc') }}</span>
              </div>
            </template>
          </ListItemMeta>
        </ListItem>
      </List>
    </Card>
  </CollapseContainer>
</template>

<script setup lang="ts">
  import type { FormInstance } from 'ant-design-vue/lib/form/Form';
  import { ref, onMounted } from 'vue';
  import { Button, Card, Col, Form, List, Input, Row, Steps } from 'ant-design-vue';
  import { QrCode } from '/@/components/Qrcode';
  import { CollapseContainer } from '/@/components/Container';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { copyTextToClipboard } from '/@/hooks/web/useCopyToClipboard';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import {
    getAuthenticator,
    verifyAuthenticatorCode,
    resetAuthenticator,
  } from '/@/api/account/profiles';
  import { AuthenticatorDto } from '/@/api/account/profiles/model';

  const FormItem = Form.Item;
  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;
  const Step = Steps.Step;

  const form = ref<FormInstance>();
  const authenticator = ref<AuthenticatorDto>({});
  const submiting = ref(false);
  const hasSubmit = ref(false);
  const formModel = ref<{
    code?: string;
  }>({});
  const recoveryCodes = ref<string[]>([]);
  const step = ref(0);
  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AbpAccount', 'AbpIdentity', 'AbpUi']);

  async function fetchAuthenticator() {
    hasSubmit.value = false;
    submiting.value = false;
    authenticator.value = await getAuthenticator();
  }

  async function handleValidateCode() {
    try {
      await form.value?.validate();
      submiting.value = true;
      const result = await verifyAuthenticatorCode({ authenticatorCode: formModel.value.code! });
      recoveryCodes.value = result.recoveryCodes;
      hasSubmit.value = true;
      handleNextStep();
      formModel.value = {};
    } finally {
      submiting.value = false;
    }
  }

  async function handleRestAuthenticator() {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ResetAuthenticatorWarning'),
      onOk: async () => {
        try {
          await resetAuthenticator();
          await fetchAuthenticator();
        } finally {
          submiting.value = false;
        }
      },
    });
  }

  function handleCopy(val?: string) {
    if (val && copyTextToClipboard(val)) {
      createMessage.success(L('Successful'));
    }
  }

  function handlePreStep() {
    step.value -= 1;
  }

  function handleNextStep() {
    step.value += 1;
  }

  onMounted(fetchAuthenticator);
</script>

<style lang="scss" scoped>
  .steps {
    margin-top: 30px;
  }
  .actions {
    display: flex;
    justify-content: flex-end;
    margin-right: 15px;
    gap: 10px;
  }

  .title {
    font-size: 18px;
    font-weight: 300;
  }

  .desc {
    font-size: 12px;
    color: grey;
  }

  .card {
    height: 100%;

    .content {
      display: flex;
      justify-content: center;
      align-items: center;

      .key-box {
        display: flex;
        flex-direction: column;
        background-color: rgb(218, 198, 198);
        font-size: 20px;
        font-weight: bold;
        height: 100%;
        border-radius: 12px;
        justify-content: center;
        align-items: center;

        .key {
          color: blue;
          margin: 16px;
        }
      }
    }
  }
</style>
