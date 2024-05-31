<template>
  <Tooltip placement="top">
    <template #title>
      <span>{{ t('common.export') }}</span>
    </template>
    <DownloadOutlined @click="exportData" />
    <ExpExcelModal @register="registerModal" @success="handleExport"  />
  </Tooltip>
</template>
<script lang="ts" setup>
  import { Tooltip } from 'ant-design-vue';
  import { DownloadOutlined } from '@ant-design/icons-vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useTableContext } from '../../hooks/useTableContext';
  import { ExpExcelModal, jsonToSheetXlsx } from '/@/components/Excel';
  import { useModal } from '/@/components/Modal';
  import { isString } from '/@/utils/is';
  import { BasicColumn } from '../../types/table';

  const table = useTableContext();
  const { t } = useI18n();
  const [registerModal, { openModal }] = useModal();

  function exportData() {
    openModal(true);
  }

  function handleExport(options) {
    table.setLoading(true);
    try {
      exportDataToExcel(options);
    } finally {
      table.setLoading(false);
    }
  }

  function fillDataRows(columns: BasicColumn[], data: any, rows: any[]) {
    const row: {[key:string]: string} = {};
    columns.forEach((col) => {
      const colName = String(col.dataIndex);
      if (Reflect.has(data, colName)) {
        row[colName] = data[colName];
      }
    });
    if (Object.keys(row).length > 0) {
      rows.push(row);
    }
    if (Reflect.has(data, 'children') && Array.isArray(data.children)) {
      data.children.forEach((d) => {
        fillDataRows(columns, d, rows);
      });
    }
  }

  function exportDataToExcel(options) {
    const dataSource = table.getDataSource();
    // 列排序过滤
    const columns = table.getColumns()
      .filter((col) => !col.flag || col.flag === 'DEFAULT')
      .filter((col) => isString(col.title) && isString(col.dataIndex));
    // 标题列
    const header: {[key:string]: string} = {};
    columns.forEach((col) => {
      header[String(col.dataIndex)] = String(col.title);
    });
    // 数据列
    const rows: {[key:string]: string}[] = [];
    dataSource.forEach((data) => fillDataRows(columns, data, rows));
    // 输出到excel
    jsonToSheetXlsx({
      data: rows,
      header: header,
      filename: options.filename,
      write2excelOpts: {
        bookType: options.bookType,
      },
    });
  }
</script>
