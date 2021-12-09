import type { App, Directive, DirectiveBinding } from 'vue';
import { useFeatures } from '/@/hooks/abp/useFeatures';

function isFeature(el: Element, binding: any) {
  const { featureChecker } = useFeatures();

  const value = binding.value;
  if (!value) return;
  if (!featureChecker.isEnabled(value)) {
    el.parentNode?.removeChild(el);
  }
}

const mounted = (el: Element, binding: DirectiveBinding<any>) => {
  isFeature(el, binding);
};

const featureDirective: Directive = {
  mounted,
};

export function setupFeatureDirective(app: App) {
  app.directive('feature', featureDirective);
}

export default featureDirective;
