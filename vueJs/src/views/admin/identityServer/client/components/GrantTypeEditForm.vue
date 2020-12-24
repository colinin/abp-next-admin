<template>
  <div>
    <el-form
      ref="formEditGrantType"
      :model="newGrantType"
    >
      <el-form-item
        prop="grantType"
        label="授权类型"
      >
        <el-select
          v-model="newGrantType.grantType"
          style="width: 100%"
          filterable
          allow-create
          clearable
        >
          <el-option
            v-for="(grantType, index) in grantTypeOptions"
            :key="index"
            :label="grantType"
            :value="grantType"
          />
        </el-select>
      </el-form-item>

      <el-form-item>
        <el-button
          type="success"
          class="add-button"
          @click="onSave"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          添加一个新的
        </el-button>
      </el-form-item>
    </el-form>

    <el-table
      :row-key="propName"
      :data="grantTypes"
      :stripe="true"
      :show-header="false"
      highlight-current-row
      style="width: 100%; margin-bottom: 30px;"
    >
      <el-table-column
        :prop="propName"
        sortable
        align="left"
      >
        <template slot-scope="{row}">
          <span>{{ row[propName] }}</span>
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
            @click="onDelete(row[propName])"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { Form } from 'element-ui'
import { ClientGrantType } from '@/api/clients'
import IdentityServer4Service from '@/api/identity-server4'
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'GrantTypeEditForm',
  model: {
    prop: 'grantTypes',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => { return [] } })
  private grantTypes!: [{[key: string]: any}]

  @Prop({ default: 'grantType' })
  private propName!: string

  private newGrantType = new ClientGrantType()
  private supportedGrantypes = new Array<string>()

  get grantTypeOptions() {
    return this.supportedGrantypes
      .filter(grant => !this.grantTypes.some(x => x[this.propName] === grant))
  }

  mounted() {
    this.handleGetSupportGrantTypes()
  }

  private handleGetSupportGrantTypes() {
    IdentityServer4Service
      .getOpenIdConfiguration()
      .then(res => {
        this.supportedGrantypes = res.grant_types_supported
      })
  }

  private onSave() {
    const formEditGrantType = this.$refs.formEditGrantType as Form
    formEditGrantType.validate(valid => {
      if (valid) {
        const grantType: {[key: string]: any} = {}
        grantType[this.propName] = this.newGrantType.grantType
        this.$emit('change', this.grantTypes.concat(grantType))
        formEditGrantType.resetFields()
      }
    })
  }

  private onDelete(key: any) {
    this.$emit('change', this.grantTypes.filter(type => type[this.propName] !== key))
  }
}
</script>

<style scoped>
.add-button {
  width: 140px;
  float: right;
}
</style>
