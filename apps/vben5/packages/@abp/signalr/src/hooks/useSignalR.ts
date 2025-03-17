/* eslint-disable @typescript-eslint/no-non-null-assertion */
import type { HubConnection, IHttpConnectionOptions } from '@microsoft/signalr';

import { useAccessStore } from '@vben/stores';

import { useEventBus } from '@abp/core';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

interface SignalROptions {
  /** 断线自动重连 */
  automaticReconnect?: boolean;
  /** 初始化自动建立连接 */
  autoStart?: boolean;
  /** 下次重试间隔（ms） */
  nextRetryDelayInMilliseconds?: number;
  /** 服务端url */
  serverUrl: string;
  /** 是否携带访问令牌 */
  useAccessToken?: boolean;
}

export function useSignalR() {
  const { publish, subscribe } = useEventBus();

  let connection: HubConnection | null = null;

  /** 初始化SignalR */
  async function init({
    automaticReconnect = true,
    autoStart = false,
    nextRetryDelayInMilliseconds = 60_000,
    serverUrl,
    useAccessToken = true,
  }: SignalROptions) {
    const httpOptions: IHttpConnectionOptions = {};
    if (useAccessToken) {
      httpOptions.accessTokenFactory = () => {
        const accessStore = useAccessStore();
        const token = accessStore.accessToken;
        if (!token) {
          return '';
        }
        return token.startsWith('Bearer ') ? token.slice(7) : token;
      };
    }
    const connectionBuilder = new HubConnectionBuilder()
      .withUrl(serverUrl, httpOptions)
      .configureLogging(LogLevel.Warning);
    if (automaticReconnect && nextRetryDelayInMilliseconds) {
      connectionBuilder.withAutomaticReconnect({
        nextRetryDelayInMilliseconds: () => nextRetryDelayInMilliseconds,
      });
    }
    connection = connectionBuilder.build();
    if (autoStart) {
      await start();
    }
  }

  /** 启动连接 */
  async function start(): Promise<void> {
    _throwIfNotInit();
    publish('signalR:beforeStart');
    try {
      await connection!.start();
      publish('signalR:onStart');
    } catch (error) {
      publish('signalR:onError', error);
    }
  }

  /** 关闭连接 */
  async function stop(): Promise<void> {
    _throwIfNotInit();
    publish('signalR:beforeStop');
    try {
      await connection!.stop();
      publish('signalR:onStop');
    } catch (error) {
      publish('signalR:onError', error);
    }
  }

  /** 连接前事件 */
  function beforeStart<T = any>(callback: (event?: T) => void) {
    subscribe('signalR:beforeStart', callback);
  }

  /** 连接后事件 */
  function onStart<T = any>(callback: (event?: T) => void) {
    subscribe('signalR:onStart', callback);
  }

  /** 关闭连接前事件 */
  function beforeStop<T = any>(callback: (event?: T) => void) {
    subscribe('signalR:beforeStop', callback);
  }

  /** 关闭连接后事件 */
  function onStop<T = any>(callback: (event?: T) => void) {
    subscribe('signalR:onStop', callback);
  }

  /** 连接错误事件 */
  function onError(callback: (error?: Error) => void) {
    subscribe('signalR:onError', callback);
  }

  /** 订阅服务端消息 */
  function on(methodName: string, newMethod: (...args: any[]) => void): void {
    connection?.on(methodName, newMethod);
  }

  /** 注销服务端消息 */
  function off(methodName: string, method: (...args: any[]) => void): void {
    connection?.off(methodName, method);
  }

  /** 连接关闭事件 */
  function onClose(callback: (error?: Error) => void): void {
    connection?.onclose(callback);
  }

  /** 发送消息 */
  function send(methodName: string, ...args: any[]): Promise<void> {
    _throwIfNotInit();
    return connection!.send(methodName, ...args);
  }

  /** 调用函数 */
  function invoke<T = any>(methodName: string, ...args: any[]): Promise<T> {
    _throwIfNotInit();
    return connection!.invoke(methodName, ...args);
  }

  function _throwIfNotInit() {
    if (connection === null) {
      throw new Error('unable to send message, connection not initialized!');
    }
  }

  return {
    beforeStart,
    beforeStop,
    init,
    invoke,
    off,
    on,
    onClose,
    onError,
    onStart,
    onStop,
    send,
    start,
    stop,
  };
}
