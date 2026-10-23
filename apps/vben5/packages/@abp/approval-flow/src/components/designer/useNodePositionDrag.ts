import { ref } from 'vue';

export interface Positionable {
  position?: { x: number; y: number };
}

export function useNodePositionDrag() {
  const draggingId = ref<null | string>(null);
  const suppressClick = ref(false);

  let active = false;
  let item: Positionable | null = null;
  let startX = 0;
  let startY = 0;
  let baseX = 0;
  let baseY = 0;
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
    item = null;
    document.body.style.cursor = '';
    document.body.style.userSelect = '';
  }

  function start(event: PointerEvent, target: Positionable) {
    if (event.button !== 0) {
      return;
    }
    const element = event.target as HTMLElement | null;
    if (
      element?.closest(
        'button, input, textarea, .ant-select, .ant-input, .node-delete, .add-btn',
      )
    ) {
      return;
    }

    cleanup();

    item = target;
    startX = event.clientX;
    startY = event.clientY;
    baseX = target.position?.x ?? 0;
    baseY = target.position?.y ?? 0;
    suppressClick.value = true;

    timer = setTimeout(() => {
      active = true;
      draggingId.value = (target as Positionable & { id?: string }).id ?? null;
      document.body.style.cursor = 'grabbing';
      document.body.style.userSelect = 'none';
    }, 300);

    moveHandler = (moveEvent: PointerEvent) => {
      if (!active || !item) {
        if (Math.hypot(moveEvent.clientX - startX, moveEvent.clientY - startY) > 8) {
          cleanup();
        }
        return;
      }

      moveEvent.preventDefault();
      item.position = {
        x: baseX + moveEvent.clientX - startX,
        y: baseY + moveEvent.clientY - startY,
      };
      window.dispatchEvent(new CustomEvent('approval-flow-layout-change'));
    };

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
    start,
    suppressClick,
  };
}
