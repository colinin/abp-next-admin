/*
  天气接口
  数据来源: http://www.nmc.cn (中央气象台)
*/
import { defHttp } from '/@/utils/http/axios';
import { Position, Province, WeatherResult } from './model';
import { format } from '/@/utils/strings';

//const Host = 'http://www.nmc.cn';
const Api = {
  GetProvinces: '/wapi/rest/province/all',
  GetPosition: '/wapi/rest/position',
  GetCitys: '/wapi/rest/province/{province}',
  GetWeather: '/wapi/rest/weather?stationid={code}',
};

export const getProvinces = () => {
  return defHttp.get<Province[]>({
    url: Api.GetProvinces,
    //baseURL: Host,
    headers: {
      'X-Requested-With': 'XMLHttpRequest'
    }
  }, {
    apiUrl: '',
    joinTime: false,
    withToken: false,
    withAcceptLanguage: false,
  });
};

export const getPosition = () => {
  return defHttp.get<Position>({
    url: Api.GetPosition,
    //baseURL: Host,
    headers: {
      'X-Requested-With': 'XMLHttpRequest',
    }
  }, {
    apiUrl: '',
    joinTime: false,
    withToken: false,
    withAcceptLanguage: false,
  });
}

export const getCitys = (provinceCode: string) => {
  return defHttp.get<Position[]>({
    url: format(Api.GetCitys, {province: provinceCode}),
    //baseURL: Host,
    headers: {
      'X-Requested-With': 'XMLHttpRequest'
    }
  }, {
    apiUrl: '',
    joinTime: false,
    withToken: false,
    withAcceptLanguage: false,
  });
}

export const getWeather = (cityCode: string) => {
  return defHttp.get<WeatherResult>({
    url: format(Api.GetWeather, {code: cityCode}),
    //baseURL: Host,
    headers: {
      'X-Requested-With': 'XMLHttpRequest'
    }
  }, {
    apiUrl: '',
    joinTime: false,
    withToken: false,
    withAcceptLanguage: false,
  });
}
