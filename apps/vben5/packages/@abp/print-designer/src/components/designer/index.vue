<script setup lang="ts">
import type { PrintDesignerElement } from 'vue-print-designer';

import type { TemplateDto } from '../../types';
import type {
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
} from '../../types/print-designer';

import {
  nextTick,
  onBeforeUnmount,
  onMounted,
  useTemplateRef,
  watch,
} from 'vue';
import 'vue-print-designer';

import { usePreferences } from '@vben/preferences';

import { buildUUID } from '@abp/core';

import { usePrintDotFetcher } from '../../api/usePrintDotFetcher';
import { useTemplatesApi } from '../../api/useTemplatesApi';

const props = withDefaults(defineProps<PrintDesignerOptions>(), {
  brandVariable: () => ({}),
  /** 默认打印模式为本地客户端打印 */
  print: () => ({
    printMode: 'local',
    localSettings: {
      wsAddress: 'wss://127.0.0.1:1122/ws',
    },
  }),
});

const { locale, theme } = usePreferences();
const fetcher = usePrintDotFetcher();
const { getApi, getByNameApi } = useTemplatesApi();
const designer = useTemplateRef<PrintDesignerElement>('designer');
const designerComponent = 'print-designer';

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
  if (props.headless !== true) {
    setCrudConfig(designer.value);
  }
  setDefaultTemplate(designer.value, props.defaultTemplateId);
  setLocale();
  setTheme();
}

async function setDefaultProps(
  designer: PrintDesignerElement,
  defaultProps: PrintDesignerOptions,
) {
  if (defaultProps.branding) {
    designer.setBranding(defaultProps.branding);
  }
  if (defaultProps.brandVariable) {
    designer.setBrandVars(defaultProps.brandVariable.vars ?? {}, {
      persist: defaultProps.brandVariable.persist,
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

  if (defaultProps.print) {
    designer.setPrintDefaults(defaultProps.print);
    if (defaultProps.print.printMode !== 'browser') {
      // 本地/远程打印连接测试成功后再次设置一下打印模式,即可无需在设计器中再次设置打印模式
      await designer.fetchLocalPrinters();
      designer.setPrintDefaults({
        printMode: defaultProps.print.printMode,
      });
    }
  }
}

function setCrudConfig(designer: PrintDesignerElement) {
  designer.setCrudEndpoints(
    {
      templates: {
        get: { method: 'GET', url: '/api/print-designer/templates/{id}' },
        upsert: {
          method: 'POST',
          url: '/api/print-designer/templates/upsert',
        },
        delete: {
          method: 'DELETE',
          url: '/api/print-designer/templates/{id}',
        },
        list: { method: 'GET', url: '/api/print-designer/templates/all' },
      },
      customElements: {
        get: {
          method: 'GET',
          url: '/api/print-designer/custom-elements/{id}',
        },
        upsert: {
          method: 'POST',
          url: '/api/print-designer/custom-elements/upsert',
        },
        delete: {
          method: 'DELETE',
          url: '/api/print-designer/custom-elements/{id}',
        },
        list: {
          method: 'GET',
          url: '/api/print-designer/custom-elements/all',
        },
      },
    },
    { fetcher },
  );
  designer.setCrudMode('remote');
}

async function setCurrentTemplte(templateId: string) {
  const el = designer.value;
  if (!el || !templateId) {
    return;
  }
  await setDefaultTemplate(el, templateId);
}

async function print(request: TemplateIdPrintRequest): Promise<void> {
  const template = await getApi(request.templateId);
  await printWithTemplate(template, request);
}

async function printWithName(request: TemplateNamePrintRequest): Promise<void> {
  const template = await getByNameApi(request.templateName);
  await printWithTemplate(template, request);
}

async function printWithTemplate(
  template: TemplateDto,
  request: TemplatePrintRequest,
) {
  const el = designer.value;
  if (!el) {
    return;
  }
  el.loadTemplateData({
    ...template.data,
    ext: template.ext,
  });
  await el.setVariables(request.variables);
  const printMode = request.mode ?? props.print?.printMode;
  request.options ??= {
    copies: 1,
    colorMode: '',
    jobName: `print_${buildUUID()}`,
    intervalMs: 0,
    orientation: '',
    pageRange: '',
    pageSet: '',
    scale: '',
    paperSize: '',
    sidesMode: '',
    trayBin: '',
    printer: '',
  };
  if (printMode === 'local' && !request.options.printer) {
    const printers = await el.fetchLocalPrinters();
    request.options.printer = printers.find((p) => p.isDefault === true)?.name;
  } else if (printMode === 'remote' && !request.options.printer) {
    const printers = await el.fetchRemotePrinters();
    request.options.printer = printers.find((p) => p.isDefault === true)?.name;
  }
  await el.print(request);
}

async function printBatch(
  data: TemplatePrintBatchData[],
  request: TemplateIdPrintBatchRequest,
): Promise<void> {
  const template = await getApi(request.templateId);
  await printBatchWithTemplate(template, data, request);
}

async function printBatchWithName(
  data: TemplatePrintBatchData[],
  request: TemplateNamePrintBatchRequest,
): Promise<void> {
  const template = await getByNameApi(request.templateName);
  await printBatchWithTemplate(template, data, request);
}

async function printBatchWithTemplate(
  template: TemplateDto,
  data: TemplatePrintBatchData[],
  request: TemplatePrintBatchRequest,
) {
  const el = designer.value;
  if (!el) {
    return;
  }
  el.loadTemplateData({
    ...template.data,
    ext: template.ext,
  });
  const printMode = request.mode ?? props.print?.printMode;
  request.options ??= {
    copies: 1,
    colorMode: '',
    jobName: `print_${buildUUID()}`,
    intervalMs: 0,
    orientation: '',
    pageRange: '',
    pageSet: '',
    scale: '',
    paperSize: '',
    sidesMode: '',
    trayBin: '',
    printer: '',
  };
  if (
    printMode === 'local' &&
    (!request.options.printer || !request.label?.printer)
  ) {
    const printers = await el.fetchLocalPrinters();
    const defaultPrinter = printers.find((p) => p.isDefault === true)?.name;
    request.options.printer = defaultPrinter;
    if (request.label) {
      request.label.printer = defaultPrinter;
    }
  } else if (
    printMode === 'remote' &&
    (!request.options.printer || !request.label?.printer)
  ) {
    const printers = await el.fetchRemotePrinters();
    const defaultPrinter = printers.find((p) => p.isDefault === true)?.name;
    request.options.printer = defaultPrinter;
    if (request.label) {
      request.label.printer = defaultPrinter;
    }
  }
  await el.printBatch(data, request);
}

async function preview(request: TemplateIdPreviewRequest): Promise<void> {
  const template = await getApi(request.templateId);
  await previewWithTemplate(template, request);
}

async function previewWithName(
  request: TemplateNamePreviewRequest,
): Promise<void> {
  const template = await getByNameApi(request.templateName);
  await previewWithTemplate(template, request);
}

async function previewWithTemplate(
  template: TemplateDto,
  request: TemplatePreviewRequest,
) {
  const el = designer.value;
  if (!el) {
    return;
  }
  el.loadTemplateData({
    ...template.data,
    ext: template.ext,
  });
  await el.setVariables(request.variables);
  await el.preview(request);
}

async function setDefaultTemplate(
  designer: PrintDesignerElement,
  defaultTemplateId?: string,
) {
  if (!defaultTemplateId) {
    return;
  }
  const templates = await designer.refreshTemplates({ includeData: true });
  const list = Array.isArray(templates) ? [...templates] : [];
  if (!list.some((x) => x.id === defaultTemplateId)) {
    list.unshift({ id: defaultTemplateId, name: '', updatedAt: Date.now() });
  }
  designer.setTemplates(list, {
    currentTemplateId: defaultTemplateId,
  });
}

function setTheme() {
  designer.value?.setTheme(theme.value);
}

function setLocale() {
  switch (locale.value) {
    case 'zh-CN': {
      designer.value?.setLanguage('zh');
      break;
    }
    default: {
      designer.value?.setLanguage('en');
      break;
    }
  }
}

watch(
  () => theme.value,
  () => {
    setTheme();
  },
  {
    deep: true,
    immediate: true,
  },
);

watch(
  () => locale.value,
  () => {
    setLocale();
  },
  {
    deep: true,
    immediate: true,
  },
);

onMounted(() => {
  initDesigner();
  initDesignerStackingContext();
});
onBeforeUnmount(() => {
  modalObserver?.disconnect();
  modalObserver = undefined;
});

defineExpose<PrintDesignerInstance>({
  get designerApi() {
    return designer.value;
  },
  setCurrentTemplte,
  preview,
  previewWithName,
  print,
  printWithName,
  printBatch,
  printBatchWithName,
});
</script>

<template>
  <component
    :is="designerComponent"
    ref="designer"
    :headless="props.headless"
  />
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
