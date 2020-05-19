<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="formClient"
        label-width="100px"
        :model="client"
        :rules="clientRules"
      >
        <el-form-item
          prop="clientId"
          :label="$t('identityServer.clientId')"
        >
          <el-input
            v-model="client.clientId"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientId')})"
          />
        </el-form-item>
        <el-form-item
          prop="clientName"
          :label="$t('identityServer.clientName')"
        >
          <el-input
            v-model="client.clientName"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientName')})"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.description')"
        >
          <el-input
            v-model="client.description"
          />
        </el-form-item>
        <el-form-item
          prop="allowedGrantTypes"
          :label="$t('identityServer.allowedGrantTypes')"
          label-width="120px"
        >
          <el-input-tag-ex
            v-model="client.allowedGrantTypes"
            :data="client.allowedGrantTypes"
            label="grantType"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveClient"
          >
            {{ $t('identityServer.createClient') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script lang="ts">
import ClientService, { ClientCreate } from '@/api/clients'
import { Component, Vue } from 'vue-property-decorator'
import ElInputTagEx from '@/components/InputTagEx/index.vue'

@Component({
  name: 'ClientCreateForm',
  components: {
    ElInputTagEx
  }
})
export default class extends Vue {
  private client: ClientCreate
  private clientRules = {
    clientId: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientId') }), trigger: 'change' }
    ],
    clientName: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientName') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.client = new ClientCreate()
  }

  private onSaveClient() {
    const frmClient = this.$refs.formClient as any
    frmClient.validate((valid: boolean) => {
      if (valid) {
        ClientService.createClient(this.client).then(client => {
          const successMessage = this.l('identityServer.createClientSuccess', { id: client.clientId })
          this.$message.success(successMessage)
          frmClient.resetFields()
          this.$emit('clientChanged')
          this.$emit('closed')
        })
      }
    })
  }

  public resetFields() {
    const frmClient = this.$refs.formClient as any
    frmClient.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>
