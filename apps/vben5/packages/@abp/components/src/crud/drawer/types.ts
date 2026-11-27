import type { DefineSetupFnComponent, SlotsType } from 'vue';

import type {
  DrawerProps,
  ExtendedDrawerApi,
  FormValues,
  VbenFormSlots,
} from '@vben/common-ui';

import type {
  AbpEditorFormOptions,
  AbpEditorMode,
  AbpEditorOpenContext,
} from '../form/types';
import type { AbpIdOf, AbpRecord } from '../types';

export type {
  AbpEditorFormOptions as AbpEditorDrawerFormOptions,
  AbpEditorFormSchema as AbpEditorDrawerFormSchema,
  AbpEditorMode as AbpEditorDrawerMode,
  AbpEditorOpenContext as AbpEditorDrawerOpenContext,
  AbpEditorTab as AbpEditorDrawerTab,
} from '../form/types';

export interface AbpEditorDrawerBaseSlots {
  'append-footer'?: () => any;
  cancelText?: () => any;
  'center-footer'?: () => any;
  'close-icon'?: () => any;
  confirmText?: () => any;
  description?: () => any;
  extra?: () => any;
  footer?: () => any;
  'prepend-footer'?: () => any;
  title?: () => any;
  titleTooltip?: () => any;
}

type KebabCase<S extends string> = S extends `${infer First}${infer Rest}`
  ? First extends Lowercase<First>
    ? `${First}${KebabCase<Rest>}`
    : `-${Lowercase<First>}${KebabCase<Rest>}`
  : S;

type FormSlotName<K extends PropertyKey> = K extends string
  ? `form-${KebabCase<K>}` | `form-${K}`
  : never;

/**
 * Drawer 内部 Form 的插槽，统一以 `form-` 前缀暴露。
 * 例如 `#form-email` 会传给 Form 的 `email` 插槽，`#form-property-changes` 会传给 Form 的 `propertyChanges` 插槽。
 */
export type AbpEditorDrawerFormSlots<
  TFormValues extends FormValues = FormValues,
> = {
  [
    K in keyof VbenFormSlots<TFormValues> as FormSlotName<K>
  ]?: VbenFormSlots<TFormValues>[K];
};

export type AbpEditorDrawerSlots<TFormValues extends FormValues = FormValues> =
  AbpEditorDrawerBaseSlots & AbpEditorDrawerFormSlots<TFormValues>;

export type AbpEditorDrawerComponent<
  TFormValues extends FormValues = FormValues,
> = DefineSetupFnComponent<
  DrawerProps,
  Record<string, never>,
  SlotsType<AbpEditorDrawerSlots<TFormValues>>
>;

export interface AbpEditorDrawerOptions<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> {
  /** vben 表单配置 */
  formOptions: AbpEditorFormOptions<TFormValues>;
  /** 新增 API */
  create?: (input: TFormValues) => Promise<any>;
  /** 修改 API，id 由 idKey 计算 */
  update?: (id: AbpIdOf<TDto>, input: TFormValues) => Promise<any>;
  /** 按 id 获取 DTO，用于 drawerApi.update(id)/view(id) */
  get?: (id: AbpIdOf<TDto>) => Promise<TDto>;
  /** 保存成功后的刷新回调（通常刷新表格） */
  refresh?: () => Promise<void> | void;
  /** 编辑标题中的业务值 */
  titleKey?: (dto: TDto) => string;
  /** 从 DTO 获取主键，传给 update */
  idKey: (dto: TDto) => AbpIdOf<TDto>;
  /** 透传给 vben Drawer 的配置 */
  drawerOptions?: Partial<DrawerProps>;
  /** 抽屉打开状态变化回调 */
  onOpenChange?: (isOpen: boolean) => Promise<void> | void;
  /** 抽屉打开时（表单已挂载）回调，可用于动态调整 schema */
  onOpen?: (
    context: AbpEditorOpenContext<TDto, TFormValues>,
  ) => Promise<void> | void;
  /** 新增时的默认表单值 */
  defaultValues?: (() => Partial<TFormValues>) | Partial<TFormValues>;
  /** 自定义标题 */
  title?: (mode: AbpEditorMode, dto?: TDto) => string | undefined;
  /** 自定义描述 */
  description?: (mode: AbpEditorMode, dto?: TDto) => string | undefined;
  /** 保存成功回调 */
  onSuccess?: (mode: AbpEditorMode, dto?: TDto) => void;
}

export type AbpEditorDrawerApi<TDto extends AbpRecord = AbpRecord> = Omit<
  ExtendedDrawerApi<TDto>,
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
