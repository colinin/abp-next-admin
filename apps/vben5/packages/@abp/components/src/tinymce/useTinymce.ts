let unique = 0;
export function useTinymce() {
  function buildShortUUID(prefix = ''): string {
    const time = Date.now();
    const random = Math.floor(Math.random() * 1_000_000_000);
    unique++;
    return `${prefix}_${random}${unique}${String(time)}`;
  }

  return {
    buildShortUUID,
  };
}
