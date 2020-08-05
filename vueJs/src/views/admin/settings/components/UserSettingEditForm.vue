<template>
  <SettingEditForm
    provider-name="U"
    :provider-key="userId"
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
  private userId?: string
}
</script>
