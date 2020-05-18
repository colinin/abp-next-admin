<template>
  <el-form
    ref="formClient"
    label-width="100px"
    :model="clientPermission"
    label-position="top"
  >
    <el-form-item v-if="hasLoadPermission">
      <PermissionTree
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
  @Prop({ default: '' })
  private clientId!: string

  private clientPermission: PermissionDto
  private hasLoadPermission: boolean
  /** 角色权限已变更 */
  private clientPermissionChanged: boolean
  /** 变更角色权限数据 */
  private editClientPermissions: IPermission[]

  constructor() {
    super()
    this.hasLoadPermission = false
    this.clientPermissionChanged = false
    this.clientPermission = new PermissionDto()
    this.editClientPermissions = new Array<IPermission>()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    if (this.clientId) {
      PermissionService.getPermissionsByKey('C', this.clientId).then(permission => {
        this.clientPermission = permission
        this.hasLoadPermission = true
      })
    }
  }

  /** 角色权限树变更事件 */
  private onPermissionChanged(permissions: IPermission[]) {
    this.clientPermissionChanged = true
    this.editClientPermissions = permissions
  }

  private onSaveClientPemissions() {
    if (this.clientPermissionChanged) {
      const setClientPermissions = new UpdatePermissionsDto()
      setClientPermissions.permissions = this.editClientPermissions
      PermissionService.setPermissionsByKey('C', this.clientId, setClientPermissions).then(() => {
        this.onCancel()
      })
    }
  }

  private onCancel() {
    const permissionTree = this.$refs.PermissionTree as PermissionTree
    permissionTree.resetPermissions()
    this.$emit('closed')
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
