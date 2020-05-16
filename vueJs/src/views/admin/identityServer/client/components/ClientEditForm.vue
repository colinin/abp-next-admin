<template>
  <el-form
    ref="formClient"
    label-width="100px"
    :model="client"
  >
    <el-tabs>
      <el-tab-pane :label="$t('identityServer.basicOptions')">
        <el-form-item
          prop="enabled"
          :label="$t('identityServer.enabled')"
        >
          <el-switch
            v-model="client.enabled"
          />
        </el-form-item>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="clientId"
              :label="$t('identityServer.clientId')"
            >
              <el-input
                v-model="client.clientId"
                :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientId')})"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="clientName"
              :label="$t('identityServer.clientName')"
            >
              <el-input
                v-model="client.clientName"
                :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientName')})"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="description"
          :label="$t('identityServer.description')"
        >
          <el-input
            v-model="client.description"
          />
        </el-form-item>
        <el-form-item
          prop="protocolType"
          :label="$t('identityServer.protocolType')"
        >
          <el-select
            v-model="client.protocolType"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('identityServer.protocolType')})"
          >
            <el-option
              key="oidc"
              label="OpenID Connect"
              value="oidc"
            />
          </el-select>
        </el-form-item>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="requirePkce"
              :label="$t('identityServer.requirePkce')"
            >
              <el-switch
                v-model="client.requirePkce"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="allowPlainTextPkce"
              :label="$t('identityServer.allowPlainTextPkce')"
            >
              <el-switch
                v-model="client.allowPlainTextPkce"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="allowOfflineAccess"
              :label="$t('identityServer.allowOfflineAccess')"
            >
              <el-switch
                v-model="client.allowOfflineAccess"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="allowAccessTokensViaBrowser"
              :label="$t('identityServer.allowAccessTokensViaBrowser')"
            >
              <el-switch
                v-model="client.allowAccessTokensViaBrowser"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="allowedScopes"
          :label="$t('identityServer.allowedScopes')"
        >
          <el-input-tag-ex
            v-model="client.allowedScopes"
            :data="client.allowedScopes"
            label="scope"
          />
        </el-form-item>
        <el-form-item
          prop="redirectUris"
          :label="$t('identityServer.redirectUris')"
        >
          <el-input-tag-ex
            v-model="client.redirectUris"
            :data="client.redirectUris"
            label="redirectUri"
            validate="url"
          />
        </el-form-item>
        <el-form-item
          prop="allowedGrantTypes"
          :label="$t('identityServer.allowedGrantTypes')"
        >
          <el-input-tag-ex
            v-model="client.allowedGrantTypes"
            :data="client.allowedGrantTypes"
            label="grantType"
          />
        </el-form-item>
      </el-tab-pane>
    </el-tabs>

    <el-form-item>
      <el-button
        class="cancel"
        style="width:100px"
        @click="onCancel"
      >
        {{ $t('table.cancel') }}
      </el-button>
      <el-button
        class="confirm"
        type="primary"
        style="width:100px"
        @click="onSubmitEdit('formRoute')"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import ElInputTagEx from '@/components/InputTagEx/index.vue'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import ClientService, { ClientGetById, Client } from '@/api/clients'

@Component({
  name: 'ClientEditForm',
  components: {
    ElInputTagEx
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private clientId!: string

  private client: Client

  get isEditRoute() {
    if (this.clientId) {
      return true
    }
    return false
  }

  constructor() {
    super()
    this.client = new Client()
  }

  @Watch('clientId', { immediate: true })
  private handleClientIdChanged(id: string) {
    if (id) {
      ClientService.getClientById(id).then(client => {
        this.client = client
      })
    }
  }

  private onSubmitEdit() {
    const routeEditForm = this.$refs.formClient as any
    routeEditForm.validate(async(valid: boolean) => {
      if (valid) {
        console.log('onSubmitEdit')
        console.log(this.client)
        // this.$message('successful')
        // routeEditForm.resetFields()
        // this.$emit('closed', true)
      }
    })
  }

  private onCancel() {
    const routeEditForm = this.$refs.formClient as any
    routeEditForm.resetFields()
    this.$emit('closed')
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
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
