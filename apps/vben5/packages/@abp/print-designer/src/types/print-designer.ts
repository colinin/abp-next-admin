/** 设计器主题；light / dark 明确生效，其他值（含 system）跟随系统 */
type PrintDesignerTheme = 'dark' | 'light' | 'system';

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

interface PrintDesignerOptions {
  /** 设计器品牌 */
  branding?: PrintDesignerBranding;
  /** 设计器字体 */
  font?: PrintDesignerFont;
  /** 设计器主题 */
  theme?: PrintDesignerTheme;
  /** 设计器变量 */
  variable?: PrintDesignerBrandVariable;
}

export type { PrintDesignerFont, PrintDesignerOptions, PrintDesignerTheme };
