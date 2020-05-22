<template>
  <div style="width:auto; height:auto;">
    <div
      class="el-input-tag input-tag-wrapper"
      :class="[size ? 'el-input-tag--' + size : '']"
      @click="foucusTagInput"
    >
      <span v-if="data.length>0">
        <el-tag
          v-for="(tag, idx) in data"
          :key="idx"
          :size="size"
          :closable="!readOnly"
          :disable-transitions="false"
          @close="remove(idx)"
        >
          {{ tag[label] }}
        </el-tag>
      </span>
      <input
        v-if="!readOnly"
        v-model="newTag"
        :size="size"
        :class="[size ? 'tag-input--' + size : 'tag-input']"
        @keydown="addNew"
        @blur="addNew"
      >
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { AppModule } from '@/store/modules/app'

/* eslint-disable */
const regExpValidate: { [key: string]: RegExp } = {
  url: /^(?=^.{3,255}$)((http|https|ftp)?:\/\/)?(www\.)?[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+(:\d+)*(\/)?(?:\/(.+)\/?$)?(\/\w+\.\w+)*([\?&]\w+=\w*|[\u4e00-\u9fa5]+)*$/
}
/* eslint-enable */

@Component({
  name: 'ElInputTagEx',
  model: {
    prop: 'value',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => new Array<any>() })
  private value!: any[]

  @Prop({ default: 'key' })
  private label!: string

  @Prop({ default: () => [13, 188, 9] })
  private addTagOnKeys!: Array<number>

  @Prop({ default: false })
  private readOnly!: boolean

  @Prop({ default: 'none' })
  private validate!: string
  // 'none' | 'email' | 'phone' | 'ipaddress' | 'ip:port' | 'key:value' | 'url'

  private data: any[]
  private size = AppModule.size
  public newTag: string

  constructor() {
    super()
    this.newTag = ''
    this.data = new Array<any>()
  }

  @Watch('value', { immediate: true })
  private onValueChanged() {
    this.data = this.value
    this.$parent.$emit('validate', this.data)
  }

  private foucusTagInput() {
    const tagInputClass = this.size ? '.tag-input--' + this.size : '.tag-input'
    if (this.readOnly || !this.$el.querySelector(tagInputClass)) {

    } else {
      const tagInput = this.$el.querySelector(tagInputClass) as any
      tagInput.focus()
    }
  }

  private addNew(e: any) {
    if (e && (!this.addTagOnKeys.includes(e.keyCode)) && (e.type !== 'blur')) {
      return false
    }
    if (e) {
      e.stopPropagation()
      e.preventDefault()
    }
    let addSuucess = false
    if (this.newTag.includes(',')) {
      this.newTag.split(',').forEach(item => {
        if (this.addTag(item.trim())) {
          addSuucess = true
        }
      })
    } else {
      if (this.addTag(this.newTag.trim())) {
        addSuucess = true
      }
    }
    if (addSuucess) {
      this.tagChange()
      this.newTag = ''
    }
  }

  private addTag(tag: string) {
    if (!tag) {
      return false
    }
    tag = tag.trim()
    if (this.validate !== 'none') {
      if (regExpValidate[this.validate] && !regExpValidate[this.validate].test(tag)) {
        return false
      }
    }
    if (this.data.length === 0) {
      this.data.push({ [this.label]: tag })
      return true
    } else if (!this.data.some(d => d[this.label] === tag)) {
      this.data.push({ [this.label]: tag })
      return true
    }
    return false
  }

  private remove(index: number) {
    this.data.splice(index, 1)
    this.tagChange()
  }

  private removeLastTa() {
    if (this.newTag) {
      return false
    }
    this.data.pop()
    this.tagChange()
  }

  private tagChange() {
    this.$emit('change', this.data)
  }
}
</script>

<style lang="scss" scoped>
  .input-tag-wrapper {
    position: relative;
    font-size: 14px;
    background-color: #fff;
    background-image: none;
    border-radius: 4px;
    border: 1px solid #dcdfe6;
    box-sizing: border-box;
    color: #606266;
    display: inline-block;
    outline: none;
    padding: 0 5px 0 5px;
    transition: border-color .2s cubic-bezier(.645,.045,.355,1);
    width: 100%;
    height: 100%;
  }

  .el-tag {
    margin-right: 4px;
    margin-top: 4px;
  }

  .tag-input--default {
    background: transparent;
    border: 0;
    font-size: 14px;
    height: 38px;
    outline: none;
    padding-left: 0;
    width: 150px;
  }

  .tag-input--mini{
    background: transparent;
    border: 0;
    font-size: 14px;
    height: 26px;
    outline: none;
    padding-left: 0;
    width: 150px;
  }

  .tag-input--small{
    background: transparent;
    border: 0;
    font-size: 14px;
    height: 30px;
    outline: none;
    padding-left: 0;
    width: 150px;
  }

  .tag-input--medium{
    background: transparent;
    border: 0;
    font-size: 14px;
    height: 34px;
    outline: none;
    padding-left: 0;
    width: 150px;
  }
</style>
