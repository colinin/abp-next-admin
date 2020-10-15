<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.clientClaim')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <div class="app-container">
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
          <el-select
            v-model="clientClaim.type"
            style="width: 100%"
            :placeholder="$t('pleaseSelectBy', {key: $t('identityServer.claimType')})"
            @change="onClaimTypeChanged"
          >
            <el-option
              v-for="claim in claimTypes"
              :key="claim.id"
              :label="claim.name"
              :value="claim.name"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('identityServer.claimValue')"
        >
          <el-input
            v-if="hasStringValueType(clientClaim.type)"
            v-model="clientClaim.value"
            type="text"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.claimValue')})"
          />
          <el-input
            v-else-if="hasIntegerValueType(clientClaim.type)"
            v-model="clientClaim.value"
            type="number"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.claimValue')})"
          />
          <el-switch
            v-else-if="hasBooleanValueType(clientClaim.type)"
            v-model="clientClaim.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.claimValue')})"
          />
          <el-date-picker
            v-else-if="hasDateTimeValueType(clientClaim.type)"
            v-model="clientClaim.value"
            type="datetime"
            style="width: 100%"
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
        width="150px"
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
        min-width="100%"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ claimValue(row.type, row.value) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.Clients.Claims.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteClientClaim(row.type, row.value)"
          >
            {{ $t('identityServer.deleteClaim') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import { dateFormat } from '@/utils/index'
import ClaimTypeApiService, { IdentityClaimType, IdentityClaimValueType } from '@/api/cliam-type'
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
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => new Array<ClientClaim>() })
  private clientClaims!: ClientClaim[]

  private clientClaim: ClientClaimCreate
  private claimTypes = new Array<IdentityClaimType>()
  private clientClaimRules = {
    type: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.claimType') }), trigger: 'change' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.claimValue') }), trigger: 'blur' }
    ]
  }

  get cliamType() {
    return (claimName: string) => {
      const claimIndex = this.claimTypes.findIndex(cliam => cliam.name === claimName)
      if (claimIndex >= 0) {
        return this.claimTypes[claimIndex].valueType
      }
      return IdentityClaimValueType.String
    }
  }

  get claimValue() {
    return (type: string, value: string) => {
      const valueType = this.cliamType(type)
      switch (valueType) {
        case IdentityClaimValueType.Int :
        case IdentityClaimValueType.String :
          return value
        case IdentityClaimValueType.Boolean :
          return value.toLowerCase() === 'true'
        case IdentityClaimValueType.DateTime :
          return dateFormat(new Date(value), 'YYYY-mm-dd HH:MM:SS')
      }
    }
  }

  get hasStringValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.String
    }
  }

  get hasBooleanValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.Boolean
    }
  }

  get hasDateTimeValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.DateTime
    }
  }

  get hasIntegerValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.Int
    }
  }

  constructor() {
    super()
    this.clientClaim = new ClientClaimCreate()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.clientClaim.clientId = this.clientId
  }

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      this.claimTypes = res.items
    })
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

  private onClaimTypeChanged() {
    const valueType = this.cliamType(this.clientClaim.type)
    switch (valueType) {
      case IdentityClaimValueType.Int :
        this.clientClaim.value = '0'
        break
      case IdentityClaimValueType.String :
        this.clientClaim.value = ''
        break
      case IdentityClaimValueType.Boolean :
        this.clientClaim.value = 'false'
        break
      case IdentityClaimValueType.DateTime :
        this.clientClaim.value = ''
        break
    }
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

  private onFormClosed() {
    this.resetFields()
    this.$emit('closed')
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
