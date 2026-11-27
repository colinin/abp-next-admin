import type { useVbenVxeGrid, VxeGridListeners } from '@abp/ui';
import type {
  VxeGridSlots,
  VxeGridSlotTypes,
  VxeGridProps as VxeTableGridProps,
} from 'vxe-table';

import type { DefineSetupFnComponent, SlotsType } from 'vue';

import type {
  ActionItem,
  BaseFormComponentType,
  FormValues,
  VbenFormProps,
} from '@vben/common-ui';

import type { VbenIcon } from '@vben-core/shadcn-ui';

import type { AbpIdOf, AbpRecord } from '../types';

export interface AbpDataTableSortable {
  /** 排序字段 */
  field: string;
  /** 排序方向 */
  order: string;
  /** 已拼接好的排序字符串，例如 name asc */
  sorting: string;
}

export interface AbpDataTableLoadParams {
  /** 分页参数 */
  pager: {
    maxResultCount: number;
    skipCount: number;
  };
  /** 查询表单参数 */
  searchValues: Record<string, any>;
  /** 排序参数 */
  sortable?: AbpDataTableSortable;
}

/** 响应解析配置 */
export interface AbpDataTableResponseOptions {
  /** 列表字段，默认 items */
  list?: string;
  /** 总数字段，默认 totalCount */
  total?: string;
}

export type AbpRowKey<TDto extends AbpRecord = AbpRecord> = Extract<
  keyof TDto,
  string
>;

export type AbpDataTableGridEvents<TDto extends AbpRecord = AbpRecord> =
  VxeGridListeners<TDto> & {
    /** 表格选择项变化时触发 */
    onCheckboxChange?: (rows: TDto[]) => void;
  };

export interface AbpDataTableActionColumnOptions {
  title?: string;
  width?: number;
}

export interface AbpDataTableOptions<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> {
  /** vben 表单配置 */
  formOptions?: VbenFormProps<
    BaseFormComponentType,
    Record<string, never>,
    TFormValues
  >;
  /** vxe-grid 配置 */
  gridOptions?: VxeTableGridProps<TDto>;
  /** vxe-grid 事件 */
  gridEvents?: AbpDataTableGridEvents<TDto>;
  /** 表格标题 */
  tableTitle?: string;
  /** 表格标题帮助 */
  tableTitleHelp?: string;
  /** 是否显示搜索表单 */
  showSearchForm?: boolean;
  /** 搜索表单与表格之间的分隔条 */
  separator?: boolean | Record<string, any>;
  /** 分段模式 */
  segmented?: 'card' | 'zone' | false;
  /** 表单外层 class */
  formClass?: any;
  /** 组件 class */
  class?: any;
  /** vxe-grid class */
  gridClass?: any;
  /** 加载数据 */
  load: (params: AbpDataTableLoadParams) => Promise<any>;
  /** 删除单个 */
  delete?: (dto: TDto) => Promise<any>;
  /** 删除多个 */
  deleteMany?: (ids: AbpIdOf<TDto>[]) => Promise<any>;
  /** 行主键字段名，用于主键读取、删除和 checkbox 选择保留，默认 id */
  rowKey?: AbpRowKey<TDto>;
  /** 单条删除提示，未提供时使用 ABP 默认文案 */
  deleteWarnMessage?: (dto: TDto) => string;
  /** 响应解析配置 */
  response?: AbpDataTableResponseOptions;
  /** 编辑操作，调用方在这里打开编辑弹窗，例如 modalApi.open(data) */
  edit?: (dto: TDto) => void;
  /** 编辑按钮权限码 */
  editAuth?: ActionItem['auth'];
  /** 编辑按钮文案 */
  editText?: string;
  /** 编辑按钮图标组件 */
  editIcon?: typeof VbenIcon.icon;
  /** 删除按钮权限码 */
  deleteAuth?: ActionItem['auth'];
  /** 删除按钮文案 */
  deleteText?: string;
  /** 删除按钮图标组件 */
  deleteIcon?: typeof VbenIcon.icon;
  /** 操作列配置，传 false 不生成默认操作列 */
  actionColumn?: AbpDataTableActionColumnOptions | false;
  /** 追加操作项 */
  actions?: (dto: TDto) => ActionItem[];
  /** “更多”下拉中的操作 */
  dropdownActions?: (dto: TDto) => ActionItem[];
}

export type AbpDataTableComponentProps<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> = Omit<
  AbpDataTableOptions<TDto, TFormValues>,
  | 'actionColumn'
  | 'actions'
  | 'delete'
  | 'deleteAuth'
  | 'deleteMany'
  | 'deleteText'
  | 'deleteWarnMessage'
  | 'dropdownActions'
  | 'edit'
  | 'editAuth'
  | 'editText'
  | 'hasPermission'
  | 'load'
  | 'response'
  | 'rowKey'
>;

export type AbpDataTableSlots<TDto extends AbpRecord = AbpRecord> = {
  [
    K in keyof VxeGridSlots<TDto> as K extends 'form' ? never : K
  ]?: VxeGridSlots<TDto>[K];
} & {
  /** 表格标题插槽 */
  'table-title'?: undefined;
  /** 工具栏左侧插槽 */
  'toolbar-actions'?: VxeGridSlotTypes.DefaultSlotParams<TDto>;
  /** 工具栏右侧插槽 */
  'toolbar-tools'?: VxeGridSlotTypes.DefaultSlotParams<TDto>;
};

export type AbpDataTableComponent<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> = DefineSetupFnComponent<
  AbpDataTableComponentProps<TDto, TFormValues>,
  Record<string, never>,
  SlotsType<AbpDataTableSlots<TDto>>
>;

type VbenGridApi = ReturnType<typeof useVbenVxeGrid>[1];

export type AbpDataTableApi<TDto extends AbpRecord = AbpRecord> =
  VbenGridApi & {
    /** 删除单个（带确认） */
    delete: (dto: TDto) => Promise<void>;
    /** 删除多个（带确认） */
    deleteMany: (ids: AbpIdOf<TDto>[]) => Promise<void>;
    /** 刷新表格 */
    refresh: () => Promise<void>;
    /** 触发编辑 */
    edit: (dto: TDto) => void;
  };
