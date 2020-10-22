<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentityServer.Client:New')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="formClient"
      label-width="100px"
      :model="client"
    >
      <el-form-item
        prop="clientId"
        :label="$t('AbpIdentityServer.Client:Id')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:Id')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="client.clientId"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:Id')})"
        />
      </el-form-item>
      <el-form-item
        prop="clientName"
        :label="$t('AbpIdentityServer.Name')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="client.clientName"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')})"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t('AbpIdentityServer.Description')"
      >
        <el-input
          v-model="client.description"
        />
      </el-form-item>
      <el-form-item
        prop="allowedGrantTypes"
        :label="$t('AbpIdentityServer.Client:AllowedGrantTypes')"
        label-width="120px"
      >
        <el-select
          v-model="client.allowedGrantTypes"
          multiple
          filterable
          allow-create
          clearable
          class="full-select"
        >
          <el-option
            v-for="(grantType, index) in supportedGrantypes"
            :key="index"
            :label="grantType"
            :value="grantType"
          />
        </el-select>
      </el-form-item>

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed(false)"
        >
          {{ $t('AbpIdentityServer.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'

import { Form } from 'element-ui'
import ClientClaimEditForm from './ClientClaimEditForm.vue'
import SecretEditForm from '../../components/SecretEditForm.vue'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'

import ClientApiService, { ClientCreate } from '@/api/clients'

@Component({
  name: 'ClientCreateForm',
  components: {
    SecretEditForm,
    PropertiesEditForm,
    ClientClaimEditForm
  }
})
export default class ClientCreateForm extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: () => { return new Array<string>() } })
  private supportedGrantypes!: string[]

  private client = new ClientCreate()

  private onSave() {
    const clientEditForm = this.$refs.formClient as Form
    clientEditForm.validate(valid => {
      if (valid) {
        ClientApiService.createClient(this.client)
          .then(() => {
            this.$message.success(this.$t('global.successful').toString())
            this.onFormClosed(true)
          })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    const clientEditForm = this.$refs.formClient as Form
    clientEditForm.resetFields()
    this.$emit('closed', changed)
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
}
</style>
