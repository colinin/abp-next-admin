<template>
  <div id="app">
    <router-view />
    <service-worker-update-popup />
  </div>
</template>

<script lang="ts">
import { AbpModule } from '@/store/modules/abp'
import { Component, Vue } from 'vue-property-decorator'
import ServiceWorkerUpdatePopup from '@/pwa/components/ServiceWorkerUpdatePopup.vue'

@Component({
  name: 'App',
  components: {
    ServiceWorkerUpdatePopup
  }
})
export default class extends Vue {
  created() {
    this.initializeAbpConfiguration()
  }

  private async initializeAbpConfiguration() {
    await AbpModule.GetAbpConfiguration()
  }
}
</script>
