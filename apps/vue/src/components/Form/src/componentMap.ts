import type { Component } from 'vue';
import type { ComponentType } from './types/index';

/**
 * Component list, register here to setting it in the form
 */
import {
  Input,
  Select,
  Radio,
  Checkbox,
  AutoComplete,
  Cascader,
  DatePicker,
  InputNumber,
  Switch,
  TimePicker,
  TreeSelect,
  Slider,
  Rate,
  Divider,
} from 'ant-design-vue';
import { ColorPicker } from 'vue3-colorpicker';

import ApiRadioGroup from './components/ApiRadioGroup.vue';
import RadioButtonGroup from './components/RadioButtonGroup.vue';
import ApiSelect from './components/ApiSelect.vue';
import ApiTree from './components/ApiTree.vue';
import ApiTreeSelect from './components/ApiTreeSelect.vue';
import ApiCascader from './components/ApiCascader.vue';
import ApiTransfer from './components/ApiTransfer.vue';
import { BasicUpload } from '/@/components/Upload';
import { StrengthMeter } from '/@/components/StrengthMeter';
import { IconPicker } from '/@/components/Icon';
import { CountdownInput } from '/@/components/CountDown';
import { Input as BInput } from '/@/components/Input';
import { CodeEditorX } from '/@/components/CodeEditor';
import {
  ExtraPropertyDictionary,
  LocalizableInput
} from '/@/components/Abp';

const componentMap = new Map<ComponentType, Component>();
const customComponentMap = new Map<ComponentType, Component>();

customComponentMap.set('CodeEditorX', CodeEditorX);
customComponentMap.set('Input', BInput);
customComponentMap.set('ApiSelect', ApiSelect);
customComponentMap.set('ApiTree', ApiTree);
customComponentMap.set('ApiTreeSelect', ApiTreeSelect);
customComponentMap.set('ApiRadioGroup', ApiRadioGroup);
customComponentMap.set('ApiCascader', ApiCascader);
customComponentMap.set('StrengthMeter', StrengthMeter);
customComponentMap.set('InputCountDown', CountdownInput);
customComponentMap.set('IconPicker', IconPicker);
customComponentMap.set('Upload', BasicUpload);
customComponentMap.set('ApiTransfer', ApiTransfer);
customComponentMap.set('RadioButtonGroup', RadioButtonGroup);

componentMap.set('InputGroup', Input.Group);
componentMap.set('InputPassword', Input.Password);
componentMap.set('InputSearch', Input.Search);
componentMap.set('InputTextArea', Input.TextArea);
componentMap.set('InputNumber', InputNumber);
componentMap.set('AutoComplete', AutoComplete);

componentMap.set('Select', Select);
componentMap.set('TreeSelect', TreeSelect);
componentMap.set('Switch', Switch);
componentMap.set('RadioGroup', Radio.Group);
componentMap.set('Checkbox', Checkbox);
componentMap.set('CheckboxGroup', Checkbox.Group);
componentMap.set('Cascader', Cascader);
componentMap.set('Slider', Slider);
componentMap.set('Rate', Rate);

componentMap.set('DatePicker', DatePicker);
componentMap.set('MonthPicker', DatePicker.MonthPicker);
componentMap.set('RangePicker', DatePicker.RangePicker);
componentMap.set('WeekPicker', DatePicker.WeekPicker);
componentMap.set('TimePicker', TimePicker);
componentMap.set('Divider', Divider);

componentMap.set('ColorPicker', ColorPicker);

componentMap.set('ExtraPropertyDictionary', ExtraPropertyDictionary);
componentMap.set('LocalizableInput', LocalizableInput);

customComponentMap.forEach((v, k) => {
  componentMap.set(k, v);
});

export function add(compName: ComponentType, component: Component) {
  componentMap.set(compName, component);
}

export function del(compName: ComponentType) {
  componentMap.delete(compName);
}

export { componentMap, customComponentMap };
