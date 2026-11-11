<script setup lang="ts">
import type { PrintDesignerElement } from 'vue-print-designer';

import type { PrintDesignerOptions } from '../../types/print-designer';

import { nextTick, onBeforeUnmount, onMounted, useTemplateRef } from 'vue';
import 'vue-print-designer';

import { PRINT_DESIGNER_COLLECTION_PATHS } from '../../api/printDesignerCrudFetcher';
import { usePrintDotFetcher } from '../../api/usePrintDotFetcher';

const props = withDefaults(defineProps<PrintDesignerOptions>(), {
  vars: () => ({}),
  theme: 'system',
});
const templateCollection = PRINT_DESIGNER_COLLECTION_PATHS.templates;
const customElementCollection = PRINT_DESIGNER_COLLECTION_PATHS.customElements;

const fetcher = usePrintDotFetcher();
const designer = useTemplateRef<PrintDesignerElement>('designer');

let modalObserver: MutationObserver | undefined;

function bindDesignerModalStackingContext(
  host: HTMLElement,
  modalContainer: HTMLElement,
) {
  const sync = () => {
    host.style.isolation =
      modalContainer.childElementCount > 0 ? 'auto' : 'isolate';
  };
  sync();
  modalObserver = new MutationObserver(sync);
  modalObserver.observe(modalContainer, { childList: true });
}

async function initDesignerStackingContext() {
  await nextTick();
  const host = designer.value as unknown as HTMLElement | null;
  const modalContainer = host?.shadowRoot?.querySelector<HTMLElement>(
    '.print-designer-modals',
  );
  if (host && modalContainer) {
    bindDesignerModalStackingContext(host, modalContainer);
  }
}

async function initDesigner() {
  await nextTick();
  if (!designer.value) {
    return;
  }
  setDefaultProps(designer.value, props);
  setCrudConfig(designer.value);
}

function setDefaultProps(
  designer: PrintDesignerElement,
  defaultProps: PrintDesignerOptions,
) {
  if (defaultProps.branding) {
    designer.setBranding(defaultProps.branding);
  }
  if (defaultProps.variable) {
    designer.setBrandVars(defaultProps.variable.vars ?? {}, {
      persist: defaultProps.variable.persist,
    });
  }
  if (defaultProps.font?.fontFamily) {
    designer.setDesignerFont(defaultProps.font.fontFamily, {
      persist: defaultProps.font.persist,
    });
  }
  if (defaultProps.font?.options) {
    designer.setFontOptions(defaultProps.font.options);
  }
}

function setCrudConfig(designer: PrintDesignerElement) {
  designer.setCrudEndpoints(
    {
      templates: {
        get: { method: 'GET', url: `${templateCollection}/{id}` },
        upsert: {
          method: 'POST',
          url: templateCollection,
        },
        delete: {
          method: 'DELETE',
          url: `${templateCollection}/{id}`,
        },
        list: { method: 'GET', url: templateCollection },
      },
      customElements: {
        get: {
          method: 'GET',
          url: `${customElementCollection}/{id}`,
        },
        upsert: {
          method: 'POST',
          url: customElementCollection,
        },
        delete: {
          method: 'DELETE',
          url: `${customElementCollection}/{id}`,
        },
        list: {
          method: 'GET',
          url: customElementCollection,
        },
      },
    },
    { fetcher },
  );
  designer.setCrudMode('remote');
}

onMounted(initDesigner);
onMounted(initDesignerStackingContext);
onBeforeUnmount(() => {
  modalObserver?.disconnect();
  modalObserver = undefined;
});
</script>

<template>
  <print-designer ref="designer" />
</template>

<style scoped>
print-designer {
  display: block;
  width: 100%;
  height: 100%;
  min-height: 0;
  isolation: isolate;
}
</style>
