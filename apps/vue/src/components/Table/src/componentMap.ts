import type { Component } from 'vue';
import { Select, Checkbox, InputNumber, Switch, DatePicker, TimePicker } from 'ant-design-vue';

import type { ComponentType } from './types/componentType';
import { Input as BInput } from '/@/components/Input';
import { ApiSelect, ApiTreeSelect } from '/@/components/Form';

const componentMap = new Map<ComponentType, Component>();

componentMap.set('Input', BInput);
componentMap.set('InputNumber', InputNumber);
componentMap.set('Select', Select);
componentMap.set('ApiSelect', ApiSelect);
componentMap.set('ApiTreeSelect', ApiTreeSelect);
componentMap.set('Switch', Switch);
componentMap.set('Checkbox', Checkbox);
componentMap.set('DatePicker', DatePicker);
componentMap.set('TimePicker', TimePicker);

export function add(compName: ComponentType, component: Component) {
  componentMap.set(compName, component);
}

export function del(compName: ComponentType) {
  componentMap.delete(compName);
}

export { componentMap };
