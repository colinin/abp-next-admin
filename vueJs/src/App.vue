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
export default class App extends Vue {
  created() {
    this.initialize()
  }

  private async initialize() {
    await HttpProxyModule.Initialize()
    await AbpModule.Initialize()
  }
}
</script>
