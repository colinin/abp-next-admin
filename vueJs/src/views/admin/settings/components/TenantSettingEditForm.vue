<template>
  <SettingEditForm
    provider-name="T"
    :provider-key="tenantId"
  />
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import SettingEditForm from './SettingEditForm.vue'

@Component({
  name: 'TenantSettingEditForm',
  components: {
    SettingEditForm
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private tenantId?: string
}
</script>
