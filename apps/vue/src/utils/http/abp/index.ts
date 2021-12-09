import { abpRequest } from './abp';

function createAbpAxios() {
  return new abpRequest();
}

export const defAbpHttp = createAbpAxios();
