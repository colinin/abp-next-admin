import type { useVbenVxeGrid } from '@abp/ui';
import type { VxeGridProps as VxeTableGridProps } from 'vxe-table';

import type {
  ActionItem,
  BaseFormComponentType,
  ExtendedFormApi,
  ExtendedModalApi,
  FormValues,
  ModalProps,
  VbenFormProps,
} from '@vben/common-ui';

export type AbpRecord = Record<string, any>;

export type AbpId = number | string;

export type AbpIdOf<TDto extends AbpRecord> = TDto extends { id: infer TId }
  ? TId
  : AbpId;

export interface AbpDataTableLoadParams {
  /** 分页参数 */
  pager: {
    maxResultCount: number;
    skipCount: number;
  };
  /** 查询表单参数 */
  searchValues: Record<string, any>;
  /** 排序参数，例如 `name asc` */
  sorting?: string;
}

/** 响应解析配置 */
export interface AbpDataTableResponseOptions {
  /** 列表字段，默认 items */
  list?: string;
  /** 总数字段，默认 totalCount */
  total?: string;
}

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
  gridEvents?: Record<string, any>;
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
  /** 从 DTO 获取主键 */
  idKey?: (dto: TDto) => AbpIdOf<TDto>;
  /** 单条删除提示，未提供时使用 ABP 默认文案 */
  deleteWarnMessage?: (dto: TDto) => string;
  /** 响应解析配置 */
  response?: AbpDataTableResponseOptions;
  /** 编辑操作，调用方在这里打开编辑弹窗，例如 modalApi.open(data) */
  edit?: (dto: TDto) => void;
  /** 编辑按钮权限码 */
  editAuth?: ActionItem['auth'];
  /** 删除按钮权限码 */
  deleteAuth?: ActionItem['auth'];
  /** 编辑按钮文案 */
  editText?: string;
  /** 删除按钮文案 */
  deleteText?: string;
  /** 操作列配置，传 false 不生成默认操作列 */
  actionColumn?: AbpDataTableActionColumnOptions | false;
  /** 追加操作项 */
  actions?: (dto: TDto) => ActionItem[];
  /** “更多”下拉中的操作 */
  dropdownActions?: (dto: TDto) => ActionItem[];
}

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

export type AbpEditorMode = 'create' | 'edit' | 'view';

export interface AbpEditorOpenContext<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> {
  dto?: TDto;
  formApi: ExtendedFormApi<TFormValues>;
  mode: AbpEditorMode;
}

export interface AbpEditorModalOptions<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> {
  /** vben 表单配置 */
  formOptions: VbenFormProps<
    BaseFormComponentType,
    Record<string, never>,
    TFormValues
  >;
  /** 新增 API */
  create: (input: TFormValues) => Promise<any>;
  /** 修改 API，id 由 idKey 计算 */
  update: (id: AbpIdOf<TDto>, input: TFormValues) => Promise<any>;
  /** 按 id 获取 DTO，用于 modalApi.update(id)/view(id) */
  get?: (id: AbpIdOf<TDto>) => Promise<TDto>;
  /** 保存成功后的刷新回调（通常刷新表格） */
  refresh?: () => Promise<void> | void;
  /** 编辑标题中的业务值 */
  titleKey: (dto: TDto) => string;
  /** 从 DTO 获取主键，传给 update */
  idKey: (dto: TDto) => AbpIdOf<TDto>;
  /** 透传给 vben Modal 的配置 */
  modalOptions?: Partial<ModalProps>;
  /** 弹窗打开状态变化回调 */
  onOpenChange?: (isOpen: boolean) => Promise<void> | void;
  /** 弹窗打开时（表单已挂载）回调，可用于动态调整 schema */
  onOpen?: (
    context: AbpEditorOpenContext<TDto, TFormValues>,
  ) => Promise<void> | void;
  /** 新增时的默认表单值 */
  defaultValues?: (() => Partial<TFormValues>) | Partial<TFormValues>;
  /** 自定义标题 */
  title?: (mode: AbpEditorMode, dto?: TDto) => string;
  /** 保存成功回调 */
  onSuccess?: (mode: AbpEditorMode, dto?: TDto) => void;
}

export type AbpEditorModalApi<TDto extends AbpRecord = AbpRecord> = Omit<
  ExtendedModalApi<TDto>,
  'open'
> & {
  /** 无数据：新增；有数据（DTO 或 id）：编辑（或指定 view） */
  open: (data?: AbpIdOf<TDto> | TDto, mode?: 'edit' | 'view') => Promise<void>;
  /** 新增 */
  create: () => Promise<void>;
  /** 编辑 */
  update: (data: AbpIdOf<TDto> | TDto) => Promise<void>;
  /** 浏览 */
  view: (data: AbpIdOf<TDto> | TDto) => Promise<void>;
  /** 按当前数据重置表单 */
  refresh: () => Promise<void>;
};
