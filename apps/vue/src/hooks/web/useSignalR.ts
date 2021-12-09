import { onMounted, onUnmounted } from 'vue';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  IHttpConnectionOptions,
} from '@microsoft/signalr';
import { useUserStoreWithOut } from '/@/store/modules/user';

import mitt from '/@/utils/mitt';

interface UseSignalR {
  serverUrl: string;
  autoStart?: boolean;
  useAccessToken?: boolean;
  automaticReconnect?: boolean;
  nextRetryDelayInMilliseconds?: number;
}

export function useSignalR({
  serverUrl,
  autoStart = false,
  useAccessToken = true,
  automaticReconnect = true,
  nextRetryDelayInMilliseconds = 60000,
}: UseSignalR) {
  const emitter = mitt();
  let connection: HubConnection | null = null;

  onMounted(() => {
    _initlizaConnection();
  });

  onUnmounted(() => {
    if (connection !== null && connection.state === HubConnectionState.Connected) {
      stop();
    }
  });

  function _initlizaConnection() {
    const httpOptions: IHttpConnectionOptions = {};
    if (useAccessToken) {
      const userStore = useUserStoreWithOut();
      const token = userStore.getToken;
      httpOptions.accessTokenFactory = () =>
        token.startsWith('Bearer ') ? token.substring(7) : token;
    }
    var connectionBuilder = new HubConnectionBuilder().withUrl(serverUrl, httpOptions);
    if (automaticReconnect) {
      connectionBuilder.withAutomaticReconnect({
        nextRetryDelayInMilliseconds: () => nextRetryDelayInMilliseconds,
      });
    }
    connection = connectionBuilder.build();
    if (autoStart) {
      start();
    }
  }

  async function start(): Promise<void> {
    if (connection == null) {
      return Promise.reject('unable to start, connection not initialized!');
    }
    emitter.emit('signalR:beforeStart');
    await connection.start();
    emitter.emit('signalR:onStart');
  }

  async function stop(): Promise<void> {
    if (connection == null) {
      return Promise.reject('unable to stop, connection not initialized!');
    }
    emitter.emit('signalR:beforeStop');
    await connection.stop();
    emitter.emit('signalR:onStop');
  }

  function beforeStart<T = any>(callback: (event?: T) => void) {
    emitter.on('signalR:beforeStart', callback);
  }

  function onStart<T = any>(callback: (event?: T) => void) {
    emitter.on('signalR:onStart', callback);
  }

  function beforeStop<T = any>(callback: (event?: T) => void) {
    emitter.on('signalR:beforeStop', callback);
  }

  function onStop<T = any>(callback: (event?: T) => void) {
    emitter.on('signalR:onStop', callback);
  }

  function on(methodName: string, newMethod: (...args: any[]) => void): void {
    connection?.on(methodName, newMethod);
  }

  function off(methodName: string, method: (...args: any[]) => void): void {
    connection?.off(methodName, method);
  }

  function onclose(callback: (error?: Error) => void): void {
    connection?.onclose(callback);
  }

  function send(methodName: string, ...args: any[]): Promise<void> {
    if (connection == null) {
      return Promise.reject('unable to send message, connection not initialized!');
    }
    return connection.send(methodName, ...args);
  }

  function invoke<T = any>(methodName: string, ...args: any[]): Promise<T> {
    if (connection == null) {
      return Promise.reject('unable to send message, connection not initialized!');
    }
    return connection.invoke(methodName, ...args);
  }

  return {
    on,
    off,
    onclose,
    beforeStart,
    onStart,
    beforeStop,
    onStop,
    send,
    invoke,
    start,
    stop,
  };
}
