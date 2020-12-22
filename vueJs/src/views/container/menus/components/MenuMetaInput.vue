<template>
  <div>
    <el-input
      v-if="dataItem.valueType===0"
      :value="value"
      @input="onInputMetaChanged"
    />
    <el-input
      v-else-if="dataItem.valueType===1"
      :value="value"
      type="number"
      @input="onInputMetaChanged"
    />
    <el-switch
      v-else-if="dataItem.valueType===2"
      :value="inputBoolValue(value)"
      @change="onInputMetaChanged"
    />
    <el-date-picker
      v-else-if="dataItem.valueType===3"
      :value="value"
      @change="onInputMetaChanged"
    />
    <el-date-picker
      v-else-if="dataItem.valueType===4"
      :value="value"
      type="datetime"
      @change="onInputMetaChanged"
    />
    <el-input-tag
      v-else-if="dataItem.valueType===5"
      :value="inputArrayValue(value)"
      @input="onInputMetaChanged"
    />
    <input-object
      v-else-if="dataItem.valueType===6"
      :value="inputObjectValue"
      @input="onInputMetaChanged"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { DataItem } from '@/api/data-dictionary'
import { isBoolean } from 'lodash'
import ElInputTag from '@/components/InputTag/index.vue'
import InputObject from '@/components/InputObject/index.vue'
import { isArray } from '@/utils/validate'

@Component({
  name: 'MenuMetaInput',
  components: {
    ElInputTag,
    InputObject
  }
})
export default class MenuMetaInput extends Vue {
  @Prop({ default: null })
  private value: any

  @Prop({ default: () => { return new DataItem() } })
  private dataItem!: DataItem

  @Prop({ default: '' })
  private propName!: string

  get inputBoolValue() {
    return (value: any) => {
      if (isBoolean(value)) {
        return value
      }
      return value === 'true'
    }
  }

  set inputBoolValue(value: any) {
    this.value = value
  }

  get inputArrayValue() {
    return (value: any) => {
      if (value) {
        return isArray(value) ? value : String(value).split(',')
      }
      return []
    }
  }

  set inputArrayValue(value: any) {
    if (isArray(value)) {
      this.value = value
    }
    this.value = String(value).split(',')
  }

  get inputObjectValue() {
    if (typeof this.value === 'object') {
      return this.value
    }
    if (typeof this.value === 'string') {
      return JSON.parse(this.value)
    }
    return JSON.parse(String(this.value))
  }

  onInputMetaChanged(value: any) {
    this.$emit('input', value)
  }
}
</script>

<style lang="stylus" scoped>

</style>
