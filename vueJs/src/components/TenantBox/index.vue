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
          @click="handleSwitchTenant"
        >
          ({{ $t('AbpUiMultiTenancy.SwitchTenant') }})
        </el-link>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import TenantService from '@/api/tenant-management'
import { AbpModule } from '@/store/modules/abp'

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
      if (val.value) {
        TenantService.findTenantByName(val.value).then(tenant => {
          if (tenant.success) {
            AbpModule.configuration.currentTenant.isAvailable = true
            AbpModule.configuration.currentTenant.id = tenant.tenantId
            AbpModule.configuration.currentTenant.name = tenant.name
            this.$emit('input', tenant.name)
          } else {
            AbpModule.configuration.currentTenant.isAvailable = false
            this.$message.warning(this.$t('login.tenantIsNotAvailable', { name: val.value }).toString())
          }
        })
      } else {
        AbpModule.configuration.currentTenant.isAvailable = false
        AbpModule.Initialize().finally(() => {
          this.$emit('input', '')
        })
      }
    }).catch(() => {
      console.log()
    })
  }
}
</script>
