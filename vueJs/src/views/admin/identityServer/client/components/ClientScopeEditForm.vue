<template>
  <el-transfer
    :value="rightScopes"
    class="transfer-scope"
    :data="leftScopes"
    :titles="[$t('AbpIdentityServer.Assigned'), $t('AbpIdentityServer.Available')]"
    :props="{
      key: 'scope',
      label: 'scope'
    }"
    @change="onChanged"
  />
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { ClientScope } from '@/api/clients'

@Component({
  name: 'ClientScopeEditForm',
  model: {
    prop: 'allowedScopes',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => { return new Array<ClientScope>() } })
  private allowedScopes!: ClientScope[]

  @Prop({ default: () => { return new Array<string>() } })
  private scopes!: string[]

  get rightScopes() {
    return this.allowedScopes.map(scope => scope.scope)
  }

  get leftScopes() {
    return this.scopes.map(key => {
      const scope = new ClientScope()
      scope.scope = key
      return scope
    })
  }

  private onChanged(value: string[], operation: 'left' | 'right', movedKeys: string[]) {
    if (operation === 'left') {
      const changed = this.allowedScopes.filter(scope => !movedKeys.some(key => scope.scope === key))
      this.$emit('change', changed)
    } else {
      // 需要添加不存在的数据
      // v-model 绑定
      const added = movedKeys.map(key => {
        const scope = new ClientScope()
        scope.scope = key
        return scope
      })
      this.$emit('change', this.allowedScopes.concat(added))
    }
  }
}
</script>

<style scoped>
.transfer-scope ::v-deep .el-transfer-panel{
  width: 285px;
}
</style>
