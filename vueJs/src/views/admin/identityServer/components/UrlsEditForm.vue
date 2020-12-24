<template>
  <div>
    <h3 style="margin-bottom: 30px;">
      {{ title }}
    </h3>
    <el-form
      ref="formEditUrl"
      :model="newUrl"
    >
      <el-form-item
        prop="url"
        label="Url"
      >
        <el-autocomplete
          v-if="fetchUrls.length>0"
          v-model="newUrl.url"
          :fetch-suggestions="querySearch"
          class="inline-input"
        />
        <el-input
          v-else
          v-model="newUrl.url"
        />
      </el-form-item>

      <el-form-item>
        <el-button
          type="success"
          class="add-button"
          @click="onSave"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AbpIdentityServer.AddNew') }}
        </el-button>
      </el-form-item>
    </el-form>

    <el-table
      :row-key="urlKey"
      :data="urls"
      :show-header="false"
      highlight-current-row
      style="width: 100%; margin-bottom: 30px;"
    >
      <el-table-column
        label="Url"
        :prop="urlKey"
        sortable
        align="left"
      >
        <template slot-scope="{row}">
          <span>{{ row[urlKey] }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Actions')"
        align="right"
        min-width="80px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="onDelete(row[urlKey])"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { Form } from 'element-ui'
import { Component, Prop, Vue } from 'vue-property-decorator'

class Url {
  url = ''
}

@Component({
  name: 'UrlsEditForm',
  model: {
    prop: 'urls',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => { return [] } })
  private urls!: [{[key: string]: string}]

  @Prop({ default: '' })
  private urlKey!: string

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: () => { return new Array<string>() } })
  private fetchUrls!: string[]

  private newUrl = new Url()

  querySearch(queryString: string, cb: any) {
    const results = this.fetchUrls.filter(url => url.indexOf(queryString) === 0)
    cb(results)
  }

  private onSave() {
    const formEditUrl = this.$refs.formEditUrl as Form
    formEditUrl.validate(valid => {
      if (valid) {
        const url: {[key: string]: string} = {}
        url[this.urlKey] = this.newUrl.url
        this.$emit('change', this.urls.concat(url))
        formEditUrl.resetFields()
      }
    })
  }

  private onDelete(key: any) {
    this.$emit('change', this.urls.filter(url => url[this.urlKey] !== key))
  }
}
</script>

<style scoped>
.add-button {
  width: 150px;
  float: right;
}
</style>
