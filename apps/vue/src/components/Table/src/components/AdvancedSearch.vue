<template>
  <BasicModal
    @register="registerModal"
    :width="800"
    :title="t('component.table.advancedSearch.title')"
    :can-fullscreen="false"
    @ok="handleSubmit"
  >
    <Card :title="t('component.table.advancedSearch.conditions')">
      <template #extra>
        <Button @click="resetFields" danger>{{ t('component.table.advancedSearch.clearCondition') }}</Button>
        <Button @click="handleAddField" type="primary" style="margin-left: 20px;">{{ t('component.table.advancedSearch.addCondition') }}</Button>
      </template>
      <Table
        size="small"
        rowKey="field"
        :columns="columns"
        :data-source="formMdel.paramters"
        :pagination="false"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.dataIndex==='field'">
            <Select
              style="width: 100%;"
              show-search
              v-model:value="record.field"
              :options="getAvailableOptions"
              :filter-option="filterOption"
              @change="(field) => handleFieldChange(field, record)"
            />
          </template>
          <template v-else-if="column.dataIndex==='comparison'">
            <Select
              style="width: 100%;"
              v-model:value="record.comparison"
              :options="getAvailableComparisonOptions(record)"
            />
          </template>
          <template v-else-if="column.dataIndex==='value'">
            <Input v-if="record.javaScriptType==='string'" v-model:value="record.value" />
            <Select
              v-else-if="record.javaScriptType==='number' && record.options && record.options.length > 0"
              style="width: 100%;"
              v-model:value="record.value"
              :options="record.options"
              :field-names="{
                label: 'key',
                value: 'value'
              }"
            />
            <InputNumber v-else-if="record.javaScriptType==='number'" style="width: 100%;" v-model:value="record.value" />
            <Switch v-else-if="record.javaScriptType==='boolean'" v-model:checked="record.value" />
            <DatePicker
              v-else-if="record.javaScriptType==='Date'"
              style="width: 100%;"
              v-model:value="record.value"
              format="YYYY-MM-DD 00:00:00"
              value-format="YYYY-MM-DDT00:00:00"
            />
            <CodeEditorX
              v-else-if="['array', 'object'].includes(record.javaScriptType)"
              style="width: 100%; height: 300px"
              :mode="MODE.JSON"
              v-model="record.value"
            />
          </template>
          <template v-else-if="column.dataIndex==='logic'">
            <Select
              style="width: 100%;"
              v-model:value="record.logic"
              :options="logicOptions"
            />
          </template>
          <template v-else-if="column.dataIndex==='actions'">
            <Popconfirm
              v-if="formMdel.paramters.length"
              :title="t('table.sureToDelete')"
              @confirm="handleDelField(record)"
            >
              <DeleteTwoTone two-tone-color="#FF4500" />
            </Popconfirm>
          </template>
        </template>
      </Table>
    </Card>
  </BasicModal>
</template>

<script lang="ts" setup>
  import type { ColumnsType } from 'ant-design-vue/lib/table/interface';
  import { computed, ref, reactive, unref, onMounted } from 'vue';
  import { DeleteTwoTone } from '@ant-design/icons-vue';
  import {
    Button,
    Card,
    DatePicker,
    Input,
    InputNumber,
    Popconfirm,
    Select,
    Switch,
    Table
  } from 'ant-design-vue';
  import { CodeEditorX, MODE } from '/@/components/CodeEditor';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { DefineParamter, DynamicLogic, DynamicComparison, DynamicQueryable, DynamicParamter } from '../types/advancedSearch';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import { isFunction } from '/@/utils/is';
  import { get } from 'lodash-es';

  const emits = defineEmits(['register', 'search', 'change']);
  const props = defineProps({
    useAdvancedSearch: {
      type: Boolean,
      default: false,
    },
    allowDuplicateFieldSearch: {
      type: Boolean,
      default: true,
    },
    defineFieldApi: {
      type: Function as PropType<() => Promise<any>>
    },
    defineFieldReplace: {
      type: Function as PropType<(response: any) => DefineParamter[]>,
    },
    listField: {
      type: String,
    }
  });
  const { t } = useI18n();
  const loadingRef = ref(false);
  const formMdel = reactive<DynamicQueryable>({
    paramters: [],
  });

  const columns = reactive<ColumnsType>([
    {
      dataIndex: 'field',
      key: 'field',
      title: t('component.table.advancedSearch.field'),
      width: 240,
    },
    {
      dataIndex: 'comparison',
      key: 'comparison',
      title: t('component.table.advancedSearch.comparison'),
      width: 120,
    },
    {
      dataIndex: 'value',
      key: 'value',
      title: t('component.table.advancedSearch.value'),
    },
    {
      dataIndex: 'logic',
      key: 'logic',
      title: t('component.table.advancedSearch.logic'),
    },
    {
      title: t('table.action'),
      dataIndex: 'actions',
      align: 'center',
      width: 120,
    },
  ]);

  const defineParamsRef = ref<DefineParamter[]>([]);

  const logicOptions = reactive([
    {
      label: t('component.table.advancedSearch.and'),
      value: DynamicLogic.And,
    },
    {
      label: t('component.table.advancedSearch.or'),
      value: DynamicLogic.Or,
    },
  ]);

  const comparisonOptions = reactive([
    {
      label: t('component.table.advancedSearch.equal'),
      value: DynamicComparison.Equal,
    },
    {
      label: t('component.table.advancedSearch.notEqual'),
      value: DynamicComparison.NotEqual,
    },
    {
      label: t('component.table.advancedSearch.lessThan'),
      value: DynamicComparison.LessThan,
    },
    {
      label: t('component.table.advancedSearch.lessThanOrEqual'),
      value: DynamicComparison.LessThanOrEqual,
    },
    {
      label: t('component.table.advancedSearch.greaterThan'),
      value: DynamicComparison.GreaterThan,
    },
    {
      label: t('component.table.advancedSearch.greaterThanOrEqual'),
      value: DynamicComparison.GreaterThanOrEqual,
    },
    {
      label: t('component.table.advancedSearch.startsWith'),
      value: DynamicComparison.StartsWith,
    },
    {
      label: t('component.table.advancedSearch.notStartsWith'),
      value: DynamicComparison.NotStartsWith,
    },
    {
      label: t('component.table.advancedSearch.endsWith'),
      value: DynamicComparison.EndsWith,
    },
    {
      label: t('component.table.advancedSearch.notEndsWith'),
      value: DynamicComparison.NotEndsWith,
    },
    {
      label: t('component.table.advancedSearch.contains'),
      value: DynamicComparison.Contains,
    },
    {
      label: t('component.table.advancedSearch.notContains'),
      value: DynamicComparison.NotContains,
    },
    {
      label: t('component.table.advancedSearch.null'),
      value: DynamicComparison.Null,
    },
    {
      label: t('component.table.advancedSearch.notNull'),
      value: DynamicComparison.NotNull,
    },
  ]);

  const getAvailableParams = computed(() => {
    const { allowDuplicateFieldSearch } = props;
    if (allowDuplicateFieldSearch) {
      // 允许字段出现多次, 直接返回原数据
      return defineParamsRef.value;
    }
    // 每个字段只允许出现一次，已选择字段不再出现在可选列表
    const defineParams = unref(defineParamsRef);
    if (!defineParams.length) return[];
    return defineParams.filter(dp => !formMdel.paramters.some(fp => fp.field === dp.name));
  });

  const getAvailableOptions = computed(() => {
    const availableParams = unref(getAvailableParams);
    if (!availableParams.length) return[];
    return availableParams
      .map((item) => {
        return {
          label: item.description,
          value: item.name,
          children: [],
        }
      });
  });

  const getAvailableComparisonOptions = computed(() => {
    return (paramter: DynamicParamter) => {
      const defineParams = unref(defineParamsRef);
      const defineParam = defineParams.find(p => p.name === paramter.field);
      if (!defineParam) {
        return comparisonOptions;
      }
      const availableComparator = defineParam.availableComparator ?? [];
      if (availableComparator.length === 0) {
        return comparisonOptions;
      }
      // 过滤可用比较符
      return comparisonOptions
        .filter(c => availableComparator.includes(c.value));
    }
  });

  const filterOption = (input: string, option: any) => {
    if (isNullOrWhiteSpace(option.label) && isNullOrWhiteSpace(option.value)) return false;
    return option.label.toLowerCase().indexOf(input.toLowerCase()) >= 0 ||
      option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0 ;
  };

  onMounted(fetch);

  const [registerModal, { closeModal }] = useModalInner();

  function fetch() {
    const { useAdvancedSearch, defineFieldApi, defineFieldReplace, listField } = props;
    if (!useAdvancedSearch) return;
    if (!defineFieldApi || !isFunction(defineFieldApi)) return;
    setLoading(true);
    defineFieldApi().then((res) => {
      let resultItems: DefineParamter[] = [];
      if (defineFieldReplace && isFunction(defineFieldReplace)) {
        resultItems = defineFieldReplace(res);
      } else {
        const isArrayResult = Array.isArray(res);
        resultItems = isArrayResult ? res : get(res, listField || 'items');
      }
      defineParamsRef.value = resultItems;
    }).finally(() => {
      setLoading(false);
    });
  }

  function handleAddField() {
    const availableParams = unref(getAvailableParams);
    if (availableParams.length > 0) {
      const bindParamter = availableParams[availableParams.length - 1];
      const newParamter: DynamicParamter = {
        field: bindParamter.name,
        logic: DynamicLogic.And,
        comparison: DynamicComparison.Equal,
        javaScriptType: bindParamter.javaScriptType,
        value: undefined,
      };
      if (bindParamter.javaScriptType === 'boolean') {
        newParamter.value = false;
      }
      formMdel.paramters.push(newParamter);
    }
  }

  function handleDelField(paramter) {
    const index = formMdel.paramters.findIndex(p => p.field === paramter.field);
    formMdel.paramters.splice(index, 1);
    emits('change', getSearchInput());
  }

  function handleFieldChange(field, record) {
    const defineParams = unref(defineParamsRef);
    const defineParam = defineParams.find(dp => dp.name === field);
    if (defineParam) {
      record.field = defineParam.name;
      record.javaScriptType = defineParam.javaScriptType;
      record.value = undefined;
      record.options = defineParam.options ?? [];
      if (defineParam.javaScriptType === 'boolean') {
        record.value = false;
      }
      emits('change', getSearchInput());
    }
  }

  function handleSubmit() {
    emits('search', getSearchInput());
    closeModal();
  }

  function resetFields() {
    formMdel.paramters = [];
    emits('change', getSearchInput());
  }

  function getSearchInput() {
    const searchInput = {
      // 过滤未定义值
      paramters: formMdel.paramters.filter(p => p.value !== undefined)
    };
    return searchInput;
  }

  function setLoading(loading: boolean) {
    loadingRef.value = loading;
  }

  defineExpose({ resetFields });
</script>

<style lang="less" scoped>

</style>