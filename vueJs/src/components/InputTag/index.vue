<template>
  <div style="width:auto; height:auto;">
    <div
      class="el-input-tag input-tag-wrapper"
      :class="[size ? 'el-input-tag--' + size : '']"
      @click="foucusTagInput"
    >
      <span v-if="innerTags.length>0">
        <el-tag
          v-for="(tag, idx) in innerTags"
          :key="tag"
          :size="size"
          :closable="!readOnly"
          :disable-transitions="false"
          @close="remove(idx)"
        >
          {{ tag }}
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

@Component({
  name: 'ElInputTag'
})
export default class extends Vue {
  @Prop({ default: () => [] })
  private value!: string[]

  @Prop({ default: () => [13, 188, 9] })
  private addTagOnKeys!: Array<number>

  @Prop({ default: false })
  private readOnly!: boolean

  private size = AppModule.size
  public newTag: string
  public innerTags: string[]

  constructor() {
    super()
    this.newTag = ''
    this.innerTags = new Array<string>()
  }

  @Watch('value', { immediate: true })
  private onValueChanged(val: string[]) {
    this.innerTags = val
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

  private addTag(tag: any) {
    if (!tag) {
      return false
    }
    tag = tag.trim()
    if (!this.innerTags.includes(tag)) {
      this.innerTags.push(tag)
      return true
    }
    return false
  }

  private remove(index: number) {
    this.innerTags.splice(index, 1)
    this.tagChange()
  }

  private removeLastTa() {
    if (this.newTag) {
      return false
    }
    this.innerTags.pop()
    this.tagChange()
  }

  private tagChange() {
    this.$emit('input', this.innerTags)
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
