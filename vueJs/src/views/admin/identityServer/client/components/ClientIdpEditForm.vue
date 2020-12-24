<template>
  <div>
    <h3 style="margin-bottom: 30px;">
      {{ $t('AbpIdentityServer.Client:IdentityProviderRestrictions') }}
    </h3>
    <el-form
      ref="formEditIdp"
      :model="newProvider"
    >
      <el-form-item
        prop="enableLocalLogin"
        label-width="0"
      >
        <el-checkbox
          v-model="client.enableLocalLogin"
          class="label-title"
        >
          {{ $t('AbpIdentityServer.Client:EnableLocalLogin') }}
        </el-checkbox>
      </el-form-item>
      <el-form-item
        prop="provider"
        :label="$t('AbpIdentityServer.Name')"
      >
        <el-input
          v-model="newProvider.provider"
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
      row-key="provider"
      :data="client.identityProviderRestrictions"
      :show-header="false"
      highlight-current-row
      style="width: 100%; margin-bottom: 30px;"
    >
      <el-table-column
        prop="provider"
        sortable
        align="left"
      >
        <template slot-scope="{row}">
          <span>{{ row.provider }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Actions')"
        align="right"
        min-width="80px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="onDelete(row.provider)"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { Form } from 'element-ui'
import { Client, ClientIdPRestriction } from '@/api/clients'
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'ClientIdpEditForm',
  model: {
    prop: 'providers',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => { return new Client() } })
  private client!: Client

  private newProvider = new ClientIdPRestriction()

  private onSave() {
    const formEditIdp = this.$refs.formEditIdp as Form
    formEditIdp.validate(valid => {
      if (valid) {
        this.$emit('change', this.client.identityProviderRestrictions.concat({
          provider: this.newProvider.provider
        }))
        formEditIdp.resetFields()
      }
    })
  }

  private onDelete(key: any) {
    this.$emit('change', this.client.identityProviderRestrictions.filter(provider => provider.provider !== key))
  }
}
</script>

<style scoped>
.add-button {
  width: 150px;
  float: right;
}
</style>
