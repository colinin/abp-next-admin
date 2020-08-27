<template>
  <div class="dashboard-container">
    <iframe
      id="iframe-hangifre"
      ref="hangfireIframe"
      :src="hangfireUrl"
      scrolling="auto"
      frameborder="0"
      class="iframe-hangifre"
    />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'HangfireDashboard'
})
export default class extends Vue {
  @Prop({ default: '' })
  private hangfireUrl!: string

  get showIframe() {
    if (this.hangfireUrl) {
      return true
    }
    return false
  }

  mounted() {
    const hangfireFrame = this.$refs.hangfireIframe as any
    if (hangfireFrame) {
      const deviceWidth = document.documentElement.clientWidth
      const deviceHeight = document.documentElement.clientHeight
      hangfireFrame.style.width = deviceWidth + 'px'
      hangfireFrame.style.height = deviceHeight + 'px'
    }
  }
}
</script>

<style lang="scss" scoped>
.iframe-hangifre {
  width: 100%;
  height: 100%;
  overflow: hidden;
}
</style>
