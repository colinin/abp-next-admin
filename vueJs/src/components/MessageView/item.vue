<template>
  <div class="item-inner">
    <div class="head">
      <el-avatar
        :icon="{iconType}"
        :style="{iconStyle}"
      />
      <span class="name">{{ source.name }}</span>
      <span class="time">{{ renderDateTime(source.datetime) }}</span>
    </div>
    <div class="desc">
      {{ source.description }}
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'

export class Message {
  id!: string
  title!: string
  message!: string
  datetime!: Date
  description?: string
  exception?: string
}

@Component({
  name: 'Item'
})
export default class extends Vue {
  private source: Message = new Message()
  private iconType: string = this.source.exception ? 'el-icon-circle-close' : 'el-icon-circle-check'
  private iconStyle: string = this.source.exception ? 'backgroundColor: #f56a00' : 'backgroundColor: #87d068'
  private renderDateTime(time: Date) {
    return dateFormat(time, 'YYYY-mm-dd HH:MM:SS')
  }
}
</script>

<style lang="scss" scoped>
.item-inner {
  .head {
    font-weight: 500;
  }
  .name {
    margin-left: 1em;
  }
  .time {
    margin-right: 1em;
    margin-top: 10px;
  }
  .desc {
    padding-top: .5em;
    text-align: justify;
  }
}
</style>
