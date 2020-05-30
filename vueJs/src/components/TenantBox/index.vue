<template>
  <div>
    <el-row justify="center">
      <el-col :span="4">
        <label>{{ $t('AbpUiMultiTenancy.Tenant') }}</label>
      </el-col>
      <el-col :span="16">
        <label>{{ value }}</label>
      </el-col>
      <el-col :span="4">
        <el-link
          type="info"
          @click="handleSwitchTenant"
        >
          {{ $t('AbpUiMultiTenancy.SwitchTenant') }}
        </el-link>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import TenantService from '@/api/tenant'
import { setTenant, removeTenant } from '@/utils/sessions'

@Component({
  name: 'TenantSelect'
})
export default class extends Vue {
  @Prop({ default: '' })
  private value?: string

  private handleSwitchTenant() {
    this.$prompt(this.$t('AbpUiMultiTenancy.SwitchTenantHint').toString(),
      this.$t('AbpUiMultiTenancy.SwitchTenant').toString(), {
        showInput: true
      }).then((val: any) => {
      removeTenant()
      if (val.value) {
        TenantService.getTenantByName(val.value).then(tenant => {
          if (tenant.success) {
            setTenant(tenant.tenantId)
            this.$emit('input', tenant.name)
          } else {
            this.$message.warning(this.$t('login.tenantIsNotAvailable', { name: val.value }).toString())
          }
        })
      } else {
        this.$emit('input', '')
      }
    }).catch(() => {
      console.log()
    })
  }
}
</script>
