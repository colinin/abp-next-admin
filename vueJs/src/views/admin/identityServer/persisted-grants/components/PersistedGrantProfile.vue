<template>
  <el-dialog
    width="800px"
    :title="$t('AbpIdentityServer.DisplayName:PersistedGrants')"
    :visible="showDialog"
    custom-class="modal-form"
    @close="onFormClosed"
  >
    <div class="app-container">
      <el-form
        ref="formPersistedGrant"
        label-width="130px"
      >
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:Key')"
        >
          <el-input
            :value="persistedGrant.key"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:Type')"
        >
          <el-input
            :value="persistedGrant.type"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:SubjectId')"
        >
          <el-input
            :value="persistedGrant.subjectId"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:SessionId')"
        >
          <el-input
            :value="persistedGrant.sessionId"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Description')"
        >
          <el-input
            :value="persistedGrant.description"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.CreationTime')"
        >
          <el-input
            :value="persistedGrant.creationTime | dateTimeFilter"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:ConsumedTime')"
        >
          <el-input
            :value="persistedGrant.consumedTime | dateTimeFilter"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Expiration')"
        >
          <el-input
            :value="persistedGrant.expiration | dateTimeFilter"
            readonly
          />
        </el-form-item>
        <el-form-item
          :label="$t('AbpIdentityServer.Grants:Data')"
        >
          <json-editor
            :value="formatData(persistedGrant.data)"
          />
        </el-form-item>
      </el-form>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import { PersistedGrant } from '@/api/grants'
import { Component, Prop, Watch, Mixins } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import JsonEditor from '@/components/JsonEditor/index.vue'

@Component({
  name: 'PersistedGrantProfile',
  components: {
    JsonEditor
  },
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
      }
      return ''
    }
  }
})
export default class extends Mixins(HttpProxyMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private id!: string

  private persistedGrant = new PersistedGrant()

  get formatData() {
    return (data: string) => {
      if (data) {
        return JSON.parse(data)
      }
      return {}
    }
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetPersistedGrant()
  }

  private handleGetPersistedGrant() {
    if (this.id && this.showDialog) {
      this.request<PersistedGrant>({
        service: 'IdentityServer',
        controller: 'PersistedGrant',
        action: 'GetAsync',
        params: {
          id: this.id
        }
      }).then(res => {
        this.persistedGrant = res
      })
    }
  }

  private onFormClosed() {
    this.resetFields()
    this.$emit('closed')
  }

  private onCancel() {
    this.onFormClosed()
  }

  public resetFields() {
    const formPersistedGrant = this.$refs.formPersistedGrant as any
    formPersistedGrant.resetFields()
  }
}
</script>
