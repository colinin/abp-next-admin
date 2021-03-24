<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="title"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="formMenu"
      :model="menu"
      label-width="120px"
    >
      <el-tabs v-model="activedTab">
        <el-tab-pane
          name="basic"
          :label="$t(('AppPlatform.DisplayName:Basic'))"
        >
          <el-form-item
            v-if="!isEdit"
            :label="$t('AppPlatform.DisplayName:Layout')"
            :rules="{
              required: true
            }"
          >
            <el-select
              v-model="layoutId"
              style="width: 100%;"
              class="filter-item"
              clearable
              :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:Layout')})"
              @change="onLayoutChanged"
            >
              <el-option
                v-for="layout in layouts"
                :key="layout.name"
                :label="layout.displayName"
                :value="layout.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="name"
            :label="$t(('AppPlatform.DisplayName:IsPublic'))"
          >
            <el-switch
              v-model="menu.isPublic"
            />
          </el-form-item>
          <el-form-item
            prop="name"
            :label="$t(('AppPlatform.DisplayName:Name'))"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:Name')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="menu.name"
            />
          </el-form-item>
          <el-form-item
            prop="displayName"
            :label="$t(('AppPlatform.DisplayName:DisplayName'))"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:DisplayName')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="menu.displayName"
            />
          </el-form-item>
          <el-form-item
            prop="path"
            :label="$t(('AppPlatform.DisplayName:Path'))"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:Path')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="menu.path"
            />
          </el-form-item>
          <el-form-item
            prop="component"
            :label="$t(('AppPlatform.DisplayName:Component'))"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:Component')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="menu.component"
            />
          </el-form-item>
          <el-form-item
            prop="redirect"
            :label="$t(('AppPlatform.DisplayName:Redirect'))"
          >
            <el-input
              v-model="menu.redirect"
            />
          </el-form-item>
          <el-form-item
            prop="description"
            :label="$t(('AppPlatform.DisplayName:Description'))"
          >
            <el-input
              v-model="menu.description"
              type="textarea"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="meta"
          :label="$t(('AppPlatform.DisplayName:Meta'))"
        >
          <el-form-item
            v-for="(dataItem) in dataItems"
            :key="dataItem.id"
            :label="dataItem.displayName"
            :prop="'meta.' + dataItem.name"
            :rules="{
              required: !dataItem.allowBeNull,
              message: $t('pleaseInputBy', {key: dataItem.displayName}),
              trigger: 'blur'
            }"
          >
            <!-- <el-popover
              :ref="dataItem.name"
              trigger="hover"
              :title="dataItem.displayName"
              :content="dataItem.description || dataItem.displayName"
            />
            <span
              slot="label"
              v-popover="dataItem.name"
            >{{ dataItem.displayName }}</span> -->
            <menu-meta-input
              v-model="menu.meta[dataItem.name]"
              :prop-name="'meta.' + dataItem.name"
              :data-item="dataItem"
            />
          </el-form-item>
        </el-tab-pane>
      </el-tabs>

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed(false)"
        >
          {{ $t('AbpUi.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpUi.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

import { Form } from 'element-ui'
import MenuService, {
  Menu,
  MenuCreate,
  MenuUpdate,
  MenuCreateOrUpdate
} from '@/api/menu'
import DataService, { Data, DataItem } from '@/api/data-dictionary'
import LayoutService, { Layout } from '@/api/layout'

import MenuMetaInput from './MenuMetaInput.vue'

@Component({
  name: 'CreateOrUpdateMenuDialog',
  components: {
    MenuMetaInput
  }
})
export default class CreateOrUpdateMenuDialog extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private menuId!: string

  @Prop({ default: '' })
  private parentId?: string

  get isEdit() {
    if (this.menuId) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.l('AppPlatform.Menu:EditByName', { 0: this.menu.displayName })
    }
    return this.l('AppPlatform.Menu:AddNew')
  }

  get dataItems() {
    const items = this.bindData.items.sort((pre: DataItem, next: DataItem) => {
      return pre.valueType < next.valueType ? -1 : 0
    })
    return items
  }

  private activedTab = 'basic'
  private menu = new Menu()
  private bindData = new Data()
  private layouts = new Array<Layout>()
  private layoutId = ''

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetMenu()
  }

  mounted() {
    this.handleGetLayouts()
  }

  private handleGetLayouts() {
    if (!this.isEdit) {
      LayoutService
        .getAllList()
        .then(res => {
          this.layouts = res.items
        })
    }
  }

  private handleGetMenu() {
    if (this.showDialog && this.menuId) {
      MenuService
        .get(this.menuId)
        .then(res => {
          this.menu = res
          this.layoutId = res.layoutId
          // 获取数据字典约束
          this.onLayoutChanged()
        })
    } else {
      this.menu = new Menu()
    }
  }

  private handleGetBindData() {
    DataService
      .get(this.menu.layoutId)
  }

  private onLayoutChanged() {
    const layout = this.layouts.find(x => x.id === this.layoutId)
    if (layout) {
      if (!this.isEdit) {
        this.menu.meta = {}
        if (!this.parentId) {
          // 对于根菜单,自动设置组件路径为布局路径
          this.menu.component = layout.path
        }
      }
      DataService
        .get(layout.dataId)
        .then(res => {
          this.bindData = res
        })
    }
  }

  private onSave() {
    const formMenu = this.$refs.formMenu as Form
    formMenu
      .validate(valid => {
        if (valid) {
          if (this.isEdit) {
            const updateMenu = new MenuUpdate()
            this.updateMenuByInput(updateMenu)
            MenuService
              .update(this.menuId, updateMenu)
              .then(res => {
                this.menu = res
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          } else {
            if (!this.layoutId) {
              this.$message.warning(this.l('pleaseSelectBy', { key: this.l('AppPlatform.DisplayName:Layout') }))
              return
            }
            const createMenu = new MenuCreate()
            this.updateMenuByInput(createMenu)
            createMenu.layoutId = this.layoutId
            createMenu.parentId = this.parentId
            MenuService
              .create(createMenu)
              .then(res => {
                this.menu = res
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          }
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formMenu = this.$refs.formMenu as Form
    formMenu.resetFields()
    this.activedTab = 'basic'
    this.$emit('closed', changed)
  }

  private updateMenuByInput(update: MenuCreateOrUpdate) {
    update.name = this.menu.name
    update.path = this.menu.path
    update.component = this.menu.component
    update.displayName = this.menu.displayName
    update.description = this.menu.description
    update.redirect = this.menu.redirect
    update.isPublic = this.menu.isPublic
    update.meta = this.menu.meta
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
}
</style>
