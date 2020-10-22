import { PluginFunction } from 'vue'

export declare class VueEvents {
  static install: PluginFunction<never>

  emit(event: string, ...args: any[]): void

  fire(event: string, ...args: any[]): void

  on(event: string, callback: (eventData: any) => void): void

  listen(event: string, callback: (eventData: any) => void): void

  once(event: string, callback: (eventData: any) => void): void

  off(event: string, callback?: (eventData: any) => void): void

  unlisten (event: string, callback?: (eventData: any) => void): void

  removeAll(): void
}
