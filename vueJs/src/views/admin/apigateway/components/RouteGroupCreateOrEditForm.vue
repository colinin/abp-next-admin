<template>
  <el-form
    ref="formRouteGroup"
    label-width="80px"
    :model="apiGateWayRouteGroup"
    :rules="apiGateWayRouteGroupRules"
  >
    <el-form-item
      prop="name"
      :label="$t('apiGateWay.groupName')"
    >
      <el-input
        v-model="apiGateWayRouteGroup.name"
        :placeholder="$t('apiGateWay.pleaseInputGroupName')"
      />
    </el-form-item>
    <el-form-item
      prop="appId"
      :label="$t('apiGateWay.appId')"
    >
      <el-input
        v-model="apiGateWayRouteGroup.appId"
        :disabled="isEditRouteGroup"
        :placeholder="$t('apiGateWay.pleaseInputAppId')"
      />
    </el-form-item>
    <el-form-item
      prop="appName"
      :label="$t('apiGateWay.appName')"
    >
      <el-input
        v-model="apiGateWayRouteGroup.appName"
        :placeholder="$t('apiGateWay.pleaseInputAppName')"
      />
    </el-form-item>
    <el-form-item
      prop="appIpAddress"
      :label="$t('apiGateWay.appIpAddress')"
    >
      <el-input
        v-model="apiGateWayRouteGroup.appIpAddress"
      />
    </el-form-item>
    <el-form-item
      prop="description"
      :label="$t('apiGateWay.description')"
    >
      <el-input
        v-model="apiGateWayRouteGroup.description"
      />
    </el-form-item>
    <el-form-item
      prop="isActive"
      label-width="100px"
      :label="$t('apiGateWay.isActive')"
    >
      <el-switch v-model="apiGateWayRouteGroup.isActive" />
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
        @click="onSubmitEdit('formRouteGroup')"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ApiGateWayService, { RouteGroupDto, RouteGroupUpdateDto, RouteGroupCreateDto } from '@/api/apigateway'

@Component({
  name: 'RouteGroupCreateOrEditForm'
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private appId!: string

  private apiGateWayRouteGroup: RouteGroupDto

  private apiGateWayRouteGroupRules = {
    appId: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.appId') }), trigger: 'blur' }
    ],
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.groupName') }), trigger: 'blur' }
    ],
    appName: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.appName') }), trigger: 'blur' }
    ],
    appIpAddress: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.appIpAddress') }), trigger: 'blur' }
    ]
  }

  get isEditRouteGroup() {
    return this.appId !== ''
  }

  constructor() {
    super()
    this.apiGateWayRouteGroup = new RouteGroupDto()
  }

  @Watch('appId', { immediate: true })
  private handleAppIdChange(val: any) {
    if (val) {
      ApiGateWayService.getRouteGroupByAppId(val).then(router => {
        this.apiGateWayRouteGroup = router
      })
    }
  }

  private onSubmitEdit(formName: string) {
    const routerEditForm = this.$refs[formName] as any
    routerEditForm.validate(async(valid: boolean) => {
      if (valid) {
        if (this.appId) {
          const updateRouterDto = new RouteGroupUpdateDto()
          updateRouterDto.name = this.apiGateWayRouteGroup.name
          updateRouterDto.appId = this.apiGateWayRouteGroup.appId
          updateRouterDto.appName = this.apiGateWayRouteGroup.appName
          updateRouterDto.isActive = this.apiGateWayRouteGroup.isActive
          updateRouterDto.appIpAddress = this.apiGateWayRouteGroup.appIpAddress
          updateRouterDto.description = this.apiGateWayRouteGroup.description
          this.apiGateWayRouteGroup = await ApiGateWayService.updateRouteGroup(updateRouterDto)
        } else {
          const createRouterDto = new RouteGroupCreateDto()
          createRouterDto.name = this.apiGateWayRouteGroup.name
          createRouterDto.appId = this.apiGateWayRouteGroup.appId
          createRouterDto.appName = this.apiGateWayRouteGroup.appName
          createRouterDto.isActive = this.apiGateWayRouteGroup.isActive
          createRouterDto.appIpAddress = this.apiGateWayRouteGroup.appIpAddress
          createRouterDto.description = this.apiGateWayRouteGroup.description
          this.apiGateWayRouteGroup = await ApiGateWayService.createRouteGroup(createRouterDto)
        }
        this.$message('successful')
        routerEditForm.resetFields()
        this.$emit('closed', true)
      }
    })
  }

  private onCancel() {
    this.apiGateWayRouteGroup = new RouteGroupDto()
    const routerEditForm = this.$refs.formRouteGroup as any
    routerEditForm.resetFields()
    this.$emit('closed', false)
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
