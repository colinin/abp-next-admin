<template>
  <div id="app">
    <router-view />
    <service-worker-update-popup />
  </div>
</template>

<script lang="ts">
import { AbpModule } from '@/store/modules/abp'
import { HttpProxyModule } from '@/store/modules/http-proxy'
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
    await HttpProxyModule.Initialize()
    await AbpModule.LoadAbpConfiguration()
  }
}
</script>
