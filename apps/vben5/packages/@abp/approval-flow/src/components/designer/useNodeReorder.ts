import type { ApprovalFlowNode } from '../../types/approval-flow';

import { ref } from 'vue';

export function useNodeReorder() {
  const draggingId = ref<null | string>(null);
  const suppressClick = ref(false);

  let active = false;
  let list: ApprovalFlowNode[] | null = null;
  let minIndex = 0;
  let maxIndex = Number.MAX_SAFE_INTEGER;
  let startX = 0;
  let startY = 0;
  let timer: ReturnType<typeof setTimeout> | null = null;
  let moveHandler: ((event: PointerEvent) => void) | null = null;
  let upHandler: (() => void) | null = null;

  function cleanup() {
    if (timer) {
      clearTimeout(timer);
      timer = null;
    }
    if (moveHandler) {
      window.removeEventListener('pointermove', moveHandler);
      moveHandler = null;
    }
    if (upHandler) {
      window.removeEventListener('pointerup', upHandler);
      upHandler = null;
    }
    active = false;
    draggingId.value = null;
    list = null;
    document.body.style.cursor = '';
    document.body.style.userSelect = '';
  }

  function move(event: PointerEvent) {
    if (!active || !list) {
      if (Math.hypot(event.clientX - startX, event.clientY - startY) > 8) {
        cleanup();
      }
      return;
    }

    event.preventDefault();

    const element = (document.elementFromPoint(event.clientX, event.clientY) as HTMLElement | null)?.closest('[data-node-id]') as
      | HTMLElement
      | null;
    const targetId = element?.dataset.nodeId;
    if (!element || !targetId || targetId === draggingId.value) {
      return;
    }

    const from = list.findIndex((item) => item.id === draggingId.value);
    const to = list.findIndex((item) => item.id === targetId);
    if (from < 0 || to < 0 || from === to) {
      return;
    }

    const rect = element.getBoundingClientRect();
    let insert = event.clientY > rect.top + rect.height / 2 ? to + 1 : to;
    if (from < insert) {
      insert -= 1;
    }
    insert = Math.max(minIndex, Math.min(maxIndex, insert));
    if (insert === from) {
      return;
    }

    const [moved] = list.splice(from, 1);
    if (moved) {
      list.splice(insert, 0, moved);
    }
  }

  function startReorder(
    event: PointerEvent,
    node: ApprovalFlowNode,
    targetList: ApprovalFlowNode[],
    options?: { maxIndex?: number; minIndex?: number },
  ) {
    if (event.button !== 0) {
      return;
    }

    const target = event.target as HTMLElement | null;
    if (
      target?.closest(
        'button, input, textarea, .ant-select, .ant-input, .node-delete, .add-btn',
      )
    ) {
      return;
    }

    cleanup();

    list = targetList;
    minIndex = options?.minIndex ?? 0;
    maxIndex = options?.maxIndex ?? targetList.length;
    startX = event.clientX;
    startY = event.clientY;
    suppressClick.value = true;

    timer = setTimeout(() => {
      active = true;
      draggingId.value = node.id;
      document.body.style.cursor = 'grabbing';
      document.body.style.userSelect = 'none';
    }, 350);

    moveHandler = (moveEvent: PointerEvent) => move(moveEvent);
    upHandler = () => {
      const didDrag = active;
      cleanup();
      if (didDrag) {
        setTimeout(() => {
          suppressClick.value = false;
        }, 150);
      } else {
        suppressClick.value = false;
      }
    };

    window.addEventListener('pointermove', moveHandler);
    window.addEventListener('pointerup', upHandler, { once: true });
  }

  function isDragging(id: string): boolean {
    return draggingId.value === id;
  }

  return {
    draggingId,
    isDragging,
    startReorder,
    suppressClick,
  };
}
