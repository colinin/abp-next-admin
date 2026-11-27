import type {
  BaseFormComponentType,
  ExtendedFormApi,
  FormValues,
  VbenFormProps,
  VbenFormSchema,
} from '@vben/common-ui';

import type { AbpRecord } from '../types';

/** 编辑弹窗/抽屉中的 Tab 配置 */
export interface AbpEditorTab {
  /** Tab 唯一标识，对应 schema.tabKey */
  key: string;
  /** Tab 标题 */
  title: string;
  /** 是否禁用 */
  disabled?: boolean;
}

/** 带 Tab 归属的 Form schema */
export type AbpEditorFormSchema<TFormValues extends FormValues = FormValues> = {
  /** 该字段所属的 Tab key，未设置时归入第一个 Tab */
  tabKey?: string;
} & VbenFormSchema<BaseFormComponentType, Record<string, never>, TFormValues>;

/** 编辑弹窗/抽屉 Form 配置，在 Vben Form 基础上增加 tabs */
export type AbpEditorFormOptions<TFormValues extends FormValues = FormValues> =
  {
    /** Tab 列表，未设置时使用普通表单 */
    tabs?: AbpEditorTab[];
    /** 表单 schema，每项可通过 tabKey 指定所属 Tab */
    schema?: AbpEditorFormSchema<TFormValues>[];
  } & Omit<
    VbenFormProps<BaseFormComponentType, Record<string, never>, TFormValues>,
    'schema'
  >;

export type AbpEditorMode = 'create' | 'edit' | 'view';

export interface AbpEditorOpenContext<
  TDto extends AbpRecord = AbpRecord,
  TFormValues extends FormValues = FormValues,
> {
  dto?: TDto;
  formApi: ExtendedFormApi<TFormValues>;
  mode: AbpEditorMode;
}
