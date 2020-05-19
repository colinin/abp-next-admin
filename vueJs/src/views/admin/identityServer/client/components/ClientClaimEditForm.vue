<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="formClientClaim"
        label-width="100px"
        :model="clientClaim"
        :rules="clientClaimRules"
      >
        <el-form-item
          prop="type"
          :label="$t('identityServer.claimType')"
        >
          <el-input
            v-model="clientClaim.type"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.claimType')})"
          />
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('identityServer.claimValue')"
        >
          <el-input
            v-model="clientClaim.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.claimValue')})"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            :disabled="!checkPermission(['IdentityServer.Clients.Claims.Create'])"
            @click="onSaveClientClaim"
          >
            {{ $t('identityServer.createClaim') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>

    <el-divider />

    <el-table
      row-key="clientId"
      :data="clientClaims"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('identityServer.claimType')"
        prop="type"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.claimValue')"
        prop="value"
        sortable
        min-width="400px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.Clients.Claims.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteClientClaim(row.type, row.value)"
          >
            {{ $t('identityServer.deleteClaim') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import ClientService, { ClientClaim, ClientClaimCreate } from '@/api/clients'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'ClientClaimEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => new Array<ClientClaim>() })
  private clientClaims!: ClientClaim[]

  private clientClaim: ClientClaimCreate
  private clientClaimRules = {
    type: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.claimType') }), trigger: 'change' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.claimValue') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.clientClaim = new ClientClaimCreate()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.clientClaim.clientId = this.clientId
  }

  private handleDeleteClientClaim(type: string, value: string) {
    this.$confirm(this.l('identityServer.deleteClaimByType', { type: value }),
      this.l('identityServer.deleteClaim'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClientService.deleteClientClaim(this.clientId, type, value).then(() => {
              const deleteClaimIndex = this.clientClaims.findIndex(claim => claim.type === type && claim.value === value)
              this.clientClaims.splice(deleteClaimIndex, 1)
              this.$message.success(this.l('identityServer.deleteClaimSuccess', { type: value }))
              this.$emit('clientClaimChanged')
            })
          }
        }
      })
  }

  private onSaveClientClaim() {
    const frmClientClaim = this.$refs.formClientClaim as any
    frmClientClaim.validate((valid: boolean) => {
      if (valid) {
        this.clientClaim.clientId = this.clientId
        ClientService.addClientClaim(this.clientClaim).then(claim => {
          this.clientClaims.push(claim)
          const successMessage = this.l('identityServer.createClaimSuccess', { type: this.clientClaim.type })
          this.$message.success(successMessage)
          frmClientClaim.resetFields()
          this.$emit('clientClaimChanged')
        })
      }
    })
  }

  public resetFields() {
    const frmClientClaim = this.$refs.formClientClaim as any
    frmClientClaim.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
</style>
