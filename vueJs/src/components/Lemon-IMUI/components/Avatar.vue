<template>
  <span
    :style="style"
    class="lemon-avatar"
    @click="onClick"
  >
    <i
      v-if="imageFinishLoad"
      :class="icon"
    />
    <img
      :src="src"
      @load="onLoad"
    >
  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'

@Component({
  name: 'LemonAvatar'
})
export default class extends Vue {
  @Prop({ default: '' })
  private src!: string

  @Prop({ default: 'lemon-icon-people' })
  private icon!: string

  @Prop({ default: 32 })
  private size!: number

  get style() {
    const size = `${this.size}px`
    return {
      width: size,
      height: size,
      lineHeight: size,
      fontSize: `${this.size / 2}px`
    }
  }

  private imageFinishLoad = true

  private onClick(e: any) {
    this.$emit('click', e)
  }

  private onLoad() {
    this.imageFinishLoad = false
  }
}
</script>
