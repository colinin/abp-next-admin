<template>
  <div>
    <el-form-item
      prop="alwaysSendClientClaims"
      label-width="0"
    >
      <el-checkbox
        v-model="client.alwaysSendClientClaims"
        class="label-title"
      >
        {{ $t('AbpIdentityServer.Client:AlwaysSendClientClaims') }}
      </el-checkbox>
    </el-form-item>
    <el-form-item
      prop="alwaysIncludeUserClaimsInIdToken"
      label-width="0"
    >
      <el-checkbox
        v-model="client.alwaysIncludeUserClaimsInIdToken"
        class="label-title"
      >
        {{ $t('AbpIdentityServer.Client:AlwaysIncludeUserClaimsInIdToken') }}
      </el-checkbox>
    </el-form-item>
    <el-form
      ref="clientClaimForm"
      v-permission="['AbpIdentityServer.Clients.ManageClaims']"
      label-width="100px"
      :model="clientClaim"
    >
      <el-form-item
        prop="type"
        :label="$t('AbpIdentityServer.Claims:Type')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Type')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="clientClaim.type"
          style="width: 100%"
          :placeholder="$t('pleaseSelectBy', {key: $t('AbpIdentityServer.Claims:Type')})"
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
        :label="$t('AbpIdentityServer.Claims:Value')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Value')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-if="hasStringValueType(clientClaim.type)"
          v-model="clientClaim.value"
          type="text"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Value')})"
        />
        <el-input
          v-else-if="hasIntegerValueType(clientClaim.type)"
          v-model="clientClaim.value"
          type="number"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Value')})"
        />
        <el-switch
          v-else-if="hasBooleanValueType(clientClaim.type)"
          v-model="clientClaim.value"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Value')})"
        />
        <el-date-picker
          v-else-if="hasDateTimeValueType(clientClaim.type)"
          v-model="clientClaim.value"
          type="datetime"
          style="width: 100%"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Claims:Value')})"
        />
      </el-form-item>

      <el-form-item>
        <el-button
          type="success"
          class="add-button"
          @click="onSave"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AbpIdentityServer.AddNew') }}
        </el-button>
      </el-form-item>
    </el-form>

    <el-table
      row-key="clientId"
      :data="clientClaims"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('AbpIdentityServer.Claims:Type')"
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
        :label="$t('AbpIdentityServer.Claims:Value')"
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
        :label="$t('AbpIdentityServer.Actions')"
        align="center"
        width="150px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentityServer.Clients.ManageClaims'])"
            size="mini"
            type="danger"
            @click="onDeleted(row.type, row.value)"
          >
            {{ $t('AbpIdentityServer.Claims:Delete') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils/index'
import ClaimTypeApiService, { IdentityClaimType, IdentityClaimValueType } from '@/api/cliam-type'
import { Client, ClientClaim } from '@/api/clients'
import { Component, Mixins, Prop } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import { checkPermission } from '@/utils/permission'
import { Form } from 'element-ui'

@Component({
  name: 'ClientClaimEditForm',
  methods: {
    checkPermission
  },
  model: {
    prop: 'clientClaims',
    event: 'change'
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: () => { return new Client() } })
  private client!: Client

  @Prop({ default: () => new Array<ClientClaim>() })
  private clientClaims!: ClientClaim[]

  private clientClaim = new ClientClaim('', '')
  private claimTypes = new Array<IdentityClaimType>()

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
          return value
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

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      this.claimTypes = res.items
    })
  }

  private onDeleted(type: string, value: string) {
    this.$emit('change', this.clientClaims.filter(claim => claim.value !== value || claim.type !== type))
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

  private onSave() {
    const clientClaimForm = this.$refs.clientClaimForm as Form
    clientClaimForm.validate((valid: boolean) => {
      if (valid) {
        if (this.clientClaims.some(claim => claim.value === this.clientClaim.value || claim.type === this.clientClaim.type)) {
          this.$message.warning(this.l('AbpIdentityServer.Claims:DuplicateValue'))
          return
        }
        this.$emit('change', this.clientClaims.concat({
          type: this.clientClaim.type,
          value: this.clientClaim.value
        }))
        clientClaimForm.resetFields()
      }
    })
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
.add-button {
  width: 150px;
  float: right;
}
</style>
