import type { App } from 'vue';
import { Button } from './Button';
import { Input } from './Input';
import {
  // Need
  Button as AntButton,
  Layout,
  Input as AInput,
} from 'ant-design-vue';

const compList = [AntButton.Group];

export function registerGlobComp(app: App) {
  compList.forEach((comp) => {
    app.component(comp.name || comp.displayName, comp);
  });

  app.use(Input).use(AInput).use(Button).use(Layout);
}
