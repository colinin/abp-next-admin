const FORM_ACTION_SLOT_NAMES = new Set([
  'default',
  'expand-after',
  'expand-before',
  'reset-before',
  'submit-before',
]);

function toCamelCaseSlotName(name: string) {
  return name.replaceAll(/-([a-z])/g, (_, char: string) => char.toUpperCase());
}

/**
 * 拆分编辑器插槽：
 * - form-* 透传给内部 Form
 * - 其余插槽透传给外部容器
 */
export function splitEditorSlots(slots: Record<string, any>) {
  const formSlots: Record<string, any> = {};
  const containerSlots: Record<string, any> = {};

  for (const [name, slot] of Object.entries(slots)) {
    if (!name.startsWith('form-')) {
      containerSlots[name] = slot;
      continue;
    }
    const formSlotName = name.slice('form-'.length);
    if (!formSlotName) {
      continue;
    }
    formSlots[formSlotName] = slot;
    if (!FORM_ACTION_SLOT_NAMES.has(formSlotName)) {
      const camelCaseName = toCamelCaseSlotName(formSlotName);
      if (camelCaseName !== formSlotName) {
        formSlots[camelCaseName] = slot;
      }
    }
  }

  return { containerSlots, formSlots };
}
