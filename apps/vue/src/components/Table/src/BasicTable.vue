<template>
  <div ref="wrapRef" :class="getWrapperClass">
    <BasicForm
      ref="formRef"
      submitOnReset
      v-bind="getFormProps"
      v-if="getBindValues.useSearchForm"
      :tableAction="tableAction"
      @register="registerForm"
      @submit="handleSearchInfoChange"
      @reset="handleSearchInfoReset"
      @advanced-change="redoHeight"
    >
      <template #[replaceFormSlotKey(item)]="data" v-for="item in getFormSlotKeys">
        <slot :name="item" v-bind="data || {}"></slot>
      </template>
      <template #advanceBefore>
        <Button
          v-if="getAdvancedSearchProps?.useAdvancedSearch"
          type="link"
          size="small"
          @click="handleAdvanceSearch"
        >
          {{ t('component.table.advancedSearch.title') }}
        </Button>
      </template>
    </BasicForm>

    <Table
      ref="tableElRef"
      v-bind="getBindValues"
      :rowClassName="getRowClassName"
      v-show="getEmptyDataIsShowTable"
      @change="handleTableChange"
      @resizeColumn="handleResizeColumn"
      @expand="handleTableExpand"
    >
      <template #[item]="data" v-for="item in Object.keys($slots)" :key="item">
        <slot :name="item" v-bind="data || {}"></slot>
      </template>
      <template #headerCell="{ column }">
        <slot name="headerCell" v-bind="{ column }">
          <HeaderCell :column="column" />
        </slot>
      </template>
       <!-- 增加对antdv3.x兼容 -->
      <template #bodyCell="data">
        <slot name="bodyCell" v-bind="data || {}"></slot>
      </template>
      <!--      <template #[`header-${column.dataIndex}`] v-for="(column, index) in columns" :key="index">-->
      <!--        <HeaderCell :column="column" />-->
      <!--      </template>-->
    </Table>
    <AdvancedSearch
      ref="advancedSearchRef"
      @register="registerAdSearchModal"
      v-bind="getAdvancedSearchProps"
      @change="handleAdvanceSearchChange"
      @search="handleAdvanceSearchInfoChange"
    />
  </div>
</template>
<script lang="ts">
  import type {
    BasicTableProps,
    TableActionType,
    SizeType,
    ColumnChangeParam,
    InnerMethods,
  } from './types/table';

  import { defineComponent, ref, reactive, computed, unref, toRaw, inject, watchEffect, nextTick } from 'vue';
  import { Button, Table } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { useModal } from '/@/components/Modal/index';
  import { PageWrapperFixedHeightKey } from '/@/components/Page';
  import HeaderCell from './components/HeaderCell.vue';
  import AdvancedSearch from './components/AdvancedSearch.vue';
  import { InnerHandlers } from './types/table';

  import { usePagination } from './hooks/usePagination';
  import { useColumns } from './hooks/useColumns';
  import { useDataSource } from './hooks/useDataSource';
  import { useLoading } from './hooks/useLoading';
  import { useRowSelection } from './hooks/useRowSelection';
  import { useTableAlert } from './hooks/useTableAlert';
  import { useTableScroll } from './hooks/useTableScroll';
  import { useTableScrollTo } from './hooks/useScrollTo';
  import { useCustomRow } from './hooks/useCustomRow';
  import { useTableStyle } from './hooks/useTableStyle';
  import { useTableHeader } from './hooks/useTableHeader';
  import { useTableExpand } from './hooks/useTableExpand';
  import { createTableContext } from './hooks/useTableContext';
  import { useTableFooter } from './hooks/useTableFooter';
  import { useTableForm } from './hooks/useTableForm';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';

  import { omit } from 'lodash-es';
  import { basicProps } from './props';
  import { isFunction, isNumber } from '/@/utils/is';
  import { warn } from '/@/utils/log';

  export default defineComponent({
    name: 'BasicTable',
    components: {
      Table,
      BasicForm,
      Button,
      HeaderCell,
      AdvancedSearch,
    },
    props: basicProps,
    emits: [
      'fetch-success',
      'fetch-error',
      'selection-change',
      'register',
      'row-click',
      'row-dbClick',
      'row-contextmenu',
      'row-mouseenter',
      'row-mouseleave',
      'edit-end',
      'edit-cancel',
      'edit-row-end',
      'edit-change',
      'expanded-rows-change',
      'change',
      'columns-change',
    ],
    setup(props, { attrs, emit, slots, expose }) {
      const { t } = useI18n();
      const tableElRef = ref(null);
      const tableData = ref<Recordable[]>([]);

      const wrapRef = ref(null);
      const formRef = ref(null);
      const advancedSearchRef = ref<any>(null);
      const innerPropsRef = ref<Partial<BasicTableProps>>();

      const { prefixCls } = useDesign('basic-table');
      const [registerForm, formActions] = useForm();
      const [registerAdSearchModal, { openModal: openAdSearchModal }] = useModal();

      const getProps = computed(() => {
        return { ...props, ...unref(innerPropsRef) } as BasicTableProps;
      });

      const isFixedHeightPage = inject(PageWrapperFixedHeightKey, false);
      watchEffect(() => {
        unref(isFixedHeightPage) &&
          props.canResize &&
          warn(
            "'canResize' of BasicTable may not work in PageWrapper with 'fixedHeight' (especially in hot updates)",
          );
      });

      const { getLoading, setLoading } = useLoading(getProps);
      const {
        getPaginationInfo,
        getPagination,
        setPagination,
        setShowPagination,
        getShowPagination,
      } = usePagination(getProps);

      const {
        getRowSelection,
        getRowSelectionRef,
        getSelectRows,
        setSelectedRows,
        clearSelectedRowKeys,
        getSelectRowKeys,
        deleteSelectRowByKey,
        setSelectedRowKeys,
      } = useRowSelection(getProps, tableData, emit);

      const {
        handleTableChange: onTableChange,
        getDataSourceRef,
        getDataSource,
        getRawDataSource,
        setTableData,
        updateTableDataRecord,
        deleteTableDataRecord,
        insertTableDataRecord,
        findTableDataRecord,
        fetch,
        getRowKey,
        reload,
        getAutoCreateKey,
        updateTableData,
      } = useDataSource(
        getProps,
        {
          tableData,
          getPaginationInfo,
          setLoading,
          setPagination,
          getFieldsValue: formActions.getFieldsValue,
          clearSelectedRowKeys,
        },
        emit,
      );

      function handleTableChange(pagination: any, filters: any, sorter: any, extra: any) {
        onTableChange(pagination, filters, sorter);
        emit('change', pagination, filters, sorter);
        // 解决通过useTable注册onChange时不起作用的问题
        const { onChange } = unref(getProps);
        onChange && isFunction(onChange) && onChange(pagination, filters, sorter, extra);
      }

      const {
        getViewColumns,
        getColumns,
        setCacheColumnsByField,
        setColumns,
        getColumnsRef,
        getCacheColumns,
      } = useColumns(getProps, getPaginationInfo);

      const { getScrollRef, redoHeight } = useTableScroll(
        getProps,
        tableElRef,
        getColumnsRef,
        getRowSelectionRef,
        getDataSourceRef,
        wrapRef,
        formRef,
      );

      const { scrollTo } = useTableScrollTo(tableElRef, getDataSourceRef);

      const { customRow } = useCustomRow(getProps, {
        setSelectedRowKeys,
        getSelectRowKeys,
        clearSelectedRowKeys,
        getAutoCreateKey,
        emit,
      });

      const { getRowClassName } = useTableStyle(getProps, prefixCls);

      const { getExpandOption, expandAll, expandRows, collapseRows, collapseAll, handleTableExpand } = useTableExpand(
        getProps,
        tableData,
        emit,
      );

      const handlers: InnerHandlers = {
        onColumnsChange: (data: ColumnChangeParam[]) => {
          emit('columns-change', data);
          // support useTable
          unref(getProps).onColumnsChange?.(data);
        },
        deSelect: () => {
          clearSelectedRowKeys();
        },
      };

      const methods: InnerMethods = {
        clearSelectedRowKeys,
        getSelectRowKeys,
      };

      const { getAlertEnabled, getAlertMessage } = useTableAlert(getProps, getRowSelectionRef);

      const { getHeaderProps } = useTableHeader(getProps, slots, handlers, methods, getAlertEnabled, getAlertMessage);

      const { getFooterProps } = useTableFooter(
        getProps,
        getScrollRef,
        tableElRef,
        getDataSourceRef,
      );

      const {
        getFormProps,
        getAdvancedSearchProps,
        replaceFormSlotKey,
        getFormSlotKeys,
        handleSearchInfoChange,
        handleAdvanceSearchChange,
        handleAdvanceSearchInfoChange
      } =
        useTableForm(getProps, slots, fetch, getLoading, formActions.setFieldsValue);

      const getBindValues = computed(() => {
        const dataSource = unref(getDataSourceRef);
        let propsData: Recordable = {
          ...attrs,
          customRow,
          ...unref(getProps),
          ...unref(getHeaderProps),
          scroll: unref(getScrollRef),
          loading: unref(getLoading),
          tableLayout: 'fixed',
          rowSelection: unref(getRowSelectionRef),
          rowKey: unref(getRowKey),
          columns: reactive(unref(getViewColumns)),
          pagination: toRaw(unref(getPaginationInfo)),
          dataSource,
          footer: unref(getFooterProps),
          ...unref(getExpandOption),
        };
        // if (slots.expandedRowRender) {
        //   propsData = omit(propsData, 'scroll');
        // }

        propsData = omit(propsData, ['class', 'onChange']);
        return propsData;
      });

      const getWrapperClass = computed(() => {
        const values = unref(getBindValues);
        return [
          prefixCls,
          attrs.class,
          {
            [`${prefixCls}-form-container`]: values.useSearchForm,
            [`${prefixCls}--inset`]: values.inset,
          },
        ];
      });

      const getEmptyDataIsShowTable = computed(() => {
        const { emptyDataIsShowTable, useSearchForm } = unref(getProps);
        if (emptyDataIsShowTable || !useSearchForm) {
          return true;
        }
        return !!unref(getDataSourceRef).length;
      });

      function setProps(props: Partial<BasicTableProps>) {
        innerPropsRef.value = { ...unref(innerPropsRef), ...props };
      }

      const tableAction: TableActionType = {
        reload,
        getSelectRows,
        setSelectedRows,
        clearSelectedRowKeys,
        getSelectRowKeys,
        deleteSelectRowByKey,
        setPagination,
        setTableData,
        updateTableDataRecord,
        deleteTableDataRecord,
        insertTableDataRecord,
        findTableDataRecord,
        redoHeight,
        setSelectedRowKeys,
        setColumns,
        setLoading,
        getDataSource,
        getRawDataSource,
        setProps,
        getRowSelection,
        getPaginationRef: getPagination,
        getColumns,
        getCacheColumns,
        emit,
        updateTableData,
        setShowPagination,
        getShowPagination,
        setCacheColumnsByField,
        expandAll,
        expandRows,
        collapseRows,
        collapseAll,
        scrollTo,
        getSize: () => {
          return unref(getBindValues).size as SizeType;
        },
      };
      createTableContext({ ...tableAction, wrapRef, getBindValues });

      expose(tableAction);

      emit('register', tableAction, formActions);

      function handleResizeColumn(w, col) {
        if (isNumber(col.width)) {
          col.width = w;
        }
      }

      function handleSearchInfoReset() {
        const advancedSearch = unref(advancedSearchRef);
        advancedSearch?.resetFields();
      }

      function handleAdvanceSearch() {
        nextTick(() => openAdSearchModal(true));
      }

      return {
        t,
        formRef,
        tableElRef,
        advancedSearchRef,
        getBindValues,
        getLoading,
        registerForm,
        handleSearchInfoChange,
        registerAdSearchModal,
        handleAdvanceSearchChange,
        handleAdvanceSearchInfoChange,
        handleSearchInfoReset,
        handleAdvanceSearch,
        getEmptyDataIsShowTable,
        handleTableChange,
        getRowClassName,
        wrapRef,
        tableAction,
        redoHeight,
        getFormProps: getFormProps as any,
        getAdvancedSearchProps,
        replaceFormSlotKey,
        getFormSlotKeys,
        getWrapperClass,
        columns: getViewColumns,
        handleResizeColumn,
        handleTableExpand,
      };
    },
  });
</script>
<style lang="less">
  @border-color: #cecece4d;

  @prefix-cls: ~'@{namespace}-basic-table';

  [data-theme='dark'] {
    .ant-table-tbody > tr:hover.ant-table-row-selected > td,
    .ant-table-tbody > tr.ant-table-row-selected td {
      background-color: #262626;
    }
  }

  .@{prefix-cls} {
    max-width: 100%;
    height: 100%;

    &-row__striped {
      td {
        background-color: @app-content-background;
      }
    }

    &-form-container {
      padding: 16px;

      .ant-form {
        width: 100%;
        padding: 12px 10px 6px;
        margin-bottom: 16px;
        background-color: @component-background;
        border-radius: 2px;
      }
    }

    .ant-alert {
      margin-bottom: 16px;
    }

    .ant-table-cell {
      .ant-tag {
        margin-right: 0;
      }
    }

    .ant-table-wrapper {
      padding: 6px;
      background-color: @component-background;
      border-radius: 2px;

      .ant-table-title {
        min-height: 40px;
        padding: 0 0 8px !important;
      }

      .ant-table.ant-table-bordered .ant-table-title {
        border: none !important;
      }
    }

    .ant-table {
      width: 100%;
      overflow-x: hidden;

      // .ant-table-body {
      //   overflow: auto !important; // 注释解决列头宽度不一致问题
      // }

      &-title {
        display: flex;
        padding: 8px 6px;
        border-bottom: none;
        justify-content: space-between;
        align-items: center;
      }

      //.ant-table-tbody > tr.ant-table-row-selected td {
      //background-color: fade(@primary-color, 8%) !important;
      //}
    }

    .ant-pagination {
      margin: 10px 0 0;
    }

    .ant-table-footer {
      padding: 0;

      .ant-table-wrapper {
        padding: 0;
      }

      table {
        border: none !important;
      }

      .ant-table-body {
        overflow-x: hidden !important;
        //  overflow-y: scroll !important;
      }

      td {
        padding: 12px 8px;
      }
    }

    &--inset {
      .ant-table-wrapper {
        padding: 0;
      }
    }
  }
</style>
