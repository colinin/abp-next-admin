import type { ButtonType } from 'ant-design-vue/lib/button';

interface BindButton {
  click: () => Promise<void> | void;
  title: string;
  type?: ButtonType;
}

interface BindItem {
  buttons?: BindButton[];
  description?: string;
  enable?: boolean;
  slot?: string;
  title: string;
}

export type { BindItem };
