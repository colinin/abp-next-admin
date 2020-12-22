<template>
  <div>
    <el-row
      v-for="(key, index) in Object.keys(objectValue)"
      :key="index"
      style="margin-bottom: 10px;"
    >
      <el-col :span="10">
        <span style="width: 30%;">名称</span>
        <el-input
          :value="key"
          style="width: 80%;margin-left: 10px;"
        />
      </el-col>
      <el-col :span="10">
        <span style="width: 30%;">值</span>
        <el-input
          :value="value[key]"
          style="width: 87%;margin-left: 10px;"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="danger"
          icon="el-icon-delete"
          style="width: 100%;"
          @click="onItemDeleted(key)"
        >
          删除项目
        </el-button>
      </el-col>
    </el-row>
    <el-row style="margin-top: 10px;">
      <el-col :span="10">
        <span style="width: 30%;">名称</span>
        <el-input
          v-model="newItemName"
          style="width: 80%;margin-left: 10px;"
        />
      </el-col>
      <el-col :span="10">
        <span style="width: 30%;">值</span>
        <el-input
          v-model="newItemValue"
          style="width: 87%;margin-left: 10px;"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="success"
          style="width: 100%;"
          @click="onItemAdded(newItemName, newItemValue)"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          添加项目
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'

@Component({
  name: 'InputObject'
})
export default class extends Vue {
  @Prop({ default: () => { return {} } })
  private value!: object

  private newItemName = ''
  private newItemValue = ''

  get objectValue() {
    if (this.value) {
      return this.value
    }
    return {}
  }

  onItemAdded(key: string, value: any) {
    if (key && value) {
      let added: {[key: string]: any} = {}
      added = Object.assign(this.objectValue, added)
      added[key] = value
      console.log(added)
      this.$emit('input', added)
      this.newItemName = ''
      this.newItemValue = ''
    }
  }

  onItemDeleted(key: string) {
    let changed: {[key: string]: any} = {}
    changed = Object.assign(this.objectValue, changed)
    delete changed[key]
    this.$emit('input', changed)
    this.$forceUpdate()
  }
}
</script>
