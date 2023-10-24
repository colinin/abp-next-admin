import { Rule } from 'ant-design-vue/lib/form';

export interface ModalState {
  activeTab: string,
  allowedChange: boolean,
  entity: Recordable,
  entityRules?: Dictionary<string, Rule>,
  entityChanged: boolean,
  entityEditFlag: boolean,
}
