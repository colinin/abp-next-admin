import type { PrintDesignerElement } from 'vue-print-designer';

/** 设计器字体选项 */
type PrintDesignerFontOption = {
  /** 下拉显示文本 */
  label: string;
  /** 对应字体族字符串 */
  value: string;
};

interface PrintDesignerFont {
  /** 设计器字体 */
  fontFamily?: string;
  /** 字体选项平铺数组，不包含分组字段；默认 [] */
  options?: PrintDesignerFontOption[];
  /** 是否保存到本地 */
  persist?: boolean;
}

interface PrintDesignerBranding {
  /** Logo 地址 */
  logoUrl?: string;
  /** 是否保存到本地 */
  persist?: boolean;
  /** 是否显示 Logo */
  showLogo?: boolean;
  /** 是否显示标题 */
  showTitle?: boolean;
  /** 标题文本 */
  title?: string;
}

interface PrintDesignerBrandVariable {
  /** 是否保存到本地 */
  persist?: boolean;
  /** 主题色变量集合 */
  vars?: Record<string, string>;
}

type PrintMode = 'browser' | 'local' | 'remote';

interface LocalConnectionSettings {
  secretKey: string;
  wsAddress: string;
}

interface RemoteConnectionSettings {
  apiBaseUrl: string;
  password: string;
  username: string;
  wsAddress: string;
}

interface PrintOptions {
  colorMode: '' | 'color' | 'monochrome';
  copies: number;
  intervalMs: number;
  jobName: string;
  orientation: '' | 'landscape' | 'portrait';
  pageRange: string;
  pageSet: '' | 'even' | 'odd';
  paperSize: string;
  printer: string;
  scale: '' | 'fit' | 'noscale' | 'shrink';
  sidesMode: '' | 'duplex' | 'duplexlong' | 'duplexshort' | 'simplex';
  timeout?: number;
  trayBin: string;
}

interface PrintDesignerPrintOptions {
  exportImageMerged?: boolean;
  localPrintOptions?: Partial<PrintOptions>;
  localSettings?: Partial<LocalConnectionSettings>;
  printMode?: PrintMode;
  remotePrintOptions?: Partial<PrintOptions>;
  remoteSettings?: Partial<RemoteConnectionSettings>;
  silentPrint?: boolean;
}

interface PrintDesignerOptions {
  /** 设计器品牌 */
  branding?: PrintDesignerBranding;
  /** 设计器品牌变量 */
  brandVariable?: PrintDesignerBrandVariable;
  /** 默认模板Id */
  defaultTemplateId?: string;
  /** 设计器字体 */
  font?: PrintDesignerFont;
  /** 无头模式 (headless) 后台静默打印 */
  headless?: boolean;
  /** 打印配置 */
  print?: PrintDesignerPrintOptions;
}

type DesignerProgressScope = 'export' | 'preview' | 'print';

type DesignerProgressPayload = {
  current: number;
  message: string;
  percent: number;
  phase: string;
  scope: DesignerProgressScope;
  total: number;
};

type DesignerProgressCallback = (payload: DesignerProgressPayload) => void;

interface TemplatePrintRequest {
  mode?: PrintMode;
  onProgress?: DesignerProgressCallback;
  options?: PrintOptions;
  variables: Record<string, any>;
}

interface TemplateIdPrintRequest extends TemplatePrintRequest {
  templateId: string;
}

interface TemplateNamePrintRequest extends TemplatePrintRequest {
  templateName: string;
}

type DesignerLabelOptions = {
  background?: 'auto' | 'off';
  /** 黑标位置，单位 mm。 */
  bline?: number;
  density?: number;
  dpi?: number;
  /** 前置条件不满足时回退 PDF 通道，默认 false（直接报错）。 */
  fallbackToPdf?: boolean;
  /** 标签间隙，单位 mm。 */
  gap?: number;
  /** > 0 时逐张一个 RAW 作业，用于留出回退定位时间。 */
  intervalMs?: number;
  jobName?: string;
  /** 指令集，缺省 auto：按打印机名与驱动名推断，推断失败需显式指定。 */
  language?: 'auto' | 'cpcl' | 'escpos' | 'tspl' | 'zpl';
  offset?: number;
  /** 纸张规格（mm）；不传时用 canvasSize 换算。 */
  pageSize?: { height: number; width: number };
  printer?: string;
  speed?: number;
  textMode?: '' | 'bitmap' | 'device';
  /** 下发超时，默认 60s。 */
  timeout?: number;
};

interface TemplatePrintBatchData {
  /** 打印份数 */
  copies?: number;
  /** 测试数据 */
  testData?: Record<string, any>;
  /** 变量数据 */
  variables?: Record<string, any>;
}

interface TemplatePrintBatchRequest {
  /** 打印通道：pdf（默认）或 label（客户端原生标签指令）。 */
  format?: 'label' | 'pdf';
  /** format 为 label 时必填。 */
  label?: DesignerLabelOptions;
  mode?: PrintMode;
  onProgress?: DesignerProgressCallback;
  options?: PrintOptions;
}

interface TemplateIdPrintBatchRequest extends TemplatePrintBatchRequest {
  templateId: string;
}

interface TemplateNamePrintBatchRequest extends TemplatePrintBatchRequest {
  templateName: string;
}

type PrintQuality = 'fast' | 'high' | 'normal' | 'ultra';

interface TemplatePreviewRequest {
  canvasSize?: { height: number; width: number };
  html?: string;
  json?: object | string;
  key?: string;
  mode?: 'html' | 'json' | 'pdf';
  onProgress?: DesignerProgressCallback;
  printQuality?: PrintQuality;
  timeoutMs?: number;
  title?: string;
  variables: Record<string, any>;
}

interface TemplateIdPreviewRequest extends TemplatePreviewRequest {
  templateId: string;
}

interface TemplateNamePreviewRequest extends TemplatePreviewRequest {
  templateName: string;
}

interface PrintDesignerInstance {
  /** 设计器api,使用它操作设计器的能力 */
  designerApi?: null | PrintDesignerElement;
  /** 打印预览（模板Id） */
  preview(request: TemplateIdPreviewRequest): Promise<void>;
  /** 打印预览（模板名称） */
  previewWithName(request: TemplateNamePreviewRequest): Promise<void>;
  /** 打印（模板Id） */
  print(request: TemplateIdPrintRequest): Promise<void>;
  /** 批量打印（模板Id） */
  printBatch(
    data: TemplatePrintBatchData[],
    request: TemplateIdPrintBatchRequest,
  ): Promise<void>;
  /** 批量打印（模板名称） */
  printBatchWithName(
    data: TemplatePrintBatchData[],
    request: TemplateNamePrintBatchRequest,
  ): Promise<void>;
  /** 打印（模板名称） */
  printWithName(request: TemplateNamePrintRequest): Promise<void>;
  /** 设置当前模板 */
  setCurrentTemplte(templateId: string): Promise<void>;
}

export type {
  PrintDesignerFont,
  PrintDesignerInstance,
  PrintDesignerOptions,
  TemplateIdPreviewRequest,
  TemplateIdPrintBatchRequest,
  TemplateIdPrintRequest,
  TemplateNamePreviewRequest,
  TemplateNamePrintBatchRequest,
  TemplateNamePrintRequest,
  TemplatePreviewRequest,
  TemplatePrintBatchData,
  TemplatePrintBatchRequest,
  TemplatePrintRequest,
};
