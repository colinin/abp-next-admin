<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.clientPermission')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <el-form
      ref="formClient"
      label-width="100px"
      :model="clientPermission"
      label-position="top"
    >
      <el-form-item v-if="hasLoadPermission">
        <permission-tree
          ref="PermissionTree"
          :expanded="false"
          :readonly="!checkPermission(['IdentityServer.Clients.ManagePermissions'])"
          :permission="clientPermission"
          @onPermissionChanged="onPermissionChanged"
        />
      </el-form-item>
      <el-form-item>
        <el-button
          class="cancel"
          style="width:100px"
          @click="onCancel"
        >
          {{ $t('table.cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          style="width:100px"
          :disabled="!checkPermission(['IdentityServer.Clients.ManagePermissions'])"
          @click="onSaveClientPemissions"
        >
          {{ $t('table.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { IPermission } from '@/api/types'
import { checkPermission } from '@/utils/permission'
import PermissionTree from '@/components/PermissionTree/index.vue'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import PermissionService, { PermissionDto, UpdatePermissionsDto } from '@/api/permission'

@Component({
  name: 'ClientPermissionEditForm',
  components: {
    PermissionTree
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  /** 客户端标识 */
  @Prop({ default: '' })
  private clientId!: string

  /** 客户端权限 */
  private clientPermission: PermissionDto
  /** 是否已加载权限 */
  private hasLoadPermission: boolean
  /** 客户端权限已变更 */
  private clientPermissionChanged: boolean
  /** 变更客户端权限数据 */
  private editClientPermissions: IPermission[]

  constructor() {
    super()
    this.hasLoadPermission = false
    this.clientPermissionChanged = false
    this.clientPermission = new PermissionDto()
    this.editClientPermissions = new Array<IPermission>()
  }

  /** 监听客户端标识变更事件,刷新客户端权限数据 */
  @Watch('clientId')
  private onClientIdChanged() {
    this.handledGetClientPermissions()
  }

  mounted() {
    this.handledGetClientPermissions()
  }

  private handledGetClientPermissions() {
    if (this.showDialog && this.clientId) {
      PermissionService.getPermissionsByKey('C', this.clientId).then(permission => {
        this.clientPermission = permission
        this.hasLoadPermission = true
      })
    }
  }

  /** 客户端权限树变更事件
   * @param permissions 变更权限数据
   */
  private onPermissionChanged(permissions: IPermission[]) {
    this.clientPermissionChanged = true
    this.editClientPermissions = permissions
  }

  /** 保存客户端权限 */
  private onSaveClientPemissions() {
    if (this.clientPermissionChanged) {
      const setClientPermissions = new UpdatePermissionsDto()
      setClientPermissions.permissions = this.editClientPermissions
      PermissionService.setPermissionsByKey('C', this.clientId, setClientPermissions).then(() => {
        this.onFormClosed()
      })
    }
  }

  private onFormClosed() {
    this.$emit('closed')
  }

  /** 取消操作 */
  private onCancel() {
    this.onFormClosed()
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
