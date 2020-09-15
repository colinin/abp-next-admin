<template>
  <div class="app-container">
    <el-form
      ref="frmFeature"
      :model="features"
      label-width="100px"
    >
      <el-tabs v-model="selectTab">
        <el-tab-pane
          v-for="(feature, fi) in features.features"
          :key="feature.name"
          :label="feature.displayName"
          :name="feature.name"
        >
          <el-form-item
            v-for="(featureChildren, fci) in feature.children"
            :key="featureChildren.name"
            :label="featureChildren.displayName"
            :prop="'features.' + fi + '.children.' + fci + '.value'"
            :rules="featureChildren.valueType.validator | inputRuleFilter(localizer)"
          >
            <el-popover
              :ref="'popover_' + fi + '_' + fci"
              trigger="hover"
              :title="featureChildren.displayName"
              :content="featureChildren.description"
            />
            <span
              slot="label"
              v-popover="'popover_' + fi + '_' + fci"
            >{{ featureChildren.displayName }}</span>
            <div
              v-if="featureChildren.valueType.name === 'ToggleStringValueType'"
            >
              <el-switch
                v-if="featureChildren.valueType.validator.name === 'BOOLEAN'"
                v-model="featureChildren.value"
              />
              <el-input
                v-else-if="featureChildren.valueType.validator.name === 'NUMERIC'"
                v-model.number="featureChildren.value"
                type="number"
              />
              <el-input
                v-else-if="featureChildren.valueType.validator.name === 'STRING'"
                v-model="featureChildren.value"
                type="text"
              />
            </div>
            <div
              v-else-if="featureChildren.valueType.name === 'SELECTION'"
            >
              <el-select
                v-model="featureChildren.value"
              >
                <el-option
                  v-for="valueItem in featureChildren.valueType.itemSource.items"
                  :key="valueItem.value"
                  :label="valueItem.displayText"
                  :value="valueItem.value"
                />
              </el-select>
            </div>
          </el-form-item>
        </el-tab-pane>
      </el-tabs>
      <el-button
        type="primary"
        class="confirm"
        @click="onSave"
      >
        {{ $t('AbpFeatureManagement.Submit') }}
      </el-button>
      <el-button
        class="cancel"
        @click="onClosed"
      >
        {{ $t('AbpFeatureManagement.Cancel') }}
      </el-button>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import FeatureManagementService, { ValueType, Feature, Features } from '@/api/feature-management'
import { ElForm } from 'element-ui/types/form'

/**
 * 适用于动态表单的功能节点列表
 */
class FeatureItems {
  features = new Array<FeatureItem>()
}

/**
 * 适用于动态表单的功能节点
 */
class FeatureItem {
  /** 功能名称 */
  name!: string
  /** 显示名称 */
  displayName?: string
  /** 当前值 */
  value!: any
  /** 说明 */
  description?: string
  /** 值类型 */
  valueType?: ValueType
  /** 深度 */
  depth?: number
  /** 子节点 */
  children!: FeatureItem[]

  /** 构造器 */
  constructor(
    name: string,
    value: any,
    displayName?: string,
    description?: string,
    valueType?: ValueType,
    depth?: number
  ) {
    this.name = name
    this.depth = depth
    this.valueType = valueType
    this.displayName = displayName
    this.description = description
    this.children = new Array<FeatureItem>()
    if (value !== null) {
      this.value = value.toLowerCase() === 'true' ? true // boolean类型
        : !isNaN(Number(value)) ? Number(value) // number类型
          : value
    }
  }

  /**
   * 创建子节点
   * @feature 子节点
   */
  public appendChildren(feature: FeatureItem) {
    this.children.push(feature)
  }

  /**
   * 获取子节点
   * @name 节点名称
   */
  public getChildren(name: string) {
    const childrenIndex = this.children.findIndex(feature => feature.name === name)
    if (childrenIndex >= 0) {
      return this.children[childrenIndex]
    }
    return undefined
  }
}

@Component({
  name: 'FeatureManagement',
  filters: {
    /**
     * 动态处理功能表单验证
     * @validator 后端传递的验证规则
     * @localizer 本地化method
     */
    inputRuleFilter(validator: any, localizer: any) {
      const featureRules: {[key: string]: any}[] = new Array<{[key: string]: any}>()
      if (validator.name === 'NUMERIC') {
        const ruleRang: {[key: string]: any} = {}
        ruleRang.pattern = RegExp('^(' + validator.minValue + '|[1-9]d?|' + validator.maxValue + ')$')
        // ruleRang.pattern = /^(1|[1-9]\d?|10)$/
        // ruleRang.min = validator.minValue
        // ruleRang.max = validator.maxValue
        ruleRang.trigger = 'blur'
        ruleRang.message = localizer('AbpFeatureManagement.ThisFieldMustBeBetween{0}And{1}', { 0: validator.minValue, 1: validator.maxValue })
        featureRules.push(ruleRang)
      } else if (validator.name === 'STRING') {
        if (validator.allowNull && validator.allowNull.toLowerCase() === 'true') {
          const ruleRequired: {[key: string]: any} = {}
          ruleRequired.required = true
          ruleRequired.trigger = 'blur'
          ruleRequired.messahe = localizer('AbpFeatureManagement.ThisFieldIsRequired')
          featureRules.push(ruleRequired)
        }
        const ruleString: {[key: string]: any} = {}
        ruleString.min = validator.minLength
        ruleString.max = validator.maxLength
        ruleString.trigger = 'blur'
        ruleString.message = localizer('AbpFeatureManagement.ThisFieldMustBeBetween{0}And{1}', { 0: validator.minValue, 1: validator.maxValue })
        featureRules.push(ruleString)
      }
      return featureRules
    }
  },
  methods: {
    /**
     * 本地化method
     */
    localizer(name: string, values?: any[]) {
      return this.$t(name, values)
    }
  }
})
export default class extends Vue {
  /**
   * 功能提供者名称
   */
  @Prop({ default: '' })
  private providerName!: string

  /**
   * 功能提供者标识
   */
  @Prop({ default: '' })
  private providerKey!: string

  /**
   * 默认选择tab选项卡
   * 如果不定义的话,动态组合的表单需要手动点击一次才会显示?
   */
  private selectTab = ''
  /**
   * 用于拼接动态表单的功能数据,需要把abp返回的数据做一次调整
   */
  private features = new FeatureItems()

  @Watch('providerKey', { immediate: true })
  onProviderKeyChanged() {
    if (this.providerKey) {
      this.handleGetFeatures()
    }
  }

  /**
   * 重置表单数据
   */
  public resetFeature() {
    const frmFeature = this.$refs.frmFeature as ElForm
    frmFeature.resetFields()
  }

  /**
   * 获取功能列表
   */
  private handleGetFeatures() {
    FeatureManagementService.getFeatures(this.providerName, this.providerKey).then(res => {
      this.features = new FeatureItems()
      res.features.forEach(feature => {
        const featureTrue = new FeatureItem(
          feature.name,
          feature.value,
          feature.displayName,
          feature.description,
          feature.valueType,
          feature.depth
        )
        if (feature.parentName) {
          const children = this.features.features.find(f => f.name === feature.parentName)
          if (children) {
            children.appendChildren(featureTrue)
          } else {
            this.features.features.push(featureTrue)
          }
        } else {
          this.features.features.push(featureTrue)
        }
      })
      // 需要手动选择一下?
      if (this.features.features.length > 0) {
        this.selectTab = this.features.features[0].name
      }
    })
  }

  /**
   * 保存变更
   */
  private onSave() {
    if (this.features.features.length > 0) {
      const frmFeature = this.$refs.frmFeature as any
      frmFeature.validate((valid: boolean) => {
        if (valid) {
          const updateFeatures = new Features()
          this.features.features.forEach(feature => {
            this.getChangedFeatures(feature, updateFeatures)
          })
          FeatureManagementService
            .updateFeatures(this.providerName, this.providerKey, updateFeatures)
            .then(() => {
              this.$message.success(this.$t('global.successful').toString())
              this.onClosed()
            })
        }
      })
    }
  }

  /**
   * 递归获取abp的功能接口格式数据
   */
  private getChangedFeatures(feature: FeatureItem, features: Features) {
    const updateFeature = new Feature(feature.name, feature.value)
    features.features.push(updateFeature)
    feature.children.forEach(children => {
      this.getChangedFeatures(children, features)
    })
  }

  /**
   * 关闭模态窗口
   */
  private onClosed() {
    this.$emit('closed')
    this.resetFeature()
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  width: 120px;
  position: absolute;
  right: 180px;
}
.cancel {
  width: 120px;
  position: absolute;
  right: 40px;
}
</style>
