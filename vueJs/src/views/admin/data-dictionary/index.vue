<template>
  <div>
    <el-row>
      <el-col
        :span="6"
      >
        <data-dictionary-tree
          @onDataChecked="onDataChecked"
        />
      </el-col>
      <el-col :span="18">
        <el-card>
          <div
            slot="header"
          >
            <span>{{ $t('AppPlatform.Data:Items') }}</span>
          </div>
          <data-item-table
            :data-id="selectDataId"
          />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import DataItemTable from './components/DataItemTable.vue'
import DataDictionaryTree from './components/DataDictionaryTree.vue'

@Component({
  name: 'DataDictionary',
  components: {
    DataItemTable,
    DataDictionaryTree
  }
})
export default class extends Vue {
  private selectDataId = ''

  private onDataChecked(dataId: string) {
    this.selectDataId = dataId
  }
}
</script>

<style lang="scss" scoped>
</style>
