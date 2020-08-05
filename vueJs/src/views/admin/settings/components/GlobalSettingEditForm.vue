<template>
  <SettingEditForm
    provider-name="G"
  />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import SettingEditForm from './SettingEditForm.vue'

@Component({
  name: 'GlobalSettingEditForm',
  components: {
    SettingEditForm
  }
})
export default class extends Vue {}
</script>
