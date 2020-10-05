export function getItem(key: string) {
  const item = localStorage.getItem(key)
  if (item) {
    return item
  }
  return ''
}

export function getItemJson(key: string) {
  const item = localStorage.getItem(key)
  if (item) {
    return JSON.parse(item)
  }
  return null
}

export function getOrDefault<T>(key: string, defaultObject: T) {
  const item = localStorage.getItem(key)
  if (item) {
    return JSON.parse(item) as T
  }
  return defaultObject
}

export function setItem(key: string, value: string) {
  localStorage.setItem(key, value)
}

export function removeItem(key: string) {
  localStorage.removeItem(key)
}

export function clear() {
  localStorage.clear()
}
